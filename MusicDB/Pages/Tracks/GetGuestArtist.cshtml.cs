using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories.Interfaces;

namespace MusicDB.Pages.Tracks;

public class GetGuestArtistModel : PageModel
{
    private readonly ITrackRepository _trackRepository;

    public const int PageSize = 20;

    public GetGuestArtistModel(ITrackRepository trackRepository)
    {
        _trackRepository = trackRepository;
    }

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public IReadOnlyList<GuestArtistTrack> Tracks { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public async Task OnGetAsync(int pageNumber = 1)
    {
        // Only query the SP when the user has entered a search term
        if (string.IsNullOrWhiteSpace(SearchString))
            return;

        CurrentPage = Math.Max(1, pageNumber);

        var (items, totalCount) = await _trackRepository.GetGuestArtistTracksAsync(
            SearchString, CurrentPage, PageSize);

        TotalCount = totalCount;

        // Guard against pageNumber being out of bounds
        if (CurrentPage > TotalPages && TotalPages > 0)
        {
            CurrentPage = TotalPages;
            (items, _) = await _trackRepository.GetGuestArtistTracksAsync(
                SearchString, CurrentPage, PageSize);
        }

        Tracks = items;
    }
}
