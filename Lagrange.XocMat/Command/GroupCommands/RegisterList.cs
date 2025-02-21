using System.Text;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class RegisterList : Command
{
    public override string[] Alias => ["ע���б�"];
    public override string HelpText => "ע���б�";
    public override string[] Permissions => [OneBotPermissions.QueryUserList];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.MemberUin, args.GroupUin, out Terraria.TerrariaServer? server) && server != null)
        {
            List<TerrariaUser> users = TerrariaUser.GetUsers(server.Name);
            if (users == null || users.Count == 0)
            {
                await args.Event.Reply("ע���б�տ���Ҳ!");
                return;
            }
            StringBuilder sb = new StringBuilder($"[{server.Name}]ע���б�\n");
            foreach (TerrariaUser user in users)
            {
                sb.AppendLine($"{user.Name} => {user.Id}");
            }
            await args.Event.Reply(sb.ToString().Trim());
        }
        else
        {
            await args.Event.Reply("δ�л����������������Ч!", true);
        }
    }
}
