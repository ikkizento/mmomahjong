using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mahjong.Client
{
    public partial class RoomListForm : Form
    {
        public RoomListForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MakeForm f = new MakeForm();
            f.ShowDialog();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            NetWork.Player.JoinRoom(listBox1.Items[listBox1.SelectedIndex].ToString());
        }
    }
}