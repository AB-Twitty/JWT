using JWT.Auth.Models;
using JWT.Auth.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Auth.Services.Implementation
{
	public class JwtTokenService : IJwtTokenService
	{
		private readonly JwtSettings _jwtSettings;
		private readonly IClaimService _claimService;

		public JwtTokenService(IOptions<JwtSettings> options, IClaimService claimService)
		{
			_jwtSettings = options.Value;
			_claimService = claimService;
		}

		public async Task<string> GenerateToken(IdentityUser user)
		{
			var claims = await _claimService.GetClaims(user);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

			var signingCredintials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var JwtSecurityToken = new JwtSecurityToken(
				signingCredentials: signingCredintials,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
				claims: claims
			);

			return new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);
        }

		public Task<string> GenerateRefreshToken()
		{
			var byteArray = new byte[64];
			using(var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(byteArray);
				return Task.FromResult(Convert.ToBase64String(byteArray));
			}
		}
	}
}
