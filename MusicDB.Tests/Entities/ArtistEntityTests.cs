using FluentAssertions;
using MusicDB.Data.Entities;
using Xunit;
using Record = MusicDB.Data.Entities.Record;

namespace MusicDB.Tests.Entities;

public class ArtistEntityTests
{
    [Fact]
    public void Artist_ShouldInitializeWithDefaultValues()
    {
        // Act
        var artist = new Artist();

        // Assert
        artist.ArtistId.Should().Be(0);
        artist.FirstName.Should().BeNull();
        artist.LastName.Should().BeNull();
        artist.Name.Should().BeNull();
        artist.Biography.Should().BeNull();
        artist.Folder.Should().BeNull();
        artist.RecordArtistId.Should().BeNull();
        artist.Records.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Artist_ShouldAllowSettingPropertiesAndAddingRecords()
    {
        // Arrange
        var artist = new Artist
        {
            ArtistId = 1,
            FirstName = "David",
            LastName = "Bowie",
            Name = "David Bowie",
            Folder = @"D:\Music\David Bowie",
            RecordArtistId = 42
        };

        var record = new Record
        {
            RecordId = 10,
            ArtistId = 1,
            Name = "Heroes",
            Recorded = 1977
        };

        // Act
        artist.Records.Add(record);

        // Assert
        artist.Name.Should().Be("David Bowie");
        artist.Records.Should().HaveCount(1);
        artist.Records.First().Name.Should().Be("Heroes");
    }
}
