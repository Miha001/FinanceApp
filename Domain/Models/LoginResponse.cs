namespace Domain.Models;
public record LoginResponse
{
    public required string JwtToken { get; set; }
    public required string RefreshToken { get; set; }
}