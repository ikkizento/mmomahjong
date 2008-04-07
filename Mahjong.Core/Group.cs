using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong.Core
{
    public class Group
    {
        List<Tile> m_tites;

        public Group(List<Tile> tites)
        {
            m_tites = tites;
        }
        
        public Group()
        {
            m_tites = new List<Tile>();
        }

        public bool AddTile(Tile tile)
        {
            m_tites.Add(tile);
            return true;
        }

        public bool RemoveTile(Tile tile)
        {
            m_tites.Remove(tile);
            return true;
        }

    }
}
