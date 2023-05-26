using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GradedUnit2AD
{
    class MotionGraphic : StaticGraphic
    {
        protected Vector2 m_pos;
        protected Vector2 m_velo;

        public MotionGraphic(Rectangle rect, Texture2D txr)
            : base(rect, txr)
        {
            m_pos = new Vector2(rect.X, rect.Y);
            m_velo = Vector2.Zero;
        }

        /// <summary>
        /// update the sprites position
        /// </summary>
        /// <param name="pos">Position</param>
        public virtual void updateMe(Vector2 pos)
        {
            m_pos = pos;
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="sb">spriteBatch</param>
        public virtual void drawMe(SpriteBatch sb)
        {
            sb.Draw(m_txr, m_pos, Color.White);
        }
    }
}
