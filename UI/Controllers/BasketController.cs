using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;

namespace UI.Controllers
{
	public class BasketController : Controller
	{
		Context context = new Context();
		
		[HttpPost]
		public IActionResult AddToBasket([FromBody] Basket model)
		{
			model.Quantity = 1; 
			model.TotalPrice = model.Price * model.Quantity;
			model.Status = true;
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

		[HttpGet]
		public IActionResult GetBasketHtml()
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
								  TotalPrice = basket.Price * basket.Quantity,
								  Status = basket.Status
							  }).Where(x=>x.Status == true).ToList();

			var totalBasketPrice = basketList.Sum(x => x.TotalPrice);

			// Razor olmadan, HTML string oluşturuluyor
			string html = "<ul class='header-cart-wrapitem w-full'>";
			foreach (var item in basketList)
			{
				html += $@"
	<li class='header-cart-item flex-w flex-t m-b-12'>
		<div class='header-cart-item-img'>
			<img src='/ProductImages/{item.ImagePath}' alt='IMG'>
		</div>
		<div class='header-cart-item-txt p-t-8'>
			<a href='#' class='header-cart-item-name m-b-18 hov-cl1 trans-04'>
				{item.ProductName}
			</a>
			<span class='header-cart-item-info'>
				{item.Quantity} x {item.Price} ₺
			</span>

			<!-- Sil butonu -->
			<button class='btn btn-sm btn-danger mt-2 delete-from-cart' data-basket-id='{item.Id}'>
				<i class='zmdi zmdi-delete'></i> Sil
			</button>
		</div>
	</li>";
			}

			html += "</ul>";

			html += $@"
	<div class='w-full'>
		<div class='header-cart-total w-full p-tb-40'>
			Total: {totalBasketPrice} ₺
		</div>

		<div class='header-cart-buttons flex-w w-full'>
			<a href='/Basket/BasketDetail' class='flex-c-m stext-101 cl0 size-107 bg3 bor2 hov-btn3 p-lr-15 trans-04 m-r-8 m-b-10'>
				Sepet
			</a>

			<a href='shoping-cart.html' class='flex-c-m stext-101 cl0 size-107 bg3 bor2 hov-btn3 p-lr-15 trans-04 m-b-10'>
				Ödeme
			</a>
		</div>
	</div>";

			return Json(new
			{
				html,
				count = basketList.Count
			});
		}

		[HttpPost]
		public IActionResult DeleteFromBasket(int basketId)
		{
			var basketItem = context.Baskets.FirstOrDefault(x => x.Id == basketId);
			if (basketItem != null)
			{
				context.Baskets.Remove(basketItem);
				context.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false, message = "Ürün bulunamadı." });
		}

		public IActionResult BasketDetail()
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
								  TotalPrice = basket.Price * basket.Quantity,
								  Status = basket.Status
							  })
				  .Where(x => x.UserId == userId && x.Status == true)
				  .ToList();

			var totalBasketPrice = basketList.Sum(x => x.TotalPrice);

			ViewBag.TotalBasketPrice = totalBasketPrice;
			ViewBag.BasketCount = basketList.Count();
			return View(basketList);
		}

		[HttpPost]
		public IActionResult UpdateQuantity(int basketId, int quantity)
		{
			var item = context.Baskets.FirstOrDefault(x => x.Id == basketId);
			if (item != null)
			{
				item.Quantity = quantity;
				item.TotalPrice = item.Price * quantity;
				context.Baskets.Update(item);// TotalPrice'ı güncelle
				context.SaveChanges();

				return Json(new { success = true });
			}

			return Json(new { success = false, message = "Ürün bulunamadı." });
		}

		public IActionResult Checkout()
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var baskets = context.Baskets.Where(x => x.UserId == userId).ToList();
			Order order = new Order
			{
				UserId = userId,
				CreatedDate = DateTime.Now
			};
			context.Orders.Add(order);
			context.SaveChanges();
			foreach (var item in baskets)
			{
				OrderDetail orderdetail = new OrderDetail
				{
					OrderId = order.Id,
					ProductId = item.ProductId,
					Quantity = item.Quantity,
					UnitPrice = item.Price,
					TotalPrice = item.TotalPrice * item.Quantity,
				};

				context.OrderDetails.Add(orderdetail);
				
			}
			context.SaveChanges();
			foreach (var item in baskets)
			{
				var basketItem = context.Baskets.FirstOrDefault(x => x.Id == item.Id);
				basketItem.Status = false; // Sepet durumunu güncelle
				context.Update(basketItem);
				context.SaveChanges();
			}
			return RedirectToAction("Index", "Home");
		}

	}
}
