using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NTier.Business.Configuration;
using NTier.Presentation.Middlewares;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.WithThreadId() // Ensure you have the Serilog.Enrichers.Thread package installed
    .Enrich.WithEnvironmentName()
    .Enrich.WithProcessId()
    .Enrich.WithMachineName()
    .WriteTo.Console(new Serilog.Formatting.Compact.CompactJsonFormatter())
    .WriteTo.File(new Serilog.Formatting.Compact.CompactJsonFormatter(), "Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

// Add Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new AutofacModule());
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidAudience = config["JwtToken:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtToken:Token"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Fail("Token has expired.");
                }

                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Replace("bearer ", "");
                if (!string.IsNullOrEmpty(token))
                {
                    var isBlacklisted = context.HttpContext.RequestServices
                        .GetRequiredService<IDistributedCache>()
                        .GetString(token);
                    if (!string.IsNullOrEmpty(isBlacklisted))
                    {
                        context.Fail("Token is invalidated");
                    }
                }
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EzGoalTracker API v1"));
}

app.UseRequestTimeout(TimeSpan.FromSeconds(30)); 
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(myAllowSpecificOrigins);
app.UseAuthentication(); // Make sure this is correctly placed
app.UseAuthorization();
app.MapControllers();
app.Run();
