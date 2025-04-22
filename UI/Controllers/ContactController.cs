using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
