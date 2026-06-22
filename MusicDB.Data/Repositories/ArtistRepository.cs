using Microsoft.EntityFrameworkCore;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories.Interfaces;

namespace MusicDB.Data.Repositories;

/// <summary>
/// Executes Artist-related stored procedures via EF Core's SqlQuery API.
/// All parameters use SqlParameter to prevent SQL injection.
/// </summary>
public class ArtistRepository : IArtistRepository
{
    private readonly MusicDbContext _context;

    public ArtistRepository(MusicDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<ArtistTotals> Items, int TotalCount)> GetArtistTotalsAsync(
        int page, int pageSize)
    {
        // Fetch the full result set from the stored procedure.
        // SqlQuery<T> cannot be composed with Skip/Take before execution, so we
        // materialise everything first and apply paging in-memory.
        var all = await _context.Database
            .SqlQuery<ArtistTotals>($"EXEC adm_GetArtistTotals")
            .ToListAsync();

        int totalCount = all.Count;

        var items = all
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList()
            .AsReadOnly();

        return (items, totalCount);
    }
}
