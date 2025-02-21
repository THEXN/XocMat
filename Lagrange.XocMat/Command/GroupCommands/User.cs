using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Exceptions;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class User : Command
{
    public override string[] Alias => ["user"];
    public override string HelpText => "user����";
    public override string[] Permissions => [OneBotPermissions.UserAdmin];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (UserLocation.Instance.TryGetServer(args.Event.Chain.GroupMemberInfo!.Uin, args.Event.Chain.GroupUin!.Value, out Terraria.TerrariaServer? server) && server != null)
        {
            if (args.Parameters.Count == 2)
            {
                switch (args.Parameters[0].ToLower())
                {
                    case "del":
                        try
                        {
                            TerrariaUser.Remove(server.Name, args.Parameters[1]);
                            await args.Event.Reply("�Ƴ��ɹ�!", true);
                        }
                        catch (TerrariaUserException ex)
                        {
                            await args.Event.Reply(ex.Message);
                        }
                        break;
                    default:
                        await args.Event.Reply("δ֪������!");
                        break;
                }
            }
        }
        else
        {
            await args.Event.Reply("δ�л����������������Ч!", true);
        }
    }
}
