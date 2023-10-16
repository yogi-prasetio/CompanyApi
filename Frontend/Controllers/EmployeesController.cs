using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Active()
        {
            return View();
        }
        public IActionResult Resign()
        {
            return View();
        }
    }
}
