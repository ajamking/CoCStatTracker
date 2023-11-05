using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderUpdateLastRaidRH : BaseRequestHandler
{
    public LeaderUpdateLastRaidRH()
    {
        Header = "Обновить последний рейд";
        HandlerMenuLevel = MenuLevel.LeaderUpdateMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            UpdateDbCommandHandler.UpdateClanCurrentRaid(parameters.LastClanTagToMerge);

            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Операция успешна, информация о последнем рейде обновлена", UiTextStyle.Default));
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