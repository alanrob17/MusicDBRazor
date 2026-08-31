using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;
using NuGet.Common;
using Serilog.Core;

namespace MusicDB.Pages.Records
{
    public class EditModel : PageModel
    {
        private readonly MusicDbContext _context;

        private readonly ILogger<EditModel> _logger;

        public EditModel(MusicDbContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Record Record { get; set; } = default!;

        public SelectList? ArtistList { get; set; }

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
            ArtistList = new SelectList(
                await _context.Artists.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync(),
                "ArtistId", "Name", Record.ArtistId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Record.Artist");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid. Returning to page with validation errors.");

                ArtistList = new SelectList(
                    await _context.Artists.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync(),
                    "ArtistId", "Name", Record.ArtistId);
                return Page();
            }

            _context.Attach(Record).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated Record with id {RecordId} ({RecordName})", Record.RecordId, Record.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Records.AnyAsync(r => r.RecordId == Record.RecordId))
                    return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
