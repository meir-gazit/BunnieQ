using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Patite.Domain.Bus;
using Patite.Infra.Bus;
using Patite.Infra.IoC;
using Patite.Microservice.Banking.Data.Context;
using System.Configuration;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<BankingDBContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("BankingDBContext"));

});

RegisterServices(builder.Services);

void RegisterServices(IServiceCollection services)
{    
    DependencyContainer.RegisterServices(services);
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEventBus, RabbitMqBus>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
