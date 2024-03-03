using CoCStatsTracker;
using CoCStatsTrackerBot.BotMenues;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class LeaderClanMembersRH : BaseRequestHandler
{
    public LeaderClanMembersRH()
    {
        Header = "Список членов клана";
        HandlerMenuLevel = MenuLevel.LeaderTgGroupCustomize2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            var answer = new StringBuilder(500);

            if (string.IsNullOrEmpty(parameters.LastClanTagToMerge))
            {
                answer.AppendLine(StylingHelper.MakeItStyled("Для получения списка членов клана необходимо выбрать редактируемый клан. Сделать это можно" +
                    " на предыдущем уровне меню.", UiTextStyle.Default));

                ResponseSender.SendAnswer(parameters, true, answer.ToString());

                return;
            }

            answer.AppendLine(StylingHelper.MakeItStyled($"Члены клана {parameters.LastClanTagToMerge}, которым можно переприсвоить @Username\n", UiTextStyle.Header));

            answer.AppendLine(StylingHelper.MakeItStyled("Формат: Ник - Тег - ﴾ Юзернейм ﴿\n\nНики выводятся для удобства навигации. \nПомните, что " +
                "для изменения юзернеймов вводить их нужно в формате:  #12345678-@username без лишних пробелов.\n", UiTextStyle.Default));

            var clanMembers = GetFromDbQueryHandler.GetAllClanMembersUi(parameters.LastClanTagToMerge);

            foreach (var member in clanMembers)
            {
                var userName = "Нет";

                if (!string.IsNullOrEmpty(member.TelegramUserName))
                {
                    userName = member.TelegramUserName;
                }

                answer.AppendLine(StylingHelper.MakeItStyled($"{member.Name.RemoveInvalidSymbols()} - {member.Tag} - ﴾ {userName} ﴿", UiTextStyle.Name));
            }

            ResponseSender.SendAnswer(parameters, true, answer.ToString());

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