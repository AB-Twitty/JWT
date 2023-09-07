﻿using JWT.Auth.Models;
using JWT.Auth.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace JWT.Auth.Services.Implementation
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<AuthResponse> Login(AuthRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);

			if (user == null)
				throw new Exception("Invalid Email or Password");

			var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

			if (!result.Succeeded)
				throw new Exception("Invalid Email or Password");

			var response = new AuthResponse
			{
				Id = user.Id,
				Email = user.Email,
				Username = user.UserName,
				Token = null
			};

			return response;
		}
	}
}