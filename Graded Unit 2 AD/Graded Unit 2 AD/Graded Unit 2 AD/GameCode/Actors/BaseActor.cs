using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GradedUnit2AD
{
    class BaseActor : AnimatedGraphic
    {
        protected Texture2D m_newTxr;
        protected Point m_posPoint;
        protected float m_speed;

        public BaseActor(Rectangle rect, Texture2D txr, Texture2D newTxr, int fps, int xFrames, int yFrames, Vector2 velo, float speed)
            : base(rect, txr, fps, xFrames, yFrames)
        {
            m_newTxr = newTxr;
            m_posPoint = new Point((int)(m_pos.X / 25), (int)(m_pos.Y / 25));
            m_speed = speed;
            m_velo = velo;
        }

        /// <summary>
        /// Animate the Actor
        /// </summary>
        /// <param name="gt">gameTime</param>
        public override void animateMe(GameTime gt)
        {
            if (m_velo.X == 0)
            m_trigger += (float)gt.ElapsedGameTime.TotalSeconds * m_fps * 0.25f;
            else if (m_velo.X > 0)
            m_trigger += (float)gt.ElapsedGameTime.TotalSeconds * m_fps * m_velo.X;
            else
            m_trigger += (float)gt.ElapsedGameTime.TotalSeconds * m_fps * -m_velo.X;

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
    }
}
