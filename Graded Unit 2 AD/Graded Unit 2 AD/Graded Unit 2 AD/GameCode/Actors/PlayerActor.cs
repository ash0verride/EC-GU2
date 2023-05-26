using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GradedUnit2AD
{
    class PlayerActor : BaseActor
    {
        Texture2D m_txr2;
        Texture2D m_newTxr2;
        Texture2D m_txr3;
        Texture2D m_newTxr3;
        Texture2D m_txr4;
        Texture2D m_newTxr4;

        private bool m_jacket;
        private bool m_hat;
        private bool m_equipped;

        public Vector2 Pos
        {
            get
            {
                return m_pos;
            }
            set
            {
                m_pos = value;
            }
        }

        public Vector2 Velo
        {
            get
            {
                return m_velo;
            }
            set
            {
                m_velo = value;
            }
        }

        public Point PosPoint
        {
            get
            {
                return m_posPoint;
            }
        }

        public bool Equipped
        {
            get
            {
                return m_equipped;
            }
        }

        public PlayerActor(Rectangle rect, Texture2D txr, Texture2D newTxr, Texture2D txr2, Texture2D newTxr2, Texture2D txr3, Texture2D newTxr3, Texture2D txr4, Texture2D newTxr4, int fps, int xFrames, int yFrames, Vector2 velo, float speed)
            : base(rect, txr, newTxr, fps, xFrames, yFrames, velo, speed)
        {
            m_txr2 = txr2;
            m_newTxr2 = newTxr2;
            m_txr3 = txr3;
            m_newTxr3 = newTxr3;
            m_txr4 = txr4;
            m_newTxr4 = newTxr4;
        }

        /// <summary>
        /// Update the player
        /// </summary>
        /// <param name="currKb">Keyboard controls</param>
        /// <param name="map">Current collision map</param>
        /// <param name="items">Player items</param>
        public void updateMe(KeyboardState currKb, MapGenerator map, List<InventoryItems> items)
        {
            m_jacket = false;
            m_hat = false;
            m_equipped = false;

            for (int i = 0; i < items.Count; i++)
            {
                if (m_jacket == false && items[i].Item == "Warm Jacket")
                {
                    m_jacket = true;
                }

                if (m_hat == false && items[i].Item == "Top Hat")
                {
                    m_hat = true;
                }

                if (items[i].Item == "Lamp" && ((currKb.IsKeyDown(Keys.I)) || (currKb.IsKeyDown(Keys.Q))) && m_pos.Y < 950)
                {
                    m_equipped = true;
                }
            }

            if ((currKb.IsKeyDown(Keys.D) && currKb.IsKeyUp(Keys.A)) || (currKb.IsKeyDown(Keys.Right) && currKb.IsKeyUp(Keys.Left)))
            {
                if (m_velo.X == 0)
                    m_velo.X = (m_speed / 30);
                if (m_velo.X < 0)
                    m_velo.X += (m_speed / 30);
                if (m_velo.X <= m_speed)
                    m_velo.X += (m_speed / 30);
            }
            if ((currKb.IsKeyDown(Keys.A) && currKb.IsKeyUp(Keys.D)) || (currKb.IsKeyDown(Keys.Left) && currKb.IsKeyUp(Keys.Right)))
            {
                if (m_velo.X == 0)
                    m_velo.X = (-m_speed / 30);
                if (m_velo.X > 0)
                    m_velo.X -= (m_speed / 30);
                if (m_velo.X >= -m_speed)
                    m_velo.X -= (m_speed / 30);
            }

            if ((currKb.IsKeyUp(Keys.A) && currKb.IsKeyUp(Keys.D) && currKb.IsKeyUp(Keys.Left) && currKb.IsKeyUp(Keys.Right)) || (currKb.IsKeyDown(Keys.A) && currKb.IsKeyDown(Keys.D) && currKb.IsKeyDown(Keys.Left) && currKb.IsKeyDown(Keys.Right)))
            {
                if (m_velo.X > 0)
                {
                    if (m_velo.X > (m_speed / 30))
                        m_velo.X -= (m_speed / 30);
                    else
                        m_velo.X = 0;
                }
                if (m_velo.X < 0)
                {
                    if (m_velo.X < (-m_speed / 30))
                        m_velo.X += (m_speed / 30);
                    else
                        m_velo.X = 0;
                }
            }

            if (m_velo.X > m_speed * 4)
                m_velo.X = m_speed * 4;
            if (m_velo.X < -m_speed * 4)
                m_velo.X = -m_speed * 4;
            if (m_velo.Y > m_speed * 4)
                m_velo.Y = m_speed * 4;
            if (m_velo.Y < -m_speed * 4)
                m_velo.Y = -m_speed * 4;

            m_pos += m_velo;

            m_posPoint = new Point((int)(m_pos.X / 25), (int)(m_pos.Y / 25));

            if (map.Cells.bumpCheck(m_posPoint, m_rect))
            {
                m_pos.Y = (m_posPoint.Y + 1) * 25;
                m_posPoint = new Point((int)(m_pos.X / 25), (int)(m_pos.Y / 25));
                m_velo.Y = 0;
            }

            if (m_velo.Y == 0)
            {
                if (m_velo.X > 0 && map.Cells.rightStepCheck(m_posPoint, m_rect))
                {
                    m_pos.Y = (m_posPoint.Y - 1) * 25;
                    m_pos.X = (m_posPoint.X + 0.5f) * 25;
                    m_posPoint = new Point((int)(m_pos.X / 25), (int)(m_pos.Y / 25));

                    if (m_velo.X > 0.5)
                        m_velo.X = 0.5f;
                }
                else if (m_velo.X < 0 && map.Cells.leftStepCheck(m_posPoint, m_rect))
                {
                    m_pos.Y = (m_posPoint.Y - 1) * 25;
                    m_pos.X = (m_posPoint.X + 0.5f) * 25;
                    m_posPoint = new Point((int)(m_pos.X / 25), (int)(m_pos.Y / 25));

                    if (m_velo.X < -0.5)
                        m_velo.X = -0.5f;
                }
            }
            
            if (map.Cells.rightWallCheck(m_posPoint, m_rect))
            {
                if (m_velo.X == 0 && map.Cells.rightStepExtraCheck(m_posPoint, m_rect))
                    m_pos.X = m_posPoint.X * 25 + 5;
                else
                    m_pos.X = m_posPoint.X * 25;

                m_posPoint = new Point((int)(m_pos.X / 25), (int)(m_pos.Y / 25));
                m_velo.X = 0;
            }
            else if (map.Cells.leftWallCheck(m_posPoint, m_rect))
            {
                if (m_velo.X == 0 && map.Cells.leftStepExtraCheck(m_posPoint, m_rect))
                    m_pos.X = (m_posPoint.X + 1) * 25 - 5;
                else
                    m_pos.X = (m_posPoint.X + 1) * 25;

                m_posPoint = new Point((int)(m_pos.X / 25), (int)(m_pos.Y / 25));
                m_velo.X = 0;
            }

            if ((m_velo.X < 0 && map.Cells.groundCheck(m_posPoint, m_rect)) || (m_velo.X > 0 && map.Cells.groundAltCheck(m_posPoint, m_rect)) || (m_velo.X == 0 && map.Cells.groundCheck(m_posPoint, m_rect) &&  map.Cells.groundAltCheck(m_posPoint, m_rect)))
            {
                m_pos.Y = m_posPoint.Y * 25;
                m_posPoint = new Point((int)(m_pos.X / 25), (int)(m_pos.Y / 25));
                m_velo.Y = 0;

                if (currKb.IsKeyDown(Keys.W) || currKb.IsKeyDown(Keys.Up))
                {
                    if (map.Cells.jumpCheck(m_posPoint, m_rect))
                        m_velo.Y = 0;
                    else
                        m_velo.Y = -m_speed * 2;
                }
            }
            else
                m_velo.Y += 0.3f;

            m_rect.X = (int)m_pos.X;
            m_rect.Y = (int)m_pos.Y;
        }

        /// <summary>
        /// Draw player
        /// </summary>
        /// <param name="sb">spriteBatch</param>
        public override void drawMe(SpriteBatch sb)
        {
            if (m_pos.Y > 950)
            {
                if (m_hat && m_jacket)
                {
                    if (m_velo.X < 0)
                    {
                        sb.Draw(m_txr4, m_pos, m_srcRect, Color.Aqua);
                    }

                    if (m_velo.X > 0)
                    {
                        sb.Draw(m_txr4, new Rectangle((int)m_pos.X, (int)m_pos.Y, m_srcRect.Width, m_srcRect.Height), m_srcRect, Color.Aqua, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }

                    if (m_velo.X == 0)
                        sb.Draw(m_newTxr4, m_pos, m_srcRect, Color.Aqua);
                }
                else if (m_hat)
                {
                    if (m_velo.X < 0)
                    {
                        sb.Draw(m_txr3, m_pos, m_srcRect, Color.Aqua);
                    }

                    if (m_velo.X > 0)
                    {
                        sb.Draw(m_txr3, new Rectangle((int)m_pos.X, (int)m_pos.Y, m_srcRect.Width, m_srcRect.Height), m_srcRect, Color.Aqua, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }

                    if (m_velo.X == 0)
                        sb.Draw(m_newTxr3, m_pos, m_srcRect, Color.Aqua);
                }
                else if (m_jacket)
                {
                    if (m_velo.X < 0)
                    {
                        sb.Draw(m_txr2, m_pos, m_srcRect, Color.Aqua);
                    }

                    if (m_velo.X > 0)
                    {
                        sb.Draw(m_txr2, new Rectangle((int)m_pos.X, (int)m_pos.Y, m_srcRect.Width, m_srcRect.Height), m_srcRect, Color.Aqua, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }

                    if (m_velo.X == 0)
                        sb.Draw(m_newTxr2, m_pos, m_srcRect, Color.Aqua);
                }
                else
                {
                    if (m_velo.X < 0)
                    {
                        sb.Draw(m_txr, m_pos, m_srcRect, Color.Aqua);
                    }

                    if (m_velo.X > 0)
                    {
                        sb.Draw(m_txr, new Rectangle((int)m_pos.X, (int)m_pos.Y, m_srcRect.Width, m_srcRect.Height), m_srcRect, Color.Aqua, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }

                    if (m_velo.X == 0)
                        sb.Draw(m_newTxr, m_pos, m_srcRect, Color.Aqua);
                }
            }
            else
            {
                if (m_hat && m_jacket)
                {
                    if (m_velo.X < 0)
                    {
                        sb.Draw(m_txr4, m_pos, m_srcRect, Color.White);
                    }

                    if (m_velo.X > 0)
                    {
                        sb.Draw(m_txr4, new Rectangle((int)m_pos.X, (int)m_pos.Y, m_srcRect.Width, m_srcRect.Height), m_srcRect, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }

                    if (m_velo.X == 0)
                        sb.Draw(m_newTxr4, m_pos, m_srcRect, Color.White);
                }
                else if (m_hat)
                {
                    if (m_velo.X < 0)
                    {
                        sb.Draw(m_txr3, m_pos, m_srcRect, Color.White);
                    }

                    if (m_velo.X > 0)
                    {
                        sb.Draw(m_txr3, new Rectangle((int)m_pos.X, (int)m_pos.Y, m_srcRect.Width, m_srcRect.Height), m_srcRect, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }

                    if (m_velo.X == 0)
                        sb.Draw(m_newTxr3, m_pos, m_srcRect, Color.White);
                }
                else if (m_jacket)
                {
                    if (m_velo.X < 0)
                    {
                        sb.Draw(m_txr2, m_pos, m_srcRect, Color.White);
                    }

                    if (m_velo.X > 0)
                    {
                        sb.Draw(m_txr2, new Rectangle((int)m_pos.X, (int)m_pos.Y, m_srcRect.Width, m_srcRect.Height), m_srcRect, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }

                    if (m_velo.X == 0)
                        sb.Draw(m_newTxr2, m_pos, m_srcRect, Color.White);
                }
                else
                {
                    if (m_velo.X < 0)
                    {
                        sb.Draw(m_txr, m_pos, m_srcRect, Color.White);
                    }

                    if (m_velo.X > 0)
                    {
                        sb.Draw(m_txr, new Rectangle((int)m_pos.X, (int)m_pos.Y, m_srcRect.Width, m_srcRect.Height), m_srcRect, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }

                    if (m_velo.X == 0)
                        sb.Draw(m_newTxr, m_pos, m_srcRect, Color.White);
                }

            }
        }
    }
}
