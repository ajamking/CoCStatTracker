using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class DeveloperDeleteClanRH : BaseRequestHandler
{
    public DeveloperDeleteClanRH()
    {
        Header = "Удалить клан из БД";
        HandlerMenuLevel = MenuLevel.DeveloperMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            if (!string.IsNullOrEmpty(parameters.LastClanTagToMerge))
            {
                DeleteFromDbCommandHandler.DeleteTrackedClan(parameters.LastClanTagToMerge);

                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Клан, успешно удален из БД.", UiTextStyle.Default));
            }
            else
            {
                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Тег для изменяемого клана не проставлен.", UiTextStyle.Default));
            }
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