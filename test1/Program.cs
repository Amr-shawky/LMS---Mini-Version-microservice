using LMS___Mini_Version.Messaging;
using MassTransit;
using test1.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EnrollmentTransferredConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host(builder.Configuration["RabbitMq:Host"] ?? "localhost", "Elevate", h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMq:Password"] ?? "guest");
        });

        cfg.ReceiveEndpoint("test1-enrollment-transferred", e =>
        {
            e.ConfigureConsumer<EnrollmentTransferredConsumer>(context);
        });

    });
});
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.MapGet("/api/notifications/ping", () =>
{
    return Results.Ok("Notification Service is running");
});



app.Run();


