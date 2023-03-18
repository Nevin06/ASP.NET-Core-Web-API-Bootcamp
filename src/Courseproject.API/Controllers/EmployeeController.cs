using Courseproject.Common.Dtos.Employee;
using Courseproject.Common.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Serilog.Context;
//67

namespace Courseproject.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private IEmployeeService EmployeeService { get; }
    private ILogger<EmployeeController> Logger { get; }


    public EmployeeController(IEmployeeService employeeService,
        ILogger<EmployeeController> logger) //103
	{
        EmployeeService = employeeService;
        Logger = logger;
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateEmployee(EmployeeCreate employeeCreate)
    {
        var id = await EmployeeService.CreateEmployeeAsync(employeeCreate);
        return Ok(id);
    }

    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> UpdateEmployee(EmployeeUpdate employeeUpdate)
    {
        await EmployeeService.UpdateEmployeeAsync(employeeUpdate);
        return Ok();
    }

    [HttpDelete]
    [Route("Delete")]
    public async Task<IActionResult> DeleteEmployee(EmployeeDelete employeeDelete)
    {
        await EmployeeService.DeleteEmployeeAsync(employeeDelete);
        return Ok();
    }

    [HttpGet]
    [Route("Get/{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        //103
        //Logger.LogInformation("GetEmployee was called with id {id}", id);
        using (LogContext.PushProperty("Employee Id", id))
        {
            var employee = await EmployeeService.GetEmployeeAsync(id);
            return Ok(employee);
        }
    }

    [HttpGet]
    [Route("Get")]
    //68
    public async Task<IActionResult> GetEmployees([FromQuery]EmployeeFilter employeeFilter)
    {
        var employees = await EmployeeService.GetEmployeeesAsync(employeeFilter);
        return Ok(employees);
    }

    [HttpPut]
    [Route("Update/Profilephoto")]
    // 100
    // we need to get profilePhotoUpdate from a form because we need form data to upload these files in HTTP
    public async Task<IActionResult> UpdateProfilePhoto([FromForm]ProfilePhotoUpdate profilePhotoUpdate)
    {
        await EmployeeService.UpdateProfilePhotoAsync(profilePhotoUpdate);
        return Ok();
    }
}
