using System;

namespace SmartSchool.Domain.Entities;

public class Student
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }=string.Empty;
    public string LastName { get; set; }=string.Empty;
    public DateTime DOB { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public int Grade { get; set; }
}
