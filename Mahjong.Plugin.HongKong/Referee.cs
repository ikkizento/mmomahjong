using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Plugin;
using Mahjong.Referee.HongKong;
using Mahjong.Core;


namespace Mahjong.Referee.HongKong
{
    public class Referee : IReferee
    {
        public Referee()
            : base()
        {
            m_rules.Add(new Pong());
        }

        public override String GetName()
        {
            return "HongKong";
        }

        public override String GetDescription()
        {
            return "lala toto";
        }

        public override Mahjong.Core.Player.Position HasWin()
        {
            return Mahjong.Core.Player.Position.Est;
        }

        public override List<m_rulepossibility> GetRulesPossibilities(Player player)
        {
            List<m_rulepossibility> ins = new List<m_rulepossibility>();

            for (int i = 0; i < m_rules.Count; i++)
            {
                List<m_rulepossibility> tmp = m_rules[i].Execute(m_players, player);
                for (int j = 0; j < tmp.Count; j++)
                    ins.Add(tmp[j]);
            }

            return ins;
        }
    }
}
