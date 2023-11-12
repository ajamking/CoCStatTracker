using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.UIEntities;
using Domain.Entities;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public static class PlayerFunctions
{
    public static string GetShortPlayerInfo(ClanMemberUi clanMemberui)
    {
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Краткая информация об игроке", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(clanMemberui.Name + " - " + clanMemberui.Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(clanMemberui.UpdatedOn.GetUpdatedOnString());
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled("μ - медианный процент разрушений.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("З/С - золото столицы.", UiTextStyle.Default));

        var dic = new Dictionary<string, string>()
        {
            { "КВ μ%", $"{clanMemberui.CwMedianDP}" },
            { "КВ μ% без 14,15ТХ", $"{clanMemberui.CwMedianDPWithout14_15Th}" },
            { "Рейды μ%", $"{clanMemberui.RaidsMedianDP}" },
            { "Рейды μ% без Пика", $"{clanMemberui.RaidsMedianDPWithoutPeak}" },
            { "Щит войны", $"{clanMemberui.WarPreference}" },
            { "Войск отправлено", $"{clanMemberui.DonationsSent.GetDividedString()}" },
            { "Войск получено", $"{clanMemberui.DonationsRecieved.GetDividedString()}" },
            { "Звезд завоевано", $"{clanMemberui.WarStars.GetDividedString()}" },
            { "З/С награблено", $"{clanMemberui.TotalCapitalGoldLooted.GetDividedString()}" },
            { "З/С вложено", $"{clanMemberui.TotalCapitalContributed.GetDividedString()}" },
        };

        var firstColumnName = "Параметр";

        var secondColumnName = "Значение";

        var tableSize = StylingHelper.DefineTableMaxSize(dic, firstColumnName, secondColumnName);

        str.AppendLine($"``` |{firstColumnName.PadRight(tableSize.KeyMaxLength)}|{StylingHelper.GetCenteredString(secondColumnName, tableSize.ValueMaxLength)}|");
        
        str.AppendLine($" " +
            $"|{new string('-', tableSize.KeyMaxLength)}" +
            $"|{new string('-', tableSize.ValueMaxLength)}|");

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
        var str = new StringBuilder();

        str.AppendLine(StylingHelper.MakeItStyled("Информация об игроке", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(clanMemberUi.Name + " - " + clanMemberUi.Tag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(clanMemberUi.UpdatedOn.GetUpdatedOnString());
        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
        str.AppendLine(StylingHelper.MakeItStyled("μ - медианный процент разрушений.", UiTextStyle.Default));
        str.AppendLine(StylingHelper.MakeItStyled("З/С - золото столицы.", UiTextStyle.Default));

        var dic = new Dictionary<string, string>()
        {
            { "Роль в клане", $"{clanMemberUi.RoleInClan}" },
            { "Уровень опыта", $"{clanMemberUi.ExpLevel}" },
            { "Уровень ТХ", $"{clanMemberUi.TownHallLevel}" },
            { "Уровень оружия", $"{clanMemberUi.TownHallWeaponLevel}" },
            { "Текущая лига", $"{clanMemberUi.League}" },
            { "Текущие трофеи", $"{clanMemberUi.Trophies.GetDividedString()}" },
            { "Макс. трофеи", $"{clanMemberUi.BestTrophies.GetDividedString()}" },
            { "Трофеи ДС", $"{clanMemberUi.VersusTrophies.GetDividedString()}" },
            { "Макс. трофеи ДС", $"{clanMemberUi.BestVersusTrophies.GetDividedString()}" },
            { "Атак выиграно", $"{clanMemberUi.AttackWins.GetDividedString()}" },
            { "Защит выиграно", $"{clanMemberUi.DefenseWins.GetDividedString()}" },
            { "Щит войны", $"{clanMemberUi.WarPreference}" },
            { "Войск отправлено", $"{clanMemberUi.DonationsSent.GetDividedString()}" },
            { "Войск получено", $"{clanMemberUi.DonationsRecieved.GetDividedString()}" },
            { "Звезд завоевано", $"{clanMemberUi.WarStars.GetDividedString()}" },
            { "З/С награблено", $"{clanMemberUi.TotalCapitalGoldLooted.GetDividedString()}" },
            { "З/С вложено", $"{clanMemberUi.TotalCapitalContributed.GetDividedString()}" },

            { "КВ μ%", $"{clanMemberUi.CwMedianDP}" },
            { "КВ μ% без 14,15ТХ", $"{clanMemberUi.CwMedianDPWithout14_15Th}" },
            { "Рейды μ%", $"{clanMemberUi.RaidsMedianDP}" },
            { "Рейды μ% без Пика", $"{clanMemberUi.RaidsMedianDPWithoutPeak}" },
        };

        var firstColumnName = "Параметр";

        var secondColumnName = "Значение";

        var tableSize = StylingHelper.DefineTableMaxSize(dic, firstColumnName, secondColumnName);

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

        if (recordsCount > 1)
        {
            str.AppendLine(StylingHelper.MakeItStyled("В войнах на стороне клана", UiTextStyle.Header));
        }
        else
        {
            str.AppendLine(StylingHelper.MakeItStyled("В войнe на стороне клана", UiTextStyle.Header));
        }

        str.AppendLine(StylingHelper.MakeItStyled(cwCwlMembershipsUi.First().ClanName + " - " + cwCwlMembershipsUi.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();

        var counter = 0;

        foreach (var uiMembership in cwCwlMembershipsUi.OrderByDescending(x => x.StartedOn))
        {
            if (recordsCount > 1)
            {
                str.AppendLine($@"{messageSplitToken}");
            }

            str.AppendLine(uiMembership.UpdatedOn.GetUpdatedOnString());
            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Начало подготовки:", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled(uiMembership.PreparationStartedOn.FormateToUiDateTime(), UiTextStyle.Default));
            str.AppendLine(StylingHelper.MakeItStyled("Начало войны:", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled(uiMembership.StartedOn.FormateToUiDateTime(), UiTextStyle.Default));
            str.AppendLine(StylingHelper.MakeItStyled("Конец войны:", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled(uiMembership.EndedOn.FormateToUiDateTime(), UiTextStyle.Default));

            if (uiMembership.EndedOn > DateTime.Now)
            {
                str.AppendLine();
                str.AppendLine(StylingHelper.MakeItStyled("Осталось до конца войны:", UiTextStyle.Subtitle));
                str.AppendLine(StylingHelper.MakeItStyled(uiMembership.EndedOn.GetTimeLeft(), UiTextStyle.Default));
            }

            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Позиция на карте: " + uiMembership.MapPosition, UiTextStyle.Subtitle));
            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Худшая защита:", UiTextStyle.Subtitle));

            if (uiMembership.BestOpponentsTime == 0 || uiMembership.BestOpponentsPercent == 0 || uiMembership.BestOpponentStars == 0)
            {
                str.AppendLine(StylingHelper.MakeItStyled("Защит пока не было.", UiTextStyle.Default));
            }
            else
            {
                str.AppendLine($"``` " +
               $"|{StylingHelper.GetCenteredString("Секунд", maxAttackLenght)}" +
               $"|{StylingHelper.GetCenteredString("%", maxDestructionPercent)}" +
               $"|{StylingHelper.GetCenteredString("Звезд", maxStars)}|");

                str.AppendLine($" " +
                    $"|{new string('-', maxAttackLenght)}" +
                    $"|{new string('-', maxDestructionPercent)}" +
                    $"|{new string('-', maxStars)}|");

                str.Append($" |{StylingHelper.GetCenteredString(uiMembership.BestOpponentsTime.ToString(), maxAttackLenght)}|");
                str.Append($"{StylingHelper.GetCenteredString(uiMembership.BestOpponentsPercent.ToString(), maxDestructionPercent)}|");
                str.AppendLine($"{StylingHelper.GetCenteredString(uiMembership.BestOpponentStars.ToString(), maxStars)}|");
                str.Append("```");
            }


            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));

            if (uiMembership.Attacks.Count == 0)
            {
                str.AppendLine(StylingHelper.MakeItStyled("Атаки не проводились.", UiTextStyle.Default));
            }
            else
            {
                str.AppendLine();
                str.AppendLine(StylingHelper.MakeItStyled("Пояснение таблицы:", UiTextStyle.TableAnnotation));
                str.AppendLine(StylingHelper.MakeItStyled("Атака - очередность атаки в войне", UiTextStyle.Default));
                str.AppendLine(StylingHelper.MakeItStyled("Противник - позиция на карте / уровень ТХ", UiTextStyle.Default));
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
                    str.Append($" |{StylingHelper.GetCenteredString(attack.AttackOrder.ToString(), maxAttackLenght)}|");

                    str.Append($"{StylingHelper.GetCenteredString(attack.EnemyMapPosition + " / " + attack.EnemyTHLevel, maxOpponentLenght)}|");

                    str.Append($"{StylingHelper.GetCenteredString(attack.DestructionPercent.ToString(), maxDestructionPercent)}|");

                    str.AppendLine($"{StylingHelper.GetCenteredString(attack.Stars.ToString(), maxStars)}|");
                }

                str.Append("```\n");
            }

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

        if (recordsCount>1)
        {
            str.AppendLine(StylingHelper.MakeItStyled("В рейдах на стороне клана", UiTextStyle.Header));
            
        }
        else
        {
            str.AppendLine(StylingHelper.MakeItStyled("В рейдe на стороне клана", UiTextStyle.Header));
        }

        str.AppendLine();
        str.AppendLine(StylingHelper.MakeItStyled(raidMembershipsUi.First().ClanName + " - " + raidMembershipsUi.First().ClanTag, UiTextStyle.Name));
        str.AppendLine();

        var counter = 0;

        foreach (var uiMembership in raidMembershipsUi.OrderByDescending(x => x.StartedOn))
        {
            if (recordsCount>1)
            {
                str.AppendLine($@"{messageSplitToken}");
            }
           
            str.AppendLine(StylingHelper.MakeItStyled("Начало рейдов", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled(uiMembership.StartedOn.FormateToUiDateTime(), UiTextStyle.Default));
            str.AppendLine(StylingHelper.MakeItStyled("Конец рейдов", UiTextStyle.Subtitle));
            str.AppendLine(StylingHelper.MakeItStyled(uiMembership.EndedOn.FormateToUiDateTime(), UiTextStyle.Default));
            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Золота заработано: " + uiMembership.TotalLoot.GetDividedString(), UiTextStyle.Subtitle));
            str.AppendLine();
            str.AppendLine(StylingHelper.MakeItStyled("Показатели атак:", UiTextStyle.Subtitle));

            if (uiMembership.Attacks.Count == 0)
            {
                str.AppendLine(StylingHelper.MakeItStyled("Атаки не проводились.", UiTextStyle.Default));
            }
            else
            {
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

                    str.Append($"{StylingHelper.GetCenteredString(attack.DistrictName.ToString(), maxDistrictLenght)}|");

                    str.Append($"{StylingHelper.GetCenteredString(attack.DestructionPercentFrom.ToString(), maxDestructionFrom)}|");

                    str.AppendLine($"{StylingHelper.GetCenteredString(attack.DestructionPercentTo.ToString(), maxDestructionTo)}|");

                    attackNumber++;
                }

                str.Append("```\n");
            }

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
                        break;
                    }
                case UnitType.EveryUnit:
                    {
                        //chosenUnits.AddRange(armyUi.Heroes);
                        //chosenUnits.AddRange(armyUi.SiegeMachines);
                        //chosenUnits.AddRange(armyUi.SuperUnits);
                        //chosenUnits.AddRange(armyUi.Pets);
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

        var dic = new Dictionary<UnitType, string>
        {
            {UnitType.Hero, "Герои" },
            {UnitType.SiegeMachine, "Осадные машины" },
            {UnitType.SuperUnit, "Активные супер юниты" },
            {UnitType.EveryUnit, "Обычные юниты" },
        };

        str.AppendLine(StylingHelper.MakeItStyled($"{dic[uniType]} игрока", UiTextStyle.Header));
        str.AppendLine(StylingHelper.MakeItStyled(armyUi.PlayerName + " - " + armyUi.PlayerTag, UiTextStyle.Name));
        str.AppendLine();
        str.AppendLine(armyUi.UpdatedOn.GetUpdatedOnString());
        str.AppendLine();

        if (chosenUnits.Count == 0)
        {
            str.AppendLine(StylingHelper.MakeItStyled("Игрок пока не имеет войск этого типа.", UiTextStyle.Default));

            return str.ToString();
        }

        str.AppendLine($"``` " +
            $"|{StylingHelper.GetCenteredString("Имя юнита", maxNameLength)}" +
            $"|{StylingHelper.GetCenteredString("Ур", maxLvlLength)}|");

        str.AppendLine($" " +
            $"|{new string('-', maxNameLength)}" +
            $"|{new string('-', maxLvlLength)}|");

        var counter = 0;

        foreach (var unit in chosenUnits.OrderByDescending(x => x.Village))
        {
            if (unit.Village != "home" && counter == 0)
            {
                str.AppendLine($" " +
                    $"|{StylingHelper.GetCenteredStringDash("ДС", maxNameLength)}" +
                    $"|{new string('-', maxLvlLength)}|");

                counter++;
            }

            if (unit.Village != "home" && unit.Lvl == 1)
            {
                continue;
            }

            str.Append($" |{StylingHelper.GetCenteredString(unit.Name, maxNameLength)}|");

            str.AppendLine($"{StylingHelper.GetCenteredString(unit.Lvl.ToString(), maxLvlLength)}|");
        }

        str.Append("```\n");

        return str.ToString();
    }
}