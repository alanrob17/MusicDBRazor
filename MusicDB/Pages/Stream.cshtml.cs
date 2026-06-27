using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MusicDB.Pages
{
    public class StreamModel : PageModel
    {
        public IActionResult OnGet(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                return BadRequest("File parameter is required");

            try
            {
                // URL-decode first (because it was URL-encoded in Music2.cshtml.cs)
                string urlDecodedFile = System.Net.WebUtility.UrlDecode(file);

                // Then base64-decode
                byte[] decodedBytes = Convert.FromBase64String(urlDecodedFile);
                string filePath = System.Text.Encoding.UTF8.GetString(decodedBytes);

                // Security: Ensure the file is within an authorized directory
                // Updated to use the wwwroot Music folder
                string musicBaseFolder = @"D:\Projects\MusicDBRazor\MusicDB\wwwroot\Music";
                string fullPath = Path.GetFullPath(filePath);
                string normalizedBasePath = Path.GetFullPath(musicBaseFolder);

                // Check if path is authorized
                if (!fullPath.StartsWith(normalizedBasePath, StringComparison.OrdinalIgnoreCase))
                    return Forbid("Access to this file is not allowed");

                // Check if file exists
                if (!System.IO.File.Exists(fullPath))
                    return NotFound($"File not found: {fullPath}");

                // Open the file
                var fileStream = System.IO.File.OpenRead(fullPath);

                string contentType = Path.GetExtension(fullPath).ToLower() switch
                {
                    ".flac" => "audio/flac",
                    ".mp3" => "audio/mpeg",
                    ".wav" => "audio/wav",
                    ".m4a" => "audio/mp4",
                    ".ogg" => "audio/ogg",
                    _ => "application/octet-stream"
                };

                var result = new FileStreamResult(fileStream, contentType)
                {
                    EnableRangeProcessing = true,
                    FileDownloadName = Path.GetFileName(fullPath)
                };

                return result;
            }
            catch (FormatException)
            {
                return StatusCode(500, "Invalid file parameter format");
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(403, "Permission denied accessing file");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error streaming file: {ex.Message}");
            }
        }
    }
}


