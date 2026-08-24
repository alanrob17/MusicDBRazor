using FluentAssertions;
using MusicDB.Data.Entities;
using MusicDB.Pages.Artists;
using MusicDB.Tests.Helpers;
using Xunit;

namespace MusicDB.Tests.Pages;

public class ArtistsIndexModelTests
{
    [Fact]
    public async Task OnGetAsync_WithNoSearch_ShouldReturnAllArtistsPaged()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateInMemoryDbContext();
        context.Artists.AddRange(
            new Artist { ArtistId = 1, FirstName = "David", LastName = "Bowie", Name = "David Bowie" },
            new Artist { ArtistId = 2, FirstName = "Kate", LastName = "Bush", Name = "Kate Bush" },
            new Artist { ArtistId = 3, FirstName = "Peter", LastName = "Gabriel", Name = "Peter Gabriel" }
        );
        await context.SaveChangesAsync();

        var pageModel = new IndexModel(context);

        // Act
        await pageModel.OnGetAsync(pageNumber: 1);

        // Assert
        pageModel.Artist.Should().HaveCount(3);
        pageModel.TotalCount.Should().Be(3);
        pageModel.CurrentPage.Should().Be(1);
        pageModel.TotalPages.Should().Be(1);
        pageModel.Artist[0].LastName.Should().Be("Bowie");
        pageModel.Artist[1].LastName.Should().Be("Bush");
        pageModel.Artist[2].LastName.Should().Be("Gabriel");
    }

    [Fact]
    public async Task OnGetAsync_WithSearchString_ShouldFilterArtists()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateInMemoryDbContext();
        context.Artists.AddRange(
            new Artist { ArtistId = 1, FirstName = "David", LastName = "Bowie", Name = "David Bowie" },
            new Artist { ArtistId = 2, FirstName = "Kate", LastName = "Bush", Name = "Kate Bush" },
            new Artist { ArtistId = 3, FirstName = "Peter", LastName = "Gabriel", Name = "Peter Gabriel" }
        );
        await context.SaveChangesAsync();

        var pageModel = new IndexModel(context)
        {
            SearchString = "Bush"
        };

        // Act
        await pageModel.OnGetAsync(pageNumber: 1);

        // Assert
        pageModel.Artist.Should().ContainSingle();
        pageModel.Artist.First().Name.Should().Be("Kate Bush");
        pageModel.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task OnGetAsync_WithOutOfBoundsPageNumber_ShouldClampToTotalPages()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateInMemoryDbContext();
        context.Artists.Add(new Artist { ArtistId = 1, FirstName = "David", LastName = "Bowie", Name = "David Bowie" });
        await context.SaveChangesAsync();

        var pageModel = new IndexModel(context);

        // Act
        await pageModel.OnGetAsync(pageNumber: 99);

        // Assert
        pageModel.CurrentPage.Should().Be(1);
        pageModel.TotalPages.Should().Be(1);
        pageModel.Artist.Should().ContainSingle();
    }
}
