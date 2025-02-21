using System.Text.RegularExpressions;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Exceptions;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;
using Lagrange.XocMat.Utility;

namespace Lagrange.XocMat.Command.GroupCommands;

public class Register : Command
{
    public override string[] Alias => ["ע��"];
    public override string HelpText => "ע��";
    public override string[] Permissions => [OneBotPermissions.RegisterUser];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            if (!UserLocation.Instance.TryGetServer(args.Event.Chain.GroupMemberInfo!.Uin, args.Event.Chain.GroupUin!.Value, out Terraria.TerrariaServer? server) || server == null)
            {
                await args.Event.Reply("δ�л����������������Ч!", true);
                return;
            }
            if (args.Parameters[0].Length > server.RegisterNameMax)
            {
                await args.Event.Reply($"ע����������Ʋ��ܴ���{server.RegisterNameMax}���ַ�!", true);
                return;
            }
            if (!new Regex("^[a-zA-Z0-9\u4e00-\u9fa5\\[\\]:/ ]+$").IsMatch(args.Parameters[0]) && server.RegisterNameLimit)
            {
                await args.Event.Reply("ע����������Ʋ��ܰ�������,��ĸ,���ֺ�/:[]������ַ�", true);
                return;
            }
            if (TerrariaUser.GetUserById(args.Event.Chain.GroupMemberInfo!.Uin, server.Name).Count >= server.RegisterMaxCount)
            {
                await args.Event.Reply($"ͬһ����������ע���˻����ܳ���{server.RegisterMaxCount}��", true);
                return;
            }
            string pass = Guid.NewGuid().ToString()[..8];
            try
            {
                TerrariaUser.Add(args.Event.Chain.GroupMemberInfo!.Uin, args.Event.Chain.GroupUin!.Value, server.Name, args.Parameters[0], pass);
                Internal.Socket.Action.Response.BaseActionResponse api = await server.Register(args.Parameters[0], pass);
                Core.Message.MessageBuilder build = args.MessageBuilder;
                if (api.Status)
                {
                    MailHelper.SendMail($"{args.Event.Chain.GroupMemberInfo!.Uin}@qq.com",
                        $"{server.Name}������ע������",
                        $"����ע��������:{pass}<br>��ע�Ᵽ�治Ҫ��¶������");
                    build.Text($"ע��ɹ�!" +
                        $"\nע�������: {server.Name}" +
                        $"\nע������: {args.Parameters[0]}" +
                        $"\nע���˺�: {args.Event.Chain.GroupMemberInfo!.Uin}" +
                        $"\nע�����ǳ�: {args.Event.Chain.GroupMemberInfo!.MemberName}" +
                        $"\nע�������ѷ�����QQ���������·����Ӳ鿴" +
                        $"\nhttps://wap.mail.qq.com/home/index" +
                        $"\n������������ʹ��/password [��ǰ����] [������] �޸��������");
                }
                else
                {
                    TerrariaUser.Remove(server.Name, args.Parameters[0]);
                    build.Text(string.IsNullOrEmpty(api.Message) ? "�޷����ӷ�������" : api.Message);
                }
                await args.Event.Reply(build);
            }
            catch (TerrariaUserException ex)
            {
                await args.Event.Reply(ex.Message);
            }
        }
        else
        {
            await args.Event.Reply($"�﷨����,��ȷ�﷨:\n{args.CommamdPrefix}{args.Name} [����]");
        }
    }
}
