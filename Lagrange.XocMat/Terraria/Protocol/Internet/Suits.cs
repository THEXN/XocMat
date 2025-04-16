﻿using ProtoBuf;

namespace Lagrange.XocMat.Terraria.Protocol.Internet;

[ProtoContract]
public class Suits
{
    [ProtoMember(1)] public Item[] armor { get; set; } = [];
    //染料
    [ProtoMember(2)] public Item[] dye { get; set; } = [];
}
