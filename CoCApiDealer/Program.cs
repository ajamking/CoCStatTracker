using CoCApiDealer.ForTests;
using Storage;

public class Program
{
    /// <summary>
    /// #YPPGCCY8 - тег клана
    /// #2VGG92CL9 - тег игрока
    /// </summary>

    static async Task Main(string[] args)
    {
        //InitializeDb();
        //UpdateDb();
        //KslmPtr();

        var updatedTrackedClans = new List<TrackedClanTest>();

        using (TestDbContext db = new TestDbContext("Data Source=./../../../../CustomSolutionElements/TestDb.db", true))
        {


        }
    }

    public static void UpdateDb()
    {
        var firstAttacks = new List<RaidAttackTest>()
        {
            new RaidAttackTest { DestructionPercentFrom = 0,  DestructionPercentTo = 10  },
            new RaidAttackTest { DestructionPercentFrom = 10, DestructionPercentTo = 20  },
            new RaidAttackTest { DestructionPercentFrom = 20, DestructionPercentTo = 30 },
        };

        var secondAttacks = new List<RaidAttackTest>()
        {
            new RaidAttackTest { DestructionPercentFrom = 0,  DestructionPercentTo = 40  },
            new RaidAttackTest { DestructionPercentFrom = 00, DestructionPercentTo = 80  },
        };

        var raidMember1 = new RaidMemberTest() { Attacks = firstAttacks };
        var raidMember2 = new RaidMemberTest() { Attacks = secondAttacks };

        var clanMember1 = new ClanMemberTest() { Name = "Updated_Alex", RaidMemberships = new List<RaidMemberTest> { raidMember1 } };
        var clanMember2 = new ClanMemberTest() { Name = "Updated_Bob", RaidMemberships = new List<RaidMemberTest> { raidMember2 } };

        var capitalRaid = new CapitalRaidTest() { Name = "Updated_Raid", RaidMembers = new List<RaidMemberTest> { raidMember1, raidMember2 } };

        var updatedTrackedClans = new List<TrackedClanTest>()
        {
            new TrackedClanTest()
            {
            Name = "UpdatedTestClan",
            Tag = "UpdatedTestTag",
            CapitalRaids = new List<CapitalRaidTest>() { capitalRaid },
            ClanMembers = new List<ClanMemberTest>() { clanMember1, clanMember2 }
            }
        };

        using (TestDbContext db = new TestDbContext("Data Source=./../../../../CustomSolutionElements/TestDb.db"))
        {
            var obsoleteTrackedClans = db.TrackedClans;

            db.TrackedClans.RemoveRange(obsoleteTrackedClans);

            db.TrackedClans.AddRange(updatedTrackedClans);

            db.Complete();

            var clan = db.TrackedClans.FirstOrDefault();

            Console.WriteLine();
            Console.WriteLine($@"Данные после редактирования:");
            Console.WriteLine($@"Клан: {clan.Name} {clan.Tag}");
            Console.WriteLine();

            foreach (var clanmember in clan.ClanMembers)
            {
                Console.WriteLine($@"Участие игрока: {clanmember.Name} в рейдах:");

                foreach (var raidMembership in clanmember.RaidMemberships)
                {
                    Console.WriteLine($@"Рейд: {raidMembership.Raid.Name}");

                    foreach (var attack in raidMembership.Attacks)
                    {
                        Console.WriteLine($@"ID Атаки: {attack.Id} - %От: {attack.DestructionPercentFrom} - %До: {attack.DestructionPercentTo}");
                    }
                }

                Console.WriteLine();
            }

        }
    }

    public static void InitializeDb()
    {
        var firstAttacks = new List<RaidAttackTest>()
        {
            new RaidAttackTest { DestructionPercentFrom = 0,  DestructionPercentTo = 30  },
            new RaidAttackTest { DestructionPercentFrom = 30, DestructionPercentTo = 60  },
            new RaidAttackTest { DestructionPercentFrom = 60, DestructionPercentTo = 100 },
        };

        var secondAttacks = new List<RaidAttackTest>()
        {
            new RaidAttackTest { DestructionPercentFrom = 0,  DestructionPercentTo = 50  },
            new RaidAttackTest { DestructionPercentFrom = 50, DestructionPercentTo = 100  },
        };

        var raidMember1 = new RaidMemberTest() { Attacks = firstAttacks };
        var raidMember2 = new RaidMemberTest() { Attacks = secondAttacks };

        var clanMember1 = new ClanMemberTest() { Name = "Alex", RaidMemberships = new List<RaidMemberTest> { raidMember1 } };
        var clanMember2 = new ClanMemberTest() { Name = "Bob", RaidMemberships = new List<RaidMemberTest> { raidMember2 } };

        var capitalRaid = new CapitalRaidTest() { Name = "FirstTestRaid", RaidMembers = new List<RaidMemberTest> { raidMember1, raidMember2 } };

        var trackedClans = new List<TrackedClanTest>()
        {
            new TrackedClanTest()
            {
            Name = "TestClan",
            Tag = "TestTag",
            CapitalRaids = new List<CapitalRaidTest>() { capitalRaid },
            ClanMembers = new List<ClanMemberTest>() { clanMember1, clanMember2 }
            }
        };

        using (TestDbContext db = new TestDbContext("Data Source=./../../../../CustomSolutionElements/TestDb.db", true))
        {
            var ts = db.ChangeTracker.Entries<RaidMemberTest>().ToList();

            db.TrackedClans.AddRange(trackedClans);

            db.Complete();

            var clan = db.TrackedClans.FirstOrDefault();

            Console.WriteLine();
            Console.WriteLine($@"Данные до редактирования:");
            Console.WriteLine($@"Клан: {clan.Name} {clan.Tag}");
            Console.WriteLine();

            foreach (var clanmember in clan.ClanMembers)
            {
                Console.WriteLine($@"Участие игрока: {clanmember.Name} в рейдах:");

                foreach (var raidMembership in clanmember.RaidMemberships)
                {
                    Console.WriteLine($@"Рейд: {raidMembership.Raid.Name}");

                    foreach (var attack in raidMembership.Attacks)
                    {
                        Console.WriteLine($@"ID Атаки: {attack.Id} - %От: {attack.DestructionPercentFrom} - %До: {attack.DestructionPercentTo}");
                    }
                }

                Console.WriteLine();
            }
        }
    }



    public static void KslmPtr()
    {
        var emloee1 = new Emploee() { Name = "Еблан1" };
        var emloee2 = new Emploee() { Name = "Еблан2" };
        var emloee3 = new Emploee() { Name = "Еблан3" };

        var company = new Company() { Name = "TestCompany", Staff = new List<Emploee>() { emloee1, emloee2, emloee3 } };

        using (KslmContext db = new KslmContext("Data Source=./../../../../CustomSolutionElements/KslmContext.db", true))
        {
            db.Companys.Add(company);

            db.Complete();

            //db.Remove(company);
            //db.Complete();
            Console.WriteLine("Ало ебло");
        }
    }
}
