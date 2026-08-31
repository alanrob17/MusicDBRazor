using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Repositories;
using MusicDB.Data.Repositories.Interfaces;
using Serilog;

namespace MusicDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try
            {
                Log.Information("Starting MusicDB web application");

                var builder = WebApplication.CreateBuilder(args);

                // Use Serilog for all application logging
                builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext());

                // Add services to the container.
                builder.Services.AddRazorPages();

                // Register the EF Core DbContext — reads "MusicDb" connection string from appsettings.json
                builder.Services.AddDbContext<MusicDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("MusicDb")));

                // Register repositories (stored procedure access layer)
                builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
                builder.Services.AddScoped<IRecordRepository, RecordRepository>();
                builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();
                builder.Services.AddScoped<ITrackRepository, TrackRepository>();

                var app = builder.Build();

                // Serilog request logging middleware
                // app.UseSerilogRequestLogging();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthorization();

                app.MapStaticAssets();
                app.MapRazorPages()
                   .WithStaticAssets();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
