using DataGenConsumer.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDatagenKafkaConsumer(builder.Configuration);

builder.Services.AddSnowflakeDb(builder.Configuration);

var app = builder.Build();

app.UseRouting();

app.Run();
