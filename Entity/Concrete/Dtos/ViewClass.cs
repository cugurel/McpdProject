using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete.Dtos
{
	public class ViewClass
	{
		public List<Campaign> Campaigns { get; set; }
		public List<ProductWithCategory> Products { get; set; }
	}
}
