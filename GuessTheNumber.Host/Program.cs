using FluentValidation;
using FluentValidation.AspNetCore;
using GuessTheNumber.Abstractions.Interfaces;
using GuessTheNumber.Application.Models.Requests;
using GuessTheNumber.Application.Models.Validators;
using GuessTheNumber.Dal.Abstractions.Interfaces;
using GuessTheNumber.Dal.Implementation;
using GuessTheNumber.Host.Filters;
using GuessTheNumber.Implementation;
using GuessTheNumber.Implementation.Managers;
using GuessTheNumber.Implementation.Services.Random;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers(options => { options.Filters.Add<ExceptionFilter>(0); })
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<MediatRMarkerClass>());
builder.Services.AddTransient<IValidator<InitializeGameRequest>, InitializeGameRequestValidator>();
builder.Services.AddScoped<IGameManager, GameManager>();
builder.Services.AddTransient<IGuessRandomNumberGenerator, GuessRandomNumberGenerator>();

builder.Services.AddDbContext<IGameSessionWriteContext, GameSessionWriteContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDbContext<IGameSessionReadContext, GameSessionReadContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();