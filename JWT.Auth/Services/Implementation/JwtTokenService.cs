using JWT.Auth.Context;
using JWT.Auth.Data;
using JWT.Auth.Models;
using JWT.Auth.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Auth.Services.Implementation
{
	public class JwtTokenService : IJwtTokenService
	{
		private readonly JwtSettings _jwtSettings;
		private readonly IClaimService _claimService;
		private readonly AppIdentityDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		

		public JwtTokenService(IOptions<JwtSettings> options, IClaimService claimService, AppIdentityDbContext context, UserManager<IdentityUser> userManager)
		{
			_jwtSettings = options.Value;
			_claimService = claimService;
			_context = context;
			_userManager = userManager;
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

		public async Task SaveRefreshToken(string token, string refreshToken, string userId)
		{
			await _context.UserRefreshTokens.AddAsync(new UserRefreshToken
			{
				RefreshToken = refreshToken,
				Token = token,
				UserId = userId,
				DateCreated = DateTime.UtcNow,
				DateExpired = DateTime.UtcNow.AddMinutes(15),
				IsInvalidated = false
			});

			await _context.SaveChangesAsync();
		}

		public async Task<AuthResponse> RefreshUserTokens(RefreshTokensRequest request)
		{
			var expiredAccessToken = ValidateAccessToken(request.AccessToken);

			var expiredRefreshToken = await ValidateRefreshToken(request.RefreshToken, request.AccessToken);

			var userId = expiredAccessToken.Claims.FirstOrDefault(c => c.Type == "uid").Value.ToString();

			var user = await _userManager.FindByIdAsync(userId);

			var newAccessToken = await GenerateToken(user);

			var newRefreshToken = await GenerateRefreshToken();

			await SaveRefreshToken(newAccessToken, newRefreshToken, userId);
			
			expiredRefreshToken.IsInvalidated = true;

			_context.UserRefreshTokens.Update(expiredRefreshToken);

			await _context.SaveChangesAsync();

			return new AuthResponse
			{
				Id = user.Id,
				Email = user.Email,
				Username = user.UserName,
				Token = newAccessToken,
				RefreshToken = newRefreshToken
			};
		}

		private JwtSecurityToken ValidateAccessToken(string accessToken)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero,
				ValidateAudience = false,
				ValidateIssuer = false
			};

			new JwtSecurityTokenHandler().ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

			var jwtSecurityToken = securityToken as JwtSecurityToken;

			if (jwtSecurityToken == null)
				throw new SecurityTokenException("Access Token Invalid.");

			/*if (jwtSecurityToken.ValidTo > DateTime.UtcNow)
				throw new SecurityTokenException("Access Token Not Expired.");*/

			return jwtSecurityToken;
		}

		private async Task<UserRefreshToken> ValidateRefreshToken(string refreshToken, string accessToken)
		{
			var RefreshToken = await _context.UserRefreshTokens.Where(t => t.RefreshToken == refreshToken
				&& t.Token == accessToken && !t.IsInvalidated).FirstOrDefaultAsync();

			if (RefreshToken == null)
				throw new SecurityTokenException("Refresh Token Invalid.");

			if (RefreshToken.DateExpired < DateTime.UtcNow)
				throw new SecurityTokenException("Refresh Token is expired");
			
			return RefreshToken;
		}

	}
}
