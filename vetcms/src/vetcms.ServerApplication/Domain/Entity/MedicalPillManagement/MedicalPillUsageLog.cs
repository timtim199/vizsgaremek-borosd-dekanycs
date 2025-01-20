using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Domain.Entity.MedicalPillManagement
{
    internal class MedicalPillUsageLog
    {
        [Key]
        public int Id { get; set; }

        public MedicalPill Pill { get; set; }

        public int QuantityUsed { get; set; }

        public DateTime UsageDate { get; set; }

        public string Purpose { get; set; }
        public User UsedBy { get; set; }
    }
}
