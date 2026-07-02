using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories;
using MusicDB.Data.Repositories.Interfaces;

namespace MusicDB.Pages.Records
{
    public class FaultyFieldTagModel : PageModel
    {
        private readonly IRecordRepository _recordRepository;

        public const int PageSize = 20;

        public FaultyFieldTagModel(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public IReadOnlyList<FaultyRecordFieldTag> Records { get; set; } = [];
        public int CurrentPage { get; set; } = 1;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public async Task OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = Math.Max(1, pageNumber);

            var (items, totalCount) = await _recordRepository.GetFaultyFieldAlbumsAsync(CurrentPage, PageSize);

            TotalCount = totalCount;

            // Guard against pageNumber being out of bounds
            if (CurrentPage > TotalPages && TotalPages > 0)
            {
                CurrentPage = TotalPages;
                (items, _) = await _recordRepository.GetFaultyFieldAlbumsAsync(CurrentPage, PageSize);
            }

            Records = items;
        }
    }
}
