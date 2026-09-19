using Azure;
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

        /// <inheritdoc/>
        public async Task<(IReadOnlyList<BriefRecord> Items, int TotalCount)> GetRecordsByYearAsync(
            int year, string? artistName, int page, int pageSize)
        {
            var pYear = new SqlParameter("@Recorded", year);
            var pArtistName = new SqlParameter("@ArtistName", (object?)artistName ?? DBNull.Value);

            var all = await _context.Database
                .SqlQuery<BriefRecord>($"EXEC up_GetRecordsByYear @Recorded={pYear}, @ArtistName={pArtistName}")
                .ToListAsync();

            int totalCount = all.Count;

            var items = all
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .AsReadOnly();

            return (items, totalCount);
        }

        public async Task<(IReadOnlyList<SingleTrackAlbum> items, int TotalCount)> GetSingleTrackAlbumsAsync(int page, int pageSize)
        {
            var all = await _context.Database
                        .SqlQuery<SingleTrackAlbum>($"EXEC adm_GetAlbumsWithOneTrack")
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
