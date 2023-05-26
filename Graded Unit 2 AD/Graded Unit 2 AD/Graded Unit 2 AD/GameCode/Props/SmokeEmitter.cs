using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradedUnit2AD
{
    class SmokeEmitter
    {
        private Vector2 m_defaultPos;
        private List<Vector2> m_pos = new List<Vector2>();
        private List<Vector2> m_offsetPos = new List<Vector2>();
        private Vector2 m_velo;
        private Texture2D m_txr;

        private List<float> m_alpha = new List<float>();
        private List<float> m_scale = new List<float>();
        private float m_trigger;
 
        public SmokeEmitter(Vector2 position)
        {
            m_defaultPos = new Vector2(position.X, position.Y);
            m_velo = new Vector2(0, -0.75f);
            m_txr = Game1.smokeTxr;

            for (int i = 0; i < 20; i++)
            {
                m_pos.Add(new Vector2(position.X, position.Y - (7.5f * i)));
                m_alpha.Add(1f - (i * 0.05f));
                m_scale.Add(i * 0.1f);
                m_offsetPos.Add(new Vector2(m_pos[i].X - (m_txr.Width * (m_scale[i] / 2)), m_pos[i].Y));
            }

            m_trigger = 600;
        }

        /// <summary>
        /// Updates the smoke
        /// </summary>
        /// <param name="gt">Gametime</param>
        public void updateMe(GameTime gt)
        {
            if (m_trigger > 0)
                m_trigger -= 1 / (float)gt.ElapsedGameTime.TotalSeconds;
            else
            {
                for (int i = 0; i < m_alpha.Count; i++)
                {
                    m_alpha[i] -= 0.05f;
                    m_scale[i] += 0.1f;
                }
                m_trigger = 600;
            }

            for (int i = 0; i < m_alpha.Count; i++)
            {
                m_pos[i] += m_velo;
                m_offsetPos[i] = new Vector2(m_pos[i].X - (m_txr.Width * (m_scale[i] / 2)), m_pos[i].Y);

                if (m_alpha[i] <= 0)
                {
                    m_scale.RemoveAt(i);
                    m_pos.RemoveAt(i);
                    m_offsetPos.RemoveAt(i);
                    m_alpha.RemoveAt(i);

                    m_scale.Add(0);
                    m_alpha.Add(1);
                    m_pos.Add(m_defaultPos);
                    m_offsetPos.Add(m_defaultPos);
                }
            }
        }

        /// <summary>
        /// Draws the smoke
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        public void drawMe(SpriteBatch sb)
        {
            for (int i = 0; i < m_alpha.Count; i++)
            {
                sb.Draw(m_txr, m_offsetPos[i], null, Color.White * m_alpha[i], 0, Vector2.Zero, m_scale[i], SpriteEffects.None, 0);
            }
        }
    }
}
