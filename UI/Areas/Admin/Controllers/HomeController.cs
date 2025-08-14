using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models.Identity;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : Controller
	{
		Context c = new Context();

		UserManager<User> _userManager;

		public HomeController(UserManager<User> userManager)
		{
			_userManager = userManager;
		}
		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users
	.Select(u => new { u.Id, u.FirstName, u.LastName })
	.ToListAsync();

			var orderList = await c.Orders
				.Select(order => new
				{
					order.Id,
					order.UserId,
					order.CreatedDate
				})
				.ToListAsync();

			var orderDetails = c.OrderDetails.ToList();

			var result = (from order in orderList
						  join user in users on order.UserId equals user.Id
						  join od in orderDetails on order.Id equals od.OrderId into orderDetailGroup
						  select new UserOrderDto
						  {
							  OrderId = order.Id,
							  UserId = user.FirstName + " " + user.LastName,
							  OrderDate = order.CreatedDate,
							  TotalPrice = orderDetailGroup.Sum(x => x.TotalPrice)
						  }).ToList();

			return View(result);
		}
	}
}
