using JWT.Auth.Models;
using JWT.Auth.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Auth.Services.Implementation
{
	public class JwtTokenService : IJwtTokenService
	{
		private readonly JwtSettings _jwtSettings;
		private readonly UserManager<IdentityUser> _userManager;

		public JwtTokenService(IOptions<JwtSettings> options, UserManager<IdentityUser> userManager)
		{
			_jwtSettings = options.Value;
			_userManager = userManager;
		}

		public async Task<string> GenerateToken(IdentityUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			
			var roles = await _userManager.GetRolesAsync(user);

			var rolesClaims = new List<Claim>();

			foreach (var role in roles)
				rolesClaims.Add(new Claim(ClaimTypes.Role, role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("uid", user.Id)
			}.Union(userClaims).Union(rolesClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

			var signingCredintials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var JwtSecurityToken = new JwtSecurityToken(
				signingCredentials: signingCredintials,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
				claims: claims
			);

			return new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);
        }
	}
}
