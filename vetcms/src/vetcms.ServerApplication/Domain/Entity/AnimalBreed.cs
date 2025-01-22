using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;

namespace vetcms.ServerApplication.Domain.Entity
{
    public class AnimalBreed : AuditedEntity
    {
        [Key]
        public int Id { get; set; }
        public AnimalType Type { get; set; }
        public string BreedName { get; set; }
        public string Charachteristics { get; set; }
    }
}
