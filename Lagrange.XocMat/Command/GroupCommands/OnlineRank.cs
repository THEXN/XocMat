using System.Text;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class OnlineRank : Command
{
    public override string[] Alias => ["��������"];
    public override string HelpText => "��������";
    public override string[] Permissions => [OneBotPermissions.OnlineRank];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
        {
            Internal.Socket.Action.Response.PlayerOnlineRank api = await server.OnlineRank();
            Core.Message.MessageBuilder body = args.MessageBuilder;
            if (api.Status)
            {
                if (api.OnlineRank.Count == 0)
                {
                    await args.Event.Reply("��ǰ��û�����ݼ�¼", true);
                    return;
                }
                StringBuilder sb = new StringBuilder($"[{server.Name}]��������:\n");
                IOrderedEnumerable<KeyValuePair<string, int>> rank = api.OnlineRank.OrderByDescending(x => x.Value);
                foreach ((string name, int duration) in rank)
                {
                    int day = duration / (60 * 60 * 24);
                    int hour = (duration - (day * 60 * 60 * 24)) / (60 * 60);
                    int minute = (duration - (day * 60 * 60 * 24) - (hour * 60 * 60)) / 60;
                    int second = duration - (day * 60 * 60 * 24) - (hour * 60 * 60) - (minute * 60);
                    sb.Append($"[{name}]����ʱ��: ");
                    if (day > 0)
                        sb.Append($"{day}��");
                    if (hour > 0)
                        sb.Append($"{hour}ʱ");
                    if (minute > 0)
                        sb.Append($"{minute}��");
                    sb.Append($"{second}��\n");
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
