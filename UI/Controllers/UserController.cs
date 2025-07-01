using DataAccess.Concrete;
using Entity.Concrete.Dtos;
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


		[HttpPost]
		public async Task<IActionResult> UpdateProfile(UserUpdateViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(model.UserId);
			if (user == null)
			{
				return NotFound();
			}

			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.PhoneNumber = model.Phone;
			user.Address = model.Address;

			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				// İstersen TempData ile bir başarı mesajı dönebilirsin
				TempData["Success"] = "Profiliniz başarıyla güncellendi.";
				return RedirectToAction("UpdateProfile", "User");
			}
			else
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View(model);
			}
		}
	}
}
