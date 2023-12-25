using CoCStatsTracker;
using CoCStatsTracker.Items.Exceptions;
using CoCStatsTrackerBot.BotMenues;

namespace CoCStatsTrackerBot.Requests;

public class LeaderAddLastWarRH : BaseRequestHandler
{
    public LeaderAddLastWarRH()
    {
        Header = "Добавить последнюю войну";
        HandlerMenuLevel = MenuLevel.LeaderAddMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            try
            {
                AddToDbCommandHandler.AddCurrentClanWarToClan(parameters.LastClanTagToMerge);

                var lastClanWarUi = GetFromDbQueryHandler.GetLastClanWarUi(parameters.LastClanTagToMerge);

                var answer = StylingHelper.MakeItStyled($"Операция успешна.\n" +
                    $"Добавлена война: {lastClanWarUi.StartedOn.ToShortDateString()} - {lastClanWarUi.EndedOn.ToShortTimeString()}\nРезультат - {lastClanWarUi.Result}", UiTextStyle.Default);

                ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
            }
            catch (FailedPullFromApiException)
            {
                AddToDbCommandHandler.AddCurrentCwlClanWarsToClan(parameters.LastClanTagToMerge);

                var answer = StylingHelper.MakeItStyled($"Операция успешна.\n" +
                    $"Добавлены все недостающие войны с ЛВК.", UiTextStyle.Default);

                ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
            }
          
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (AlreadyExistsException)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Последняя война уже отслеживается, добавить ее невозможно, но можно обновить или удалить в других вкладках.", UiTextStyle.Default));
        }
        catch (FailedPullFromApiException)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Операция не может быть выполнена. Возможные причины:\n" +
                "1. История войн клана не общедоступна.\n" +
                "2. На данный момент клан не участвует в войне, а последняя была слишком давно.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}