using DataAccess.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UI.Areas.Admin.Models;
using UI.Models.Identity;

public class StatisticViewComponent : ViewComponent
{
	Context context = new Context();
	private readonly UserManager<User> _userManager;

	public StatisticViewComponent(UserManager<User> userManager)
	{
		_userManager = userManager;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var statistic = new Statistic
		{
			NumberOfProduct = await context.Products.CountAsync(),
			NumberOfCustomer = await _userManager.Users.CountAsync(u => u.Type == "Customer"),
			NumberOfOrder = await context.Orders.CountAsync(),
			TotalIncome = await context.OrderDetails.SumAsync(o=>o.TotalPrice),
			DailyOrder = await context.Orders.CountAsync(o => o.CreatedDate.Date == DateTime.Today)
		};

		return View(statistic);
	}
}
