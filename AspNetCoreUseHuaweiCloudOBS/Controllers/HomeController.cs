using Microsoft.AspNetCore.Mvc;
using OBS;
using OBS.Model;
using System.Text;

namespace AspNetCoreUseHuaweiCloudOBS.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<string> Upload([FromForm] IEnumerable<IFormFile> files)
        {
            ObsConfig config = new ObsConfig();
            config.Endpoint = "https://obs.cn-north-4.myhuaweicloud.com";

            string accessKey = "";
            string secretKey = "";
            ObsClient client = new ObsClient(accessKey, secretKey, config);

            foreach (var file in files)
            {
                using var stream = file.OpenReadStream();
                
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = "bidianqing",
                    ObjectKey = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}",
                    InputStream = stream
                };
                var res = client.PutObject(request);
                await stream.FlushAsync();
            }
            
            return "OK";
        }
    }
}
