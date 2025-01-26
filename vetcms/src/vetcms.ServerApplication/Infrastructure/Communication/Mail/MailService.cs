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
        public async Task SendPasswordResetEmailAsync(PasswordReset passwordReset)
        {
            var fields = new Dictionary<TemplateField, string>
            {
                { TemplateField.code, passwordReset.Code },
                { TemplateField.visible_name, passwordReset.User.VisibleName }
            };

            await SendEmailAsync(passwordReset.User.Email, "VETCMS: Elfelejtett jelszó", TemplateCatalog.PasswordReset, fields);
        }

        public async Task SendFirstAuthenticationEmailAsync(FirstTimeAuthenticationCode authModel)
        {
            var fields = new Dictionary<TemplateField, string>
            {
                { TemplateField.URL, $"https://localhost/iam/first-time-login/{authModel.Code}" },
                { TemplateField.visible_name, authModel.User.VisibleName }
            };
            await SendEmailAsync(authModel.User.Email, "VETCMS: Első belépés", TemplateCatalog.AdminCreateUser, fields);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string templateName, Dictionary<TemplateField, string> fields)
        {
            var template = GetTemplate(templateName);
            var body = ReplaceFields(template, fields);
            try
            {
                await mailServiceWrapper.SendEmailAsync(toEmail, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email!");
                Console.WriteLine(ex.ToString());
                string path = await SubmitEmailToPreview(toEmail, subject, body);
                throw new CommonApiLogicException(ApiLogicExceptionCode.SEND_EMAIL_FAILED, $"[BEMUTATÓ CÉLJÁBÓL] A kiküldött emailt megtekintheti a következő linken: {path}");
            }
        }

        private async Task<string> SubmitEmailToPreview(string to, string subject, string body)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                ISender mediator = scope.ServiceProvider.GetService<ISender>();
                NewPreviewableEmailCommand command = new NewPreviewableEmailCommand()
                {
                    From = "no-reply@vetcms.hu",
                    To = to,
                    HtmlContent = body,
                    Subject = subject
                };
                return await mediator.Send(command);
            }
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
