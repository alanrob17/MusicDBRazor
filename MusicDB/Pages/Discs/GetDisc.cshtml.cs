using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Discs
{
    public class GetDiscModel : PageModel
    {
        private readonly MusicDbContext _context;

        public GetDiscModel(MusicDbContext context)
        {
            _context = context;
        }

        public Disc Disc { get; set; } = default!;
        public List<Track> Tracks { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int DiscId)
        {
            var disc = await _context.Discs
                .Include(d => d.Tracks)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DiscId == DiscId);

            var record = await _context.Records
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RecordId == disc.RecordId); 

            if (disc == null)
            {
                return NotFound();
            }

            Disc = disc;

            if (record is not null)
            {
                disc.Record = record;
            }

            Tracks = disc.Tracks.OrderBy(t => t.Number).ToList();

            return Page();
        }
    }
}
