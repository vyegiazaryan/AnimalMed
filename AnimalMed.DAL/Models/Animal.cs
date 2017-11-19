using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalMed.DAL.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordNo { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
