using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Data.Models
{
    /// <summary>
    /// DTO that maps to the result set returned by the adm_GetAlbumsWithOneTrack stored procedure.
    /// Property names match the SP column names exactly.
    /// </summary>
    public class SingleTrackAlbum
    {
        public int RecordId { get; set; }
        public int Recorded { get; set; }
        public string? Field { get; set; }
        public string? ArtistName { get; set; }
        public string? RecordName { get; set; }
        public int TotalNoTracks { get; set; }
        public string? DiscName { get; set; }
        public string? FullTrackName { get; set; }
        public TimeOnly Duration { get; set; }
    }
}
