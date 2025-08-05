using DataAccess.Concrete;
using Entity.Concrete;
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
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			
			
			ViewBag.CurrentTab = HttpContext.Session.GetString("CurrentTab") ?? "UserDetail";
			var orders = c.Orders.ToList();
			var reviews = c.ProductReviews.Where(x => x.UserId == userId).ToList();
			var products = c.Products.ToList();
			var orderDetails = c.OrderDetails.ToList();
			if (Id == null)
			{
				ViewBag.UserId = userId;
				ViewBag.SystemUserId = userId;
				var user = await _userManager.FindByIdAsync(userId);
				ViewBag.CommentCount = c.ProductReviews.Where(x => x.UserId == userId).Count();
				
				var reviewList = (from review in reviews
									join product in products on review.ProductId equals product.Id
									select new ProductReviewDto
									{
										ReviewId = review.Id,
										ProductId = review.ProductId,
										Comment = review.Comment,
										UserId = user.Id,
										UserName = user.FirstName + " " + user.LastName,
										Date = review.CreatedDate,
										ProductName = product.Name + " " + product.Description
									}).Where(x=>x.UserId == userId).ToList();

				ViewBag.ProductReviewsFromUser = reviewList;
				
				var orderList = (from orderdetail in orderDetails
								join order in orders on orderdetail.OrderId equals order.Id
								  join product in products on orderdetail.ProductId equals product.Id
								  select new UserOrderDto
								  {
									  UserId = order.UserId,
									  OrderId = orderdetail.Id,
									  ProductId = orderdetail.ProductId,
									  ProductName = product.Name,
									  Quantity = orderdetail.Quantity,
									  UnitPrice = orderdetail.UnitPrice,
									  OrderDate = order.CreatedDate
								  }).Where(x => x.UserId == userId).ToList();


				ViewBag.ProductReviewsFromUser = reviewList;
				ViewBag.ProductOrder = orderList;
				return View(user);
			}
			ViewBag.UserId = Id;
			ViewBag.SystemUserId = userId;
			var userWithId = await _userManager.FindByIdAsync(Id);
			ViewBag.CommentCount = c.ProductReviews.Where(x => x.UserId == Id).Count();
			var reviewListUserWithId = (from review in reviews
							  join product in products on review.ProductId equals product.Id
							  select new ProductReviewDto
							  {
								  ReviewId = review.Id,
								  ProductId = review.ProductId,
								  Comment = review.Comment,
								  UserId = userWithId.Id,
								  UserName = userWithId.FirstName + " " + userWithId.LastName,
								  Date = review.CreatedDate,
								  ProductName = product.Name + " " + product.Description
							  }).Where(x => x.UserId == Id).ToList();
			var orderListUser = (from orderdetail in orderDetails
								 join order in orders on orderdetail.OrderId equals order.Id
								 join product in products on orderdetail.ProductId equals product.Id
								 select new UserOrderDto
								 {
									 UserId = order.UserId,
									 OrderId = orderdetail.Id,
									 ProductId = orderdetail.ProductId,
									 ProductName = product.Name,
									 Quantity = orderdetail.Quantity,
									 UnitPrice = orderdetail.UnitPrice,
									 OrderDate = order.CreatedDate
								 }).Where(x => x.UserId == userId).ToList();
			ViewBag.ProductReviewsFromUser = reviewListUserWithId;
			ViewBag.ProductOrder = orderListUser;
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
			HttpContext.Session.SetString("CurrentTab", "UpdateProfile");
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
				return RedirectToAction("UserDetail", "User");
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

		public async Task<IActionResult> UserProductReview(string Id)
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var reviews = c.ProductReviews.Where(x => x.UserId == userId).ToList();
			var products = c.Products.ToList();
			if (Id == null)
			{
				var user = await _userManager.FindByIdAsync(userId);
				ViewBag.CommentCount = c.ProductReviews.Where(x => x.UserId == userId).Count();
				
				var reviewList = (from review in reviews
								  join product in products on review.ProductId equals product.Id
								  select new ProductReviewDto
								  {
									  ReviewId = review.Id,
									  ProductId = review.ProductId,
									  Comment = review.Comment,
									  UserId = user.Id,
									  UserName = user.FirstName + " " + user.LastName,
									  Date = review.CreatedDate,
									  ProductName = product.Name +" "+product.Description
								  }).ToList();

				ViewBag.ProductReviewsFromUser = reviewList;
				return View(user);
			}
			var userWithId = await _userManager.FindByIdAsync(Id);
			var reviewListForAnotheruser = (from review in reviews
							  join product in products on review.ProductId equals product.Id
							  select new ProductReviewDto
							  {
								  ReviewId = review.Id,
								  ProductId = review.ProductId,
								  Comment = review.Comment,
								  UserId = userWithId.Id,
								  UserName = userWithId.FirstName + " " + userWithId.LastName,
								  Date = review.CreatedDate,
								  ProductName = product.Name+" "+product.Description
							  }).ToList();

			ViewBag.ProductReviewsFromUser = reviewListForAnotheruser;
			
			ViewBag.CommentCount = c.ProductReviews.Where(x => x.UserId == Id).Count();
			return View(userWithId);

		}
	}
}
