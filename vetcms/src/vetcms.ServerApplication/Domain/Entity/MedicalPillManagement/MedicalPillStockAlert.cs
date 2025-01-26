using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Domain.Entity.MedicalPillManagement
{
    internal class MedicalPillStockAlert
    {
        [Key]
        public int Id { get; set; }

        public MedicalPill Pill { get; set; }

        [MaxLength(255)]
        public string Type { get; set; }

        public string Message { get; set; }

        public DateTime GeneratedOn { get; set; }
    }
}
