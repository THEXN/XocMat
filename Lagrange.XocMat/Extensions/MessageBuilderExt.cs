﻿using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.XocMat.Utility;

namespace Lagrange.XocMat.Extensions;

public static class MessageBuilderExt
{
    public static MessageBuilder MarkdownImage(this MessageBuilder builder, string content)
    {
        try
        {
            var buffer = MarkdownHelper.ToImage(content).Result;
            return builder.Image(buffer);
        }
        catch (Exception ex)
        {
            return builder.Text(ex.Message);
        }
    }

    public static MessageBuilder MultiMsg(this MessageBuilder builder, List<MessageChain> chains)
    {
        var m = new MultiMsgEntity(chains);
        builder.Add(m);
        return builder;
    }
}
