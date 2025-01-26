using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Api;
using vetcms.ServerApplication.Features.Misc.AutomatedEmailPreview.EmailContent;

namespace vetcms.ServerApplication.Features.Misc.AutomatedEmailPreview
{
    public partial class MiscController : ApiV1ControllerBase
    {
        [HttpGet("email-preview/{id}")]
        public async Task<IActionResult> PreviewEmail(int id)
        {
            PreviewableEmailFrameQuery query = new PreviewableEmailFrameQuery();
            query.EmailId = id;

            string htmlContent = await Mediator.Send(query);

            return Content(htmlContent, "text/html");
        }

        [HttpGet("email-content/{id}")]
        public async Task<IActionResult> GetEmailContent(int id)
        {
            PreviewableEmailContentQuery query = new PreviewableEmailContentQuery();
            query.EmailId = id;

            string htmlContent = await Mediator.Send(query);

            return Content(htmlContent, "text/html");
        }
    }
}
