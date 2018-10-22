﻿using System.Linq;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Families;
using ChickenAPI.Game.Families.Events;
using ChickenAPI.Game.Test.Mocks;
using ChickenAPI.Packets.Game.Client.Families;
using NUnit.Framework;

namespace ChickenAPI.Game.Test.Families
{
    [TestFixture]
    public class Family_Leave
    {
        private readonly BasicFamilyEventHandler _familyEventHandler = new BasicFamilyEventHandler();

        [OneTimeSetUp]
        public void Setup()
        {
            TestHelper.Initialize();
        }

        private static IPlayerEntity LoadPlayer(string name) => TestHelper.LoadPlayer(name);

        [Test]
        public void Family_Kick_Success()
        {
            const string familyName = "family_leave_test";
            IPlayerEntity leader = LoadPlayer("family_leave_leader_sucess");
            IPlayerEntity newPlayer = LoadPlayer("family_leave_member_sucess");

            _familyEventHandler.Execute(leader, new FamilyCreationEvent
            {
                Leader = leader,
                FamilyName = familyName
            });
            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = leader.Family,
            });
            Assert.IsTrue(newPlayer.HasFamily);
            Assert.AreEqual(leader.Family, newPlayer.Family);
            Assert.AreEqual(leader.Family.Id, newPlayer.FamilyCharacter.FamilyId);
            Assert.AreEqual(newPlayer.Character.Id, newPlayer.FamilyCharacter.CharacterId);
            Assert.AreEqual(newPlayer.FamilyCharacter.Authority, FamilyAuthority.Member);
            /// todo improve packet quality tests
            Assert.IsTrue(((SessionMock)newPlayer.Session).Packets.Any(s => s.Item1 == typeof(GidxPacket)));

            //
            _familyEventHandler.Execute(leader, new FamilyKickEvent
            {
                CharacterName = newPlayer.Character.Name,
                Family = newPlayer.Family,
                Kicker = leader
            });

            Assert.IsFalse(newPlayer.HasFamily);
            /// todo improve packet quality tests
            Assert.IsTrue(((SessionMock)newPlayer.Session).Packets.Any(s => s.Item1 == typeof(GidxPacket)));
        }

        [Test]
        public void Family_Leave_Fail_Family_Head()
        {
            // leader
            IPlayerEntity leader = LoadPlayer("family_test_leader");
            const string familyName = "family_leave_fail_family_head";

            _familyEventHandler.Execute(leader, new FamilyCreationEvent
            {
                Leader = leader,
                FamilyName = familyName
            });


            _familyEventHandler.Execute(leader, new FamilyLeaveEvent
            {
                Player = leader,
                Family = leader.Family,
            });

            Assert.IsTrue(leader.HasFamily);
            Assert.IsTrue(leader.IsFamilyLeader);
        }
    }
}