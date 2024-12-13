using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;

namespace vetcms.ClientApplication.Presistence
{
    internal class BrowserPresistenceDriver(ILocalStorageService localStorageService) : IClientPresistenceDriver
    {
        public async Task SaveItem<T>(string key, T item)
        {
            await localStorageService.SetItemAsync(key, item);
        }

        public async Task<T> GetItem<T>(string key)
        {
            T? result = await localStorageService.GetItemAsync<T>(key);
            if (result == null)
            {
                throw new KeyNotFoundException($"Item not found: {key}");
            }
            return result;
        }
        public async Task<IEnumerable<string>> GetKeysAsync<T>()
        {

            IEnumerable<string> keys = await localStorageService.KeysAsync();
            return keys;

            /// TODO: Continue here: Implement presistence driver, and credential store
        }

        public Task ClearItems()
        {
            throw new NotImplementedException();
        }
    }
}
