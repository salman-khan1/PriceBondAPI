using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;
using PriceBondAPI.Repositories.BondRepository;
using PriceBondAPI.Repositories.DenominationRepository;
using PriceBondAPI.Repositories.DrawRepository;
using PriceBondAPI.Repositories.UserRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PbdatabaseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("PbConnectionString")));

builder.Services.AddScoped<IUserRepository, SqlUserRepository> ();
builder.Services.AddScoped<IDenominationRepository, SqlDenominationRepository> ();
builder.Services.AddScoped<IBondRepository, SqlBondRepository>();
builder.Services.AddScoped<IDrawRepository, SqlDrawRepository>();




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
