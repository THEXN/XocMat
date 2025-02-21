using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class ChangeServer : Command
{
    public override string[] Alias => ["�л�"];
    public override string HelpText => "�л�������";
    public override string[] Permissions => [OneBotPermissions.ChangeServer];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            Terraria.TerrariaServer? server = XocMatSetting.Instance.GetServer(args.Parameters[0], args.GroupUin);
            if (server == null)
            {
                await args.Event.Reply("���л��ķ�����������! ��������������Ƿ���ȷ����Ⱥ�Ƿ����÷�����!");
                return;
            }
            UserLocation.Instance.Change(args.MemberUin, server);
            await args.Event.Reply($"���л���`{server.Name}`������", true);
        }
        else
        {
            await args.Event.Reply($"�﷨����,��ȷ�﷨:\n{args.CommamdPrefix}{args.Name} [����������]");
        }
    }
}
