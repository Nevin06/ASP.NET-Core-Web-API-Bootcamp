using Courseproject.Common.Dtos.Employee;

namespace Courseproject.Common.Interfaces;
//67
public interface IEmployeeService
{
    Task<int> CreateEmployeeAsync(EmployeeCreate employeeCreate);
    Task UpdateEmployeeAsync(EmployeeUpdate employeeUpdate);
    Task DeleteEmployeeAsync(EmployeeDelete employeeDelete);
    Task<EmployeeDetails> GetEmployeeAsync(int id);
    Task<List<EmployeeList>> GetEmployeeesAsync(EmployeeFilter employeeFilter);
    //99
    Task UpdateProfilePhotoAsync(ProfilePhotoUpdate profilePhotoUpdate);
}
