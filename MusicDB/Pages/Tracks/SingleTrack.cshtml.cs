using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories;
using MusicDB.Data.Repositories.Interfaces;

namespace MusicDB.Pages.Tracks
{
    public class SingleTrackModel : PageModel
    {
        private readonly ITrackRepository _trackRepository;

        public const int PageSize = 20;

        public SingleTrackModel(ITrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public IReadOnlyList<SingleTrack> SingleTracks { get; set; } = [];
        public int TotalCount { get; set; }

        public async Task OnGetAsync()
        {
            var (items, totalCount) = await _trackRepository.GetSingleTrackAlbumsAsync();

            TotalCount = totalCount;

            SingleTracks = items;
        }
    }
}
