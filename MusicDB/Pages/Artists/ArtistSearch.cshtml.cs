using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Artists
{
    public class ArtistSearchModel : PageModel
    {
        private readonly MusicDbContext _context;

        public const int PageSize = 20;

        public ArtistSearchModel(MusicDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int? SelectedArtistId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public List<SelectListItem> ArtistSelectList { get; set; } = new();

        public Artist? SelectedArtist { get; set; }

        public IList<Record> Records { get; set; } = default!;

        public int CurrentPage { get; set; } = 1;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public int WindowStart { get; set; }
        public int WindowEnd { get; set; }
        public int PrevWindowPage { get; set; }
        public int NextWindowPage { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = Math.Max(1, pageNumber);

            // Populate the dropdown list with artists, filtered if SearchString is specified
            var artistsQuery = _context.Artists.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                artistsQuery = artistsQuery.Where(a => a.Name != null && a.Name.Contains(SearchString));
            }

            ArtistSelectList = await artistsQuery
                .OrderBy(a => a.Name)
                .Select(a => new SelectListItem
                {
                    Value = a.ArtistId.ToString(),
                    Text = a.Name ?? "Unknown Artist"
                })
                .ToListAsync();

            if (SelectedArtistId.HasValue && SelectedArtistId.Value > 0)
            {
                SelectedArtist = await _context.Artists
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.ArtistId == SelectedArtistId.Value);

                if (SelectedArtist != null)
                {
                    var recordsQuery = _context.Records
                        .AsNoTracking()
                        .Where(r => r.ArtistId == SelectedArtistId.Value);

                    TotalCount = await recordsQuery.CountAsync();

                    if (CurrentPage > TotalPages && TotalPages > 0)
                    {
                        CurrentPage = TotalPages;
                    }

                    const int windowSize = 15;
                    WindowStart = ((CurrentPage - 1) / windowSize) * windowSize + 1;
                    WindowEnd = Math.Min(WindowStart + windowSize - 1, TotalPages);
                    PrevWindowPage = WindowStart - 1;
                    NextWindowPage = WindowEnd + 1;

                    Records = await recordsQuery
                        .OrderBy(r => r.Recorded)
                        .Skip((CurrentPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToListAsync();
                }
            }

            return Page();
        }
    }
}
