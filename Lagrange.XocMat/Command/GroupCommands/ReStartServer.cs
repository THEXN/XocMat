using Lagrange.Core.Message;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class ReStartServer : Command
{
    public override string[] Alias => ["����������"];
    public override string HelpText => "����������";
    public override string[] Permissions => [OneBotPermissions.ResetTShock];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
        {
            Internal.Socket.Action.Response.BaseActionResponse api = await server.ReStartServer(args.CommamdLine);
            MessageBuilder build = MessageBuilder.Group(args.GroupUin);
            if (api.Status)
            {
                build.Text("�������������������Ժ�...");
            }
            else
            {
                build.Text(api.Message);
            }
            await args.Event.Reply(build);
        }
        else
        {
            await args.Event.Reply("δ�л����������������Ч!", true);
        }
    }
}
