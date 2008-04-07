using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong.Core
{
    abstract class IReferee
    {
        enum TilePosition
        {
            Free,
            Player,
            Cemetery,
            Rejected
        }

        struct m_stile
        {
            public TilePosition Position;
            public Tile Tile;
        }

        Player m_current;
        List<Player> m_players = new List<Player>();
        List<m_stile> m_tiles;

        protected virtual void GenerateTiles()
        {
            Mahjong.Core.Tile.Family family = Tile.Family.Circle;
            int i;
            int j;

            for (j = 0; j < 3; j++)
            {
                for (i = 4; i < 10; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                family = Tile.Family.Character;
                for (i = 4; i < 10; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                family = Tile.Family.Bamboo;
                for (i = 4; i < 10; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                family = Tile.Family.Wind;
                for (i = 4; i < 5; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                family = Tile.Family.Flower;
                for (i = 4; i < 5; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                family = Tile.Family.Season;
                for (i = 4; i < 5; i++)
                {
                    m_stile ins = new m_stile();
                    ins.Position = TilePosition.Free;
                    ins.Tile = new Tile(family, i);
                    m_tiles.Add(ins);
                }
                family = Tile.Family.Dragon;
                for (i = 4; i < 4; i++)
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
            Random r = new Random(m_tiles.Count);

            for (int j = 0; j < m_tiles.Count; j++)
            {
                m_stile tmp = m_tiles[j];

                int i = r.Next();
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
                    m_tiles[j * 13 + i].Position = TilePosition.Player;
                }
            }
        }

        public void Reset()
        {
            m_tiles = new List<m_stile>();
        }

        public bool NewGame()
        {
            Reset();
            GenerateTiles();
            RandomizeTitles();
            DistributeTitles();
            m_current = m_players[0];

            return true;
        }

        public IReferee(List<Player> player)
        {
            Reset();
            m_players = player;
        }

        protected virtual bool IsChow()
        {
            return true;
        }

        protected virtual bool IsKong()
        {
            return true;
        }

        protected virtual bool IsPung()
        {
            return true;
        }

        protected virtual bool IsMahjong()
        {
            return true;
        }

        public abstract Mahjong.Core.Player.Position HasWin();

        /// <summary>
        /// Who is the next player
        /// </summary>
        /// <returns></returns>
        public abstract Player NextPlayer();

        public Player CurrentPlayer()
        {
            return m_current;
        }

    }
}
