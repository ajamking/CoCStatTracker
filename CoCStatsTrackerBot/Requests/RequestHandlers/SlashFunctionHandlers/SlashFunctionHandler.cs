using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCStatsTrackerBot.Requests.RequestHandlers.SlashFunctionHandlers;

public static class SlashFunctionHandler
{
    public static void Handle(BotUserRequestParameters parameters, BotSlashFunction slashFunc)
    {
        switch (slashFunc)
        {
            case BotSlashFunction.GroupGetChatId:
                HandleGroupGetChatId(parameters);
                break;
            case BotSlashFunction.GroupGetRaidShortInfo:
                HandleGroupGetRaidShortInfo(parameters);
                break;
            case BotSlashFunction.GroupGetWarShortInfo:
                HandleGroupGetWarShortInfo(parameters);
                break;
            case BotSlashFunction.GroupGetWarMap:
                HandleGroupGetWarShortInfo(parameters);
                break;
            default:
                break;
        }

    }

    private static void HandleGroupGetChatId(BotUserRequestParameters parameters)
    {
        ResponseSender.SendAnswer(parameters, true,
            StylingHelper.MakeItStyled($"{parameters.Message.Chat.Id}", UiTextStyle.Default));
    }

    private static void HandleGroupGetRaidShortInfo(BotUserRequestParameters parameters)
    {


    }

    private static void HandleGroupGetWarShortInfo(BotUserRequestParameters parameters)
    {


    }

}
