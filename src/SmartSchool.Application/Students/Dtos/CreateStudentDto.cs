namespace SmartSchool.Application.Students.Dtos
{
    public record CreateStudentDto(
     Guid UserId,
     string FullName,
     int Grade,
     DateTime DOB,
     string? Nationality,
     string? NationalId
 );
}
