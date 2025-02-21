using Lagrange.Core.Common.Interface.Api;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class UnsetGroupAdmin : Command
{
    public override string[] Alias => ["ȡ������"];
    public override string HelpText => "ȡ������";
    public override string[] Permissions => [OneBotPermissions.ChangeGroupOption];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count == 0)
        {
            Core.Message.Entity.MentionEntity atlist = args.Event.Chain.GetMention().First();
            if (atlist != null)
            {
                await args.Bot.SetGroupAdmin(args.Event.Chain.GroupUin!.Value, atlist.Uin, false);
                await args.Event.Reply($"��ȡ��`{atlist.Uin}`�Ĺ���Ա!");
            }
            else
            {
                await args.Event.Reply("��ѡ��һλ��Ա��");
            }
        }
        else
        {
            await args.Event.Reply($"�﷨����,��ȷ�﷨:\n{args.CommamdPrefix}{args.Name} [AT]");
        }
    }
}
