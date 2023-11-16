using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class DeveloperSetClanChatIdRH : BaseRequestHandler
{
    public DeveloperSetClanChatIdRH()
    {
        Header = "Установить клану ChatId.";
        HandlerMenuLevel = MenuLevel.DeveloperMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            if (string.IsNullOrEmpty(parameters.LastClanTagToMerge) || string.IsNullOrEmpty(parameters.ClanChatIdToMerge))
            {
                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Тег изменяемого клана или изменяемый ChatId не проставлены.", UiTextStyle.Default));
            }
            else
            {
                UpdateDbCommandHandler.ResetClanChatId(parameters.LastClanTagToMerge, parameters.ClanChatIdToMerge);

                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("ChatId клана успешно переопределен.", UiTextStyle.Default));
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