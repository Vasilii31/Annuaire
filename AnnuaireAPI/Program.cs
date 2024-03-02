using AnnuaireLib.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AnnuaireLib.Context;
using AnnuaireAPI.Services.Interfaces;
using AnnuaireAPI.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
builder.Services.AddAuthorizationBuilder();

builder.Services.AddIdentityCore<User>()
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<AnnuaireDbContext>()
	.AddApiEndpoints();
//chaine de connexion
string connexionString = builder.Configuration.GetConnectionString("MainConnectionString") ??
	throw (new Exception("Connection string is missing"));

builder.Services.AddDbContext<AnnuaireDbContext>(options => options
		.UseMySql(connexionString, ServerVersion.AutoDetect(connexionString)));

// Add services to the container.
builder.Services.AddScoped<AnnuaireDbContext>();
builder.Services.AddScoped<IRoleService, RolesService>();
builder.Services.AddScoped<ISeedService, SeedService>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapIdentityApi<User>();

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
