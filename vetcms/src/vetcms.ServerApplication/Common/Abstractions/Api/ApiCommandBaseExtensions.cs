using Azure.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.SharedModels.Common;
using vetcms.SharedModels.Common.Abstract;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace vetcms.ServerApplication.Common.Abstractions.Api
{
    internal static class ApiCommandBaseExtensions
    {
        public static AuthenticatedApiCommandBase<T> Prepare<T>(this AuthenticatedApiCommandBase<T> command, HttpRequest request)
            where T : ICommandResult
        {
            string token = Utility.ExtractBearerToken(request);
            command.BearerToken = token;

            return command;
        }

        public static UnauthenticatedApiCommandBase<T> Prepare<T>(this UnauthenticatedApiCommandBase<T> command, HttpRequest request)
            where T : ICommandResult
        {
            return command;
        }
    }
}
