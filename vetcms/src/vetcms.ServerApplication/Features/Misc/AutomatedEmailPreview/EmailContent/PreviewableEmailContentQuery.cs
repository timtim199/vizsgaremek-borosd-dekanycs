using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Features.Misc.AutomatedEmailPreview.EmailContent
{
    public class PreviewableEmailContentQuery : IRequest<string>
    {
        public int EmailId { get; set; }
    }
}
