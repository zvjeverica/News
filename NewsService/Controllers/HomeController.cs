using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NewsService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Api()
        {
            ViewData["Message"] = "Application Programming Interface.";

            return View();
        }

        public ActionResult GetStructure()
        {
            return File("files/StructureOnly.sql", "application/sql", "Structure.sql");
        }

        public ActionResult GetStructureAndData()
        {
            return File("files/StructureAndData.sql", "application/sql", "StructureAndData.sql");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
