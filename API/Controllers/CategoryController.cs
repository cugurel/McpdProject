using Business.Concrete;
using DataAccess.Concrete.EfRepositories;
using Entity.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		CategoryManager manager = new CategoryManager(new EfCategoryRepository()); 

		[HttpGet("GetAllCategory")]
		public IActionResult Get()
		{
			var list = manager.GetAll();
			if(list == null)
			{
				return BadRequest("Kategoriler bulunamadı");
			}
			else
			{
				return Ok(list);
			}
		}

		[HttpGet("GetById")]
		public IActionResult GetById(int Id)
		{
			var list = manager.GetById(Id);
			if (list == null)
			{
				return BadRequest("Kategoriler bulunamadı");
			}
			else
			{
				return Ok(list);
			}
		}

		[HttpDelete("DeleteCategory")]
		public IActionResult DeleteCategory(int Id)
		{
			var category = manager.GetById(Id);
			if(category == null)
			{
				return BadRequest("Veri bulunaması");
			}
			else
			{
				manager.Delete(category);
				return Ok("Veri silindi");
			}
		}


		[HttpPost("AddCategory")]
		public IActionResult AddCategory(Category category)
		{
			manager.Add(category);
			return Ok(category);
		}

		[HttpPut("UpdateCategory")]
		public IActionResult UpdateCategory(Category category)
		{
			manager.Update(category);
			return Ok(category);
		}
	}
}
