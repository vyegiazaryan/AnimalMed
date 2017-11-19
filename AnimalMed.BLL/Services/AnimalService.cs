using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AnimalMed.BLL.Interfaces;
using AnimalMed.DTO.Animals;
using AnimalMed.DAL.Models;
using AnimalMed.DAL.EF;
using AnimalMed.DTO;
using AnimalMed.DTO.SearchCriterias;
using AnimalMed.DTO.MedicalRecords;

namespace AnimalMed.BLL.Services
{
    public class AnimalService : ServiceBase, IAnimalService
    {
        private readonly IMedicalRecordsService _medicalRecordsService;

        public AnimalService(AnimalMedDbContext context, IMedicalRecordsService medicalRecordsService) : base(context)
        {
            _medicalRecordsService = medicalRecordsService;
        }

        public async Task<ServiceResult<EditAnimalModel>> AddAsync(EditAnimalModel model)
        {
            var entity = new Animal
            {
                Name = model.Name,
                PasswordNo = model.PasswordNo,
                ImageUrl = model.ImageUrl
            };

            _db.Add(entity);

            try
            {
                await _db.SaveChangesAsync();

                return ServiceResult<EditAnimalModel>.Success(new EditAnimalModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    PasswordNo = entity.PasswordNo,
                    ImageUrl = entity.ImageUrl
                });
            }
            catch (Exception ex)
            {
                //log error
                return ServiceResult<EditAnimalModel>.Fail("Internal server error");
            }
        }

        public async Task<ServiceResult<EditAnimalModel>> EditAsync(EditAnimalModel model)
        {
            var entity = await _db.Animals.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (entity == null)
            {
                return ServiceResult<EditAnimalModel>.Fail("Animal not found");
            }
            entity.Name = model.Name;
            entity.PasswordNo = model.PasswordNo;
            entity.ImageUrl = model.ImageUrl;

            try
            {
                await _db.SaveChangesAsync();

                return ServiceResult<EditAnimalModel>.Success(new EditAnimalModel
                {
                    Id = entity.Id,
                    PasswordNo = entity.PasswordNo,
                    ImageUrl = entity.ImageUrl
                });
            }
            catch (Exception ex)
            {
                //log error
                return ServiceResult<EditAnimalModel>.Fail("Internal server error");
            }
        }

        public async Task<ServiceResult<AnimalModel>> RemoveAsync(int id)
        {
            var entity = await _db.Animals.Include(x => x.MedicalRecords).FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return ServiceResult<AnimalModel>.Fail("Animal not found");
            }


            try
            {
                _db.MedicalRecords.RemoveRange(entity.MedicalRecords);
                _db.Animals.Remove(entity);
                await _db.SaveChangesAsync();

                return ServiceResult<AnimalModel>.Success(new AnimalModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    PasswordNo = entity.PasswordNo,
                    ImageUrl = entity.ImageUrl
                });
            }
            catch (Exception ex)
            {
                //todo: log error
                return ServiceResult<AnimalModel>.Fail("Internal server error");
            }
        }

        public async Task<AnimalModel> FirstAsync(int id)
        {
            var animal = await _db.Animals.FirstOrDefaultAsync(x => x.Id == id);
            if (animal == null)
            {
                return null;
            }
            return new AnimalModel
            {
                Id = animal.Id,
                Name = animal.Name,
                PasswordNo = animal.PasswordNo,
                ImageUrl = animal.ImageUrl
            };
        }

        public async Task<IList<AnimalModel>> GetAsync(AnimalSearchCriteria criteria)
        {
            var query = _db.Animals.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Name))
            {
                query = query.Where(x => x.Name.StartsWith(criteria.Name));
            }
            var animals = await query.ToListAsync();
            return animals.Select(x => new AnimalModel
            {
                Id = x.Id,
                Name = x.Name,
                PasswordNo = x.PasswordNo,
                ImageUrl = x.ImageUrl
            }).ToList();
        }
    }
}
