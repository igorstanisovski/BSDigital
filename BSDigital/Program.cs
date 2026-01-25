
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

            builder.WebHost.UseUrls("http://0.0.0.0:8081");

            builder.Services.AddSingleton<IOrderBookSnapshotRepository, OrderBookSnapshotRepository>();
            builder.Services.AddSingleton<IAuditService, AuditService>();

            // Add services to the container.
            builder.Services.AddSingleton<IBitstampClient, BitstampClient>();
            builder.Services.AddSingleton<BitstampService>();

            builder.Services.AddHostedService<BitstampBackgroundService>();

            builder.Services.AddControllers();

            builder.Services.AddSignalR();

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("Main"));
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                    policy
                        .WithOrigins("http://localhost:4200")
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

            // app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
