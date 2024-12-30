using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Common.Abstractions.Data
{
    public abstract class AuditedEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public int LastModifiedByUserId { get; set; }
        public bool Deleted { get; set; }
    }
}
