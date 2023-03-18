
using Courseproject.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Courseproject.API.Controllers;
//100

[ApiController]
[Route("[controller]")]
public class DownloadController : ControllerBase
{
    //private IFileService FileService { get; } //101
    private IUploadService UploadService { get; } //101

    public DownloadController(/*IFileService fileService*/ IUploadService uploadService)
	{
		//FileService = fileService; //101
		UploadService = uploadService;
	}

    [HttpGet]
	[Route("Get/{path}")]
	public async Task<IActionResult> GetFile(string path)
	{
		//var bytes = FileService.GetFile(path); //101
		var bytes = await UploadService.GetFileAsync(path);
		return File(bytes, "APPLICATION/octet-stream", path);
	}
}
