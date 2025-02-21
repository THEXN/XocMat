using System.Text;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class ExecuteCommandAll : Command
{
    public override string[] Alias => ["ִ��ȫ��"];
    public override string HelpText => "ִ��ȫ������";
    public override string[] Permissions => [OneBotPermissions.ExecuteCommand];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count < 1)
        {
            await args.Event.Reply("������Ҫִ�е�����!", true);
            return;
        }
        StringBuilder sb = new StringBuilder();
        foreach (Terraria.TerrariaServer server in XocMatSetting.Instance.Servers)
        {
            string cmd = "/" + string.Join(" ", args.Parameters);
            Internal.Socket.Action.Response.ServerCommand api = await server.Command(cmd);
            sb.AppendLine($"[{server.Name}]����ִ�н��:");
            sb.AppendLine(api.Status ? string.Join("\n", api.Params) : "�޷����ӵ���������");
        }
        await args.Event.Reply(sb.ToString().Trim());
    }
}
