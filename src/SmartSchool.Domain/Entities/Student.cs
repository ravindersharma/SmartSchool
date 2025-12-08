namespace SmartSchool.Domain.Entities;

public class Student : BaseEntity
{

    public string FullName { get; set; } = default!;
    public DateTime DOB { get; set; }
    public int Grade { get; set; }

    //Navigation property
    public Guid UserId { get; set; }
    public User? User { get; set; }

    // add national properties
    public string? Nationality { get; set; }
    public string? NationalId { get; set; }

}
