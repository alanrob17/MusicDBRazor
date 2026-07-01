using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories.Interfaces;

namespace MusicDB.Data.Repositories;

/// <summary>
/// Executes Track-related stored procedures via EF Core's SqlQuery API.
/// All parameters use SqlParameter to prevent SQL injection.
/// </summary>
public class TrackRepository : ITrackRepository
{
    private readonly MusicDbContext _context;

    public TrackRepository(MusicDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<SingleTrack> Items, int TotalCount)> GetSingleTrackAlbumsAsync()
    {
        var items = await _context.Database
            .SqlQuery<SingleTrack>($"EXEC adm_GetAlbumsWithOneTrack")
            .ToListAsync();

        return (items, items.Count);
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<GuestArtistTrack> Items, int TotalCount)> GetGuestArtistTracksAsync(
        string searchTerm, int page, int pageSize)
    {
        var param = new SqlParameter("@ArtistName", searchTerm ?? string.Empty);

        var all = await _context.Database
            .SqlQuery<GuestArtistTrack>($"EXEC adm_GetArtistGuestTracks @ArtistName={param}")
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
