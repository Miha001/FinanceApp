namespace Finances.Domain.Models.Dto;

public record RegisteredUserDto(string Name, string Password, string ConfirmPassword);