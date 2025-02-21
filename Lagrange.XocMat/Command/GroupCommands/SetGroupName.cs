using Lagrange.Core.Common.Interface.Api;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class SetGroupName : Command
{
    public override string[] Alias => ["����Ⱥ��"];
    public override string HelpText => "����Ⱥ��";
    public override string[] Permissions => [OneBotPermissions.ChangeGroupOption];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            if (string.IsNullOrEmpty(args.Parameters[0]))
            {
                await args.Event.Reply("Ⱥ������δ�գ�");
                return;
            }
            await args.Bot.RenameGroup(args.GroupUin, args.Parameters[0]);
            await args.Event.Reply($"Ⱥ�������޸�Ϊ`{args.Parameters[0]}`");
        }
        else
        {
            await args.Event.Reply($"�﷨����,��ȷ�﷨:\n{args.CommamdPrefix}����Ⱥ�� [����]");
        }
    }
}
