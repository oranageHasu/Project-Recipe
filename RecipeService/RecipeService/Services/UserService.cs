using CoreService.Classes.Error_Logging_System;
using CoreService.Classes.JWT_System;
using Microsoft.Extensions.Options;
using RecipeService.Classes;
using RecipeService.Models;
using System;
using System.Linq;

namespace RecipeService.Services
{
    public class UserService
    {
        #region Private Variables

        private readonly AppSettings _appSettings;
        private readonly ErrorLogger _logger;
        private readonly DatabaseContext _context;

        #endregion
        #region Constructors

        public UserService(IOptions<AppSettings> appSettings, ErrorLogger logger, DatabaseContext context)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _context = context;
        }

        #endregion
        #region Public Methods

        public bool ResetPassword(User user)
        {
            bool retval = false;
            User tmpUser = null;

            try
            {
                // Get the user (to ensure they exist, and the provided the correct login info)
                tmpUser = _context.Users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

                // Only proceed to change the password if the User was validated above
                if (tmpUser != null)
                {
                    // Set the new Password
                    tmpUser.Password = user.NewPassword;

                    // Update the User
                    _context.Users.Update(tmpUser);
                    _context.SaveChanges();

                    // Indicate to the caller that we succeeded
                    retval = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return retval;
        }

        public User Authenticate(string userName, string password)
        {
            int tokenExpiryInDays = 1;
            User user = new User
            {
                UserId = Guid.NewGuid()
            };

            try
            {
                // Note: Passwords should really be encrypted at rest (doing this for simplicity)
                user = _context.Users
                    .FirstOrDefault(u => u.UserName == userName && u.Password == password);

                // return null if user not found
                if (user == null)
                    return null;

                // generate an JWT for this user (it will be stored in a domain specific cookie)
                user.JwtToken = JwtTokenizer.CreateJWT(_appSettings.SecretKey, user.UserId.ToString(), tokenExpiryInDays, _logger);

                // remove password before returning
                user.Password = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return user;
        }

        #endregion
    }
}
