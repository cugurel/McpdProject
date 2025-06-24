using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;

namespace Entity.Concrete
{
	public class ProductReview
	{
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
	}
}
