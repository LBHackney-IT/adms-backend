using System;
using Application.Apprentices;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EntityFramework constructor options dependency injection.
var localConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//var AWSconnectionString = builder.Configuration.AddEnvironmentVariables("toBeAscertained");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(localConnectionString));
// 
builder.Services.AddScoped<IReadRepository<Transaction, ResponseTransactionDto>, ReadTransactionRepository>();
builder.Services.AddScoped<IWriteRepository<Transaction, WriteTransactionDto>, WriteTransactionRepository>();
builder.Services.AddScoped<IReadRepository<Apprentice, ResponseApprenticeDto>, ReadApprenticeRepository>();
builder.Services.AddScoped<IWriteRepository<Apprentice, WriteApprenticeDto>, WriteApprenticeRepository>();
builder.Services.AddScoped<IReadRepository<AuditLog, AuditLog>, ReadAuditLogRepository>();
builder.Services.AddScoped<IWriteRepository<AuditLog, AuditLog>, WriteAuditLogRepository>();
//
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();