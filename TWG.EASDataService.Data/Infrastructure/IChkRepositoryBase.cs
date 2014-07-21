using System;

namespace TWG.EASDataService.Data.Infrastructure
{
    public interface IChkRepositoryBase<T> where T : class
    {
        T Get(int id);
    }
}
