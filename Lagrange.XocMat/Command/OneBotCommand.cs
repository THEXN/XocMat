﻿using Lagrange.Core.Common.Entity;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat.Attributes;
using Lagrange.XocMat.Commands;
using Lagrange.XocMat.Configuration;
using Lagrange.XocMat.DB.Manager;
using Lagrange.XocMat.Enumerates;
using Lagrange.XocMat.Event;
using Lagrange.XocMat.EventArgs;
using Lagrange.XocMat.Exceptions;
using Lagrange.XocMat.Extensions;
using Lagrange.XocMat.Internal.Terraria;
using Lagrange.XocMat.Permission;
using Lagrange.XocMat.Terraria.Picture;
using Lagrange.XocMat.Utility;
using Lagrange.XocMat.Utility.Images;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Web;


namespace Lagrange.XocMat.Command;

//[CommandSeries]
//public class OneBotCommand
//{
//    [CommandMap("泰拉商店")]
//    [CommandPermission(OneBotPermissions.TerrariaShop)]
//    public static async ValueTask Shop(CommandArgs args)
//    {
//        var tableBuilder = new TableBuilder()
//            .SetTitle("泰拉商店")
//            .AddRow("商品ID", "商品名称", "数量", "价格");
//        var id = 1;
//        foreach (var item in TerrariaShop.Instance.TrShop)
//        {
//            tableBuilder.AddRow(item.ID.ToString(), item.Name, item.Num.ToString(), item.Price.ToString());
//            id++;
//        }
//        await args.MessageBuilder.Image(await tableBuilder.BuildAsync()).Reply();
//    }
//#endregion

//    #region 泰拉奖池管理
//    [CommandMap("prize")]
//    [CommandPermission(OneBotPermissions.TerrariaPrizeAdmin)]
//    public static async ValueTask PrizeManager(CommandArgs args)
//    {
//        if (args.Parameters.Count == 5 && args.Parameters[0].ToLower() == "add")
//        {
//            if (!int.TryParse(args.Parameters[1], out int id) || id < 1)
//            {
//                await args.EventArgs.Reply("请输入一个正确的物品ID", true);
//                return;
//            }
//            if (!int.TryParse(args.Parameters[2], out int max) || max < 0)
//            {
//                await args.EventArgs.Reply("请输入一个正确最大数", true);
//                return;
//            }
//            if (!int.TryParse(args.Parameters[3], out int min) || min < 0)
//            {
//                await args.EventArgs.Reply("请输入一个正确最小数", true);
//                return;
//            }
//            if (!int.TryParse(args.Parameters[4], out int rate) || rate < 0)
//            {
//                await args.EventArgs.Reply("请输入一个正确概率", true);
//                return;
//            }
//            var item = SystemHelper.GetItemById(id);
//            if (item == null)
//            {
//                await args.EventArgs.Reply("没有找到此物品", true);
//                return;
//            }
//            TerrariaPrize.Instance.Add(item.Name, id, rate, max, min);
//            await args.EventArgs.Reply("添加成功", true);
//            TerrariaPrize.Save();
//        }
//        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "del")
//        {
//            if (!int.TryParse(args.Parameters[1], out int index))
//            {
//                await args.EventArgs.Reply("请输入一个正确序号", true);
//                return;
//            }
//            var prize = TerrariaPrize.Instance.GetPrize(index);
//            if (prize == null)
//            {
//                await args.EventArgs.Reply("该奖品不存在!", true);
//                return;
//            }
//            TerrariaPrize.Instance.Remove(prize);
//            await args.EventArgs.Reply("删除成功", true);
//            TerrariaPrize.Save();
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n" +
//                $"{args.CommamdPrefix}{args.Name} add [物品id] [最大数] [最小数] [概率]\n" +
//                $"{args.CommamdPrefix}{args.Name} del [奖品序号]");
//        }
//    }
//    #endregion

//    #region 泰拉商店管理
//    [CommandMap("shop")]
//    [CommandPermission(OneBotPermissions.TerrariaShopAdmin)]
//    public static async ValueTask ShopManager(CommandArgs args)
//    {
//        if (args.Parameters.Count == 4 && args.Parameters[0].ToLower() == "add")
//        {
//            if (!int.TryParse(args.Parameters[1], out int id) || id < 1)
//            {
//                await args.EventArgs.Reply("请输入一个正确的物品ID", true);
//                return;
//            }
//            if (!int.TryParse(args.Parameters[2], out int cost) || cost < 0)
//            {
//                await args.EventArgs.Reply("请输入一个正确价格", true);
//                return;
//            }
//            if (!int.TryParse(args.Parameters[3], out int num) || num < 0)
//            {
//                await args.EventArgs.Reply("请输入一个正确数量", true);
//                return;
//            }
//            var item = SystemHelper.GetItemById(id);
//            if (item != null)
//            {
//                TerrariaShop.Instance.TrShop.Add(new Shop(item.Name, id, cost, num));
//                await args.EventArgs.Reply("添加成功", true);
//                TerrariaShop.Save();
//            }
//            else
//            {
//                await args.EventArgs.Reply("没有找到此物品", true);
//            }
//        }
//        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "del")
//        {
//            if (!int.TryParse(args.Parameters[1], out int index))
//            {
//                await args.EventArgs.Reply("请输入一个正确序号", true);
//                return;
//            }
//            var shop = TerrariaShop.Instance.GetShop(index);
//            if (shop == null)
//            {
//                await args.EventArgs.Reply("该商品不存在!", true);
//                return;
//            }
//            TerrariaShop.Instance.TrShop.Remove(shop);
//            await args.EventArgs.Reply("删除成功", true);
//            TerrariaShop.Save();
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n" +
//                $"{args.CommamdPrefix}{args.Name} add [物品id] [价格] [数量]\n" +
//                $"{args.CommamdPrefix}{args.Name} del [商品序号]");
//        }
//    }
//    #endregion

//    #region 泰拉奖池
//    [CommandMap("泰拉奖池")]
//    [CommandPermission(OneBotPermissions.TerrariaPrize)]
//    public static async ValueTask Prize(CommandArgs args)
//    {
//        var tableBuilder = new TableBuilder()
//            .SetTitle("泰拉奖池")
//            .AddRow("奖品ID", "奖品名称", "最大数量", "最小数量", "中奖概率");
//        var id = 1;
//        foreach (var item in TerrariaPrize.Instance.Pool)
//        {
//            tableBuilder.AddRow(id.ToString(), item.Name, item.Max.ToString(), item.Min.ToString(), item.Probability.ToString());
//            id++;
//        }
//        var s = await args.MessageBuilder.Image(await tableBuilder.BuildAsync()).Reply();
//        Console.WriteLine(s.Result);
//    }
//    #endregion

//    #region 清理内存
//    [CommandMap("清理内存")]
//    [CommandPermission(OneBotPermissions.ClearMemory)]
//    public static async ValueTask Memory(CommandArgs args)
//    {
//        var old = SystemHelper.GetUsedPhys();
//        SystemHelper.FreeMemory();
//        var curr = old - SystemHelper.GetUsedPhys();
//        await args.EventArgs.Reply($"已释放内存:{SystemHelper.FormatSize(curr)}");
//    }
//    #endregion

//    #region 搜索物品
//    [CommandMap("sitem")]
//    [CommandPermission(OneBotPermissions.SearchItem)]
//    public static async ValueTask SearchItem(CommandArgs args)
//    {
//        if (args.Parameters.Count > 0)
//        {
//            var items = SystemHelper.GetItemByIdOrName(args.Parameters[0]);
//            await args.EventArgs.Reply(items.Count == 0 ? "未查询到指定物品" : $"查询结果:\n{string.Join(",", items.Select(x => $"{x.Name}({x.netID})"))}");
//        }

//    }
//    #endregion

//    #region 帮助
//    [CommandMap("help")]
//    [CommandPermission(OneBotPermissions.Help)]
//    public async ValueTask Help(CommandArgs args)
//    {
//        void Show(List<string> line)
//        {
//            if (PaginationTools.TryParsePageNumber(args.Parameters, 0, args.EventArgs, out int page))
//            {
//                PaginationTools.SendPage(args.EventArgs, page, line, new PaginationTools.Settings()
//                {
//                    MaxLinesPerPage = 10,
//                    NothingToDisplayString = "当前没有指令可用",
//                    HeaderFormat = "指令列表 ({0}/{1})：",
//                    FooterFormat = $"输入 {args.CommamdPrefix}help {{0}} 查看更多"
//                });
//            }
//        }
//        var commands = XocMatAPI.Command!.GroupCommandDelegate.Select(x => args.CommamdPrefix + x.Name.First()).ToList();
//        Show(commands);
//        await ValueTask.CompletedTask;
//    }
//    #endregion

//    #region 签到排行
//    [CommandMap("签到排行")]
//    [CommandPermission(OneBotPermissions.Sign)]
//    public static async ValueTask SignRank(CommandArgs args)
//    {
//        try
//        {
//            var signs = DB.Manager.Sign.GetSigns().Where(x => x.GroupID == args.EventArgs.Chain.GroupUin!.Value).OrderByDescending(x => x.Date).Take(10);
//            var sb = new StringBuilder("签到排行\n\n");
//            int i = 1;
//            foreach (var sign in signs)
//            {
//                sb.AppendLine($"签到排名: {i}");
//                sb.AppendLine($"账号: {sign.UserId}");
//                sb.AppendLine($"时长: {sign.Date}");
//                sb.AppendLine();
//                i++;
//            }

//            await args.EventArgs.Reply(sb.ToString().Trim());
//        }
//        catch (Exception e)
//        {
//            await args.EventArgs.Reply(e.Message);
//        }
//    }
//    #endregion

//    #region 签到
//    [CommandMap("签到")]
//    [CommandPermission(OneBotPermissions.Sign)]
//    public static async ValueTask Sign(CommandArgs args)
//    {
//        try
//        {
//            var rand = new Random();
//            long num = rand.NextInt64(XocMatSetting.Instance.SignMinCurrency, XocMatSetting.Instance.SignMaxCurrency);
//            var result = DB.Manager.Sign.SingIn(args.EventArgs.Chain.GroupUin!.Value, args.EventArgs.Chain.GroupMemberInfo!.Uin);
//            var currency = DB.Manager.Currency.Add(args.EventArgs.Chain.GroupUin!.Value, args.EventArgs.Chain.GroupMemberInfo!.Uin, num);
//            await args.MessageBuilder
//                .Image(await HttpUtils.HttpGetByte($"http://q.qlogo.cn/headimg_dl?dst_uin={args.EventArgs.Chain.GroupMemberInfo!.Uin}&spec=640&img_type=png"))
//                .Text($"签到成功！\n")
//                .Text($"[签到时长]：{result.Date}\n")
//                .Text($"[获得{XocMatSetting.Instance.Currency}]：{num}\n")
//                .Text($"[{XocMatSetting.Instance.Currency}总数]：{currency.Num}")
//                .Reply();
//        }
//        catch (Exception e)
//        {
//            await args.EventArgs.Reply(e.Message);
//        }
//    }
//    #endregion

//    #region 重读
//    [CommandMap("reload")]
//    [CommandPermission(OneBotPermissions.Reload)]
//    public static async ValueTask Reload(CommandArgs args)
//    {
//        var sw = new Stopwatch();
//        sw.Start();
//        var reloadArgs = new ReloadEventArgs(args.EventArgs.Chain.GroupUin!.Value);
//        await OperatHandler.Reload(reloadArgs);
//        sw.Stop();
//        reloadArgs.Message.Text($"所有配置文件已成功重新加载，耗时 {sw.Elapsed.TotalSeconds:F5} 秒。");
//        await args.EventArgs.Reply(reloadArgs.Message);
//    }
//    #endregion

//    #region Terraria账号信息
//    [CommandMap("ui")]
//    [CommandPermission(OneBotPermissions.QueryUserList)]
//    public static async ValueTask UserInfo(CommandArgs args)
//    {
//        if (!UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) || server == null)
//        {
//            await args.EventArgs.Reply("服务器不存在或，未切换至一个服务器！", true);
//            return;
//        }
//        if (args.Parameters.Count == 1)
//        {
//            var userName = args.Parameters[0];
//            var info = await server.QueryAccount(userName);
//            var account = info.Accounts.Find(x => x.Name == userName);
//            if (!info.Status || account == null)
//            {
//                await args.EventArgs.Reply(info.Message, true);
//                return;
//            }

//            var sb = new StringBuilder($"查询`{userName}\n");
//            sb.AppendLine($"ID: {account.ID}");
//            sb.AppendLine($"Group: {account.Group}");
//            sb.AppendLine($"LastLogin: {account.LastLoginTime}");
//            sb.AppendLine($"Registered: {account.RegisterTime}");
//            await args.EventArgs.Reply(sb.ToString().Trim());
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误，正确语法:\n{args.CommamdPrefix}{args.Name} [名称]");
//            return;
//        }
//    }
//    #endregion

//    #region 账户管理
//    [CommandMap("account")]
//    [CommandPermission(OneBotPermissions.Account)]
//    public static async ValueTask Account(CommandArgs args)
//    {
//        void Show(List<string> line)
//        {
//            if (PaginationTools.TryParsePageNumber(args.Parameters, 1, args.EventArgs, out int page))
//            {
//                PaginationTools.SendPage(args.EventArgs, page, line, new PaginationTools.Settings()
//                {
//                    MaxLinesPerPage = 6,
//                    NothingToDisplayString = "当前没有账户",
//                    HeaderFormat = "账户列表 ({0}/{1})：",
//                    FooterFormat = $"输入 {args.CommamdPrefix}account list {{0}} 查看更多"
//                });
//            }
//        }
//        var atList = args.EventArgs.Chain.GetMention();
//        if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "add" && atList.Any())
//        {
//            try
//            {
//                DB.Manager.Account.AddAccount(atList.First().Uin, args.Parameters[1]);
//                await args.EventArgs.Reply($"{atList.First().Uin} 已添加到组 {args.Parameters[1]}");
//            }
//            catch (Exception ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "add")
//        {
//            if (long.TryParse(args.Parameters[1], out long id))
//            {
//                try
//                {
//                    DB.Manager.Account.AddAccount(id, args.Parameters[2]);
//                    await args.EventArgs.Reply($"{id} 已添加到组 {args.Parameters[2]}");
//                }
//                catch (Exception ex)
//                {
//                    await args.EventArgs.Reply(ex.Message);
//                }
//            }
//            else
//            {
//                await args.EventArgs.Reply("错误的QQ账号，无法转换!");
//            }
//        }
//        else if (args.Parameters.Count == 1 && args.Parameters[0].ToLower() == "del" && args.EventArgs.Chain.GetMention().Any())
//        {
//            try
//            {
//                DB.Manager.Account.RemoveAccount(atList.First().Uin);
//                await args.EventArgs.Reply($"删除成功!");
//            }
//            catch (Exception ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "del")
//        {
//            if (long.TryParse(args.Parameters[1], out long id))
//            {
//                try
//                {
//                    DB.Manager.Account.RemoveAccount(id);
//                    await args.EventArgs.Reply($"删除成功!");
//                }
//                catch (Exception ex)
//                {
//                    await args.EventArgs.Reply(ex.Message);
//                }
//            }
//            else
//            {
//                await args.EventArgs.Reply("错误的QQ账号，无法转换!");
//            }
//        }
//        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "group" && atList.Any())
//        {
//            try
//            {
//                DB.Manager.Account.ReAccountGroup(atList.First().Uin, args.Parameters[1]);
//                await args.EventArgs.Reply($"账户 {atList.First().Uin} 的组 成功更改为 {args.Parameters[1]}");
//            }
//            catch (Exception ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "group")
//        {
//            if (long.TryParse(args.Parameters[1], out long id))
//            {
//                try
//                {
//                    DB.Manager.Account.ReAccountGroup(id, args.Parameters[1]);
//                    await args.EventArgs.Reply($"账户 {id} 的组 成功更改为 {args.Parameters[1]}");
//                }
//                catch (Exception ex)
//                {
//                    await args.EventArgs.Reply(ex.Message);
//                }
//            }
//            else
//            {
//                await args.EventArgs.Reply("错误的QQ账号，无法转换!");
//            }
//        }
//        else if (args.Parameters.Count >= 1 && args.Parameters[0].ToLower() == "list")
//        {
//            try
//            {
//                var accounts = DB.Manager.Account.Accounts;
//                var lines = accounts.Select(x => $"\n账户:{x.UserId}\n权限:{x.Group.Name}");
//                Show(lines.ToList());
//            }
//            catch (AccountException ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        else
//        {
//            var build = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value)
//                .Text("语法错误，正确的语法:\n")
//                .Text($"{args.CommamdPrefix}account add <组> at\n")
//                .Text($"{args.CommamdPrefix}account del <组> at\n")
//                .Text($"{args.CommamdPrefix}account add <QQ> <组>\n")
//                .Text($"{args.CommamdPrefix}account del <QQ> <组>\n")
//                .Text($"{args.CommamdPrefix}account group <组> at\n")
//                .Text($"{args.CommamdPrefix}account group <QQ> <组>\n")
//                .Text($"{args.CommamdPrefix}account list");
//            await args.EventArgs.Reply(build);
//        }
//    }
//    #endregion

//    #region 权限组管理
//    [CommandMap("group")]
//    [CommandPermission(OneBotPermissions.Group)]
//    public static async ValueTask Group(CommandArgs args)
//    {
//        if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "add")
//        {
//            try
//            {
//                DB.Manager.Group.AddGroup(args.Parameters[1]);
//                await args.EventArgs.Reply($"组 {args.Parameters[1]} 添加成功!");
//            }
//            catch (GroupException ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        删除组
//        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "del")
//        {
//            try
//            {
//                DB.Manager.Group.RemoveGroup(args.Parameters[1]);
//                await args.EventArgs.Reply($"组 {args.Parameters[1]} 删除成功!");
//            }
//            catch (GroupException ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        更改父组
//        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "parent")
//        {
//            try
//            {
//                DB.Manager.Group.ReParentGroup(args.Parameters[1], args.Parameters[2]);
//                await args.EventArgs.Reply($"组 {args.Parameters[1]} 的组已更改为 {args.Parameters[2]}!");
//            }
//            catch (Exception ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        添加权限
//        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "addperm")
//        {
//            try
//            {
//                DB.Manager.Group.AddPerm(args.Parameters[1], args.Parameters[2]);
//                await args.EventArgs.Reply($"权限添加成功!");
//            }
//            catch (GroupException ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        删除权限
//        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "delperm")
//        {
//            try
//            {
//                DB.Manager.Group.RemovePerm(args.Parameters[1], args.Parameters[2]);
//                await args.EventArgs.Reply($"权限删除成功!");
//            }
//            catch (GroupException ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        else
//        {
//            var build = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value)

//                .Text("语法错误，正确语法:\n")
//                .Text($"{args.CommamdPrefix}group add <组>\n")
//                .Text($"{args.CommamdPrefix}group del <组>\n")
//                .Text($"{args.CommamdPrefix}group addperm <组> <权限>\n")
//                .Text($"{args.CommamdPrefix}group delperm <组> <权限>\n")
//                .Text($"{args.CommamdPrefix}group parent <组> <父组>");
//            await args.EventArgs.Reply(build);
//        }
//    }
//    #endregion

//    #region bank管理
//    [CommandMap("bank")]
//    [CommandPermission(OneBotPermissions.CurrencyUse, OneBotPermissions.CurrencyAdmin)]
//    public static async ValueTask Currency(CommandArgs args)
//    {
//        var at = args.EventArgs.Chain.GetMention();
//        if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "add")
//        {
//            if (!args.Account.HasPermission(OneBotPermissions.CurrencyAdmin))
//            {
//                await args.EventArgs.Reply("你没有权限执行此命令!");
//                return;
//            }
//            if (!long.TryParse(args.Parameters[1], out long qq))
//            {
//                await args.EventArgs.Reply("错误得QQ账号，无法转换为数值!");
//                return;
//            }

//            if (!long.TryParse(args.Parameters[2], out long num))
//            {
//                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
//                return;
//            }
//            try
//            {
//                var result = DB.Manager.Currency.Add(args.EventArgs.Chain.GroupUin!.Value, qq, num);
//                await args.EventArgs.Reply($"成功为 {qq} 添加{num}个{XocMatSetting.Instance.Currency}!");
//            }
//            catch (Exception ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }

//        }
//        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "add" && at.Count() == 1)
//        {
//            if (!args.Account.HasPermission(OneBotPermissions.CurrencyAdmin))
//            {
//                await args.EventArgs.Reply("你没有权限执行此命令!");
//                return;
//            }
//            if (!long.TryParse(args.Parameters[1], out long num))
//            {
//                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
//                return;
//            }
//            try
//            {
//                var result = DB.Manager.Currency.Add(args.EventArgs.Chain.GroupUin!.Value, at.First().Uin, num);
//                await args.EventArgs.Reply($"成功为 {at.First().Name} 添加{num}个{XocMatSetting.Instance.Currency}!");
//            }
//            catch (Exception ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "del")
//        {
//            if (!args.Account.HasPermission(OneBotPermissions.CurrencyAdmin))
//            {
//                await args.EventArgs.Reply("你没有权限执行此命令!");
//                return;
//            }
//            if (!long.TryParse(args.Parameters[1], out long qq))
//            {
//                await args.EventArgs.Reply("错误得QQ账号，无法转换为数值!");
//                return;
//            }

//            if (!long.TryParse(args.Parameters[2], out long num))
//            {
//                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
//                return;
//            }
//            try
//            {
//                var result = DB.Manager.Currency.Del(args.EventArgs.Chain.GroupUin!.Value, qq, num);
//                await args.EventArgs.Reply($"成功删除 {qq} 的 {num}个{XocMatSetting.Instance.Currency}!");
//            }
//            catch (Exception ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }

//        }
//        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "del" && at.Count() == 1)
//        {
//            if (!args.Account.HasPermission(OneBotPermissions.CurrencyAdmin))
//            {
//                await args.EventArgs.Reply("你没有权限执行此命令!");
//                return;
//            }
//            if (!long.TryParse(args.Parameters[1], out long num))
//            {
//                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
//                return;
//            }
//            try
//            {
//                var result = DB.Manager.Currency.Del(args.EventArgs.Chain.GroupUin!.Value, at.First().Uin, num);
//                await args.EventArgs.Reply($"成功扣除 {at.First().Name} 的 {num}个{XocMatSetting.Instance.Currency}!");
//            }
//            catch (Exception ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }
//        }
//        else if (args.Parameters.Count == 3 && args.Parameters[0].ToLower() == "pay")
//        {
//            if (!long.TryParse(args.Parameters[1], out long qq))
//            {
//                await args.EventArgs.Reply("错误得QQ账号，无法转换为数值!");
//                return;
//            }

//            if (!long.TryParse(args.Parameters[2], out long num))
//            {
//                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
//                return;
//            }
//            var usercurr = DB.Manager.Currency.Query(args.EventArgs.Chain.GroupUin!.Value, args.EventArgs.Chain.GroupMemberInfo!.Uin);
//            if (usercurr == null || usercurr.Num < num)
//            {
//                await args.EventArgs.Reply($"你没有足够的{XocMatSetting.Instance.Currency}付给他人!");
//            }
//            else
//            {
//                try
//                {
//                    DB.Manager.Currency.Del(args.EventArgs.Chain.GroupUin!.Value, args.EventArgs.Chain.GroupMemberInfo!.Uin, num);
//                    DB.Manager.Currency.Add(args.EventArgs.Chain.GroupUin!.Value, qq, num);
//                    await args.EventArgs.Reply($"成功付给 {qq}  {num}个{XocMatSetting.Instance.Currency}!");
//                }
//                catch (Exception ex)
//                {
//                    await args.EventArgs.Reply(ex.Message);
//                }
//            }
//        }
//        else if (args.Parameters.Count == 2 && args.Parameters[0].ToLower() == "pay" && at.Count() == 1)
//        {
//            if (!long.TryParse(args.Parameters[1], out long num))
//            {
//                await args.EventArgs.Reply("错误得数量，无法转换为数值!");
//                return;
//            }
//            var usercurr = DB.Manager.Currency.Query(args.EventArgs.Chain.GroupUin!.Value, args.EventArgs.Chain.GroupMemberInfo!.Uin);
//            if (usercurr == null || usercurr.Num < num)
//            {
//                await args.EventArgs.Reply($"你没有足够的{XocMatSetting.Instance.Currency}付给他人!");
//            }
//            else
//            {
//                try
//                {
//                    DB.Manager.Currency.Del(args.EventArgs.Chain.GroupUin!.Value, args.EventArgs.Chain.GroupMemberInfo!.Uin, num);
//                    DB.Manager.Currency.Add(args.EventArgs.Chain.GroupUin!.Value, at.First().Uin, num);
//                    await args.EventArgs.Reply($"成功付给 {at.First().Name}  {num}个{XocMatSetting.Instance.Currency}!");
//                }
//                catch (Exception ex)
//                {
//                    await args.EventArgs.Reply(ex.Message);
//                }
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply("语法错误，正确语法:\n" +
//                $"{args.CommamdPrefix}bank add <qq> <数量>\n" +
//                $"{args.CommamdPrefix}bank add <数量> at\n" +
//                $"{args.CommamdPrefix}bank del <qq> <数量>\n" +
//                $"{args.CommamdPrefix}bank del <数量> at\n" +
//                $"{args.CommamdPrefix}bank pay <qq> 数量\n" +
//                $"{args.CommamdPrefix}bank pay <数量> at");
//        }
//    }
//    #endregion

//    #region 查询指令权限
//    [CommandMap("sperm")]
//    [CommandPermission(OneBotPermissions.SearchCommandPerm)]
//    public static async ValueTask CmdBan(CommandArgs args)
//    {
//        if (args.Parameters.Count == 1)
//        {
//            var banName = args.Parameters[0];
//            var comm = XocMatAPI.Command?.GroupCommandDelegate.Where(x => x.Name.Contains(banName)).SelectMany(x => x.Permission).ToList();
//            if (comm == null || comm.Count == 0)
//            {
//                await args.EventArgs.Reply("没有找到该指令，无法查询！");
//            }
//            else
//            {
//                var build = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value);
//                build.Text(banName + "指令的权限可能为:\n");

//                comm.ForEach(x => build.Text(x));
//                await args.EventArgs.Reply(build);
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误，正确语法:{args.CommamdPrefix}scmdperm [指令名]");
//        }
//    }
//    #endregion

//    #region 缩写查询
//    [CommandMap("缩写")]
//    [CommandPermission(OneBotPermissions.Nbnhhsh)]
//    public static async ValueTask Nbnhhsh(CommandArgs args)
//    {
//        if (args.Parameters.Count == 1)
//        {
//            var url = $"https://oiapi.net/API/Nbnhhsh?text={args.Parameters[0]}";
//            HttpClient client = new();
//            var result = await client.GetStringAsync(url);
//            var data = JsonNode.Parse(result);
//            var trans = data?["data"]?[0]?["trans"]?.AsArray();
//            if (trans != null && trans.Any())
//            {

//                await args.EventArgs.Reply($"缩写:`{args.Parameters[0]}`可能为:\n{string.Join(",", trans)}");
//            }
//            else
//            {
//                await args.EventArgs.Reply("也许该缩写没有被收录!");
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误，正确语法:{args.CommamdPrefix}缩写 [文本]");
//        }
//    }
//    #endregion

//    #region 禁言
//    [CommandMap("禁")]
//    [CommandPermission(OneBotPermissions.Mute)]
//    public static async ValueTask Mute(CommandArgs args)
//    {
//        if (args.Parameters.Count == 1)
//        {
//            if (!uint.TryParse(args.Parameters[0].ToString(), out var muted))
//            {
//                await args.EventArgs.Reply("请输入正确的禁言时长!");
//                return;
//            }
//            var atlist = args.EventArgs.Chain.GetMention();
//            if (!atlist.Any())
//            {
//                await args.EventArgs.Reply("未指令目标成员!");
//                return;
//            }
//            atlist.ForEach(async x => await args.Bot.MuteGroupMember(args.EventArgs.Chain.GroupUin!.Value, x.Uin, muted * 60));
//            await args.EventArgs.Reply("禁言成功！");
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}禁 [AT] [时长]！");
//        }
//    }
//    #endregion

//    #region 解禁
//    [CommandMap("解")]
//    [CommandPermission(OneBotPermissions.Mute)]
//    public static async ValueTask UnMute(CommandArgs args)
//    {
//        if (args.Parameters.Count == 0)
//        {
//            var atlist = args.EventArgs.Chain.GetMention();
//            if (!atlist.Any())
//            {
//                await args.EventArgs.Reply("未指令目标成员!");
//                return;
//            }
//            atlist.ForEach(async x => await args.Bot.MuteGroupMember(args.EventArgs.Chain.GroupUin!.Value, x.Uin, 0));
//            await args.EventArgs.Reply("解禁成功！");
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}解 [AT] [时长]！");
//        }
//    }
//    #endregion

//    #region 全体禁言
//    [CommandMap("全禁")]
//    [CommandPermission(OneBotPermissions.Mute)]
//    public static async ValueTask MuteAll(CommandArgs args)
//    {
//        if (args.Parameters.Count == 1)
//        {
//            switch (args.Parameters[0])
//            {
//                case "开启":
//                case "开":
//                case "true":
//                    await args.Bot.MuteGroupGlobal(args.EventArgs.Chain.GroupUin!.Value, true);
//                    await args.EventArgs.Reply("开启成功！");
//                    break;
//                case "关闭":
//                case "关":
//                case "false":
//                    await args.Bot.MuteGroupGlobal(args.EventArgs.Chain.GroupUin!.Value, false);
//                    await args.EventArgs.Reply("关闭成功");
//                    break;
//                default:
//                    await args.EventArgs.Reply("语法错误,正确语法:\n全禁 [开启|关闭]");
//                    break;
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply("语法错误,正确语法:\n全禁 [开启|关闭]");
//        }
//    }
//    #endregion

//    #region 设置群名
//    [CommandMap("设置群名")]
//    [CommandPermission(OneBotPermissions.ChangeGroupOption)]
//    public static async ValueTask SetGroupName(CommandArgs args)
//    {
//        if (args.Parameters.Count == 1)
//        {
//            if (string.IsNullOrEmpty(args.Parameters[0]))
//            {
//                await args.EventArgs.Reply("群名不能未空！");
//                return;
//            }
//            await args.Bot.RenameGroup(args.EventArgs.Chain.GroupUin!.Value, args.Parameters[0]);
//            await args.EventArgs.Reply($"群名称已修改为`{args.Parameters[0]}`");
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}设置群名 [名称]");
//        }
//    }
//    #endregion

//    #region 设置群成员名片
//    [CommandMap("设置昵称")]
//    [CommandPermission(OneBotPermissions.ChangeGroupOption)]
//    public static async ValueTask SetGroupMemeberNick(CommandArgs args)
//    {
//        if (args.Parameters.Count == 1)
//        {
//            var atlist = args.EventArgs.Chain.GetMention().First();
//            if (atlist != null)
//            {
//                await args.Bot.RenameGroupMember(args.EventArgs.Chain.GroupUin!.Value, atlist.Uin, args.Parameters[0]);
//                await args.EventArgs.Reply("修改昵称成功!");
//            }
//            else
//            {
//                await args.EventArgs.Reply("请选择一位成员！");
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [名称]");
//        }
//    }
//    #endregion

//    #region 设置管理
//    [CommandMap("设置管理")]
//    [CommandPermission(OneBotPermissions.ChangeGroupOption)]
//    public static async ValueTask SetGroupAdmin(CommandArgs args)
//    {
//        if (args.Parameters.Count == 0)
//        {
//            var atlist = args.EventArgs.Chain.GetMention().First();
//            if (atlist != null)
//            {
//                await args.Bot.SetGroupAdmin(args.EventArgs.Chain.GroupUin!.Value, atlist.Uin, true);
//                await args.EventArgs.Reply($"已将`{atlist.Uin}`设置为管理员!");
//            }
//            else
//            {
//                await args.EventArgs.Reply("请选择一位成员！");
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [AT]");
//        }
//    }
//    #endregion

//    #region 取消管理
//    [CommandMap("取消管理")]
//    [CommandPermission(OneBotPermissions.ChangeGroupOption)]
//    public static async ValueTask UnsetGroupAdmin(CommandArgs args)
//    {
//        if (args.Parameters.Count == 0)
//        {
//            var atlist = args.EventArgs.Chain.GetMention().First();
//            if (atlist != null)
//            {
//                await args.Bot.SetGroupAdmin(args.EventArgs.Chain.GroupUin!.Value, atlist.Uin, false);
//                await args.EventArgs.Reply($"已取消`{atlist.Uin}`的管理员!");
//            }
//            else
//            {
//                await args.EventArgs.Reply("请选择一位成员！");
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [AT]");
//        }
//    }
//    #endregion

//    #region 服务器列表
//    [CommandMap("服务器列表")]
//    [CommandPermission(OneBotPermissions.ServerList)]
//    public static async ValueTask ServerList(CommandArgs args)
//    {
//        var groupServers = XocMatSetting.Instance.Servers.Where(s => s.Groups.Contains(args.EventArgs.Chain.GroupUin!.Value));
//        if (!groupServers.Any())
//        {
//            await args.EventArgs.Reply("此群未配置任何服务器!", true);
//            return;
//        }
//        var tableBuilder = new TableBuilder();
//        tableBuilder.SetTitle("服务器列表");
//        tableBuilder.AddRow("服务器名称", "服务器IP", "服务器端口", "服务器版本", "服务器介绍", "运行状态", "世界名称", "世界种子", "世界大小");
//        foreach (var server in groupServers)
//        {
//            var status = await server.ServerStatus();
//            tableBuilder.AddRow(server.Name, server.IP, server.NatProt.ToString(), server.Version, server.Describe,
//                !status.Status ? "无法连接" : $"已运行:{status.RunTime:dd\\.hh\\:mm\\:ss}",
//                !status.Status ? "无法获取" : status.WorldName,
//                !status.Status ? "无法获取" : status.WorldSeed,
//                !status.Status ? "无法获取" : $"{status.WorldWidth}x{status.WorldHeight}");
//        }

//        await args.MessageBuilder.Image(await tableBuilder.BuildAsync()).Reply();
//    }
//    #endregion

//    #region 服务器信息
//    [CommandMap("插件列表")]
//    [CommandPermission(OneBotPermissions.ServerList)]
//    public static async ValueTask ServerInfo(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            var status = await server.ServerStatus();
//            if (!status.Status)
//            {
//                await args.EventArgs.Reply("无法连接服务器!", true);
//                return;
//            }
//            var tableBuilder = new TableBuilder();
//            tableBuilder.SetTitle($"{server.Name}插件列表");
//            tableBuilder.SetTitleBottom(true);
//            tableBuilder.AddRow("插件名称", "插件说明", "插件作者");
//            foreach (var plugin in status.Plugins)
//            {
//                tableBuilder.AddRow(plugin.Name, plugin.Description, plugin.Author);
//            }
//            await args.MessageBuilder.Image(await tableBuilder.BuildAsync()).Reply();
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region 重启服务器
//    [CommandMap("重启服务器")]
//    [CommandPermission(OneBotPermissions.ResetTShock)]
//    public static async ValueTask ReStartServer(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            var api = await server.ReStartServer(args.CommamdLine);
//            var build = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value);
//            if (api.Status)
//            {
//                build.Text("正在重启服务器，请稍后...");
//            }
//            else
//            {
//                build.Text(api.Message);
//            }
//            await args.EventArgs.Reply(build);
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region 切换服务器
//    [CommandMap("切换")]
//    [CommandPermission(OneBotPermissions.ChangeServer)]
//    public static async ValueTask ChangeServer(CommandArgs args)
//    {
//        if (args.Parameters.Count == 1)
//        {
//            var server = XocMatSetting.Instance.GetServer(args.Parameters[0], Convert.ToUInt32(args.EventArgs.Chain.GroupUin));
//            if (server == null)
//            {
//                await args.EventArgs.Reply("你切换的服务器不存在! 请检查服务器名称是否正确，此群是否配置服务器!");
//                return;
//            }
//            UserLocation.Instance.Change(args.EventArgs.Chain.GroupMemberInfo!.Uin, server);
//            await args.EventArgs.Reply($"已切换至`{server.Name}`服务器", true);
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [服务器名称]");
//        }
//    }
//    #endregion

//    #region 查询在线玩家
//    [CommandMap("在线")]
//    [CommandPermission(OneBotPermissions.QueryOnlienPlayer)]
//    public static async ValueTask OnlinePlayers(CommandArgs args)
//    {
//        if (XocMatSetting.Instance.Servers.Count == 0)
//        {
//            await args.EventArgs.Reply("还没有配置任何一个服务器!", true);
//            return;
//        }
//        var groupServers = XocMatSetting.Instance.Servers.Where(s => s.Groups.Contains(args.EventArgs.Chain.GroupUin!.Value));
//        if (!groupServers.Any())
//        {
//            await args.EventArgs.Reply("此群未配置任何服务器!", true);
//            return;
//        }
//        var sb = new StringBuilder();
//        foreach (var server in groupServers)
//        {
//            var api = await server.ServerOnline();
//            sb.AppendLine($"[{server.Name}]在线玩家数量({(api.Status ? api.Players.Count : 0)}/{api.MaxCount})");
//            sb.AppendLine(api.Status ? string.Join(",", api.Players.Select(x => x.Name)) : "无法连接服务器");
//        }
//        await args.EventArgs.Reply(sb.ToString().Trim());
//    }
//    #endregion

//    #region 生成地图
//    [CommandMap("生成地图")]
//    [CommandPermission(OneBotPermissions.GenerateMap)]
//    public static async ValueTask GenerateMap(CommandArgs args)
//    {
//        var type = ImageType.Jpg;
//        if (args.Parameters.Count > 0 && args.Parameters[0] == "-p")
//            type = ImageType.Png;
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            var api = await server.MapImage(type);
//            if (api.Status)
//            {
//                var tempDir = Path.Combine(Environment.CurrentDirectory, "TempImage");
//                if (!Directory.Exists(tempDir))
//                {
//                    Directory.CreateDirectory(tempDir);
//                }
//                var fileName = Guid.NewGuid().ToString() + ".jpg";
//                var path = Path.Combine(tempDir, fileName);
//                System.IO.File.WriteAllBytes(path, api.Buffer);
//                await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).Image(api.Buffer));
//                File.Delete(path);
//            }
//            else
//            {
//                await args.EventArgs.Reply(api.Message);
//            }

//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region 注册
//    [CommandMap("注册")]
//    [CommandPermission(OneBotPermissions.RegisterUser)]
//    public static async ValueTask Register(CommandArgs args)
//    {
//        if (args.Parameters.Count == 1)
//        {
//            if (!UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) || server == null)
//            {
//                await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//                return;
//            }
//            if (args.Parameters[0].Length > server.RegisterNameMax)
//            {
//                await args.EventArgs.Reply($"注册的人物名称不能大于{server.RegisterNameMax}个字符!", true);
//                return;
//            }
//            if (!new Regex("^[a-zA-Z0-9\u4e00-\u9fa5\\[\\]:/ ]+$").IsMatch(args.Parameters[0]) && server.RegisterNameLimit)
//            {
//                await args.EventArgs.Reply("注册的人物名称不能包含中文,字母,数字和/:[]以外的字符", true);
//                return;
//            }
//            if (TerrariaUser.GetUserById(args.EventArgs.Chain.GroupMemberInfo!.Uin, server.Name).Count >= server.RegisterMaxCount)
//            {
//                await args.EventArgs.Reply($"同一个服务器上注册账户不能超过{server.RegisterMaxCount}个", true);
//                return;
//            }
//            var pass = Guid.NewGuid().ToString()[..8];
//            try
//            {
//                TerrariaUser.Add(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, server.Name, args.Parameters[0], pass);
//                var api = await server.Register(args.Parameters[0], pass);
//                var build = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value);
//                if (api.Status)
//                {
//                    MailHelper.SendMail($"{args.EventArgs.Chain.GroupMemberInfo!.Uin}@qq.com",
//                        $"{server.Name}服务器注册密码",
//                        $"您的注册密码是:{pass}<br>请注意保存不要暴露给他人");
//                    build.Text($"注册成功!" +
//                        $"\n注册服务器: {server.Name}" +
//                        $"\n注册名称: {args.Parameters[0]}" +
//                        $"\n注册账号: {args.EventArgs.Chain.GroupMemberInfo!.Uin}" +
//                        $"\n注册人昵称: {args.EventArgs.Chain.GroupMemberInfo!.MemberName}" +
//                        $"\n注册密码已发送至QQ邮箱请点击下方链接查看" +
//                        $"\nhttps://wap.mail.qq.com/home/index" +
//                        $"\n进入服务器后可使用/password [当前密码] [新密码] 修改你的密码");
//                }
//                else
//                {
//                    TerrariaUser.Remove(server.Name, args.Parameters[0]);
//                    build.Text(string.IsNullOrEmpty(api.Message) ? "无法连接服务器！" : api.Message);
//                }
//                await args.EventArgs.Reply(build);
//            }
//            catch (TerrariaUserException ex)
//            {
//                await args.EventArgs.Reply(ex.Message);
//            }


//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [名称]");
//        }
//    }
//    #endregion

//    #region 我的密码
//    [CommandMap("我的密码")]
//    [CommandPermission(OneBotPermissions.SelfPassword)]
//    public static async ValueTask SelfPassword(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            var user = TerrariaUser.GetUserById(args.EventArgs.Chain.GroupMemberInfo!.Uin, server.Name);
//            if (user.Count > 0)
//            {
//                var sb = new StringBuilder();
//                foreach (var u in user)
//                    sb.AppendLine($"人物{u.Name}的注册密码为: {u.Password}");
//                sb.AppendLine("请注意保存不要暴露给他人");
//                MailHelper.SendMail($"{args.EventArgs.Chain.GroupMemberInfo!.Uin}@qq.com",
//                            $"{server.Name}服务器注册密码",
//                            sb.ToString().Trim());
//                await args.EventArgs.Reply("密码查询成功已发送至你的QQ邮箱。", true);
//                return;
//            }
//            await args.EventArgs.Reply($"{server.Name}上未找到你的注册信息。");
//            return;
//        }
//        await args.EventArgs.Reply("服务器无效或未切换至一个有效服务器!");
//    }
//    #endregion

//    #region 重置密码
//    [CommandMap("重置密码")]
//    [CommandPermission(OneBotPermissions.SelfPassword)]
//    public static async ValueTask ResetPassword(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            try
//            {
//                var user = TerrariaUser.GetUserById(args.EventArgs.Chain.GroupMemberInfo!.Uin, server.Name);

//                if (user.Count > 0)
//                {
//                    var sb = new StringBuilder();
//                    foreach (var u in user)
//                    {
//                        var pwd = Guid.NewGuid().ToString()[..8];
//                        sb.Append($"人物 {u.Name}的密码重置为: {pwd}<br>");
//                        var res = await server.ResetPlayerPwd(u.Name, pwd);
//                        if (!res.Status)
//                        {
//                            await args.EventArgs.Reply("无法连接到服务器更改密码!");
//                            return;
//                        }
//                        TerrariaUser.ResetPassword(args.EventArgs.Chain.GroupMemberInfo!.Uin, server.Name, u.Name, pwd);
//                    }
//                    sb.Append("请注意保存不要暴露给他人");
//                    MailHelper.SendMail($"{args.EventArgs.Chain.GroupMemberInfo!.Uin}@qq.com",
//                                $"{server.Name}服密码重置",
//                                sb.ToString().Trim());
//                    await args.EventArgs.Reply("密码重置成功已发送至你的QQ邮箱。", true);
//                    return;
//                }
//                await args.EventArgs.Reply($"{server.Name}上未找到你的注册信息。");
//                return;
//            }
//            catch (Exception ex)
//            {
//                await args.EventArgs.Reply(ex.Message, true);
//            }
//        }
//        await args.EventArgs.Reply("服务器无效或未切换至一个有效服务器!");
//    }
//    #endregion

//    #region 查询注册人
//    [CommandMap("注册查询", "name")]
//    [CommandPermission(OneBotPermissions.SearchUser)]
//    public static async ValueTask SearchUser(CommandArgs args)
//    {
//        async ValueTask GetRegister(long id)
//        {
//            var users = TerrariaUser.GetUsers(id);
//            if (users.Count == 0)
//            {
//                await args.EventArgs.Reply("未查询到该用户的注册信息!");
//                return;
//            }
//            StringBuilder sb = new("查询结果:\n");
//            foreach (var user in users)
//            {
//                sb.AppendLine($"注册名称: {user.Name}");
//                sb.AppendLine($"注册账号: {user.Id}");
//                var result = (await args.Bot.FetchMembers(args.EventArgs.Chain.GroupUin!.Value)).FirstOrDefault(x => x.Uin == user.Id);
//                if (result != null)
//                {
//                    sb.AppendLine($"群昵称: {result.MemberName}");
//                }
//                else
//                {
//                    sb.AppendLine("注册人不在此群中");
//                }
//                sb.AppendLine("");
//            }
//            await args.EventArgs.Reply(sb.ToString().Trim());
//        }
//        var atlist = args.EventArgs.Chain.GetMention();
//        if (args.Parameters.Count == 0 && atlist.Any())
//        {
//            var target = atlist.First();
//            await GetRegister(target.Uin);

//        }
//        else if (args.Parameters.Count == 1)
//        {
//            if (long.TryParse(args.Parameters[0], out var id))
//            {
//                await GetRegister(id);
//            }
//            else
//            {
//                var user = TerrariaUser.GetUsersByName(args.Parameters[0]);
//                if (user == null)
//                {
//                    await args.EventArgs.Reply("未查询到注册信息", true);
//                    return;
//                }
//                else
//                {
//                    await GetRegister(user.Id);
//                }
//            }
//        }
//    }
//    #endregion

//    #region 注册列表
//    [CommandMap("注册列表")]
//    [CommandPermission(OneBotPermissions.QueryUserList)]
//    public static async ValueTask RegisterList(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            var users = TerrariaUser.GetUsers(server.Name);
//            if (users == null || users.Count == 0)
//            {
//                await args.EventArgs.Reply("注册列表空空如也!");
//                return;
//            }
//            var sb = new StringBuilder($"[{server.Name}]注册列表\n");
//            foreach (var user in users)
//            {
//                sb.AppendLine($"{user.Name} => {user.Id}");
//            }
//            await args.EventArgs.Reply(sb.ToString().Trim());
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region user管理
//    [CommandMap("user")]
//    [CommandPermission(OneBotPermissions.UserAdmin)]
//    public static async ValueTask User(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            if (args.Parameters.Count == 2)
//            {
//                switch (args.Parameters[0].ToLower())
//                {
//                    case "del":
//                        try
//                        {
//                            TerrariaUser.Remove(server.Name, args.Parameters[1]);
//                            await args.EventArgs.Reply("移除成功!", true);
//                        }
//                        catch (TerrariaUserException ex)
//                        {
//                            await args.EventArgs.Reply(ex.Message);
//                        }
//                        break;
//                    default:
//                        await args.EventArgs.Reply("未知子命令!");
//                        break;
//                }
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }

//    }
//    #endregion

//    #region 进度查询
//    [CommandMap("进度查询")]
//    [CommandPermission(OneBotPermissions.QueryProgress)]
//    public static async ValueTask GameProgress(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            var api = await server.QueryServerProgress();
//            var body = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value);
//            if (api.Status)
//            {
//                var stream = ProgressImage.Start(api.Progress, server.Name);
//                body.Image(stream.ToArray());
//            }
//            else
//            {
//                body.Text("无法获取服务器信息！");
//            }
//            await args.EventArgs.Reply(body);
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region 查询背包
//    [CommandMap("查背包")]
//    [CommandPermission(OneBotPermissions.QueryInventory)]
//    public static async ValueTask QueryInventory(CommandArgs args)
//    {
//        if (args.Parameters.Count == 1)
//        {
//            if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//            {
//                var api = await server.PlayerInventory(args.Parameters[0]);
//                var body = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value);
//                if (api.Status)
//                {

//                    var ms = DrawInventory.Start(api.PlayerData!, api.PlayerData!.Username, api.ServerName);
//                    body.Image(ms.ToArray());
//                    body.Add(MomoSegment.Image(new InventoryImage().DrawImg(api.PlayerData, args.Parameters[0], server.Name)));
//                }
//                else
//                {
//                    body.Text("无法获取用户信息！");
//                }
//                await args.EventArgs.Reply(body);
//            }
//            else
//            {
//                await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [用户名]");
//        }

//    }
//    #endregion

//    #region 执行命令
//    [CommandMap("执行")]
//    [CommandPermission(OneBotPermissions.ExecuteCommand)]
//    public static async ValueTask ExecuteCommand(CommandArgs args)
//    {
//        if (args.Parameters.Count < 1)
//        {
//            await args.EventArgs.Reply("请输入要执行的命令!", true);
//            return;
//        }
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            var cmd = "/" + string.Join(" ", args.Parameters);
//            var api = await server.Command(cmd);
//            var body = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value);
//            if (api.Status)
//            {
//                var cmdResult = $"[{server.Name}]命令执行结果:\n{string.Join("\n", api.Params)}";
//                body.Text(cmdResult);
//            }
//            else
//            {
//                body.Text("无法连接到服务器！");
//            }
//            await args.EventArgs.Reply(body);
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region 执行全部
//    [CommandMap("执行全部")]
//    [CommandPermission(OneBotPermissions.ExecuteCommand)]
//    public static async ValueTask ExecuteCommandAll(CommandArgs args)
//    {
//        if (args.Parameters.Count < 1)
//        {
//            await args.EventArgs.Reply("请输入要执行的命令!", true);
//            return;
//        }
//        var sb = new StringBuilder();
//        foreach (var server in XocMatSetting.Instance.Servers)
//        {
//            var cmd = "/" + string.Join(" ", args.Parameters);
//            var api = await server.Command(cmd);
//            sb.AppendLine($"[{server.Name}]命令执行结果:");
//            sb.AppendLine(api.Status ? string.Join("\n", api.Params) : "无法连接到服务器！");
//        }
//        await args.EventArgs.Reply(sb.ToString().Trim());
//    }
//    #endregion

//    #region 击杀排行
//    [CommandMap("击杀排行")]
//    [CommandPermission(OneBotPermissions.KillRank)]
//    public static async ValueTask KillRank(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            var data = await server.GetStrikeBoss();
//            if (data.Damages != null)
//            {
//                var sb = new StringBuilder();
//                foreach (var damage in data.Damages.OrderByDescending(x => x.KillTime))
//                {
//                    sb.AppendLine($"Boss: {damage.Name}");
//                    sb.AppendLine($"总血量: {damage.MaxLife}");
//                    sb.AppendLine($"更新时间: {damage.KillTime.ToString("yyyy-MM-dd HH:mm:ss")}");
//                    sb.AppendLine($"状态: {(damage.IsAlive ? "未被击杀" : "已被击杀")}");
//                    if (!damage.IsAlive)
//                    {
//                        sb.AppendLine($"击杀用时: {(damage.KillTime - damage.SpawnTime).TotalSeconds}秒");
//                        sb.AppendLine($"丢失伤害: {damage.MaxLife - damage.Strikes.Sum(x => x.Damage)}");
//                    }
//                    foreach (var strike in damage.Strikes.OrderByDescending(x => x.Damage))
//                    {
//                        sb.AppendLine($"{strike.Player}伤害 {Convert.ToSingle(strike.Damage) / damage.MaxLife * 100:F0}%({strike.Damage})");
//                    }
//                    sb.AppendLine();
//                }
//                await args.EventArgs.Reply(sb.ToString().Trim());
//            }
//            else
//            {
//                await args.EventArgs.Reply("暂无击杀数据可以统计!", true);
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region 在线排行
//    [CommandMap("在线排行")]
//    [CommandPermission(OneBotPermissions.OnlineRank)]
//    public static async ValueTask OnlineRank(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            var api = await server.OnlineRank();
//            var body = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value);
//            if (api.Status)
//            {
//                if (api.OnlineRank.Count == 0)
//                {
//                    await args.EventArgs.Reply("当前还没有数据记录", true);
//                    return;
//                }
//                var sb = new StringBuilder($"[{server.Name}]在线排行:\n");
//                var rank = api.OnlineRank.OrderByDescending(x => x.Value);
//                foreach (var (name, duration) in rank)
//                {
//                    var day = duration / (60 * 60 * 24);
//                    var hour = (duration - day * 60 * 60 * 24) / (60 * 60);
//                    var minute = (duration - day * 60 * 60 * 24 - hour * 60 * 60) / 60;
//                    var second = duration - day * 60 * 60 * 24 - hour * 60 * 60 - minute * 60;
//                    sb.Append($"[{name}]在线时长: ");
//                    if (day > 0)
//                        sb.Append($"{day}天");
//                    if (hour > 0)
//                        sb.Append($"{hour}时");
//                    if (minute > 0)
//                        sb.Append($"{minute}分");
//                    sb.Append($"{second}秒\n");
//                }
//                body.Text(sb.ToString().Trim());
//            }
//            else
//            {
//                body.Text("无法连接到服务器！");
//            }
//            await args.EventArgs.Reply(body);
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region 死亡排行
//    [CommandMap("死亡排行")]
//    [CommandPermission(OneBotPermissions.DeathRank)]
//    public static async ValueTask DeathRank(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            var api = await server.DeadRank();
//            var body = MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value);
//            if (api.Status)
//            {
//                if (api.Rank.Count == 0)
//                {
//                    await args.EventArgs.Reply("当前还没有数据记录", true);
//                    return;
//                }
//                var sb = new StringBuilder($"[{server.Name}]死亡排行:\n");
//                var rank = api.Rank.OrderByDescending(x => x.Value);
//                foreach (var (name, count) in rank)
//                {
//                    sb.AppendLine($"[{name}]死亡次数: {count}");
//                }
//                body.Text(sb.ToString().Trim());
//            }
//            else
//            {
//                body.Text("无法连接到服务器！");
//            }
//            await args.EventArgs.Reply(body);
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region 消息转发
//    [CommandMap("消息转发")]
//    [CommandPermission(OneBotPermissions.ForwardMsg)]
//    public static async ValueTask MessageForward(CommandArgs args)
//    {
//        if (args.Parameters.Count == 1)
//        {
//            if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//            {
//                switch (args.Parameters[0])
//                {
//                    case "开启":
//                    case "true":
//                        server.ForwardGroups.Add(args.EventArgs.Chain.GroupUin!.Value);
//                        await args.EventArgs.Reply("开启成功", true);
//                        break;
//                    case "关闭":
//                    case "false":
//                        server.ForwardGroups.Remove(args.EventArgs.Chain.GroupUin!.Value);
//                        await args.EventArgs.Reply("关闭成功", true);
//                        break;
//                    default:
//                        await args.EventArgs.Reply("未知子命令！", true);
//                        break;
//                }
//                XocMatSetting.Instance.SaveTo();
//            }
//            else
//            {
//                await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//            }
//        }
//        else
//        {
//            await args.EventArgs.Reply($"语法错误,正确语法:\n{args.CommamdPrefix}{args.Name} [开启|关闭]!", true);
//        }
//    }
//    #endregion

//    #region 查询他人信息
//    [CommandMap("查")]
//    [CommandPermission(OneBotPermissions.OtherInfo)]
//    public static async ValueTask AcountInfo(CommandArgs args)
//    {
//        var at = args.EventArgs.Chain.GetMention();
//        if (at.Any())
//        {
//            var group = DB.Manager.Account.GetAccountNullDefault(at.First().Uin);
//            await args.EventArgs.Reply(await CommandUtils.GetAccountInfo(args.EventArgs.Chain.GroupUin!.Value, at.First().Uin, group.Group.Name));
//        }
//        else if (args.Parameters.Count == 1 && uint.TryParse(args.Parameters[0], out var id))
//        {
//            var group = DB.Manager.Account.GetAccountNullDefault(id);
//            await args.EventArgs.Reply(await CommandUtils.GetAccountInfo(args.EventArgs.Chain.GroupUin!.Value, id, group.Group.Name));
//        }
//        else
//        {
//            await args.EventArgs.Reply("查谁呢?", true);
//        }
//    }
//    #endregion

//    #region 我的信息
//    [CommandMap("我的信息")]
//    [CommandPermission(OneBotPermissions.SelfInfo)]
//    public static async ValueTask SelfInfo(CommandArgs args)
//    {
//        await args.EventArgs.Reply(await CommandUtils.GetAccountInfo(args.EventArgs.Chain.GroupUin!.Value, args.EventArgs.Chain.GroupMemberInfo!.Uin, args.Account.Group.Name));
//    }
//    #endregion

//    #region Wiki
//    [CommandMap("wiki")]
//    [CommandPermission(OneBotPermissions.TerrariaWiki)]
//    public static async ValueTask Wiki(CommandArgs args)
//    {
//        string url = "https://terraria.wiki.gg/zh/index.php?search=";
//        var msg = args.Parameters.Count > 0 ? url += HttpUtility.UrlEncode(args.Parameters[0]) : url.Split("?")[0];
//        await args.EventArgs.Reply(msg);
//    }
//    #endregion

//    #region 启动服务器
//    [CommandMap("启动")]
//    [CommandPermission(OneBotPermissions.StartTShock)]
//    public static async ValueTask StartTShock(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            if (server.Start(args.CommamdLine))
//            {
//                await args.EventArgs.Reply($"{server.Name} 正在对其执行启动命令!", true);
//                return;
//            }
//            await args.EventArgs.Reply($"{server.Name} 启动失败!", true);
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region 重置服务器
//    [CommandMap("泰拉服务器重置")]
//    [CommandPermission(OneBotPermissions.StartTShock)]
//    public static async ValueTask ResetTShock(CommandArgs args)
//    {
//        if (UserLocation.Instance.TryGetServer(args.EventArgs.Chain.GroupMemberInfo!.Uin, args.EventArgs.Chain.GroupUin!.Value, out var server) && server != null)
//        {
//            TerrariaUser.RemoveByServer(server.Name);
//            await server.Reset(args.CommamdLine, async type =>
//            {
//                switch (type)
//                {
//                    case RestServerType.WaitFile:
//                        {
//                            await args.EventArgs.Reply("正在等待上传地图，60秒后失效!");
//                            break;
//                        }
//                    case RestServerType.TimeOut:
//                        {
//                            await args.EventArgs.Reply("地图上传超时，自动创建地图。");
//                            break;
//                        }
//                    case RestServerType.Success:
//                        {
//                            await args.EventArgs.Reply("正在重置服务器!!");
//                            break;
//                        }
//                    case RestServerType.LoadFile:
//                        {
//                            await args.EventArgs.Reply("已接受到地图，正在上传服务器!!");
//                            break;
//                        }
//                    case RestServerType.UnLoadFile:
//                        {
//                            await args.EventArgs.Reply("上传的地图非国际正版，或地图不合法，请尽快重写上传!");
//                            break;
//                        }
//                }
//            });
//        }
//        else
//        {
//            await args.EventArgs.Reply("未切换服务器或服务器无效!", true);
//        }
//    }
//    #endregion

//    #region 随机视频
//    [CommandMap("randv")]
//    [CommandPermission("onebot.video.rand")]
//    public static async ValueTask RandVideo(CommandArgs args)
//    {
//        await args.EventArgs.Reply(MessageBuilder.Group(args.EventArgs.Chain.GroupUin!.Value).Video(await HttpUtils.HttpGetByte("https://www.yujn.cn/api/heisis.php")));
//    }
//    #endregion
//}
