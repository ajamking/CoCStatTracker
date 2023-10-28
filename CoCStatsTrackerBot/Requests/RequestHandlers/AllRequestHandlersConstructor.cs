using System.Reflection;

namespace CoCStatsTrackerBot.Requests.RequestHandlers;

public static class AllRequestHandlersConstructor
{
    public static List<BaseRequestHandler> AllRequestHandlers { get; set; }

    static AllRequestHandlersConstructor()
    {
        AllRequestHandlers = new List<BaseRequestHandler>();

        var childrenTypes = Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(BaseRequestHandler)));

        foreach (var item in childrenTypes)
        {
            var instance = (BaseRequestHandler)Activator.CreateInstance(item);

            AllRequestHandlers.Add(instance);
        }

        //AddVtupuy();
    }

    private static void AddVtupuy()
    {
        AllRequestHandlers = new List<BaseRequestHandler>()
        {
             //Level 0
             new StartMenuHandler(),

             //Level 1
             new ClanMemberMenuHandler(),
             new LeaderMenuHandler(),
             new OtherMenuHandler(),

             //Level 2
             new CurrentRaidShortInfoRH(),
             new CurrentRaidStatisticsRH(),

             new CurrentClanWarMapRH(),
             new CurrentClanWarShortInfoRH(),
             new CurrentClanWarStatisticsRH(),

             new ClanCurrentRaidInfoMenuHandler(),
             new ClanCurrentWarInfoMenuHandler(),
             new ClanInfoMenuHandler(),
             new PlayerInfoMenuHandler(),

             new ClanShortInfoRH(),
             new ClanActiveSuperUnitsRH(),
             new ClanSiegeMachinesRH(),
             new ClanAllMembersRH(),
             new ClanSeasonalStatisticRH(),
             new MemberFullInfoRH(),
             new MemberShortInfoRH(),

             //Level 3
             new ClanWarHistoryRH1(),
             new ClanWarHistoryRH3(),
             new ClanWarHistoryRH5(),
             new ClanWarHistoryRHBase(),

             new AverageRaidPerfomanceRH(),
             new ClanRaidsHistoryRH1(),
             new ClanRaidsHistoryRH3(),
             new ClanRaidsHistoryRH5(),
             new ClanRaidHistoryRHBase(),

             new CurrentDistrictStatisticsRHBase(),
             new D1CapitalPeakRH(),
             new D2BarbariansRH(),
             new D3WizzardsRH(),
             new D4BaloonsRH(),
             new D5BuildersRH(),
             new D6DragonsRH(),
             new D7GolemsRH(),
             new D8SkeletonsRH(),

             new MemberArmyRHBase(),
             new MemberEveryUnitInfoRH(),
             new MemberHeroesRH(),
             new MemberSiegeMachinesRH(),
             new MemberSuperUnitsRH(),

             new MemberWarStatisticsRH1(),
             new MemberWarStatisticsRH3(),
             new MemberWarStatisticsRH5(),
             new MemberWarStatisticsRHBase(),

             new MemberRaidStatisticsRH1(),
             new MemberRaidStatisticsRH3(),
             new MemberRaidStatisticsRH5(),
             new MemberRaidStatisticsRHBase(),

             new ClanCurrentDistrictStatisticsMenuHandler(),
             new ClanRaidHistoryMenuHandler(),
             new ClanWarHistoryMenuHandler(),
             new PlayerArmyMenuHandler(),
             new PlayerRaidStatisticsMenuHandler(),
             new PlayerWarStatisticsMenuHandler(),
        };
    }
}
