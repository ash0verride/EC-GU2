using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradedUnit2AD
{
    class ItemPickup : StaticGraphic
    {
        private List<TextEmitter> m_speech = new List<TextEmitter>();
        private Vector2 m_pos;
        private Texture2D m_txr2;
        private InventoryItems m_item;
        private bool m_used;
        private bool m_interact;

        public bool Interact
        {
            set
            {
                m_interact = value;
            }
        }

        public ItemPickup(Rectangle rect, Texture2D txr, Texture2D txr2, InventoryItems item)
            : base(rect, txr)
        {
            m_pos = new Vector2(rect.X, rect.Y);
            m_used = false;
            m_interact = false;
            m_txr2 = txr2;
            m_item = item;
        }

        /// <summary>
        /// Update the pickup
        /// </summary>
        /// <param name="gt">gameTime</param>
        /// <param name="items">player items</param>
        /// <param name="player">player</param>
        public void updateMe(GameTime gt, List<InventoryItems> items, PlayerActor player)
        {
            if (m_interact && m_used == false)
            {
                items.Add(m_item);
                m_speech.Add(new TextEmitter(new Vector2(player.Pos.X + (player.Rect.Width / 2), player.Pos.Y), m_item.Item + " picked up", Color.Violet));
                m_used = true;
                m_interact = false;
            }
            else if (m_interact)
            {
                m_speech.Add(new TextEmitter(new Vector2(player.Pos.X + (player.Rect.Width / 2), player.Pos.Y), "I already took " + m_item.Item, Color.Violet));
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
        /// Draw the sprites
        /// </summary>
        /// <param name="sb">spriteBatch</param>
        public void drawMe(SpriteBatch sb)
        {
            if (m_used)
                sb.Draw(m_txr2, m_rect, Color.White);
            else
                sb.Draw(m_txr, m_rect, Color.White);

            for (int i = 0; i < m_speech.Count; i++)
            {
                m_speech[i].drawMe(sb);
            }
        }
    }
}
