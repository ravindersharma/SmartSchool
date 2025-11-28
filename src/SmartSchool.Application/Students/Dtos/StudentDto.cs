namespace SmartSchool.Application.Students.Dtos;

public record StudentDto(Guid Id, string FirstName, string LastName, DateTime DOB, int Grade);