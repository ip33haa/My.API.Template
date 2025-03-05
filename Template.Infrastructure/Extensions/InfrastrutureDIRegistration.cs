using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Template.Application.Interfaces;
using Template.Application.Services;
using Template.Infrastructure.Data;
using Template.Infrastructure.Repositories;

namespace Template.Infrastructure.Extensions
{
    public static class InfrastrutureDIRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<TemplateDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TemplateDatabase"),
            o => o.EnableRetryOnFailure()));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = configuration["AppSettings:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = configuration["AppSettings:Audience"],
                            ValidateLifetime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!)),
                            ValidateIssuerSigningKey = true
                        };
                    });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddSignalR();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
