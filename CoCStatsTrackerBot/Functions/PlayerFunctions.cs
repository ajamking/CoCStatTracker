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
        try
        {
            var member = UiHelper.GetClanMember(trackedClans, playerTag);

            if (member == null)
            {
                return UiHelper.Ecranize($"Игрока с тегом {playerTag} нет в отслеживаемых кланах, введите корректный тег игрока.");
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

            str.AppendLine(UiHelper.MakeItStyled("Краткая информация об игроке", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(shortPlayerInfoUi.Name + " - " + shortPlayerInfoUi.Tag, UiTextStyle.Name));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
            str.AppendLine(UiHelper.MakeItStyled("μ - cредние показатели атак.", UiTextStyle.Default));
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
        catch (Exception e)
        {
            return "Bad Response";
        }

    }

    public static string GetFullPlayerInfo(string playerTag, ICollection<TrackedClan> trackedClans)
    {
        try
        {
            var member = UiHelper.GetClanMember(trackedClans, playerTag);

            if (member == null)
            {
                return UiHelper.Ecranize($"Игрока с тегом {playerTag} нет в отслеживаемых кланах, введите корректный тег игрока.");
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

            str.AppendLine(UiHelper.MakeItStyled("Информация об игроке", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(playerInfoUi.Name + " - " + playerInfoUi.Tag, UiTextStyle.Name));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
            str.AppendLine(UiHelper.MakeItStyled("μ - cредние показатели атак.", UiTextStyle.Default));
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
        catch (Exception e)
        {
            return "Bad Response";
        }

    }

    public static string GetWarStatistics(string playerTag, ICollection<TrackedClan> trackedClans, int recordsCount, string messageSplitToken)
    {
        try
        {
            //Эмпирически подобранные константы для адекватного отображения таблицы.
            var maxAttackLenght = 6;
            var maxOpponentLenght = 9;
            var maxDestructionPercent = 3;
            var maxStars = 5;

            var member = UiHelper.GetClanMember(trackedClans, playerTag);

            if (member == null)
            {
                return UiHelper.Ecranize($"Игрока с тегом {playerTag} нет в отслеживаемых кланах, введите корректный тег игрока.");
            }

            if (member.WarMemberships.Count == 0)
            {
                return UiHelper.Ecranize($"Информация об участии в войнах игрока с тегом {playerTag} не найдена");
            }

            var uiMemberships = new List<CwCwlMembershipUi>();

            foreach (var warMembership in member.WarMemberships.OrderByDescending(cw => cw.ClanWar.EndTime))
            {
                uiMemberships.Add(Mapper.MapToCwCwlMembershipUi(warMembership));
            }

            var str = new StringBuilder();

            str.AppendLine(UiHelper.MakeItStyled("Показатели игрока", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(uiMemberships.First().Name + " - " + uiMemberships.First().Tag, UiTextStyle.Name));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("В войне на стороне клана", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(uiMemberships.First().ClanName + " - " + uiMemberships.First().ClanTag, UiTextStyle.Name));
            str.AppendLine();


            var counter = 0;

            foreach (var uiMembership in uiMemberships)
            {
                str.AppendLine($@"{messageSplitToken}");
                str.AppendLine(UiHelper.MakeItStyled("Начало войны", UiTextStyle.Subtitle));
                str.AppendLine(UiHelper.MakeItStyled(uiMembership.StartedOn, UiTextStyle.Default));
                str.AppendLine(UiHelper.MakeItStyled("Конец войны", UiTextStyle.Subtitle));
                str.AppendLine(UiHelper.MakeItStyled(uiMembership.EndedOn, UiTextStyle.Default));
                str.AppendLine();
                str.AppendLine(UiHelper.MakeItStyled("Позиция на карте - " + uiMembership.MapPosition, UiTextStyle.Subtitle));
                str.AppendLine();
                str.AppendLine(UiHelper.MakeItStyled("Худшая защита:", UiTextStyle.Subtitle));
                str.AppendLine();

                str.AppendLine($"``` " +
                    $"|{UiHelper.GetCenteredString("Секунд", maxAttackLenght)}" +
                    $"|{UiHelper.GetCenteredString("%", maxDestructionPercent)}" +
                    $"|{UiHelper.GetCenteredString("Звезд", maxStars)}|");

                str.AppendLine($" " +
                    $"|{new string('-', maxAttackLenght)}" +
                    $"|{new string('-', maxDestructionPercent)}" +
                    $"|{new string('-', maxStars)}|");

                str.Append($" |{UiHelper.GetCenteredString(uiMembership.BestOpponentsTime, maxAttackLenght)}|");
                str.Append($"{UiHelper.GetCenteredString(uiMembership.BestOpponentsPercent, maxDestructionPercent)}|");
                str.AppendLine($"{UiHelper.GetCenteredString(uiMembership.BestOpponentStars, maxStars)}|");
                str.Append("```");

                str.AppendLine();
                str.AppendLine(UiHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));
                str.AppendLine();
                str.AppendLine(UiHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
                str.AppendLine(UiHelper.MakeItStyled("Атака - очередность атаки в войне", UiTextStyle.Default));
                str.AppendLine(UiHelper.MakeItStyled("Противник - позиция/уровень ТХ", UiTextStyle.Default));
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
            return "Bad Response";
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

            if (member == null)
            {
                return UiHelper.Ecranize($"Игрока с тегом {playerTag} нет в отслеживаемых кланах, введите корректный тег игрока.");
            }

            if (member.RaidMemberships.Count == 0)
            {
                return UiHelper.Ecranize($"Информация об участии в рейдах игрока с тегом {playerTag} не найдена");
            }

            var uiMemberships = new List<RaidMembershipUi>();

            foreach (var raidMembership in member.RaidMemberships.OrderByDescending(cw => cw.Raid.StartedOn))
            {
                uiMemberships.Add(Mapper.MapToRaidMembershipUi(raidMembership));
            }

            var str = new StringBuilder();

            str.AppendLine(UiHelper.MakeItStyled("Показатели игрока", UiTextStyle.Header));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled(uiMemberships.First().Name + " - " + uiMemberships.First().Tag, UiTextStyle.Name));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled("В рейдах на стороне клана", UiTextStyle.Header));
            str.AppendLine();
            str.AppendLine(UiHelper.MakeItStyled(uiMemberships.First().ClanName + " - " + uiMemberships.First().ClanTag, UiTextStyle.Name));
            str.AppendLine();

            var counter = 0;

            foreach (var uiMembership in uiMemberships)
            {
                str.AppendLine($@"{messageSplitToken}");
                str.AppendLine(UiHelper.MakeItStyled("Начало рейдов", UiTextStyle.Subtitle));
                str.AppendLine(UiHelper.MakeItStyled(uiMembership.StartedOn, UiTextStyle.Default));
                str.AppendLine(UiHelper.MakeItStyled("Конец рейдов", UiTextStyle.Subtitle));
                str.AppendLine(UiHelper.MakeItStyled(uiMembership.EndedOn, UiTextStyle.Default));
                str.AppendLine();
                str.AppendLine(UiHelper.MakeItStyled("Золота заработано - " + uiMembership.TotalLoot, UiTextStyle.Subtitle));
                str.AppendLine();
                str.AppendLine(UiHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));
                str.AppendLine();

                str.AppendLine($"``` " +
                    $"|{UiHelper.GetCenteredString("N", maxAttackLenght)}" +
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
            return "Bad Response";
        }
    }

    public static string GetMembersArmyInfo(string playerTag, ICollection<TrackedClan> trackedClans, UnitType uniType)
    {
        try
        {
            var member = UiHelper.GetClanMember(trackedClans, playerTag);

            if (member == null)
            {
                return UiHelper.Ecranize($"Игрока с тегом {playerTag} нет в отслеживаемых кланах, введите корректный тег игрока.");
            }
            if (member.Units.Count == 0)
            {
                return UiHelper.Ecranize($"Информация о войске игрока с тегом {playerTag} не найдена");
            }

            var armyUi = Mapper.MapToArmyUi(member.Units);

            var chosenUnits = new List<TroopUi>();

            try
            {
                switch (uniType)
                {
                    case UnitType.Hero:
                        {
                            chosenUnits = armyUi.Heroes;
                            break;
                        }
                    case UnitType.SiegeMachine:
                        {
                            chosenUnits = armyUi.SiegeMachines;
                            break;
                        }
                    case UnitType.SuperUnit:
                        {
                            foreach (var unit in armyUi.SuperUnits)
                            {
                                if (unit.SuperTroopIsActivated == "True")
                                {
                                    chosenUnits.Add(unit);
                                }
                            }
                        }
                        break;
                    case UnitType.Unit:
                        {
                            chosenUnits.AddRange(armyUi.Heroes);
                            chosenUnits.AddRange(armyUi.SiegeMachines);
                            chosenUnits.AddRange(armyUi.SuperUnits);
                            chosenUnits.AddRange(armyUi.Pets);
                            chosenUnits.AddRange(armyUi.Units);
                            break;
                        }
                    default:
                        {
                            return "Ошибка при определении типа юнита";
                        }
                }
            }
            catch (Exception e)
            {
                return "Этот игрок пока не обзавелся юнитами такого типа";
            }

            var str = new StringBuilder();

            var maxNameLength = 20;
            var maxLvlLength = 4;

            str.AppendLine(UiHelper.MakeItStyled("Войска выбранного типа у игрока", UiTextStyle.Header));
            str.AppendLine(UiHelper.MakeItStyled(member.Name + " - " + member.Tag, UiTextStyle.Name));
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
            return "Bad Response";
        }
    }

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