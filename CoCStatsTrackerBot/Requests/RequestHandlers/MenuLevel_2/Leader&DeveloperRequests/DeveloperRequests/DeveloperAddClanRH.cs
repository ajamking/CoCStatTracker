using CoCStatsTracker;
using CoCStatsTrackerBot.Menu;

namespace CoCStatsTrackerBot.Requests;

public class DeveloperAddClanRH : BaseRequestHandler
{
    public DeveloperAddClanRH()
    {
        Header = "Добавить клан в БД";
        HandlerMenuLevel = MenuLevel.DeveloperMenu2;
    }

    override public void Execute(BotUserRequestParameters parameters)
    {
        try
        {
            if (!(string.IsNullOrEmpty(parameters.TagToAddClan) && string.IsNullOrEmpty(parameters.AdminKeyToMerge)))
            {
                AddToDbCommandHandler.AddTrackedClan(parameters.TagToAddClan, parameters.AdminKeyToMerge);

                AddToDbCommandHandler.AddClanMembers(parameters.TagToAddClan);

                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Клан, члены клана, сезонная статистика добавлены в БД.", UiTextStyle.Default));
            }
            else
            {
                ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Тег для нового клана или токен главы не проставлены.", UiTextStyle.Default));
            }
        }
        catch (NotFoundException e)
        {
            ResponseSender.SendAnswer(parameters, true, DefaultNotFoundMessage);
        }
        catch (AlreadyExistsException e)
        {
            ResponseSender.SendAnswer(parameters, true, StylingHelper.MakeItStyled("Этот клан уже отслеживается, добавить его нельзя.", UiTextStyle.Default));
        }
        catch (Exception e)
        {
            ResponseSender.SendAnswer(parameters, false, e.StackTrace, e.Message);
        }
    }
}