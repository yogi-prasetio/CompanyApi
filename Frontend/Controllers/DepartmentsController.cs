﻿using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
