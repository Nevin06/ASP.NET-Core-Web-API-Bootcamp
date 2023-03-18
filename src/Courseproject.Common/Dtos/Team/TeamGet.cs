using Courseproject.Common.Dtos.Employee;

namespace Courseproject.Common.Dtos.Team;
//69
//EmployeeList is the Dto for returning employees in list view, we want names of employees only, not jobs
public record TeamGet(string Id, string Name, List<EmployeeList> Employees);