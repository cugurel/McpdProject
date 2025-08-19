using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Mvc;
using UI.Models.Identity;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
		Context context = new Context();
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult OrderDetail(int id)
		{
			var orderDetails = context.OrderDetails.Where(x => x.OrderId == id).ToList();
			var result = (from od in orderDetails
						  join p in context.Products on od.ProductId equals p.Id
						  select new OrderDetailDto
						  {
							  ProductName = p.Name,
							  Price = od.UnitPrice,
							  Quantity = od.Quantity,
							  TotalPrice = od.TotalPrice,
						  }).ToList();
			

			return View(result);
		}
	}
}
