using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete;
using DataAccess.Concrete.DapperRepositoru;
using DataAccess.Concrete.EfRepositories;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace UI.Controllers
{
	public class ProductController : Controller
	{
		IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		Context c= new Context();
		public IActionResult Index()
		{
			var value = _productService.GetAll();
			return View(value);
		}

		public IActionResult DeleteProduct(int Id)
		{
			var value = _productService.GetById(Id);
			_productService.Delete(value);
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
		public async Task<IActionResult> AddProduct(Product product)
		{
			ViewBag.Categories = c.Categories.Select(u => new SelectListItem
			{
				Value = u.Id.ToString(),
				Text = u.Name
			}).ToList();


			if(product.File != null)
			{
				var item = product.File;
				var extend = Path.GetExtension(item.FileName);
				var randomName = ($"{Guid.NewGuid()}{extend}");
				var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\ProductImages", randomName);

				using (var stream = new FileStream(path,FileMode.Create))
				{
					await item.CopyToAsync(stream);
				}

				product.ImagePath = randomName;
			}

			product.CreatedDate = DateTime.Now;
			product.IsActive = true;
			_productService.Add(product);
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

			var value = _productService.GetById(Id);
			return View(value);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateProduct(Product product)
		{
			ViewBag.Categories = c.Categories.Select(u => new SelectListItem
			{
				Value = u.Id.ToString(),
				Text = u.Name
			}).ToList();

			if (product.File != null)
			{
				var item = product.File;
				var extend = Path.GetExtension(item.FileName);
				var randomName = ($"{Guid.NewGuid()}{extend}");
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ProductImages", randomName);

				if (!string.IsNullOrEmpty(product.ImagePath))
				{
					var oldPath = Path.Combine(Directory.GetCurrentDirectory(), 
						"wwwroot\\ProductImages", product.ImagePath);
					if (System.IO.File.Exists(oldPath))
					{
						System.IO.File.Delete(oldPath);
					}
				}

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await item.CopyToAsync(stream);
				}

				product.ImagePath = randomName;
				product.CreatedDate = DateTime.Now;
				product.IsActive = true;
			}
			_productService.Update(product);
			return RedirectToAction("Index", "Product");
		}

		public IActionResult ProductDetail(int Id)
		{
			var value = _productService.GetById(Id);
			return View(value);
		}
	}
}
