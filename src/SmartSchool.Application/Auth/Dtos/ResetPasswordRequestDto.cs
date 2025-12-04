namespace SmartSchool.Application.Auth.Dtos;

public record ResetPasswordRequestDto(string Token,
    string Password,
    string ConfirmPassword);
