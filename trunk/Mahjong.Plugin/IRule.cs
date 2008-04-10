using System;
using System.Collections.Generic;
using System.Text;
using Mahjong.Core;

namespace Mahjong.Plugin
{
    public interface IRule
    {
        List<Mahjong.Plugin.IReferee.m_rulepossibility> Execute(List<PlayerData> players, PlayerData current);

        int GetScore();

        String GetName();

        String GetDescription();
    }
}
