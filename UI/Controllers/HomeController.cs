using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace UI.Controllers
{
	public class HomeController : Controller
	{
		[Authorize]
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

        public IActionResult List()
        {
            ViewBag.Name = "Çaðrý";
            return View();
        }
    }
}
