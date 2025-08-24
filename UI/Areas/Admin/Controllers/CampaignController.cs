using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CampaignController : Controller
	{
		Context context = new Context();
		public IActionResult Index()
		{
			var values = context.Campaigns.ToList();
			return View(values);
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Campaign campaign)
		{

			if (campaign.File != null)
			{
				var item = campaign.File;
				var extend = Path.GetExtension(item.FileName);
				var randomName = ($"{Guid.NewGuid()}{extend}");
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CampaignImages", randomName);

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await item.CopyToAsync(stream);
				}

				campaign.ImagePath = randomName;
			}
			context.Campaigns.Add(campaign);
			context.SaveChanges();
			return RedirectToAction("Index", "Campaign", new { area = "Admin" });
		}
	}
}
