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
					NormalizedUserName = "ADMIN",
					Email = "admin@localhost.com",
					NormalizedEmail = "ADMIN@LOCALHOST.COM",
					PasswordHash = hasher.HashPassword(null, "P@ssword1")
				});
		}
	}
}
