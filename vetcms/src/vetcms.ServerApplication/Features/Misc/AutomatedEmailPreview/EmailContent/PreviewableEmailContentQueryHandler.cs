using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Features.Misc.AutomatedEmailPreview.EmailContent
{
    internal class PreviewableEmailContentQueryHandler(IRepositoryBase<SentEmail> repository) : IRequestHandler<PreviewableEmailContentQuery, string>
    {
        public async Task<string> Handle(PreviewableEmailContentQuery request, CancellationToken cancellationToken)
        {
            if (await repository.ExistAsync(request.EmailId))
            {
                var email = await repository.GetByIdAsync(request.EmailId);
                return email.HtmlContent;
            }
            else
            {
                return "nincs adat";
            }
        }
    }
}
