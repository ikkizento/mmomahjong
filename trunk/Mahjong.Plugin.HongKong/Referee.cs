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
            m_rules.Add(new Kong());
            m_rules.Add(new Chow());
        }

        public override String GetName()
        {
            return "Hong Kong";
        }

        public override String GetDescription()
        {
            return "lala toto";
        }

        public override int GetMaxPlayer()
        {
            return 4;
        }

        public override bool Call(m_rulepossibility rulepos)
        {
            List<m_rulepossibility> ins = rulepos.Rule.Execute(m_players, rulepos.Player);
            if (ins.Count > 0)
            {
                for (int i = 0; i < ins.Count; i++)
                {
                    if (ins[i].Group.Equal(rulepos.Group))
                    {
                        for (int j = 0; j < ins[i].Group.Count; j++)
                        {
                            rulepos.Player.AddExposed(ins[i].Group[j]);
                            rulepos.Player.RemoveHand(ins[i].Group[j]);
                        }
                        m_current = rulepos.Player;

                        //Take();
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
