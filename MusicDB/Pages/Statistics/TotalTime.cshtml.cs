using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicDB.Data.Models;
using MusicDB.Data.Repositories;
using MusicDB.Data.Repositories.Interfaces;

namespace MusicDB.Pages.Statistics
{
    public class TotalTimeModel : PageModel
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public TotalTimeModel(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public IEnumerable<TotalTime> TotalTimes { get; set; } = [];

        public async Task OnGetAsync()
        {
            var (items, totalCount) = await _statisticsRepository.GetTotalAlbumTimeAsync();

            if (totalCount > 0)
            {
                var item = items[0].TotalLengthFormatted;
                string[]? parts = item?.Split(':');
                var totalTime = string.Empty;
                if (parts.Length > 0)
                {
                    static string RemoveLeadingZero(string value) =>
                        value.StartsWith("0") ? value[1..] : value;

                    parts[1] = RemoveLeadingZero(parts[1]);
                    parts[2] = RemoveLeadingZero(parts[2]);

                    totalTime = $"{parts[0]} Days {parts[1]} Hours {parts[2]} Minutes";
                    items[0].TotalLengthFormatted = totalTime;
                }
            }
                TotalTimes = items;
        }
    }
}



