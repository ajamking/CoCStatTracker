using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class DeveloperAddClanToBlackListRH : BaseRequestHandler
{
    public DeveloperAddClanToBlackListRH()
    {
        Header = "Добавить клан в ЧС";
        HandlerMenuLevel = MenuLevel.DeveloperMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            if (!string.IsNullOrEmpty(parameters.LastClanTagToMerge))
            {
                UpdateDbCommandHandler.ResetClanIsBlasckListProperty(parameters.LastClanTagToMerge, true);

                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Клан внесен в ЧС.", UiTextStyle.Default));
            }
            else
            {
                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Тег для изменяемого клана не проставлен.", UiTextStyle.Default));
            }
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