﻿using Lagrange.Core.Event.EventArg;
using Lagrange.Core.Message;


namespace Lagrange.XocMat.Extensions;

public static class GroupMessageEventExt
{
    public static async Task Reply(this GroupMessageEvent e, MessageBuilder builder, bool type = false)
    { 
        await XocMatAPI.BotContext.Reply(builder);
    }

    public static async Task Reply(this GroupMessageEvent e, string text, bool type = false)
    {
        var build = MessageBuilder.Group(e.Chain.GroupUin!.Value).Text(text);
        if (type) build.Forward(e.Chain);
        await e.Reply(build);
    }
}