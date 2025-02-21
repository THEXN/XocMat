using System.Text;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class SearchUser : Command
{
    public override string[] Alias => ["ע���ѯ", "name"];
    public override string HelpText => "��ѯע����";
    public override string[] Permissions => [OneBotPermissions.SearchUser];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        async ValueTask GetRegister(long id)
        {
            List<TerrariaUser> users = TerrariaUser.GetUsers(id);
            if (users.Count == 0)
            {
                await args.Event.Reply("δ��ѯ�����û���ע����Ϣ!");
                return;
            }
            StringBuilder sb = new("��ѯ���:\n");
            foreach (TerrariaUser user in users)
            {
                sb.AppendLine($"ע������: {user.Name}");
                sb.AppendLine($"ע���˺�: {user.Id}");
                Core.Common.Entity.BotGroupMember? result = (await args.Bot.FetchMembers(args.Event.Chain.GroupUin!.Value)).FirstOrDefault(x => x.Uin == user.Id);
                if (result != null)
                {
                    sb.AppendLine($"Ⱥ�ǳ�: {result.MemberName}");
                }
                else
                {
                    sb.AppendLine("ע���˲��ڴ�Ⱥ��");
                }
                sb.AppendLine("");
            }
            await args.Event.Reply(sb.ToString().Trim());
        }
        IEnumerable<Core.Message.Entity.MentionEntity> atlist = args.Event.Chain.GetMention();
        if (args.Parameters.Count == 0 && atlist.Any())
        {
            Core.Message.Entity.MentionEntity target = atlist.First();
            await GetRegister(target.Uin);

        }
        else if (args.Parameters.Count == 1)
        {
            if (long.TryParse(args.Parameters[0], out long id))
            {
                await GetRegister(id);
            }
            else
            {
                TerrariaUser? user = TerrariaUser.GetUsersByName(args.Parameters[0]);
                if (user == null)
                {
                    await args.Event.Reply("δ��ѯ��ע����Ϣ", true);
                    return;
                }
                else
                {
                    await GetRegister(user.Id);
                }
            }
        }
    }
}
