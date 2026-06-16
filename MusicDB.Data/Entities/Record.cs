using System;
using System.Collections.Generic;

namespace MusicDB.Data.Entities;

public partial class Record
{
    public int RecordId { get; set; }

    public int ArtistId { get; set; }

    public string? Name { get; set; }

    public string? SubTitle { get; set; }

    public string? Field { get; set; }

    public int? Recorded { get; set; }

    public int? Discs { get; set; }

    public string? CoverName { get; set; }

    public string? Review { get; set; }

    public string? Folder { get; set; }

    public string? Length { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Disc> DiscsNavigation { get; set; } = new List<Disc>();
}
