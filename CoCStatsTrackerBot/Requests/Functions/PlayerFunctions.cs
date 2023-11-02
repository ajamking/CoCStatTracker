using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public static class PlayerFunctions
{
    public static string GetShortPlayerInfo(ClanMemberUi clanMemberui)
    {
        var dic = new Dictionary<string, string>()
        {
            { "КВ μ%", $"{clanMemberui.CwAverageDestructionPercent}" },
            { "КВ μ% без 14,15ТХ", $"{clanMemberui.CwAverageDestructionPercentWithout14_15Th}" },
            { "Рейды μ%", $"{clanMemberui.RaidsAverageDestructionPercent}" },
            { "Рейды μ% без Пика", $"{clanMemberui.RaidsAverageDestructionPercentWithoutPeak}" },
            { "Участие в войне", $"{clanMemberui.WarPreference}" },
            { "Войск отправлено", $"{clanMemberui.DonationsSent}" },
            { "Войск получено", $"{clanMemberui.DonationsRecieved}" },
            { "Звезд завоевано", $"{clanMemberui.WarStars}" },
            { "Золото столицы", $"{clanMemberui.TotalCapitalContributions}" },
        };

        var str = new StringBuilder();

        var firstColumnName = "Параметр";

        var secondColumnName = "Значение";

        var tableSize = StylingHelper.DefineTableMaxSize(dic, firstColumnName, secondColumnName);

        str.AppendLine(StylingHelper.MakeItStyled("Краткая информация об игроке", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(clanMemberui.Name + " - " + clanMemberui.Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled("μ - cредние показатели атак.", UiTextStyle.Default));
        str.AppendLine();
        str.AppendLine($"``` |{firstColumnName.PadRight(tableSize.KeyMaxLength)}|{StylingHelper.GetCenteredString(secondColumnName, tableSize.ValueMaxLength)}|");
        str.AppendLine($" |{new string('-', tableSize.KeyMaxLength)}|{new string('-', tableSize.ValueMaxLength)}|");

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(tableSize.KeyMaxLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(item.Value.ToString(), tableSize.ValueMaxLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string GetFullPlayerInfo(ClanMemberUi clanMemberUi)
    {
        var dic = new Dictionary<string, string>()
        {
            { "Роль в клане", $"{clanMemberUi.RoleInClan}" },
            { "Уровено опыта", $"{clanMemberUi.ExpLevel}" },
            { "Уровень ТХ", $"{clanMemberUi.TownHallLevel}" },
            { "Уровень оружия", $"{clanMemberUi.TownHallWeaponLevel}" },
            { "Трофеи", $"{clanMemberUi.Trophies}" },
            { "Max Трофеи", $"{clanMemberUi.BestTrophies}" },
            { "Текущая лига", $"{clanMemberUi.League.Replace("League ", "")}" },
            { "Трофеи ДС", $"{clanMemberUi.VersusTrophies}" },
            { "Max Трофеи ДС", $"{clanMemberUi.BestVersusTrophies}" },
            { "Атак выиграно", $"{clanMemberUi.AttackWins}" },
            { "Защит выиграно", $"{clanMemberUi.DefenseWins}" },
            { "Участие в войне", $"{clanMemberUi.WarPreference}" },
            { "Войск отправлено", $"{clanMemberUi.DonationsSent}" },
            { "Войск получено", $"{clanMemberUi.DonationsRecieved}" },
            { "Звезд завоевано", $"{clanMemberUi.WarStars}" },
            { "Золото столицы", $"{clanMemberUi.TotalCapitalContributions}" },

            { "КВ μ%", $"{clanMemberUi.CwAverageDestructionPercent}" },
            { "КВ μ% без 14,15ТХ", $"{clanMemberUi.CwAverageDestructionPercentWithout14_15Th}" },
            { "Рейды μ%", $"{clanMemberUi.RaidsAverageDestructionPercent}" },
            { "Рейды μ% без Пика", $"{clanMemberUi.RaidsAverageDestructionPercentWithoutPeak}" },
        };

        var str = new StringBuilder();

        var firstColumnName = "Параметр";

        var secondColumnName = "Значение";

        var tableSize = StylingHelper.DefineTableMaxSize(dic, firstColumnName, secondColumnName);

        str.AppendLine(StylingHelper.MakeItStyled("Информация об игроке", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(clanMemberUi.Name + " - " + clanMemberUi.Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled("μ - cредние показатели атак.", UiTextStyle.Default));
        str.AppendLine();

        str.AppendLine($"``` |{firstColumnName.PadRight(tableSize.KeyMaxLength)}|{StylingHelper.GetCenteredString(secondColumnName, tableSize.ValueMaxLength)}|");
        str.AppendLine($" |{new string('-', tableSize.KeyMaxLength)}|{new string('-', tableSize.ValueMaxLength)}|");

        foreach (var item in dic)
        {
            str.Append($" |{item.Key.PadRight(tableSize.KeyMaxLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(item.Value.ToString(), tableSize.ValueMaxLength)}|");
        }

        str.Append("```");

        return str.ToString();
    }

    public static string GetWarStatistics(List<CwCwlMembershipUi> cwCwlMembershipsUi, int recordsCount, string messageSplitToken)
    {
        //Эмпирически подобранные константы для адекватного отображения таблицы.
        var maxAttackLenght = 6;
        var maxOpponentLenght = 9;
        var maxDestructionPercent = 3;
        var maxStars = 5;

        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Показатели игрока", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(cwCwlMembershipsUi.First().Name + " - " + cwCwlMembershipsUi.First().Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("В войне на стороне клана", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(cwCwlMembershipsUi.First().ClanName + " - " + cwCwlMembershipsUi.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();

        var counter = 0;

        foreach (var uiMembership in cwCwlMembershipsUi.OrderByDescending(x => x.StartedOn))
        {
            str.AppendLine($@"{messageSplitToken}");
            str.AppendLine(StylingHelper.MakeItStyled("Начало войны", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled(uiMembership.StartedOn, UiTextStyle.Default));
            str.AppendLine(StylingHelper.MakeItStyled("Конец войны", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled(uiMembership.EndedOn, UiTextStyle.Default));
            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Позиция на карте: " + uiMembership.MapPosition, UiTextStyle.Subtitle));
            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Худшая защита:", UiTextStyle.Subtitle));
            str.AppendLine();

            str.AppendLine($"``` " +
                $"|{StylingHelper.GetCenteredString("Секунд", maxAttackLenght)}" +
                $"|{StylingHelper.GetCenteredString("%", maxDestructionPercent)}" +
                $"|{StylingHelper.GetCenteredString("Звезд", maxStars)}|");

            str.AppendLine($" " +
                $"|{new string('-', maxAttackLenght)}" +
                $"|{new string('-', maxDestructionPercent)}" +
                $"|{new string('-', maxStars)}|");

            str.Append($" |{StylingHelper.GetCenteredString(uiMembership.BestOpponentsTime, maxAttackLenght)}|");
            str.Append($"{StylingHelper.GetCenteredString(uiMembership.BestOpponentsPercent, maxDestructionPercent)}|");
            str.AppendLine($"{StylingHelper.GetCenteredString(uiMembership.BestOpponentStars, maxStars)}|");
            str.Append("```");

            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));
            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
            str.AppendLine(StylingHelper.MakeItStyled("Атака - очередность атаки в войне", UiTextStyle.Default));
            str.AppendLine(StylingHelper.MakeItStyled("Противник - позиция/уровень ТХ", UiTextStyle.Default));
            str.AppendLine();

            str.AppendLine($"``` " +
                $"|{StylingHelper.GetCenteredString("Атака", maxAttackLenght)}" +
                $"|{StylingHelper.GetCenteredString("Противник", maxOpponentLenght)}" +
                $"|{StylingHelper.GetCenteredString("%", maxDestructionPercent)}" +
                $"|{StylingHelper.GetCenteredString("Звезд", maxStars)}|");

            str.AppendLine($" " +
                $"|{new string('-', maxAttackLenght)}" +
                $"|{new string('-', maxOpponentLenght)}" +
                $"|{new string('-', maxDestructionPercent)}" +
                $"|{new string('-', maxStars)}|");

            foreach (var attack in uiMembership.Attacks)
            {
                str.Append($" |{StylingHelper.GetCenteredString(attack.AttackOrder, maxAttackLenght)}|");

                str.Append($"{StylingHelper.GetCenteredString(attack.EnemyMapPosition + " / " + attack.EnemyTHLevel, maxOpponentLenght)}|");

                str.Append($"{StylingHelper.GetCenteredString(attack.DestructionPercent, maxDestructionPercent)}|");

                str.AppendLine($"{StylingHelper.GetCenteredString(attack.Stars, maxStars)}|");
            }

            str.Append("```\n");

            counter++;

            if (counter == recordsCount || counter == cwCwlMembershipsUi.Count)
            {
                break;
            }
        }

        return str.ToString();
    }

    public static string GetRaidStatistics(List<RaidMembershipUi> raidMembershipsUi, int recordsCount, string messageSplitToken)
    {
        //Эмпирически подобранные константы для адекватного отображения таблицы.
        var maxAttackLenght = 1;
        var maxDistrictLenght = 15;
        var maxDestructionFrom = 3;
        var maxDestructionTo = 3;

        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Показатели игрока", UiTextStyle.Header));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled(raidMembershipsUi.First().Name + " - " + raidMembershipsUi.First().Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("В рейдах на стороне клана", UiTextStyle.Header));
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled(raidMembershipsUi.First().ClanName + " - " + raidMembershipsUi.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();

        var counter = 0;

        foreach (var uiMembership in raidMembershipsUi.OrderByDescending(x => x.StartedOn))
        {
            str.AppendLine($@"{messageSplitToken}");
            str.AppendLine(StylingHelper.MakeItStyled("Начало рейдов", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled(uiMembership.StartedOn, UiTextStyle.Default));
            str.AppendLine(StylingHelper.MakeItStyled("Конец рейдов", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled(uiMembership.EndedOn, UiTextStyle.Default));
            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Золота заработано - " + uiMembership.TotalLoot, UiTextStyle.Subtitle));
            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));
            str.AppendLine();

            str.AppendLine($"``` " +
                $"|{StylingHelper.GetCenteredString("N", maxAttackLenght)}" +
                $"|{StylingHelper.GetCenteredString("Район", maxDistrictLenght)}" +
                $"|{StylingHelper.GetCenteredString("%От", maxDestructionFrom)}" +
                $"|{StylingHelper.GetCenteredString("%До", maxDestructionTo)}|");

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

                str.Append($" |{StylingHelper.GetCenteredString(attackNumber.ToString(), maxAttackLenght)}|");

                str.Append($"{StylingHelper.GetCenteredString(attack.DistrictName, maxDistrictLenght)}|");

                str.Append($"{StylingHelper.GetCenteredString(attack.DestructionPercentFrom, maxDestructionFrom)}|");

                str.AppendLine($"{StylingHelper.GetCenteredString(attack.DestructionPercentTo, maxDestructionTo)}|");

                attackNumber++;
            }

            str.Append("```\n");

            counter++;

            if (counter == recordsCount || counter == raidMembershipsUi.Count)
            {
                break;
            }
        }

        return str.ToString();
    }

    public static string GetMembersArmyInfo(ArmyUi armyUi, UnitType uniType)
    {
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
                            if (unit.SuperTroopIsActivated == true)
                            {
                                chosenUnits.Add(unit);
                            }
                        }
                    }
                    break;
                case UnitType.EveryUnit:
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
        catch (Exception)
        {
            return "Этот игрок пока не обзавелся юнитами такого типа";
        }

        var str = new StringBuilder();

        var maxNameLength = 20;
        var maxLvlLength = 4;

        str.AppendLine(StylingHelper.MakeItStyled("Войска выбранного типа у игрока", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(armyUi.PlayerName + " - " + armyUi.PlayerTag, UiTextStyle.Name));
        str.AppendLine();

        str.AppendLine($"``` " +
            $"|{StylingHelper.GetCenteredString("Name", maxNameLength)}" +
            $"|{StylingHelper.GetCenteredString("Lvl", maxLvlLength)}|");

        str.AppendLine($" " +
            $"|{new string('-', maxNameLength)}" +
            $"|{new string('-', maxLvlLength)}|");

        foreach (var unit in chosenUnits)
        {
            str.Append($" |{StylingHelper.GetCenteredString(unit.Name, maxNameLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(unit.Lvl, maxLvlLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }
}
