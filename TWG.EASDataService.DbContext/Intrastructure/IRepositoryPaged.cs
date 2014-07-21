
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PagedList;

namespace TWG.EASDataService.DbContext.Intrastructure
{
    public interface IRepositoryPaged<T>
    {
        IPagedList<T> GetPage<TOrder>(Page page, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order);
    }
}
