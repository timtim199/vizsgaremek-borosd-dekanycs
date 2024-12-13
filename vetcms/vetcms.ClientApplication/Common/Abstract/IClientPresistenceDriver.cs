using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ClientApplication.Common.Abstract
{
    internal interface IClientPresistenceDriver
    {
        public Task SaveItem<T>(string key, T item);
        public Task<T> GetItem<T>(string key);
        public Task<List<T>> GetKeysAsync<T>();
        public Task ClearItems();
    }
}
