using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

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
            var raids = GetFromDbQueryHandler.GetAllRaids(parameters.LastClanTagMessage).OrderByDescending(x => x.StartedOn);

            var answer = CurrentStatisticsFunctions.GetDistrictStatistics(raids.First(), parameters.DistrictType);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
