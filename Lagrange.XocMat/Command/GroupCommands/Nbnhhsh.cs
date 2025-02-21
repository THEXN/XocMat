using System.Text.Json.Nodes;
using Lagrange.XocMat.Command.CommandArgs;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal;

namespace Lagrange.XocMat.Command.GroupCommands;

public class Nbnhhsh : Command
{
    public override string[] Alias => ["��д"];
    public override string HelpText => "��ѯ��д";
    public override string[] Permissions => [OneBotPermissions.Nbnhhsh];

    public override async Task InvokeAsync(GroupCommandArgs args)
    {
        if (args.Parameters.Count == 1)
        {
            string url = $"https://oiapi.net/API/Nbnhhsh?text={args.Parameters[0]}";
            HttpClient client = new();
            string result = await client.GetStringAsync(url);
            JsonNode? data = JsonNode.Parse(result);
            JsonArray? trans = data?["data"]?[0]?["trans"]?.AsArray();
            if (trans != null && trans.Any())
            {
                await args.Event.Reply($"��д:`{args.Parameters[0]}`����Ϊ:\n{string.Join(",", trans)}");
            }
            else
            {
                await args.Event.Reply("Ҳ�����дû�б���¼!");
            }
        }
        else
        {
            await args.Event.Reply($"�﷨������ȷ�﷨:{args.CommamdPrefix}��д [�ı�]");
        }
    }
}
