using beestje_op_je_feestje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using beestje_op_je_feestje.ViewModels;

namespace beestje_op_je_feestje.Controllers
{
    public class AnimalController : Controller
    {
        private readonly AnimalPartyContext _context;
        private readonly IWebHostEnvironment _environment;

        public AnimalController(AnimalPartyContext context, IWebHostEnvironment environment)
        {
            _context = context;
            //for uploading files
            _environment = environment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var animals = _context.animals.ToList();
            return View(animals);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AnimalViewModel viewModel, IFormFile imageFile)
        {
            //upload & store image first
            await ProcessImage(viewModel, imageFile);

            if (!ModelState.IsValid && viewModel.ImageUrl == null)
            {
                return View(viewModel);
            }

            var animal = new Animal
            {
                Name = viewModel.Name,
                Type = viewModel.Type,
                Price = viewModel.Price,
                ImageUrl = viewModel.ImageUrl
            };

            _context.animals.Add(animal);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = animal.Name + " is succesvol toegevoegd!";
            return RedirectToAction("Index");
        }

        private async Task ProcessImage(AnimalViewModel viewModel, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                viewModel.ImageUrl = "/uploads/" + fileName;
            }
            else
            {
                ModelState.AddModelError("imageFile", "An image is required.");
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var animal = _context.animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            var viewModel = new AnimalViewModel
            {
                Id = animal.Id,
                Name = animal.Name,
                Type = animal.Type,
                Price = animal.Price,
                ImageUrl = animal.ImageUrl
            };

            return View(viewModel);  
        }


        [HttpPost]
        public async Task<IActionResult> Edit(AnimalViewModel viewModel, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);  
            }

            var animal = _context.animals.FirstOrDefault(a => a.Id == viewModel.Id);
            if (animal == null)
            {
                return NotFound();
            }

            //voor image
            if (imageFile != null && imageFile.Length > 0 && viewModel.ImageUrl == null)
            {
                await ProcessImage(viewModel, imageFile);  
            }

            animal.Name = viewModel.Name;
            animal.Type = viewModel.Type;
            animal.Price = viewModel.Price;
            animal.ImageUrl = viewModel.ImageUrl;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Animal updated successfully!";
            return RedirectToAction("Index");  
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var animal = _context.animals
                .Where(a => a.Id == id)
                .Select(a => new AnimalViewModel
                {
                    Name = a.Name,
                    Type = a.Type,
                    Price = a.Price,
                    ImageUrl = a.ImageUrl
                })
                .FirstOrDefault();

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var animal = _context.animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
            {
                TempData["ErrorMessage"] = "Dier niet gevonden!";
                return RedirectToAction("Index");
            }

            _context.animals.Remove(animal);
            _context.SaveChanges();

            if (!_context.animals.Any())
            {
                _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('animals', RESEED, 0)");
            }

            TempData["SuccessMessage"] = animal.Name + " is succesvol verwijderd!";
            return RedirectToAction("Index");
        }

    }
}
