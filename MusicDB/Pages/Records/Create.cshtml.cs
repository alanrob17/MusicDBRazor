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
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(MusicDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
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
            ModelState.Remove("Record.Artist");

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .Select(e => $"{e.Key}: {string.Join(", ", e.Value!.Errors.Select(err => err.ErrorMessage))}");
                _logger.LogWarning("Record creation validation failed. Errors: {Errors}", string.Join("; ", errors));

                ArtistList = new SelectList(
                    await _context.Artists.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync(),
                    "ArtistId", "Name");
                return Page();
            }

            _context.Records.Add(Record);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully created Record with id {RecordId} ({RecordName})", Record.RecordId, Record.Name);
            return RedirectToPage("./Index");
        }
    }
}
