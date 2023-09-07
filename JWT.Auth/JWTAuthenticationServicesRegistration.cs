using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JWT.Auth
{
	public static class JWTAuthenticationServicesRegistration
	{
		public static IServiceCollection ConfigureJWTAuthenticationServices (this IServiceCollection services, IConfiguration configuration)
		{


			return services;
		} 
	}
}
