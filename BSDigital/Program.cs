
using BSDigital.Clients;
using BSDigital.Hubs;
using BSDigital.Interfaces;
using BSDigital.Services;

namespace BSDigital
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IBitstampClient, BitstampClient>();
            builder.Services.AddSingleton<BitstampService>();

            builder.Services.AddHostedService<BitstampBackgroundService>();

            builder.Services.AddControllers();

            builder.Services.AddSignalR();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.MapHub<MarketDataHub>("/market-data");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
