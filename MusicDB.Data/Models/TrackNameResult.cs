namespace MusicDB.Data.Models;

/// <summary>
/// DTO that represents a track returned by a partial Track.Name search.
/// Properties are populated via EF Core LINQ projection.
/// </summary>
public class TrackNameResult
{
    public int TrackId { get; set; }
    public int ArtistId { get; set; }
    public string? ArtistName { get; set; }
    public int RecordId { get; set; }
    public string? RecordName { get; set; }
    public int? Recorded { get; set; }
    public string? Field { get; set; }
    public int DiscId { get; set; }
    public string? DiscName { get; set; }
    public int? DiscNumber { get; set; }
    public int? Number { get; set; }
    public string? TrackName { get; set; }
    public string? Length { get; set; }
}
