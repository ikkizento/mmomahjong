using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Plugin;


namespace Mahjong.Plugin.HongKong
{
    public class Referee : IReferee
    {
        public Referee()
            : base()
        {

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

    }
}
