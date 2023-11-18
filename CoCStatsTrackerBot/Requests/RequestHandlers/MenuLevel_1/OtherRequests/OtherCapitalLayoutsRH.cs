using CoCStatsTrackerBot.BotMenues;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public class OtherCapitalLayoutsRH : BaseRequestHandler
{
    public OtherCapitalLayoutsRH()
    {
        Header = "Дефолтные планировки столицы";
        HandlerMenuLevel = MenuLevel.Other1;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        var answer = new StringBuilder(StylingHelper.MakeItStyled("Ссылки на исходные планировки столичных районов:\n\n", UiTextStyle.Header));

        var linksDic = new Dictionary<string, string>()
        {
            { "Лагерь варваров","https://link.clashofclans.com/ru?action=OpenLayout&id=TH5%3ACC%3A1%3AAAAASwAAAAGhKKJTkzvy9ZPzvy81jXNu" },
            { "Долина колдунов","https://link.clashofclans.com/ru?action=OpenLayout&id=TH5%3ACC%3A2%3AAAAASwAAAAGhKJ-iC-PuZFwb84bXQnj8" },
            { "Лагуна шаров","https://link.clashofclans.com/ru?action=OpenLayout&id=TH5%3ACC%3A3%3AAAAASwAAAAGhKJuw6RnGgV93eONmFaQY" },
            { "Мастерская строителя","https://link.clashofclans.com/ru?action=OpenLayout&id=TH5%3ACC%3A4%3AAAAASwAAAAGhKJBFk-Q71LexAz0Dsm-E" },
            { "Драконьи утесы","https://link.clashofclans.com/ru?action=OpenLayout&id=TH5%3ACC%3A5%3AAAAASwAAAAGhKIsKtUa4U-kMQ8Pz9AGl" },
            { "Карьер големов","https://link.clashofclans.com/ru?action=OpenLayout&id=TH5%3ACC%3A6%3AAAAASwAAAAGhKId0DjsF1qkeB3cJVs99" },
            { "Парк скелетов","https://link.clashofclans.com/ru?action=OpenLayout&id=TH4%3ACC%3A7%3AAAAASwAAAAGhKILVYHGqUto1vgD41lt5" },
            { "Гоблинские шахты","https://link.clashofclans.com/ru?action=OpenLayout&id=TH4%3ACC%3A8%3AAAAASwAAAAGhKHz-PhO55MzIvwflO8tM" },
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