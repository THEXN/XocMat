using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Microsoft.Extensions.Logging;

namespace Lagrange.XocMat.Command.GroupCommands;

public class KickCommand : Command
{
    public override string[] Alias => ["踢", "kick"];
    public override string HelpText => "踢出群成员";
    public override string[] Permissions => [OneBotPermissions.Kick];

    public override async Task InvokeAsync(GroupCommandArgs args, ILogger log)
    {
        var member = args.Event.Chain.GetEntity<MentionEntity>();
        if (member == null)
        {
            await args.MessageBuilder.Text("请 @ 需要踢出的成员").Reply();
            return;
        }
        else if (member.Uin == args.MemberUin)
        {
            await args.MessageBuilder.Text("不能踢出自己哦").Reply();
            return;
        }
        else if (member.Uin == args.Bot.BotUin)
        {
            await args.MessageBuilder.Text("不能踢出我哦").Reply();
            return;
        }
        else if (args.IsAdmin != Core.Common.Entity.GroupMemberPermission.Owner && member.Uin != args.MemberUin)
        {
            await args.MessageBuilder.Text("只有群主可以踢出其他管理员").Reply();
            return;
        }
        else
        { 
            var success = await args.Bot.KickGroupMember(args.GroupUin, member.Uin, false);
            if (success) await args.MessageBuilder.Text($"已将 {member.Uin} 踢出群聊").Reply();
            else await args.MessageBuilder.Text($"踢出 {member.Uin} 失败，请检查权限").Reply();
        }
    }

}
