using System.Text;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class KillRank : Command
{
    public override string[] Alias => ["��ɱ����"];
    public override string HelpText => "��ɱ����";
    public override string[] Permissions => [OneBotPermissions.KillRank];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
        {
            Internal.Socket.Action.Response.PlayerStrikeBoss data = await server.GetStrikeBoss();
            if (data.Damages != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Internal.Socket.Internet.KillNpc? damage in data.Damages.OrderByDescending(x => x.KillTime))
                {
                    sb.AppendLine($"Boss: {damage.Name}");
                    sb.AppendLine($"��Ѫ��: {damage.MaxLife}");
                    sb.AppendLine($"����ʱ��: {damage.KillTime:yyyy-MM-dd HH:mm:ss}");
                    sb.AppendLine($"״̬: {(damage.IsAlive ? "δ����ɱ" : "�ѱ���ɱ")}");
                    if (!damage.IsAlive)
                    {
                        sb.AppendLine($"��ɱ��ʱ: {(damage.KillTime - damage.SpawnTime).TotalSeconds}��");
                        sb.AppendLine($"��ʧ�˺�: {damage.MaxLife - damage.Strikes.Sum(x => x.Damage)}");
                    }
                    foreach (Internal.Socket.Internet.PlayerStrike? strike in damage.Strikes.OrderByDescending(x => x.Damage))
                    {
                        sb.AppendLine($"{strike.Player}�˺� {Convert.ToSingle(strike.Damage) / damage.MaxLife * 100:F0}%({strike.Damage})");
                    }
                    sb.AppendLine();
                }
                await args.Event.Reply(sb.ToString().Trim());
            }
            else
            {
                await args.Event.Reply("���޻�ɱ���ݿ���ͳ��!", true);
            }
        }
        else
        {
            await args.Event.Reply("δ�л����������������Ч!", true);
        }
    }
}
