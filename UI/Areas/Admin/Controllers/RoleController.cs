using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models.Identity;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,SuperAdmin")]
	public class RoleController : Controller
	{
		RoleManager<IdentityRole> _roleManager;
		UserManager<User> _userManager;

		public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task<IActionResult> List()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			return View(roles);
		}

		[HttpGet]
		public async Task<IActionResult> AddRole()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddRole(RoleModel roleModel)
		{
			if (ModelState.IsValid)
			{
				var result = await _roleManager.CreateAsync(new IdentityRole(roleModel.Name));
				if (result.Succeeded)
				{
					return RedirectToAction("List", "Role", new { area = "Admin" });
				}
			}
			return View();
		}

		public async Task<IActionResult> DeleteRole(string Id)
		{
			var role = await _roleManager.FindByIdAsync(Id);
			await _roleManager.DeleteAsync(role);
			return RedirectToAction("Index", "Role", new { area = "Admin" });
		}

		public async Task<IActionResult> UpdateRole(string Id)
		{
			var role = await _roleManager.FindByIdAsync(Id);

			var members = new List<User>();
			var nonMembers = new List<User>();

			foreach (var user in _userManager.Users)
			{
				var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
				list.Add(user);
			}

			var model = new RoleDetails()
			{
				Role = role,
				Members = members,
				NonMembers = nonMembers
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateRole(RoleEditModel model)
		{
			if (ModelState.IsValid)
			{
				foreach (var userId in model.IdsToAdd ?? new string[] { })
				{
					var user = await _userManager.FindByIdAsync(userId);
					if (user != null)
					{
						var result = await _userManager.AddToRoleAsync(user, model.RoleName);
						if (!result.Succeeded)
						{
							foreach (var error in result.Errors)
							{
								ModelState.AddModelError("", error.Description);
							}
						}
					}
				}

				foreach (var userId in model.IdsToDelete ?? new string[] { })
				{
					var user = await _userManager.FindByIdAsync(userId);
					if (user != null)
					{
						var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
						if (!result.Succeeded)
						{
							foreach (var error in result.Errors)
							{
								ModelState.AddModelError("", error.Description);
							}
						}
					}
				}
			}

			return RedirectToAction(
	"UpdateRole",      // action adı
	"Role",            // controller adı
	new { area = "Admin", Id = model.RoleId } // area + parametre
);
		}
	}
}