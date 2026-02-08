
using BSDigital.API.Interfaces;
using BSDigital.API.Services;
using BSDigital.Clients;
using BSDigital.DataAccess;
using BSDigital.Hubs;
using BSDigital.Interfaces;
using BSDigital.Repositories;
using BSDigital.Services;
using Microsoft.EntityFrameworkCore;

namespace BSDigital
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IOrderBookSnapshotRepository, OrderBookSnapshotRepository>();
            builder.Services.AddSingleton<IAuditService, AuditService>();
            builder.Services.AddSingleton<IOrderBookService, OrderBookService>();
            builder.Services.AddSingleton<IBitstampClient, BitstampClient>();
            builder.Services.AddSingleton<IBitstampService, BitstampService>();
            builder.Services.AddSingleton<IOrderBookApiService, OrderBookApiService>();

            builder.Services.AddHostedService<BitstampBackgroundService>();

            builder.Services.AddControllers();

            builder.Services.AddSignalR();

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("Main"));
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var corsOrigins = builder.Configuration["CORS_ORIGINS"]
                ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                ?? Array.Empty<string>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                    policy
                        .WithOrigins(corsOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                );
            });

            var app = builder.Build();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.MapHub<MarketDataHub>("/market-data");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
