namespace MusicDB.Data.Models;

/// <summary>
/// DTO that maps to the result set returned by the adm_GetArtistTotals stored procedure.
/// </summary>
public class ArtistTotals
{
    public int ArtistId { get; set; }
    public string? Name { get; set; }
    public int TotalDiscs { get; set; }
    public int TotalTracks { get; set; }
    public string? TotalLengthFormatted { get; set; }
}
