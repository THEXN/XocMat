﻿using ProtoBuf;

namespace Lagrange.XocMat.Internal.Socket.Action.Receive;

[ProtoContract]
public class ServerCommandArgs : BaseAction
{
    [ProtoMember(5)] public string Text { get; set; } = string.Empty;
}
