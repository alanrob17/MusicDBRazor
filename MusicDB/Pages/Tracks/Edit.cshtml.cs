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

        /// <summary>
        /// Receives the HTML time input value ("HH:mm:ss") and is manually
        /// converted to <see cref="TimeOnly"/> before saving.
        /// </summary>
        [BindProperty]
        public string? DurationString { get; set; }

        public SelectList? DiscList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var track = await _context.Tracks.FirstOrDefaultAsync(t => t.TrackId == id);

            if (track is null)
                return NotFound();

            Track = track;
            DurationString = Track.Duration?.ToString("hh\\:mm\\:ss");
            await PopulateDiscListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Parse the duration string submitted by the HTML time input ("HH:mm:ss")
            // into a TimeOnly value before the ModelState validity check.
            if (!string.IsNullOrWhiteSpace(DurationString) &&
                TimeOnly.TryParse(DurationString, out var parsedDuration))
            {
                Track.Duration = parsedDuration;
            }
            else
            {
                Track.Duration = null;
            }

            // Remove fields that are not submitted by the form so they don't
            // block validation: Duration is set above; Disc is a navigation property.
            ModelState.Remove("Track.Duration");
            ModelState.Remove("Track.Disc");

            if (!ModelState.IsValid)
            {
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
