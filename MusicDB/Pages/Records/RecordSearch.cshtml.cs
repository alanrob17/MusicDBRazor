using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Records
{
    public class RecordSearchModel : PageModel
    {
        private readonly MusicDbContext _context;

        public RecordSearchModel(MusicDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int? SelectedRecordId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public List<SelectListItem> RecordSelectList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // If the user has selected a record from the dropdown and clicked submit,
            // redirect them to the GetRecord page with the selected RecordId.
            if (SelectedRecordId.HasValue && SelectedRecordId.Value > 0)
            {
                return RedirectToPage("/Records/GetRecord", new { recordId = SelectedRecordId.Value });
            }

            // Populate the dropdown list with records, filtered if SearchString is specified
            var recordsQuery = _context.Records.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                recordsQuery = recordsQuery.Where(r => r.Name != null && r.Name.Contains(SearchString));
            }

            RecordSelectList = await recordsQuery
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Value = r.RecordId.ToString(),
                    Text = r.Name ?? "Unknown Record"
                })
                .ToListAsync();

            return Page();
        }
    }
}
