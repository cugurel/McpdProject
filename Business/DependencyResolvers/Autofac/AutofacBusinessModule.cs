using Autofac;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EfRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
	public class AutofacBusinessModule:Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
			builder.RegisterType<DapperProductRepository>().As<IProductDal>().SingleInstance();

			builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
			builder.RegisterType<EfCategoryRepository>().As<ICategoryDal>().SingleInstance();
		}
	}
}
