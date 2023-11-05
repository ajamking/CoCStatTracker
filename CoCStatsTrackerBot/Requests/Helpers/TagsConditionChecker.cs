using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using CoCStatsTrackerBot.Requests;
using System.Text;

namespace CoCStatsTrackerBot.Helpers;

public static class TagsConditionChecker
{
    public static void SendMemberTagMessageIsEmpty(BotUserRequestParameters parameters)
    {
        ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Введите тег отслеживаемого игрока в формате #123456789, а затем выберите пункт из меню.\n" +
            "\r\nСписок всех отслеживаемых членов клана можно получить в Клан/Члены клана", UiTextStyle.Default));
    }

    public static void SendClanTagMessageIsEmpty(BotUserRequestParameters parameters)
    {
        var answer = new StringBuilder(StylingHelper.MakeItStyled("Введите тег клана клана из представленных ниже (можно скопировать и вставить), а затем выберите пункт из меню.\n\n", UiTextStyle.Default));

        var allowedClans = new StringBuilder(StylingHelper.MakeItStyled("Доступные отслеживаемые кланы:\n", UiTextStyle.Subtitle));

        var blockedClans = new StringBuilder(StylingHelper.MakeItStyled("Недоступные отслеживаемые кланы:\n", UiTextStyle.Subtitle));

        foreach (var clan in GetFromDbQueryHandler.GetAllTrackedClans())
        {
            if (clan.IsInBlackList is false)
            {
                allowedClans.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag}", UiTextStyle.Name));
            }
            else
            {
                blockedClans.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag}", UiTextStyle.Name));
            }
        }

        answer.Append(allowedClans);
        answer.AppendLine();
        answer.Append(blockedClans);

        ResponseSender.SendAnswer(parameters, true, answer.ToString());
    }

    public static bool CheckMemberExistInDb(BotUserRequestParameters parameters)
    {
        var clanMembers = new List<ClanMemberUi>(300);

        foreach (var clan in GetFromDbQueryHandler.GetAllTrackedClans().Where(x => x.IsInBlackList is false))
        {
            clanMembers.AddRange(GetFromDbQueryHandler.GetAllClanMembers(clan.Tag));
        }

        return clanMembers.Any(x => x.Tag == parameters.Message.Text);
    }

    public static bool CheckClanExistInDb(BotUserRequestParameters parameters)
    {
        return GetFromDbQueryHandler.GetAllTrackedClans()
            .Where(x => x.Tag == parameters.Message.Text)
            .Where(x => x.IsInBlackList is false)
            .Any();
    }


    public static void SendAdminsKeyIsEmpty(BotUserRequestParameters parameters)
    {
        ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Для доступа к интерфейсу главы клана - введите токен авторизации.", UiTextStyle.Default));
    }

    public static bool CheckClanIsAllowedToMerge(BotUserRequestParameters parameters)
    {
        return GetFromDbQueryHandler.GetAllTrackedClans()
              .Where(x => x.Tag == parameters.Message.Text)
              .Where(x => x.IsInBlackList is false)
              .Any(x=>x.AdminsKey == parameters.AdminsKey);
    }

    public static bool CheckClanWithAdminsKeyExistsInDb(this string adminsKey)
    {
        return GetFromDbQueryHandler.GetAllTrackedClans()
               .Any(x => x.AdminsKey == adminsKey);
    }
}