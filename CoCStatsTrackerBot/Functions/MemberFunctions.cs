using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoCStatsTracker;
using CoCApiDealer.UIEntities;
using CoCStatsTracker.UIEntities;
using Telegram.Bot.Types;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Collections;

namespace CoCStatsTrackerBot;

public static class MemberFunctions
{
    public static string FullPlayerInfo(string playerTag, ICollection<TrackedClan> trackedClans)
    {

        var member = Helper.GetClanMember(trackedClans, playerTag);

        if (member == null)
        {
            return "Игрока с таким тегом нет, введите тег заново";
        }

        var playerInfoUi = Mapper.MapToPlayerInfoUi(member);

        var dic = new Dictionary<string, string>()
        {
            { "Тег игрока", $"{playerInfoUi.Tag}" },
            { "Тег клана", $"{playerInfoUi.ClanTag}" },
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

        str.AppendLine($"ㅤИнформация об игроке\nㅤ**{playerInfoUi.Name} \\- \\{playerInfoUi.Tag}\nㅤИз клана {playerInfoUi.ClanName} \\- \\{playerInfoUi.ClanTag}");
        str.AppendLine($"``` |{"Параметр".PadRight(maxKeyLength)}|{Helper.CenteredString("Значение", maxValueLength)}|");

        str.AppendLine($" |{new string('-', maxKeyLength)}|{new string('-', maxValueLength)}|");

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(maxKeyLength)}|");

            str.AppendLine($"{Helper.CenteredString(item.Value.ToString(), maxValueLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string ShortPlayerInfo(string playerTag, ICollection<TrackedClan> trackedClans)
    {
        var member = Helper.GetClanMember(trackedClans, playerTag);

        if (member == null)
        {
            return "Игрока с таким тегом нет, введите тег заново";
        }

        var shortPlayerInfoUi = Mapper.MapToShortPlayerInfoUi(member);

        var dic = new Dictionary<string, string>()
        {
            { "Тег игрока", $"{shortPlayerInfoUi.Tag}" },
            { "Участие в войне", $"{shortPlayerInfoUi.WarPreference}" },
            { "Войск отправлено", $"{shortPlayerInfoUi.DonationsSent}" },
            { "Войск получено", $"{shortPlayerInfoUi.DonationsRecieved}" },
            { "Звезд завоевано", $"{shortPlayerInfoUi.WarStars}" },
            { "Золото столицы", $"{shortPlayerInfoUi.TotalCapitalContributions}" },
        };

        var maxKeyLength = dic.Keys.Select(x => x.Length).Max();

        var maxValueLength = dic.Values.Select(x => x.Length).Max();

        var str = new StringBuilder();

        str.AppendLine($"ㅤКраткая информация об игроке\nㅤ**{shortPlayerInfoUi.Name} \\- \\{shortPlayerInfoUi.Tag}");
        str.AppendLine($"``` |{"Параметр".PadRight(maxKeyLength)}|{Helper.CenteredString("Значение", maxValueLength)}|");

        str.AppendLine($" |{new string('-', maxKeyLength)}|{new string('-', maxValueLength)}|");

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(maxKeyLength)}|");

            str.AppendLine($"{Helper.CenteredString(item.Value.ToString(), maxValueLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string MemberDrawMembership(string playerTag, ICollection<TrackedClan> trackedClans)
    {
        var member = Helper.GetClanMember(trackedClans, playerTag);

        if (member.DrawMemberships == null)
        {
            return "Этот игрок не участвует в розыгрыше";
        }

        var drawMembership = new DrawMembershipUi();

        foreach (var drawMember in member.DrawMemberships)
        {
            if (drawMember.PrizeDraw.EndedOn > DateTime.Now)
            {
                drawMembership = Mapper.MapToDrawMembershipUi(drawMember);
            }
            else
            {
                return "Этот игрок не участвует в текущем розыгрыше";
            }
        }

        var dic = new Dictionary<string, string>()
        {
            { "Начало", $"{drawMembership.Start}" },
            { "Конец", $"{drawMembership.End}" },
            { "Участник", $"{drawMembership.PlayersName}" },
            { "Тег", $"{drawMembership.PlayersTag}" },
            { "Очков", $"{drawMembership.DrawTotalScore}" },
            { "Позиция", $"{drawMembership.PositionInClan}" },
        };

        var maxKeyLength = dic.Keys.Select(x => x.Length).Max();

        var maxValueLength = dic.Values.Select(x => x.Length).Max();

        var str = new StringBuilder();

        str.AppendLine($"ㅤРозыгрыш в клане\nㅤ**{drawMembership.ClanName} \\- \\{drawMembership.ClanTag}\n");
        str.AppendLine($"``` |{"Параметр".PadRight(maxKeyLength)}|{Helper.CenteredString("Значение", maxValueLength)}|");

        str.AppendLine($" |{new string('-', maxKeyLength)}|{new string('-', maxValueLength)}|");

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(maxKeyLength)}|");

            str.AppendLine($"{Helper.CenteredString(item.Value.ToString(), maxValueLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string MemberCarmaHistory(string playerTag, ICollection<TrackedClan> trackedClans)
    {
        var maxPointsLenght = 4;

        try
        {
            var member = Helper.GetClanMember(trackedClans, playerTag);

            var carma = Mapper.MapToCarmaUi(member);

            if (carma.Activities.Count == 0)
            {
                return "У этого игрока пока нет зафиксированных активностей";
            }

            foreach (var activity in carma.Activities)
            {
                activity.Name = activity.Name.Substring(0, 10);
            }

            var maxActivityNameLength = carma.Activities.Select(x => x.Name.Length).Max();

            var maxEarnedPointsLength = carma.Activities.Select(x => x.EarnedPoints.Length).Max();

            if (maxEarnedPointsLength < maxPointsLenght)
            {
                maxEarnedPointsLength = maxPointsLenght;
            }

            var maxDateLength = carma.Activities.Select(x => x.UpdatedOn.Length).Max();

            var str = new StringBuilder();

            str.AppendLine($"ㅤИстория кармы игрока\nㅤ**{carma.PlayersName} \\- \\{carma.PlayersTag}\n" +
                $"ㅤ**Очки активностей влияют на карму**\n" +
                $"ㅤ**Карма учитывается в розыгрыше**\n");

            str.AppendLine($"``` " +
                $"|{"Активность".PadRight(maxActivityNameLength)}" +
                $"|{Helper.CenteredString("Очки", maxEarnedPointsLength)}" +
                $"|{Helper.CenteredString("Дата", maxDateLength)}|");

            str.AppendLine($" " +
                $"|{new string('-', maxActivityNameLength)}" +
                $"|{new string('-', maxEarnedPointsLength)}" +
                $"|{new string('-', maxDateLength)}|");

            foreach (var activity in carma.Activities)
            {
                str.Append($" |{activity.Name.PadRight(maxActivityNameLength)}|");

                str.Append($"{Helper.CenteredString(activity.EarnedPoints, maxEarnedPointsLength)}|");

                str.AppendLine($"{Helper.CenteredString(activity.UpdatedOn, maxDateLength)}|");
            }

            str.Append("```");

            return str.ToString();
        }

        catch (Exception e)
        {
            return "При считывании кармы игрока что-то пошло не так";
        }
    }

    public static string WarStatistics(string playerTag, ICollection<TrackedClan> trackedClans, int recordsCount)
    {
        try
        {
            var maxAttackLenght = 5;
            var maxOpponentLenght = 9;
            var maxDestructionPercent = 5;
            var maxStars = 5;

            var member = Helper.GetClanMember(trackedClans, playerTag);

            if (member.WarMemberships.Count == 0)
            {
                return "Этот игрок пока не принимал участия в войнах";
            }

            var sortedMemberships = member.WarMemberships.OrderBy(cw => cw.ClanWar.EndTime).ToList();

            var uiMemberships = new List<CwCwlMembershipUi>();

            foreach (var warMembership in sortedMemberships)
            {
                uiMemberships.Add(Mapper.MapToCwCwlMembershipUi(warMembership));
            }

            var str = new StringBuilder();

            foreach (var uiMembership in uiMemberships)
            {
                var counter = 0;

                str.AppendLine($"ㅤПоказатели игрока\nㅤ**{uiMembership.Name} \\- \\{uiMembership.Tag}\n" +
                    $"ㅤВ войне на стороне клана\n" +
                    $"ㅤ{uiMembership.ClanName} \\- \\{uiMembership.ClanTag} ``` \n" +
                    $"ㅤНачало войны \\- {uiMembership.StartedOn}\n" +
                    $"ㅤКонец войны \\- {uiMembership.EndedOn}\n" +
                    $"ㅤПозиция на карте \\- {uiMembership.MapPosition}\n\n" +
                    $"ㅤИнформация обороны:\n" +
                    $"ㅤЛучшее время противника \\- {uiMembership.BestOpponentsTime}\n" +
                    $"ㅤЛучший процент противника \\- {uiMembership.BestOpponentsPercent}\n" +
                    $"ㅤЛучшие звезды противника \\- {uiMembership.BestOpponentStars}\n\n" +
                    $"ㅤИнформация нападений:\n" +
                    $"ㅤАтака\\- Номер атаки по всему клану\n" +
                    $"ㅤПротивник\\- Позиция\\/Уровень ТХ противника```\n");

                str.AppendLine($"``` " +
                    $"|{Helper.CenteredString("Атака", maxAttackLenght)}" +
                    $"|{Helper.CenteredString("Противник", maxOpponentLenght)}" +
                    $"|{Helper.CenteredString("%", maxDestructionPercent)}" +
                    $"|{Helper.CenteredString("Звезд", maxStars)}|");

                str.AppendLine($" " +
                    $"|{new string('-', maxAttackLenght)}" +
                    $"|{new string('-', maxOpponentLenght)}" +
                    $"|{new string('-', maxDestructionPercent)}" +
                    $"|{new string('-', maxStars)}|");

                foreach (var attack in uiMembership.Attacks)
                {
                    str.Append($" |{Helper.CenteredString(attack.AttackOrder, maxAttackLenght)}|");

                    str.Append($"{Helper.CenteredString(attack.EnemyMapPosition + " / " + attack.EnemyTHLevel, maxOpponentLenght)}|");

                    str.Append($"{Helper.CenteredString(attack.DestructionPercent, maxDestructionPercent)}|");

                    str.AppendLine($"{Helper.CenteredString(attack.Stars, maxStars)}|");
                }

                str.Append("```\n");

                counter++;

                if (counter == recordsCount)
                {
                    break;
                }
            }

            return str.ToString();
        }

        catch (Exception e)
        {
            return "При считывании WarStatistics игрока что-то пошло не так";
        }
    }
}
