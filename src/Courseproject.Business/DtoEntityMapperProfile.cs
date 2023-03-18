//59
//All our Business logic lives here
using AutoMapper;
using Courseproject.Common.Dtos.Address;
using Courseproject.Common.Dtos.Employee;
using Courseproject.Common.Dtos.Job;
using Courseproject.Common.Dtos.Team;
using Courseproject.Common.Model;

namespace Courseproject.Business;

//65
public class DtoEntityMapperProfile : Profile
{
	public DtoEntityMapperProfile()
	{
        // we dont have Id and AddressCreate, need to tell AutoMapper that it should be ignored
        CreateMap<AddressCreate, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        //Tells AutoMapper pls just ignore Id because else automapper won't know what to do with this field and will throw an exception
        CreateMap<AddressUpdate, Address>();
        CreateMap<Address, AddressGet>();
        // doubt AddressGet doesn't have Id right?

        CreateMap<JobCreate, Job>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<JobUpdate, Job>();
        CreateMap<Job, JobGet>();

        //68
        CreateMap<EmployeeCreate, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Teams, opt => opt.Ignore())
            .ForMember(dest => dest.Job, opt => opt.Ignore()); //Address?
        CreateMap<EmployeeUpdate, Employee>()
            .ForMember(dest => dest.Teams, opt => opt.Ignore())
            .ForMember(dest => dest.Job, opt => opt.Ignore());
        CreateMap<Employee, EmployeeDetails>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Teams, opt => opt.Ignore())
            .ForMember(dest => dest.Job, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore());
        CreateMap<Employee, EmployeeList>();

        //70
        CreateMap<TeamCreate, Team>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Employees, opt => opt.Ignore());
        CreateMap<TeamUpdate, Team>()
            .ForMember(dest => dest.Employees, opt => opt.Ignore());
        CreateMap<Team, TeamGet>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}