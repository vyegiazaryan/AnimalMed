using System.Collections.Generic;
using System.Threading.Tasks;
using AnimalMed.DTO.Animals;
using AnimalMed.DTO;
using AnimalMed.DTO.SearchCriterias;
using AnimalMed.DTO.MedicalRecords;

namespace AnimalMed.BLL.Interfaces
{
    public interface IAnimalService : IService
    {
        Task<ServiceResult<EditAnimalModel>> AddAsync(EditAnimalModel model);
        Task<ServiceResult<EditAnimalModel>> EditAsync(EditAnimalModel model);
        Task<ServiceResult<AnimalModel>> RemoveAsync(int id);

        Task<AnimalModel> FirstAsync(int id);
        Task<IList<AnimalModel>> GetAsync(AnimalSearchCriteria criteria);

    }
}
