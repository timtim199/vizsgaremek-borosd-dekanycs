using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vetcms.ServerApplication.Domain.Entity.MedicalPillManagement
{
    internal class MedicalPillStock
    {
        [Key]
        public int Id { get; set; }

        public MedicalPill Pill { get; set; }

        public int Quantity { get; set; }

        public DateTime ExpiryDate { get; set; }

        [MaxLength(255)]
        public string StorageLocation { get; set; }

    }
}
