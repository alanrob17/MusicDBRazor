using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace MusicDB.Pages.Tracks
{
    public class EditModel : PageModel
    {
        private readonly MusicDbContext _context;

        public EditModel(MusicDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Track Track { get; set; } = default!;

        public SelectList? DiscList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var track = await _context.Tracks.FirstOrDefaultAsync(t => t.TrackId == id);

            if (track is null)
                return NotFound();

            Track = track;
            await PopulateDiscListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                // TODO: For testing only, remove later. This is to help with debugging the model state errors.
                var track = Track;
                await PopulateDiscListAsync();
                return Page();
            }

            _context.Attach(Track).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Tracks.AnyAsync(t => t.TrackId == Track.TrackId))
                    return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }

        private async Task PopulateDiscListAsync()
        {
            var discs = await _context.Discs
                .Include(d => d.Record)
                .OrderBy(d => d.Record.Name)
                .ThenBy(d => d.Name)
                .ThenBy(d => d.DiscNumber)
                .ToListAsync();

            var list = discs.Select(d => new
            {
                d.DiscId,
                DisplayName = $"{d.Record?.Name ?? "Unknown Record"} — {d.Name ?? "Unnamed"} (Disc {d.DiscNumber})"
            });

            DiscList = new SelectList(list, "DiscId", "DisplayName", Track.DiscId);
        }
    }
}
