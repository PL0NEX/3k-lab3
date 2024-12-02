using GoalsExample.WebApi.Infrastructure;
using MyWebApi.Domain.Entities;
using MyWebApi.Domain.interfaces;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IRepository<Period>, PeriodRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();


builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
