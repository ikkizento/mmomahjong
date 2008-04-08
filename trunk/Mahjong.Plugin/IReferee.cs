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

        protected Player m_current;
        protected List<Player> m_players = new List<Player>();
        protected List<m_stile> m_tiles;

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
                family = Tile.Family.Flower;
                for (i = 1; i < 5; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
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

        private int GetIndexFreeTile()
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

        public bool NewGame(List<Player> players)
        {
            Reset();
            GenerateTiles();
            RandomizeTitles();
            DistributeTitles();
            m_current = m_players[0];
            m_players = players;
            SetPlayerPosition();
            int ti = GetIndexFreeTile();
            ChangeTileStatus(m_tiles[ti].Tile, TilePosition.Player);
            m_current.AddHand(m_tiles[ti].Tile);
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
            int ti = GetIndexFreeTile();
            ChangeTileStatus(m_tiles[ti].Tile, TilePosition.Player);
            m_current.AddHand(m_tiles[ti].Tile);
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

        public bool AddPlayer(Player ins)
        {
            m_players.Add(ins);
            return true;
        }

        public IReferee()
        {
            Reset();
            
        }

        public abstract Mahjong.Core.Player.Position HasWin();

        /// <summary>
        /// Who is the next player
        /// </summary>
        /// <returns></returns>
        private Player NextPlayer()
        {
            int idx = m_players.IndexOf(m_current);
            idx++;
            if (idx == m_players.Count)
                idx = 0;
            return m_players[idx];
        }

        public abstract String GetName();

        public abstract String GetDescription();

        public Player CurrentPlayer()
        {
            return m_current;
        }

        private bool ChangeTileStatus(Tile t, TilePosition tp)
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

        public bool Rejected(Tile t)
        {
            m_current.GetHand().Remove(t);
            ChangeTileStatus(t, TilePosition.Rejected);
            m_current = NextPlayer();
            int ti = GetIndexFreeTile();

            if (ti == -1)
                return false;

            ChangeTileStatus(m_tiles[ti].Tile, TilePosition.Player);
            m_current.AddHand(m_tiles[ti].Tile);
            return true;
        }
    }
}
