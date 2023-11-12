using CoCStatsTracker;
using CoCStatsTracker.Items.Exceptions;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class LeaderUpdateLastWarRH : BaseRequestHandler
{
    public LeaderUpdateLastWarRH()
    {
        Header = "Обновить последнюю войну";
        HandlerMenuLevel = MenuLevel.LeaderUpdateMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            try
            {
                UpdateDbCommandHandler.UpdateCurrentClanWar(parameters.LastClanTagToMerge);

                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Операция успешна, информация о последней войне обновлена", UiTextStyle.Default));
            }
            catch (FailedPullFromApiException e)
            {
                UpdateDbCommandHandler.UpdateCwlClanWars(parameters.LastClanTagToMerge);

                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Операция успешна, информация о ранее зафиксированных " +
                    $"войнах текущей лиги обновлена.", UiTextStyle.Default));
            }
            
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Обновить последнюю войну невозможно, в базе нет сведений о войнах этого клана. Попробуйте добавить, а не обновлять.", UiTextStyle.Default));
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