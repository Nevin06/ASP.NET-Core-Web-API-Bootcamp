using AutoMapper;
using Courseproject.Business.Exceptions;
using Courseproject.Business.Validation;
using Courseproject.Common.Dtos.Employee;
using Courseproject.Common.Dtos.Job;
using Courseproject.Common.Dtos.Team;
using Courseproject.Common.Interfaces;
using Courseproject.Common.Model;
using FluentValidation;
using System.Linq.Expressions;
//70
namespace Courseproject.Business.Services;

public class TeamService : ITeamService
{
    private IMapper Mapper { get; }
    private IGenericRepository<Team> TeamRepository { get; }
    private IGenericRepository<Employee> EmployeeRepository { get; }
    private TeamCreateValidator TeamCreateValidator { get; }
    private TeamUpdateValidator TeamUpdateValidator { get; }

    public TeamService(IMapper mapper, IGenericRepository<Team> teamRepository, 
        IGenericRepository<Employee> employeeRepository,
        TeamCreateValidator teamCreateValidator, TeamUpdateValidator teamUpdateValidator)
    {
        Mapper = mapper;
        TeamRepository = teamRepository;
        EmployeeRepository = employeeRepository;
        TeamCreateValidator = teamCreateValidator;
        TeamUpdateValidator = teamUpdateValidator;
    }


    public async Task<int> CreateTeamAsync(TeamCreate teamCreate)
    {
        //72
        await TeamCreateValidator.ValidateAndThrowAsync(teamCreate);

        //employee filter
        Expression<Func<Employee, bool>> employeeFilter = (employee) => teamCreate.Employees
        .Contains(employee.Id);
        var employees = await EmployeeRepository.GetFilteredAsync(new[] { employeeFilter },
            null, null);
        //78
        //assign employees of dto , then checked if there are employees that are not in db
        //if they are not in db, we can't assign them
        var missingEmployees = teamCreate.Employees.Where((id) => !employees.Any(existingemp => existingemp.Id == id));
        if (missingEmployees.Any())
            throw new EmployeesNotFoundException(missingEmployees.ToArray());

        var entity = Mapper.Map<Team>(teamCreate);
        entity.Employees = employees;
        await TeamRepository.InsertAsync(entity);
        await TeamRepository.SaveChangesAsync();
        return entity.Id;
    }

    public async Task DeleteTeamAsync(TeamDelete teamDelete)
    {
        var entity = await TeamRepository.GetByIdAsync(teamDelete.Id);
        //78
        if (entity == null)
            throw new TeamNotFoundException(teamDelete.Id);
        if (entity.Employees.Count > 0)
            throw new DependentEmployeesExistException(entity.Employees);

        TeamRepository.Delete(entity);
        await TeamRepository.SaveChangesAsync();
    }

    public async Task<TeamGet> GetTeamAsync(int id)
    {
        var entity = await TeamRepository.GetByIdAsync(id, (team) => team.Employees);
        //78
        if (entity == null)
            throw new TeamNotFoundException(id);
        return Mapper.Map<TeamGet>(entity);
    }

    public async Task<List<TeamGet>> GetTeamsAsync()
    {
        var entities = await TeamRepository.GetAsync(null, null, (team) => team.Employees);
        return Mapper.Map<List<TeamGet>>(entities);
    }

    public async Task UpdateTeamAsync(TeamUpdate teamUpdate)
    {
        //72
        await TeamUpdateValidator.ValidateAndThrowAsync(teamUpdate);

        Expression<Func<Employee, bool>> employeeFilter = (employee) => teamUpdate.Employees
        .Contains(employee.Id);
        var employees = await EmployeeRepository.GetFilteredAsync(new[] { employeeFilter },
            null, null);
        //78
        //assign employees of dto , then checked if there are employees that are not in db
        //if they are not in db, we can't assign them
        var missingEmployees = teamUpdate.Employees.Where((id) => !employees.Any(existingemp => existingemp.Id == id));
        if (missingEmployees.Any())
            throw new EmployeesNotFoundException(missingEmployees.ToArray());

        //why fetching existingEntity from db, we have Employees(list of employees) here
        //we would't do this way and work with just our mapper to create this entity and not
        //fetching from db,it would not be tracked by our entity framework (special case)
        var existingEntity = await TeamRepository.GetByIdAsync(teamUpdate.Id, (team) => team.Employees);

        //78
        if (existingEntity == null)
            throw new TeamNotFoundException(teamUpdate.Id);

        Mapper.Map(teamUpdate, existingEntity);
        existingEntity.Employees = employees;
        TeamRepository.Update(existingEntity);
        await TeamRepository.SaveChangesAsync();
    }
}
