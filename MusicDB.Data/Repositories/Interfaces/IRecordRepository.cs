using MusicDB.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Data.Repositories.Interfaces
{
    public interface IRecordRepository
    {
        /// <summary>
        /// Executes up_GetFaultyFieldAlbums with the supplied search term, applies
        /// in-memory paging, and returns the current page together with the total row count.
        /// </summary>
        Task<(IReadOnlyList<FaultyRecordFieldTag> Items, int TotalCount)> GetFaultyFieldAlbumsAsync(int page, int pageSize);

    }
}
