using MusicDB.Data.Models;

namespace MusicDB.Data.Repositories.Interfaces;

/// <summary>
/// Repository contract for Track-related stored procedure queries.
/// </summary>
public interface ITrackRepository
{
    /// <summary>
    /// Executes adm_GetAlbumsWithOneTrack and returns all single-track albums.
    /// </summary>
    Task<(IReadOnlyList<SingleTrack> Items, int TotalCount)> GetSingleTrackAlbumsAsync();

    /// <summary>
    /// Executes up_GetBriefTrackListByYear and returns the brief track list for a specific year.
    /// </summary>
    Task<(IReadOnlyList<BriefTrack> Items, int TotalCount)> GetGetBriefTrackListByYearAsync(string searchTerm, int page, int pageSize);

    /// <summary>
    /// Executes adm_GetArtistGuestTracks with the supplied search term, applies
    /// in-memory paging, and returns the current page together with the total row count.
    /// </summary>
    Task<(IReadOnlyList<GuestArtistTrack> Items, int TotalCount)> GetGuestArtistTracksAsync(string searchTerm, int page, int pageSize);

    /// <summary>
    /// Searches <c>Track.Name</c> for a partial match using LINQ, applies
    /// in-memory paging, and returns the current page together with the total row count.
    /// </summary>
    Task<(IReadOnlyList<TrackNameResult> Items, int TotalCount)> GetTracksByNameAsync(string searchTerm, int page, int pageSize);
}
