using CoreService.Classes.Error_Logging_System;
using CoreService.Models.AWS_Cognito_System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeService.Models;
using RecipeService.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static RecipeService.Classes.Constants;

namespace RecipeService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoginController : ControllerBase
    {
        #region Private Variables

        private readonly UserService _service;
        private readonly ErrorLogger _logger;

        #endregion
        #region Constructors

        public LoginController(
            UserService service,
            ErrorLogger logger)
        {
            _service = service;
            _logger = logger;
        }

        #endregion
        #region Public Methods

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] User user)
        {
            User tmpUser;
            ActionResult retval = Ok(new User());

            try
            {
                tmpUser = _service.Authenticate(user.UserName, user.Password);

                if (tmpUser != null)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(Jwt_Token, tmpUser.JwtToken),
                    };

                    AuthenticationProperties authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = true,
                        IssuedUtc = DateTime.Now,
                        ExpiresUtc = DateTime.Now.AddDays(1)
                    };

                    ClaimsIdentity claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sets the Auth Cookie for the Session
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), authProperties);

                    // Clear the password and token from CognitoLogin
                    tmpUser.JwtToken = null;
                    tmpUser.Password = null;

                    // Set the return value
                    retval = Ok(tmpUser);
                }
            }
            catch (Exception ex)
            {
                retval = BadRequest(new CognitoLogin());
                _logger.LogError(ex);
            }

            return retval;
        }

        // We are using the post method to call invalidate the JWT token issued to this user
        // POST: api/login/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            ActionResult retval = BadRequest(new { message = "Failed logout operation." });

            try
            {
                // Perform a "Signout";  Invalidating any domain specific cookie
                await HttpContext.SignOutAsync();

                // Indicate the operation to sign out went successfully
                retval = Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return retval;
        }

        #endregion
    }
}
