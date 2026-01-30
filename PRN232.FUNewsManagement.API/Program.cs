using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using FUNewsManagementSystemRepository.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PRN232.FUNewsManagement.API.Helper;
using PRN232.FUNewsManagement.Repo;
using PRN232.FUNewsManagement.Repo.Interface;
using PRN232.FUNewsManagement.Repo.Repository;
using PRN232.FUNewsManagement.Services.Interface;
using PRN232.FUNewsManagement.Services.Service;
using PRN232.FUNewsManagement.Services.Utility;
using PRN232.FUNewsManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.DefaultIgnoreCondition= System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Lab 2 API", Version = "v1" });

	// 1. Define the 'Bearer' security scheme
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter your JWT token in the text input below.\r\n\r\nExample: 'eyJhbGciOiJIUzI1Ni...' (Do not prefix with 'Bearer ')"
	});

	// 2. Make sure Swagger uses that scheme globally
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

builder.Services.AddDbContext<FUNewsManagementContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped<DbContext>(provider =>
	provider.GetRequiredService<FUNewsManagementContext>());

//Add Repository and UnitOfWork dependencies
builder.Services.AddInfrastructure();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MapperProfile).Assembly));


// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	var JWTsettings = builder.Configuration.GetSection("JwtSetting");
	var key = System.Text.Encoding.UTF8.GetBytes(JWTsettings["SecretKey"]!);

	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = JWTsettings["Issuer"],
		ValidAudience = JWTsettings["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(key),
	};
});



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		var context = services.GetRequiredService<FUNewsManagementContext>();

		// Use context.Database.GetService<T> instead of calling it directly on the context
		var databaseCreator = context.Database.GetService<IRelationalDatabaseCreator>();

		if (!databaseCreator.Exists())
		{
			databaseCreator.Create();
			Console.WriteLine("Database 'FUNewsManagement' created.");
		}

		context.Database.Migrate();
		Console.WriteLine("Migrations applied successfully.");

		// Seeding Logic
		if (!context.Categories.Any())
		{
			// context.Categories.Add(new Category { ... });
			context.SaveChanges();
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Database Init Error: {ex.Message}");
	}
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

/*
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v2/swagger.json", "FU News API V1");
	c.RoutePrefix = string.Empty; 
});
*/

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
