namespace SmartSchool.Application.Students.Dtos;

public record StudentDto(Guid Id, string FullName, DateTime DOB, int Grade,DateTime CreatedAt);