using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderUpdateLastWarRH : BaseRequestHandler
{
    public LeaderUpdateLastWarRH()
    {
        Header = "Обновить последнюю войну";
        HandlerMenuLevel = MenuLevel.LeaderUpdateMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            UpdateDbCommandHandler.UpdateCurrentClanWar(parameters.LastClanTagToMerge);

            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Операция успешна, информация о последней войне обновлена", UiTextStyle.Default));
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