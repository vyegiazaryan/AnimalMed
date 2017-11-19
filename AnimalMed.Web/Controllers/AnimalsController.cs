using AnimalMed.BLL.Interfaces;
using AnimalMed.DTO.Animals;
using AnimalMed.DTO.Infrastructure.Helpers;
using AnimalMed.DTO.SearchCriterias;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalMed.Web.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly IAnimalService _animalService;
        private readonly IHostingEnvironment _environment;

        public AnimalsController(IAnimalService animalService, IHostingEnvironment environment)
        {
            _animalService = animalService;
            _environment = environment;
        }

        public async Task<IActionResult> Index(AnimalSearchCriteria criteria)
        {
            var animals = await _animalService.GetAsync(criteria);
            return View(animals);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var animal = await _animalService.FirstAsync(id.Value);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }


        public IActionResult Create()
        {
            return View(new EditAnimalModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,PasswordNo,ImageUrl")] EditAnimalModel model, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _animalService.AddAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Error", result.Error);
                return View(model);
            }

            if (image != null)
            {
                var imgUrl = Path.Combine("/Images" + "/Animals/" + result.Data.Id + Path.GetExtension(image.FileName));
                var ok = FileHelper.SaveImageFromStream(image.OpenReadStream(), Path.GetFullPath(_environment.WebRootPath + imgUrl));
                if (ok)
                {
                    model = result.Data;
                    model.ImageUrl = imgUrl;
                    await _animalService.EditAsync(model);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var animal = await _animalService.FirstAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            return View(new EditAnimalModel
            {
                 Id = animal.Id,
                 Name = animal.Name,
                 PasswordNo = animal.PasswordNo,
                 ImageUrl = animal.ImageUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAnimalModel model, IFormFile image)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (image != null)
            {
                var imgUrl = Path.Combine("/Images" + "Animals/" + model.Id + Path.GetExtension(image.FileName));
                var ok = FileHelper.SaveImageFromStream(image.OpenReadStream(), Path.GetFullPath(_environment.WebRootPath + imgUrl));
                if (ok)
                {
                    model.ImageUrl = imgUrl;
                }
            }

            var result = await _animalService.EditAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Error", result.Error);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _animalService.FirstAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            return View(animal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _animalService.RemoveAsync(id);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Error", result.Error);
                return View(result.Data);
            }
            FileHelper.DeleteImage(result.Data.ImageUrl);
            return RedirectToAction(nameof(Index));
        }

    }
}
