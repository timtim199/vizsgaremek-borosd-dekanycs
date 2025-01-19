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

        /// <summary>
        /// Elmenti az elemet a megadott kulccsal.
        /// </summary>
        /// <typeparam name="T">Az elem típusa.</typeparam>
        /// <param name="key">A kulcs, amellyel az elemet elmentjük.</param>
        /// <param name="item">Az elmentendő elem.</param>
        public async Task SaveItem<T>(string key, T item)
        {
            await localStorageService.SetItemAsync(key, item);
        }

        /// <summary>
        /// Visszaadja az elemet a megadott kulccsal.
        /// </summary>
        /// <typeparam name="T">Az elem típusa.</typeparam>
        /// <param name="key">A kulcs, amellyel az elemet lekérjük.</param>
        /// <returns>Az elem.</returns>
        /// <exception cref="KeyNotFoundException">Ha az elem nem található.</exception>
        public async Task<T> GetItem<T>(string key)
        {
            T? result = await localStorageService.GetItemAsync<T>(key);
            return result == null ? throw new KeyNotFoundException($"Item not found: {key}") : result;
        }

        /// <summary>
        /// Visszaadja az összes kulcsot.
        /// </summary>
        /// <typeparam name="T">Az elem típusa.</typeparam>
        /// <returns>Az összes kulcs.</returns>
        public async Task<IEnumerable<string>> GetKeysAsync<T>()
        {
            IEnumerable<string> keys = await localStorageService.KeysAsync();
            return keys;
        }

        /// <summary>
        /// Törli az összes elemet.
        /// </summary>
        public async Task ClearItems()
        {
            await localStorageService.ClearAsync();
        }
    }
}
