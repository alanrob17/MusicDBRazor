using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicDB.Data;
using Xunit;

namespace MusicDB.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptors = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<MusicDbContext>) ||
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType == typeof(MusicDbContext) ||
                (d.ServiceType.FullName != null && d.ServiceType.FullName.Contains("EntityFrameworkCore")) ||
                (d.ImplementationType?.FullName != null && d.ImplementationType.FullName.Contains("EntityFrameworkCore"))
            ).ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<MusicDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestsDb_" + Guid.NewGuid().ToString());
            });
        });
    }
}

public class RazorPagesRoutingTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RazorPagesRoutingTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Artists")]
    [InlineData("/Records")]
    [InlineData("/Discs")]
    [InlineData("/Tracks")]
    [InlineData("/Privacy")]
    public async Task Get_Endpoints_ReturnSuccessAndCorrectContentType(string url)
    {
        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");
    }
}
