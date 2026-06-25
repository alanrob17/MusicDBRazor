using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace MusicDB.Pages.Records
{
    public class MusicModel : PageModel
    {
        public IActionResult OnGetPlay()
        {
            // Get all MP3 files from your music folder
            string musicFolder = @"G:\Music\Library\Bert Jansch\1969 - Birthday Blues\";

            if (!Directory.Exists(musicFolder))
            {
                return NotFound("Music folder not found");
            }

            // Get all MP3 files
            string[] musicFiles = Directory.GetFiles(musicFolder, "*.flac");

            if (musicFiles.Length == 0)
            {
                return NotFound("No music files found");
            }

            // Create M3U content dynamically
            StringBuilder m3uContent = new StringBuilder();
            m3uContent.AppendLine("#EXTM3U"); // M3U header

            foreach (string file in musicFiles)
            {
                // Use forward slashes for compatibility
                string cleanPath = file.Replace(@"\", "/");
                m3uContent.AppendLine(cleanPath);
            }

            // Convert to bytes for download
            byte[] fileBytes = Encoding.UTF8.GetBytes(m3uContent.ToString());

            // Return as M3U file
            return File(fileBytes, "audio/x-mpegurl", "_playlist.m3u");
        }
    }
}
