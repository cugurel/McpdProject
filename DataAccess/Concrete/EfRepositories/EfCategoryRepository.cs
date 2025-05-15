using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EfRepositories
{
	public class EfCategoryRepository : ICategoryDal
	{
		public void Add(Category category)
		{
			using var c = new Context();
			c.Categories.Add(category);
			c.SaveChanges();
		}

		public void Delete(Category category)
		{
			using var c = new Context();
			c.Categories.Remove(category);
			c.SaveChanges();
		}

		public List<Category> GetAll()
		{
			using var c = new Context();
			return c.Categories.ToList();
		}

		public Category GetById(int Id)
		{
			using var c = new Context();
			return c.Categories.Find(Id);
		}

		public void Update(Category category)
		{
			using var c = new Context();
			c.Categories.Update(category);
			c.SaveChanges();
		}
	}
}
