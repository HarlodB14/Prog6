using beestje_op_je_feestje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Create(Animal model, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                if (imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    model.imageUrl = "/uploads/" + fileName;

                    ModelState.Remove("imageUrl");
                }
            }
            else
            {
                ModelState.AddModelError("imageFile", "An image is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.animals.Add(model);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = model.name + " is succesvol toegevoegd!";
            return RedirectToAction("Index");
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

            TempData["SuccessMessage"] = animal.name + " is succesvol verwijderd!";
            return RedirectToAction("Index");
        }

    }
}
