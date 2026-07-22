using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data.Entities;
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

    public async Task<(IReadOnlyList<BriefTrack> Items, int TotalCount)> GetGetBriefTrackListByYearAsync(string searchTerm, int page, int pageSize)
    {
        var param = new SqlParameter("@Recorded", searchTerm ?? string.Empty);

        var all = await _context.Database
            .SqlQuery<BriefTrack>($"EXEC up_GetBriefTrackListByYear @Recorded={param}")
            .ToListAsync();

        int totalCount = all.Count;

        var items = all
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList()
            .AsReadOnly();

        return (items, totalCount);
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<GuestArtistTrack> Items, int TotalCount)> GetGuestArtistTracksAsync(string searchTerm, int page, int pageSize)
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

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<TrackNameResult> Items, int TotalCount)> GetTracksByNameAsync(string searchTerm, int page, int pageSize)
    {
        var term = searchTerm ?? string.Empty;

        var all = await _context.Tracks
            .Where(t => t.Name != null && t.Name.Contains(term))
            .OrderBy(t => t.Disc.Record.Artist.Name)
            .ThenBy(t => t.Disc.Record.Recorded)
            .Select(t => new TrackNameResult
            {
                TrackId     = t.TrackId,
                ArtistId    = t.Disc.Record.ArtistId,
                ArtistName  = t.Disc.Record.Artist.Name,
                RecordId    = t.Disc.RecordId,
                RecordName  = t.Disc.Record.Name,
                Recorded    = t.Disc.Record.Recorded,
                Field       = t.Disc.Record.Field,
                DiscId      = t.DiscId,
                DiscName    = t.Disc.Name,
                DiscNumber  = t.Disc.DiscNumber,
                Number      = t.Number,
                TrackName   = t.Name,
                Length      = t.Length,
            })
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
