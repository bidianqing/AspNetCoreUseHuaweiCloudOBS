using OBS;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel((options) =>
{
    // Handle requests up to 47 MB
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 47;
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ObsClient>(sp =>
{
    return new ObsClient("", "", new ObsConfig { Endpoint = "https://obs.cn-north-4.myhuaweicloud.com" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
