﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete.Dtos
{
	public class ProductWithCategory
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public int? QuantityInStock { get; set; }
		public int? Price { get; set; }
		public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? ImagePath { get; set; }
    }
}
