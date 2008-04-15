using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Plugin;
using Mahjong.Core;

namespace Mahjong.Referee.HongKong
{
    class Kong : IRule
    {
        private List<PlayerData> m_players;

        public List<Mahjong.Plugin.IReferee.m_rulepossibility> Execute(List<PlayerData> players, PlayerData current)
        {
            m_players = players;
            List<Mahjong.Plugin.IReferee.m_rulepossibility> findrules = new List<IReferee.m_rulepossibility>();
            PlayerData tmp = GetRejectTilePlayer();
            if (tmp == null)
                return findrules;
            Tile rejected = tmp.GetRejected();
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
                    if (rulepos.Group.Count == 3)
                        findrules.Add(rulepos);
                }
            }
            Mahjong.Plugin.IReferee.m_rulepossibility rulepos2 = new IReferee.m_rulepossibility();
            rulepos2.Rule = this;
            rulepos2.Player = current;
            rulepos2.Group = new Group();
            Group etmp = current.GetExposed();
            for (int i = 0; i < etmp.Count; i++)
            {
                Tile ttmp = etmp[i];
                if ((rejected.GetNumber() == ttmp.GetNumber()) && (rejected.GetFamily() == ttmp.GetFamily()))
                {
                    rulepos2.Group.Add(ttmp);
                    if (rulepos2.Group.Count == 3)
                        findrules.Add(rulepos2);
                }
            }
            return findrules;
        }

        private PlayerData GetRejectTilePlayer()
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                if (m_players[i].GetRejected() != null)
                    return m_players[i];
            }
            return null;
        }

        public int GetScore()
        {
            return 42;
        }

        public String GetName()
        {
            return "Kong";
        }

        public String GetDescription()
        {
            return "Dans ton cul";
        }
    }
}
