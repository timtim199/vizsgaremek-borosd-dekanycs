using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Features.Misc.AutomatedEmailPreview
{
    internal class PreviewableEmailFrameQueryHandler(IRepositoryBase<SentEmail> repository) : IRequestHandler<PreviewableEmailFrameQuery, string>
    {
        public async Task<string> Handle(PreviewableEmailFrameQuery request, CancellationToken cancellationToken)
        {
            if(await repository.ExistAsync(request.EmailId))
            {
                return await GetEmailFrame(await repository.GetByIdAsync(request.EmailId));
            }
            else
            {
                return "nincs adat";
            }
        }

        private Task<string> GetEmailFrame(SentEmail sentEmail)
        {
            string template = GetTemplate();

            template = template.Replace("{{email_from}}", sentEmail.From);
            template = template.Replace("{{email_to}}", sentEmail.To);
            template = template.Replace("{{email_subject}}", sentEmail.Subject);
            template = template.Replace("{{email_timestamp}}", sentEmail.Timestamp.ToString());
            template = template.Replace("{{email_src}}", $"/api/v1/misc/email-content/{sentEmail.Id}");

            return Task.FromResult(template);
        }

        private string GetTemplate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "vetcms.ServerApplication.Features.Misc.AutomatedEmailPreview.Templates.EmailContainer.html";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
