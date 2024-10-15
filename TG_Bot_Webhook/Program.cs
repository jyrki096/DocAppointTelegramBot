using IRON_PROGRAMMER_BOT_Common;
using Newtonsoft.Json;
using TG_Bot_Webhook;


var builder = WebApplication.CreateBuilder(args);

ContainerConfigurator.Configure(builder.Configuration, builder.Services);
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Formatting = Formatting.Indented; 
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<WebHookConfigurator>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
