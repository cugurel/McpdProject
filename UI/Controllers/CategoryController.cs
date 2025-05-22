using Business.Concrete;
using DataAccess.Concrete;
using DataAccess.Concrete.EfRepositories;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Controllers
{
	public class CategoryController : Controller
	{
		CategoryManager manager = new CategoryManager(new EfCategoryRepository());
		public IActionResult Index()
		{
			var value = manager.GetAll();
			return View(value);
		}


		public IActionResult DeleteCategory(int Id)
		{
			var value = manager.GetById(Id);
			manager.Delete(value);
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
			manager.Add(category);
			return RedirectToAction("Index", "Category");
		}

		[HttpGet]
		public IActionResult UpdateCategory(int Id)
		{
			var value = manager.GetById(Id);
			return View(value);
		}

		[HttpPost]
		public IActionResult UpdateCategory(Category category)
		{
			manager.Update(category);
			return RedirectToAction("Index", "Category");
		}
	}
}
