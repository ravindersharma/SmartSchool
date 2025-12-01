namespace SmartSchool.Domain.Entities;

public class Student: BaseEntity
{
    public string FullName { get; set; } =default!;
    public DateTime DOB { get; set; }
    public int Grade { get; set; }

}
