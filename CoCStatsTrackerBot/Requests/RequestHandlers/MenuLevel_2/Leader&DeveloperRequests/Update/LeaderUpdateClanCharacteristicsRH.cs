using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderUpdateClanCharacteristicsRH : BaseRequestHandler
{
    public LeaderUpdateClanCharacteristicsRH()
    {
        Header = "Характеристики клана";
        HandlerMenuLevel = MenuLevel.LeaderUpdateMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            UpdateDbCommandHandler.UpdateTrackedClanBaseProperties(parameters.LastClanTagToMerge, parameters.AdminsKey);

            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Операция успешна, основные показатели клана обновлены.", UiTextStyle.Default));

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