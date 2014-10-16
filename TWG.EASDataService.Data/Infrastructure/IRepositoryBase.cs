using System;

namespace TWG.EASDataService.Data.Infrastructure
{
    public interface IRepositoryBase<T> where T : class
    {
        T Get(int id);
    }
}
