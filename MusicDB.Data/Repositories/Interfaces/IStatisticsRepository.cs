using MusicDB.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Data.Repositories.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<(IReadOnlyList<TotalTime> TotalTimes, int TotalCount)> GetTotalAlbumTimeAsync();
    }
}
