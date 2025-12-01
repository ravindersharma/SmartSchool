namespace SmartSchool.Application.Auth.Dtos;

public record ResetPasswordRequestDto(string Token, string NewPassword);
