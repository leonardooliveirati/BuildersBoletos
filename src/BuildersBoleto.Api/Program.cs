using System.Reflection;
using BuildersBoleto.Api.Configuration;
using BuildersBoleto.Api.Token;
using BuildersBoleto.Api.Policy;
using BuildersBoleto.Domain.Intefaces.Proxy;
using BuildersBoleto.Domain.Intefaces.Repositories;
using BuildersBoleto.Domain.Intefaces.Services;
using BuildersBoleto.Infrastructure.Data;
using BuildersBoleto.Infrastructure.Repositories;
using BuildersBoleto.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace BuildersBoleto.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddCheck<ApiHealthCheck>("external_api");

        builder.Services.AddHttpClient<ApiHealthCheck>();

        builder.Services.AddControllers();

        builder.Services.AddDbContext<BuildersBoletoDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Builders Boleto API", Version = "v1" });    
        });

        builder.Services.AddMvc().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
        });

        builder.Services.AddScoped<IBoletoService, BoletoService>();
        builder.Services.AddScoped<IBoletoRepository, BoletoRepository>();
        builder.Services.AddScoped<IBoletoApiClient, BoletoClient>();

        builder.Services.AddHttpClient<BoletoClient>(client =>
        {
            client.BaseAddress = new Uri("https://vagas.builders/api/builders/");
        });

        builder.Services.AddMemoryCache();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Boleto API V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseMiddleware<MiddlewareToken>();
        app.MapControllers();
        app.MapHealthChecks("/health");
        app.Run();
    }
}