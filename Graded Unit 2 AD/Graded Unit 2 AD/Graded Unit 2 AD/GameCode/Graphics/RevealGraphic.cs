using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradedUnit2AD
{
    class RevealGraphic : StaticGraphic
    {
        // Class Variables
        private int m_maxValue;
        private float m_unit;

        /// <summary>
        /// Sets the bars variables
        /// </summary>
        /// <param name="xPos">X Position</param>
        /// <param name="yPos">Y Position</param>
        /// <param name="txr">Texture used</param>
        /// <param name="maxValue">The maximum value the bar can be</param>
        /// <param name="tint">The value for the alpha</param>
        /// <param name="inverted">A boolean for if the bar has to be upside down</param>
        public RevealGraphic(Rectangle rect, Texture2D txr, int maxValue)
            : base(rect, txr)
        {
            // Sets the max value
            m_maxValue = maxValue;
            // Creates a percentage of what the total Width is per unit
            m_unit = m_rect.Width / m_maxValue;
        }

        /// <summary>
        /// Draws the Revealed part of the image
        /// </summary>
        /// <param name="sb">The spritebatch used</param>
        /// <param name="value">The units that the bar must display</param>
        public void drawMe(SpriteBatch sb, float value, float alpha)
        {
            sb.Draw(m_txr, new Rectangle(m_rect.X, m_rect.Y, (int)(m_unit * value), m_rect.Height), new Rectangle(0, 0, (int)(m_unit * value), m_rect.Height), Color.White * alpha);
        }
    }
}
