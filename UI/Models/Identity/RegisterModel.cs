using System.ComponentModel.DataAnnotations;

namespace UI.Models.Identity
{
	public class RegisterModel
	{
        [Required]
        public string Firstname { get; set; }
		[Required]
		public string Surname { get; set; }
		[Required]
		public string Username { get; set; }
		[Required]
		public string Phone { get; set; }
		[Required]
		public string Email { get; set; }

		[Required]
		public string Type { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
