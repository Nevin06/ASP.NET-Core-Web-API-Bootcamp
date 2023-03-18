using Courseproject.Business.Services;
using Courseproject.Business.Validation;
using Courseproject.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Courseproject.Business;
//65
public class DIConfiguration
{
    public static void RegisterServices(IServiceCollection services)
    {
        //which type our profile is, and its Dto entity mapper profile, that registers our profile for dependency injection
        services.AddAutoMapper(typeof(DtoEntityMapperProfile));
        //so we have same instance of this address service all the time in one request
        //each request will have its own instance, but during the same request we will have the same request
        services.AddScoped<IAddressService, AddressService>();
        //66
        services.AddScoped<IJobService, JobService>();

        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ITeamService, TeamService>();

        //72
        services.AddScoped<AddressCreateValidator>();
        services.AddScoped<AddressUpdateValidator>();
        services.AddScoped<EmployeeCreateValidator>();
        services.AddScoped<EmployeeUpdateValidator>();
        services.AddScoped<JobCreateValidator>();
        services.AddScoped<JobUpdateValidator>();
        services.AddScoped<TeamCreateValidator>();
        services.AddScoped<TeamUpdateValidator>();

        //98
        services.AddScoped<ImageFileValidator>();
    }
}
