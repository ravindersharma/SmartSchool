namespace SmartSchool.Application.Students.Dtos;

public record StudentDto(
    Guid Id,
    Guid UserId,
    string Email,
    string UserName,
    string Role,
    string FullName,
    int Grade,
    DateTime DOB,
    string? Nationality,
    string? NationalId
);