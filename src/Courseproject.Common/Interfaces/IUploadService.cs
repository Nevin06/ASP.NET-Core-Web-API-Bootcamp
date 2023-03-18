using Microsoft.AspNetCore.Http;

namespace Courseproject.Common.Interfaces;
//101
public interface IUploadService
{
    Task DeleteFileAsync(string filePath);
    Task<string> UploadFileAsync(IFormFile formFile);
    Task<byte[]> GetFileAsync(string filePath);
}
