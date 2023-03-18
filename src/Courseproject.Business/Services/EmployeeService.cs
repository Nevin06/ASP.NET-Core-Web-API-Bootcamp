using AutoMapper;
using Courseproject.Business.Exceptions;
using Courseproject.Business.Validation;
using Courseproject.Common.Dtos.Address;
using Courseproject.Common.Dtos.Employee;
using Courseproject.Common.Interfaces;
using Courseproject.Common.Model;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
//67
namespace Courseproject.Business.Services;

public class EmployeeService : IEmployeeService
{
    private IMapper Mapper { get; }
    private IGenericRepository<Employee> EmployeeRepository { get; }
    private IGenericRepository<Job> JobRepository { get; }
    private IGenericRepository<Address> AddressRepository { get; }
    private EmployeeCreateValidator EmployeeCreateValidator { get; }

    private EmployeeUpdateValidator EmployeeUpdateValidator { get; }
    private IUploadService UploadService { get; }

    //private IFileService FileService { get; } //101

    private ImageFileValidator ImageFileValidator { get; }
    private ILogger<EmployeeService> Logger { get; }

    public EmployeeService(IMapper mapper, IGenericRepository<Employee> employeeRepository, 
        IGenericRepository<Job> jobRepository, IGenericRepository<Address> addressRepository,
        EmployeeCreateValidator employeeCreateValidator, EmployeeUpdateValidator employeeUpdateValidator,
        /*IFileService fileService,*/ /*101*/
        IUploadService uploadService, //101
        ImageFileValidator imageFileValidator, //99 
        ILogger<EmployeeService> logger) //103
    {
        Mapper = mapper;
        EmployeeRepository = employeeRepository;
        JobRepository = jobRepository;
        AddressRepository = addressRepository;
        EmployeeCreateValidator = employeeCreateValidator;
        EmployeeUpdateValidator = employeeUpdateValidator;
        //99
        //FileService = fileService; //101
        UploadService = uploadService; //101
        ImageFileValidator = imageFileValidator;
        Logger = logger; //103
    }


    public async Task<int> CreateEmployeeAsync(EmployeeCreate employeeCreate)
    {
        //72
        await EmployeeCreateValidator.ValidateAndThrowAsync(employeeCreate);

        //68
        //several db trips but good for error handling
        var address = await AddressRepository.GetByIdAsync(employeeCreate.AddressId);
        var job = await JobRepository.GetByIdAsync(employeeCreate.JobId);

        //76 77
         if (address == null)
            throw new AddressNotFoundException(employeeCreate.AddressId);
        if (job == null)
            throw new JobNotFoundException(employeeCreate.JobId);

        var entity = Mapper.Map<Employee>(employeeCreate);
        //68
        entity.Address = address;
        entity.Job = job;

        await EmployeeRepository.InsertAsync(entity);
        await EmployeeRepository.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteEmployeeAsync(EmployeeDelete employeeDelete)
    {
        var entity = await EmployeeRepository.GetByIdAsync(employeeDelete.Id,
            (employee) => employee.Address, (employee) => employee.Job, (employee) => employee.Teams);  //76 77
        //76 77
        if (entity == null)
            throw new EmployeeNotFoundException(employeeDelete.Id);
        //Don't want to delete employees if we have an team that has this employee
        //if (entity.Teams.Count > 0)
        //    throw new DependentTeamsExistException(entity.Teams);
        //if (entity.Address != null)
        //    throw new DependentAddressExistException(entity.Address);
        //if (entity.Job != null)
        //    throw new DependentJobExistException(entity.Job);

        EmployeeRepository.Delete(entity);
        await EmployeeRepository.SaveChangesAsync();
    }

    public async Task<EmployeeDetails> GetEmployeeAsync(int id)
    {
        //103
        Logger.LogInformation("GetEmployeeAsync called.");
        var entity = await EmployeeRepository.GetByIdAsync(id, (employee) => employee.Address, (employee) => employee.Job, (employee) => employee.Teams);
        //76 77
        if (entity == null)
            throw new EmployeeNotFoundException(id);

        return Mapper.Map<EmployeeDetails>(entity);
    }

    public async Task<List<EmployeeList>> GetEmployeeesAsync(EmployeeFilter employeeFilter)
    {
        //if FirstName == null, true as we dont want to filter with this => filter by all employees
        //leaving it true, doesn't have an effect on the query later
        // else, every employee in our db where FirstName  starts with FirstName and our filter will be found by query
        // //translated to sql and processed in db
        Expression<Func<Employee, bool>> firstNameFilter = (employee) => employeeFilter.FirstName == null ? true : 
        employee.FirstName.StartsWith(employeeFilter.FirstName);
        Expression<Func<Employee, bool>> lastNameFilter = (employee) => employeeFilter.LastName == null ? true :
        employee.LastName.StartsWith(employeeFilter.LastName);
        Expression<Func<Employee, bool>> jobFilter = (employee) => employeeFilter.Job == null ? true :
        employee.Job.Name.StartsWith(employeeFilter.Job);
        Expression<Func<Employee, bool>> teamFilter = (employee) => employeeFilter.Team == null ? true :
        employee.Teams.Any(t => t.Name.StartsWith(employeeFilter.Team));

        var entities = await EmployeeRepository.GetFilteredAsync(new Expression<Func<Employee, bool>>[]{
        firstNameFilter, lastNameFilter, jobFilter, teamFilter }, employeeFilter.Skip, employeeFilter.Take,
        (employee) => employee.Address, (employee) => employee.Job, (employee) => employee.Teams);
        return Mapper.Map<List<EmployeeList>>(entities);
    }

    public async Task UpdateEmployeeAsync(EmployeeUpdate employeeUpdate)
    {
        //72
        await EmployeeUpdateValidator.ValidateAndThrowAsync(employeeUpdate);

        //76 77
        var existingEmployee = await EmployeeRepository.GetByIdAsync(employeeUpdate.Id); //db roundtrip
        if (existingEmployee == null)
            throw new EmployeeNotFoundException(employeeUpdate.Id);

        var entity = Mapper.Map<Employee>(employeeUpdate);
        //68
        var address = await AddressRepository.GetByIdAsync(employeeUpdate.AddressId);
        var job = await JobRepository.GetByIdAsync(employeeUpdate.JobId);
        //76 77
        if (address == null)
            throw new AddressNotFoundException(employeeUpdate.AddressId);
        if (job == null)
            throw new JobNotFoundException(employeeUpdate.JobId);

        entity.Address = address;
        entity.Job = job;

        EmployeeRepository.Update(entity);
        await EmployeeRepository.SaveChangesAsync();
    }

    //99
    public async Task UpdateProfilePhotoAsync(ProfilePhotoUpdate profilePhotoUpdate)
    {
        await ImageFileValidator.ValidateAndThrowAsync(profilePhotoUpdate.Photo);

        var employee = await EmployeeRepository.GetByIdAsync(profilePhotoUpdate.EmployeeId);
        if (employee == null) 
            throw new EmployeeNotFoundException(profilePhotoUpdate.EmployeeId);
        if (employee.ProfilePhotoPath != null)
            await UploadService.DeleteFileAsync(employee.ProfilePhotoPath); //101
            //FileService.DeleteFile(employee.ProfilePhotoPath); //101
        //var filename = await FileService.SaveFileAsync(profilePhotoUpdate.Photo); //101
        var filename = await UploadService.UploadFileAsync(profilePhotoUpdate.Photo); //101
        employee.ProfilePhotoPath = filename;
        EmployeeRepository.Update(employee);
        await EmployeeRepository.SaveChangesAsync();
    }
}
