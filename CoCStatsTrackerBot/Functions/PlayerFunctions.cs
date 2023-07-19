using CoCApiDealer.UIEntities;
using CoCStatsTracker;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System.Text;

namespace CoCStatsTrackerBot;

public static class PlayerFunctions
{
    public static string GetShortPlayerInfo(string playerTag, ICollection<TrackedClan> trackedClans)
    {
        var member = UiHelper.GetClanMember(trackedClans, playerTag);

        if (member == null)
        {
            return "Игрока с таким тегом нет, введите тег заново";
        }

        var shortPlayerInfoUi = Mapper.MapToShortPlayerInfoUi(member);

        var dic = new Dictionary<string, string>()
        {
            { "КВ μ%", $"{CalculateAveragePercent(member, AvgType.ClanWar)}" },
            { "КВ μ% без 14,15ТХ", $"{CalculateAveragePercent(member,AvgType.ClanWarWithout1415Th)}" },
            { "Рейды μ%", $"{CalculateAveragePercent(member, AvgType.Raids)}" },
            { "Рейды μ% без Пика", $"{CalculateAveragePercent(member, AvgType.RaidsWithoutPeak)}" },
            { "Участие в войне", $"{shortPlayerInfoUi.WarPreference}" },
            { "Войск отправлено", $"{shortPlayerInfoUi.DonationsSent}" },
            { "Войск получено", $"{shortPlayerInfoUi.DonationsRecieved}" },
            { "Звезд завоевано", $"{shortPlayerInfoUi.WarStars}" },
            { "Золото столицы", $"{shortPlayerInfoUi.TotalCapitalContributions}" },
        };

        var str = new StringBuilder();

        var firstColumnName = "Параметр";

        var secondColumnName = "Значение";

        var tableSize = UiHelper.DefineTableMaxSize(dic, firstColumnName, secondColumnName);

        str.AppendLine($@"```  Краткая информация об игроке```");
        str.AppendLine($@"     *{UiHelper.Ecranize(shortPlayerInfoUi.Name + " - " + shortPlayerInfoUi.Tag)}*");
        str.AppendLine();
        str.AppendLine($@"```  μ - cредние показатели атак.```");
        str.AppendLine();
        str.AppendLine($"``` |{firstColumnName.PadRight(tableSize.KeyMaxLength)}|{UiHelper.GetCenteredString(secondColumnName, tableSize.ValueMaxLength)}|");
        str.AppendLine($" |{new string('-', tableSize.KeyMaxLength)}|{new string('-', tableSize.ValueMaxLength)}|");

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(tableSize.KeyMaxLength)}|");

            str.AppendLine($"{UiHelper.GetCenteredString(item.Value.ToString(), tableSize.ValueMaxLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string GetFullPlayerInfo(string playerTag, ICollection<TrackedClan> trackedClans)
    {

        var member = UiHelper.GetClanMember(trackedClans, playerTag);

        if (member == null)
        {
            return "Игрока с таким тегом нет, введите тег заново";
        }

        var playerInfoUi = Mapper.MapToPlayerInfoUi(member);

        var dic = new Dictionary<string, string>()
        {
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
            { "КВ μ%", $"{CalculateAveragePercent(member, AvgType.ClanWar)}" },
            { "КВ μ% без 14,15ТХ", $"{CalculateAveragePercent(member,AvgType.ClanWarWithout1415Th)}" },
            { "Рейды μ%", $"{CalculateAveragePercent(member, AvgType.Raids)}" },
            { "Рейды μ% без Пика", $"{CalculateAveragePercent(member, AvgType.RaidsWithoutPeak)}" },
        };

        var str = new StringBuilder();

        var firstColumnName = "Параметр";

        var secondColumnName = "Значение";

        var tableSize = UiHelper.DefineTableMaxSize(dic, firstColumnName, secondColumnName);

        str.AppendLine($@"```  Информация об игроке ```");
        str.AppendLine($@"     *{UiHelper.Ecranize(playerInfoUi.Name + " - " + playerInfoUi.Tag)}*");
        str.AppendLine($@"```  Из клана ```");
        str.AppendLine($@"     *{UiHelper.Ecranize(playerInfoUi.ClanName + " - " + playerInfoUi.ClanTag)}*");
        str.AppendLine();
        str.AppendLine($@"```  μ - cредние показатели атак.```");
        str.AppendLine();

        str.AppendLine($"``` |{firstColumnName.PadRight(tableSize.KeyMaxLength)}|{UiHelper.GetCenteredString(secondColumnName, tableSize.ValueMaxLength)}|");
        str.AppendLine($" |{new string('-', tableSize.KeyMaxLength)}|{new string('-', tableSize.ValueMaxLength)}|");

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(tableSize.KeyMaxLength)}|");

            str.AppendLine($"{UiHelper.GetCenteredString(item.Value.ToString(), tableSize.ValueMaxLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string GetWarStatistics(string playerTag, ICollection<TrackedClan> trackedClans, int recordsCount, string messageSplitToken)
    {
        try
        {
            //Эмпирически подобранные константы для адекватного отображения таблицы.
            var maxAttackLenght = 5;
            var maxOpponentLenght = 9;
            var maxDestructionPercent = 5;
            var maxStars = 5;

            var member = UiHelper.GetClanMember(trackedClans, playerTag);

            if (member.WarMemberships.Count == 0)
            {
                return "Этот игрок пока не принимал участия в войнах";
            }

            var uiMemberships = new List<CwCwlMembershipUi>();

            foreach (var warMembership in member.WarMemberships.OrderByDescending(cw => cw.ClanWar.EndTime))
            {
                uiMemberships.Add(Mapper.MapToCwCwlMembershipUi(warMembership));
            }

            var str = new StringBuilder();

            str.AppendLine($@"```  Показатели игрока```");
            str.AppendLine($@"     *{UiHelper.Ecranize(uiMemberships.First().Name + " - " + uiMemberships.First().Tag)}*");
            str.AppendLine($@"```  В войне на стороне клана```");
            str.AppendLine($@"     *{UiHelper.Ecranize(uiMemberships.First().ClanName + " - " + uiMemberships.First().ClanTag)}*");
            str.AppendLine();
            str.AppendLine($@"```  Показатели атак:```");
            str.AppendLine($@"     Атака \- порядковый номер");
            str.AppendLine($@"     Противник \- позиция \/ уровень ТХ");
            str.AppendLine();

            var counter = 0;

            foreach (var uiMembership in uiMemberships)
            {
                str.AppendLine($@"{messageSplitToken}");
                str.AppendLine($@"```  Начало войны```");
                str.AppendLine($@"     *{UiHelper.Ecranize(uiMembership.StartedOn)}*");
                str.AppendLine($@"```  Конец войны```");
                str.AppendLine($@"     *{UiHelper.Ecranize(uiMembership.EndedOn)}*");
                str.AppendLine($@"```  Позиция на карте \- {uiMembership.MapPosition}```");
                str.AppendLine();
                str.AppendLine($@"```  Показатели обороны:```");
                str.AppendLine($@"     Лучшее время противника \- {UiHelper.Ecranize(uiMembership.BestOpponentsTime + " c.")}");
                str.AppendLine($@"     Лучший процент противника \- {UiHelper.Ecranize(uiMembership.BestOpponentsPercent + "%")}");
                str.AppendLine($@"     Лучшие звезды противника \- {UiHelper.Ecranize(uiMembership.BestOpponentsPercent + " зв.")}");
                str.AppendLine();

                str.AppendLine($"``` " +
                    $"|{UiHelper.GetCenteredString("Атака", maxAttackLenght)}" +
                    $"|{UiHelper.GetCenteredString("Противник", maxOpponentLenght)}" +
                    $"|{UiHelper.GetCenteredString("%", maxDestructionPercent)}" +
                    $"|{UiHelper.GetCenteredString("Звезд", maxStars)}|");

                str.AppendLine($" " +
                    $"|{new string('-', maxAttackLenght)}" +
                    $"|{new string('-', maxOpponentLenght)}" +
                    $"|{new string('-', maxDestructionPercent)}" +
                    $"|{new string('-', maxStars)}|");

                foreach (var attack in uiMembership.Attacks)
                {
                    str.Append($" |{UiHelper.GetCenteredString(attack.AttackOrder, maxAttackLenght)}|");

                    str.Append($"{UiHelper.GetCenteredString(attack.EnemyMapPosition + " / " + attack.EnemyTHLevel, maxOpponentLenght)}|");

                    str.Append($"{UiHelper.GetCenteredString(attack.DestructionPercent, maxDestructionPercent)}|");

                    str.AppendLine($"{UiHelper.GetCenteredString(attack.Stars, maxStars)}|");
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

    public static string GetRaidStatistics(string playerTag, ICollection<TrackedClan> trackedClans, int recordsCount, string messageSplitToken)
    {
        try
        {
            //Эмпирически подобранные константы для адекватного отображения таблицы.
            var maxAttackLenght = 1;
            var maxDistrictLenght = 15;
            var maxDestructionFrom = 3;
            var maxDestructionTo = 3;

            var member = UiHelper.GetClanMember(trackedClans, playerTag);

            if (member.RaidMemberships.Count == 0)
            {
                return "Этот игрок пока не принимал участия в рейдах";
            }

            var uiMemberships = new List<RaidMembershipUi>();

            foreach (var raidMembership in member.RaidMemberships.OrderByDescending(cw => cw.Raid.StartedOn))
            {
                uiMemberships.Add(Mapper.MapToRaidMembershipUi(raidMembership));
            }

            var str = new StringBuilder();

            str.AppendLine($@"```  Показатели игрока```");
            str.AppendLine($@"     *{UiHelper.Ecranize(uiMemberships.First().Name + " - " + uiMemberships.First().Tag)}*");
            str.AppendLine($@"```  В рейдах на стороне клана```");
            str.AppendLine($@"     *{UiHelper.Ecranize(uiMemberships.First().ClanName + " - " + uiMemberships.First().ClanTag)}*");

            var counter = 0;

            foreach (var uiMembership in uiMemberships)
            {
                str.AppendLine();
                str.AppendLine($@"{messageSplitToken}");
                str.AppendLine($@"```  Начало рейдов```");
                str.AppendLine($@"     *{UiHelper.Ecranize(uiMembership.StartedOn)}*");
                str.AppendLine($@"```  Конец рейдов```");
                str.AppendLine($@"     *{UiHelper.Ecranize(uiMembership.EndedOn)}*");
                str.AppendLine();
                str.AppendLine($@"     *{UiHelper.Ecranize("Всего золота заработано - " + uiMembership.TotalLoot)}*");
                str.AppendLine();
                str.AppendLine($@"```  Информация об атаках: ```");
                str.AppendLine();

                str.AppendLine($"``` " +
                    $"|{UiHelper.GetCenteredString("№", maxAttackLenght)}" +
                    $"|{UiHelper.GetCenteredString("Район", maxDistrictLenght)}" +
                    $"|{UiHelper.GetCenteredString("%От", maxDestructionFrom)}" +
                    $"|{UiHelper.GetCenteredString("%До", maxDestructionTo)}|");

                str.AppendLine($" " +
                    $"|{new string('-', maxAttackLenght)}" +
                    $"|{new string('-', maxDistrictLenght)}" +
                    $"|{new string('-', maxDestructionFrom)}" +
                    $"|{new string('-', maxDestructionTo)}|");

                var attackNumber = 1;

                foreach (var attack in uiMembership.Attacks)
                {
                    if (attack.DistrictName.Length > maxDistrictLenght)
                    {
                        attack.DistrictName = attack.DistrictName.Substring(0, maxDistrictLenght);
                    }

                    str.Append($" |{UiHelper.GetCenteredString(attackNumber.ToString(), maxAttackLenght)}|");

                    str.Append($"{UiHelper.GetCenteredString(attack.DistrictName, maxDistrictLenght)}|");

                    str.Append($"{UiHelper.GetCenteredString(attack.DestructionPercentFrom, maxDestructionFrom)}|");

                    str.AppendLine($"{UiHelper.GetCenteredString(attack.DestructionPercentTo, maxDestructionTo)}|");

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

    //public static string GetMemberDrawMembership(string playerTag, ICollection<TrackedClan> trackedClans)
    //{
    //    var member = UiHelper.GetClanMember(trackedClans, playerTag);

    //    if (member.DrawMemberships == null)
    //    {
    //        return "Этот игрок не участвовал ни в одном розыгрыше";
    //    }

    //    var drawMembership = new DrawMembershipUi();

    //    foreach (var drawMember in member.DrawMemberships)
    //    {
    //        if (drawMember.PrizeDraw.EndedOn > DateTime.Now)
    //        {
    //            drawMembership = Mapper.MapToDrawMembershipUi(drawMember);
    //        }
    //        else
    //        {
    //            return "На данный момент розыгрыш не проводится или игрок в нем не участвует";
    //        }
    //    }

    //    var dic = new Dictionary<string, string>()
    //    {
    //        { "Очков", $"{drawMembership.DrawTotalScore}" },
    //        { "Позиция", $"{drawMembership.PositionInClan}" },
    //    };

    //    var str = new StringBuilder();

    //    var firstColumnName = "Параметр";

    //    var secondColumnName = "Значение";

    //    var tableSize = UiHelper.DefineTableMaxSize(dic, firstColumnName, secondColumnName);

    //    str.AppendLine($@"```  Показатели игрока```");
    //    str.AppendLine($@"     *{UiHelper.Ecranize(drawMembership.PlayersName + " - " + drawMembership.PlayersTag)}*");
    //    str.AppendLine($@"```  В текущем розыгрыше в клане```");
    //    str.AppendLine($@"     *{UiHelper.Ecranize(drawMembership.ClanName + " - " + drawMembership.ClanTag)}*");
    //    str.AppendLine();
    //    str.AppendLine($@"```  Начало розыгрыша```");
    //    str.AppendLine($@"     *{UiHelper.Ecranize(drawMembership.Start)}*");
    //    str.AppendLine($@"```  Конец розыгрыша```");
    //    str.AppendLine($@"     *{UiHelper.Ecranize(drawMembership.End)}*");
    //    str.AppendLine();

    //    str.AppendLine($"``` |{firstColumnName.PadRight(tableSize.KeyMaxLength)}|{UiHelper.CenteredString(secondColumnName, tableSize.ValueMaxLength)}|");
    //    str.AppendLine($" |{new string('-', tableSize.KeyMaxLength)}|{new string('-', tableSize.ValueMaxLength)}|");

    //    foreach (var item in dic)
    //    {
    //        str.Append($" |{item.Key.PadRight(tableSize.KeyMaxLength)}|");

    //        str.AppendLine($"{UiHelper.CenteredString(item.Value.ToString(), tableSize.ValueMaxLength)}|");
    //    }

    //    str.Append("```");

    //    return str.ToString();
    //}

    public static string GetMembersArmyInfo(string playerTag, ICollection<TrackedClan> trackedClans, UnitType uniType)
    {
        try
        {
            var member = UiHelper.GetClanMember(trackedClans, playerTag);

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
                        return "Ошибка при определении типа юнита";
                }
            }

            catch (Exception e)
            {
                return "Этот игрок пока не обзавелся юнитами такого типа";
            }

            var str = new StringBuilder();

            var maxNameLength = 20;
            var maxLvlLength = 4;

            str.AppendLine($@"```  Войска выбранного типа у игрока```");
            str.AppendLine($@"     *{UiHelper.Ecranize(member.Name + " - " + member.Tag)}*");
            str.AppendLine();

            str.AppendLine($"``` " +
                $"|{UiHelper.GetCenteredString("Name", maxNameLength)}" +
                $"|{UiHelper.GetCenteredString("Lvl", maxLvlLength)}|");

            str.AppendLine($" " +
                $"|{new string('-', maxNameLength)}" +
                $"|{new string('-', maxLvlLength)}|");

            foreach (var unit in chosenUnits)
            {
                str.Append($" |{UiHelper.GetCenteredString(unit.Name, maxNameLength)}|");

                str.AppendLine($"{UiHelper.GetCenteredString(unit.Lvl, maxLvlLength)}|");
            }

            str.Append("```\n");

            return str.ToString();
        }

        catch (Exception e)
        {
            return "При считывании MembersArmyInfo игрока что-то пошло не так";
        }
    }

    //public static string GetMemberCarmaHistory(string playerTag, ICollection<TrackedClan> trackedClans)
    //{
    //    var earnedPointsMaxLenght = 4;
    //    var activityNameMaxLenght = 10;

    //    try
    //    {
    //        var member = UiHelper.GetClanMember(trackedClans, playerTag);

    //        var carma = Mapper.MapToCarmaUi(member);

    //        if (carma.Activities.Count == 0)
    //        {
    //            return "У этого игрока пока нет зафиксированных активностей";
    //        }

    //        var str = new StringBuilder();

    //        str.AppendLine($@"*История кармы игрока*");
    //        str.AppendLine($@"*{UiHelper.Ecranize(carma.PlayersName + " - " + carma.PlayersTag)}*");
    //        str.AppendLine();
    //        str.AppendLine($@"_{UiHelper.Ecranize("Очки активностей обновляются каждый месяц вместе с розыгрышем и влияют на карму, а карма учитывается в розыгрыше.")}_");
    //        str.AppendLine();
    //        str.AppendLine($@"*Формат списка:*");
    //        str.AppendLine($@"{UiHelper.Ecranize("[Дата][Очки][Название][Описание]")}");
    //        str.AppendLine();
    //        str.AppendLine($@"*Cписок активностей:*");

    //        foreach (var activity in carma.Activities.OrderByDescending(x => x.UpdatedOn))
    //        {
    //            str.AppendLine($@"*{UiHelper.Ecranize("[" + activity.UpdatedOn.Substring(0, 6) + "]" + " [" + UiHelper.CenteredString(activity.EarnedPoints, earnedPointsMaxLenght) + "]" + 
    //                " [" + UiHelper.CenteredString(activity.Name, activityNameMaxLenght) + "]" + " [" + activity.Description + "]")}*");
    //            str.AppendLine();
    //        }

    //        return str.ToString();
    //    }

    //    catch (Exception e)
    //    {
    //        return "При считывании кармы игрока что-то пошло не так";
    //    }
    //}


    public static string CalculateAveragePercent(ClanMember member, AvgType avgType)
    {
        try
        {
            switch (avgType)
            {
                case AvgType.ClanWar:
                    {
                        var warAvg = 0;
                        var warCounter = 0;

                        foreach (var war in member.WarMemberships)
                        {
                            foreach (var attack in war.WarAttacks)
                            {
                                warAvg += attack.DestructionPercent;

                                warCounter++;
                            }
                        }

                        return (warAvg / warCounter).ToString();
                    }
                case AvgType.ClanWarWithout1415Th:
                    {
                        var warAvg = 0;
                        var warCounter = 0;

                        foreach (var war in member.WarMemberships)
                        {
                            foreach (var attack in war.WarAttacks)
                            {
                                if (attack.EnemyWarMember.THLevel != 15 && attack.EnemyWarMember.THLevel != 14)
                                {
                                    warAvg += attack.DestructionPercent;

                                    warCounter++;
                                }
                            }
                        }

                        return (warAvg / warCounter).ToString();
                    }
                case AvgType.Raids:
                    {
                        var raidsAvg = 0;
                        var raidsCounter = 0;

                        foreach (var raid in member.RaidMemberships)
                        {
                            foreach (var attack in raid.Attacks)
                            {
                                raidsAvg += (attack.DestructionPercentTo - attack.DestructionPercentFrom);
                                raidsCounter++;
                            }
                        }

                        return (raidsAvg / raidsCounter).ToString();
                    }
                case AvgType.RaidsWithoutPeak:
                    {
                        var raidsAvg = 0;
                        var raidsCounter = 0;

                        foreach (var raid in member.RaidMemberships)
                        {
                            foreach (var attack in raid.Attacks)
                            {
                                if (attack.OpponentDistrict.Name != "Capital Peak")
                                {
                                    raidsAvg += (attack.DestructionPercentTo - attack.DestructionPercentFrom);
                                    raidsCounter++;
                                }
                            }
                        }

                        return (raidsAvg / raidsCounter).ToString();
                    }
                default:
                    return ("WTF");
            }
        }
        catch (Exception e)
        {
            return ("0");
        }

    }

    public enum AvgType
    {
        ClanWar,
        ClanWarWithout1415Th,
        Raids,
        RaidsWithoutPeak
    }
}
