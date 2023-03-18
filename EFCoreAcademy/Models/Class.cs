namespace EFCoreAcademy.Models
{
    // Class has 1 professor, professor has list of classes, 1 to many relationship
    public class Class : BaseEntity
    {
        public string Title { get; set; } = default!;
        public List<Student> Students { get; set; } = default!;
        public Professor Professor { get; set; } = default!;

    }
}