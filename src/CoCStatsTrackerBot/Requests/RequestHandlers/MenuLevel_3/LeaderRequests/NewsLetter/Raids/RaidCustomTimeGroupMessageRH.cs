using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class RaidCustomTimeGroupMessageRH : BaseRequestHandler
{
    public RaidCustomTimeGroupMessageRH()
    {
        Header = $"Рейды задать собственное время";
        HandlerMenuLevel = MenuLevel.LeaderNewsLetterCustomize3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var answer = new StringBuilder(500);

            if (!string.IsNullOrEmpty(parameters.LastClanTagToMerge))
            {
                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled(
                    $"Вы можете задать собственное время для рассылки о приближающемся конце рейдов.\n" +
                    "\nЧтобы сделать это введите сообщение в формате РЕЙДЫ-24." +
                    "\nЧисло 24 здесь для примера. Это время до конца рейдов, в которое бот отправит в ваш чат сообщение о текущем положении дел.\n" +
                    "\nЧисло или цифра после тире должны быть в пределах от 0 до 48." +
                    "\nЧтобы отключить рассылку по собственному времени введите сообщение РЕЙДЫ-0" +
                    "\nЛишних пробелов быть не должно.", UiTextStyle.Default));
            }
            else
            {
                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Для использования этой функции необходимо выбрать модерируемый клан.", UiTextStyle.Default));
            }
        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Либо такой клан не отслеживается, либо в нем нет участников.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}