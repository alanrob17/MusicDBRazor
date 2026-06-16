using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Artists
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

        public IList<Artist> Artist { get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public async Task OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = Math.Max(1, pageNumber);

            var query = _context.Artists.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                query = query.Where(a => a.Name != null && a.Name.Contains(SearchString));
            }

            TotalCount = await query.CountAsync();

            // Guard against pageNumber being out of bounds for the filtered dataset
            if (CurrentPage > TotalPages && TotalPages > 0)
            {
                CurrentPage = TotalPages;
            }

            Artist = await query
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
