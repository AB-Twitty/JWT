using JWT.Auth.Models;
using JWT.Auth.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JWT.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AccountController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("login")]
		public async Task<AuthResponse> Login(AuthRequest request) =>
			await _authService.Login(request);

		[HttpPost("refresh")]
		public async Task<AuthResponse> Refresh(RefreshTokensRequest request) =>
			await _authService.RefreshTokens(request);
	}
}
