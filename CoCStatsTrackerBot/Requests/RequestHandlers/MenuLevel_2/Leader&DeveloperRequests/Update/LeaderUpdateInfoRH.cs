using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class LeaderUpdateInfoRH : BaseRequestHandler
{
    public LeaderUpdateInfoRH()
    {
        Header = "Описание функций";
        HandlerMenuLevel = MenuLevel.LeaderUpdateMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var msg = new StringBuilder(StylingHelper.MakeItStyled("Описание доступных функций:\n\n", UiTextStyle.Header));

            msg.Append(StylingHelper.MakeItStyled("Характериситики клана ", UiTextStyle.Name));

            msg.AppendLine(StylingHelper.MakeItStyled("- обновляются лишь базовые характеристики, по типу уровня, лиги ЛВК, описания и т.д.\n" +
           "Все связанные с кланом записи - игроки, войны, рейды - сохраняются.\n", UiTextStyle.Default));

            msg.Append(StylingHelper.MakeItStyled("Игроки клана ", UiTextStyle.Name));

            msg.AppendLine(StylingHelper.MakeItStyled("- обновляется состав клана, а также базовые характериситики игроков, по типу уровня ТХ, боевых звезд, армии и т.д.\n" +
                "Все имеющиеся записи об участии игроков в рейдах и войнах - сохраняются. Операция занимает больше времени, чем остальные. \n", UiTextStyle.Default));

            msg.Append(StylingHelper.MakeItStyled("Обновить последний рейд ", UiTextStyle.Name));

            msg.AppendLine(StylingHelper.MakeItStyled("- обновляется вся информация о последнем рейде, включая участие игроков клана в рейде.\n", UiTextStyle.Default));

            msg.Append(StylingHelper.MakeItStyled("Обновить последнюю войну ", UiTextStyle.Name));

            msg.AppendLine(StylingHelper.MakeItStyled("- обновляется вся информация о последней войне, включая участие игроков клана в войне." +
                "Если клан участвует в ЛВК, то будут добавлены все недостающие войны текущей лиги.\n", UiTextStyle.Default));

            msg.Append(StylingHelper.MakeItStyled("Сезонные показатели ", UiTextStyle.Name));

            msg.AppendLine(StylingHelper.MakeItStyled("- старые сезонные показатели сбрасываются, фиксация прогресса игроков начинается заново.\n" +
                "Cразу после обновления этого параметра начальные показатели игроков будут равны текущим.\n", UiTextStyle.Default));

            ResponseSender.SendAnswer(parameters, true, msg.ToString());
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