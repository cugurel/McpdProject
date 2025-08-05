using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Mvc;
using UI.Models.Identity;

namespace UI.Controllers
{
	public class OrderController : Controller
	{
		Context c = new Context();
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult List()
		{
			var orderList = (from order in c.Orders
							 join product in c.Products on order.ProductId equals product.Id
							 select new UserOrderDto
							 {
								 OrderId = order.Id,
								 ProductId = order.ProductId,
								 ProductName = product.Name,
								 Quantity = order.Quantity,
								 UnitPrice = order.UnitPrice,
								 OrderDate = order.CreatedDate
							 }).ToList();

			return View(orderList);
		}
	}
}
