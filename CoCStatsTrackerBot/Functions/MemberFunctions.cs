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

        var defaultActivityLength = 10;

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
                if (activity.Name.Length > defaultActivityLength)
                {
                    activity.Name = activity.Name.Substring(0, defaultActivityLength);
                }
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

                if (counter == recordsCount || counter == member.WarMemberships.Count)
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

    public static string RaidStatistics(string playerTag, ICollection<TrackedClan> trackedClans, int recordsCount)
    {
        try
        {
            var maxAttackLenght = 2;
            var maxDistrictLenght = 15;
            var maxDestructionFrom = 3;
            var maxDestructionTo = 3;

            var member = Helper.GetClanMember(trackedClans, playerTag);

            if (member.RaidMemberships.Count == 0)
            {
                return "Этот игрок пока не принимал участия в рейдах";
            }

            var sortedMemberships = member.RaidMemberships.OrderBy(cw => cw.Raid.EndedOn).ToList();

            var uiMemberships = new List<RaidMembershipUi>();

            foreach (var raidMembership in sortedMemberships)
            {
                uiMemberships.Add(Mapper.MapToRaidMembershipUi(raidMembership));
            }

            var str = new StringBuilder();

            foreach (var uiMembership in uiMemberships)
            {
                var counter = 0;

                str.AppendLine($"ㅤПоказатели игрока\nㅤ**{uiMembership.Name} \\- \\{uiMembership.Tag}\n" +
                    $"ㅤВ рейдах на стороне клана\n" +
                    $"ㅤ{uiMembership.ClanName} \\- \\{uiMembership.ClanTag} ``` \n" +
                    $"ㅤНачало рейдов \\- {uiMembership.StartedOn}\n" +
                    $"ㅤКонец рйедов \\- {uiMembership.EndedOn}\n" +
                    $"ㅤВсего золота заработано \\- {uiMembership.TotalLoot}\n\n" +
                    $"ㅤИнформация об атаках: ```\n");

                str.AppendLine($"``` " +
                    $"|{Helper.CenteredString("No", maxAttackLenght)}" +
                    $"|{Helper.CenteredString("Район", maxDistrictLenght)}" +
                    $"|{Helper.CenteredString("%От", maxDestructionFrom)}" +
                    $"|{Helper.CenteredString("%До", maxDestructionTo)}|");

                str.AppendLine($" " +
                    $"|{new string('-', maxAttackLenght)}" +
                    $"|{new string('-', maxDistrictLenght)}" +
                    $"|{new string('-', maxDestructionFrom)}" +
                    $"|{new string('-', maxDestructionTo)}|");

                foreach (var attack in uiMembership.Attacks)
                {
                    if (attack.DistrictName.Length > maxDistrictLenght)
                    {
                        attack.DistrictName = attack.DistrictName.Substring(0, maxDistrictLenght);
                    }

                    var attackNumber = 1;

                    str.Append($" |{Helper.CenteredString(attackNumber.ToString(), maxAttackLenght)}|");

                    str.Append($"{Helper.CenteredString(attack.DistrictName, maxDistrictLenght)}|");

                    str.Append($"{Helper.CenteredString(attack.DestructionPercentFrom, maxDestructionFrom)}|");

                    str.AppendLine($"{Helper.CenteredString(attack.DestructionPercentTo, maxDestructionTo)}|");

                    attackNumber++;
                }

                str.Append("```\n");

                counter++;

                if (counter == recordsCount || counter == member.RaidMemberships.Count)
                {
                    break;
                }
            }

            return str.ToString();
        }

        catch (Exception e)
        {
            return "При считывании RaidStatistics игрока что-то пошло не так";
        }
    }

    public static string MembersArmyInfo(string playerTag, ICollection<TrackedClan> trackedClans, UnitType uniType)
    {
        try
        {
            var maxNameLength = 20;
            var maxLvlLength = 4;

            var member = Helper.GetClanMember(trackedClans, playerTag);

            if (member.Units.Count == 0)
            {
                return "Этот игрок пока не обзавелся юнитами";
            }

            var armyUi = Mapper.MapToArmyUi(member.Units);

            var chosenUnits = new List<TroopUi>();

            try
            {
                switch (uniType)
                {
                    case UnitType.Hero:
                        chosenUnits = armyUi.Heroes;
                        break;
                    case UnitType.SiegeMachine:
                        chosenUnits = armyUi.SiegeMachines;
                        break;
                    case UnitType.SuperUnit:
                        foreach (var unit in armyUi.SuperUnits)
                        {
                            if (unit.SuperTroopIsActivated == "True")
                            {
                                chosenUnits.Add(unit);
                            }
                        }
                        break;
                    case UnitType.Unit:
                        chosenUnits.AddRange(armyUi.Heroes);
                        chosenUnits.AddRange(armyUi.SiegeMachines);
                        chosenUnits.AddRange(armyUi.SuperUnits);
                        chosenUnits.AddRange(armyUi.Pets);
                        chosenUnits.AddRange(armyUi.Units);
                        break;

                    default:
                        return "Этот игрок пока не обзавелся юнитами такого типа";
                }
            }

            catch (Exception e)
            {
                return "Этот игрок пока не обзавелся юнитами такого типа";
            }


            var str = new StringBuilder();


            str.AppendLine($"ㅤВойска выбранного типа у игрока\nㅤ**{member.Name} \\- \\{member.Tag}\n");

            str.AppendLine($"``` " +
                $"|{Helper.CenteredString("Name", maxNameLength)}" +
                $"|{Helper.CenteredString("Lvl", maxLvlLength)}|");

            str.AppendLine($" " +
                $"|{new string('-', maxNameLength)}" +
                $"|{new string('-', maxLvlLength)}|");

            foreach (var unit in chosenUnits)
            {
                var name = unit.Name;

                if (unit.SuperTroopIsActivated == "true")
                {
                    name += "+";
                }

                str.Append($" |{Helper.CenteredString(name, maxNameLength)}|");

                str.AppendLine($"{Helper.CenteredString(unit.Lvl, maxLvlLength)}|");
            }

            str.Append("```\n");

            return str.ToString();
        }

        catch (Exception e)
        {
            return "При считывании MembersArmyInfo игрока что-то пошло не так";
        }
    }

}
