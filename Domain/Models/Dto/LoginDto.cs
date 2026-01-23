namespace Finances.Domain.Models.Dto;
public record LoginDto
{
    public required string JwtToken { get; set; }
    public required string RefreshToken { get; set; }
}