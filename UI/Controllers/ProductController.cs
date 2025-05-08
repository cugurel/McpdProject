using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
			ViewBag.Categories = c.Categories.Select(u => new SelectListItem
			{
				Value = u.Id.ToString(),
				Text = u.Name
			}).ToList();

			return View();
		}

		[HttpPost]
		public IActionResult AddProduct(Product product)
		{
			ViewBag.Categories = c.Categories.Select(u => new SelectListItem
			{
				Value = u.Id.ToString(),
				Text = u.Name
			}).ToList();

			product.CreatedDate = DateTime.Now;
			product.IsActive = true;
			c.Products.Add(product);
			c.SaveChanges();
			return RedirectToAction("Index", "Product");
		}


		[HttpGet]
		public IActionResult UpdateProduct(int Id)
		{
			ViewBag.Categories = c.Categories.Select(u => new SelectListItem
			{
				Value = u.Id.ToString(),
				Text = u.Name
			}).ToList();

			var value = c.Products.Find(Id);
			return View(value);
		}

		[HttpPost]
		public IActionResult UpdateProduct(Product product)
		{
			ViewBag.Categories = c.Categories.Select(u => new SelectListItem
			{
				Value = u.Id.ToString(),
				Text = u.Name
			}).ToList();

			product.CreatedDate = DateTime.Now;
			product.IsActive = true;
			c.Products.Update(product);
			c.SaveChanges();
			return RedirectToAction("Index", "Product");
		}
	}
}
