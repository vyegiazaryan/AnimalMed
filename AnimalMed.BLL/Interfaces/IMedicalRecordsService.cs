using AnimalMed.DTO;
using AnimalMed.DTO.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalMed.BLL.Interfaces
{
    public interface IMedicalRecordsService : IService
    {
        Task<ServiceResult<EditMedicalRecordModel>> AddAsync(EditMedicalRecordModel model);
        Task<ServiceResult<EditMedicalRecordModel>> EditAsync(EditMedicalRecordModel model);
        Task<ServiceResult<MedicalRecordModel>> RemoveAsync(int id);

        Task<MedicalRecordModel> FirstAsync(int id);
        Task<IList<MedicalRecordModel>> GetAsync(int? animalId);
    }
}
