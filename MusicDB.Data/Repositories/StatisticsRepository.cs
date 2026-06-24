using Azure;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Data.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly MusicDbContext _context;

        public StatisticsRepository(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyList<TotalTime> TotalTimes, int TotalCount)> GetTotalAlbumTimeAsync()
        {
            var all = await _context.Database
                .SqlQuery<TotalTime>($"EXEC adm_CalculateTotalAlbumTime")
                .ToListAsync();

            int totalCount = all.Count;

            // TODO: Reformat TotalLengthFormatted here

            return (all, totalCount);
        }
    }
}
