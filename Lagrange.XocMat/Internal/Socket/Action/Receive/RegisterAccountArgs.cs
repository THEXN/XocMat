﻿using Lagrange.XocMat.Internal.Socket.Action;
using ProtoBuf;

namespace Lagrange.XocMat.Internal.Socket.Action.Receive;

[ProtoContract]
public class RegisterAccountArgs : BaseAction
{
    [ProtoMember(5)] public string Name { get; set; }

    [ProtoMember(6)] public string Group { get; set; }

    [ProtoMember(7)] public string Password { get; set; }
}
