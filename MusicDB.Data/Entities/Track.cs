using System;
using System.Collections.Generic;

namespace MusicDB.Data.Entities;

public partial class Track
{
    public int TrackId { get; set; }

    public int DiscId { get; set; }

    public int? DiscNumber { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public int? Recorded { get; set; }

    public string? Length { get; set; }

    public TimeOnly? Duration { get; set; }

    public int? Bits { get; set; }

    public int? BitRate { get; set; }

    public int? AudioSampleRate { get; set; }

    public int? AudioChannels { get; set; }

    public string? Media { get; set; }

    public string? Album { get; set; }

    public string? Artist { get; set; }

    public string? Field { get; set; }

    public int? Number { get; set; }

    public string? Folder { get; set; }

    public virtual Disc Disc { get; set; } = null!;
}
