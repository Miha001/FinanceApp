namespace Finances.Domain.Models.Dto;

public class UserDto
{
    public string Name { get; set; }
    public string Password { get; set; }
    public  string ConfirmPassword { get; set; }
}