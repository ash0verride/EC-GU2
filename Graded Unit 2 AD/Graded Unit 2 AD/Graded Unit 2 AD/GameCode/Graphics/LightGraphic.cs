using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GradedUnit2AD
{
    class LightGraphic : MotionGraphic
    {
        private Vector2 m_offset;
        private Texture2D m_aura;
        private float m_flicker;

        public float Flicker
        {
            get
            {
                return m_flicker;
            }
            set
            {
                m_flicker = value;
            }
        }

        public LightGraphic(Rectangle rect, Texture2D txr, Texture2D light)
            : base(rect, txr)
        {
            m_aura = light;

            m_offset = new Vector2(m_rect.Center.X, m_rect.Center.Y);
            m_flicker = 5;
        }

        /// <summary>
        /// Update the lamps position
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="gt">GameTime</param>
        public void updateMe(PlayerActor player, GameTime gt)
        {
            if (player.Equipped)
            {
                m_pos.X = player.Rect.Center.X;
                m_pos.Y = player.Rect.Y;

                if (m_flicker > 0)
                m_flicker -= (float)gt.ElapsedGameTime.TotalSeconds;
            }
            else if (m_flicker < 5)
            {
                m_flicker += (float)gt.ElapsedGameTime.TotalSeconds * 1.1f;
            }

            if (player.Velo.X < 0)
                base.updateMe(new Vector2(player.Rect.Center.X - 6, player.Rect.Center.Y + 14));
            else
                base.updateMe(new Vector2(player.Rect.Center.X + 6, player.Rect.Center.Y + 14));
        }

        public override void drawMe(SpriteBatch sb)
        {
            if (m_flicker < 5)
            base.drawMe(sb);
        }

        public void drawMask(SpriteBatch sb, PlayerActor player)
        {
            if (player.Equipped && m_flicker > 0)
            {
                if ((m_flicker > Game1.RNG.Next(0, 1)) && (m_flicker < Game1.RNG.Next(0, 3)))
                {
                    Game1.sfxPlayerLamp.CreateInstance().Play();
                }
                else
                    sb.Draw(m_aura, m_pos, null, Color.White, 0, m_offset, 1, SpriteEffects.None, 0);
            }
        }
    }
}
