using Aplication.Services;
using Aplication.UseCases;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using Api.Extensions;
using Infrastructure.Repositories;
var builder = WebApplication.CreateBuilder(args);

//dbcontext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
builder.Services.AddScoped<UserRepositoryI, UserRepository > ();
builder.Services.AddScoped<RefreshTokenRepositoryI, RefreshTokenRepository>();
builder.Services.AddScoped<LoginAttemptRepositoryI, LoginAttemptRepository>();

builder.Services.AddScoped<UserServicesI, UserService>();
builder.Services.AddScoped<JwtServiceI,JwtService> ();
builder.Services.AddScoped<RefreshTokenServiceI, RefreshTokenService>();
builder.Services.AddScoped<EmailAttemptsServiceI,EmailAttemptsService> ();
builder.Services.AddScoped<LoginAttemptsServiceI, LoginAttemptsService>();

builder.Services.AddScoped<AuthUseCase>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//--------------------------EXTENSIONS----------------------------------------
builder.Services.AddIpRateLimit(builder.Configuration);
//-----------------------------JWT--------------------------------------------
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerJwt(builder.Configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<DataContext>();
db.Database.EnsureCreated();//create DB if not exist

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();  

app.MapControllers();

app.Run();
