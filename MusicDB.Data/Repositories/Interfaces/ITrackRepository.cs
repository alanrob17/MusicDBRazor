using MusicDB.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Data.Repositories.Interfaces
{
    public interface ITrackRepository
    {
        Task<(IReadOnlyList<SingleTrack> Items, int TotalCount)> GetSingleTrackAlbumsAsync();
    }
}
