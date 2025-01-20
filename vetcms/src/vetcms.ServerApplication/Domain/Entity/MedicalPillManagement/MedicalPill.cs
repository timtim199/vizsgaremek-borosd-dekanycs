using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Domain.Entity.MedicalPillManagement
{
    class MedicalPill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Dosage { get; set; }

        public string Form { get; set; }

        public string Manufacturer { get; set; }

        public string ActiveIngredient { get; set; }

        public ICollection<MedicalPillStock> Stocks { get; set; }
        public ICollection<MedicalPillUsageLog> UsageLogs { get; set; }
        public ICollection<MedicalPillStockAlert> Alerts { get; set; }
    }
}
