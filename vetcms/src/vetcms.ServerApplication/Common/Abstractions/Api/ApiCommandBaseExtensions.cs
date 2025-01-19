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
        /// <summary>
        /// Előkészíti az AuthenticatedApiCommandBase parancsot a HttpRequest alapján.
        /// </summary>
        /// <typeparam name="T">A parancs eredményének típusa.</typeparam>
        /// <param name="command">Az AuthenticatedApiCommandBase parancs.</param>
        /// <param name="request">A HttpRequest objektum.</param>
        /// <returns>Az előkészített AuthenticatedApiCommandBase parancs.</returns>
        public static AuthenticatedApiCommandBase<T> Prepare<T>(this AuthenticatedApiCommandBase<T> command, HttpRequest request)
            where T : ICommandResult
        {
            string token = Utility.ExtractBearerToken(request);
            command.BearerToken = token;

            return command;
        }

        /// <summary>
        /// Előkészíti az UnauthenticatedApiCommandBase parancsot a HttpRequest alapján.
        /// </summary>
        /// <typeparam name="T">A parancs eredményének típusa.</typeparam>
        /// <param name="command">Az UnauthenticatedApiCommandBase parancs.</param>
        /// <param name="request">A HttpRequest objektum.</param>
        /// <returns>Az előkészített UnauthenticatedApiCommandBase parancs.</returns>
        public static UnauthenticatedApiCommandBase<T> Prepare<T>(this UnauthenticatedApiCommandBase<T> command, HttpRequest request)
            where T : ICommandResult
        {
            return command;
        }
    }
}
