using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class ExecuteCommand : Command
{
    public override string[] Alias => ["ִ��"];
    public override string HelpText => "ִ������";
    public override string[] Permissions => [OneBotPermissions.ExecuteCommand];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count < 1)
        {
            await args.Event.Reply("������Ҫִ�е�����!", true);
            return;
        }
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
        {
            string cmd = "/" + string.Join(" ", args.Parameters);
            Internal.Socket.Action.Response.ServerCommand api = await server.Command(cmd);
            Core.Message.MessageBuilder body = args.MessageBuilder;
            if (api.Status)
            {
                string cmdResult = $"[{server.Name}]����ִ�н��:\n{string.Join("\n", api.Params)}";
                body.Text(cmdResult);
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
