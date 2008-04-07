using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong.Core
{
    public interface IRule
    {
        public bool Execute();

        public int score();
    }
}
