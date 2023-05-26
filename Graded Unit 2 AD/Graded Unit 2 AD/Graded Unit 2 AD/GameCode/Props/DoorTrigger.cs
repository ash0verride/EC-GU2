using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GradedUnit2AD
{
    // What the levels resets to
    enum LevelReset
    {
        playing,
        level0,
        level1,
        level2,
        level3,
        level4,
        level5,
        level6,
        level7,
        level8,
        level9,
        level10,
        end
    }

    class DoorTrigger
    {
        private List<TextEmitter> m_speech = new List<TextEmitter>();
        private Rectangle m_rect;
        private LevelReset m_reset;
        private LevelReset m_savedReset;
        private string m_in1;
        private string m_in2;
        private string m_message;

        private bool m_interact;

        public Rectangle Rect
        {
            get
            {
                return m_rect;
            }
        }

        public LevelReset Reset
        {
            get
            {
                return m_reset;
            }
        }

        public bool Interact
        {
            set
            {
                m_interact = value;
            }
        }

        /// <summary>
        /// Creates a door
        /// </summary>
        /// <param name="rect">Rectangle</param>
        /// <param name="reset">Level to Reset to</param>
        /// <param name="in1">1st required item</param>
        /// <param name="in2">2nd required item</param>
        /// <param name="message">Invalid output message</param>
        public DoorTrigger(Rectangle rect, LevelReset reset, string in1, string in2, string message)
        {
            m_rect = rect;
            m_savedReset = reset;
            m_in1 = in1;
            m_in2 = in2;
            m_message = message;

            m_reset = LevelReset.playing;
            m_interact = false;
        }

        /// <summary>
        /// Updates the doors
        /// </summary>
        /// <param name="gt">GameTime</param>
        /// <param name="items">Players items</param>
        /// <param name="player">Player</param>
        public void updateMe(GameTime gt, List<InventoryItems> items, PlayerActor player)
        {
            if (m_interact)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (m_in1 != null && items[i].Item == m_in1)
                    {
                        for (int j = 0; j < items.Count; j++)
                        {
                            if (m_in2 != null && items[j].Item == m_in2)
                            {
                                m_reset = m_savedReset;
                                break;
                            }
                            else if (m_in2 == null)
                            {
                                m_reset = m_savedReset;
                                break;
                            }
                            else if (j == items.Count - 1)
                            {
                                m_speech.Add(new TextEmitter(new Vector2(player.Pos.X + (player.Rect.Width / 2), player.Pos.Y), m_message, Color.Violet));
                            }
                        }
                        break;
                    }
                    else if (m_in1 == null)
                    {
                        m_reset = m_savedReset;
                        break;
                    }
                    else if (i == items.Count - 1)
                    {
                        m_speech.Add(new TextEmitter(new Vector2(player.Pos.X + (player.Rect.Width / 2), player.Pos.Y), m_message, Color.Violet));
                    }
                }

                m_interact = false;
            }

            for (int i = 0; i < m_speech.Count; i++)
            {
                m_speech[i].updateMe(gt);

                if (m_speech[i].Alpha < 0)
                {
                    m_speech.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Draws the pop up messages
        /// </summary>
        /// <param name="sb">SpriteBatch</param>
        public void drawMe(SpriteBatch sb)
        {
            for (int i = 0; i < m_speech.Count; i++)
            {
                m_speech[i].drawMe(sb);
            }
        }
    }
}
