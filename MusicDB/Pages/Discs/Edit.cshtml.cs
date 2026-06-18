using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Discs
{
    public class EditModel : PageModel
    {
        private readonly MusicDbContext _context;

        public EditModel(MusicDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Disc Disc { get; set; } = default!;

        public SelectList? RecordList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var disc = await _context.Discs.FirstOrDefaultAsync(d => d.DiscId == id);

            if (disc is null)
                return NotFound();

            Disc = disc;
            RecordList = new SelectList(
                await _context.Records.OrderBy(r => r.Name).ToListAsync(),
                "RecordId", "Name", Disc.RecordId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                RecordList = new SelectList(
                    await _context.Records.OrderBy(r => r.Name).ToListAsync(),
                    "RecordId", "Name", Disc.RecordId);
                return Page();
            }

            _context.Attach(Disc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Discs.AnyAsync(d => d.DiscId == Disc.DiscId))
                    return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
