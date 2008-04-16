using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mahjong.Core;

namespace Mahjong.Client.Test
{
    public partial class Form1 : Form
    {
        Referee.HongKong.Referee refe;
        PlayerData p1;
        PlayerData p2;
        Group p1Hand;
        Group p2Hand;
        List<Mahjong.Plugin.IReferee.m_rulepossibility> p1Possi;
        List<Mahjong.Plugin.IReferee.m_rulepossibility> p2Possi;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            p1 = new PlayerData("Player 1");
            p2 = new PlayerData("Player 2");
            refe = new Mahjong.Referee.HongKong.Referee();
            
            refe.AddPlayer(p1);
            refe.AddPlayer(p2);
            refe.NewGame();
            Draw();
        }

        private void Draw()
        {
            Draw1();
            Draw2();
        }

        private void Draw1()
        {
            listBox1.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox2.Items.Clear();
            Group g = p1.GetHand();
            p1Hand = g;
            for (int i = 0; i < g.Count; i++)
                listBox1.Items.Add(g[i].GetFamily() + " " + g[i].GetNumber().ToString());
            g = p1.GetCemetery();
            for (int i = 0; i < g.Count; i++)
                listBox3.Items.Add(g[i].GetFamily() + " " + g[i].GetNumber().ToString());
            List<Group> lg = p1.GetExposed();
            for (int j = 0; j < lg.Count; j++)
            {
                for (int i = 0; i < lg[j].Count; i++)
                    listBox4.Items.Add(lg[j][i].GetFamily() + " " + lg[j][i].GetNumber().ToString());
                listBox4.Items.Add("-----");
            }
            List<Mahjong.Plugin.IReferee.m_rulepossibility> possi = refe.GetRulesPossibilities(p1);
            p1Possi = possi;
            for (int j = 0; j < possi.Count; j++)
            {
                String tmp = possi[j].Rule.GetName();
                for (int i = 0; i < possi[j].Group.Count; i++)
                    tmp += " " + possi[j].Group[i].GetFamily() + " " + possi[j].Group[i].GetNumber().ToString();
                listBox2.Items.Add(tmp);
            }
            Tile t = p1.GetRejected();
            if (t != null)
                label4.Text = t.GetFamily() + " " + t.GetNumber().ToString();
            else
                label4.Text = "NULL";
            //listBox1.Sorted = true;
        }
        
        private void Draw2()
        {
            listBox8.Items.Clear();
            listBox7.Items.Clear();
            listBox6.Items.Clear();
            listBox5.Items.Clear();
            Group g = p2.GetHand();
            p2Hand = g;
            for (int i = 0; i < g.Count; i++)
                listBox8.Items.Add(g[i].GetFamily() + " " + g[i].GetNumber().ToString());
            g = p2.GetCemetery();
            for (int i = 0; i < g.Count; i++)
                listBox7.Items.Add(g[i].GetFamily() + " " + g[i].GetNumber().ToString());
            
            List<Group> lg = p2.GetExposed();
            for (int j = 0; j < lg.Count; j++)
            {
                for (int i = 0; i < lg[j].Count; i++)
                    listBox6.Items.Add(lg[j][i].GetFamily() + " " + lg[j][i].GetNumber().ToString());
                listBox6.Items.Add("-----");
            }
            List<Mahjong.Plugin.IReferee.m_rulepossibility> possi = refe.GetRulesPossibilities(p2);
            p2Possi = possi;
            for (int j = 0; j < possi.Count; j++)
            {
                String tmp = possi[j].Rule.GetName();
                for (int i = 0; i < possi[j].Group.Count; i++)
                    tmp += " " + possi[j].Group[i].GetFamily() + " " + possi[j].Group[i].GetNumber().ToString();
                listBox5.Items.Add(tmp);
            }
            Tile t = p2.GetRejected();
            if (t != null)
                label7.Text = t.GetFamily() + " " + t.GetNumber().ToString();
            else
                label7.Text = "NULL";
            //listBox8.Sorted = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            refe.Take();
            Draw();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            refe.Rejected(p1Hand[listBox1.SelectedIndex]);
            Draw();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            refe.Take();
            Draw();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            refe.Rejected(p2Hand[listBox8.SelectedIndex]);
            Draw();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            refe.Call(p2Possi[listBox5.SelectedIndex]);
            Draw();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            refe.Call(p1Possi[listBox2.SelectedIndex]);
            Draw();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Draw();
        }
    }
}