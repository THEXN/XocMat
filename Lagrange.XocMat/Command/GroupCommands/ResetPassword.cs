using System.Text;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Lagrange.XocMat.Utility;

namespace Lagrange.XocMat.Command.GroupCommands;

public class ResetPassword : Command
{
    public override string[] Alias => ["��������"];
    public override string HelpText => "��������";
    public override string[] Permissions => [OneBotPermissions.SelfPassword];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.Event.Chain.GroupMemberInfo!.Uin, args.Event.Chain.GroupUin!.Value, out Terraria.TerrariaServer? server) && server != null)
        {
            try
            {
                List<TerrariaUser> user = TerrariaUser.GetUserById(args.Event.Chain.GroupMemberInfo!.Uin, server.Name);

                if (user.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (TerrariaUser u in user)
                    {
                        string pwd = Guid.NewGuid().ToString()[..8];
                        sb.Append($"���� {u.Name}����������Ϊ: {pwd}<br>");
                        Internal.Socket.Action.Response.BaseActionResponse res = await server.ResetPlayerPwd(u.Name, pwd);
                        if (!res.Status)
                        {
                            await args.Event.Reply("�޷����ӵ���������������!");
                            return;
                        }
                        TerrariaUser.ResetPassword(args.Event.Chain.GroupMemberInfo!.Uin, server.Name, u.Name, pwd);
                    }
                    sb.Append("��ע�Ᵽ�治Ҫ��¶������");
                    MailHelper.SendMail($"{args.Event.Chain.GroupMemberInfo!.Uin}@qq.com",
                                $"{server.Name}����������",
                                sb.ToString().Trim());
                    await args.Event.Reply("�������óɹ��ѷ��������QQ���䡣", true);
                    return;
                }
                await args.Event.Reply($"{server.Name}��δ�ҵ����ע����Ϣ��");
                return;
            }
            catch (Exception ex)
            {
                await args.Event.Reply(ex.Message, true);
            }
        }
        await args.Event.Reply("��������Ч��δ�л���һ����Ч������!");
    }
}
