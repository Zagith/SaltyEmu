﻿using ChickenAPI.Packets.Attributes;

namespace ChickenAPI.Packets.Game.Client.QuickList
{
    [PacketHeader("qset")]
    public class QsetPacket : PacketBase
    {
        #region Properties

        [PacketIndex(0)]
        public bool IsSkill { get; set; }

        [PacketIndex(1)]
        public short Q1 { get; set; }

        [PacketIndex(2)]
        public short Q2 { get; set; }

        [PacketIndex(3)]
        public short Data1 { get; set; }

        [PacketIndex(4)]
        public short Data2 { get; set; }

        #endregion
    }
}