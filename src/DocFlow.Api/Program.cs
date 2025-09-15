using DocFlow.Api.Features;
using DocFlow.Application.Repositories;
using DocFlow.Application.Services.Implementations.BFF;
using DocFlow.Application.Services.Interfaces;
using DocFlow.Infrastructure.Auth;
using DocFlow.Infrastructure.Data.EntityFramework;
using DocFlow.Infrastructure.Data.EntityFramework.Repositories;
using DocFlow.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DocFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddSingleton<IFileStorageService>(new LocalStorageService("wwwroot/uploads"));
builder.Services.AddScoped<IAuthenticationService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAuthEndpoints();
app.MapDocumentEndpoints();

app.Run();
