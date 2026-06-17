using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicDB.Data;
using MusicDB.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MusicDB.Pages.Records
{
    public class CreateModel : PageModel
    {
        private readonly MusicDbContext _context;

        public CreateModel(MusicDbContext context)
        {
            _context = context;
        }

        public SelectList? ArtistList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ArtistList = new SelectList(
                await _context.Artists.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync(),
                "ArtistId", "Name");
            return Page();
        }

        [BindProperty]
        public Record Record { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ArtistList = new SelectList(
                    await _context.Artists.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync(),
                    "ArtistId", "Name");
                return Page();
            }

            _context.Records.Add(Record);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
