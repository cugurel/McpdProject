using Business.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Models.Identity;

namespace UI.Controllers
{
	public class ProductController : Controller
	{
		IProductService _productService;
		UserManager<User> _userManager;
		SignInManager<User> _signInManager;
		public ProductController(IProductService productService, UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_productService = productService;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		Context c= new Context();

		[Authorize]
		public IActionResult Index()
		{
			var value = _productService.GetAll();
			return View(value);
		}

		public IActionResult DeleteProduct(int Id)
		{
			var value = _productService.GetById(Id);
			_productService.Delete(value);
			return RedirectToAction("Index", "Product");
		}

		[HttpGet]
		public IActionResult AddProduct()
		{
			ViewBag.Categories = c.Categories.Select(u => new SelectListItem
			{
				Value = u.Id.ToString(),
				Text = u.Name
			}).ToList();

			return View();
		}

		[HttpGet]
		public IActionResult UpdateProductDescription()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> AddProduct(Product product)
		{
			ViewBag.Categories = c.Categories.Select(u => new SelectListItem
			{
				Value = u.Id.ToString(),
				Text = u.Name
			}).ToList();


			if(product.File != null)
			{
				var item = product.File;
				var extend = Path.GetExtension(item.FileName);
				var randomName = ($"{Guid.NewGuid()}{extend}");
				var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\ProductImages", randomName);

				using (var stream = new FileStream(path,FileMode.Create))
				{
					await item.CopyToAsync(stream);
				}

				product.ImagePath = randomName;
			}

			product.CreatedDate = DateTime.Now;
			product.IsActive = true;
			_productService.Add(product);
			return RedirectToAction("Index", "Product");
		}


		[HttpGet]
		public IActionResult UpdateProduct(int Id)
		{
			ViewBag.Categories = c.Categories.Select(u => new SelectListItem
			{
				Value = u.Id.ToString(),
				Text = u.Name
			}).ToList();

			var value = _productService.GetById(Id);
			return View(value);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateProduct(Product product)
		{
			ViewBag.Categories = c.Categories.Select(u => new SelectListItem
			{
				Value = u.Id.ToString(),
				Text = u.Name
			}).ToList();

			if (product.File != null)
			{
				var item = product.File;
				var extend = Path.GetExtension(item.FileName);
				var randomName = ($"{Guid.NewGuid()}{extend}");
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\ProductImages", randomName);

				if (!string.IsNullOrEmpty(product.ImagePath))
				{
					var oldPath = Path.Combine(Directory.GetCurrentDirectory(), 
						"wwwroot\\ProductImages", product.ImagePath);
					if (System.IO.File.Exists(oldPath))
					{
						System.IO.File.Delete(oldPath);
					}
				}

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await item.CopyToAsync(stream);
				}

				product.ImagePath = randomName;
				product.CreatedDate = DateTime.Now;
				product.IsActive = true;
			}
			product.CreatedDate = DateTime.Now;
			product.IsActive = true;
			_productService.Update(product);
			return RedirectToAction("Index", "Product");
		}

		public async Task<IActionResult> ProductDetail(int Id)
		{
			var value = _productService.GetById(Id);

			var reviews = c.ProductReviews.Where(x => x.ProductId == Id).ToList();
			var ratings = c.ProductRatings.Where(x => x.ProductId == Id).ToList();

			double averageRating = 0;

			if (ratings.Any())
			{
				averageRating = ratings.Average(x => x.Rating);
			}

			ViewBag.AverageRating = averageRating;
			var users =  _userManager.Users.ToList();

			var reviewList = (from review in reviews
							  join user in users on review.UserId equals user.Id
							  join rating in ratings on review.UserId equals rating.UserId
							  select new ProductReviewDto
							  {
								  ReviewId = review.Id,
								  ProductId = review.ProductId,
								  Comment = review.Comment,
								  UserId = user.Id,
								  UserName = user.FirstName + " "+user.LastName,
								  Date = review.CreatedDate,
								  Rating = rating.Rating
							  }).Where(x=>x.ProductId == Id).ToList();

			ViewBag.ProductReviews = reviewList;

			return View(value);
		}


		[HttpPost]
		public async Task<IActionResult> AddComment(ProductReview review)
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			review.UserId = userId;
			review.ProductId = review.ProductId;
			review.CreatedDate = DateTime.Now;
			c.ProductReviews.Add(review);
			c.SaveChanges();
			return RedirectToAction("ProductDetail","Product",new { Id = review.ProductId});
		}


		[HttpPost]
		public IActionResult SubmitRating([FromBody] RatingDto dto)
		{
			// Örneğin UserId'yi session'dan ya da Identity'den alabilirsiniz
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
				return Unauthorized();

			// Daha önce rating var mı kontrol
			var existingRating = c.ProductRatings
				.FirstOrDefault(x => x.ProductId == dto.ProductId && x.UserId == userId);

			if (existingRating != null)
			{
				// Update
				existingRating.Rating = dto.Rating;
				c.ProductRatings.Update(existingRating);
			}
			else
			{
				// Yeni ekle
				var rating = new ProductRating
				{
					ProductId = dto.ProductId,
					UserId = userId,
					Rating = dto.Rating
				};
				c.ProductRatings.Add(rating);
			}

			c.SaveChanges();

			return Ok(new { success = true });
		}
	}
}
