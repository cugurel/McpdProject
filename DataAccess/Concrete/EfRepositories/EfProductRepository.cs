using DataAccess.Abstract;
using Entity.Concrete;

namespace DataAccess.Concrete.EfRepositories
{
	public class EfProductRepository : IProductDal
	{
		public void Add(Product product)
		{
			using var c = new Context();
			c.Products.Add(product);
			c.SaveChanges();
		}

		public void Delete(Product product)
		{
			using var c = new Context();
			c.Products.Remove(product);
			c.SaveChanges();
		}

		public List<Product> GetAll()
		{
			using var c = new Context();
			return c.Products.ToList();
		}

		public Product GetById(int Id)
		{
			using var c = new Context();
			return c.Products.Find(Id);
		}

		public void Update(Product product)
		{
			using var c = new Context();
			c.Products.Update(product);
			c.SaveChanges();
		}
	}
}
