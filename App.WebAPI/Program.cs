using App.WebAPI.Utils;
using CleanArchMonolit.Application.Auth.Login.Commands.Login;
using CleanArchMonolit.Application.Auth.Validators.Behaviors;
using CleanArchMonolit.Infrastruture.Data;
using CleanArchMonolit.Shared.Extensions;
using CleanArchMonolit.Shared.Middlewares;
using CleanArchMonolit.Shared.Responses;
using CleanArchMonolit.Shared.Settings;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using static CleanArchMonolit.Application.Auth.Login.Commands.Login.LoginCommandHandler;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token JWT}"
    });

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

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention());

builder.Services.AddAuthorization();
builder.Services.Configure<JwtSettings>(
builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer", options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
        var statusCodeExpired = 440;
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                context.HandleResponse();

                var expired = context.Response.Headers.ContainsKey("Token-Expired");
                var code = expired ? statusCodeExpired : StatusCodes.Status401Unauthorized;
                var message = expired
                    ? "Sessão expirada, por favor faça login novamente."
                    : "Faça login para continuar.";

                context.Response.StatusCode = code;
                context.Response.ContentType = "application/json";
                var resp = Result<object>.Fail(message); ;
                var payload = JsonSerializer.Serialize(resp);
                return context.Response.WriteAsync(payload);
            }
        };
    });
builder.Services.AddHttpContextAccessor();
builder.AddClients();
builder.AddServices();
builder.AddRepositories();
var app = builder.Build();

app.ConfigurePermissions();


app.UseRouting();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
