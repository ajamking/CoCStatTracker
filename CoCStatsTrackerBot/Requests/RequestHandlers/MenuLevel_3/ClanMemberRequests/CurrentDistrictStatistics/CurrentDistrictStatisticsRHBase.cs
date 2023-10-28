using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class CurrentDistrictStatisticsRHBase : BaseRequestHandler
{
    public CurrentDistrictStatisticsRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevels.CurrentDistrictStatistics3;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var raids = GetFromDbQueryHandler.GetAllRaids(parameters.LastTagMessage).OrderByDescending(x => x.StartedOn);

            var answer = CurrentStatisticsFunctions.GetDistrictStatistics(raids.First(), parameters.DistrictType);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
