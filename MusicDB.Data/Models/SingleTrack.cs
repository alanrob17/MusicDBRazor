using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Data.Models
{
    public class SingleTrack
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
