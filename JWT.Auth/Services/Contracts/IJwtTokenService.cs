using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace JWT.Auth.Services.Contracts
{
	public interface IJwtTokenService
	{
		Task<string> GenerateToken(IdentityUser user);
	}
}
