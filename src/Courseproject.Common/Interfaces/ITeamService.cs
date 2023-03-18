using Courseproject.Common.Dtos.Team;

namespace Courseproject.Common.Interfaces;
//69
public interface ITeamService
{
    Task<int> CreateTeamAsync(TeamCreate teamCreate);
    Task UpdateTeamAsync(TeamUpdate teamUpdate);
    Task DeleteTeamAsync(TeamDelete teamDelete);
    Task<TeamGet> GetTeamAsync(int id);
    Task<List<TeamGet>> GetTeamsAsync();
}
