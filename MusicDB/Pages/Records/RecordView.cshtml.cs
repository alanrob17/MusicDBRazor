using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Records
{
    public class RecordViewModel : PageModel
    {
        private readonly MusicDbContext _context;

        public RecordViewModel(MusicDbContext context)
        {
            _context = context;
        }

        public Record Record { get; set; } = default!;
        public List<Disc> Discs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var record = await _context.Records
                .Include(r => r.Artist)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RecordId == id);

            if (record is null)
                return NotFound();

            Record = record;

            // Load discs and their tracks ordered by disc number / track number
            Discs = await _context.Discs
                .Include(d => d.Tracks)
                .Where(d => d.RecordId == id)
                .OrderBy(d => d.DiscNumber)
                .AsNoTracking()
                .ToListAsync();

            // Sort tracks in-memory
            foreach (var disc in Discs)
            {
                disc.Tracks = disc.Tracks.OrderBy(t => t.Number).ToList();
            }

            return Page();
        }
    }
}
