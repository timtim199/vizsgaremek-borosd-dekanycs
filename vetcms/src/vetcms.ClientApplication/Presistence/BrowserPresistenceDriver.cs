using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;

namespace vetcms.ClientApplication.Presistence
{
    public class BrowserPresistenceDriver : IClientPresistenceDriver
    {
        private readonly ILocalStorageService localStorageService;
        public BrowserPresistenceDriver(ILocalStorageService _localStorageService)
        {
            localStorageService = _localStorageService;   
        }

        public async Task SaveItem<T>(string key, T item)
        {
            await localStorageService.SetItemAsync(key, item);
        }

        public async Task<T> GetItem<T>(string key)
        {
            T? result = await localStorageService.GetItemAsync<T>(key);
            return result == null ? throw new KeyNotFoundException($"Item not found: {key}") : result;
        }
        public async Task<IEnumerable<string>> GetKeysAsync<T>()
        {
            IEnumerable<string> keys = await localStorageService.KeysAsync();
            return keys;
        }

        public async Task ClearItems()
        {
            await localStorageService.ClearAsync();
        }
    }
}
