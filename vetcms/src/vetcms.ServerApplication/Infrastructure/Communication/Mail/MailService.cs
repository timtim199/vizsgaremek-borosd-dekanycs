using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Features.Misc.AutomatedEmailPreview;
using vetcms.SharedModels.Common.ApiLogicExceptionHandling;

[assembly: InternalsVisibleTo("vetcms.ServerApplication.Tests")]
namespace vetcms.ServerApplication.Infrastructure.Communication.Mail
{
    internal class MailService(IMailDeliveryProviderWrapper mailServiceWrapper, IServiceScopeFactory serviceScopeFactory) : IMailService
    {
        public async Task<int> SendPasswordResetEmailAsync(PasswordReset passwordReset)
        {
            var fields = new Dictionary<TemplateField, string>
            {
                { TemplateField.code, passwordReset.Code },
                { TemplateField.visible_name, passwordReset.User.VisibleName }
            };

            return await SendEmailAsync(passwordReset.User.Email, "VETCMS: Elfelejtett jelszó", TemplateCatalog.PasswordReset, fields);
        }

        public async Task<int> SendFirstAuthenticationEmailAsync(FirstTimeAuthenticationCode authModel)
        {
            var fields = new Dictionary<TemplateField, string>
            {
                { TemplateField.URL, $"https://localhost/iam/first-time-login/{authModel.Code}" },
                { TemplateField.visible_name, authModel.User.VisibleName }
            };
            return await SendEmailAsync(authModel.User.Email, "VETCMS: Első belépés", TemplateCatalog.AdminCreateUser, fields);
        }

        private async Task<int> SendEmailAsync(string toEmail, string subject, string templateName, Dictionary<TemplateField, string> fields)
        {
            var template = GetTemplate(templateName);
            var body = ReplaceFields(template, fields);
            return await mailServiceWrapper.SendEmailAsync(toEmail, subject, body);
        }

        private string GetTemplate(string template)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = template;

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        private string ReplaceFields(string template, Dictionary<TemplateField, string> fields)
        {
            fields.Add(TemplateField.year, DateTime.Now.Year.ToString());

            foreach (var field in fields)
            {
                template = template.Replace("{{"+ Enum.GetName<TemplateField>(field.Key) + "}}", field.Value);
            }

            return template;
        }

        public string GetEmailPreviewRoute(int emailId)
        {
            return $"/api/v1/misc/email-preview/{emailId}";
        }
    }
}
