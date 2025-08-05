using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models.Identity;

namespace UI.Controllers
{
	public class OrderController : Controller
	{
		Context c = new Context();

		UserManager<User> _userManager;

		public OrderController(UserManager<User> userManager)
		{
			_userManager = userManager;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> List()
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

			var result = (from order in orderList
						  join user in users on order.UserId equals user.Id
						  select new UserOrderDto
						  {
							  OrderId = order.Id,
							  UserId = user.FirstName + " " + user.LastName,
							  OrderDate = order.CreatedDate
						  }).ToList();

			return View(result);

		}
	}
}
