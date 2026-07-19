using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Data.Models
{
    /// <summary>
    /// DTO that maps to the result set returned by the up_GetFaultyFieldAlbums stored procedure.
    /// </summary>
    public class FaultyRecordFieldTag
    {
        public int ArtistId { get; set; }
        public string? ArtistName { get; set; }
        public int RecordId { get; set; }
        public  string? Name { get; set; }
        public string? Field { get; set; }
        public int? Recorded { get; set; }
        public int? Discs { get; set; }
        public string? Length { get; set; }
    }
}

