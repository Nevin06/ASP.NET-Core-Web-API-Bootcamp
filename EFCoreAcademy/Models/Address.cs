namespace EFCoreAcademy.Models
{
    //Inherits primary key from base entity
    public class Address : BaseEntity
    {
        public string City { get; set; } = default!;
        public string Zip { get; set; } = default!;
        public string Street { get; set; } = default!;
        public int HouseNumber { get; set; }

    }
}