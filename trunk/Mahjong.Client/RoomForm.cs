using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mahjong.Client
{
    public partial class RoomForm : Form
    {
        public RoomForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NetWork.Player.LeaveRoom();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            NetWork.Player.SelectReferee(comboBox1.Text);
        }

        private void RoomForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetWork.Player.StartGame();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NetWork.Player.TakeTile();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string[] tab = listBox2.Items[listBox2.SelectedIndex].ToString().Split(' ');
            NetWork.Player.RemoveTile(tab[0], Convert.ToInt32(tab[1]));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] tab = listBox4.Items[listBox4.SelectedIndex].ToString().Split(' ');
            Mahjong.Plugin.Player.IPlayer.RuleGroup tmp = new Mahjong.Plugin.Player.IPlayer.RuleGroup();
            tmp.Group = new List<Mahjong.Plugin.Player.IPlayer.NetTile>();
            tmp.name = tab[0];
            for (int i = 1; i < tab.Length; )
            {
                Mahjong.Plugin.Player.IPlayer.NetTile ins = new Mahjong.Plugin.Player.IPlayer.NetTile();
                ins.family = tab[i];
                ins.number = Convert.ToInt32(tab[i + 1]);
                tmp.Group.Add(ins);
                i += 2;
            }

            NetWork.Player.Call(tmp);
        }

        private void RoomForm_Leave(object sender, EventArgs e)
        {
            NetWork.Player.LeaveRoom();
        }

        private void RoomForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            NetWork.Player.LeaveRoom();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NetWork.Player.Disconnect();
            Application.Exit();
        }
    }
}