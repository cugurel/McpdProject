using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.Models.Identity;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AuthController : Controller
	{
		UserManager<User> _userManager;
		SignInManager<User> _signInManager;

		public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Login(LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Login", "Auth", new { area = "Admin" });
			}

			var userWithEmail = await _userManager.FindByEmailAsync(model.Email);
			if (userWithEmail == null)
			{
				TempData["LoginError"] = "Email veya Şifre hatalı";
			}

			var result = await _signInManager.PasswordSignInAsync(userWithEmail, model.Password, false, false);
			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home", new { area = "Admin" });
			}
			return View();
		}

		

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Register(RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var user = new User
			{
				FirstName = model.Firstname,
				Email = model.Email,
				LastName = model.Surname,
				PhoneNumber = model.Phone,
				UserName = model.Username,
				Type = "System User"
			};

			var userWithEmail = await _userManager.FindByEmailAsync(user.Email);
			if (userWithEmail != null)
			{
				return View();
			}

			var result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				return RedirectToAction("Login", "Auth", new { area = "Admin" });
			}
			return View();
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Auth", new { area = "Admin" });
		}

	}
}
