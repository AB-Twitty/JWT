﻿using System.ComponentModel.DataAnnotations;

namespace JWT.Auth.Models
{
	public class AuthRequest
	{
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
