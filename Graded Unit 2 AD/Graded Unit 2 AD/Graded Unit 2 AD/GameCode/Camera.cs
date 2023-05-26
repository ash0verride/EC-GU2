using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GradedUnit2AD
{
    struct Camera
    {
        private Vector2 m_pos;
        private float m_zoom;
        private float m_maxDepth;
        private float m_maxRight;

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

        /// <summary>
        /// Sets the cameras matrix
        /// </summary>
        /// <returns></returns>
        public Matrix getCam()
        {
            return Matrix.CreateTranslation(new Vector3(-m_pos.X, -m_pos.Y, 0)) * Matrix.CreateScale(m_zoom, m_zoom, 1f);
        }

        /// <summary>
        /// Updates the camera
        /// </summary>
        /// <param name="playerPos">Players position</param>
        /// <param name="playerVelo">Players velovity</param>
        /// <param name="graphics">Games graphics device</param>
        /// <param name="playerRect">Players rectangle</param>
        /// <param name="currLevel">The games current level</param>
        public void updateMe(Vector2 playerPos, Vector2 playerVelo, GraphicsDeviceManager graphics, Rectangle playerRect, Level currLevel)
        {
            m_zoom = currLevel.Scale;
            m_maxDepth = currLevel.Depth;
            m_maxRight = currLevel.Right;

            m_pos.X += (playerVelo.X * 0.5f);

            if (playerPos.X < (((graphics.PreferredBackBufferWidth / m_zoom) * 0.25f) + m_pos.X))
                m_pos.X = (playerPos.X - ((graphics.PreferredBackBufferWidth / m_zoom) * 0.25f));
            else if (playerPos.X > ((((graphics.PreferredBackBufferWidth / m_zoom) * 0.75f) + m_pos.X) - playerRect.Width))
                m_pos.X = ((playerPos.X - ((graphics.PreferredBackBufferWidth / m_zoom) * 0.75f)) + playerRect.Width);

            if (m_pos.X < 0)
                m_pos.X = 0;
            else if (m_pos.X > (m_maxRight - (graphics.PreferredBackBufferWidth / m_zoom)))
                m_pos.X = (m_maxRight - (graphics.PreferredBackBufferWidth / m_zoom));

            if (playerPos.Y < (((graphics.PreferredBackBufferHeight / m_zoom) * 0.33f) + m_pos.Y))
                m_pos.Y = (playerPos.Y - ((graphics.PreferredBackBufferHeight / m_zoom) * 0.33f));
            else if (playerPos.Y > ((((graphics.PreferredBackBufferHeight / m_zoom) * 0.88f) + m_pos.Y) - playerRect.Height))
                m_pos.Y = ((playerPos.Y - ((graphics.PreferredBackBufferHeight / m_zoom) * 0.88f)) + playerRect.Height);

            if (m_pos.Y < 0)
                m_pos.Y = 0;
            else if (m_pos.Y > (m_maxDepth - (graphics.PreferredBackBufferHeight / m_zoom)))
                m_pos.Y = (m_maxDepth - (graphics.PreferredBackBufferHeight / m_zoom));
        }
    }
}
