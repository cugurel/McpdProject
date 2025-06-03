using Microsoft.AspNetCore.Identity;

namespace UI.Models.Identity
{
	public class User:IdentityUser
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
