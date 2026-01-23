namespace Finances.Domain.Models.Dto;

/// <summary>
/// Модель данных для отображения токена.
/// </summary>
public record TokenDto
{
    public string JWT { get; set; }
}