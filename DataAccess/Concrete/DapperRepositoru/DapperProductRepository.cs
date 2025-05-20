using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.DapperRepositoru
{
	public class DapperProductRepository : IProductDal
	{
		public void Add(Product t)
		{
			throw new NotImplementedException();
		}

		public void Delete(Product t)
		{
			throw new NotImplementedException();
		}

		public List<Product> GetAll()
		{
			throw new NotImplementedException();
		}

		public Product GetById(int Id)
		{
			throw new NotImplementedException();
		}

		public List<ProductWithCategory> ListProductView()
		{
			throw new NotImplementedException();
		}

		public void Update(Product t)
		{
			throw new NotImplementedException();
		}
	}
}
