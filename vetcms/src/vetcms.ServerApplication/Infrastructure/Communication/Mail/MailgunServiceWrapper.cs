using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions;

namespace vetcms.ServerApplication.Infrastructure.Communication.Mail
{
    internal class MailgunServiceWrapper : IMailDeliveryProviderWrapper
    {
        private readonly string _domain;
        private readonly string _apiKey;
        private readonly string _senderEmail;

        public MailgunServiceWrapper(string domain, string apiKey, string senderEmail)
        {
            _domain = domain;
            _apiKey = apiKey;
            _senderEmail = senderEmail;
        }

        private RestClient PrepareClient()
        {
            var options = new RestClientOptions("https://api.mailgun.net/v3")
            {
                Authenticator = new HttpBasicAuthenticator("api", _apiKey)
            };
            var client = new RestClient(options);
            return client;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var client = PrepareClient();
            var request = new RestRequest();
            request.AddParameter("domain", _domain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";

            request.AddParameter("from", _senderEmail);
            request.AddParameter("to", toEmail);
            request.AddParameter("subject", subject);
            request.AddParameter("html", body);
            request.Method = Method.Post;
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Failed to send email: {response.Content}");
            }
        }
    }
}
