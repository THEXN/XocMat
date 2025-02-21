using Lagrange.Core.Common.Interface.Api;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class UnMute : Command
{
    public override string[] Alias => ["��"];
    public override string HelpText => "�������";
    public override string[] Permissions => [OneBotPermissions.Mute];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count == 0)
        {
            IEnumerable<Core.Message.Entity.MentionEntity> atlist = args.Event.Chain.GetMention();
            if (!atlist.Any())
            {
                await args.Event.Reply("δָ��Ŀ���Ա!");
                return;
            }
            atlist.ForEach(async x => await args.Bot.MuteGroupMember(args.GroupUin, x.Uin, 0));
            await args.Event.Reply("����ɹ���");
        }
        else
        {
            await args.Event.Reply($"�﷨����,��ȷ�﷨:\n{args.CommamdPrefix}�� [AT] [ʱ��]��");
        }
    }
}
