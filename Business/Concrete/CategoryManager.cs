﻿using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class CategoryManager : ICategoryService
	{
		ICategoryDal _categoryDal;

		public CategoryManager(ICategoryDal categoryDal)
		{
			_categoryDal = categoryDal;
		}

		public void Add(Category category)
		{
			_categoryDal.Add(category);
		}

		public void Delete(Category category)
		{
			_categoryDal.Delete(category);
		}

		public List<Category> GetAll()
		{
			return _categoryDal.GetAll();
		}

		public Category GetById(int Id)
		{
			return _categoryDal.GetById(Id);
		}

		public void Update(Category category)
		{
			_categoryDal.Update(category);
		}
	}
}
