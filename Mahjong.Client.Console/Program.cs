using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Library.Network;
using System.Threading;

namespace Mahjong.Client.Console
{
    class Program
    {
        public static NetClient Client;
 
        
        static void Main(string[] args)
        {
            Program p = new Program();
        }

        public Program()
        {
            NetAppConfiguration myConfig = new NetAppConfiguration("MMO Mahjong", 14242);

            NetLog log = new NetLog();

            NetClient Client = new NetClient(myConfig, log);
            Client.Connect("localhost", 14242); // takes IP number or hostnameHow to detect connects/disconnects
            Client.StatusChanged += new EventHandler<NetStatusEventArgs>(StatusChanged);// to track connect/disconnect etc

            bool keepGoing = true;
            while (keepGoing)
            {
                Client.Heartbeat();

                NetMessage msg;
                while ((msg = Client.ReadMessage()) != null)
                {
                    string str = msg.ReadString(); // <- for example
                    System.Console.WriteLine("You got a packet containing: " + str);
                    Thread.Sleep(1);
                }
            }
        }

        void StatusChanged(object sender, NetStatusEventArgs e)
        {
            // e.Connection is the connection for which status changed
            // e.Connectionn.Status is the new status
            // e.Reason is a human readable reason for the status change
            if (e.Connection.Status == NetConnectionStatus.Connected)
            {
                System.Console.WriteLine("Status: " + e.Connection.Status.ToString());
                NetMessage outMsg = new NetMessage();
                //Client.SendMessage(outMsg, NetChannel.ReliableUnordered); // <- for example
            }
        }

    }
}
