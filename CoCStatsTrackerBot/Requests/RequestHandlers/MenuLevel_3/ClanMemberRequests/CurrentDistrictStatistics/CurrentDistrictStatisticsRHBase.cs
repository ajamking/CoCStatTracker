using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class CurrentDistrictStatisticsRHBase : BaseRequestHandler
{
    public CurrentDistrictStatisticsRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevel.CurrentDistrictStatistics3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var lastRaidUi = GetFromDbQueryHandler.GetLastRaidUi(parameters.LastClanTagMessage);

            var answer = CurrentStatisticsFunctions.GetDistrictStatistics(lastRaidUi, parameters.DistrictType);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}