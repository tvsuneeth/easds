using System;
using Microsoft.AspNet.Identity.EntityFramework;
using twg.chk.DataService.DbContext.Intrastructure;


namespace twg.chk.DataService.DbContext.Repository
{
    public class UserRepository: RepositoryBase<IdentityUser>
    {
        public UserRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IUserRepository: IRepository<IdentityUser>
    {

    }
}
