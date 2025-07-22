using Business.Abstract;
using DataAccess.Concrete;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace UI.Controllers
{
	public class HomeController : Controller
	{
		ICategoryService _categoryService;
		IProductService _productService;
		Context context = new Context();

		public HomeController(IProductService productService, ICategoryService categoryService)
		{
			_productService = productService;
			_categoryService = categoryService;
		}

		public IActionResult Index()
		{
			ViewClass viewClass = new ViewClass
			{
				Categories = context.Categories.Take(3).ToList(),
				Products = _productService.GetAll()
			};
			
			return View(viewClass);
		}

        public IActionResult List()
        {
            return View();
        }
    }
}
