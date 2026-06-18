using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Discs
{
    public class DetailsModel : PageModel
    {
        private readonly MusicDbContext _context;

        public DetailsModel(MusicDbContext context)
        {
            _context = context;
        }

        public Disc Disc { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var disc = await _context.Discs
                .Include(d => d.Record)
                .FirstOrDefaultAsync(d => d.DiscId == id);

            if (disc is null)
                return NotFound();

            Disc = disc;
            return Page();
        }
    }
}
