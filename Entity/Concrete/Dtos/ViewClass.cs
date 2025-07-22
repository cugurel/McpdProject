using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete.Dtos
{
	public class ViewClass
	{
		public List<Category> Categories { get; set; }
		public List<ProductWithCategory> Products { get; set; }
	}
}
