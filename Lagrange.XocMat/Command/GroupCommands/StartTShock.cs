using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class StartTShock : Command
{
    public override string[] Alias => ["����"];
    public override string HelpText => "����������";
    public override string[] Permissions => [OneBotPermissions.StartTShock];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.Event.Chain.GroupMemberInfo!.Uin, args.Event.Chain.GroupUin!.Value, out Terraria.TerrariaServer? server) && server != null)
        {
            if (server.Start(args.CommamdLine))
            {
                await args.Event.Reply($"{server.Name} ���ڶ���ִ����������!", true);
                return;
            }
            await args.Event.Reply($"{server.Name} ����ʧ��!", true);
        }
        else
        {
            await args.Event.Reply("δ�л����������������Ч!", true);
        }
    }
}
