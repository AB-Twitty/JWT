using JWT.Auth.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace JWT.Auth.Services.Contracts
{
	public interface IJwtTokenService
	{
		Task<string> GenerateToken(IdentityUser user);

		Task<string> GenerateRefreshToken();

		Task SaveRefreshToken(string token, string refreshToken, string userId);

		Task<AuthResponse> RefreshUserTokens(RefreshTokensRequest request);

		Task<JwtSecurityToken> ValidateAccessToken(string accessToken, string ipAddress);
	}
}
