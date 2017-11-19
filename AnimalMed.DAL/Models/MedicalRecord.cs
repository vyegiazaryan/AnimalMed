using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalMed.DAL.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public string MedicalRecordUrl { get; set; }
        public int AnimalId { get; set; }

        public virtual Animal Animal { get; set; }
    }
}
