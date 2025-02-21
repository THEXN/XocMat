using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class AcountInfo : Command
{
    public override string[] Alias => ["��"];
    public override string HelpText => "��ѯ������Ϣ";
    public override string[] Permissions => [OneBotPermissions.OtherInfo];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        IEnumerable<Core.Message.Entity.MentionEntity> at = args.Event.Chain.GetMention();
        if (at.Any())
        {
            Account group = Account.GetAccountNullDefault(at.First().Uin);
            await args.Event.Reply(await CommandUtils.GetAccountInfo(args.Event.Chain.GroupUin!.Value, at.First().Uin, group.Group.Name));
        }
        else if (args.Parameters.Count == 1 && uint.TryParse(args.Parameters[0], out uint id))
        {
            Account group = Account.GetAccountNullDefault(id);
            await args.Event.Reply(await CommandUtils.GetAccountInfo(args.Event.Chain.GroupUin!.Value, id, group.Group.Name));
        }
        else
        {
            await args.Event.Reply("��˭��?", true);
        }
    }
}
