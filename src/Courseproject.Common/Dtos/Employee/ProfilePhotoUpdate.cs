using Microsoft.AspNetCore.Http;

namespace Courseproject.Common.Dtos.Employee;

//97
public record ProfilePhotoUpdate(int EmployeeId, IFormFile Photo);
