using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GradedUnit2AD
{
    class MapGenerator
    {
        private int[,] m_cells;
        private int m_width;
        private int m_height;

        public int[,] Cells
        {
            get
            {
                return m_cells;
            }
        }

        public MapGenerator(int[,] map)
        {
            m_width = map.GetLength(1);
            m_height = map.GetLength(0);

            m_cells = new int[m_width, m_height];
            for (int x = 0; x < m_width; x++)
                for (int y = 0; y < m_height; y++)
                {
                    m_cells[x, y] = map[y, x];
                }
        }
    }
}
