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
    internal class AuthenticationManger
    {
        private readonly IClientPresistenceDriver _presistenceDriver;


        const string accessTokenPresistenceKey = "access-token";
        const string permissionSetPresistenceKey = "permission-set";
        public AuthenticationManger(IClientPresistenceDriver presistenceDriver)
        {
            _presistenceDriver = presistenceDriver;
        }

        internal async Task SaveAccessToken(string accessToken)
        {
            await _presistenceDriver.SaveItem(accessTokenPresistenceKey, accessToken);
        }

        internal async Task SavePermissionSet(string set)
        {
            await _presistenceDriver.SaveItem(permissionSetPresistenceKey, set);
        }

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

        public async Task<bool> IsAuthenticated()
            => await HasPermissionSet() &&  await HasAccessToken();

        public async Task<bool> HasPermission(params PermissionFlags[] permissions)
            => (await GetPermissionSet()).HasPermissionFlag(permissions);
    }
}