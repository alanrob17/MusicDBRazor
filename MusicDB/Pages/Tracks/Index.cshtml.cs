using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Tracks
{
    public class IndexModel : PageModel
    {
        private readonly MusicDbContext _context;

        public const int PageSize = 20;

        public IndexModel(MusicDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public IList<Track> Track { get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public async Task OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = Math.Max(1, pageNumber);

            var query = _context.Tracks
                .Include(t => t.Disc)
                    .ThenInclude(d => d.Record)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                query = query.Where(t =>
                    (t.Name != null && t.Name.Contains(SearchString)) ||
                    (t.Artist != null && t.Artist.Contains(SearchString)) ||
                    (t.Title != null && t.Title.Contains(SearchString)));
            }

            TotalCount = await query.CountAsync();

            if (CurrentPage > TotalPages && TotalPages > 0)
                CurrentPage = TotalPages;

            Track = await query
                .OrderBy(t => t.Title)
                .ThenBy(t => t.Number)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
