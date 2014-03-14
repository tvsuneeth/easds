using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Core
{
    public interface ICacheService
    {
        T Get<T>(String key) where T : class;
        void Add<T>(String key, T obj) where T : class;
        void Replace<T>(String key, T obj) where T : class;
        Boolean Exist<T>(String key) where T : class;
        void Remove(String key);
        void Clear();
    }

    public class CacheService: ICacheService
    {
        public T Get<T>(string key) where T : class
        {
            throw new NotImplementedException();
        }

        public void Add<T>(string key, T obj) where T : class
        {
            throw new NotImplementedException();
        }

        public void Replace<T>(string key, T obj) where T : class
        {
            throw new NotImplementedException();
        }

        public bool Exist<T>(string key) where T : class
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
