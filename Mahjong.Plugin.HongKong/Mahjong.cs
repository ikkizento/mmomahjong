using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Plugin;
using Mahjong.Core;

namespace Mahjong.Referee.HongKong
{
    public class Mahjongg : IRule
    {
        public enum e_type
        {
            Single,
            Double,
            Chow,
            Pong,
            Kong
        };

        public struct s_type
        {
            public bool exposed;
            public e_type Type;
            public Group Group;
        }

        class GroupType : List<s_type> { }

        List<GroupType> m_results = new List<GroupType>();

        public List<Mahjong.Plugin.IReferee.m_rulepossibility> Execute(List<PlayerData> players, PlayerData current)
        {
            m_results.Clear();
            List<Mahjong.Plugin.IReferee.m_rulepossibility> findrules = new List<IReferee.m_rulepossibility>();
            PlayerData PrevPlayer = GetPreviousPlayer(players, current);
            Tile ttmp = PrevPlayer.GetRejected();
            Group gtmp = current.GetHand().Clone();

            if (ttmp != null)
                gtmp.Add(ttmp);

            RecurMah(gtmp, new GroupType());

            AddExposed(current);
            // Analyse real Mahjong
            RemoveFalseMahjong();
            // Add Mahjong to findrules
            for (int i = 0; i < m_results.Count; i++)
            {
                Mahjong.Plugin.IReferee.m_rulepossibility ins = new IReferee.m_rulepossibility();
                ins.Player = current;
                ins.Rule = this;
                ins.Group = current.GetHand();
                findrules.Add(ins);
            }
            return findrules;
        }

        private void AddExposed(PlayerData p)
        {
            for (int i = 0; i < m_results.Count; i++)
                AddTypeExposed(p, m_results[i]);
        }

        private void AddTypeExposed(PlayerData p, GroupType gtmp)
        {
            List<Group> tmp = p.GetExposed();
            
            for (int i = 0; i < tmp.Count; i++)
            {
                Group grotmp = tmp[i].Clone();
                s_type st = new s_type();

                grotmp.Remove(grotmp[0]);
                
                st.Group = tmp[i];
                st.exposed = true;
                st.Type = e_type.Single;
                if (IsChow(grotmp, tmp[i][0], 0).Count > 0)
                    st.Type = e_type.Chow;
                if (IsChow(grotmp, tmp[i][0], 1).Count > 0)
                    st.Type = e_type.Chow;
                if (IsChow(grotmp, tmp[i][0], 2).Count > 0)
                    st.Type = e_type.Chow;
                if (IsSimilar(grotmp, tmp[i][0], 3).Count > 0)
                    st.Type = e_type.Kong;
                if (st.Type == e_type.Single)
                    if (IsSimilar(grotmp, tmp[i][0], 2).Count > 0)
                        st.Type = e_type.Pong;
                if (st.Type != e_type.Single)
                    gtmp.Add(st);
            }
        }

        private bool RemoveFalseMahjong()
        {
            for (int i = 0; i < m_results.Count; i++)
            {
                if (NormalMahjong(m_results[i]) == false)
                {
                    m_results.Remove(m_results[i]);
                    i--;
                }
            }
            return true;
        }

        private bool NormalMahjong(GroupType gt)
        {
            int nbtwo = 0;
            for (int i = 0; i < gt.Count; i++)
            {
                if (gt[i].Type == e_type.Single)
                    return false;
                if (gt[i].Type == e_type.Double)
                    nbtwo++;
            }
            if (nbtwo > 1)
                return false;
            return true;
        }

        private bool RecurMah(Group hand, GroupType types)
        {
            if (hand.Count == 0)
            {
                m_results.Add(types);
                return true;
            }
            Group gtmp = hand.Clone();
            
            Tile tiletmp = gtmp[0];
            gtmp.Remove(tiletmp);

            #region Kong

            Group kong = IsSimilar(gtmp, tiletmp, 3);
            if (kong.Count > 0)
            {
                Group insgtmp = gtmp.Clone();
                insgtmp.Remove(kong[0]);
                insgtmp.Remove(kong[1]);
                insgtmp.Remove(kong[2]);
                GroupType ttmp = CopyType(types);
                s_type tmp = new s_type();
                tmp.exposed = false;
                tmp.Type = e_type.Kong;
                tmp.Group = kong;
                ttmp.Add(tmp);
                RecurMah(insgtmp, ttmp);
            }

            #endregion

            #region Pong
            
            Group pong = IsSimilar(gtmp, tiletmp, 2);
            if (pong.Count > 0)
            {
                Group insgtmp = gtmp.Clone();
                insgtmp.Remove(pong[0]);
                insgtmp.Remove(pong[1]);
                GroupType ttmp = CopyType(types);
                s_type tmp = new s_type();
                tmp.exposed = false;
                tmp.Type = e_type.Pong;
                tmp.Group = pong;
                ttmp.Add(tmp);
                RecurMah(insgtmp, ttmp);
            }

            #endregion

            #region Chow

            Group chow1 = IsChow(gtmp, tiletmp, 1);
            if (chow1.Count > 0)
            {
                Group insgtmp = gtmp.Clone();
                insgtmp.Remove(chow1[0]);
                insgtmp.Remove(chow1[1]);
                GroupType ttmp = CopyType(types);
                s_type tmp = new s_type();
                tmp.exposed = false;
                tmp.Type = e_type.Chow;
                tmp.Group = chow1;
                ttmp.Add(tmp);
                RecurMah(insgtmp, ttmp);
            }

            Group chow2 = IsChow(gtmp, tiletmp, 2);
            if (chow2.Count > 0)
            {
                Group insgtmp = gtmp.Clone();
                insgtmp.Remove(chow2[0]);
                insgtmp.Remove(chow2[1]);
                GroupType ttmp = CopyType(types);
                s_type tmp = new s_type();
                tmp.exposed = false;
                tmp.Type = e_type.Chow;
                tmp.Group = chow2;
                ttmp.Add(tmp);
                RecurMah(insgtmp, ttmp);
            }

            Group chow3 = IsChow(gtmp, tiletmp, 3);
            if (chow3.Count > 0)
            {
                Group insgtmp = gtmp.Clone();
                insgtmp.Remove(chow3[0]);
                insgtmp.Remove(chow3[1]);
                GroupType ttmp = CopyType(types);
                s_type tmp = new s_type();
                tmp.exposed = false;
                tmp.Type = e_type.Chow;
                tmp.Group = chow3;
                ttmp.Add(tmp);
                RecurMah(insgtmp, ttmp);
            }

            #endregion

            #region Pair

            Group doublee = IsSimilar(gtmp, tiletmp, 1);
            if (doublee.Count > 0)
            {
                Group insgtmp = gtmp.Clone();
                insgtmp.Remove(doublee[0]);
                GroupType ttmp = CopyType(types);
                s_type tmp = new s_type();
                tmp.exposed = false;
                tmp.Type = e_type.Double;
                tmp.Group = doublee;
                ttmp.Add(tmp);
                RecurMah(insgtmp, ttmp);
            }

            #endregion

            #region Single

            Group single = IsSingle(gtmp, tiletmp);
            if (single.Count > 0)
            {
                GroupType ttmp = CopyType(types);
                s_type tmp = new s_type();
                tmp.exposed = false;
                tmp.Type = e_type.Single;
                tmp.Group = single;
                ttmp.Add(tmp);
                RecurMah(gtmp, ttmp);
            }

            #endregion

            return false;
        }

        private Group IsSingle(Group hand, Tile tile)
        {
            Group ret = new Group();

            ret.Add(tile);
            return ret;
        }

        private Group IsSimilar(Group hand, Tile tile, int nb)
        {
            Group ret = new Group();
            for (int i = 0; i < hand.Count; i++)
            {
                if ((hand[i].GetNumber() == tile.GetNumber()) && (hand[i].GetFamily() == tile.GetFamily()))
                    ret.Add(hand[i]);
                if (ret.Count == nb)
                {
                    ret.Add(tile);
                    return ret;
                }
            }
            ret.Clear();
            return ret;
        }

        private Group IsChow(Group hand, Tile tile, int type)
        {
            Group ret = new Group();

            if ((tile.GetFamily() == Tile.Family.Flower) || (tile.GetFamily() == Tile.Family.Season) || (tile.GetFamily() == Tile.Family.Wind))
                return ret;
            
            if (type == 1)
            {
                if ((HaveTile(hand, tile.GetFamily(), tile.GetNumber() - 2)) && (HaveTile(hand, tile.GetFamily(), tile.GetNumber() - 1)))
                {
                    ret.Add(GetTile(hand, tile.GetFamily(), tile.GetNumber() - 2));
                    ret.Add(GetTile(hand, tile.GetFamily(), tile.GetNumber() - 1));
                    ret.Add(tile);
                    return ret;
                }
            }

            if (type == 2)
            {
                if ((HaveTile(hand, tile.GetFamily(), tile.GetNumber() - 1)) && (HaveTile(hand, tile.GetFamily(), tile.GetNumber() + 1)))
                {
                    ret.Add(GetTile(hand, tile.GetFamily(), tile.GetNumber() - 1));
                    ret.Add(GetTile(hand, tile.GetFamily(), tile.GetNumber() + 1));
                    ret.Add(tile);
                    return ret;
                }
            }

            if (type == 3)
            {
                if ((HaveTile(hand, tile.GetFamily(), tile.GetNumber() + 1)) && (HaveTile(hand, tile.GetFamily(), tile.GetNumber() + 2)))
                {
                    ret.Add(GetTile(hand, tile.GetFamily(), tile.GetNumber() + 1));
                    ret.Add(GetTile(hand, tile.GetFamily(), tile.GetNumber() + 2));
                    ret.Add(tile);
                    return ret;
                }
            }

            ret.Clear();
            return ret;
        }

        private GroupType CopyType(List<s_type> source)
        {
            GroupType ret = new GroupType();

            for (int i = 0; i < source.Count; i++)
                ret.Add(source[i]);

            return ret;
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


        private bool HaveTile(Group p, Tile.Family f, int pos)
        {
            Group g = p;

            for (int i = 0; i < g.Count; i++)
            {
                if ((g[i].GetFamily() == f) && (g[i].GetNumber() == pos))
                    return true;
            }
            return false;
        }

        private Tile GetTile(Group p, Tile.Family f, int pos)
        {
            Group g = p;

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
            return "Mahjong";
        }

        public String GetDescription()
        {
            return "Shot description of mahjong rule";
        }
    }
}
