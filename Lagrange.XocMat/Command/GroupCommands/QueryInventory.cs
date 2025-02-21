using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Lagrange.XocMat.Terraria.Picture;

namespace Lagrange.XocMat.Command.GroupCommands;

public class QueryInventory : Command
{
    public override string[] Alias => ["�鱳��"];
    public override string HelpText => "��ѯ����";
    public override string[] Permissions => [OneBotPermissions.QueryInventory];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
            {
                Internal.Socket.Action.Response.PlayerInventory api = await server.PlayerInventory(args.Parameters[0]);
                Core.Message.MessageBuilder body = args.MessageBuilder;
                if (api.Status)
                {
                    MemoryStream ms = DrawInventory.Start(api.PlayerData!, api.PlayerData!.Username, api.ServerName);
                    body.Image(ms.ToArray());
                }
                else
                {
                    body.Text("�޷���ȡ�û���Ϣ��");
                }
                await args.Event.Reply(body);
            }
            else
            {
                await args.Event.Reply("δ�л����������������Ч!", true);
            }
        }
        else
        {
            await args.Event.Reply($"�﷨����,��ȷ�﷨:\n{args.CommamdPrefix}{args.Name} [�û���]");
        }
    }
}
