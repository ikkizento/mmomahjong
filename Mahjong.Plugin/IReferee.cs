using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Core;

namespace Mahjong.Plugin
{
    public abstract class IReferee
    {
        protected enum TilePosition
        {
            Free,
            Player,
            Cemetery,
            Rejected
        }

        protected struct m_stile
        {
            public TilePosition Position;
            public Tile Tile;
        }

        public struct m_rulepossibility
        {
            public Group Group;
            public IRule Rule;
            public PlayerData Player;
            public bool Equal(m_rulepossibility tmp)
            {
                if (Player != tmp.Player)
                    return false;
                if (Rule != tmp.Rule)
                    return false;
                if (Group.Equal(tmp.Group) == false)
                    return false;
                return true;
            }
        }

        protected PlayerData m_current;
        protected List<PlayerData> m_players = new List<PlayerData>();
        protected List<m_stile> m_tiles;
        protected List<IRule> m_rules = new List<IRule>();

        protected bool m_mutextaken;
        protected virtual void GenerateTiles()
        {
            Mahjong.Core.Tile.Family family = Tile.Family.Circle;
            int i;
            int j;

            for (j = 0; j < 3; j++)
            {
                for (i = 1; i < 10; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                family = Tile.Family.Character;
                for (i = 1; i < 10; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                family = Tile.Family.Bamboo;
                for (i = 1; i < 10; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                family = Tile.Family.Wind;
                for (i = 1; i < 5; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                //family = Tile.Family.Flower;
                //for (i = 1; i < 5; i++)
                //{
                //    m_stile ins = new m_stile();
                //    ins.Position = TilePosition.Free;
                //    ins.Tile = new Tile(family, i);
                //    m_tiles.Add(ins);
                //}
                family = Tile.Family.Season;
                for (i = 1; i < 5; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                family = Tile.Family.Dragon;
                for (i = 1; i < 4; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
            }
        }

        protected virtual void RandomizeTitles()
        {
            Random r = new Random();

            for (int j = 0; j < m_tiles.Count; j++)
            {
                m_stile tmp = m_tiles[j];

                int i = r.Next(m_tiles.Count);
                m_tiles[j] = m_tiles[i];
                m_tiles[i] = tmp;
            }
        }

        protected virtual void DistributeTitles()
        {
            for (int j = 0; j < m_players.Count; j++)
            {
                for (int i = 0; i < 13; i++)
                {
                    m_players[j].AddHand(m_tiles[j * 13 + i].Tile);
                    m_stile p = m_tiles[j * 13 + i];
                    p.Position = TilePosition.Player;
                    m_tiles[j * 13 + i] = p;
                }
            }
        }

        protected int GetIndexFreeTile()
        {
            for (int j = 0; j < m_tiles.Count; j++)
            {
                if (m_tiles[j].Position == TilePosition.Free)
                    return j;
            }
            return -1;
        }

        public int GetNumberFreeTile()
        {
            int i = 0;
            for (int j = 0; j < m_tiles.Count; j++)
            {
                if (m_tiles[j].Position == TilePosition.Free)
                    i++;
            }
            return i;
        }

        public void Reset()
        {
            m_tiles = new List<m_stile>();
        }

        protected Tile GetNewTile()
        {
            int ti = GetIndexFreeTile();

            // End of game no tile left
            if (ti == -1)
                throw new EndGameException();

            ChangeTileStatus(m_tiles[ti].Tile, TilePosition.Player);
            return m_tiles[ti].Tile;
        }

        public Tile GetTile(String family, int number)
        {
            for (int j = 0; j < m_tiles.Count; j++)
            {
                if ((m_tiles[j].Tile.GetFamily().ToString() == family) && (m_tiles[j].Tile.GetNumber() == number))
                {
                    return m_tiles[j].Tile;
                }
            }
            return null;
        }

        public bool NewGame(List<PlayerData> players)
        {
            if (players.Count == 0)
                return false;
            Reset();
            m_players = players;
            GenerateTiles();
            RandomizeTitles();
            DistributeTitles();
            m_current = m_players[0];
            
            SetPlayerPosition();

            return true;
        }

        public bool NewGame()
        {
            if (m_players.Count == 0)
                return false;
            Reset();
            GenerateTiles();
            RandomizeTitles();
            DistributeTitles();
            m_current = m_players[0];
            SetPlayerPosition();

            return true;
        }

        protected bool SetPlayerPosition()
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                m_players[i].SetPosition(i);
            }
            return true;
        }

        public bool AddPlayer(PlayerData ins)
        {
            m_players.Add(ins);
            return true;
        }

        public IReferee()
        {
            Reset();
            
        }

        /// <summary>
        /// Who is the next player
        /// </summary>
        /// <returns></returns>
        private PlayerData GetNextPlayer()
        {
            int idx = m_players.IndexOf(m_current);
            idx++;
            if (idx == m_players.Count)
                idx = 0;
            return m_players[idx];
        }

        private PlayerData GetPreviousPlayer()
        {
            int idx = m_players.IndexOf(m_current);
            idx--;
            if (idx == -1)
                idx = m_players.Count - 1;
            return m_players[idx];
        }

        public Tile GetRejectTile()
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                if (m_players[i].GetRejected() != null)
                    return m_players[i].GetRejected();
            }
            return null;
        }

        public PlayerData GetRejectPlayer()
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                if (m_players[i].GetRejected() != null)
                    return m_players[i];
            }
            return null;
        }

        public abstract String GetName();

        public abstract int GetMaxPlayer();

        public List<m_rulepossibility> GetRulesPossibilities(PlayerData player)
        {
            List<m_rulepossibility> ins = new List<m_rulepossibility>();

            for (int i = 0; i < m_rules.Count; i++)
            {
                List<m_rulepossibility> tmp = m_rules[i].Execute(m_players, player);
                for (int j = 0; j < tmp.Count; j++)
                    ins.Add(tmp[j]);
            }

            return ins;
        }

        public abstract bool Call(m_rulepossibility rulepos);


        public abstract String GetDescription();

        public PlayerData CurrentPlayer()
        {
            return m_current;
        }

        protected bool ChangeTileStatus(Tile t, TilePosition tp)
        {
            for (int i = 0; i < m_tiles.Count; i++)
            {
                if (m_tiles[i].Tile == t)
                {
                    m_stile tmp = m_tiles[i];
                    tmp.Position = tp;
                    m_tiles[i] = tmp;
                    return true;
                }
            }
            return false;

        }

        protected TilePosition GetTilePosition(Tile t)
        {
            for (int i = 0; i < m_tiles.Count; i++)
                if (m_tiles[i].Tile == t)
                    return m_tiles[i].Position;
            return TilePosition.Cemetery;
        }

        public bool Rejected(Tile t)
        {
            if (m_mutextaken == false)
                return false;
            m_mutextaken = false;
            m_current.GetHand().Remove(t);
            m_current.AddRejected(t);
            ChangeTileStatus(t, TilePosition.Rejected);
            GetPreviousPlayer().AddRejected(null);
            m_current = GetNextPlayer();

            return true;
        }

        public Tile Take()
        {
            if (m_mutextaken == true)
                return null;
            try
            {
                Tile tmp = GetNewTile();
                m_current.AddHand(tmp);
                m_mutextaken = true;
                Tile ttmp = GetPreviousPlayer().GetRejected();
                TilePosition tp = GetTilePosition(ttmp);
                if (tp == TilePosition.Rejected)
                {
                    ChangeTileStatus(GetPreviousPlayer().GetRejected(), TilePosition.Cemetery);
                    GetPreviousPlayer().AddCemetery(GetPreviousPlayer().GetRejected());
                    GetPreviousPlayer().AddRejected(null);
                }
                return tmp;
            }
            catch (EndGameException edg)
            {
                ///
            }
            return null;
        }

        public IRule GetRuleByName(string p)
        {
            for (int i = 0; i < m_rules.Count; i++)
            {
                if (m_rules[i].GetName() == p)
                    return m_rules[i];
            }

            return null;
        }
    }
}
