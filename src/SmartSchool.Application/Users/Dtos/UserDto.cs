namespace SmartSchool.Application.Users.Dtos
{
    public record UserDto(Guid Id,
        string Email,
        string UserName,
        string Role,
        bool IsDeleted
    );
}
