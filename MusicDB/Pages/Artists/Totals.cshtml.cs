using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories.Interfaces;

namespace MusicDB.Pages.Artists;

public class TotalsModel : PageModel
{
    private readonly IArtistRepository _artistRepository;

    public const int PageSize = 20;

    public TotalsModel(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }

    public IReadOnlyList<ArtistTotals> ArtistTotals { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public async Task OnGetAsync(int pageNumber = 1)
    {
        CurrentPage = Math.Max(1, pageNumber);

        var (items, totalCount) = await _artistRepository.GetArtistTotalsAsync(
            CurrentPage, PageSize);

        TotalCount = totalCount;

        // Guard against pageNumber being out of bounds
        if (CurrentPage > TotalPages && TotalPages > 0)
        {
            CurrentPage = TotalPages;
            (items, _) = await _artistRepository.GetArtistTotalsAsync(CurrentPage, PageSize);
        }

        ArtistTotals = items;
    }
}
