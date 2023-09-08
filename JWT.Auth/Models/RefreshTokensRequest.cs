namespace JWT.Auth.Models
{
	public class RefreshTokensRequest
	{
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
