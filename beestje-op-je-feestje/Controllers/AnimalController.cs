using beestje_op_je_feestje.Models;
using Microsoft.AspNetCore.Mvc;

namespace beestje_op_je_feestje.Controllers
{
    public class AnimalController : Controller
    {
        private readonly AnimalPartyContext _context;
        public AnimalController(AnimalPartyContext context)
        {
            _context = context;

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
            if (!ModelState.IsValid)
            {

                return View(model); 
            }

            if (imageFile != null)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine("wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                model.imageUrl = "/uploads/" + fileName;
            }

            _context.animals.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }



}
