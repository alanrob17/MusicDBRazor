using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Data.Models
{
    /// <summary>
    /// DTO that maps to the result set returned by the up_GetBriefRecordListByYear stored procedure.
    /// Property names match the SP column names exactly.
    /// </summary>
    public class BriefRecord
    {
        public int ArtistId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ArtistName { get; set; }
        public int RecordId { get; set; }
        public string? Album { get; set; }
        public int Recorded { get; set; }
        public int Discs { get; set; }
        public string? Length { get; set; }
        public string? DiscName { get; set; }
        public int DiscNumber { get; set; }
    }
}
