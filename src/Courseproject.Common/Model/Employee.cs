namespace Courseproject.Common.Model;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public Address Address { get; set; } = default!;
    public Job Job { get; set; } = default!;
    public List<Team> Teams { get; set; } = default!;
    //97
    public string? ProfilePhotoPath { get; set; }
}
