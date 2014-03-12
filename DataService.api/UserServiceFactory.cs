using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using twg.chk.DataService.DbContext;

namespace twg.chk.DataService.api
{
    public interface IUserServiceFactory
    {
        Func<UserManager<IdentityUser>> Factory { get; }
    }

    public class UserServiceFactory : IUserServiceFactory
    {
        public Func<UserManager<IdentityUser>> Factory { get; private set; }

        public UserServiceFactory()
        {
            Factory = () => new UserManager<IdentityUser>(new UserStore<IdentityUser>(new UserContext()));
        }
    }
}
