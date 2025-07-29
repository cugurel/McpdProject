using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
	public class Basket
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public int Price { get; set; }
		public int TotalPrice { get; set; }
		public string UserId { get; set; }
		public bool Status { get; set; }
	}
}
