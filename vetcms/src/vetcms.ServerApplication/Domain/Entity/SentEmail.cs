using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;

namespace vetcms.ServerApplication.Domain.Entity
{
    public class SentEmail : AuditedEntity
    {
        [Key]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public DateTime Timestamp { get; set; }
        public string HtmlContent { get; set; }
    }
}
