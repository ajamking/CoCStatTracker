using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class ClanAllMembersRH : BaseRequestHandler
{
    public ClanAllMembersRH()
    {
        Header = "Члены клана";
        HandlerMenuLevel = MenuLevel.ClanInfo2;
    }

    override public void Execute(RequestHadnlerParameters parameters)
    {
        try
        {
            var clanMembers = GetFromDbQueryHandler.GetAllClanMembers(parameters.LastClanTagMessage);

            var answer = ClanFunctions.GetClanMembers(clanMembers);

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
