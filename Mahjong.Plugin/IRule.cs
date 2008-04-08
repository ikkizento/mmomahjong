using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Core;

namespace Mahjong.Plugin
{
    public interface IRule
    {
        bool Execute(List<Player> players);

        int score();

        String GetName();

        String GetDescription();
    }
}
