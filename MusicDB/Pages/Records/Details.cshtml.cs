using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Records
{
    public class DetailsModel : PageModel
    {
        private readonly MusicDbContext _context;

        public DetailsModel(MusicDbContext context)
        {
            _context = context;
        }

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
    }
}
