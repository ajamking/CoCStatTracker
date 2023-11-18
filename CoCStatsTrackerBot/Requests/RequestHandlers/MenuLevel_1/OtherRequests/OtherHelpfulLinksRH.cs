using CoCStatsTrackerBot.BotMenues;
using System.Text;
using static System.Net.WebRequestMethods;

namespace CoCStatsTrackerBot.Requests;

public class OtherHelpfulLinksRH : BaseRequestHandler
{
    public OtherHelpfulLinksRH()
    {
        Header = "Полезные ссылки";
        HandlerMenuLevel = MenuLevel.Other1;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        var answer = new StringBuilder(StylingHelper.MakeItStyled("Ссылки на полезные ресурсы по CoC:\n\n", UiTextStyle.Header));

        var linksDic = new Dictionary<string, string>()
        {
            { "Wiki по Clash of Clans 🔥" , "https://clashofclans.fandom.com/wiki/Clash_of_Clans_Wiki" },
            { "Русскоязычная Wiki по Clash of Clans","https://clashofclans.fandom.com/ru/wiki/Clash_of_Clans_Wiki" },
            { "Качественные png изображенияя сущностей из CoC","https://fankit.supercell.com/d/vkEdmkUCngKw/game-assets" },
            { "Планировки для ТХ и ДС","https://clashofclans-layouts.com/ru/" },
            { "Планировки для столицы","https://clashcodes.com" },
            { "Авторское руководство для начинающих клешеров","https://disk.yandex.ru/d/ZcPoSyQsDSPImA" },
            { "Поиск видео атак на базу по загружаемому скрину 🔥","https://findthisbase.com/search" },
            { "Ютуб WhitewolvesNL Raids","https://www.youtube.com/@whitewolvesnlraids" },
            { "Ютуб Killers Empire CoC 🔥","https://www.youtube.com/@KillersEmpire" },

        };

        var counter = 1;

        foreach (var link in linksDic)
        {
            answer.AppendLine($"{StylingHelper.GetInlineLink($"{counter} 》 {link.Key}", link.Value)}\n");

            counter++;
        }

        ResponseSender.SendAnswer(parameters, true, answer.ToString());
    }
}