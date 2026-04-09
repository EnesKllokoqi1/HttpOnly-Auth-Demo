namespace CookieAPI.DTOs
{
    public class TokenResponseDTO
    {
        public required string AcessToken { get; set; } = string.Empty;
        public required string RefreshToken { get; set; } = string.Empty;
    }
}
