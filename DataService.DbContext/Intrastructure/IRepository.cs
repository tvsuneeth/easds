﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PagedList;

namespace twg.chk.DataService.DbContext.Intrastructure
{
    public interface IRepository<T> : IRepositoryPaged<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
    }
}
