using DGTickets.Backend.Data;
using DGTickets.Backend.Helpers;
using DGTickets.Backend.Repositories.Implementations;
using DGTickets.Backend.Repositories.Interfaces;
using DGTickets.Backend.UnitsOfWork.Implementations;
using DGTickets.Backend.UnitsOfWork.Interfaces;
using DGTickets.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));
builder.Services.AddTransient<SeedDb>();

builder.Services.AddScoped<IFileStorage, FileStorage>();

builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IMedicinesStockRepository, MedicinesStockRepository>();
builder.Services.AddScoped<IMedicinesStockUnitOfWork, MedicinesStockUnitOfWork>();

builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();

builder.Services.AddScoped<IStatesRepository, StatesRepository>();
builder.Services.AddScoped<IStatesUnitOfWork, StatesUnitOfWork>();

builder.Services.AddScoped<ICitiesRepository, CitiesRepository>();
builder.Services.AddScoped<ICitiesUnitOfWork, CitiesUnitOfWork>();

builder.Services.AddScoped<IHeadquartersRepository, HeadquartersRepository>();
builder.Services.AddScoped<IHeadquartersUnitOfWork, HeadquartersUnitOfWork>();

builder.Services.AddScoped<IHeadquarterMedicinesRepository, HeadquarterMedicinesRepository>();
builder.Services.AddScoped<IHeadquarterMedicinesUnitOfWork, HeadquarterMedicinesUnitOfWork>();

builder.Services.AddScoped<IModulesRepository, ModulesRepository>();
builder.Services.AddScoped<IModulesUnitOfWork, ModulesUnitOfWork>();

builder.Services.AddScoped<IRatingsRepository, RatingsRepository>();
builder.Services.AddScoped<IRatingsUnitOfWork, RatingsUnitOfWork>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();

builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero
    });

var app = builder.Build();

SeedData(app);

void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using var scope = scopedFactory!.CreateScope();
    var service = scope.ServiceProvider.GetService<SeedDb>();
    service!.SeedAsync().Wait();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();