using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Common.Abstract;
using vetcms.ClientApplication.Presistence;

namespace vetcms.ClientApplication.Common.Authentication
{
    internal class CredentialStore
    {
        private readonly IClientPresistenceDriver _presistenceDriver;


        const string accessTokenPresistenceKey = "access-token";
        public CredentialStore(IClientPresistenceDriver presistenceDriver)
        {
            _presistenceDriver = presistenceDriver;
        }

        internal async Task SaveAccessToken(string accessToken)
        {
            await _presistenceDriver.SaveItem(accessTokenPresistenceKey, accessToken);
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
    }
}
