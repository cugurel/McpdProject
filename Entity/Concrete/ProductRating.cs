using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
	public class ProductRating
	{
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string UserId { get; set; }
    }
}
