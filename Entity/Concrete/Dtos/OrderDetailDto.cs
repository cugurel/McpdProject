using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete.Dtos
{
	public class OrderDetailDto
	{
		public string ProductName { get; set; }
		public int Price { get; set; }
		public int Quantity { get; set; }
		public int TotalPrice { get; set; }
	}
}
