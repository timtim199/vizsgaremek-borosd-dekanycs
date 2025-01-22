using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;

namespace vetcms.ServerApplication.Domain.Entity
{
    public class AnimalType : AuditedEntity
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
    }
}
