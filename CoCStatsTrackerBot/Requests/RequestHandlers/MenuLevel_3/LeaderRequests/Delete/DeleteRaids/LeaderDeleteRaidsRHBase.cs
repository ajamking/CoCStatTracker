using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteRaidsRHBase : BaseRequestHandler
{
    public LeaderDeleteRaidsRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevel.LeaderDeleteRaidsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var raidsCountBeforeRemove = GetFromDbQueryHandler.GetAllRaids(parameters.LastClanTagToMerge).Count;

            DeleteFromDbCommandHandler.DeleteClanRaids(parameters.LastClanTagToMerge, parameters.EntriesCount);

            var raidsCountAfterRemove = GetFromDbQueryHandler.GetAllRaids(parameters.LastClanTagToMerge).Count;

            var answer = StylingHelper.MakeItStyled($"Операция успешна.\n" +
                $"Записей удалено: {raidsCountBeforeRemove - raidsCountAfterRemove}", UiTextStyle.Default);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Все записи о рейдах удалены.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}