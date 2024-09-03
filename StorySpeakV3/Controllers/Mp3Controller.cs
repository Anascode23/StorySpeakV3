using Microsoft.AspNetCore.Mvc;

namespace StorySpeakV3.Controllers
{
    public class Mp3Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
