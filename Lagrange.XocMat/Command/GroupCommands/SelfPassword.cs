using System.Text;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Lagrange.XocMat.Utility;

namespace Lagrange.XocMat.Command.GroupCommands;

public class SelfPassword : Command
{
    public override string[] Alias => ["�ҵ�����"];
    public override string HelpText => "��ѯ�Լ�������";
    public override string[] Permissions => [OneBotPermissions.SelfPassword];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
        {
            List<TerrariaUser> user = TerrariaUser.GetUserById(args.MemberUin, server.Name);
            if (user.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (TerrariaUser u in user)
                    sb.AppendLine($"����{u.Name}��ע������Ϊ: {u.Password}");
                sb.AppendLine("��ע�Ᵽ�治Ҫ��¶������");
                MailHelper.SendMail($"{args.MemberUin}@qq.com",
                            $"{server.Name}������ע������",
                            sb.ToString().Trim());
                await args.Event.Reply("�����ѯ�ɹ��ѷ��������QQ���䡣", true);
                return;
            }
            await args.Event.Reply($"{server.Name}��δ�ҵ����ע����Ϣ��");
            return;
        }
        await args.Event.Reply("��������Ч��δ�л���һ����Ч������!");
    }
}
