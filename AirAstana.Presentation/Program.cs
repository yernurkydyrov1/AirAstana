using System.Security.Claims;
using AirAstana.Application.Interfaces;
using AirAstana.Application.Services;
using AirAstana.Application.Queries;
using AirAstana.Infrastructure.Data;
using AirAstana.Infrastructure.Repositories;
using AirAstana.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using AirAstana.Application.Behaviors;
using AirAstana.Application.Commands;
using AirAstana.Application.Commands.Handler;
using AirAstana.Application.Queries.Handlers;
using FluentValidation;
using Microsoft.OpenApi.Models;
using AirAstana.Presentation.Filters;
using AirAstana.Application.Validators;
using AirAstana.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Serilog --------------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// -------------------- Controllers & Filters --------------------
builder.Services.AddControllers(options =>
{
    // Global exception filter для дружелюбных ошибок
    options.Filters.Add<GlobalExceptionFilter>();
});

// -------------------- FluentValidation --------------------
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddFlightCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateFlightStatusCommandValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


// -------------------- Swagger --------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AirAstana API", Version = "v1" });
    
    c.EnableAnnotations();

    // JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT токен с префиксом **Bearer**"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// -------------------- Database --------------------
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(conn));

// -------------------- Repositories & Services --------------------
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IFlightService, FlightService>();

// -------------------- Mediatr --------------------
builder.Services.AddMediatR(
    typeof(LoginUserCommandHandler).Assembly,
    typeof(RegisterUserCommandHandler).Assembly,
    typeof(AddFlightCommandHandler).Assembly,
    typeof(GetFlightsQueryHandler).Assembly,
    typeof(UpdateFlightStatusCommandHandler).Assembly,
    typeof(GetFlightsQuery).Assembly,
    typeof(AddFlightCommand).Assembly,
    typeof(LoginUserCommand).Assembly,
    typeof(RegisterUserCommand).Assembly,
    typeof(UpdateFlightStatusCommand).Assembly
);



// -------------------- MemoryCache --------------------
builder.Services.AddMemoryCache();

// -------------------- JWT Authentication --------------------
var jwtKey = builder.Configuration["Jwt:Key"]!;
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// -------------------- Authorization --------------------
builder.Services.AddAuthorization(options =>
{
    // Политика для модераторов
    options.AddPolicy("ModeratorPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Moderator"));
});

var app = builder.Build();

// -------------------- Database Migrations & Seed --------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try { db.Database.Migrate(); }
    catch
    {
        // ignored
    }

    SeedData.EnsureSeedData(db);
}

// -------------------- Middleware --------------------
app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirAstana API v1"); c.RoutePrefix = "swagger"; });

app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
