using Microsoft.AspNetCore.Identity;

namespace UI.Models.Identity
{
	public class RoleDetails
	{
		public IdentityRole Role { get; set; }
		public IEnumerable<User> Members { get; set; }
		public IEnumerable<User> NonMembers { get; set; }
	}
}
