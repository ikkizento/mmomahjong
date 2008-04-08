using System;
using System.Collections.Generic;
using System.Text;

//class IReferee;

namespace Mahjong.Core
{
    class Board
    {
        List<Player> m_players = new List<Player>();
        //IReferee m_referee;

        public bool AddPlayer(String name)
        {
            Player ins = new Player(name);
            m_players.Add(ins);
            return true;
        }

    }
}
