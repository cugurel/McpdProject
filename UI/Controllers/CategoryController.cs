using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Controllers
{
	public class CategoryController : Controller
	{
		Context c = new Context();
		public IActionResult Index()
		{
			var value = c.Categories.ToList();
			return View(value);
		}


		public IActionResult DeleteCategory(int Id)
		{
			//Linq ile id kullanarak veri çekme
			var value = c.Categories.Find(Id);
			c.Categories.Remove(value);
			c.SaveChanges();
			return RedirectToAction("Index", "Category");
		}

		[HttpGet]
		public IActionResult AddCategory()
		{
			
			return View();
		}

		[HttpPost]
		public IActionResult AddCategory(Category category)
		{
			c.Categories.Add(category);
			c.SaveChanges();
			return RedirectToAction("Index", "Category");
		}

		[HttpGet]
		public IActionResult UpdateCategory(int Id)
		{
			var value = c.Categories.Find(Id);
			return View(value);
		}

		[HttpPost]
		public IActionResult UpdateCategory(Category category)
		{
			c.Categories.Update(category);
			c.SaveChanges();
			return RedirectToAction("Index", "Category");
		}
	}
}
