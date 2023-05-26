using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradedUnit2AD
{
    class LargeBackdrops : MotionGraphic
    {
        private bool m_freezeHorizontal;
        private bool m_freezeVertical;

        public LargeBackdrops(Rectangle rect, Texture2D txr, bool horizontal, bool vertical)
            : base (rect, txr)
        {
            m_freezeHorizontal = horizontal;
            m_freezeVertical = vertical;
        }

        /// <summary>
        /// Update the background based on the cameras position
        /// </summary>
        /// <param name="camPos">Camera Position</param>
        public void updateMe(Vector2 camPos)
        {
            if (m_freezeHorizontal)
                m_pos.X = camPos.X;

            if (m_freezeVertical)
                m_pos.Y = camPos.Y;
        }
    }
}
