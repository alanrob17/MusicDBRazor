using FluentAssertions;
using MusicDB.Data.Entities;
using Xunit;
using Record = MusicDB.Data.Entities.Record;

namespace MusicDB.Tests.Entities;

public class RecordEntityTests
{
    [Fact]
    public void Record_ShouldInitializeWithDefaultValues()
    {
        // Act
        var record = new Record();

        // Assert
        record.RecordId.Should().Be(0);
        record.ArtistId.Should().Be(0);
        record.Name.Should().BeNull();
        record.DiscsNavigation.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Record_ShouldAllowAddingDiscs()
    {
        // Arrange
        var record = new Record
        {
            RecordId = 1,
            Name = "The Dark Side of the Moon",
            Recorded = 1973
        };

        var disc = new Disc
        {
            DiscId = 1,
            RecordId = 1,
            Name = "Disc 1",
            DiscNumber = 1
        };

        // Act
        record.DiscsNavigation.Add(disc);

        // Assert
        record.DiscsNavigation.Should().ContainSingle();
        record.DiscsNavigation.First().Name.Should().Be("Disc 1");
    }
}
