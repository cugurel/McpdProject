using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
	public class Campaign
	{
		public int Id { get; set; }
		public string ImagePath { get; set; }

		[NotMapped]
		public IFormFile File { get; set; }
	}
}
