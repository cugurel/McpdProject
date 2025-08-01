﻿using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
	public class Order:IEntity
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public int Quantity { get; set; }
		public int ProductId { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
