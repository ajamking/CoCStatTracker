using CoCStatsTracker;
using CoCStatsTrackerBot.Requests;
using System.Text;

namespace CoCStatsTrackerBot;

public static class TagsConditionChecker
{
    public static void SendMemberTagMessageIsEmpty(BotUserRequestParameters parameters)
    {
        var answer = new StringBuilder(StylingHelper.MakeItStyled("Для получения информации об отслеживаемом игроке:\n\n", UiTextStyle.Header));

        answer.Append(StylingHelper.MakeItStyled("Введите тег отслеживаемого игрока в формате ", UiTextStyle.Default));

        answer.Append(StylingHelper.MakeItStyled("#123456789", UiTextStyle.Header));

        answer.AppendLine(StylingHelper.MakeItStyled(", а затем выберите интересующий пункт из меню.", UiTextStyle.Default));

        answer.Append(StylingHelper.MakeItStyled("\nСписок всех отслеживаемых членов клана можно получить в ", UiTextStyle.Default));

        answer.AppendLine(StylingHelper.MakeItStyled("Клан ➙ Члены клана.", UiTextStyle.Subtitle));

        answer.Append(StylingHelper.MakeItStyled("\nЕсли вы хотите ознакомиться со статистикой другого игрока - можете ввести новый тег в чат ", UiTextStyle.Default));

        answer.Append(StylingHelper.MakeItStyled("в любое время.", UiTextStyle.TableAnnotation));

        ResponseSender.SendAnswer(parameters, true, answer.ToString());
    }

    public static void SendClanTagMessageIsEmpty(BotUserRequestParameters parameters)
    {
        var answer = new StringBuilder(StylingHelper.MakeItStyled("Для получения информации об отслеживаемом клане:\n\n", UiTextStyle.Header));

        answer.Append(StylingHelper.MakeItStyled("Введите тег клана клана из представленных ниже (можно скопировать и вставить), а затем выберите пункт из меню.\n\n", UiTextStyle.Default));

        var allowedClansStr = new StringBuilder(StylingHelper.MakeItStyled("Доступные отслеживаемые кланы:\n\n", UiTextStyle.Subtitle));

        var blockedClansStr = new StringBuilder(StylingHelper.MakeItStyled("\nНедоступные отслеживаемые кланы:\n\n", UiTextStyle.Subtitle));

        var beautifulDveider = StylingHelper.MakeItStyled(BeautyIcons.BeautyClansDeviderString, UiTextStyle.Default);

        var trackedClans = GetFromDbQueryHandler.GetAllTrackedClans();

        var allowedClansCount = trackedClans.Where(x => x.IsInBlackList == false).ToList().Count;

        var blockedClansCount = trackedClans.Where(x => x.IsInBlackList == true).ToList().Count;

        var allowedCounter = 0;

        var blockedCounter = 0;

        foreach (var clan in trackedClans)
        {
            if (clan.IsInBlackList is false)
            {
                allowedCounter++;

                allowedClansStr.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag}", UiTextStyle.Name));

                if (allowedCounter < allowedClansCount)
                {
                    allowedClansStr.AppendLine(beautifulDveider);
                }
            }
            else
            {
                blockedCounter++;

                blockedClansStr.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag}", UiTextStyle.Name));

                if (blockedCounter < blockedClansCount)
                {
                    blockedClansStr.AppendLine(beautifulDveider);
                }
            }
        }

        answer.Append(allowedClansStr);

        if (blockedClansCount > 0)
        {
            answer.Append(blockedClansStr);
        }

        answer.Append(StylingHelper.MakeItStyled("\nЕсли вы хотите получить информацию о другом отслеживаемом клане - " +
            "просто введите в чат его тег ", UiTextStyle.Default));

        answer.AppendLine(StylingHelper.MakeItStyled("в любое время.", UiTextStyle.TableAnnotation));


        ResponseSender.SendAnswer(parameters, true, answer.ToString());
    }

    public static void SendAdminsKeyIsEmpty(BotUserRequestParameters parameters)
    {
        ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Для доступа к интерфейсу главы клана - введите токен авторизации." +
            "\n\nТокен авторизации выдается владельцем бота главе клана, дальше - на усмотрение главы.", UiTextStyle.Default));
    }

    public static bool CheckClanIsAllowedToMerge(BotUserRequestParameters parameters)
    {
        return GetFromDbQueryHandler.GetAllTrackedClans()
              .Where(x => x.Tag == parameters.Message.Text)
              .Where(x => x.IsInBlackList is false)
              .Any(x => x.AdminsKey == parameters.AdminsKey);
    }

    public static bool CheckClanWithAdminsKeyExistsInDb(this string adminsKey)
    {
        return GetFromDbQueryHandler.GetAllTrackedClans()
               .Any(x => x.AdminsKey == adminsKey);
    }

    public static bool CheckClanIsInBlackList(BotUserRequestParameters parameters)
    {
        if (string.IsNullOrEmpty(parameters.LastClanTagMessage))
        {
            return false;
        }

        return GetFromDbQueryHandler.GetTrackedClan(parameters.LastClanTagMessage).IsInBlackList;
    }
}