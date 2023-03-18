using Courseproject.Common.Dtos.Job;
using Courseproject.Common.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courseproject.API.Controllers;
//66
[ApiController]
[Route("[controller]")]
//route of this will be Job only, 'controller' reserve key word cut out
public class JobController : ControllerBase
{
    private IJobService JobService { get; }

    //inject service
    public JobController(IJobService jobService)
	{
        JobService = jobService;
    }

    //endpoint
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateJob(JobCreate jobCreate)
    {
        var id = await JobService.CreateJobAsync(jobCreate);
        return Ok(id);
    }

    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> UpdateJob(JobUpdate jobUpdate)
    {
        await JobService.UpdateJobAsync(jobUpdate);
        return Ok();
    }

    [HttpDelete]
    [Route("Delete")]
    public async Task<IActionResult> DeleteJob(JobDelete jobDelete)
    {
        await JobService.DeleteJobAsync(jobDelete);
        return Ok();
    }

    [HttpGet]
    [Route("Get/{id}")]
    public async Task<IActionResult> GetJob(int id)
    {
        var job = await JobService.GetJobAsync(id);
        return Ok(job);
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> GetJobs()
    {
        var jobs = await JobService.GetJobsAsync();
        return Ok(jobs);
    }
}
