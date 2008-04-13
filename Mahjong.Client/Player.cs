using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Plugin.Player;
using System.Windows.Forms;

namespace Mahjong.Client
{
    public class NetWork
    {
        public static Player Player;
    }

    public class Player : IPlayer
    {
        RoomListForm m_roomlistform;
        RoomForm m_roomform;

        public Player(String dns, int port)
            : base(dns, port)
        {
        }

        protected override bool OnLoginOK()
        {
            m_roomlistform = new RoomListForm();

            m_roomlistform.ShowDialog();
            return true;
        }

        protected override bool OnLoginKO()
        {
            MessageBox.Show("Invalide login or password");
            return true;
        }

        protected override bool OnRoomAddKO()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool OnRoomAddOK()
        {
            return true;
        }

        protected override bool OnRoomList(string name)
        {
            m_roomlistform.listBox1.Items.Add(name);

            return true;
        }

        protected override bool  OnMessage(string name, string msg)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool OnRoomJoin()
        {
            m_roomform = new RoomForm();

            m_roomform.ShowDialog();
            return true;
        }

        protected override bool OnRoomLeave()
        {
            m_roomform.Close();
            return true;
        }

        protected override bool OnRoomPlayerAdd(string name)
        {
            m_roomform.listBox1.Items.Add(name);
            return true;
        }

        protected override bool OnRoomPlayerLeave(string name)
        {
            m_roomform.listBox1.Items.Remove(name);
            return true;
        }

        protected override bool OnRoomRefereeAdd(string name)
        {
            m_roomform.comboBox1.Items.Add(name);
            return true;
        }

        protected override bool OnRoomRefereeSelected(string name)
        {
            if (m_roomform != null)
            {
                m_roomform.comboBox1.Text = name;
            }
            return true;
        }

        protected override bool OnStartGame(string name, int turn)
        {
            return true;
        }

        protected override bool OnGetTile(string family, int number)
        {
            m_roomform.listBox2.Items.Add(family + " " + number.ToString());
            m_roomform.listBox2.Sorted = true;
            m_roomform.listBox4.Items.Clear();
            return true;
        }

        protected override bool OnRemoveTile(string family, int number)
        {
            m_roomform.listBox2.Items.Remove(family + " " + number.ToString());

            return true;
        }

        //protected override bool OnRejectedTile(string family, int number)
        //{
        //    m_roomform.label1.Text = family + " " + number.ToString();
        //    return true;
        //}

        protected override bool OnRemovedTile(string name, string family, int number)
        {
            m_roomform.label1.Text = family + " " + number.ToString();
            return true;
        }
        protected override bool OnMyTurn()
        {
            MessageBox.Show("My turn");
            return true;
        }

        protected override bool OnTurn()
        {
            return true;
        }

        protected override bool OnGetPosibility(RuleGroup rulegroup)
        {
            String tmp;

            tmp = rulegroup.name;
            for (int i = 0; i < rulegroup.Group.Count; i++)
            {
                tmp += " " + rulegroup.Group[i].family + " " + rulegroup.Group[i].number.ToString();
            }
            m_roomform.listBox4.Items.Add(tmp);

            return true;
        }

        protected override bool OnRoomMeLeave()
        {
            m_roomform.Close();

            return true;
        }
    }


}
