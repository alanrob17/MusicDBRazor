using FluentAssertions;
using MusicDB.Data.Entities;
using MusicDB.Pages.Records;
using MusicDB.Tests.Helpers;
using Xunit;
using Record = MusicDB.Data.Entities.Record;

namespace MusicDB.Tests.Pages;

public class RecordsIndexModelTests
{
    [Fact]
    public async Task OnGetAsync_WithYearSearch_ShouldFilterByRecordedYear()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateInMemoryDbContext();
        var artist = new Artist { ArtistId = 1, FirstName = "David", LastName = "Bowie", Name = "David Bowie" };
        context.Artists.Add(artist);
        context.Records.AddRange(
            new Record { RecordId = 1, ArtistId = 1, Artist = artist, Name = "Heroes", Recorded = 1977 },
            new Record { RecordId = 2, ArtistId = 1, Artist = artist, Name = "Low", Recorded = 1977 },
            new Record { RecordId = 3, ArtistId = 1, Artist = artist, Name = "Let's Dance", Recorded = 1983 }
        );
        await context.SaveChangesAsync();

        var pageModel = new IndexModel(context)
        {
            SearchString = "1977"
        };

        // Act
        await pageModel.OnGetAsync(pageNumber: 1);

        // Assert
        pageModel.Record.Should().HaveCount(2);
        pageModel.Recorded.Should().Be("1977");
        pageModel.Record.Should().OnlyContain(r => r.Recorded == 1977);
    }

    [Fact]
    public async Task OnGetAsync_WithArtistNameSearch_ShouldFilterByArtist()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateInMemoryDbContext();
        var bowie = new Artist { ArtistId = 1, FirstName = "David", LastName = "Bowie", Name = "David Bowie" };
        var gabriel = new Artist { ArtistId = 2, FirstName = "Peter", LastName = "Gabriel", Name = "Peter Gabriel" };
        context.Artists.AddRange(bowie, gabriel);
        context.Records.AddRange(
            new Record { RecordId = 1, ArtistId = 1, Artist = bowie, Name = "Heroes", Recorded = 1977 },
            new Record { RecordId = 2, ArtistId = 2, Artist = gabriel, Name = "So", Recorded = 1986 }
        );
        await context.SaveChangesAsync();

        var pageModel = new IndexModel(context)
        {
            SearchString = "Gabriel"
        };

        // Act
        await pageModel.OnGetAsync(pageNumber: 1);

        // Assert
        pageModel.Record.Should().ContainSingle();
        pageModel.Record.First().Name.Should().Be("So");
        pageModel.MatchedArtistName.Should().Be("Peter Gabriel");
    }
}
