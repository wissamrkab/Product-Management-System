using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using PMS.Domain.Common;
using PMS.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PMS.Application.Common.Mapping;
using PMS.Application.Features.Products.Commands.CreateProduct;
using PMS.Application.Helpers;
using PMS.Application.Interfaces;

namespace PMS.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly()]);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        services.AddHttpContextAccessor();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IUserAuthService, UserAuthService>();
        services.AddScoped<UploadFileHelper>();
     
        // services.AddTransient(typeof(UserManager<>), typeof(UserHelper<>));
        services.AddHostedService<OutboxProcessor>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
                };
        });

    }
}