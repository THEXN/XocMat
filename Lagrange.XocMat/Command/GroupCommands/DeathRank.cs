using System.Text;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class DeathRank : Command
{
    public override string[] Alias => new[] { "��������" };
    public override string HelpText => "��������";
    public override string[] Permissions => new[] { OneBotPermissions.DeathRank };

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.Event.Chain.GroupMemberInfo!.Uin, args.Event.Chain.GroupUin!.Value, out Terraria.TerrariaServer? server) && server != null)
        {
            Internal.Socket.Action.Response.DeadRank api = await server.DeadRank();
            Core.Message.MessageBuilder body = args.MessageBuilder;
            if (api.Status)
            {
                if (api.Rank.Count == 0)
                {
                    await args.Event.Reply("��ǰ��û�����ݼ�¼", true);
                    return;
                }
                StringBuilder sb = new StringBuilder($"[{server.Name}]��������:\n");
                IOrderedEnumerable<KeyValuePair<string, int>> rank = api.Rank.OrderByDescending(x => x.Value);
                foreach ((string name, int count) in rank)
                {
                    sb.AppendLine($"[{name}]��������: {count}");
                }
                body.Text(sb.ToString().Trim());
            }
            else
            {
                body.Text("�޷����ӵ���������");
            }
            await args.Event.Reply(body);
        }
        else
        {
            await args.Event.Reply("δ�л����������������Ч!", true);
        }
    }
}
