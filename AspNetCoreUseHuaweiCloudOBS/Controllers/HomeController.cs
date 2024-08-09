using Microsoft.AspNetCore.Mvc;
using OBS;
using OBS.Model;

namespace AspNetCoreUseHuaweiCloudOBS.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ObsClient _obsClient;

        public HomeController(ILogger<HomeController> logger, ObsClient obsClient)
        {
            _logger = logger;
            _obsClient = obsClient;
        }

        [HttpPost("upload")]
        public async Task<string> Upload([FromForm] IEnumerable<IFormFile> files)
        {
            foreach (var file in files)
            {
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = "bidianqing",
                    ObjectKey = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}",
                    ContentType = file.ContentType,
                    ContentDisposition = file.ContentDisposition,
                    InputStream = file.OpenReadStream()
                };
                var res = _obsClient.PutObject(request);
                await request.InputStream.FlushAsync();
            }
            
            return "OK";
        }
    }
}
