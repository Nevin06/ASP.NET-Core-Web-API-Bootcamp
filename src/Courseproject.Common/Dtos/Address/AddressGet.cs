namespace Courseproject.Common.Dtos.Address;
//64
//return Street, Zip, City, Email, Phone
public record AddressGet(string Id, string Street, string Zip, string City, string Email, string? Phone);