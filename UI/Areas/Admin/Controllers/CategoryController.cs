using Business.Concrete;
using DataAccess.Concrete.EfRepositories;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		CategoryManager manager = new CategoryManager(new EfCategoryRepository());

		string apiUrl = "https://localhost:7212/api/Category/";
		HttpClient client = new HttpClient();
		public async Task<IActionResult> Index()
		{
			var result = await client.GetAsync(apiUrl + "GetAllCategory");
			var jsonString = result.Content.ReadAsStringAsync().Result;
			var categories = JsonConvert.DeserializeObject<List<Category>>(jsonString);
			return View(categories);
		}
		//https://localhost:7212/api/Category/DeleteCategory?Id=12
		public async Task<IActionResult> DeleteCategory(int Id)
		{
			var result = await client.DeleteAsync(apiUrl + "DeleteCategory?Id=" + Id);
			if (result.IsSuccessStatusCode)
			{
				return RedirectToAction("Index", "Category", new {area="Admin" });
			}
			else
			{
				return View();
			}

		}

		[HttpGet]
		public IActionResult AddCategory()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddCategory(Category category)
		{
			var jsonCategory = JsonConvert.SerializeObject(category);
			StringContent content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
			var result = await client.PostAsync(apiUrl + "AddCategory", content);
			if (result.IsSuccessStatusCode)
			{
				return RedirectToAction("Index", "Category",new {area = "Admin" });
			}
			else
			{
				return View();
			}
		}

		[HttpGet]
		public async Task<IActionResult> UpdateCategory(int Id)
		{
			var result = await client.GetAsync(apiUrl + "GetById?Id=" + Id);
			if (result.IsSuccessStatusCode)
			{
				var jsonCategory = await result.Content.ReadAsStringAsync();
				var category = JsonConvert.DeserializeObject<Category>(jsonCategory);
				return View(category);
			}
			else
			{
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateCategory(Category category)
		{
			var jsonCategory = JsonConvert.SerializeObject(category);
			StringContent content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
			var result = await client.PutAsync(apiUrl + "UpdateCategory", content);
			if (result.IsSuccessStatusCode)
			{
				return RedirectToAction("Index", "Category");
			}
			else
			{
				return View();
			}
		}
	}
}
