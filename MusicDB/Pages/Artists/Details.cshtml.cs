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
    public class DetailsModel : PageModel
    {
        private readonly MusicDB.Data.MusicDbContext _context;

        public DetailsModel(MusicDB.Data.MusicDbContext context)
        {
            _context = context;
        }

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
    }
}
