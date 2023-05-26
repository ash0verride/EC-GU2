using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradedUnit2AD
{
    [Serializable]
    public struct GameSave
    {
        private int m_level;
        private bool m_lamp;
        private bool m_jacket;
        private bool m_hat;

        public int Level
        {
            get
            {
                return m_level;
            }
            set
            {
                m_level = value;
            }
        }

        public bool Lamp
        {
            get
            {
                return m_lamp;
            }
            set
            {
                m_lamp = value;
            }
        }

        public bool Jacket
        {
            get
            {
                return m_jacket;
            }
            set
            {
                m_jacket = value;
            }
        }

        public bool Hat
        {
            get
            {
                return m_hat;
            }
            set
            {
                m_hat = value;
            }
        }

        /// <summary>
        /// Stores the games progression
        /// </summary>
        /// <param name="level"></param>
        public GameSave(int level)
        {
            m_level = level;

            m_lamp = false;
            m_jacket = false;
            m_hat = false;
        }
    }
}
