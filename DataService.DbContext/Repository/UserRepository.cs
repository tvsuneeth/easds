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
    public interface IUserRepository : IRepository<IdentityUser>, IUserStore<IdentityUser>
    {

    }

    public class UserRepository : UserStore<IdentityUser>, IUserRepository
    {
        private DataServiceEntities dataContext;
        private readonly IDbSet<IdentityUser> dbset;
        public UserRepository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<IdentityUser>();
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
        public virtual void Add(IdentityUser entity)
        {
            dbset.Add(entity);
        }
        public virtual void Update(IdentityUser entity)
        {
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(IdentityUser entity)
        {
            dbset.Remove(entity);
        }
        public virtual void Delete(Expression<Func<IdentityUser, bool>> where)
        {
            IEnumerable<IdentityUser> objects = dbset.Where<IdentityUser>(where).AsEnumerable();
            foreach (IdentityUser obj in objects)
                dbset.Remove(obj);
        }
        public virtual IdentityUser GetById(long id)
        {
            return dbset.Find(id);
        }
        public virtual IdentityUser GetById(string id)
        {
            return dbset.Find(id);
        }
        public virtual IEnumerable<IdentityUser> GetAll()
        {
            return dbset.ToList();
        }

        public virtual IEnumerable<IdentityUser> GetMany(Expression<Func<IdentityUser, bool>> where)
        {
            return dbset.Where(where).ToList();
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
            var results = dbset.OrderBy(order).Where(where).GetPage(page).ToList();
            var total = dbset.Count(where);
            return new StaticPagedList<IdentityUser>(results, page.PageNumber, page.PageSize, total);
        }

        public IdentityUser Get(Expression<Func<IdentityUser, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault<IdentityUser>();
        }
    }
}
