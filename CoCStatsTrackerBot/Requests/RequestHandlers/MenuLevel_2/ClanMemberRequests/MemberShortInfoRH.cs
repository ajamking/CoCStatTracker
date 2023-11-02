using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;
using CoCStatsTrackerBot.Requests;

namespace CoCStatsTrackerBot.Requests;

public class MemberShortInfoRH : BaseRequestHandler
{
    public MemberShortInfoRH()
    {
        Header = "Главное об игроке";
        HandlerMenuLevel = MenuLevel.PlayerInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var member = GetFromDbQueryHandler.GetClanMember(parameters.LastMemberTagMessage);

            var answer = PlayerFunctions.GetShortPlayerInfo(member);

            ResponseSender.SendAnswer(parameters, true, SplitAnswer(answer));
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Пока не обладаю такими сведениями.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}
