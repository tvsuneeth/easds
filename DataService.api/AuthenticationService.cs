using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace twg.chk.DataService.api
{
    public interface IAuthenticationService
    {
        Task<ClaimsIdentity> RequestAuthenticationToken(String userName, String password);
        Boolean CreateUser(String userName, String password, out IEnumerable<String> errors);
    }

    public class AuthenticationService : IAuthenticationService
    {
        public IUserServiceFactory UserFactory { get; private set;}
        public AuthenticationService(IUserServiceFactory userServiceFactory)
        {
            UserFactory = userServiceFactory;
        }

        public async Task<ClaimsIdentity> RequestAuthenticationToken(string userName, string password)
        {
            using (UserManager<IdentityUser> userManager = UserFactory.Factory())
            {
                var user = await userManager.FindAsync(userName, password);

                if (user == null)
                {
                    throw new AuthenticationException("The user name or password is incorrect.");
                }
                else
                {
                    return await userManager.CreateIdentityAsync(user, "Bearer");
                }
            }
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Boolean CreateUser(String userName, String password, out IEnumerable<String> errors)
        {
            var newUser = new IdentityUser { UserName = userName };
            using (UserManager<IdentityUser> userManager = UserFactory.Factory())
            {
                IdentityResult result =  userManager.Create(newUser, password);
                errors = result.Succeeded ? null : result.Errors;

                return result.Succeeded;
            }
        }
    }
}
