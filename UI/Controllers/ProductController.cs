using DataAccess.Concrete;
using DataAccess.Concrete.EfRepositories;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace UI.Controllers
{
	public class ProductController : Controller
	{
		EfProductRepository repository = new EfProductRepository();
		Context c= new Context();
		public IActionResult Index()
		{
			var value = repository.GetAll();
			return View(value);
		}

		public IActionResult DeleteProduct(int Id)
		{
			//Linq ile id kullanarak veri çekme
			var value = repository.GetById(Id);
			repository.Delete(value);
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
			repository.Add(product);
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

			var value = repository.GetById(Id);
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
			repository.Update(product);
			return RedirectToAction("Index", "Product");
		}
	}
}
