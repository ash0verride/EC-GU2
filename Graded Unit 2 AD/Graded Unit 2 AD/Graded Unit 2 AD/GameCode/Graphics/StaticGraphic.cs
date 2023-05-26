using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GradedUnit2AD
{
    class StaticGraphic
    {
        protected Rectangle m_rect;
        protected Texture2D m_txr;

        public Rectangle Rect
        {
            get
            {
                return m_rect;
            }
        }

        public StaticGraphic(Rectangle rect, Texture2D txr)
        {
            m_rect = rect;
            m_txr = txr;
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="sb">spriteBatch</param>
        public virtual void drawMe(SpriteBatch sb)
        {
            sb.Draw(m_txr, m_rect, Color.White);
        }

        /// <summary>
        /// Fade the sprite out
        /// </summary>
        /// <param name="sb">spriteBatch</param>
        /// <param name="alpha">alpha</param>
        public void fadeMe(SpriteBatch sb, float alpha)
        {
            sb.Draw(m_txr, m_rect, Color.Black * alpha);
        }

        /// <summary>
        /// Hide the sprite
        /// </summary>
        /// <param name="sb">spriteBatch</param>
        /// <param name="alpha">alpha</param>
        public void hideMe(SpriteBatch sb, float alpha)
        {
            sb.Draw(m_txr, m_rect, Color.White * alpha);
        }
    }
}
