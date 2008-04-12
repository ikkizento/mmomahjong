using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Library.Network;
using Mahjong.Plugin;
using Mahjong.Core;

namespace Mahjong.Server
{
    class MMOServer
    {
        public static NetServer Server;
        private List<NetPlayer> m_players = new List<NetPlayer>();
        private List<Room> m_rooms = new List<Room>();
        private bool HaveDeconnection;
        private NetLog m_log;
        private Plugin m_plugin;

        private struct GetMessages
        {
            public GetMessages(String op, GetMessage f)
            {
                OpCode = op;
                Funct = f;
            }
            public String OpCode;
            public GetMessage Funct;
        }
        private delegate bool GetMessage(NetPlayer p, String[] tab, NetMessage msg, ref String DiconnectionReason);
        GetMessages[] OpCodes;

        static void Main(string[] args)
        {
            MMOServer m = new MMOServer(24242);
        }

        public void Init()
        {
            OpCodes = new GetMessages[]
            { 
                //Login
                new GetMessages("LOG", new GetMessage(GetMessageLOG)), 
                //Room ADD JOIN LEAVE
                new GetMessages("ROM", new GetMessage(GetMessageROM)), 
                //Tile 
                new GetMessages("TIL", new GetMessage(GetMessageTIL)), 
            };
        }


        MMOServer(int port)
        {
            m_plugin = new Plugin(Environment.CurrentDirectory);

            Init();
            NetAppConfiguration config = new NetAppConfiguration("multi");
            config.MaximumConnections = 1024;
            config.Port = port;
            config.ServerName = Environment.MachineName + " server";

            m_log = new NetLog();
            m_log.LogEvent += new EventHandler<NetLogEventArgs>(OnLogEvent);

            Server = new NetServer(config, m_log);
            Server.StatusChanged += new EventHandler<NetStatusEventArgs>(OnStatusChange);

            while (true)
            {
                Server.Heartbeat();

                NetMessage msg;
                while ((msg = Server.ReadMessage()) != null)
                    HandleMessage(msg);

                System.Threading.Thread.Sleep(1);
            }
        }

        void OnStatusChange(object sender, NetStatusEventArgs e)
        {
            if (e.Connection.Status == NetConnectionStatus.Connected)
            {
                NetMessage outMsg = new NetMessage();
                outMsg.Write("MSG:Server:Welcome:0.01:" + m_players.Count);
                Server.SendMessage(outMsg, e.Connection, NetChannel.Unreliable);
                NetPlayer ins = new NetPlayer(e.Connection, "");
                m_players.Add(ins);
                Console.WriteLine("Client connected; " + Server.NumConnected + " of 100");
            }
            if (e.Connection.Status == NetConnectionStatus.Disconnected)
            {
                HaveDeconnection = true;
                RemoveDeconnectedPlayer();
            }
        }

        void OnLogEvent(object sender, NetLogEventArgs e)
        {
            Console.WriteLine(e.Entry.What);
        }

        void HandleMessage(NetMessage msg)
        {
            string str = msg.ReadString();
            string[] tab = str.Split(':');
            
            NetPlayer pltmp = GetPlayer(msg.Sender);
            Console.WriteLine("Client:" + str);
            if (HaveDeconnection == true)
            {
                RemoveDeconnectedPlayer();
                HaveDeconnection = false;
            }
            
            for (int i = 0; i < OpCodes.Length; i++)
            {
                if (tab[0] == OpCodes[i].OpCode)
                {
                    String Disconnect = "";
                    m_log.Info(OpCodes[i].OpCode);
                    if (OpCodes[i].Funct(pltmp, tab, msg, ref Disconnect) == false)
                    {
                        msg.Sender.Disconnect(Disconnect);
                        HaveDeconnection = true;
                    }
                }
            }
            
        }

        private NetPlayer GetPlayer(NetConnection nc)
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                NetPlayer tmp = m_players[i];
                if (nc == tmp.PlayerConnection)
                    return tmp;
            }
            return null;
        }

        private bool SendAll(String Msg)
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                NetPlayer tmp = m_players[i];
                tmp.Send(Msg);
            }
            return true;
        }

        private void RemoveDeconnectedPlayer()
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                NetPlayer tmp = m_players[i];
                if (tmp.PlayerConnection.Status == NetConnectionStatus.Disconnected)
                {
                    m_players.Remove(tmp);
                    if (tmp.CurrentRoom != null)
                    {
                        tmp.CurrentRoom.Send("ROM:PLAYER:LEAVE:" + tmp.GetName(), null);
                        tmp.CurrentRoom.RemovePlayer(tmp);
                    }
                }
            }
        }

        private bool GetMessageLOG(NetPlayer p, String[] tab, NetMessage msg, ref String DiconnectionReason)
        {
            p.Send("LOG:OK");
            p.SetName(tab[1]);
            for (int i = 0; i < m_rooms.Count; i++)
            {
                p.Send("ROM:LST:" + m_rooms[i].Name + ":" + m_rooms[i].GetPlayer() + ":" + m_rooms[i].GetMaxPlayer()); 
            }

            return true;
        }

        private Room GetRoomByName(string name)
        {
            for (int i = 0; i < m_rooms.Count; i++)
            {
                if (m_rooms[i].Name == name)
                    return m_rooms[i];
            }
            return null;
        }

        private bool GetMessageROM(NetPlayer p, String[] tab, NetMessage msg, ref String DiconnectionReason)
        {
            

            if (tab[1] == "ADD")
            {
                //Room already exist ??
                Room ins = new Room(tab[2]);
                m_rooms.Add(ins);
                SendAll("ROM:LST:" + ins.Name + ":" + ins.GetPlayer() + ":" + ins.GetMaxPlayer());
                p.Send("ROM:ADD:OK");
            }

            if (tab[1] == "DEL")
            {
                //Room ins = new Room(tab[2]);
            }

            if (tab[1] == "JOIN")
            {
                //Room ins = new Room(tab[2]);
                Room tmp = GetRoomByName(tab[2]);
                if (tmp.GetPlayer() == tmp.GetMaxPlayer())
                {
                    p.Send("ROM:JOIN:KO");
                    return true;
                }
                p.Send("ROM:JOIN:OK");
                tmp.AddPlayer(p);
                p.CurrentRoom = tmp;
                for (int i = 0; i < tmp.GetPlayer(); i++)
                {
                    NetPlayer tmpp = tmp.GetPlayer(i);
                    p.Send("ROM:PLAYER:ADD:" + tmpp.GetName());
                }
                List<IReferee> plugins = m_plugin.GetReferees();
                for (int i = 0; i < plugins.Count; i++)
                {
                    p.Send("ROM:REFEREE:ADD:" + plugins[i].GetName());
                }
                if (tmp.GetReferee() != null)
                {
                    p.Send("ROM:REFEREE:SELECTED:" + tmp.GetReferee().GetName());
                }
            }

            if (tab[1] == "LEAVE")
            {
                Room tmp = p.CurrentRoom;
                for (int i = 0; i < tmp.GetPlayer(); i++)
                {
                    NetPlayer tmpp = tmp.GetPlayer(i);
                    tmpp.Send("ROM:PLAYER:LEAVE:" + tmpp.GetName());
                }
                tmp.RemovePlayer(p);
            }

            if (tab[1] == "REFEREE")
            {
                Room tmp = p.CurrentRoom;
                List<IReferee> plugins = m_plugin.GetReferees();
                IReferee iref = null;
                for (int i = 0; i < plugins.Count; i++)
                {
                    if (plugins[i].GetName() == tab[2])
                        iref = plugins[i];
                }
                if (iref != null)
                {
                    tmp.AddReferee(iref);
                    p.Send("ROM:REFEREE:SELECTED:" + tmp.GetReferee().GetName());
                }
                else
                    p.Send("ROM:REFEREE:KO");
            }

            if (tab[1] == "START")
            {
                // Make PlayerData List
                Room tmp = p.CurrentRoom;
                List<PlayerData> ltmp = new List<PlayerData>();

                tmp.Send("STR:" + p.GetName(), null);
                for (int i = 0; i < tmp.GetPlayer(); i++)
                {
                    ltmp.Add(tmp.GetPlayer(i));
                }
                
                p.CurrentRoom.GetReferee().NewGame(ltmp);
                for (int i = 0; i < tmp.GetPlayer(); i++)
                    SendHandTiles(tmp.GetPlayer(i));
                SendTurn(p);
            }

            return true;
        }

        private bool SendHandTiles(NetPlayer p)
        {
            Group tmp = p.GetHand();
            for (int i = 0; i < tmp.Count; i++)
            {
                p.Send("TIL:GET:" + tmp[i].GetFamily().ToString() + ":" + tmp[i].GetNumber().ToString());
            }
            return true;
        }

        private bool SendTurn(NetPlayer p)
        {
            Room tmp = p.CurrentRoom;
            NetPlayer ntmp = (NetPlayer)tmp.GetReferee().CurrentPlayer();

            return tmp.Send("TRN:" + ntmp.GetName(), null);
        }

        private bool GetMessageTIL(NetPlayer p, String[] tab, NetMessage msg, ref String DiconnectionReason)
        {
            if (tab[1] == "TAKE")
            {
                Tile t = p.CurrentRoom.GetReferee().Take();

                if (t == null)
                {
                    p.Send("TIL:TAKE:KO");
                }
                else
                {
                    p.Send("TIL:GET:" + t.GetFamily().ToString() + ":" + t.GetNumber().ToString());
                }
            }
            if (tab[1] == "REMOVE")
            {
                Tile t = p.CurrentRoom.GetReferee().GetTile(tab[2], Convert.ToInt32(tab[3]));
                if (p.CurrentRoom.GetReferee().Rejected(t) == false)
                    p.Send("TIL:REMOVE:KO");
                else
                {
                    p.Send("TIL:REMOVE:" + tab[2] + ":" + tab[3]);
                    p.CurrentRoom.Send("TIL:REMOVED:" + p.GetName() + ":" + tab[2] + ":" + tab[3], null);
                    SendTurn(p);
                }
            }

            return true;
        }
    }
}
