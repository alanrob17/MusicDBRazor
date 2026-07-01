namespace MusicDB.Data.Models;

/// <summary>
/// DTO that maps to the result set returned by the adm_GetArtistGuestTracks stored procedure.
/// Property names match the SP column names exactly.
/// </summary>
public class GuestArtistTrack
{
    public int ArtistId { get; set; }
    public string? ArtistName { get; set; }
    public int RecordId { get; set; }
    public int? Recorded { get; set; }
    public string? Field { get; set; }
    public string? RecordName { get; set; }
    public int? Discs { get; set; }
    public int DiscId { get; set; }
    public string? DiscName { get; set; }
    public int? DiscNumber { get; set; }
    public int TrackId { get; set; }
    public int? Number { get; set; }
    public string? FullTrackName { get; set; }
    public string? Length { get; set; }
}
