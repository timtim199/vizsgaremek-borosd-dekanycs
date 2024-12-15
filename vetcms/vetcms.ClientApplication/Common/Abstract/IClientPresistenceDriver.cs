using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ClientApplication.Common.Abstract
{
    internal interface IClientPresistenceDriver
    {
        internal Task SaveItem<T>(string key, T item);
        internal Task<T> GetItem<T>(string key);
        internal Task<IEnumerable<string>> GetKeysAsync<T>();
        internal Task ClearItems();
    }
}
