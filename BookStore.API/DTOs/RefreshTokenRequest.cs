namespace BookStore.API.DTOs
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
    public class RevokeRefreshToken
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
