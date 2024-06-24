using Microsoft.EntityFrameworkCore;
using OnlineRailwayReservation.Data;
using OnlineRailwayReservation.Repository;
using OnlineRailwayReservation.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OnlineRailwayReservation.Profiles;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var provider = builder.Services.BuildServiceProvider();
var config = provider.GetService<IConfiguration>();
builder.Services.AddDbContext<ApplicationDbContext>(item => item.UseSqlServer(config.GetConnectionString("dbRailway")));

builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<ITrainRepository, TrainRepository>();
//builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(StationProfile));
builder.Services.AddAutoMapper(typeof(TrainProfile));
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
