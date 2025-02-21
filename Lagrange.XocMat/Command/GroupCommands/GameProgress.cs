using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Lagrange.XocMat.Terraria.Picture;

namespace Lagrange.XocMat.Command.GroupCommands;

public class GameProgress : Command
{
    public override string[] Alias => ["���Ȳ�ѯ"];
    public override string HelpText => "���Ȳ�ѯ";
    public override string[] Permissions => [OneBotPermissions.QueryProgress];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
        {
            Internal.Socket.Action.Response.GameProgress api = await server.QueryServerProgress();
            Core.Message.MessageBuilder body = args.MessageBuilder;
            if (api.Status)
            {
                MemoryStream stream = ProgressImage.Start(api.Progress, server.Name);
                body.Image(stream.ToArray());
            }
            else
            {
                body.Text("�޷���ȡ��������Ϣ��");
            }
            await args.Event.Reply(body);
        }
        else
        {
            await args.Event.Reply("δ�л����������������Ч!", true);
        }
    }
}
