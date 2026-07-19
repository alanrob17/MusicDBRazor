using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Data.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly MusicDbContext _context;

        public RecordRepository(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<(IReadOnlyList<FaultyRecordFieldTag> Items, int TotalCount)> GetFaultyFieldAlbumsAsync(int page, int pageSize)
        {
            var all = await _context.Database
                .SqlQuery<FaultyRecordFieldTag>($"EXEC up_GetFaultyFieldAlbums")
                .ToListAsync();

            int totalCount = all.Count;

            var items = all
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .AsReadOnly();

            return (items, totalCount);
        }
    }
}
