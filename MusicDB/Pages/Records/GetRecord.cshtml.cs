using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data;
using MusicDB.Data.Entities;

namespace MusicDB.Pages.Records
{
    public class GetRecordModel : PageModel
    {
        private readonly MusicDbContext _context;

        public GetRecordModel(MusicDbContext context)
        {
            _context = context;
        }

        public Record Record { get; set; } = default!;
        public List<Disc> Discs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int recordId)
        {
            var record = await _context.Records
                .Include(r => r.Artist)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RecordId == recordId);

            if (record == null)
            {
                return NotFound();
            }

            Record = record;

            // Load discs and their tracks associated with the record
            Discs = await _context.Discs
                .Include(d => d.Tracks)
                .Where(d => d.RecordId == recordId)
                .OrderBy(d => d.DiscNumber)
                .AsNoTracking()
                .ToListAsync();

            // Sort tracks in memory for each disc by Track.Number
            foreach (var disc in Discs)
            {
                disc.Tracks = disc.Tracks.OrderBy(t => t.Number).ToList();
            }

            return Page();
        }
    }
}
