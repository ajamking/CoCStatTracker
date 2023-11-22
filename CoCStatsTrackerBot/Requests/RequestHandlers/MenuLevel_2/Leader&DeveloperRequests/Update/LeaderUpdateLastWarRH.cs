using CoCStatsTracker;
using CoCStatsTracker.Items.Exceptions;
using CoCStatsTrackerBot.BotMenues;

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
            catch (FailedPullFromApiException)
            {
                UpdateDbCommandHandler.UpdateCurrentCwlClanWars(parameters.LastClanTagToMerge);

                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Операция успешна, информация о ранее зафиксированных " +
                    $"войнах текущей лиги обновлена.", UiTextStyle.Default));
            }
            
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled($"Обновить последнюю войну невозможно, в базе нет сведений о войнах этого клана. Попробуйте добавить, а не обновлять.", UiTextStyle.Default));
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