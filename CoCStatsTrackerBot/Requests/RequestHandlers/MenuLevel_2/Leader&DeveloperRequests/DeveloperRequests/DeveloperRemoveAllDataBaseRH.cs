using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class DeveloperRemoveAllDataBaseRH : BaseRequestHandler
{
    public DeveloperRemoveAllDataBaseRH()
    {
        Header = "Снести всю базу";
        HandlerMenuLevel = MenuLevel.DeveloperMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            AddToDbCommandHandler.ResetDb();

            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("БД полностью очищена.", UiTextStyle.Default));
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