using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MyWebApi.Infrastructure;
using MyWebApi.Repositories;
using MyWebApi.Services;
using MyWebApi.Models;
using MongoDB.Driver;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Конфігурація
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // === JWT НАЛАШТУВАННЯ ===
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        if (jwtSettings == null)
            throw new InvalidOperationException("JwtSettings not found in configuration.");

        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

        builder.Services.AddAuthorization();

        // Сервіси
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWebApi", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization. Example: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new List<string>()
                }
            });
        });

        builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MyWebApi.Mapping.BookingProfile>(), typeof(Program));
        builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient("mongodb://localhost:27017"));
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("MyDatabase"));

        builder.Services.AddScoped<ICarService, CarService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<ICarRepository, CarRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<DataSeeder>();
        builder.Services.AddScoped<IJwtService, JwtService>();

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        // ВИПРАВЛЕНО: Тепер await працює в Main
        // using (var scope = app.Services.CreateScope())
        // {
        //     var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        //     seeder.SeedDataAsync().GetAwaiter().GetResult();
        // }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}