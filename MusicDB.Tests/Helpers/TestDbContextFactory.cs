using Microsoft.EntityFrameworkCore;
using MusicDB.Data;

namespace MusicDB.Tests.Helpers;

public static class TestDbContextFactory
{
    public static MusicDbContext CreateInMemoryDbContext(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<MusicDbContext>()
            .UseInMemoryDatabase(databaseName: dbName ?? Guid.NewGuid().ToString())
            .Options;

        return new MusicDbContext(options);
    }
}
