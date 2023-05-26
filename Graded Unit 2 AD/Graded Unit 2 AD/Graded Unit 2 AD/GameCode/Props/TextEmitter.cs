using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GradedUnit2AD
{
    class TextEmitter
    {
        private Vector2 m_pos;
        private Vector2 m_velo;
        private Vector2 m_stringLength;
        private Vector2 m_offsetPos;

        private Color m_color;
        private SpriteFont m_font;
        private string m_message;

        private float m_alpha;
        private float m_scale;

        public float Alpha
        {
            get
            {
                return m_alpha;
            }
        }
 
        public TextEmitter(Vector2 position, string message, Color faction)
        {
            m_pos = new Vector2(position.X, position.Y);
            m_velo = new Vector2(0, -0.5f);
            m_alpha = 1.5f;
            m_scale = 0.1f;

            m_font = Game1.npcFont;
            m_message = message;
            m_color = faction;
            m_stringLength = m_font.MeasureString(m_message);
        }

        public void updateMe(GameTime gt)
        {
            m_alpha -= 0.25f * (float)gt.ElapsedGameTime.TotalSeconds;
            m_scale += 0.5f * (float)gt.ElapsedGameTime.TotalSeconds;

            m_pos += m_velo;

            m_offsetPos = new Vector2(m_pos.X - (m_stringLength.X * (m_scale / 2)), m_pos.Y);
        }

        public void drawMe(SpriteBatch sb)
        {
            sb.DrawString(m_font, m_message, m_offsetPos, m_color * m_alpha, 0, Vector2.Zero, m_scale, SpriteEffects.None, 0);
        }
    }
}
