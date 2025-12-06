namespace SmartSchool.Application.Users.Dtos
{
    public record UpdateUserDto(string UserName, string Role, string? Nationality, string? NationalId);
}
