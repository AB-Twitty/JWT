using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWT.Auth.Services.Contracts
{
	public interface IClaimService
	{
		Task<IEnumerable<Claim>> GetClaims(IdentityUser user);
	}
}
