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
			ViewBag.Name = "�a�r�";
			//ViewData
			ViewData["Yas"] = 32;
			//Tempdata
			TempData["mesaj"] = "Kay�t ba�ar�l�";
			return View();
		}

        public IActionResult List()
        {
            ViewBag.Name = "�a�r�";
            return View();
        }
    }
}
