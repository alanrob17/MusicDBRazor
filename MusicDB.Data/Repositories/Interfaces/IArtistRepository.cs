using MusicDB.Data.Models;

namespace MusicDB.Data.Repositories.Interfaces;

/// <summary>
/// Repository contract for Artist-related stored procedure queries.
/// </summary>
public interface IArtistRepository
{
    /// <summary>
    /// Executes adm_GetArtistTotals, applies in-memory paging, and returns the
    /// current page of results together with the unfiltered total row count.
    /// </summary>
    Task<(IReadOnlyList<ArtistTotals> Items, int TotalCount)> GetArtistTotalsAsync(
        int page, int pageSize);
}
