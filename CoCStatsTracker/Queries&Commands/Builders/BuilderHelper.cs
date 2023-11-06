using CoCStatsTracker.ApiEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCStatsTracker.Builders;
public static class BuilderHelper
{
    public static string GetRoleRu(this string role)
    {
        switch (role)
        {
            case "leader":
                {
                    return "Глава";
                }
            case "coLeader":
                {
                    return "Соруководитель";
                }
            case "admin":
                {
                    return "Старейшина";
                }
            default:
                {
                    return "Игрок";
                }
        }
    }

    public static string GetLeagueRU(this PlayerApi playerApi)
    {
        if (playerApi == null || playerApi.League == null)
        {
            return "Без лиги";
        }

        switch (playerApi.League.Name)
        {
            case "Unranked":
                {
                    return "Без лиги";
                }

            case "Bronze League III":
                {
                    return "Бронзовая III";
                }
            case "Bronze League II":
                {
                    return "Бронзовая II";
                }
            case "Bronze League I":
                {
                    return "Бронзовая I";
                }

            case "Silver League III":
                {
                    return "Серебряная III";
                }
            case "Silver League II":
                {
                    return "Серебряная II";
                }
            case "Silver League I":
                {
                    return "Серебряная I";
                }

            case "Gold League III":
                {
                    return "Золотая III";
                }
            case "Gold League II":
                {
                    return "Золотая II";
                }
            case "Gold League I":
                {
                    return "Золотая I";
                }

            case "Crystal League III":
                {
                    return "Хрустальная III";
                }
            case "Crystal League II":
                {
                    return "Хрустальная II";
                }
            case "Crystal League I":
                {
                    return "Хрустальная I";
                }

            case "Master League III":
                {
                    return "Мастер III";
                }
            case "Master League II":
                {
                    return "Мастер II";
                }
            case "Master League I":
                {
                    return "Мастер I";
                }

            case "Champion League III":
                {
                    return "Чемпионская III";
                }
            case "Champion League II":
                {
                    return "Чемпионская II";
                }
            case "Champion League I":
                {
                    return "Чемпионская I";
                }

            case "Titan League III":
                {
                    return "Титан III";
                }
            case "Titan League II":
                {
                    return "Титан II";
                }
            case "Titan League I":
                {
                    return "Титан I";
                }

            case "Legend League":
                {
                    return "Легендарная";
                }

            default:
                {
                    return "Без лиги";
                }
        }


    }

    public static string GetWarPreferenceRu(this string warPreference)
    {
        switch (warPreference)
        {
            case "in":
                {
                    return "Зелёный";
                }
            case "out":
                {
                    return "Красный";
                }
            default:
                {
                    return "Красный";
                }
        }
    }
}