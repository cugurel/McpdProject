using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete.Dtos
{
	public class UserOrderDto
	{
		public int OrderId { get; set; }
		public string UserId { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public int UnitPrice { get; set; }
		public int TotalPrice { get; set; }
		public DateTime OrderDate { get; set; }
	}
}
