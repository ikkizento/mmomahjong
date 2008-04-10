using System;
using System.Collections.Generic;
using System.Text;

//class IReferee;

namespace Mahjong.Core
{
    class Board
    {
        List<PlayerData> m_players = new List<PlayerData>();
        //IReferee m_referee;

        public bool AddPlayer(String name)
        {
            PlayerData ins = new PlayerData(name);
            m_players.Add(ins);
            return true;
        }

    }
}
