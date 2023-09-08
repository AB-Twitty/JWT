using JWT.Auth.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JWT.Auth.Context
{
	public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
	{
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
            
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			//builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

        public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    }
}
