using MassTransit;
using NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configuring MassTransit
builder.Services.AddMassTransit(x =>
{
    // set endpoint for the exchange
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("nt", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
        {
            h.Username(builder.Configuration.GetValue("RabbitMQUser", "guest"));
            h.Password(builder.Configuration.GetValue("RabbitMQPassword", "guest"));
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSignalR();

var app = builder.Build();
app.MapHub<NotificationHub>("/notifications");

app.Run();
