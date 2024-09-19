using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NTier.Business.Interfaces;
using NTier.Business.Services;
using NTier.DataAccess.Data;
using NTier.DataAccess.Repositories;
using NTier.DataAccess.UnitOfWork;
using System.Text;

namespace NTier.Business.Configuration
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register the logger as a generic type
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();


            // Register ApplicationDbContext
            builder.Register(ctx =>
            new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("")
                .Options
                )).As<ApplicationDbContext>().InstancePerLifetimeScope();


            // Register ApplicationDbContext with proper connection string
            builder.Register(ctx =>
            {
                var config = ctx.Resolve<IConfiguration>();
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                return new ApplicationDbContext(optionsBuilder.Options);
            }).As<ApplicationDbContext>().InstancePerLifetimeScope();

            // Register IUnitOfWork
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // Register Repositories (Generic Repository and Specific Repositories)
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

            // Register Business Logic Layer Services
            builder.RegisterType<BaseService>().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<GoalService>().As<IGoalService>().InstancePerLifetimeScope();

            // Register JwtToken class as IJwtToken
            builder.Register(ctx =>
            {
                var config = ctx.Resolve<IConfiguration>();
                return new JwtToken(config); // Pass IConfiguration to JwtToken constructor
            }).As<IJwtToken>().InstancePerLifetimeScope();



            // Register IHttpContextAccessor
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            // Register AutoMapper
            builder.Register(context =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();
                });
                return config.CreateMapper();
            }).As<IMapper>().SingleInstance();


            //builder.Register(ctx =>
            //{
            //    // Resolve IConfiguration separately
            //    var config = ctx.Resolve<IConfiguration>();

            //    // Create a new ServiceCollection to register services
            //    var services = new ServiceCollection();

            //    // Configure JWT Authentication
            //    services.AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(options =>
            //    {
            //        options.SaveToken = true;
            //        options.RequireHttpsMetadata = false;
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ValidAudience = config["JwtToken:Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtToken:Token"]))
            //        };

            //        // Events without accessing the service provider prematurely
            //        options.Events = new JwtBearerEvents
            //        {
            //            OnAuthenticationFailed = context =>
            //            {
            //                if (context.Exception is SecurityTokenExpiredException)
            //                {
            //                    context.Fail("Token has expired.");
            //                }
            //                return Task.CompletedTask;
            //            },
            //            OnMessageReceived = context =>
            //            {
            //                var token = context.Request.Headers["Authorization"].ToString()
            //                    .Replace("Bearer ", "").Replace("bearer ", "");

            //                if (!string.IsNullOrEmpty(token))
            //                {
            //                    // Deferred service provider resolution
            //                    var serviceProvider = ctx.Resolve<IServiceProvider>();
            //                    var isBlacklisted = serviceProvider.GetRequiredService<IDistributedCache>()
            //                        .GetString(token);

            //                    if (!string.IsNullOrEmpty(isBlacklisted))
            //                    {
            //                        context.Fail("Token is invalidated.");
            //                    }
            //                }
            //                return Task.CompletedTask;
            //            }
            //        };
            //    });

            //    return services.BuildServiceProvider();
            //}).As<IServiceProvider>().SingleInstance();

        }
    }
}
