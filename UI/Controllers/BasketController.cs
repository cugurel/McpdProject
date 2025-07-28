using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace UI.Controllers
{
	public class BasketController : Controller
	{
		Context context = new Context();
		
		[HttpPost]
		public IActionResult AddToBasket([FromBody] Basket model)
		{
			model.Quantity = 1; // Sepete eklenen ürünün varsayılan miktarı 1 olarak ayarlanıyor
			model.TotalPrice = model.Price * model.Quantity; // Toplam fiyat, ürün fiyatı ile miktarın çarpımı olarak hesaplanıyor
			if (model.UserId.IsNullOrEmpty())
			{
				return Json(new { success = false, message = "Öncelikle sisteme giriş yapmalısınız" });
			}
			else
			{
				context.Baskets.Add(model);
				context.SaveChanges();
				return Json(new { success = true,message = "Ürün sepete eklendi!" });
			}	
		}

		public IActionResult BasketList()
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var baskets = context.Baskets.Where(x => x.UserId == userId).ToList();
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
								  TotalPrice = basket.Price * basket.Quantity
							  })
				  .Where(x => x.UserId == userId)
				  .ToList();

			// Sepetin toplam fiyatını hesapla
			var totalBasketPrice = basketList.Sum(x => x.TotalPrice);

			ViewBag.BasketList = basketList;
			ViewBag.TotalBasketPrice = totalBasketPrice;
			ViewBag.BasketCount = basketList.Count();
			return View();
		}
	}
}
