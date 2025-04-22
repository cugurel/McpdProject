using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace UI.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			//ViewBag
			ViewBag.Name = "Çaðrý";
			//ViewData
			ViewData["Yas"] = 32;
			//Tempdata
			TempData["mesaj"] = "Kayýt baþarýlý";
			return View();
		}

		[HttpGet]
        public IActionResult List()
        {
            ViewBag.Name = "Çaðrý";
            return View();
        }
    }
}
