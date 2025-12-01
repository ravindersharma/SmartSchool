namespace SmartSchool.Application.Auth.Dtos;

public record RegisterRequestDto(string Email, string UserName, string Password, string Role = "User");
