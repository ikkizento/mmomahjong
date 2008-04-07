using System;
using System.Collections.Generic;
using System.Text;

namespace Mahjong.Core
{
    public class Player
    {
        public enum Position
        {
            North,
            South,
            West,
            Est
        }
        private Group m_hand;
        private Group m_cemetery;
        private Group m_exposed;
        private Tile m_rejected;
        private Position m_position;

        public Group GetHand()
        {
            return m_hand;
        }

        public bool AddHand(Tile add)
        {
            m_hand.AddTile(add);
            return true;
        }

        public Group GetCemetery()
        {
            return m_cemetery;
        }
        public bool AddCemetery(Tile add)
        {
            m_cemetery.AddTile(add);
            return true;
        }
        public Group GetExposed()
        {
            return m_exposed;
        }
        public bool AddExposed(Tile add)
        {
            m_exposed.AddTile(add);
            return true;
        }
        public Tile GetRejected()
        {
            return m_rejected;
        }
        public bool AddRejected(Tile add)
        {
            m_rejected = add;
            return true;
        }
        public Position GetPosition()
        {
            return m_position;
        }

        public void Reset()
        {
            m_hand = new Group();
            m_cemetery = new Group();
            m_exposed = new Group();
            m_rejected = null;
        }

    }
}

