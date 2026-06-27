using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MusicDB.Pages.Records
{
    public class Music2Model : PageModel
    {
        public List<PlaylistItem> Playlist { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Path to your local .m3u file on your hard drive
            string m3uPath = @"D:\Projects\MusicDBRazor\MusicDB\wwwroot\Music\_bob.m3u";
            string localMusicFolder = @"D:\Projects\MusicDBRazor\MusicDB\wwwroot\Music";

            if (System.IO.File.Exists(m3uPath))
            {
                var lines = await System.IO.File.ReadAllLinesAsync(m3uPath);
                PlaylistItem currentTrack = null;

                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                    {
                        if (trimmed.StartsWith("#EXTINF:"))
                        {
                            var commaIndex = trimmed.IndexOf(',');
                            if (commaIndex != -1)
                            {
                                currentTrack = new PlaylistItem { Title = trimmed.Substring(commaIndex + 1) };
                            }
                        }
                        continue;
                    }

                    // If the track is a relative path in the .m3u, resolve it to an absolute path
                    string absolutePath = Path.IsPathRooted(trimmed)
                        ? trimmed
                        : Path.GetFullPath(Path.Combine(Path.GetDirectoryName(m3uPath), trimmed));

                    if (currentTrack == null)
                    {
                        currentTrack = new PlaylistItem { Title = Path.GetFileNameWithoutExtension(absolutePath) };
                    }

                    // Transform the physical hard drive path into a web-accessible URL via the streaming endpoint
                    // Encode the absolute path in base64 for safe URL transmission
                    byte[] pathBytes = System.Text.Encoding.UTF8.GetBytes(absolutePath);
                    string encodedPath = Convert.ToBase64String(pathBytes);
                    currentTrack.FileUrl = $"/Stream?file={System.Net.WebUtility.UrlEncode(encodedPath)}";
                    Playlist.Add(currentTrack);
                    currentTrack = null;
                }
            }
        }
    }
}
