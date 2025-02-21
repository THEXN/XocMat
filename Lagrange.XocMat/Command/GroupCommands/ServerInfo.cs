using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Lagrange.XocMat.Utility.Images;

namespace Lagrange.XocMat.Command.GroupCommands;

public class ServerInfo : Command
{
    public override string[] Alias => ["����б�"];
    public override string HelpText => "��������Ϣ";
    public override string[] Permissions => [OneBotPermissions.ServerList];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
        {
            Internal.Socket.Action.Response.ServerStatus status = await server.ServerStatus();
            if (!status.Status)
            {
                await args.Event.Reply("�޷����ӷ�����!", true);
                return;
            }
            TableBuilder tableBuilder = new TableBuilder();
            tableBuilder.SetTitle($"{server.Name}����б�");
            tableBuilder.SetTitleBottom(true);
            tableBuilder.AddRow("�������", "���˵��", "�������");
            foreach (Internal.Socket.Internet.PluginInfo plugin in status.Plugins)
            {
                tableBuilder.AddRow(plugin.Name, plugin.Description, plugin.Author);
            }
            await args.MessageBuilder.Image(await tableBuilder.BuildAsync()).Reply();
        }
        else
        {
            await args.Event.Reply("δ�л����������������Ч!", true);
        }
    }
}
