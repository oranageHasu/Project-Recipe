using System;
using System.Text;
using RecipeService.Classes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Certificate;
using RecipeService.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CoreService.Classes.Error_Logging_System;
using static RecipeService.Classes.Constants;

namespace RecipeService
{
    public class Startup
    {
        #region Public Variables

        public IConfiguration Configuration { get; }

        #endregion
        #region Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion
        #region Public Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                // Configure AppSettings Dependency Injection
                ConfigureAppSettings(services);

                // Configure EF Core DB Connection
                ConfigureDatabaseConnection(services);

                // Configure SSL
                ConfigureSSL(services);

                // Configure Dependency Injection for Application Services
                ConfigureAppServices(services);

                // Configure MVC
                ConfigureMVC(services);

                // Configure CORs
                ConfigureCORs(services);

                // Configure Cookie Authentication
                ConfigureCookieAuthentication(services);

                // Configure Cookie Policies
                ConfigureCookiePolicies(services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "MVC1005:Cannot use UseMvc with Endpoint Routing.", Justification = "<Pending>")]
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseCookiePolicy();
            app.UseAuthorization();
            app.UseAuthentication();

            app.Use(async (context, next) =>
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken validToken = null;

                // Allows this service to re-read the HTTP Request's Body as many times as needed
                context.Request.EnableBuffering();

                // Get the App Settings
                IConfigurationSection appSettingsSection = Configuration.GetSection(AppSettings.TEXT_AppSettings);

                // Configure JWT authentication
                AppSettings appSettings = appSettingsSection.Get<AppSettings>();
                byte[] key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
                ClaimsPrincipal principal = context.User as ClaimsPrincipal;

                // Validation parameters used for JWT tokens
                TokenValidationParameters validationParams = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                var accessToken = principal?.Claims
                  .FirstOrDefault(c => c.Type == Jwt_Token);

                // Validate the JWT token that is stored in the Cookie (this is done ontop of the cookie authentication)
                if (accessToken != null)
                {
                    handler.ValidateToken(accessToken.Value, validationParams, out validToken);
                }

                await next();
            });

            app.UseMvc();   // Warning is still generated even though we do what Microsoft asks us to do; See Startup.ConfigureMVC()
        }

        #endregion
        #region Private Methods

        private void ConfigureAppSettings(IServiceCollection services)
        {
            IConfigurationSection appSettingsSection = Configuration.GetSection(AppSettings.TEXT_AppSettings);
            services.Configure<AppSettings>(appSettingsSection);
        }

        private void ConfigureDatabaseConnection(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Dev"));
            });
        }

        public void ConfigureSSL(IServiceCollection services)
        {
            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();
        }

        private void ConfigureAppServices(IServiceCollection services)
        {
            // Configure Dependency Injection for application services (i.e the Service Classes)
            services.AddScoped<UserService>();

            // Configure Dependency Injection for misc services (i.e. Logging, AWS, etc.)
            services.AddScoped<ErrorLogger>();
        }

        private void ConfigureMVC(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                // Here we use the policy created above to specify the type of Authorization Filter we want to use
                // This, in essence, locks down every public web method, except where [AllowAnonymous] is defined
                options.Filters.Add(new AuthorizeFilter());

                // Fixes the issue that arises from "app.UseMvc()" in Startup.Configure()
                options.EnableEndpointRouting = false;

            })
            .AddNewtonsoftJson(options =>
            {
                // Here we remove the reference loop handling so when we serialize "many-to-many" entities, we will not get a run-time error
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                // Special UTC datetime handling
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        private void ConfigureCORs(IServiceCollection services)
        {
            string[] origins = { 
                "https://localhost:4200",
                "https://localhost:4205",
                "https://192.168.1.34:4200",
                "https://192.168.1.29:4200",
                "https://192.168.1.163:4200"
            };

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowCredentials();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.WithOrigins(origins);

                });
            });
        }

        private void ConfigureCookieAuthentication(IServiceCollection services)
        {
            // configure cookie authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                // This always needs to be true, so that the front end is never able to read cookie data
                options.LoginPath = "/user-login";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.Name = "RecipeService";

                options.Events.OnRedirectToLogin = context =>
                {
                    // Return a 401 error if unauthorized and redirected to the Login
                    context.Response.StatusCode = 401;

                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    // Return a 401 error if unauthorized
                    context.Response.StatusCode = 401;

                    return Task.CompletedTask;
                };
            });
        }

        private void ConfigureCookiePolicies(IServiceCollection services)
        {
            // Policy that will be applied to a general filter of this web service
            // This policy ensures that, by default, all exposed methods require authentication 
            // EXCEPT when the [AllowAnonymous] tag is explicitly coded for a web method
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.Always;
            });
        }

        #endregion
    }
}
