using CoCStatsTracker;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public static class AdminsMessageHelper
{
    public static string GetTrackedClansStatementsMessage(bool isBotHolder, string adminsKey)
    {
        var answer = new StringBuilder();

        var trackedClans = GetFromDbQueryHandler.GetAllTrackedClans();

        var allowedTrackedClans = trackedClans;

        if (!isBotHolder)
        {
            allowedTrackedClans = trackedClans
                .Where(x => x.AdminsKey == adminsKey)
                .Where(x => x.IsInBlackList == false).ToList();
        }

        foreach (var clan in allowedTrackedClans)
        {
            var chatId = $"Айди чата: {(string.IsNullOrEmpty(clan.ClansTelegramChatId) ? "Не установлен" : clan.ClansTelegramChatId)}";

            var newsLetter = $"Рассылки: {(clan.RegularNewsLetterOn ? BeautyIcons.GreenCircleEmoji : BeautyIcons.RedCircleEmoji)}";

            var warStartMessage = $"КВ начало: {(clan.WarStartMessageOn ? BeautyIcons.GreenCircleEmoji : BeautyIcons.RedCircleEmoji)}";

            var warEndMessage = $"КВ конец: {(clan.WarEndMessageOn ? BeautyIcons.GreenCircleEmoji : BeautyIcons.RedCircleEmoji)}";

            var warCustomCircle = clan.WarTimeToMessageBeforeEnd != 0 ?
              $" за {clan.WarTimeToMessageBeforeEnd}ч. до конца {BeautyIcons.GreenCircleEmoji}" :
              $" за {clan.WarTimeToMessageBeforeEnd}ч. до конца {BeautyIcons.RedCircleEmoji}";

            var warCustomMessage = $"КВ свое: {warCustomCircle}";

            var raidStartMessage = $"Рейды начало: {(clan.RaidStartMessageOn ? BeautyIcons.GreenCircleEmoji : BeautyIcons.RedCircleEmoji)}";

            var raidEndMessage = $"Рейды конец: {(clan.RaidEndMessageOn ? BeautyIcons.GreenCircleEmoji : BeautyIcons.RedCircleEmoji)}";

            var raidCustomCircle = clan.RaidTimeToMessageBeforeEnd != 0 ?
            $" за {clan.RaidTimeToMessageBeforeEnd}ч. до конца {BeautyIcons.GreenCircleEmoji}" :
            $" за {clan.RaidTimeToMessageBeforeEnd}ч. до конца {BeautyIcons.RedCircleEmoji}";

            var raidCustomMessage = $"Рейды свое: {raidCustomCircle}";

            answer.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag}" +
                $"\n[ {chatId} ] - [ {newsLetter} ]" +
                $"\n[ {warStartMessage} ] [ {warEndMessage} ]\n[ {warCustomMessage} ]" +
                $"\n[ {raidStartMessage} ] [ {raidEndMessage} ]\n[ {raidCustomMessage} ]" +
                $"\n", UiTextStyle.Name));
        }

        return answer.ToString();
    }

    public static string GetOneTrackedClanStatement(string clanTag)
    {
        var answer = new StringBuilder();

        var clan = GetFromDbQueryHandler.GetTrackedClan(clanTag);

        var chatId = $"Айди чата: {(string.IsNullOrEmpty(clan.ClansTelegramChatId) ? "Не установлен" : clan.ClansTelegramChatId)}";

        var newsLetter = $"Рассылки: {(clan.RegularNewsLetterOn ? BeautyIcons.GreenCircleEmoji : BeautyIcons.RedCircleEmoji)}";

        var warStartMessage = $"КВ начало: {(clan.WarStartMessageOn ? BeautyIcons.GreenCircleEmoji : BeautyIcons.RedCircleEmoji)}";

        var warEndMessage = $"КВ конец: {(clan.WarEndMessageOn ? BeautyIcons.GreenCircleEmoji : BeautyIcons.RedCircleEmoji)}";

        var warCustomCircle = clan.WarTimeToMessageBeforeEnd != 0 ?
           $" за {clan.WarTimeToMessageBeforeEnd}ч. до конца {BeautyIcons.GreenCircleEmoji}" :
           $" за {clan.WarTimeToMessageBeforeEnd}ч. до конца {BeautyIcons.RedCircleEmoji}";

        var warCustomMessage = $"КВ свое: {warCustomCircle}";

        var raidStartMessage = $"Рейды начало: {(clan.RaidStartMessageOn ? BeautyIcons.GreenCircleEmoji : BeautyIcons.RedCircleEmoji)}";

        var raidEndMessage = $"Рейды конец: {(clan.RaidEndMessageOn ? BeautyIcons.GreenCircleEmoji : BeautyIcons.RedCircleEmoji)}";

        var raidCustomCircle = clan.RaidTimeToMessageBeforeEnd != 0 ?
        $" за {clan.RaidTimeToMessageBeforeEnd}ч. до конца {BeautyIcons.GreenCircleEmoji}" :
        $" за {clan.RaidTimeToMessageBeforeEnd}ч. до конца {BeautyIcons.RedCircleEmoji}";

        var raidCustomMessage = $"Рейды свое: {raidCustomCircle}";

        answer.AppendLine(StylingHelper.MakeItStyled($"{clan.Name} - {clan.Tag}" +
            $"\n[ {chatId} ] - [ {newsLetter} ]\n" +
            $"\n[ {warStartMessage} ] [ {warEndMessage} ]\n[ {warCustomMessage} ]\n" +
            $"\n[ {raidStartMessage} ] [ {raidEndMessage} ]\n[ {raidCustomMessage} ]" +
            $"\n", UiTextStyle.Name));

        return answer.ToString();
    }
}