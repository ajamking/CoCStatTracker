using CoCStatsTracker;
using System.Text;

namespace CoCStatsTrackerBot.Requests;

public static class UserNameAdder
{
    public static string TryAddUserNames(string clanTag, string userNames)
    {
        var updatedMembersDic = new Dictionary<string, string>(50);

        foreach (var userNameString in userNames.Split('\n'))
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

            var clanMembersDb = GetFromDbQueryHandler.GetAllClanMembers(clanTag);

            foreach (var clanMember in clanMembersDb)
            {
                if (clanMember.Tag == tagAndUserName[0])
                {
                    UpdateDbCommandHandler.ResetMemberUserName(tagAndUserName[0], tagAndUserName[1]);

                    updatedMembersDic.Add(clanMember.Name, tagAndUserName[1]);
                }
            }
        }

        var answer = new StringBuilder(StylingHelper.MakeItStyled("Юзернеймы для игроков успешно обновлены:\n\n", UiTextStyle.Subtitle));

        foreach (var member in updatedMembersDic)
        {
            answer.AppendLine(StylingHelper.MakeItStyled($"{member.Key}  ﴾ {member.Value} ﴿", UiTextStyle.Name));
        }

        return answer.ToString();
    }
}