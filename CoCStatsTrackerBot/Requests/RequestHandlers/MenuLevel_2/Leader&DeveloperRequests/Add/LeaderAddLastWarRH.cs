using CoCStatsTracker;
using CoCStatsTracker.Items.Exceptions;
using CoCStatsTrackerBot.Menu;

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
            AddToDbCommandHandler.AddCurrentClanWarToClan(parameters.LastClanTagToMerge);

            var lastWar = GetFromDbQueryHandler.GetAllClanWars(parameters.LastClanTagToMerge).OrderByDescending(x => x.StartedOn).First();

            var answer = StylingHelper.MakeItStyled($"Операция успешна.\n" +
                $"Добавлена война: {lastWar.StartedOn.ToShortDateString()} - {lastWar.EndedOn.ToShortTimeString()} {lastWar.Result}", UiTextStyle.Default);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (AlreadyExistsException e)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Последняя война уже отслеживается, добавить ее невозможно, но можно обновить или удалить в других вкладках.", UiTextStyle.Default));
        }
        catch (FailedPullFromApiException e)
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