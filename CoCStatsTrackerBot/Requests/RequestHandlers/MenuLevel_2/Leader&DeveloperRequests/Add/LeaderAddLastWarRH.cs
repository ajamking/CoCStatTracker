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
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Выполнение операции на данный момент невозможно.\n" +
                "Бот сможет получить сведения о войнах клана лишь если история войн будет общедоступной.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}