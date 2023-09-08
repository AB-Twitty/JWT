using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWT.Auth.Data
{
	public class UserRefreshToken
	{
        [Key]
        public int Id { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpired { get; set; }
        public bool IsInvalidated { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; }
    }
}
