using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JWT.Auth.Configuations
{
	public class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
	{
		public void Configure(EntityTypeBuilder<IdentityUser> builder)
		{
			var hasher = new PasswordHasher<IdentityUser>();

			builder.HasData(
				new IdentityUser
				{
					UserName = "Admin",
					Email = "admin@localhost.com",
					PasswordHash = hasher.HashPassword(null, "P@ssword1")
				});
		}
	}
}
