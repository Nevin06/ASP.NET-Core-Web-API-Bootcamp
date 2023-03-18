using Courseproject.Common.Dtos.Address;
using Courseproject.Common.Dtos.Job;
using Courseproject.Common.Dtos.Team;

namespace Courseproject.Common.Dtos.Employee;
//67
public record EmployeeDetails(int Id, string FirstName, string LastName, AddressGet Address, JobGet Job, List<TeamGet> Teams, string? ProfilePhotoPath); //100
