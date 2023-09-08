using JWT.Auth.Models;
using JWT.Auth.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace JWT.Auth.Services.Implementation
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IJwtTokenService _jwtTokenService;

		public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtTokenService jwtTokenService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtTokenService = jwtTokenService;
		}

		public async Task<AuthResponse> Login(AuthRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);

			if (user == null)
				throw new Exception("Invalid Email or Password");

			var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

			if (!result.Succeeded)
				throw new Exception("Invalid Email or Password");

			var token = await _jwtTokenService.GenerateToken(user);

			var refreshToken = await _jwtTokenService.GenerateRefreshToken();

			await _jwtTokenService.SaveRefreshToken(token, refreshToken, user.Id);

			var response = new AuthResponse
			{
				Id = user.Id,
				Email = user.Email,
				Username = user.UserName,
				Token = token,
				RefreshToken = refreshToken
			};

			return response;
		}

		public async Task<AuthResponse> RefreshTokens (RefreshTokensRequest request)
		{
			return await _jwtTokenService.RefreshUserTokens(request);
		}
	}
}
