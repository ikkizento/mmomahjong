using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Core;

namespace Mahjong.Plugin
{
    public interface IRule
    {
        List<Mahjong.Plugin.IReferee.m_rulepossibility> Execute(List<Player> players, Player current);

        int GetScore();

        String GetName();

        String GetDescription();
    }
}
