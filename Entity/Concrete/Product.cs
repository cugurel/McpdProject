using Entity.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Concrete
{
	public class Product:IEntity
	{
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? DetailDesc { get; set; }
        public int? QuantityInStock { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
