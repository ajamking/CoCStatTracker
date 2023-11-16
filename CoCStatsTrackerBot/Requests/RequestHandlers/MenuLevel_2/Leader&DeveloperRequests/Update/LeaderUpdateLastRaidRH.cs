using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

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
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Обновить последний рейд невозможно, в базе нет сведений о рейдах этого клана. Попробуйте добавить, а не обновлять.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}