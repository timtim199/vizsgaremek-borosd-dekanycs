using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ServerApplication.Features.Misc.AutomatedEmailPreview
{
    internal class NewPreviewableEmailCommandHandler(IRepositoryBase<SentEmail> repository) : IRequestHandler<NewPreviewableEmailCommand, string>
    {
        public async Task<string> Handle(NewPreviewableEmailCommand request, CancellationToken cancellationToken)
        {
            SentEmail newEmail = new SentEmail();
            newEmail.From = request.From;
            newEmail.To = request.To;
            newEmail.Subject = request.Subject;
            newEmail.Timestamp = DateTime.Now;
            newEmail.HtmlContent = request.HtmlContent;

           await repository.AddAsync(newEmail);

            return "";
        }
    }
}
