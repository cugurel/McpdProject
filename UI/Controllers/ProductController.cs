using DataAccess.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
	public class ProductController : Controller
	{
		Context c = new Context();
		public IActionResult Index()
		{
			var value = c.Products.ToList();
			return View(value);
		}
	}
}
