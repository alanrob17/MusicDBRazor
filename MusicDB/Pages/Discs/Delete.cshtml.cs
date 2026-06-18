using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Discs
{
    public class DeleteModel : PageModel
    {
        private readonly MusicDbContext _context;

        public DeleteModel(MusicDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Disc Disc { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disc = await _context.Discs
                .Include(d => d.Record)
                .FirstOrDefaultAsync(m => m.DiscId == id);

            if (disc is not null)
            {
                Disc = disc;
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

            var disc = await _context.Discs.FindAsync(id);
            if (disc != null)
            {
                Disc = disc;
                _context.Discs.Remove(Disc);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
