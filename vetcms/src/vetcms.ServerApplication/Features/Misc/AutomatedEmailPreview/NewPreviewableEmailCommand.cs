using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Features.Misc.AutomatedEmailPreview
{
    public class NewPreviewableEmailCommand : IRequest<string>
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
    }
}
