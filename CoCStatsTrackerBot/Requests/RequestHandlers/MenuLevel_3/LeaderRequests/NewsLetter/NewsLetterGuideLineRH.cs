using CoCStatsTrackerBot.BotMenues;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class NewsLetterGuideLineRH : BaseRequestHandler
{
    public NewsLetterGuideLineRH()
    {
        Header = "Инструкция";
        HandlerMenuLevel = MenuLevel.LeaderNewsLetterCustomize3;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var answer = new StringBuilder(500);

            answer.AppendLine(StylingHelper.MakeItStyled("Краткая Инструкция по управлению поведением бота в вашей группе.\n", UiTextStyle.Header));

            answer.AppendLine(StylingHelper.MakeItStyled(
                $"Бот, подключенный к группе, может держать в курсе событий весь ваш клан! " +
                $"Вы можете настроить регулярные сообщения для своего клана как о КВ, так и о рейдах.\n" +
                $"В обоих случаях бот может отправлять сообщения о начале или конце события, а также посылать сообщение в выбранное вами время.\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled($"Пояснение кнопок:\n", UiTextStyle.TableAnnotation));

            answer.AppendLine(StylingHelper.MakeItStyled(
              $"1. Чтобы включить или выключить рассылку от бота полностью - нажмите Рассылка {BeautyIcons.RedCircleEmoji}/{BeautyIcons.GreenCircleEmoji}\n\n" +
              $"2. Кнопки Начало КВ, Конец КВ, Начало рейдов, Конец рейдов - действуют таким же образом, как переключатель.\n\n" +
              $"3. Чтобы установить собственное время для получения рассылки введите РЕЙДЫ-24 или КВ-24.\n" +
              $"Число 24 здесь для примера, можно вводить любое значение от 0 до 48, главное - без пробелов.\n\n" +
              $"4. Чтобы отключить рассылку по собственному времени, введите РЕЙДЫ-0 или КВ-0.\n\n" +
              $"5. Сообщение по установленному главой времени будет приходить в диапазоне +-30 минут.\n\n" +
              $"6. Если бот будет уходить на тех. перерыв, рассылка не дойдет.", UiTextStyle.Default));

            ResponseSender.SendAnswer(parameters, true, answer.ToString());


        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}