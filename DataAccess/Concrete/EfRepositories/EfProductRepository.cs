using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Concrete.Dtos;

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

		public List<ProductWithCategory> ListProductView()
		{
			using var c = new Context();
			var result = (from p in c.Products
						  join ct in c.Categories on p.CategoryId equals ct.Id
						  select new ProductWithCategory
						  {
							  Id = p.Id,
							  Name = p.Name,
							  Description = p.Description,
							  QuantityInStock=p.QuantityInStock,
							  Price = p.Price,
							  CategoryName = ct.Name,
							  ImagePath = p.ImagePath
						  });

			return result.ToList();
		}

		public void Update(Product product)
		{
			using var c = new Context();
			c.Products.Update(product);
			c.SaveChanges();
		}
	}
}
