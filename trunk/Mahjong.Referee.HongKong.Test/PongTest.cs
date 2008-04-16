using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Mahjong.Core;
using Mahjong.Plugin;
using Mahjong.Referee.HongKong;
namespace Mahjong.Referee.HongKong.Test
{
    [TestFixture]
    public class PongTest
    {
        [Test]
        public void PongSimple1()
        {
            PlayerData p1 = new PlayerData("toto");
            PlayerData p2 = new PlayerData("toto2");

            p1.AddHand(new Tile( Tile.Family.Bamboo, 1));
            p1.AddHand(new Tile( Tile.Family.Bamboo, 1));

            p2.AddRejected(new Tile( Tile.Family.Bamboo , 1));
            IRule rule = new Pong();
            
            List<PlayerData> lpd = new List<PlayerData>();
            lpd.Add(p1);
            lpd.Add(p2);

            List<Mahjong.Plugin.IReferee.m_rulepossibility> possi = rule.Execute(lpd, p1);

            Assert.AreEqual(possi.Count, 1);
            List<Mahjong.Plugin.IReferee.m_rulepossibility> possi2 = rule.Execute(lpd, p2);

            Assert.AreEqual(possi2.Count, 0);
        }

        [Test]
        public void PongSimple2()
        {
            PlayerData p1 = new PlayerData("toto");
            PlayerData p2 = new PlayerData("toto2");

            p1.AddHand(new Tile(Tile.Family.Bamboo, 1));
            p1.AddHand(new Tile(Tile.Family.Bamboo, 1));
            p2.AddHand(new Tile(Tile.Family.Bamboo, 1));
            
            p2.AddRejected(new Tile(Tile.Family.Bamboo, 1));
            IRule rule = new Pong();

            List<PlayerData> lpd = new List<PlayerData>();
            lpd.Add(p1);
            lpd.Add(p2);

            List<Mahjong.Plugin.IReferee.m_rulepossibility> possi = rule.Execute(lpd, p2);
            Assert.AreEqual(possi.Count, 0);
            List<Mahjong.Plugin.IReferee.m_rulepossibility> possi2 = rule.Execute(lpd, p1);
            Assert.AreEqual(possi2.Count, 1);
        }

        [Test]
        public void PongSimple3()
        {
            PlayerData p1 = new PlayerData("toto");
            PlayerData p2 = new PlayerData("toto2");

            p1.AddHand(new Tile(Tile.Family.Bamboo, 1));
            p1.AddHand(new Tile(Tile.Family.Bamboo, 1));
            p1.AddHand(new Tile(Tile.Family.Bamboo, 1));

            p2.AddRejected(new Tile(Tile.Family.Bamboo, 1));
            IRule rule = new Pong();

            List<PlayerData> lpd = new List<PlayerData>();
            lpd.Add(p1);
            lpd.Add(p2);

            List<Mahjong.Plugin.IReferee.m_rulepossibility> possi = rule.Execute(lpd, p1);
            Assert.AreEqual(possi.Count, 1);
        }

        [Test]
        public void PongSimple4()
        {
            PlayerData p1 = new PlayerData("toto");
            PlayerData p2 = new PlayerData("toto2");

            p1.AddHand(new Tile(Tile.Family.Bamboo, 1));
            p1.AddHand(new Tile(Tile.Family.Bamboo, 1));
            p1.AddHand(new Tile(Tile.Family.Bamboo, 2));
            p1.AddHand(new Tile(Tile.Family.Bamboo, 2));
            p1.AddHand(new Tile(Tile.Family.Bamboo, 3));
            p1.AddHand(new Tile(Tile.Family.Character, 4));

            p2.AddRejected(new Tile(Tile.Family.Bamboo, 1));
            IRule rule = new Pong();

            List<PlayerData> lpd = new List<PlayerData>();
            lpd.Add(p1);
            lpd.Add(p2);

            List<Mahjong.Plugin.IReferee.m_rulepossibility> possi = rule.Execute(lpd, p1);

            Assert.AreEqual(possi.Count, 1);
        }

        [Test]
        public void PongSimple5()
        {
            PlayerData p1 = new PlayerData("toto");
            PlayerData p2 = new PlayerData("toto2");

            p1.AddHand(new Tile(Tile.Family.Bamboo, 1));
            p1.AddHand(new Tile(Tile.Family.Bamboo, 1));
            p2.AddHand(new Tile(Tile.Family.Bamboo, 1));
            p1.AddHand(new Tile(Tile.Family.Bamboo, 2));
            p1.AddHand(new Tile(Tile.Family.Bamboo, 3));
            p1.AddHand(new Tile(Tile.Family.Character, 4));

            p1.AddRejected(new Tile(Tile.Family.Bamboo, 1));
            IRule rule = new Pong();

            List<PlayerData> lpd = new List<PlayerData>();
            lpd.Add(p1);
            lpd.Add(p2);

            List<Mahjong.Plugin.IReferee.m_rulepossibility> possi = rule.Execute(lpd, p1);

            Assert.AreEqual(possi.Count, 0);
        }
    }
}
