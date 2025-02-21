using Lagrange.Core.Common.Interface.Api;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class SetGroupAdmin : Command
{
    public override string[] Alias => ["���ù���"];
    public override string HelpText => "���ù���";
    public override string[] Permissions => [OneBotPermissions.ChangeGroupOption];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count == 0)
        {
            Core.Message.Entity.MentionEntity atlist = args.Event.Chain.GetMention().First();
            if (atlist != null)
            {
                await args.Bot.SetGroupAdmin(args.GroupUin, atlist.Uin, true);
                await args.Event.Reply($"�ѽ�`{atlist.Uin}`����Ϊ����Ա!");
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
