using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult UserDetail()
		{
			return View();
		}
	}
}
