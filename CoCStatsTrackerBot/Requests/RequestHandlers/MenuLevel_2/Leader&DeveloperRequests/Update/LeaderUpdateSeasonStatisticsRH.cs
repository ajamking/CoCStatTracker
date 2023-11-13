using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderUpdateSeasonStatisticsRH : BaseRequestHandler
{
    public LeaderUpdateSeasonStatisticsRH()
    {
        Header = "Сбросить сезонные показатели";
        HandlerMenuLevel = MenuLevel.LeaderUpdateMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            UpdateDbCommandHandler.ResetLastClanMembersStaticstics(parameters.LastClanTagToMerge);

            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Операция успешна, сезонные показатели игроков сброшены, отсчет начат заново.", UiTextStyle.Default));
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