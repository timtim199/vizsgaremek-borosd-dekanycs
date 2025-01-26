using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;

namespace vetcms.ServerApplication.Domain.Entity
{
    public class FirstTimeAuthenticationCode : AuditedEntity
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public string Code { get; set; }
    }
}
