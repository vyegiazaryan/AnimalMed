using AnimalMed.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalMed.DTO;
using AnimalMed.DTO.MedicalRecords;
using AnimalMed.DAL.EF;
using AnimalMed.BLL.Services;
using AnimalMed.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimalMed.BLL.Services
{
    public class MedicalRecordsService : ServiceBase, IMedicalRecordsService
    {
        public MedicalRecordsService(AnimalMedDbContext context) : base(context)
        {

        }

        public async Task<ServiceResult<EditMedicalRecordModel>> AddAsync(EditMedicalRecordModel model)
        {
            var entity = new MedicalRecord
            {
                MedicalRecordUrl = model.MedicalRecordUrl,
                AnimalId = model.AnimalId
            };

            _db.Add(entity);

            try
            {
                await _db.SaveChangesAsync();

                return ServiceResult<EditMedicalRecordModel>.Success(new EditMedicalRecordModel
                {
                    Id = entity.Id,
                    MedicalRecordUrl = model.MedicalRecordUrl,
                    AnimalId = model.AnimalId
                });
            }
            catch (Exception ex)
            {
                //log error
                return ServiceResult<EditMedicalRecordModel>.Fail("Internal server error");
            }
        }

        public async Task<ServiceResult<EditMedicalRecordModel>> EditAsync(EditMedicalRecordModel model)
        {
            var entity = await _db.MedicalRecords.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (entity == null)
            {
                return ServiceResult<EditMedicalRecordModel>.Fail("MedicalRecord not found");
            }
            entity.AnimalId = model.AnimalId;
            entity.MedicalRecordUrl = model.MedicalRecordUrl;

            try
            {
                await _db.SaveChangesAsync();

                return ServiceResult<EditMedicalRecordModel>.Success(new EditMedicalRecordModel
                {
                    Id = entity.Id,
                    AnimalId = entity.AnimalId,
                    MedicalRecordUrl = entity.MedicalRecordUrl
                });
            }
            catch (Exception ex)
            {
                //log error
                return ServiceResult<EditMedicalRecordModel>.Fail("Internal server error");
            }
        }

        public async Task<ServiceResult<MedicalRecordModel>> RemoveAsync(int id)
        {
            var entity = await _db.MedicalRecords.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return ServiceResult<MedicalRecordModel>.Fail("MedicalRecord not found");
            }
            try
            {
                _db.MedicalRecords.Remove(entity);
                await _db.SaveChangesAsync();

                return ServiceResult<MedicalRecordModel>.Success(new MedicalRecordModel
                {
                    Id = entity.Id,
                    MedicalRecordUrl = entity.MedicalRecordUrl,
                    AnimalId = entity.AnimalId
                });
            }
            catch (Exception ex)
            {
                //todo: log error
                return ServiceResult<MedicalRecordModel>.Fail("Internal server error");
            }
        }


        public async Task<MedicalRecordModel> FirstAsync(int id)
        {
            var medicalRecord = await _db.MedicalRecords.Include(x => x.Animal).FirstOrDefaultAsync(x => x.Id == id);
            if (medicalRecord == null)
            {
                return null;
            }
            return new MedicalRecordModel
            {
                Id = medicalRecord.Id,
                MedicalRecordUrl = medicalRecord.MedicalRecordUrl,
                AnimalId = medicalRecord.AnimalId,
                AnimalName = medicalRecord.Animal?.Name
            };
        }

        public async Task<IList<MedicalRecordModel>> GetAsync(int? animalId)
        {
            var query = _db.MedicalRecords.Include(x => x.Animal).AsQueryable();
            if (animalId.HasValue)
            {
                query = query.Where(x => x.AnimalId == animalId);
            }
            var medicalRecords = await query.ToListAsync();
            return medicalRecords.Select(x => new MedicalRecordModel
            {
                Id = x.Id,
                AnimalId = x.AnimalId,
                AnimalName = x.Animal.Name,
                MedicalRecordUrl = x.MedicalRecordUrl
            }).ToList();
        }
    }
}
