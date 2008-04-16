using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Core;
using Mahjong.Plugin;

namespace Mahjong.Referee.HongKong
{
    class Chow : IRule
    {
        public List<Mahjong.Plugin.IReferee.m_rulepossibility> Execute(List<PlayerData> players, PlayerData current)
        {
            List<Mahjong.Plugin.IReferee.m_rulepossibility> findrules = new List<IReferee.m_rulepossibility>();
            PlayerData PrevPlayer = GetPreviousPlayer(players, current);
            Tile ttmp = PrevPlayer.GetRejected();
            
            if (ttmp == null)
                return findrules;
            if ((ttmp.GetFamily() == Tile.Family.Flower) || (ttmp.GetFamily() == Tile.Family.Season) || (ttmp.GetFamily() == Tile.Family.Wind))
                return findrules;

            if ((HaveTile(current, ttmp.GetFamily(), ttmp.GetNumber() - 2)) && (HaveTile(current, ttmp.GetFamily(), ttmp.GetNumber() - 1)))
            {
                Mahjong.Plugin.IReferee.m_rulepossibility rulepos = new IReferee.m_rulepossibility();
                rulepos.Rule = this;
                rulepos.Player = current;
                rulepos.Group = new Group();

                rulepos.Group.Add(GetTile(current, ttmp.GetFamily(), ttmp.GetNumber() - 2));
                rulepos.Group.Add(GetTile(current, ttmp.GetFamily(), ttmp.GetNumber() - 1));
                findrules.Add(rulepos);
            }

            if ((HaveTile(current, ttmp.GetFamily(), ttmp.GetNumber() - 1)) && (HaveTile(current, ttmp.GetFamily(), ttmp.GetNumber() + 1)))
            {
                Mahjong.Plugin.IReferee.m_rulepossibility rulepos = new IReferee.m_rulepossibility();
                rulepos.Rule = this;
                rulepos.Player = current;
                rulepos.Group = new Group();

                rulepos.Group.Add(GetTile(current, ttmp.GetFamily(), ttmp.GetNumber() - 1));
                rulepos.Group.Add(GetTile(current, ttmp.GetFamily(), ttmp.GetNumber() + 1));
                findrules.Add(rulepos);
            }

            if ((HaveTile(current, ttmp.GetFamily(), ttmp.GetNumber() + 1)) && (HaveTile(current, ttmp.GetFamily(), ttmp.GetNumber() + 2)))
            {
                Mahjong.Plugin.IReferee.m_rulepossibility rulepos = new IReferee.m_rulepossibility();
                rulepos.Rule = this;
                rulepos.Player = current;
                rulepos.Group = new Group();

                rulepos.Group.Add(GetTile(current, ttmp.GetFamily(), ttmp.GetNumber() + 1));
                rulepos.Group.Add(GetTile(current, ttmp.GetFamily(), ttmp.GetNumber() + 2));
                findrules.Add(rulepos);
            }
            return findrules;
        }

        private PlayerData GetPreviousPlayer(List<PlayerData> players, PlayerData current)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == current)
                {
                    int j = i - 1;
                    if (j == -1)
                        j = players.Count - 1;
                    return players[j];
                }
                    
            }
            return null;
        }

        private bool HaveTile(PlayerData p, Tile.Family f, int pos)
        {
            Group g = p.GetHand();

            for (int i = 0; i < g.Count; i++)
            {
                if ((g[i].GetFamily() == f) && (g[i].GetNumber() == pos))
                    return true;
            }
            return false;
        }

        private Tile GetTile(PlayerData p, Tile.Family f, int pos)
        {
            Group g = p.GetHand();

            for (int i = 0; i < g.Count; i++)
            {
                if ((g[i].GetFamily() == f) && (g[i].GetNumber() == pos))
                    return g[i];
            }
            return null;
        }

        public int GetScore()
        {
            return 42;
        }

        public String GetName()
        {
            return "Chow";
        }

        public String GetDescription()
        {
            return "Shot description of chow rule";
        }
    }
}
