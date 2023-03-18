namespace EFCoreAcademy.Models;

//Student has list of classes and class has list of sudents , many to many relationship
public class Student : BaseEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public List<Class> Classes { get; set; } = default!;
    public Address Address { get; set; } = default!;
}
