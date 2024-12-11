using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Common
{
    internal static class Utility
    {
        public static string ExtractBearerToken(HttpRequest httpRequest)
        {
            if(httpRequest.Headers.TryGetValue("Authorization", out var bearerToken))
            {
                string? token = bearerToken.First()?.Split(" ")[1];
                if (!String.IsNullOrEmpty(token))
                {
                    return token;
                }
            }
            throw new UnauthorizedAccessException("Bearer token not found in request header.");
        }
    }
}
