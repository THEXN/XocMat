using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Enumerates;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class ResetTShock : Command
{
    public override string[] Alias => ["̩������������"];
    public override string HelpText => "���÷�����";
    public override string[] Permissions => [OneBotPermissions.StartTShock];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
        {
            TerrariaUser.RemoveByServer(server.Name);
            await server.Reset(args.CommamdLine, async type =>
            {
                switch (type)
                {
                    case RestServerType.WaitFile:
                        {
                            await args.Event.Reply("���ڵȴ��ϴ���ͼ��60���ʧЧ!");
                            break;
                        }
                    case RestServerType.TimeOut:
                        {
                            await args.Event.Reply("��ͼ�ϴ���ʱ���Զ�������ͼ��");
                            break;
                        }
                    case RestServerType.Success:
                        {
                            await args.Event.Reply("�������÷�����!!");
                            break;
                        }
                    case RestServerType.LoadFile:
                        {
                            await args.Event.Reply("�ѽ��ܵ���ͼ�������ϴ�������!!");
                            break;
                        }
                    case RestServerType.UnLoadFile:
                        {
                            await args.Event.Reply("�ϴ��ĵ�ͼ�ǹ������棬���ͼ���Ϸ����뾡����д�ϴ�!");
                            break;
                        }
                }
            });
        }
        else
        {
            await args.Event.Reply("δ�л����������������Ч!", true);
        }
    }
}
