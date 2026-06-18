using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicDB.Data;
using MusicDB.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MusicDB.Pages.Discs
{
    public class CreateModel : PageModel
    {
        private readonly MusicDbContext _context;

        public CreateModel(MusicDbContext context)
        {
            _context = context;
        }

        public SelectList? RecordList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RecordList = new SelectList(
                await _context.Records.OrderBy(r => r.Name).ToListAsync(),
                "RecordId", "Name");
            return Page();
        }

        [BindProperty]
        public Disc Disc { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                RecordList = new SelectList(
                    await _context.Records.OrderBy(r => r.Name).ToListAsync(),
                    "RecordId", "Name");
                return Page();
            }

            _context.Discs.Add(Disc);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
