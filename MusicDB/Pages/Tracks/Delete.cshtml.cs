using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Tracks
{
    public class DeleteModel : PageModel
    {
        private readonly MusicDbContext _context;

        public DeleteModel(MusicDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Track Track { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var track = await _context.Tracks
                .Include(t => t.Disc)
                    .ThenInclude(d => d.Record)
                .FirstOrDefaultAsync(t => t.TrackId == id);

            if (track is null)
                return NotFound();

            Track = track;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var track = await _context.Tracks.FindAsync(id);

            if (track != null)
            {
                Track = track;
                _context.Tracks.Remove(Track);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
