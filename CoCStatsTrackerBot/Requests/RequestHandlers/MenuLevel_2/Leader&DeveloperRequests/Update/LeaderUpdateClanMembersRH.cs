using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderUpdateClanMembersRH : BaseRequestHandler
{
    public LeaderUpdateClanMembersRH()
    {
        Header = "Игроков клана";
        HandlerMenuLevel = MenuLevel.LeaderUpdateMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            UpdateDbCommandHandler.UpdateTrackedClanClanMembers(parameters.LastClanTagToMerge);

            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Операция успешна, базовые показатели игроков обновлены", UiTextStyle.Default));
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