using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        protected readonly IApiClient _apiClient;
        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> Sessions { get; set; } = null!;

        public IEnumerable<(int Offset, DayOfWeek? DayofWeek)> DayOffsets { get; set; } = null!;

        public int CurrentDayOffset { get; set; }

        public bool IsAdmin { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public IndexModel(ILogger<IndexModel> logger, IApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public async Task OnGet(int day = 0)
        {
            IsAdmin = User.IsAdmin();
            CurrentDayOffset = day;

            var sessions = await _apiClient.GetSessionsAsync();

            var startDate = sessions.Min(s => s.StartTime?.Date);

            DayOffsets = sessions.Select(s => s.StartTime?.Date)
                                  .Distinct()
                                  .OrderBy(d => d)
                                  .Select(day => ((int)Math.Floor((day!.Value - startDate)?.TotalDays ?? 0),
                                                  day?.DayOfWeek))
                                  .ToList();

            var filterDate = startDate?.AddDays(day);

            Sessions = sessions.Where(s => s.StartTime?.Date == filterDate)
                                .OrderBy(s => s.TrackId)
                                .GroupBy(s => s.StartTime)
                                .OrderBy(g => g.Key);
        }
    }
}
