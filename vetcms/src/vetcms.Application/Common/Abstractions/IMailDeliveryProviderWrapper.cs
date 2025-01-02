using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Common.Abstractions
{
    internal interface IMailDeliveryProviderWrapper
    {
        /// <summary>
        /// Sends an email to a single recipient.
        /// </summary>
        /// <param name="toEmail">Recipient's email address.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">HTML or plain-text content of the email.</param>
        public Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
