using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Presistence;
using vetcms.SharedModels.Common.IAM.Authorization;

namespace vetcms.ClientApplication.Common.IAM
{
    public class AuthenticationManger
    {
        private readonly IClientPresistenceDriver _presistenceDriver;


        const string accessTokenPresistenceKey = "access-token";
        const string permissionSetPresistenceKey = "permission-set";
        public AuthenticationManger(IClientPresistenceDriver presistenceDriver)
        {
            _presistenceDriver = presistenceDriver;
        }

        /// <summary>
        /// Elmenti a hozzáférési tokent.
        /// </summary>
        /// <param name="accessToken">A hozzáférési token.</param>
        internal async Task SaveAccessToken(string accessToken)
        {
            await _presistenceDriver.SaveItem(accessTokenPresistenceKey, accessToken);
        }

        /// <summary>
        /// Elmenti a jogosultsági készletet.
        /// </summary>
        /// <param name="set">A jogosultsági készlet.</param>
        internal async Task SavePermissionSet(string set)
        {
            await _presistenceDriver.SaveItem(permissionSetPresistenceKey, set);
        }

        /// <summary>
        /// Visszaadja a hozzáférési tokent.
        /// </summary>
        /// <returns>A hozzáférési token.</returns>
        /// <exception cref="UnauthorizedAccessException">Ha a hozzáférési token nem található.</exception>
        internal async Task<string> GetAccessToken()
        {
            try
            {
                return await _presistenceDriver.GetItem<string>(accessTokenPresistenceKey);
            }
            catch (KeyNotFoundException)
            {
                throw new UnauthorizedAccessException("Access token not found");
            }
        }

        /// <summary>
        /// Visszaadja a jogosultsági készletet.
        /// </summary>
        /// <returns>Az EntityPermissions objektum.</returns>
        /// <exception cref="UnauthorizedAccessException">Ha a jogosultsági készlet nem található.</exception>
        internal async Task<EntityPermissions> GetPermissionSet()
        {
            try
            {
                return new EntityPermissions(await _presistenceDriver.GetItem<string>(permissionSetPresistenceKey));
            }
            catch (KeyNotFoundException)
            {
                throw new UnauthorizedAccessException("Permission set not found");
            }
        }

        /// <summary>
        /// Ellenőrzi, hogy van-e hozzáférési token.
        /// </summary>
        /// <returns>Igaz, ha van hozzáférési token, különben hamis.</returns>
        internal async Task<bool> HasAccessToken()
        {
            try
            {
                string token = await _presistenceDriver.GetItem<string>(accessTokenPresistenceKey);
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Ellenőrzi, hogy van-e jogosultsági készlet.
        /// </summary>
        /// <returns>Igaz, ha van jogosultsági készlet, különben hamis.</returns>
        internal async Task<bool> HasPermissionSet()
        {
            try
            {
                string token = await _presistenceDriver.GetItem<string>(permissionSetPresistenceKey);
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Ellenőrzi, hogy a felhasználó hitelesítve van-e.
        /// </summary>
        /// <returns>Igaz, ha a felhasználó hitelesítve van, különben hamis.</returns>
        public async Task<bool> IsAuthenticated()
            => await HasPermissionSet() &&  await HasAccessToken();

        /// <summary>
        /// Ellenőrzi, hogy a felhasználó rendelkezik-e a megadott jogosultságokkal.
        /// </summary>
        /// <param name="permissions">A jogosultságok tömbje.</param>
        /// <returns>Igaz, ha a felhasználó rendelkezik a megadott jogosultságokkal, különben hamis.</returns>
        public async Task<bool> HasPermission(params PermissionFlags[] permissions)
            => (await GetPermissionSet()).HasPermissionFlag(permissions);

        /// <summary>
        /// Törli a hitelesítési adatokat.
        /// </summary>
        public async Task ClearAuthenticationDetails()
        {
            await _presistenceDriver.ClearItems();
        }
    }
}
