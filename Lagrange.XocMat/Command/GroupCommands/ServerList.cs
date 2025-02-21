using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Lagrange.XocMat.Utility.Images;

namespace Lagrange.XocMat.Command.GroupCommands;

public class ServerList : Command
{
    public override string[] Alias => ["�������б�"];
    public override string HelpText => "�������б�";
    public override string[] Permissions => [OneBotPermissions.ServerList];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        IEnumerable<Terraria.TerrariaServer> groupServers = XocMatSetting.Instance.Servers.Where(s => s.Groups.Contains(args.GroupUin));
        if (!groupServers.Any())
        {
            await args.Event.Reply("��Ⱥδ�����κη�����!", true);
            return;
        }
        TableBuilder tableBuilder = new TableBuilder();
        tableBuilder.SetTitle("�������б�");
        tableBuilder.AddRow("����������", "������IP", "�������˿�", "�������汾", "����������", "����״̬", "��������", "��������", "�����С");
        foreach (Terraria.TerrariaServer? server in groupServers)
        {
            Internal.Socket.Action.Response.ServerStatus status = await server.ServerStatus();
            tableBuilder.AddRow(server.Name, server.IP, server.NatProt.ToString(), server.Version, server.Describe,
                !status.Status ? "�޷�����" : $"������:{status.RunTime:dd\\.hh\\:mm\\:ss}",
                !status.Status ? "�޷���ȡ" : status.WorldName,
                !status.Status ? "�޷���ȡ" : status.WorldSeed,
                !status.Status ? "�޷���ȡ" : $"{status.WorldWidth}x{status.WorldHeight}");
        }

        await args.MessageBuilder.Image(await tableBuilder.BuildAsync()).Reply();
    }
}
