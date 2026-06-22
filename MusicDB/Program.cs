using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Repositories;
using MusicDB.Data.Repositories.Interfaces;

namespace MusicDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Register the EF Core DbContext — reads "MusicDb" connection string from appsettings.json
            builder.Services.AddDbContext<MusicDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MusicDb")));

            // Register repositories (stored procedure access layer)
            builder.Services.AddScoped<IArtistRepository, ArtistRepository>();

            var app = builder.Build();

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
    }
}
