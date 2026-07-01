using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Records
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

        public IList<Record> Record { get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>The full artist name matched by the search, or null when no search is active.</summary>
        public string? MatchedArtistName { get; set; }
        public string Recorded { get; set; }

        public async Task OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = Math.Max(1, pageNumber);
            var recorded = 0;

            var query = _context.Records
                .Include(r => r.Artist)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {   
                int.TryParse(SearchString, out recorded);

                if (recorded > 1900 && recorded < 2027)
                {
                    Recorded = recorded.ToString();
                    query = query.Where(r => r.Recorded == recorded);
                }
                else
                {
                    query = query.Where(r =>
                    (r.Artist.Name != null && r.Artist.Name.Contains(SearchString)) ||
                    (r.Artist.FirstName != null && r.Artist.FirstName.Contains(SearchString)) ||
                    (r.Artist.LastName != null && r.Artist.LastName.Contains(SearchString)));

                    // Capture the first matched artist's display name for the heading
                    MatchedArtistName = await query.Select(r => r.Artist.Name).FirstOrDefaultAsync();
                }
            }

            TotalCount = await query.CountAsync();

            if (CurrentPage > TotalPages && TotalPages > 0)
                CurrentPage = TotalPages;

            if (recorded > 1900 && recorded < 2027)
            {
                Record = await query
                .OrderBy(r => r.Artist.LastName)
                .ThenBy(r => r.Artist.FirstName)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
            }
            else
            {
                Record = await query
                .OrderBy(r => r.Artist.LastName)
                .ThenBy(r => r.Artist.FirstName)
                .ThenBy(r => r.Recorded)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
            }
        }
    }
}

