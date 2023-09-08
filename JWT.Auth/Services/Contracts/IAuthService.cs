using JWT.Auth.Models;
using System.Threading.Tasks;

namespace JWT.Auth.Services.Contracts
{
	public interface IAuthService
	{
		Task<AuthResponse> Login(AuthRequest request);

		Task<AuthResponse> RefreshTokens(RefreshTokensRequest request);
	}
}
