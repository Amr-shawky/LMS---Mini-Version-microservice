using LMS___Mini_Version.Messaging;
using MassTransit;
using System.Text.Json;
using System.Net.Http.Json;
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
builder.Services.AddHttpClient();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.MapGet("/api/notifications/ping", () =>
{
    return Results.Ok("Notification Service is running");
});

app.MapGet("/lms", async (IHttpClientFactory factory) => {

    var client = factory.CreateClient();
    var response = await client.GetFromJsonAsync<JsonElement>(
        "http://lms-miniversion:8080/api/v4/test"
        );

    return new
    {
        Service = "test1",
        Response = response
    };

});

app.Run();


