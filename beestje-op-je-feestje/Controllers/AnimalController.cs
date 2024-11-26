using Microsoft.AspNetCore.Mvc;

namespace beestje_op_je_feestje.Controllers
{
    public class AnimalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }


    }
}
