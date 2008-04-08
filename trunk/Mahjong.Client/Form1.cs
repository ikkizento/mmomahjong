using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mahjong.Plugin;
using Mahjong.Core;

namespace Mahjong.Client
{
    public partial class Form1 : Form
    {
        IReferee refe;
        Player p1;
        Player p2;
        Player p3;
        Player p4;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Plugin p = new Plugin(Environment.CurrentDirectory);
            refe = p.GetReferees()[0];
            p1 = new Player("Player 1");
            refe.AddPlayer(p1);
            p2 = new Player("Player 2");
            refe.AddPlayer(p2);
            p3 = new Player("Player 3");
            refe.AddPlayer(p3);
            p4 = new Player("Player 4");
            refe.AddPlayer(p4);
            refe.NewGame();
            Draw();
        }

        void Draw()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            Group g1 = p1.GetHand();
            Group g2 = p2.GetHand();
            Group g3 = p3.GetHand();
            Group g4 = p4.GetHand();
            for (int i = 0; i < g1.Count(); i++)
                listBox1.Items.Add(g1.GetTile(i).GetNumber().ToString() + " " + g1.GetTile(i).GetFamily().ToString());
            for (int i = 0; i < g2.Count(); i++)
                listBox2.Items.Add(g2.GetTile(i).GetNumber().ToString() + " " + g2.GetTile(i).GetFamily().ToString());
            for (int i = 0; i < g3.Count(); i++)
                listBox3.Items.Add(g3.GetTile(i).GetNumber().ToString() + " " + g3.GetTile(i).GetFamily().ToString());
            for (int i = 0; i < g4.Count(); i++)
                listBox4.Items.Add(g4.GetTile(i).GetNumber().ToString() + " " + g4.GetTile(i).GetFamily().ToString());

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text += refe.CurrentPlayer().GetName() + " Reject \r\n";
            if (refe.Rejected(refe.CurrentPlayer().GetHand().GetTile(0)))
            {
                Draw();
                textBox1.Text += refe.CurrentPlayer().GetName() + " Play Free " + refe.GetNumberFreeTile().ToString() + "\r\n";
            }
        }
    }
}