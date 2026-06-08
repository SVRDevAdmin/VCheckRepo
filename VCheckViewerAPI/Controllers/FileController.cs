using Microsoft.AspNetCore.Mvc;
using VCheck.Lib.Data;
using VCheckViewerAPI.Message.General;

namespace VCheckViewerAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FileController : Controller
    {
        private ApiRepository _apiRepository = new ApiRepository();
        private int CanViewOther;

        [HttpPost(Name = "DownloadLatestInstaller")]
        public IActionResult DownloadLatestInstaller(ClientDataRequest request)
        {
            var sBuilder = Host.CreateApplicationBuilder();
            string filePath = sBuilder.Configuration.GetSection("VersionConfig:Version_FilePath").Value;

            if (request.Header.clientKey != null && _apiRepository.Authenticate(request.Header.clientKey, out CanViewOther) && CanViewOther == 1)
            {
                if (!System.IO.File.Exists(filePath))
                {
                    return BadRequest(new { error = "No patch found", code = 400 });
                }
                
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "application/octet-stream", "Patch.zip");
            }
            else
            {
                return BadRequest(new { error = "Unauthorized user", code = 400 });
            }
        }
    }
}
