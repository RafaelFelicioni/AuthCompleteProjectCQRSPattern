using CleanArchMonolit.Application.Auth.Interfaces.Token;
using CleanArchMonolit.Application.Auth.Login.Commands.Login;
using CleanArchMonolit.Application.Auth.Repositories.PermissionRepositories;
using CleanArchMonolit.Application.Auth.Repositories.ProfileRepositories;
using CleanArchMonolit.Application.Auth.Repositories.UserRepositories;
using CleanArchMonolit.Application.Auth.Validators.Behaviors;
using CleanArchMonolit.Application.HttpContext;
using CleanArchMonolit.Infrastructure.Auth.Repositories.PermissionRepositories;
using CleanArchMonolit.Infrastructure.Auth.Repositories.ProfileRepositories;
using CleanArchMonolit.Infrastructure.Auth.Repositories.UserRepositories;
using CleanArchMonolit.Infrastructure.Auth.Services.TokenService;
using CleanArchMonolit.Infrastructure.DataShared.HttpContextService;
using CleanArchMonolit.Infrastructure.PoliticalParty.Repositories.PoliticalPartyRepository;
using CleanArchMonolit.Shared.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace App.WebAPI.Utils
{
    public static class ServiceRegistration
    {
        public static void AddClients(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            #region mediator
            var applicationAssembly = typeof(LoginCommand).Assembly;
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
            });

            builder.Services.AddValidatorsFromAssembly(applicationAssembly);

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            #endregion

            builder.Services.AddScoped<IHttpContextService, HttpContextService>();
            builder.Services.AddScoped<IGenerateTokenService, GenerateTokenService>();
            builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            builder.Services.ApplyCommonSettings(builder.Configuration);
        }

        public static void AddRepositories(this WebApplicationBuilder builder)
        {
            #region AuthRepos
            builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            #endregion

            #region PoliticalPartyRepos
            builder.Services.AddScoped<IPoliticalPartyRepository, PoliticalPartyRepository>();
            #endregion
        }
    }
}
