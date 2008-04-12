using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Core;
using Lidgren.Library.Network;
using System.Threading;
using System.Windows.Forms;

namespace Mahjong.Plugin.Player
{
    public abstract class IPlayer
    {
        NetLog m_log;
        private NetClient m_client;
        String m_login;

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
        private delegate bool GetMessage(String[] tab, NetMessage msg, ref String DiconnectionReason);
        GetMessages[] OpCodes;

        private void Init()
        {
            OpCodes = new GetMessages[]
            { 
                //Login
                new GetMessages("LOG", new GetMessage(GetMessageLOG)), 
                //Room ADD JOIN LEAVE
                new GetMessages("ROM", new GetMessage(GetMessageROM)), 
                //StartGame
                new GetMessages("STR", new GetMessage(GetMessageSTR)), 
                //Receive Tile
                new GetMessages("TIL", new GetMessage(GetMessageTIL)), 
                //Receive Tile
                new GetMessages("TRN", new GetMessage(GetMessageTRN)), 
            };
        }

        public IPlayer(String dns, int port)
        {
            NetAppConfiguration myConfig = new NetAppConfiguration("MMO Mahjong", port);

            m_log = new NetLog();
            Init();
            m_client = new NetClient(myConfig, m_log);
            m_client.Connect(dns, port);
            m_client.StatusChanged += new EventHandler<NetStatusEventArgs>(StatusChanged);
            Application.Idle += new EventHandler(ApplicationLoop);
        }

        private void ApplicationLoop(object sender, EventArgs e)
        {
            while (Win32.AppStillIdle)
            {
                // call heartbeat as often as possible; this sends queued packets,
                // keepalives and acknowledges etc.
                m_client.Heartbeat();

                NetMessage msg;

                // read a packet if available
                while ((msg = m_client.ReadMessage()) != null)
                    HandleMessage(msg);

                Thread.Sleep(1); // don't hog the cpu
            }
        }
        
        private void StatusChanged(object sender, NetStatusEventArgs e)
        {
           m_log.Info(e.Connection + ": " + e.Connection.Status + " - " + e.Reason);
        }

        private void HandleMessage(NetMessage msg)
        {
            string str = msg.ReadString();
            string[] tab = str.Split(':');

            for (int i = 0; i < OpCodes.Length; i++)
            {
                if (tab[0] == OpCodes[i].OpCode)
                {
                    String Disconnect = "";
                    m_log.Info(OpCodes[i].OpCode);
                    if (OpCodes[i].Funct(tab, msg, ref Disconnect) == false)
                        msg.Sender.Disconnect(Disconnect);
                }
            }
            m_log.Info(str);
        }

        private bool Send(String msg)
        {
            NetMessage outMsg = new NetMessage();
            outMsg.Write(msg);
            return m_client.SendMessage(outMsg, NetChannel.Unreliable);
        }

        public bool Connect(String login, String pass)
        {
            m_login = login;
            return Send("LOG:" + login + ":" + pass);
        }

        public bool CreateRoom(String name)
        {
            return Send("ROM:ADD:" + name);
        }

        public bool JoinRoom(String name)
        {
            return Send("ROM:JOIN:" + name);
        }

        public bool LeaveRoom()
        {
            return Send("ROM:LEAVE");
        }

        public bool SelectReferee(String name)
        {
            return Send("ROM:REFEREE:" + name);
        }

        public bool StartGame()
        {
            return Send("ROM:START");
        }

        public bool TakeTile()
        {
            return Send("TIL:TAKE");
        }

        public bool RemoveTile(String family, int number)
        {
            return Send("TIL:REMOVE:" + family + ":" + Convert.ToInt32(number));
        }
        

        private bool GetMessageLOG(String[] tab, NetMessage msg, ref String DiconnectionReason)
        {
            if (tab[1] == "OK")
                OnLoginOK();
            if (tab[1] == "KO")
                OnLoginKO();
            return true;
        }

        private bool GetMessageROM(String[] tab, NetMessage msg, ref String DiconnectionReason)
        {
            if (tab[1] == "JOIN")
            {
                OnRoomJoin();
            }
            if (tab[1] == "LEAVE")
            {
                OnRoomLeave();
            }
            if (tab[1] == "LST")
            {
                OnRoomList(tab[2]);
            }
            if (tab[1] == "ADD")
            {
                if (tab[2] == "OK")
                    OnRoomAddOK();
                if (tab[2] == "KO")
                    OnRoomAddKO();
            }
            if (tab[1] == "PLAYER")
            {
                if (tab[2] == "ADD")
                    OnRoomPlayerAdd(tab[3]);
                if (tab[2] == "LEAVE")
                    OnRoomPlayerLeave(tab[3]);
            }
            if (tab[1] == "REFEREE")
            {
                if (tab[2] == "ADD")
                    OnRoomRefereeAdd(tab[3]);
                if (tab[2] == "SELECTED")
                    OnRoomRefereeSelected(tab[3]);
            }
            return true;
        }

        private bool GetMessageSTR(String[] tab, NetMessage msg, ref String DiconnectionReason)
        {
            // String Hoster / Turn number
            OnStartGame("toto", 0);

            return true;
        }

        private bool GetMessageTIL(String[] tab, NetMessage msg, ref String DiconnectionReason)
        {
            if (tab[1] == "GET")
            {
                // String Hoster / Turn number
                OnGetTile(tab[2], Convert.ToInt32(tab[3]));
            }

            if (tab[1] == "REMOVE")
            {
                if (tab[2] != "KO")
                    OnRemoveTile(tab[2], Convert.ToInt32(tab[3]));
            }

            if (tab[1] == "REMOVED")
            {
                OnRemovedTile(tab[2],tab[3], Convert.ToInt32(tab[4]));
            }
            return true;
        }

        private bool GetMessageTRN(String[] tab, NetMessage msg, ref String DiconnectionReason)
        {
            // String Hoster / Turn number
            if (tab[1] == m_login)
                OnMyTurn();
            else
                OnTurn();
            
            return true;
        }

        protected abstract bool OnMessage(String name, String msg);
        protected abstract bool OnLoginOK();
        protected abstract bool OnLoginKO();
        protected abstract bool OnRoomAddOK();
        protected abstract bool OnRoomAddKO();
        protected abstract bool OnRoomList(String name);
        protected abstract bool OnRoomJoin();
        protected abstract bool OnRoomLeave();
        protected abstract bool OnRoomPlayerAdd(String name);
        protected abstract bool OnRoomPlayerLeave(String name);
        protected abstract bool OnRoomRefereeAdd(String name);
        protected abstract bool OnRoomRefereeSelected(String name);
        protected abstract bool OnStartGame(String name, int turn);
        protected abstract bool OnGetTile(String family, int number);
        protected abstract bool OnRemoveTile(String family, int number);
        protected abstract bool OnRemovedTile(String name, String family, int number);
        protected abstract bool OnMyTurn();
        protected abstract bool OnTurn();

    }
}
