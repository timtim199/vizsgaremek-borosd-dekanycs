using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.Application.Domain.Abstractions.Data;

namespace vetcms.Application.Domain.Entity
{
    internal class User : AuditedEntity
    {
        [Key]
        public int Id { get; set; }
        public string VisibleName { get; set; }
    }
}
