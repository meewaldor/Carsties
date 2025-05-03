var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


app.MapReverseProxy();
app.Run();
