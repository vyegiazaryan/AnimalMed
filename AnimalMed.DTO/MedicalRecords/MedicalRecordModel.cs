using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalMed.DTO.MedicalRecords
{
    public class MedicalRecordModel
    {
        public int Id { get; set; }
        public string MedicalRecordUrl { get; set; }
        public int AnimalId { get; set; }
        public string AnimalName { get; set; }
    }
}
