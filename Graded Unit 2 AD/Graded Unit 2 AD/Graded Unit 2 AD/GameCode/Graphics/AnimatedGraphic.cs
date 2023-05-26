using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GradedUnit2AD
{
    class AnimatedGraphic : MotionGraphic
    {
        protected Rectangle m_srcRect;
        protected float m_trigger;
        protected float m_fps;
        protected float m_starSpeed;

        public AnimatedGraphic(Rectangle rect, Texture2D txr, int fps, int xFrames, int yFrames)
            : base(rect, txr)
        {
            m_srcRect = new Rectangle(0,0, m_txr.Width / xFrames, m_txr.Height / yFrames);
            m_trigger = 0;
            m_fps = fps;

            m_starSpeed = Game1.RNG.Next(1, 100);
            m_starSpeed = m_starSpeed / 100;
        }

        /// <summary>
        /// Animate the sprites
        /// </summary>
        /// <param name="gt">gameTime</param>
        public virtual void animateMe(GameTime gt)
        {
            m_trigger += (float)gt.ElapsedGameTime.TotalSeconds * m_fps;

            if (m_trigger >= 1)
            {
                m_srcRect.X += m_srcRect.Width;

                if (m_srcRect.X >= m_txr.Width)
                {
                    m_srcRect.X = 0;
                    m_srcRect.Y += m_srcRect.Height;

                    if (m_srcRect.Y >= m_txr.Height)
                        m_srcRect.Y = 0;
                }

                m_trigger = 0;
            }

            m_pos += m_velo;
        }

        /// <summary>
        /// Draw the animation
        /// </summary>
        /// <param name="sb">spriteBatch</param>
        public virtual void drawMe(SpriteBatch sb)
        {
            sb.Draw(m_txr, m_pos, m_srcRect, Color.White);
        }
    }
}
