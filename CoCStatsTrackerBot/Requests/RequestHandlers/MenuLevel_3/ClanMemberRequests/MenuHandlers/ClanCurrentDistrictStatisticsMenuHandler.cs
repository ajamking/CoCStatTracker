using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanCurrentDistrictStatisticsMenuHandler : BaseRequestHandler
{
    public ClanCurrentDistrictStatisticsMenuHandler()
    {
        Header = "Статистика по районам";
        HandlerMenuLevel = MenuLevels.CurrentDistrictStatistics3;
    }
}
