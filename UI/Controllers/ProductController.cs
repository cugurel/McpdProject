using DataAccess.Concrete;
using Entity.Concrete;
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

		public IActionResult DeleteProduct(int Id)
		{
			//Linq ile id kullanarak veri çekme
			var value = c.Products.Find(Id);
			c.Products.Remove(value);
			c.SaveChanges();
			return RedirectToAction("Index", "Product");
		}

		[HttpGet]
		public IActionResult AddProduct()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddProduct(Product product)
		{
			c.Products.Add(product);
			c.SaveChanges();
			return RedirectToAction("Index", "Product");
		}
	}
}
