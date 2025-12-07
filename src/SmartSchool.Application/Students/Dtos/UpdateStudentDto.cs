namespace SmartSchool.Application.Students.Dtos
{
    public record UpdateStudentDto(
     string FullName,
     int Grade,
     DateTime DOB,
     string? Nationality,
     string? NationalId
 );
}
