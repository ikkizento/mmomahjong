using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Plugin;
using Mahjong.Core;

namespace Mahjong.Referee.HongKong
{
    class Mahjongg : IRule
    {
        enum e_type
        {
            Single,
            Double,
            Chow,
            Pong,
            Kong
        };

        struct s_type
        {
            public e_type Type;
            public Group Group;
        }

        class GroupType : List<s_type> { }

        private List<PlayerData> m_players;
        List<GroupType> m_results = new List<GroupType>();

        public List<Mahjong.Plugin.IReferee.m_rulepossibility> Execute(List<PlayerData> players, PlayerData current)
        {
            m_players = players;
            List<Mahjong.Plugin.IReferee.m_rulepossibility> findrules = new List<IReferee.m_rulepossibility>();
            PlayerData PrevPlayer = GetPreviousPlayer(players, current);
            Tile ttmp = PrevPlayer.GetRejected();
            if (ttmp == null)
                return findrules;

            Group gtmp = current.GetHand().Clone();
            if (ttmp != null)
                gtmp.Add(ttmp);

            RecurMah(gtmp, new GroupType());
            
            // Analyse real Mahjong

            // Add Mahjong to findrules
            
            return findrules;
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

        //private Group IsPong(Group hand, Tile tile)
        //{
        //    Group ret = new Group();
        //    for (int i = 0; i < hand.Count; i++)
        //    {
        //        if ((hand[i].GetNumber() == tile.GetNumber()) && (hand[i].GetFamily() == tile.GetFamily()))
        //            ret.Add(hand[i]);
        //        if (ret.Count == 2)
        //        {
        //            ret.Add(tile);
        //            return ret;
        //        }
        //    }
        //    ret.Clear();
        //    return ret;
        //}

        //private Group IsKong(Group hand, Tile tile)
        //{
        //    Group ret = new Group();
        //    for (int i = 0; i < hand.Count; i++)
        //    {
        //        if ((hand[i].GetNumber() == tile.GetNumber()) && (hand[i].GetFamily() == tile.GetFamily()))
        //            ret.Add(hand[i]);
        //        if (ret.Count == 3)
        //        {
        //            ret.Add(tile);
        //            return ret;
        //        }
        //    }
        //    ret.Clear();
        //    return ret;
        //}

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
                    return ret;
                }
            }

            if (type == 2)
            {
                if ((HaveTile(hand, tile.GetFamily(), tile.GetNumber() - 1)) && (HaveTile(hand, tile.GetFamily(), tile.GetNumber() + 1)))
                {
                    ret.Add(GetTile(hand, tile.GetFamily(), tile.GetNumber() - 1));
                    ret.Add(GetTile(hand, tile.GetFamily(), tile.GetNumber() + 1));
                    return ret;
                }
            }

            if (type == 3)
            {
                if ((HaveTile(hand, tile.GetFamily(), tile.GetNumber() + 1)) && (HaveTile(hand, tile.GetFamily(), tile.GetNumber() + 3)))
                {
                    ret.Add(GetTile(hand, tile.GetFamily(), tile.GetNumber() + 1));
                    ret.Add(GetTile(hand, tile.GetFamily(), tile.GetNumber() + 2));
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
                    return m_players[j];
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
            return "Dans ton cul";
        }
    }
}
