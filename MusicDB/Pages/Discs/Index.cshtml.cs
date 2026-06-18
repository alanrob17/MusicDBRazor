using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Discs
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

        public IList<Disc> Disc { get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>The record name matched by the search, or null when no search is active.</summary>
        public string? MatchedRecordName { get; set; }

        public async Task OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = Math.Max(1, pageNumber);

            var query = _context.Discs
                .Include(d => d.Record)
                    .ThenInclude(r => r.Artist)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                query = query.Where(d =>
                    d.Record.Name != null && d.Record.Name.Contains(SearchString));

                MatchedRecordName = await query.Select(d => d.Record.Name).FirstOrDefaultAsync();
            }

            TotalCount = await query.CountAsync();

            if (CurrentPage > TotalPages && TotalPages > 0)
                CurrentPage = TotalPages;

            Disc = await query
                .OrderBy(d => d.Record.Artist.LastName)
                .ThenBy(d => d.Record.Artist.FirstName)
                .ThenBy(d => d.Record.Recorded)
                .ThenBy(d => d.DiscNumber)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
