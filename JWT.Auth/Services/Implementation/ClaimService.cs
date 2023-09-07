using JWT.Auth.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWT.Auth.Services.Implementation
{
	public class ClaimService : IClaimService
	{
		private readonly UserManager<IdentityUser> _userManager;

		public ClaimService(UserManager<IdentityUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IEnumerable<Claim>> GetClaims(IdentityUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);

			var roles = await _userManager.GetRolesAsync(user);

			var rolesClaims = new List<Claim>();

			foreach (var role in roles)
				rolesClaims.Add(new Claim(ClaimTypes.Role, role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("uid", user.Id)
			}.Union(userClaims).Union(rolesClaims);

			return claims;
		}
	}
}
