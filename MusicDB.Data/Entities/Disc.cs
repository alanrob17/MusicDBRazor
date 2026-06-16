using System;
using System.Collections.Generic;

namespace MusicDB.Data.Entities;

public partial class Disc
{
    public int DiscId { get; set; }

    public int RecordId { get; set; }

    public string? Name { get; set; }

    public string? SubTitle { get; set; }

    public int? DiscNumber { get; set; }

    public string? Length { get; set; }

    public TimeOnly? Duration { get; set; }

    public string? Folder { get; set; }

    public virtual Record Record { get; set; } = null!;

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
