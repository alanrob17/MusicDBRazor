using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories.Interfaces;

namespace MusicDB.Pages.Tracks
{
    public class TracksByYearModel : PageModel
    {
        private readonly ITrackRepository _trackRepository;

        public const int PageSize = 20;

        public TracksByYearModel(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public IReadOnlyList<BriefTrack> Tracks { get; set; } = [];

        public int CurrentPage { get; set; } = 1;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public async Task OnGetAsync(int pageNumber = 1)
        {
            // Only query the SP when the user has entered a search term
            if (string.IsNullOrWhiteSpace(SearchString))
                return;

            var year = 0;
            int.TryParse(SearchString, out year);

            if (year < 1900 || year > DateTime.Now.Year)
            {
                return; // Invalid year, do not perform the search
            }

            CurrentPage = Math.Max(1, pageNumber);

            var (items, totalCount) = await _trackRepository.GetGetBriefTrackListByYearAsync(SearchString, CurrentPage, PageSize);

            TotalCount = totalCount;

            // Guard against pageNumber being out of bounds
            if (CurrentPage > TotalPages && TotalPages > 0)
            {
                CurrentPage = TotalPages;
                (items, _) = await _trackRepository.GetGetBriefTrackListByYearAsync(SearchString, CurrentPage, PageSize);
            }

            Tracks = items;
        }
    }
}
