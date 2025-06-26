using DataAccess.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.Models.Identity;

namespace UI.Controllers
{
	public class UserController : Controller
	{
		UserManager<User> _userManager;
		Context c = new Context();
		public UserController(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public IActionResult List()
		{
			var users = _userManager.Users.ToList();
			return View(users);
		}

		public async Task<IActionResult> UserDetail(string Id)
		{
			if(Id == null)
			{
				var userId =  User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
				var user = await _userManager.FindByIdAsync(userId);
				ViewBag.CommentCount = c.ProductReviews.Where(x => x.UserId == userId).Count();
				return View(user);
			}

			var userWithId = await _userManager.FindByIdAsync(Id);
			ViewBag.CommentCount = c.ProductReviews.Where(x => x.UserId == Id).Count();
			return View(userWithId);

		}

		public async Task<IActionResult> UpdateProfile(string Id)
		{
			if (Id == null)
			{
				var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
				var user = await _userManager.FindByIdAsync(userId);
				ViewBag.CommentCount = c.ProductReviews.Where(x => x.UserId == userId).Count();
				return View(user);
			}

			var userWithId = await _userManager.FindByIdAsync(Id);
			ViewBag.CommentCount = c.ProductReviews.Where(x => x.UserId == Id).Count();
			return View(userWithId);

		}
	}
}
