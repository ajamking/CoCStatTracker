using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class LeaderGuideLineRH : BaseRequestHandler
{
    public LeaderGuideLineRH()
    {
        Header = "Руководство";
        HandlerMenuLevel = MenuLevel.LeaderTgGroupCustomize2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var answer = new StringBuilder(500);

            answer.AppendLine(StylingHelper.MakeItStyled("Краткое руководство по добавлению юзернеймов членам клана\n", UiTextStyle.Header));

            answer.AppendLine(StylingHelper.MakeItStyled("Для чего это нужно?", UiTextStyle.TableAnnotation));

            answer.AppendLine(StylingHelper.MakeItStyled("Если вы добавлии бота в свой групповой чат и уведомили об этом админа " +
                "(передав ему ChatId вашего канала), то по умолчанию бот будет время от времени посылать в чат статистику текущих кв и рейдов. " +
                "В этой статистике, как правило, есть информация об игроках, не проведших атаки. Бот знает их теги и ники, но не знает их юзернймов из ТГ. " +
                "Если их добавить - сообщения от бота станут более полезными, т.к. он фактически сможет тегать непроатаковавших игроков.\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("Что такое юзернейм?", UiTextStyle.TableAnnotation));

            answer.AppendLine(StylingHelper.MakeItStyled("Юзернейм, он же имя пользователя - один из уникальных идентификаторов аккаунта в телеграмме. Начинается с символа @." +
                " Юзернейм не является обязательным параметром при создании аккаунта, поэтому имейте ввиду, что не у всех пользователей он есть.\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("Как добавить в бота юзернеймы игроков своего клана?", UiTextStyle.TableAnnotation));

            answer.AppendLine(StylingHelper.MakeItStyled("Для начала вам нужно выбрать один из доступных вам кланов, игрокам которого вы хотите изменить/добавить юзернеймы." +
                " Это делается на предыдущем этапе меню.\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("Затем нужно ввести одну или несколько строк вида:", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("#12345678-@Мафиозник\n", UiTextStyle.Header));

            answer.AppendLine(StylingHelper.MakeItStyled("Лишних пробелов быть не должно. Можно вводить несколько строк в одном сообщении.", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("Для удобства - можете использовать кнопку Список членов клана\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("После получения сообщения вышеописанного формата бот попытается найти указанных игроков по тегам" +
                " и присвоить им новые юзернеймы.\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("Если ранее у игрока не было юзернейма - он будет добавлен, если уже был - изменится.\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("Имейте ввиду, что бот регулярно обновляет списки членов клана, удаляя записи о покинувших клан игроках." +
                " Он не знает ушел игрок навсегда или вернется через пару часов. Поэтому если бывший соклан вновь вернется - юзернейм ему придется проставлять заново.\n", UiTextStyle.Subtitle));

            answer.AppendLine(StylingHelper.MakeItStyled("Как подключить бота в группу?\n", UiTextStyle.Header));

            answer.AppendLine(StylingHelper.MakeItStyled("Бота можно добавить в группу клана ТГ, это позволит вам получать регулярные полезные рассылки - уведомление о " +
                "приближающемся конце войны, итоги рейдов и т.д. Также у вас появистя возможность вызывать в групповом чате другие команды бота, начинающиеся со /group_ " +
                "Все остальные команды в групповых чатах недоступны.\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("Чтобы сделать это необходимо:" +
                "\n1. Добавить бота в группу и вызвать функцию /group_get_chat_id" +
                "\n2. Скопировать определенный ботом ChatId (скорее всего это будет последовательность цифр типа: -1234567890)" +
                "\n3. Ввести его в личный чат с ботом добавив спереди символ *, то есть в итоге сообщение должно иметь вид: *-1234567890" +
                "\n4. Включить рассылку, нажав на соответствующую кнопку из меню. Если она вам надоест - выключить можно в любое время.\n", UiTextStyle.Default));

            answer.AppendLine(StylingHelper.MakeItStyled("Имейте ввиду следующее:" +
                "\n1. В чатах с темами бот может отправлять сообщения лишь в основную тему." +
                "\n2. Бот может приостанавливать работу на технический перерыв, в это время рассылки поступать не будут." +
                "\n3. Если вы нажмете кнопку включить рассылку без простановки ChatId - операция завершится успешно, но рассылка приходить не будет, потому что некуда." +
                "\n4. Не нужно давать боту права администратора, рассылку он сможет проводить и без этого.", UiTextStyle.Subtitle));

            ResponseSender.SendAnswer(parameters, true, answer.ToString());

        }
        catch (NotFoundException)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}