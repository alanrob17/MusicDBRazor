using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Tracks
{
    public class CreateModel : PageModel
    {
        private readonly MusicDbContext _context;

        public CreateModel(MusicDbContext context)
        {
            _context = context;
        }

        public SelectList? DiscList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateDiscListAsync();
            return Page();
        }

        [BindProperty]
        public Track Track { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateDiscListAsync();
                return Page();
            }

            _context.Tracks.Add(Track);
            await _context.SaveChangesAsync();
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

            DiscList = new SelectList(list, "DiscId", "DisplayName");
        }
    }
}
