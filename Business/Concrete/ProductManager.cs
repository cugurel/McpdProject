using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Concrete.Dtos;

namespace Business.Concrete
{
	public class ProductManager : IProductService
	{
		IProductDal _productDal;

		public ProductManager(IProductDal productDal)
		{
			_productDal = productDal;
		}

		public void Add(Product product)
		{
			_productDal.Add(product);
		}

		public void Delete(Product product)
		{
			_productDal.Delete(product);
		}

		public List<ProductWithCategory> GetAll()
		{
			return _productDal.ListProductView();
		}

		public Product GetById(int Id)
		{
			return _productDal.GetById(Id);
		}

		public void Update(Product product)
		{
			_productDal.Update(product);
		}
	}
}
