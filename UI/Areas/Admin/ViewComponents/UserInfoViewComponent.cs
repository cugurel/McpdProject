using DataAccess.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Areas.Admin.Models;
using UI.Models.Identity;

namespace UI.Areas.Admin.ViewComponents
{
	public class UserInfoViewComponent:ViewComponent
	{
		private readonly UserManager<User> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public UserInfoViewComponent(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			UserInfo userInfo = new UserInfo();
			if (userId != null)
			{
				var user = await _userManager.FindByIdAsync(userId);
				if (user != null)
				{
					userInfo.Name = user.FirstName;
					userInfo.Email = user.Email;
					userInfo.Surname = user.LastName;
				}
			}
			return View(userInfo);

		}
	}
}
