﻿using ProtoBuf;

namespace Lagrange.XocMat.Terraria.Protocol.PlayerMessage;

[ProtoContract]
public class PlayerCommandMessage : BasePlayerMessage
{
    [ProtoMember(8)] public string Command { get; set; } = string.Empty;

    [ProtoMember(9)] public string CommandPrefix { get; set; } = string.Empty;
}
