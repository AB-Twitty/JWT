using JWT.Auth.Context;
using JWT.Auth.Models;
using JWT.Auth.Services.Contracts;
using JWT.Auth.Services.Implementation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JWT.Auth
{
	public static class JWTAuthenticationServicesRegistration
	{
		public static IServiceCollection ConfigureJWTAuthenticationServices (this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

			services.AddDbContext<AppIdentityDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"));
			});

			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<AppIdentityDbContext>();

			services.AddTransient<IAuthService, AuthService>();

			services.AddAuthentication();

			return services;
		} 
	}
}
