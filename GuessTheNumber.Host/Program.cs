using FluentValidation;
using FluentValidation.AspNetCore;
using GuessTheNumber.Abstractions.Interfaces;
using GuessTheNumber.Application.Models.Requests;
using GuessTheNumber.Application.Models.Validators;
using GuessTheNumber.Host.Filters;
using GuessTheNumber.Implementation;
using GuessTheNumber.Infrastructure.Random;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers(options => { options.Filters.Add<ExceptionFilter>(0); })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddTransient<IValidator<InitializeGameRequest>, InitializeGameRequestValidator>();
builder.Services.AddTransient<IGameManager, GameManager>();
builder.Services.AddTransient<IGuessRandomNumberGenerator, GuessRandomNumberGenerator>();

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