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
            m_rules.Add(new Mahjongg());
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
                            rulepos.Player.RemoveHand(ins[i].Group[j]);
                        
                        Tile rejtile = GetRejectTile();
                        if (rejtile != null)
                            ins[i].Group.Add(rejtile);
                        rulepos.Player.AddExposed(ins[i].Group);
                        m_current = rulepos.Player;
                        if (rejtile != null)
                        {
                            GetRejectPlayer().AddRejected(null);
                            ChangeTileStatus(rejtile, TilePosition.Cemetery);
                        }
                        m_mutextaken = true;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
