using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

using TWG.EASDataService.DbContext.Repository;
using TWG.EASDataService.DbContext.Intrastructure;

namespace TWG.EASDataService.Services
{
    public interface IUserService
    {
        Task<IdentityUser> FindAsync(string userName, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string authenticationType);
        IPagedList<IdentityUser> GetPaged(int page, int pageSize);
    }

    public class UserService : UserManager<IdentityUser>, IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public IPagedList<IdentityUser> GetPaged(int page, int pageSize)
        {
            return _userRepository.GetPage(new Page(page, pageSize), u => true, order => order.UserName);
        }
    }
}
