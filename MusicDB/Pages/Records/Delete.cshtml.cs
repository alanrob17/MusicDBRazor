using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Records
{
    public class DeleteModel : PageModel
    {
        private readonly MusicDbContext _context;

        public DeleteModel(MusicDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Record Record { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var record = await _context.Records
                .Include(r => r.Artist)
                .FirstOrDefaultAsync(r => r.RecordId == id);

            if (record is null)
                return NotFound();

            Record = record;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var record = await _context.Records.FindAsync(id);
            if (record != null)
            {
                Record = record;
                _context.Records.Remove(Record);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
