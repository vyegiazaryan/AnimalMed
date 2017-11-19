using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AnimalMed.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using AnimalMed.DTO.MedicalRecords;
using System.IO;
using AnimalMed.DTO.Infrastructure.Helpers;
using AnimalMed.DTO.SearchCriterias;
using Microsoft.AspNetCore.Mvc.Rendering;
using AnimalMed.DTO.Animals;

namespace AnimalMed.Web.Controllers
{
    public class MedicalRecordsController : Controller
    {
        private readonly IMedicalRecordsService _medicalRecordsService;
        private readonly IAnimalService _animalService;
        private readonly IHostingEnvironment _environment;

        public MedicalRecordsController(IMedicalRecordsService medicalRecordsService, IAnimalService animalService, IHostingEnvironment environment)
        {
            _medicalRecordsService = medicalRecordsService;
            _animalService = animalService;
            _environment = environment;
        }


        [Route("[controller]/[action]")]
        public async Task<ActionResult> Index(int? animalId)
        {
            var records = await _medicalRecordsService.GetAsync(animalId);
            return View(records);
        }

        public async Task<IActionResult> Create(int? animalId)
        {
            IList<AnimalModel> animals = new List<AnimalModel>();
            if (animalId.HasValue)
            {
                var animal = await _animalService.FirstAsync(animalId.Value);
                if (animal != null)
                {
                    animals.Add(animal);
                }
            }
            if(!animals.Any())
            {
                animals = await _animalService.GetAsync(new AnimalSearchCriteria());
            }
            ViewBag.Animals = animals.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });
            return View(new EditMedicalRecordModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? animalId, [Bind("AnimalId")] EditMedicalRecordModel model, IFormFile file)
        {
            IList<AnimalModel> animals = new List<AnimalModel>();
            if (animalId.HasValue)
            {
                var animal = await _animalService.FirstAsync(animalId.Value);
                if (animal != null)
                {
                    animals.Add(animal);
                }
            }
            if (!animals.Any())
            {
                animals = await _animalService.GetAsync(new AnimalSearchCriteria());
            }
            ViewBag.Animals = animals.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _medicalRecordsService.AddAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Error", result.Error);
                return View(model);
            }

            if (file != null)
            {
                var fileUrl = Path.Combine("/Files" + "/MedicalRecords/" + result.Data.AnimalId + "/" + result.Data.Id + Path.GetExtension(file.FileName));
                var ok = FileHelper.SaveFileFromStream(file.OpenReadStream(), Path.GetFullPath(_environment.WebRootPath + fileUrl));
                if (ok)
                {
                    model = result.Data;
                    model.MedicalRecordUrl = fileUrl;
                    await _medicalRecordsService.EditAsync(model);
                }
            }

            return RedirectToAction(nameof(Index), new { animalId = animalId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var record = await _medicalRecordsService.FirstAsync(id);
            if (record == null)
            {
                return NotFound();
            }

            var animals = await _animalService.GetAsync(new AnimalSearchCriteria());
            ViewBag.Animals = animals.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == record.AnimalId });

            return View(new EditMedicalRecordModel
            {
                Id = record.Id,
                AnimalId = record.AnimalId,
                MedicalRecordUrl = record.MedicalRecordUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("AnimalId,MedicalRecordUrl")] EditMedicalRecordModel model, IFormFile file)
        {
            var animals = await _animalService.GetAsync(new AnimalSearchCriteria());
            ViewBag.Animals = animals.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString(), Selected = x.Id == model.AnimalId });

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (file != null)
            {
                var fileUrl = Path.Combine("/Files" + "/MedicalRecords/" + model.AnimalId + "/" + model.Id + Path.GetExtension(file.FileName));
                var ok = FileHelper.SaveFileFromStream(file.OpenReadStream(), Path.GetFullPath(_environment.WebRootPath + fileUrl));
                if (ok)
                {
                    model.MedicalRecordUrl = fileUrl;
                    await _medicalRecordsService.EditAsync(model);
                }
            }

            var result = await _medicalRecordsService.EditAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Error", result.Error);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var record = await _medicalRecordsService.FirstAsync(id);
            if (record == null)
            {
                return NotFound();
            }
            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _medicalRecordsService.RemoveAsync(id);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Error", result.Error);
                return View(result.Data);
            }
            FileHelper.DeleteImage(result.Data.MedicalRecordUrl);
            return RedirectToAction(nameof(Index));
        }
    }
}