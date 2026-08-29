using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Tracks
{
    public class DetailsModel : PageModel
    {
        private readonly MusicDbContext _context;

        public DetailsModel(MusicDbContext context)
        {
            _context = context;
        }

        public Track Track { get; set; } = default!;

        public Record? Record { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks
                .Include(t => t.Disc)
                    .ThenInclude(d => d.Record)
                        .ThenInclude(r => r.Artist)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TrackId == id);

            if (track is null)
            {
                return NotFound();
            }

            Track = track;
            Record = track.Disc?.Record;
            return Page();
        }
    }
}
