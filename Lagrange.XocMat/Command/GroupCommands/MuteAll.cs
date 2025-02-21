using Lagrange.Core.Common.Interface.Api;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class MuteAll : Command
{
    public override string[] Alias => ["ȫ��"];
    public override string HelpText => "ȫ�����";
    public override string[] Permissions => [OneBotPermissions.Mute];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            switch (args.Parameters[0])
            {
                case "����":
                case "��":
                case "true":
                    await args.Bot.MuteGroupGlobal(args.GroupUin, true);
                    await args.Event.Reply("�����ɹ���");
                    break;
                case "�ر�":
                case "��":
                case "false":
                    await args.Bot.MuteGroupGlobal(args.GroupUin, false);
                    await args.Event.Reply("�رճɹ�");
                    break;
                default:
                    await args.Event.Reply("�﷨����,��ȷ�﷨:\nȫ�� [����|�ر�]");
                    break;
            }
        }
        else
        {
            await args.Event.Reply("�﷨����,��ȷ�﷨:\nȫ�� [����|�ر�]");
        }
    }
}
