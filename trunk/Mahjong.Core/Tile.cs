using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong.Core
{
    public class Tile
    {
        public enum Family
        {
            Circle,
            Character,
            Bamboo,
            Wind,
            Dragon,
            Flower,
            Season
        };

        private int m_number;

        private Family m_family;

        public Tile(Family family, int number)
        {
            m_family = family;
            m_number = number;
        }

        public Family GetFamily()
        {
            return m_family;
        }

        public int GetNumber()
        {
            return m_number;
        }
    }
}
