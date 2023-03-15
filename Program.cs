using Application.Services;
using Application.ServicesInterfaces;
using Domain.RepositoriesInterfaces;
using Infrastructure.Dapper.Contexts;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAssetsService, AssetsService>();
builder.Services.AddSingleton<IAssetsRepository, AssetsRepository>();
builder.Services.AddSingleton<DapperContext>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
