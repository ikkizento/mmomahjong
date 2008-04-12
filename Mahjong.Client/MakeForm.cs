using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mahjong.Client
{
    public partial class MakeForm : Form
    {
        public MakeForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetWork.Player.CreateRoom(textBox1.Text);
            NetWork.Player.JoinRoom(textBox1.Text);
        }
    }
}