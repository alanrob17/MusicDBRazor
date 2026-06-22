using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;

namespace MusicDB.Pages.Artists
{
    public class DeleteArtistModel : PageModel
    {
        private readonly MusicDbContext _context;

        public DeleteArtistModel(MusicDbContext context)
        {
            _context = context;
        }

        public SelectList ArtistSelectList { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int? SelectedArtistId { get; set; }

        public async Task OnGetAsync()
        {
            var artists = await _context.Artists
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .Select(a => new
                {
                    a.ArtistId,
                    DisplayName = (a.LastName ?? "") + ", " + (a.FirstName ?? "")
                })
                .ToListAsync();

            ArtistSelectList = new SelectList(artists, "ArtistId", "DisplayName", SelectedArtistId);
        }

        public IActionResult OnPost(int? selectedArtistId)
        {
            if (selectedArtistId == null)
            {
                return RedirectToPage();
            }

            return RedirectToPage("./Delete", new { id = selectedArtistId });
        }
    }
}
