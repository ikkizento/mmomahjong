using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Plugin;
using Mahjong.Core;

namespace Mahjong.Server
{
    class Room
    {
        IReferee m_referee = null;
        String m_name;

        private List<NetPlayer> m_players = new List<NetPlayer>();

        public Room(String name)
        {
            m_name = name;
        }

        public String Name
        {
            get
            {
                return m_name;
            }
        }

        public void AddReferee(IReferee refe)
        {
            m_referee = refe;
        }



        public IReferee GetReferee()
        {
            return m_referee;
        }

        public int GetMaxPlayer()
        {
            return 42;
        }

        public int GetPlayer()
        {
            return m_players.Count;
        }

        //public NetPlayer GetPlayer(PlayerData pd)
        //{
        //    for (int i = 0; i < m_players.Count; i++)
        //    {
        //        if ((PlayerData)m_players[i] == pd)
        //            return m_players[i];
        //    }
        //    return null;
        //}

        public NetPlayer GetPlayer(int i)
        {
            return m_players[i];
        }

        public bool AddPlayer(NetPlayer p)
        {
            m_players.Add(p);
            return true;
        }

        public bool RemovePlayer(NetPlayer p)
        {
            return m_players.Remove(p);
        }

        public bool Send(String msg, NetPlayer avoid)
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                if (m_players[i] != avoid)
                    m_players[i].Send(msg);
            }
            return true;
        }
    }
}
