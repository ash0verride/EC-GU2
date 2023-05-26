using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GradedUnit2AD
{
    enum AiState
    {
        stop,
        wonder,
        detect,
        hostile,
        follow
    }
    enum AiType
    {
        civilian,
        dog,
        bear,
        follower,
        friendly,
        enemy

    }

    class NpcActor : BaseActor
    {
        private AiState m_npcState;
        private AiType m_npcType;

        private bool m_left;
        private bool m_jump;

        private List<TextEmitter> m_speech = new List<TextEmitter>();
        private float m_talkTrigger;
        private int m_response;
        private bool m_interact;
        private bool m_trading;
        private string m_in;
        private string m_out;

        public bool Interact
        {
            set
            {
                m_interact = value;
            }
        }

        public AiType Type
        {
            get
            {
                return m_npcType;
            }
        }

        public NpcActor(Rectangle rect, Texture2D txr, Texture2D newTxr, int fps, int xFrames, int yFrames, Vector2 velo, float speed, AiState defaultAI, AiType affiliation, string itemIn, string itemOut)
            : base(rect, txr, newTxr, fps, xFrames, yFrames, velo, speed)
        {
            m_npcState = defaultAI;
            m_npcType = affiliation;
            m_left = false;
            m_jump = false;
            m_interact = false;
            m_talkTrigger = Game1.RNG.Next(4, 10);

            if (itemIn != null)
            {
                m_trading = true;
                m_in = itemIn;
                m_out = itemOut;
            }
        }

        /// <summary>
        /// Update the Actor
        /// </summary>
        /// <param name="map">current collision map</param>
        /// <param name="player">player</param>
        /// <param name="gt">gameTime</param>
        /// <param name="items">player Items</param>
        public void updateMe(MapGenerator map, PlayerActor player, GameTime gt, List<InventoryItems> items)
        {
            #region interaction
            if (m_interact && m_trading)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].Item == m_in)
                    {
                        items.Add(new InventoryItems(m_out));
                        m_speech.Add(new TextEmitter(new Vector2(player.Pos.X + (player.Rect.Width / 2), player.Pos.Y), "Let's swap " + m_in + " for " + m_out, Color.Violet));
                        m_trading = false;
                        items.RemoveAt(i);
                        break;
                    }
                    else if (items[i] == items[items.Count - 1])
                    {
                        m_speech.Add(new TextEmitter(new Vector2(player.Pos.X + (player.Rect.Width / 2), player.Pos.Y), "They need " + m_in, Color.Violet));
                    }
                }

                m_interact = false;
            }
            #endregion

            #region follower
            if (m_npcType == AiType.follower)
            {
                if (m_npcState == AiState.follow)
                {
                    if ((m_pos.X + m_rect.Width) <= player.Pos.X)
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (m_speed / 30);
                        if (m_velo.X < 0)
                            m_velo.X += (m_speed / 30);
                        if (m_velo.X <= m_speed)
                            m_velo.X += (m_speed / 30);
                    }
                    else if ((m_pos.X - m_rect.Width) > player.Pos.X)
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (-m_speed / 30);
                        if (m_velo.X > 0)
                            m_velo.X -= (m_speed / 30);
                        if (m_velo.X >= -m_speed)
                            m_velo.X -= (m_speed / 30);
                    }
                    else
                    {
                        if (m_velo.X > 0)
                        {
                            if (m_velo.X > (m_speed / 30))
                                m_velo.X -= (m_speed / 30);
                            else
                                m_velo.X = 0;
                        }
                        else if (m_velo.X < 0)
                        {
                            if (m_velo.X < (-m_speed / 30))
                                m_velo.X += (m_speed / 30);
                            else
                                m_velo.X = 0;
                        }
                    }

                    if (m_interact)
                    {
                        if (Game1.RNG.NextDouble() > 0.5f)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "O.K. I'll stay!", Color.LightBlue));
                        else
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "I'm going to wait here", Color.LightBlue));
                        m_talkTrigger = Game1.RNG.Next(4, 10);
                        m_interact = false;
                        m_npcState = AiState.stop;
                    }
                }
                else if (m_npcState == AiState.stop)
                {
                    if (m_velo.X > 0)
                    {
                        if (m_velo.X > (m_speed / 30))
                            m_velo.X -= (m_speed / 30);
                        else
                            m_velo.X = 0;
                    }
                    else if (m_velo.X < 0)
                    {
                        if (m_velo.X < (-m_speed / 30))
                            m_velo.X += (m_speed / 30);
                        else
                            m_velo.X = 0;
                    }

                    if (m_interact)
                    {
                        if (Game1.RNG.NextDouble() > 0.5f)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Lead the way!", Color.LightBlue));
                        else
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Right behind you", Color.LightBlue));
                        m_talkTrigger = Game1.RNG.Next(4, 10);
                        m_interact = false;
                        m_npcState = AiState.follow;
                    }
                }
            }
            #endregion

            #region civilain
            else if (m_npcType == AiType.civilian)
            {
                if (m_npcState == AiState.wonder)
                {
                    if (m_left == false)
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (m_speed / 30);
                        if (m_velo.X < 0)
                            m_velo.X += (m_speed / 30);
                        if (m_velo.X <= m_speed)
                            m_velo.X += (m_speed / 30);
                    }
                    else if (m_left)
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (-m_speed / 30);
                        if (m_velo.X > 0)
                            m_velo.X -= (m_speed / 30);
                        if (m_velo.X >= -m_speed)
                            m_velo.X -= (m_speed / 30);
                    }
                }
            }
            #endregion

            #region friendlyGuard
            else if (m_npcType == AiType.friendly)
            {
                if (m_npcState == AiState.wonder)
                {
                    if (m_left == false)
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (m_speed / 30);
                        if (m_velo.X < 0)
                            m_velo.X += (m_speed / 30);
                        if (m_velo.X <= m_speed)
                            m_velo.X += (m_speed / 30);
                    }
                    else if (m_left)
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (-m_speed / 30);
                        if (m_velo.X > 0)
                            m_velo.X -= (m_speed / 30);
                        if (m_velo.X >= -m_speed)
                            m_velo.X -= (m_speed / 30);
                    }

                    if (m_interact)
                    {
                        if (Game1.RNG.NextDouble() > 0.5f)
                            m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "I'll defend this position!", Color.LightGreen));
                        else
                            m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "You want me to stay here?", Color.LightGreen));
                        m_talkTrigger = Game1.RNG.Next(4, 10);
                        m_interact = false;
                        m_npcState = AiState.detect;
                    }
                }
                else if (m_npcState == AiState.detect)
                {
                    if (m_velo.X > 0)
                    {
                        if (m_velo.X > (m_speed / 30))
                            m_velo.X -= (m_speed / 30);
                        else
                            m_velo.X = 0;
                    }
                    else if (m_velo.X < 0)
                    {
                        if (m_velo.X < (-m_speed / 30))
                            m_velo.X += (m_speed / 30);
                        else
                            m_velo.X = 0;
                    }

                    if (m_pos.X - player.Pos.X < -400 || m_pos.X - player.Pos.X > 400)
                    {
                        if (Game1.RNG.NextDouble() > 0.5f)
                            m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Where she go?", Color.LightGreen));
                        else
                            m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "This is pointless!", Color.LightGreen));
                        m_talkTrigger = Game1.RNG.Next(4, 10);
                        m_npcState = AiState.wonder;
                    }
                }
            }
            #endregion

            #region enemy
            else if (m_npcType == AiType.enemy)
            {
                if (m_npcState == AiState.hostile)
                {
                    if ((m_pos.X - m_rect.Width) > player.Pos.X || (map.Cells.rightEdgeCheck(m_posPoint, m_rect) && map.Cells.rightJumpCheck(m_posPoint, m_rect) == false))
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (-m_speed / 30);
                        if (m_velo.X > 0)
                            m_velo.X -= (m_speed / 30);
                        if (m_velo.X >= -m_speed)
                            m_velo.X -= (m_speed / 30);
                    }
                    else if ((m_pos.X + m_rect.Width) <= player.Pos.X || (map.Cells.leftEdgeCheck(m_posPoint, m_rect) && map.Cells.leftJumpCheck(m_posPoint, m_rect) == false))
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (m_speed / 30);
                        if (m_velo.X < 0)
                            m_velo.X += (m_speed / 30);
                        if (m_velo.X <= m_speed)
                            m_velo.X += (m_speed / 30);
                    }

                    if (m_pos.X - player.Pos.X < -200 || m_pos.X - player.Pos.X > 200)
                    {
                        if (Game1.RNG.NextDouble() > 0.5f)
                            m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Geunyeoleul bunsil", Color.Red));// Lost her
                        else
                            m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Geunyeoneun jug-eoss?", Color.Red));// Is she dead?

                        m_talkTrigger = Game1.RNG.Next(4, 10);
                        m_npcState = AiState.detect;
                    }
                }
                else if (m_npcState == AiState.detect)
                {
                    if (m_left == false)
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (m_speed / 30);
                        if (m_velo.X < 0)
                            m_velo.X += (m_speed / 30);
                        if (m_velo.X <= m_speed)
                            m_velo.X += (m_speed / 30);
                    }
                    else if (m_left)
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (-m_speed / 30);
                        if (m_velo.X > 0)
                            m_velo.X -= (m_speed / 30);
                        if (m_velo.X >= -m_speed)
                            m_velo.X -= (m_speed / 30);
                    }

                    if (m_pos.X - player.Pos.X > -50 && m_pos.X - player.Pos.X < 50)
                    {
                        if (Game1.RNG.NextDouble() > 0.5f)
                            m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Geunyeoui balgyeon", Color.Red));// Found her
                        else
                            m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Sigan-eun jug-eul!", Color.Red));// Time to die!
                        m_talkTrigger = Game1.RNG.Next(4, 10);
                        m_npcState = AiState.hostile;
                    }
                }
            }
            #endregion

            #region bear
            else if (m_npcType == AiType.bear)
            {
                if (m_npcState == AiState.hostile)
                {
                    if ((m_pos.X - m_rect.Width) > player.Pos.X || (map.Cells.rightEdgeCheck(m_posPoint, m_rect) && map.Cells.rightJumpCheck(m_posPoint, m_rect) == false))
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (-m_speed / 30);
                        if (m_velo.X > 0)
                            m_velo.X -= (m_speed / 30);
                        if (m_velo.X >= -m_speed)
                            m_velo.X -= (m_speed / 30);
                    }
                    else if ((m_pos.X + m_rect.Width) <= player.Pos.X || (map.Cells.leftEdgeCheck(m_posPoint, m_rect) && map.Cells.leftJumpCheck(m_posPoint, m_rect) == false))
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (m_speed / 30);
                        if (m_velo.X < 0)
                            m_velo.X += (m_speed / 30);
                        if (m_velo.X <= m_speed)
                            m_velo.X += (m_speed / 30);
                    }
                }
            }
            #endregion

            #region dog
            else if (m_npcType == AiType.dog)
            {
                if (m_npcState == AiState.wonder)
                {
                    if (m_left == false)
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (m_speed / 30);
                        if (m_velo.X < 0)
                            m_velo.X += (m_speed / 30);
                        if (m_velo.X <= m_speed)
                            m_velo.X += (m_speed / 30);
                    }
                    else if (m_left)
                    {
                        if (m_velo.X == 0)
                            m_velo.X = (-m_speed / 30);
                        if (m_velo.X > 0)
                            m_velo.X -= (m_speed / 30);
                        if (m_velo.X >= -m_speed)
                            m_velo.X -= (m_speed / 30);
                    }
                }
            }
            #endregion

            #region pos and velo maintenance

            if (m_velo.X > m_speed * 4)
                m_velo.X = m_speed * 4;
            if (m_velo.X < -m_speed * 4)
                m_velo.X = -m_speed * 4;
            if (m_velo.Y > m_speed * 4)
                m_velo.Y = m_speed * 4;
            if (m_velo.Y < -m_speed * 4)
                m_velo.Y = -m_speed * 4;

            m_pos += m_velo;

#endregion

            #region collision checks

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
                m_left = true;
            }
            else if (map.Cells.leftWallCheck(m_posPoint, m_rect))
            {
                if (m_velo.X == 0 && map.Cells.leftStepExtraCheck(m_posPoint, m_rect))
                    m_pos.X = (m_posPoint.X + 1) * 25 - 5;
                else
                    m_pos.X = (m_posPoint.X + 1) * 25;

                m_posPoint = new Point((int)(m_pos.X / 25), (int)(m_pos.Y / 25));
                m_velo.X = 0;
                m_left = false;
            }
            else if (m_velo.X > 0 && map.Cells.groundAltCheck(m_posPoint, m_rect))
            {
                if (map.Cells.rightHerdleCheck(m_posPoint, m_rect))
                    m_jump = true;
                else if (map.Cells.rightEdgeCheck(m_posPoint, m_rect))
                {
                    if (map.Cells.rightJumpCheck(m_posPoint, m_rect))
                        m_jump = true;
                    else
                        m_left = true;
                }
            }
            else if (m_velo.X < 0 && map.Cells.groundCheck(m_posPoint, m_rect))
            {
                if (map.Cells.leftHerdleCheck(m_posPoint, m_rect))
                    m_jump = true;
                else if (map.Cells.leftEdgeCheck(m_posPoint, m_rect))
                {
                    if (map.Cells.leftJumpCheck(m_posPoint, m_rect))
                        m_jump = true;
                    else
                        m_left = false;
                }
            }

            if ((m_velo.X < 0 && map.Cells.groundCheck(m_posPoint, m_rect)) || (m_velo.X > 0 && map.Cells.groundAltCheck(m_posPoint, m_rect)) || (m_velo.X == 0 && map.Cells.groundCheck(m_posPoint, m_rect) && map.Cells.groundAltCheck(m_posPoint, m_rect)))
            {
                m_pos.Y = m_posPoint.Y * 25;
                m_posPoint = new Point((int)(m_pos.X / 25), (int)(m_pos.Y / 25));
                m_velo.Y = 0;

                if (m_jump)
                {
                    if (map.Cells.jumpCheck(m_posPoint, m_rect))
                        m_velo.Y = 0;
                    else
                        m_velo.Y = -m_speed * 2;

                    m_jump = false;
                }
            }
            else
                m_velo.Y += 0.3f;

            #endregion

            #region Speech
            if (m_talkTrigger < 0)
            {
                m_response = Game1.RNG.Next(10);

                if (m_npcType == AiType.follower)
                {
                    if (m_response == 1)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "I'm Tired!", Color.LightBlue));
                    else if (m_response == 2)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "I hope things will get better", Color.LightBlue));
                    else if (m_response == 3)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "How far do we have to go", Color.LightBlue));
                    else if (m_response == 4)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Why has this happened", Color.LightBlue));
                    else if (m_response == 5)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Be quiet", Color.LightBlue));
                    else if (m_response == 6)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "*sobs*", Color.LightBlue));
                    else if (m_response == 7)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Don't leave me here", Color.LightBlue));
                    else if (m_response == 8)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "I don't think anyone's left", Color.LightBlue));
                    else if (m_response == 9)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Wanna play a game?", Color.LightBlue));
                    else
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Don't talk much ehh?", Color.LightBlue));

                    Game1.sfxNpcFollower.CreateInstance().Play();
                }
                else if (m_npcType == AiType.friendly)
                {
                    if (m_response == 1)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "We don't have enough bullets", Color.LightGreen));
                    else if (m_response == 2)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Stay with your family", Color.LightGreen));
                    else if (m_response == 3)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "I can't believe this", Color.LightGreen));
                    else if (m_response == 4)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Mark was a good soldier", Color.LightGreen));
                    else if (m_response == 5)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Keep Alert", Color.LightGreen));
                    else if (m_response == 6)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Brr... It's cold", Color.LightGreen));
                    else if (m_response == 7)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "*grumbles*", Color.LightGreen));
                    else if (m_response == 8)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "No reply on the comms", Color.LightGreen));
                    else if (m_response == 9)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "I can't feel my toes", Color.LightGreen));
                    else
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "This ain't a game", Color.LightGreen));

                    Game1.sfxNpcGuard.CreateInstance().Play();
                }
                else if (m_npcType == AiType.civilian)
                {
                    if (m_response == 1)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "!", Color.Yellow));
                    else if (m_response == 2)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Still no reception", Color.Yellow));
                    else if (m_response == 3)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "This can't be happening", Color.Yellow));
                    else if (m_response == 4)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "I can't believe anything", Color.Yellow));
                    else if (m_response == 5)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Does anyone have water?", Color.Yellow));
                    else if (m_response == 6)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Wanna have a riot?", Color.Yellow));
                    else if (m_response == 7)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Who did this?", Color.Yellow));
                    else if (m_response == 8)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Have you seen many others?", Color.Yellow));
                    else if (m_response == 9)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "I would kill for a banana", Color.Yellow));
                    else
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "This sucks!", Color.Yellow));

                    Game1.sfxNpcFollower.CreateInstance().Play();
                }
                else if (m_npcType == AiType.enemy)
                {
                    if (m_response == 1)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Chuwoyo", Color.Red));// It's Cold
                    else if (m_response == 2)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Hwajae!", Color.Red));// Fire!
                    else if (m_response == 3)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Geugeos-eul jab-eul", Color.Red));// Catch it
                    else if (m_response == 4)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Wae geudeul?", Color.Red));// Why them?
                    else if (m_response == 5)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Naneun baegopa", Color.Red));// I'm hungry
                    else if (m_response == 6)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Mueos-eul?", Color.Red));// What?
                    else if (m_response == 7)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Sunjong", Color.Red));// Obey
                    else if (m_response == 8)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Yeoboseyo?", Color.Red));// Hello?
                    else if (m_response == 9)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Yoleudan eun jug-eossda?", Color.Red));// Jordan is dead?
                    else
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Hapeu laipeu se sinhwa", Color.Red));// Half-Life 3 is a myth

                    Game1.sfxNpcEnemy.CreateInstance().Play();
                }
                else if (m_npcType == AiType.bear)
                {
                    if (m_response == 1)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "ROAR", Color.SandyBrown));
                    else if (m_response == 2)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "GRRR!", Color.SandyBrown));
                    else if (m_response == 3)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "*Sniff*", Color.SandyBrown));
                    else if (m_response == 4)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "GRAWW", Color.SandyBrown));
                    else if (m_response == 5)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "HMFF", Color.SandyBrown));
                    else if (m_response == 6)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Grr?", Color.SandyBrown));
                    else if (m_response == 7)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "*heavy breathing*", Color.SandyBrown));
                    else if (m_response == 8)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "GRRRR\nRRRRR\nRRRRR", Color.SandyBrown));
                    else if (m_response == 9)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "*yawn*", Color.SandyBrown));
                    else
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Meow?", Color.SandyBrown));

                    Game1.sfxNpcBear.CreateInstance().Play();
                }
                else if (m_npcType == AiType.dog)
                {
                    if (m_response < 5)
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Woof!", Color.Orange));
                    else
                        m_speech.Add(new TextEmitter(new Vector2(m_pos.X + (m_rect.Width / 2), m_pos.Y), "Bark!", Color.Orange));

                    Game1.sfxNpcDog.CreateInstance().Play();
                }
                m_talkTrigger = Game1.RNG.Next(4, 10);
            }
            else
                m_talkTrigger -= (float)gt.ElapsedGameTime.TotalSeconds;

            for(int i = 0; i < m_speech.Count; i++)
            {
                m_speech[i].updateMe(gt);
                
                if (m_speech[i].Alpha < 0)
                {
                    m_speech.RemoveAt(i);
                }
            }

            #endregion

            m_rect.X = (int)m_pos.X;
            m_rect.Y = (int)m_pos.Y;
        }

        /// <summary>
        /// draw the actor
        /// </summary>
        /// <param name="sb">spriteBatch</param>
        public override void drawMe(SpriteBatch sb)
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

            for (int i = 0; i < m_speech.Count; i++)
            {
                m_speech[i].drawMe(sb);
            }
        }
    }
}
