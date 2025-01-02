using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Infrastructure.Communication.Mail
{
    internal class MailService(IMailDeliveryProviderWrapper mailServiceWrapper) : IMailService
    {
        public async Task SendPasswordResetEmailAsync(PasswordReset passwordReset)
        {
            var fields = new Dictionary<TemplateField, string>
            {
                { TemplateField.code, passwordReset.Code },
                { TemplateField.visible_name, passwordReset.User.VisibleName }
            };

            await SendEmailAsync(passwordReset.User.Email, "VETCMS: Elfelejtett jelszó", TemplateCatalog.PasswordReset, fields);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string templateName, Dictionary<TemplateField, string> fields)
        {
            var template = GetTemplate(templateName);
            var body = ReplaceFields(template, fields);

            await mailServiceWrapper.SendEmailAsync(toEmail, subject, body);
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
    }
}
