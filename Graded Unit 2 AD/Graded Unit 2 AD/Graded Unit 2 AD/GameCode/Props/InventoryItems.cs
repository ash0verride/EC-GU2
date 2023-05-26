using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GradedUnit2AD
{
    public class InventoryItems
    {
        private string m_item;
        private bool m_active;

        public string Item
        {
            get
            {
                return m_item;
            }
        }

        public bool Active
        {
            get
            {
                return m_active;
            }
            set
            {
                m_active = value;
            }
        }

        /// <summary>
        /// Creats an item
        /// </summary>
        /// <param name="item">Items name</param>
        public InventoryItems(string item)
        {
            m_active = true;
            m_item = item;
        }
    }
}
