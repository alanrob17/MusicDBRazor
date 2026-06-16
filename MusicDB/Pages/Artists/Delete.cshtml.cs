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
    public class DeleteModel : PageModel
    {
        private readonly MusicDB.Data.MusicDbContext _context;

        public DeleteModel(MusicDB.Data.MusicDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Artist Artist { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists.FirstOrDefaultAsync(m => m.ArtistId == id);

            if (artist is not null)
            {
                Artist = artist;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                Artist = artist;
                _context.Artists.Remove(Artist);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
