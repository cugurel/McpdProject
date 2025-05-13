using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
	public interface IProductDal
	{
		void Add(Product product);
		void Update(Product product);
		void Delete(Product product);
		Product GetById(int Id);
		List<Product> GetAll();
	}
}
