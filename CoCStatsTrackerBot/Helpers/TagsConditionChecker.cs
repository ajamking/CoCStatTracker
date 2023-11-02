using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using CoCStatsTrackerBot.Requests;
using System.Diagnostics;
using System.Text;

namespace CoCStatsTrackerBot.Helpers;

public static class TagsConditionChecker
{
    public static void SendMemberTagMessageIsEmpty(RequestHadnlerParameters parameters)
    {
        ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Введите тег отслеживаемого игрока в формате #123456789, а затем выберите пункт из меню.\n" +
            "\r\nСписок всех отслеживаемых членов клана можно получить в Клан/Члены клана", UiTextStyle.Default));
    }

    public static void SendClanTagMessageIsEmpty(RequestHadnlerParameters parameters)
    {
        var str = new StringBuilder(StylingHelper.MakeItStyled("Введите тег клана клана из представленных ниже, а затем выберите пункт из меню.\n\n", UiTextStyle.Default));

        var clans = GetFromDbQueryHandler.GetAllTrackedClans();

        foreach (var clan in clans)
        {
            str.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag}", UiTextStyle.Name));
        }

        ResponseSender.SendAnswer(parameters, true, str.ToString());
    }

    public static bool CheckMemberExistInDb(RequestHadnlerParameters parameters)
    {
        var clanMembers = new List<ClanMemberUi>(150);

        var test = new Stopwatch();

        test.Start();

        foreach (var clan in GetFromDbQueryHandler.GetAllTrackedClans())
        {
            clanMembers.AddRange(GetFromDbQueryHandler.GetAllClanMembers(clan.Tag));
        }

        test.Stop();

        return clanMembers.Any(x => x.Tag == parameters.Message.Text);
    }

    public static bool CheckClanExistInDb(RequestHadnlerParameters parameters)
    {
        return GetFromDbQueryHandler.GetAllTrackedClans().Any(x => x.Tag == parameters.Message.Text);
    }

}
