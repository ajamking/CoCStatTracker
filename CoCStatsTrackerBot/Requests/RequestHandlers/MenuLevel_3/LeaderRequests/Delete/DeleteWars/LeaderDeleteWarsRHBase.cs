using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderDeleteWarsRHBase : BaseRequestHandler
{
    public LeaderDeleteWarsRHBase()
    {
        Header = "Вызывается только через другие методы";
        HandlerMenuLevel = MenuLevel.LeaderDeleteRaidsMenu3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var clanWars = GetFromDbQueryHandler.GetAllClanWarsUi(parameters.LastClanTagToMerge).Count;

            DeleteFromDbCommandHandler.DeleteClanWars(parameters.LastClanTagToMerge, parameters.EntriesCount);

            var clanWarsAfterRemove = GetFromDbQueryHandler.GetAllClanWarsUi(parameters.LastClanTagToMerge).Count;

            var answer = StylingHelper.MakeItStyled($"Операция успешна.\n" +
                $"Записей удалено: {clanWars - clanWarsAfterRemove}", UiTextStyle.Default);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Все записи о войнах удалены.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}