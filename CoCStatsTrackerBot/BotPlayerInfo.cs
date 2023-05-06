using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoCStatsTracker;
using CoCApiDealer.UIEntities;

namespace CoCStatsTrackerBot;

public static class BotPlayerInfo
{
    public static string FullPlayerInfo(string playerTag, ICollection<TrackedClan> trackedClans)
    {
        var resultString = string.Empty;

        var member = new ClanMember();

        foreach (var clan in trackedClans)
        {
            if (clan.ClanMembers.FirstOrDefault(x => x.Tag == playerTag) != null)
            {
                member = clan.ClanMembers.First(x => x.Tag == playerTag);
                break;
            }

            else return "Игрока с таким тегом нет, введите тег заново";
        }

        var playerInfoUi = Mapper.MapToPlayerInfoUi(member);

        var dic = new Dictionary<string, string>()
        {
            { "Тег игрока", $"{playerInfoUi.Tag}" },
            //{ "Name", $"{playerInfoUi.Name}" },
            { "Тег клана", $"{playerInfoUi.ClanTag}" },
            // { "Название клана", $"{playerInfoUi.ClanName}" },
            { "Роль в клане", $"{playerInfoUi.RoleInClan}" },
            { "Уровено опыта", $"{playerInfoUi.ExpLevel}" },
            { "Уровень ТХ", $"{playerInfoUi.TownHallLevel}" },
            { "Уровень оружия", $"{playerInfoUi.TownHallWeaponLevel}" },
            { "Трофеи", $"{playerInfoUi.Trophies}" },
            { "Max Трофеи", $"{playerInfoUi.BestTrophies}" },
            { "Текущая лига", $"{playerInfoUi.League.Replace("League ", "")}" },
            { "Трофеи ДС", $"{playerInfoUi.VersusTrophies}" },
            { "Max Трофеи ДС", $"{playerInfoUi.BestVersusTrophies}" },
            { "Атак выиграно", $"{playerInfoUi.AttackWins}" },
            { "Защит выиграно", $"{playerInfoUi.DefenseWins}" },
            { "Участие в войне", $"{playerInfoUi.WarPreference}" },
            { "Войск отправлено", $"{playerInfoUi.DonationsSent}" },
            { "Войск получено", $"{playerInfoUi.DonationsRecieved}" },
            { "Звезд завоевано", $"{playerInfoUi.WarStars}" },
            { "Золото столицы", $"{playerInfoUi.TotalCapitalContributions}" },
        };

        var maxKeyLength = dic.Keys.Select(x => x.Length).Max();

        var maxValueLength = dic.Values.Select(x => x.Length).Max();

        var str = new StringBuilder();

        str.AppendLine($"ㅤИнформация об игроке** {playerInfoUi.Name}\nㅤИз клана {playerInfoUi.ClanName}");
        str.AppendLine($"``` |{"Параметр".PadRight(maxKeyLength)}|{CenteredString("Значение", maxValueLength)}|");

        str.AppendLine($" |{new string('-', maxKeyLength)}|{new string('-', maxValueLength)}|");

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(maxKeyLength)}|");

            str.AppendLine($"{CenteredString(item.Value.ToString(), maxValueLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string ShortPlayerInfo(string playerTag, ICollection<TrackedClan> trackedClans)
    {
        var resultString = string.Empty;

        var member = new ClanMember();

        foreach (var clan in trackedClans)
        {
            if (clan.ClanMembers.FirstOrDefault(x => x.Tag == playerTag) != null)
            {
                member = clan.ClanMembers.First(x => x.Tag == playerTag);
                break;
            }

            else return "Игрока с таким тегом нет, введите тег заново";
        }

        var shorrtPlayerInfoUi = Mapper.MapToShortPlayerInfoUi(member);

        var dic = new Dictionary<string, string>()
        {
            { "Тег игрока", $"{shorrtPlayerInfoUi.Tag}" },
            { "Участие в войне", $"{shorrtPlayerInfoUi.WarPreference}" },
            { "Войск отправлено", $"{shorrtPlayerInfoUi.DonationsSent}" },
            { "Войск получено", $"{shorrtPlayerInfoUi.DonationsRecieved}" },
            { "Звезд завоевано", $"{shorrtPlayerInfoUi.WarStars}" },
            { "Золото столицы", $"{shorrtPlayerInfoUi.TotalCapitalContributions}" },
        };

        var maxKeyLength = dic.Keys.Select(x => x.Length).Max();

        var maxValueLength = dic.Values.Select(x => x.Length).Max();

        var str = new StringBuilder();

        str.AppendLine($"ㅤИнформация об игроке** {shorrtPlayerInfoUi.Name}");
        str.AppendLine($"``` |{"Параметр".PadRight(maxKeyLength)}|{CenteredString("Значение", maxValueLength)}|");

        str.AppendLine($" |{new string('-', maxKeyLength)}|{new string('-', maxValueLength)}|");

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(maxKeyLength)}|");

            str.AppendLine($"{CenteredString(item.Value.ToString(), maxValueLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string CwCwlMembershipInfo(string playerTag, ICollection<TrackedClan> trackedClans)
    {
        var resultString = string.Empty;

        var member = new ClanMember();

        foreach (var clan in trackedClans)
        {
            if (clan.ClanMembers.FirstOrDefault(x => x.Tag == playerTag) != null)
            {
                member = clan.ClanMembers.First(x => x.Tag == playerTag);
                break;
            }

            else return "Игрока с таким тегом нет, введите тег заново";
        }

        var membershipUi = new List<CwCwlMembershipUi>();

        foreach (var warMember in member.WarMembership)
        {
            membershipUi.Add(Mapper.MapToCwCwlMembershipUi(warMember));
        }



        return string.Empty;
    }

    static string CenteredString(string s, int width)
    {
        if (s.Length >= width)
        {
            return s;
        }

        int leftPadding = (width - s.Length) / 2;
        int rightPadding = width - s.Length - leftPadding;

        return new string(' ', leftPadding) + s + new string(' ', rightPadding);
    }
}
