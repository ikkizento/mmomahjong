using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Core;
using Lidgren.Library.Network;

namespace Mahjong.Server
{
    class NetPlayer : PlayerData
    {
        public Room CurrentRoom = null;
        private NetConnection m_connection;

        public NetConnection PlayerConnection
        {
            get
            {
                return m_connection;
            }
        }

        public NetPlayer(NetConnection con, String name) : base(name)
        {
            m_connection = con;
        }

        public bool Send(String msg)
        {
            NetMessage outMsg = new NetMessage();
            outMsg.Write(msg);
            return MMOServer.Server.SendMessage(outMsg, m_connection, NetChannel.ReliableUnordered);
        }
    }
}
