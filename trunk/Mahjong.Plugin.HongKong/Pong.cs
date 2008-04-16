using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Plugin;
using Mahjong.Core;

namespace Mahjong.Referee.HongKong
{
    public class Pong : IRule
    {
        public List<Mahjong.Plugin.IReferee.m_rulepossibility> Execute(List<PlayerData> players, PlayerData current)
        {
            List<Mahjong.Plugin.IReferee.m_rulepossibility> findrules = new List<IReferee.m_rulepossibility>();
            PlayerData tmp = GetRejectTilePlayer(players);
            if (tmp == null)
                return findrules;
            if (tmp == current)
                return findrules;
            Tile rejected = tmp.GetRejected();
            if (rejected == null)
                return findrules;
            Group gtmp = current.GetHand();

            Mahjong.Plugin.IReferee.m_rulepossibility rulepos = new IReferee.m_rulepossibility();
            rulepos.Rule = this;
            rulepos.Player = current;
            rulepos.Group = new Group();
            for (int i = 0; i < gtmp.Count; i++)
            {
                Tile ttmp = gtmp[i];
                if ((rejected.GetNumber() == ttmp.GetNumber()) && (rejected.GetFamily() == ttmp.GetFamily()))
                {
                    rulepos.Group.Add(ttmp);
                    if (rulepos.Group.Count == 2)
                        findrules.Add(rulepos);
                }
            }

            return findrules;
        }

        private PlayerData GetRejectTilePlayer(List<PlayerData> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].GetRejected() != null)
                    return players[i];
            }
            return null;
        }

        public int GetScore()
        {
            return 42;
        }

        public String GetName()
        {
            return "Pong";
        }

        public String GetDescription()
        {
            return "Shot description of pong rule";
        }
    }
}
