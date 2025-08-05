using Entity.Abstract;
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
		public DateTime CreatedDate { get; set; }
	}
}
