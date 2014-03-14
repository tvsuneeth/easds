using System;

namespace twg.chk.DataService.chkData.Infrastructure
{
    public interface IChkRepositoryBase<T> where T : class
    {
        T Get(int id);
    }
}
