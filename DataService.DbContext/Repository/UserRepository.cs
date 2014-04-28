using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

using twg.chk.DataService.Business;
using twg.chk.DataService.DbContext.Intrastructure;

namespace twg.chk.DataService.DbContext.Repository
{
    public interface IUserRepository : IUserStore<IdentityUser>, IRepositoryPaged<IdentityUser>
    {

    }

    public class UserRepository : UserStore<IdentityUser>, IUserRepository
    {
        private DataServiceEntities dataContext;
        public UserRepository(IDatabaseFactory databaseFactory) : base(databaseFactory.Get())
        {
            DatabaseFactory = databaseFactory;
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected DataServiceEntities DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }

        /// <summary>
        /// Return a paged list of entities
        /// </summary>
        /// <typeparam name="TOrder"></typeparam>
        /// <param name="page">Which page to retrieve</param>
        /// <param name="where">Where clause to apply</param>
        /// <param name="order">Order by to apply</param>
        /// <returns></returns>
        public virtual IPagedList<IdentityUser> GetPage<TOrder>(Page page, Expression<Func<IdentityUser, bool>> where, Expression<Func<IdentityUser, TOrder>> order)
        {

            var results = dataContext.Users.OrderBy(order).Where(where).GetPage(page).ToList();
            var total = dataContext.Users.Count(where);

            return new StaticPagedList<IdentityUser>(results, page.PageNumber, page.PageSize, total);
        }
    }
}
