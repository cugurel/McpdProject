using Business.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UI.Models.Identity;

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
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			ViewBag.UserId = userId;
			ViewClass viewClass = new ViewClass
			{
				Campaigns = context.Campaigns.Take(3).ToList(),
				Products = _productService.GetAll()
			};

			var baskets = context.Baskets.Where(x=>x.UserId == userId).ToList();
			var products = context.Products.ToList();
			var basketList = (from basket in baskets
							  join product in products on basket.ProductId equals product.Id
							  select new BasketDto
							  {
								  Id = basket.Id,
								  UserId = basket.UserId,
								  ProductId = product.Id,
								  ProductName = product.Name + " " + product.Description,
								  Quantity = basket.Quantity,
								  Price = basket.Price,
								  ImagePath = product.ImagePath,
								  TotalPrice = (int)(basket.Price * basket.Quantity),
								  Status = basket.Status
							  })
				  .Where(x => x.UserId == userId && x.Status==true)
				  .ToList();

			var totalBasketPrice = basketList.Sum(x => x.TotalPrice);

			ViewBag.BasketList = basketList;
			ViewBag.TotalBasketPrice = totalBasketPrice;
			ViewBag.BasketCount = basketList.Count();

			return View(viewClass);
		}

        public IActionResult List()
        {
            return View();
        }
    }
}
