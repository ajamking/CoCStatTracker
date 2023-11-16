using CoCStatsTracker;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public static class UserNameAdder
{
    public static string TryAddUserNames(string clanTag, string userNames)
    {
        var updatedMembers = new List<string>(50);

        var userNamesString = userNames.Split('\n');

        foreach (var userNameString in userNamesString)
        {
            var tagAndUserName = userNameString.Split("-");

            if (tagAndUserName.Length == 1)
            {
                return StylingHelper.MakeItStyled("Некорректный формат сообщения, прочтите руководство и попробуйте заново.", UiTextStyle.Default);
            }

            if (string.IsNullOrEmpty(clanTag))
            {
                return StylingHelper.MakeItStyled("Перед установкой юзернеймов необходимо выбрать редактируемый клан.", UiTextStyle.Default);
            }

            var trackedClanDb = GetFromDbQueryHandler.GetTrackedClan(clanTag);

            foreach (var clanMember in trackedClanDb.ClanMembers)
            {
                if (clanMember.Tag == tagAndUserName[0])
                {
                    UpdateDbCommandHandler.ResetMemberUserName(tagAndUserName[0], tagAndUserName[1]);

                    updatedMembers.Add(clanMember.Name);
                }
            }

        }

        var answer = new StringBuilder(StylingHelper.MakeItStyled("Юзернеймы для игроков успешно обновлены:\n\n", UiTextStyle.Subtitle));

        foreach (var member in updatedMembers)
        {
            answer.AppendLine(StylingHelper.MakeItStyled($"{member}", UiTextStyle.Name));
        }

        return answer.ToString();
    }
}