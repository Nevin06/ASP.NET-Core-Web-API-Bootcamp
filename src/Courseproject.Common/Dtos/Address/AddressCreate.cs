using Courseproject.Common.Model;

namespace Courseproject.Common.Dtos.Address;
//64

// Don't need to send address of employees, done by EmployeeController
//(when we create employee, we are going to assign the address there)
public record AddressCreate(string Street, string Zip, string City, string Email, string? Phone);
