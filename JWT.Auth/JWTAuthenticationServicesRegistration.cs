﻿using JWT.Auth.Context;
using JWT.Auth.Models;
using JWT.Auth.Services.Contracts;
using JWT.Auth.Services.Implementation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWT.Auth
{
	public static class JWTAuthenticationServicesRegistration
	{
		public static IServiceCollection ConfigureJWTAuthenticationServices (this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpContextAccessor();

			services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

			services.AddDbContext<AppIdentityDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"));
			});

			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<AppIdentityDbContext>();

			services.AddTransient<IAuthService, AuthService>();

			services.AddScoped<IJwtTokenService, JwtTokenService>();

			services.AddTransient<IClaimService, ClaimService>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
			{
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
					ValidateAudience = false,
					ValidateIssuer = false
				};

				opt.Events = new JwtBearerEvents();
				opt.Events.OnTokenValidated = async (context) =>
				{
					var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
					var jwtService = context.Request.HttpContext.RequestServices.GetService<IJwtTokenService>();
					if (await jwtService.ValidateAccessToken((context.SecurityToken as JwtSecurityToken).RawData, ipAddress) == null)
						context.Fail("Invalid Token Details.");
				};
			});

			return services;
		} 
	}
}
