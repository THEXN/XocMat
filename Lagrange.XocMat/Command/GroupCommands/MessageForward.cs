using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class MessageForward : Command
{
    public override string[] Alias => ["��Ϣת��"];
    public override string HelpText => "��Ϣת��";
    public override string[] Permissions => [OneBotPermissions.ForwardMsg];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
            {
                switch (args.Parameters[0])
                {
                    case "����":
                    case "true":
                        server.ForwardGroups.Add(args.GroupUin);
                        await args.Event.Reply("�����ɹ�", true);
                        break;
                    case "�ر�":
                    case "false":
                        server.ForwardGroups.Remove(args.GroupUin);
                        await args.Event.Reply("�رճɹ�", true);
                        break;
                    default:
                        await args.Event.Reply("δ֪�����", true);
                        break;
                }
                XocMatSetting.Instance.SaveTo();
            }
            else
            {
                await args.Event.Reply("δ�л����������������Ч!", true);
            }
        }
        else
        {
            await args.Event.Reply($"�﷨����,��ȷ�﷨:\n{args.CommamdPrefix}{args.Name} [����|�ر�]!", true);
        }
    }
}
