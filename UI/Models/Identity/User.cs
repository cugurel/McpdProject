using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models.Identity
{
	public class User:IdentityUser
	{
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
