using Microsoft.EntityFrameworkCore;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Data.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly MusicDbContext _context;

        public TrackRepository(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyList<SingleTrack> Items, int TotalCount)> GetSingleTrackAlbumsAsync()
        {
            var items = await _context.Database
                .SqlQuery<SingleTrack>($"EXEC adm_GetAlbumsWithOneTrack")
                .ToListAsync();

            int totalCount = items.Count;

            return (items, totalCount);
        }
    }
}
