using DataAccess.Concrete;
using Entity.Concrete;
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
	}
}
