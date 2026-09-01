namespace MusicDB.Data.Models;

/// <summary>
/// DTO that maps to the result set returned by the up_GetTracksByYear stored procedure.
/// Property names match the SP column names exactly.
/// </summary>
public class ArtistTracksByYear
{
    public int ArtistId { get; set; }
    public string? ArtistName { get; set; }
    public int RecordId { get; set; }
    public string? RecordName { get; set; }
    public int Recorded { get; set; }
    public int? Discs { get; set; }
    public string? DiscName { get; set; }
    public int? DiscNumber { get; set; }
    public int? Number { get; set; }
    public string? TrackName { get; set; }
    public string? Length { get; set; }
    public string? Field { get; set; }
}
