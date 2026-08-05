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
    public class ArtistViewModel : PageModel
    {
        private readonly MusicDbContext _context;

        public ArtistViewModel(MusicDbContext context)
        {
            _context = context;
        }

        public Artist Artist { get; set; } = default!;

        /// <summary>Records for this artist, ordered by year recorded.</summary>
        public List<Record> Records { get; set; } = new();

        public int RecordCount => Records.Count;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var artist = await _context.Artists
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ArtistId == id);

            if (artist is null)
                return NotFound();

            Artist = artist;

            Records = await _context.Records
                .AsNoTracking()
                .Where(r => r.ArtistId == id)
                .OrderBy(r => r.Recorded)
                .ToListAsync();

            return Page();
        }
    }
}
