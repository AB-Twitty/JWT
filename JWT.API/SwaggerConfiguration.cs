﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace JWT.API
{
	public static class SwaggerConfiguration
	{
		public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT", Version = "v1" });


				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = @"JWT Authorization header using the bearer scheme. \r\n\r\n
						Enter 'Bearer' [Space] and then your token in the text input below. \r\n\r\n
						Example : Bearer 12345abcdef",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{ 
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
							Scheme = "oauth2",
							Name = "Bearer",
							In = ParameterLocation.Header
						}, 
						new List<string>() 
					}
				});
			});


			return services;
		}
	}
}
