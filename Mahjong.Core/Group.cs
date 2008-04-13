using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong.Core
{

    public class Group : List<Tile>
    {
        public bool Equal(List<Tile> tiles)
        {
            if (this.Count != tiles.Count)
                return false;
            for (int i = 0; i < this.Count; i++)
            {
                if ((this[i].GetFamily() == tiles[i].GetFamily()) && (this[i].GetNumber() == tiles[i].GetNumber()))
                return true;
            }
            return false;
        }

    }
}
