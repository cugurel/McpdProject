using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete.Dtos
{
	public class ProductReviewDto
	{
		public int ReviewId { get; set; }
		public int ProductId { get; set; }
		public string Comment { get; set; }

		public string UserId { get; set; }
		public string UserName { get; set; }
		public DateTime Date { get; set; }
	}
}
