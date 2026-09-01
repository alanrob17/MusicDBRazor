using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories.Interfaces;

namespace MusicDB.Pages.Tracks;

public class ArtistTracksByYearModel : PageModel
{
    private readonly ITrackRepository _trackRepository;

    public const int PageSize = 20;

    public ArtistTracksByYearModel(ITrackRepository trackRepository)
    {
        _trackRepository = trackRepository;
    }

    [BindProperty(SupportsGet = true)]
    public string? YearString { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ArtistName { get; set; }

    public IReadOnlyList<ArtistTracksByYear> Tracks { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>True when YearString parses as a valid recording year.</summary>
    public bool HasYear =>
        int.TryParse(YearString, out int y) && y >= 1900 && y <= DateTime.Now.Year;

    /// <summary>True when an artist-name filter has been entered.</summary>
    public bool HasArtist => !string.IsNullOrWhiteSpace(ArtistName);

    public async Task OnGetAsync(int pageNumber = 1)
    {
        // Year is required — do nothing until a valid year is entered
        if (!HasYear)
            return;

        int.TryParse(YearString, out int year);

        CurrentPage = Math.Max(1, pageNumber);

        var (items, totalCount) = await _trackRepository.GetTracksByYearAsync(
            year, ArtistName, CurrentPage, PageSize);

        TotalCount = totalCount;

        // Guard against pageNumber being out of bounds
        if (CurrentPage > TotalPages && TotalPages > 0)
        {
            CurrentPage = TotalPages;
            (items, _) = await _trackRepository.GetTracksByYearAsync(
                year, ArtistName, CurrentPage, PageSize);
        }

        Tracks = items;
    }
}
