using System;
using System.Collections.Generic;

namespace MusicDB.Data.Entities;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Name { get; set; }

    public string? Biography { get; set; }

    public string? Folder { get; set; }

    /// <summary>
    /// This field matches the ArtistId in the RocordDB dtabase. I can use this to do releated queries between the databases.
    /// </summary>
    public int? RecordArtistId { get; set; }

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}
