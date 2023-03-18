using Courseproject.Common.Dtos.Team;
using Courseproject.Common.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courseproject.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TeamController : ControllerBase
{
    private ITeamService TeamService { get; }

	public TeamController(ITeamService teamService)
	{
        TeamService = teamService;
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateTeam(TeamCreate teamCreate)
    {
        var id = await TeamService.CreateTeamAsync(teamCreate);
        return Ok(id);
    }

    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> UpdateTeam(TeamUpdate teamUpdate)
    {
        await TeamService.UpdateTeamAsync(teamUpdate);
        return Ok();
    }

    [HttpDelete]
    [Route("Delete")]
    public async Task<IActionResult> DeleteTeam(TeamDelete teamDelete)
    {
        await TeamService.DeleteTeamAsync(teamDelete);
        return Ok();
    }

    [HttpGet]
    [Route("Get/{id}")]
    public async Task<IActionResult> GetTeam(int id)
    {
        var Team = await TeamService.GetTeamAsync(id);
        return Ok(Team);
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> GetTeams()
    {
        var Teams = await TeamService.GetTeamsAsync();
        return Ok(Teams);
    }
}
