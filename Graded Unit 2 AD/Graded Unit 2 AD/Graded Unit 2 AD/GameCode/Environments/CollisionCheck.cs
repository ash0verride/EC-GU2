using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace GradedUnit2AD
{
    static class CollisionCheck
    {
        // Checks to see if intersecting
        public static bool basicCheck(this int[,] cells, Point idx)
        {
            try
            {
                if (cells[idx.X, idx.Y] > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        // Checks to see if on the ground
        public static bool groundCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X + (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height / 25)));
            Point idx2 = new Point((int)(idx.X + (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return true;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return true;
                            default:
                                return false;
                        }
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks the gound at another facing
        public static bool groundAltCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X + (rect.Width * 0.5f / 25)), (int)(idx.Y + (rect.Height / 25)));
            Point idx2 = new Point((int)(idx.X + (rect.Width * 1f / 25)), (int)(idx.Y + (rect.Height / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return true;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return true;
                            default:
                                return false;
                        }
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks to see if its safe to jump
        public static bool jumpCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X + (rect.Width * 0.5f / 25)), (int)(idx.Y - (rect.Height * 0.1f / 25)));
            Point idx2 = new Point((int)(idx.X + (rect.Width * 0.5f / 25)), (int)(idx.Y - (rect.Height * 0.1f / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return true;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return true;
                            default:
                                return false;
                        }
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks to see if the top will collide
        public static bool bumpCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X + (rect.Width * 0.25f / 25)), (int)(idx.Y));
            Point idx2 = new Point((int)(idx.X + (rect.Width * 0.75f / 25)), (int)(idx.Y));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return true;
                            default:
                                return false;
                        }
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks the right wall
        public static bool rightWallCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X + (rect.Width / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx2 = new Point((int)(idx.X + (rect.Width / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx3 = new Point((int)(idx.X + (rect.Width / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx4 = new Point((int)(idx.X + (rect.Width / 25)), (int)(idx.Y + (rect.Height * 0.875f / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return true;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return true;
                            default:
                                switch (cells[idx3.X, idx3.Y])
                                {
                                    case 1:
                                        return true;
                                    default:
                                        switch (cells[idx4.X, idx4.Y])
                                        {
                                            case 1:
                                                return true;
                                            default:
                                                return false;
                                        }
                                }
                        }
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks the left wall
        public static bool leftWallCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx2 = new Point((int)(idx.X), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx3 = new Point((int)(idx.X), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx4 = new Point((int)(idx.X), (int)(idx.Y + (rect.Height * 0.875f / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return true;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return true;
                            default:
                                switch (cells[idx3.X, idx3.Y])
                                {
                                    case 1:
                                        return true;
                                    default:
                                        switch (cells[idx4.X, idx4.Y])
                                        {
                                            case 1:
                                                return true;
                                            default:
                                                return false;
                                        }
                                }
                        }
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks to see if it can step up right
        public static bool rightStepCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X + (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height / 25)));
            Point idx2 = new Point((int)(idx.X + (rect.Width / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx3 = new Point((int)(idx.X + (rect.Width / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx4 = new Point((int)(idx.X + (rect.Width / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx5 = new Point((int)(idx.X + (rect.Width / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx6 = new Point((int)(idx.X + (rect.Width / 25)), (int)(idx.Y + (rect.Height * 0.875f / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return false;
                            default:
                                switch (cells[idx3.X, idx3.Y])
                                {
                                    case 1:
                                        return false;
                                    default:
                                        switch (cells[idx4.X, idx4.Y])
                                        {
                                            case 1:
                                                return false;
                                            default:
                                                switch (cells[idx5.X, idx5.Y])
                                                {
                                                    case 1:
                                                        return false;
                                                    default:
                                                        switch (cells[idx6.X, idx6.Y])
                                                        {
                                                            case 1:
                                                                return true;
                                                            default:
                                                                return false;
                                                        }
                                                }
                                        }
                                }
                        }
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks to see if is against a wall
        public static bool rightStepExtraCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X + (rect.Width / 25)), (int)(idx.Y + (rect.Height * 0.625 / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return false;
                    default:
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks to see if it can step up left
        public static bool leftStepCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx2 = new Point((int)(idx.X), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx3 = new Point((int)(idx.X), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx4 = new Point((int)(idx.X), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx5 = new Point((int)(idx.X), (int)(idx.Y + (rect.Height * 0.875f / 25)));
            Point idx6 = new Point((int)(idx.X + (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return false;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return false;
                            default:
                                switch (cells[idx3.X, idx3.Y])
                                {
                                    case 1:
                                        return false;
                                    default:
                                        switch (cells[idx4.X, idx4.Y])
                                        {
                                            case 1:
                                                return false;
                                            default:
                                                switch (cells[idx5.X, idx5.Y])
                                                {
                                                    case 1:
                                                        switch (cells[idx6.X, idx6.Y])
                                                        {
                                                            case 1:
                                                                return true;
                                                            default:
                                                                return false;
                                                        }
                                                    default:
                                                        return false;
                                                }
                                        }
                                }
                        }
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks to see if against a wall
        public static bool leftStepExtraCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X), (int)(idx.Y + (rect.Height * 0.625 / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return false;
                    default:
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks for a ledge
        public static bool rightEdgeCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height / 25)));
            Point idx2 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 1.25f / 25)));
            Point idx3 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 1.5f / 25)));
            Point idx4 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y + (rect.Height / 25)));
            Point idx5 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y + (rect.Height * 1.25f / 25)));
            Point idx6 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y + (rect.Height * 1.5f / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return false;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return false;
                            default:
                                switch (cells[idx3.X, idx3.Y])
                                {
                                    case 1:
                                        return false;
                                    default:
                                        switch (cells[idx4.X, idx4.Y])
                                        {
                                            case 1:
                                                return false;
                                            default:
                                                switch (cells[idx5.X, idx5.Y])
                                                {
                                                    case 1:
                                                        return false;
                                                    default:
                                                        switch (cells[idx6.X, idx6.Y])
                                                        {
                                                            case 1:
                                                                return false;
                                                            default:
                                                                return true;
                                                        }
                                                }
                                        }
                                }
                        }
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks for a ledge
        public static bool leftEdgeCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height / 25)));
            Point idx2 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height * 1.25f / 25)));
            Point idx3 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height * 1.5f / 25)));
            Point idx4 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height / 25)));
            Point idx5 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height * 1.25f / 25)));
            Point idx6 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height * 1.5f / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return false;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return false;
                            default:
                                switch (cells[idx3.X, idx3.Y])
                                {
                                    case 1:
                                        return false;
                                    default:
                                        switch (cells[idx4.X, idx4.Y])
                                        {
                                            case 1:
                                                return false;
                                            default:
                                                switch (cells[idx5.X, idx5.Y])
                                                {
                                                    case 1:
                                                        return false;
                                                    default:
                                                        switch (cells[idx6.X, idx6.Y])
                                                        {
                                                            case 1:
                                                                return false;
                                                            default:
                                                                return true;
                                                        }
                                                }
                                        }
                                }
                        }
                }
            }
            catch
            {
                return false;

            }
        }

        // Checks to see if it can jump right
        public static bool rightHerdleCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X + (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height / 25)));
            Point idx2 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx3 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx4 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx5 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx6 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx7 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.875f / 25)));
            Point idx8 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx9 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx10 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx11 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx12 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return false;
                            default:
                                switch (cells[idx3.X, idx3.Y])
                                {
                                    case 1:
                                        return false;
                                    default:
                                        switch (cells[idx4.X, idx4.Y])
                                        {
                                            case 1:
                                                return false;
                                            default:
                                                switch (cells[idx5.X, idx5.Y])
                                                {
                                                    case 1:
                                                        return false;
                                                    default:
                                                        switch (cells[idx6.X, idx6.Y])
                                                        {
                                                            case 1:
                                                                return false;
                                                            default:
                                                                switch (cells[idx7.X, idx7.Y])
                                                                {
                                                                    case 1:
                                                                        return false;
                                                                    default:
                                                                        switch (cells[idx8.X, idx8.Y])
                                                                        {
                                                                            case 1:
                                                                                return false;
                                                                            default:
                                                                                switch (cells[idx9.X, idx9.Y])
                                                                                {
                                                                                    case 1:
                                                                                        return false;
                                                                                    default:
                                                                                        switch (cells[idx10.X, idx10.Y])
                                                                                        {
                                                                                            case 1:
                                                                                                return false;
                                                                                            default:
                                                                                                switch (cells[idx11.X, idx11.Y])
                                                                                                {
                                                                                                    case 1:
                                                                                                        return false;
                                                                                                    default:
                                                                                                        switch (cells[idx12.X, idx12.Y])
                                                                                                        {
                                                                                                            case 1:
                                                                                                                return true;
                                                                                                            default:
                                                                                                                return false;
                                                                                                        }
                                                                                                }
                                                                                        }
                                                                                }
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks to see if it can jump left
        public static bool leftHerdleCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx2 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx3 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx4 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx5 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx6 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx7 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx8 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx9 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx10 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx11 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height * 0.875f / 25)));
            Point idx12 = new Point((int)(idx.X + (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return false;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return false;
                            default:
                                switch (cells[idx3.X, idx3.Y])
                                {
                                    case 1:
                                        return false;
                                    default:
                                        switch (cells[idx4.X, idx4.Y])
                                        {
                                            case 1:
                                                return false;
                                            default:

                                                switch (cells[idx5.X, idx5.Y])
                                                {
                                                    case 1:
                                                        switch (cells[idx6.X, idx6.Y])
                                                        {
                                                            case 1:
                                                                return false;
                                                            default:
                                                                switch (cells[idx7.X, idx7.Y])
                                                                {
                                                                    case 1:
                                                                        return false;
                                                                    default:
                                                                        switch (cells[idx8.X, idx8.Y])
                                                                        {
                                                                            case 1:
                                                                                return false;
                                                                            default:
                                                                                switch (cells[idx9.X, idx9.Y])
                                                                                {
                                                                                    case 1:
                                                                                        return false;
                                                                                    default:
                                                                                        switch (cells[idx10.X, idx10.Y])
                                                                                        {
                                                                                            case 1:
                                                                                                return false;
                                                                                            default:
                                                                                                switch (cells[idx11.X, idx11.Y])
                                                                                                {
                                                                                                    case 1:
                                                                                                        return false;
                                                                                                    default:
                                                                                                        switch (cells[idx12.X, idx12.Y])
                                                                                                        {
                                                                                                            case 1:
                                                                                                                return true;
                                                                                                            default:
                                                                                                                return false;
                                                                                                        }
                                                                                                }
                                                                                        }
                                                                                }
                                                                        }
                                                                }
                                                        }
                                                    default:
                                                        return false;
                                                }
                                        }
                                }
                        }
                }
            }
            catch
            {
                return false;
            }
        }

        // Checks to see if it can jump right over a gap
        public static bool rightJumpCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X + (rect.Width * 0.25f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx2 = new Point((int)(idx.X + (rect.Width * 0.25f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx3 = new Point((int)(idx.X + (rect.Width * 0.75f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx4 = new Point((int)(idx.X + (rect.Width * 0.75f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx5 = new Point((int)(idx.X + (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height / 25)));
            Point idx6 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx7 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx8 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx9 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx10 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx11 = new Point((int)(idx.X + (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.875f / 25)));
            Point idx12 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx13 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx14 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx15 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx16 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx17 = new Point((int)(idx.X + (rect.Width * 1.75f / 25)), (int)(idx.Y + (rect.Height * 0.875f / 25)));
            Point idx18 = new Point((int)(idx.X + (rect.Width * 2.25f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx19 = new Point((int)(idx.X + (rect.Width * 2.25f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx20 = new Point((int)(idx.X + (rect.Width * 2.25f / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx21 = new Point((int)(idx.X + (rect.Width * 2.25f / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx22 = new Point((int)(idx.X + (rect.Width * 2.25f / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx23 = new Point((int)(idx.X + (rect.Width * 2.25f / 25)), (int)(idx.Y + (rect.Height * 0.875f / 25)));
            Point idx24 = new Point((int)(idx.X + (rect.Width * 2.25f / 25)), (int)(idx.Y + (rect.Height / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return false;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return false;
                            default:
                                switch (cells[idx3.X, idx3.Y])
                                {
                                    case 1:
                                        return false;
                                    default:
                                        switch (cells[idx4.X, idx4.Y])
                                        {
                                            case 1:
                                                return false;
                                            default:
                                                switch (cells[idx5.X, idx5.Y])
                                                {
                                                    case 1:
                                                        switch (cells[idx6.X, idx6.Y])
                                                        {
                                                            case 1:
                                                                return false;
                                                            default:
                                                                switch (cells[idx7.X, idx7.Y])
                                                                {
                                                                    case 1:
                                                                        return false;
                                                                    default:
                                                                        switch (cells[idx8.X, idx8.Y])
                                                                        {
                                                                            case 1:
                                                                                return false;
                                                                            default:
                                                                                switch (cells[idx9.X, idx9.Y])
                                                                                {
                                                                                    case 1:
                                                                                        return false;
                                                                                    default:
                                                                                        switch (cells[idx10.X, idx10.Y])
                                                                                        {
                                                                                            case 1:
                                                                                                return false;
                                                                                            default:
                                                                                                switch (cells[idx11.X, idx11.Y])
                                                                                                {
                                                                                                    case 1:
                                                                                                        return false;
                                                                                                    default:
                                                                                                        switch (cells[idx12.X, idx12.Y])
                                                                                                        {
                                                                                                            case 1:
                                                                                                                return false;
                                                                                                            default:
                                                                                                                switch (cells[idx13.X, idx13.Y])
                                                                                                                {
                                                                                                                    case 1:
                                                                                                                        return false;
                                                                                                                    default:
                                                                                                                        switch (cells[idx14.X, idx14.Y])
                                                                                                                        {
                                                                                                                            case 1:
                                                                                                                                return false;
                                                                                                                            default:
                                                                                                                                switch (cells[idx15.X, idx15.Y])
                                                                                                                                {
                                                                                                                                    case 1:
                                                                                                                                        return false;
                                                                                                                                    default:
                                                                                                                                        switch (cells[idx16.X, idx16.Y])
                                                                                                                                        {
                                                                                                                                            case 1:
                                                                                                                                                return false;
                                                                                                                                            default:
                                                                                                                                                switch (cells[idx17.X, idx17.Y])
                                                                                                                                                {
                                                                                                                                                    case 1:
                                                                                                                                                        return false;
                                                                                                                                                    default:
                                                                                                                                                        switch (cells[idx18.X, idx18.Y])
                                                                                                                                                        {
                                                                                                                                                            case 1:
                                                                                                                                                                return false;
                                                                                                                                                            default:
                                                                                                                                                                switch (cells[idx19.X, idx19.Y])
                                                                                                                                                                {
                                                                                                                                                                    case 1:
                                                                                                                                                                        return false;
                                                                                                                                                                    default:
                                                                                                                                                                        switch (cells[idx20.X, idx20.Y])
                                                                                                                                                                        {
                                                                                                                                                                            case 1:
                                                                                                                                                                                return false;
                                                                                                                                                                            default:
                                                                                                                                                                                switch (cells[idx21.X, idx21.Y])
                                                                                                                                                                                {
                                                                                                                                                                                    case 1:
                                                                                                                                                                                        return false;
                                                                                                                                                                                    default:
                                                                                                                                                                                        switch (cells[idx22.X, idx22.Y])
                                                                                                                                                                                        {
                                                                                                                                                                                            case 1:
                                                                                                                                                                                                return false;
                                                                                                                                                                                            default:
                                                                                                                                                                                                switch (cells[idx23.X, idx23.Y])
                                                                                                                                                                                                {
                                                                                                                                                                                                    case 1:
                                                                                                                                                                                                        return false;
                                                                                                                                                                                                    default:
                                                                                                                                                                                                        switch (cells[idx24.X, idx24.Y])
                                                                                                                                                                                                        {
                                                                                                                                                                                                            case 1:
                                                                                                                                                                                                                return true;
                                                                                                                                                                                                            default:
                                                                                                                                                                                                                return false;
                                                                                                                                                                                                        }
                                                                                                                                                                                                }
                                                                                                                                                                                        }
                                                                                                                                                                                }
                                                                                                                                                                        }
                                                                                                                                                                }
                                                                                                                                                        }
                                                                                                                                                }
                                                                                                                                        }
                                                                                                                                }
                                                                                                                        }
                                                                                                                }
                                                                                                        }
                                                                                                }
                                                                                        }
                                                                                }
                                                                        }
                                                                }
                                                        }
                                                    default:
                                                        return false;
                                                }
                                        }
                                }
                        }

                }
            }
            catch
            {
                return false;
            }
        }

        // Checks to see if it can jump left over a gap
        public static bool leftJumpCheck(this int[,] cells, Point idx, Rectangle rect)
        {
            Point idx1 = new Point((int)(idx.X - (rect.Width * 1.25f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx2 = new Point((int)(idx.X - (rect.Width * 1.25f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx3 = new Point((int)(idx.X - (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx4 = new Point((int)(idx.X - (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx5 = new Point((int)(idx.X - (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx6 = new Point((int)(idx.X - (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 0.875f / 25)));
            Point idx7 = new Point((int)(idx.X - (rect.Width * 1.25f / 25)), (int)(idx.Y + (rect.Height * 1.125f / 25)));
            Point idx8 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx9 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx10 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx11 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx12 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx13 = new Point((int)(idx.X - (rect.Width * 0.75f / 25)), (int)(idx.Y + (rect.Height * 0.875f / 25)));
            Point idx14 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx15 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx16 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height * 0.125f / 25)));
            Point idx17 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height * 0.375f / 25)));
            Point idx18 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height * 0.625f / 25)));
            Point idx19 = new Point((int)(idx.X - (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height * 0.875f / 25)));
            Point idx20 = new Point((int)(idx.X + (rect.Width * 0.25f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx21 = new Point((int)(idx.X + (rect.Width * 0.25f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));
            Point idx22 = new Point((int)(idx.X + (rect.Width * 0.25f / 25)), (int)(idx.Y + (rect.Height / 25)));
            Point idx23 = new Point((int)(idx.X + (rect.Width * 0.75f / 25)), (int)(idx.Y - (rect.Height * 0.125f / 25)));
            Point idx24 = new Point((int)(idx.X + (rect.Width * 0.75f / 25)), (int)(idx.Y - (rect.Height * 0.375f / 25)));

            try
            {
                switch (cells[idx1.X, idx1.Y])
                {
                    case 1:
                        return false;
                    default:
                        switch (cells[idx2.X, idx2.Y])
                        {
                            case 1:
                                return false;
                            default:
                                switch (cells[idx3.X, idx3.Y])
                                {
                                    case 1:
                                        return false;
                                    default:
                                        switch (cells[idx4.X, idx4.Y])
                                        {
                                            case 1:
                                                return false;
                                            default:
                                                switch (cells[idx5.X, idx5.Y])
                                                {
                                                    case 1:
                                                        return false;
                                                    default:
                                                        switch (cells[idx6.X, idx6.Y])
                                                        {
                                                            case 1:
                                                                return false;
                                                            default:
                                                                switch (cells[idx7.X, idx7.Y])
                                                                {
                                                                    case 1:
                                                                        switch (cells[idx8.X, idx8.Y])
                                                                        {
                                                                            case 1:
                                                                                return false;
                                                                            default:
                                                                                switch (cells[idx9.X, idx9.Y])
                                                                                {
                                                                                    case 1:
                                                                                        return false;
                                                                                    default:
                                                                                        switch (cells[idx10.X, idx10.Y])
                                                                                        {
                                                                                            case 1:
                                                                                                return false;
                                                                                            default:
                                                                                                switch (cells[idx11.X, idx11.Y])
                                                                                                {
                                                                                                    case 1:
                                                                                                        return false;
                                                                                                    default:
                                                                                                        switch (cells[idx12.X, idx12.Y])
                                                                                                        {
                                                                                                            case 1:
                                                                                                                return false;
                                                                                                            default:
                                                                                                                switch (cells[idx13.X, idx13.Y])
                                                                                                                {
                                                                                                                    case 1:
                                                                                                                        return false;
                                                                                                                    default:
                                                                                                                        switch (cells[idx14.X, idx14.Y])
                                                                                                                        {
                                                                                                                            case 1:
                                                                                                                                return false;
                                                                                                                            default:
                                                                                                                                switch (cells[idx15.X, idx15.Y])
                                                                                                                                {
                                                                                                                                    case 1:
                                                                                                                                        return false;
                                                                                                                                    default:
                                                                                                                                        switch (cells[idx16.X, idx16.Y])
                                                                                                                                        {
                                                                                                                                            case 1:
                                                                                                                                                return false;
                                                                                                                                            default:
                                                                                                                                                switch (cells[idx17.X, idx17.Y])
                                                                                                                                                {
                                                                                                                                                    case 1:
                                                                                                                                                        return false;
                                                                                                                                                    default:
                                                                                                                                                        switch (cells[idx18.X, idx18.Y])
                                                                                                                                                        {
                                                                                                                                                            case 1:
                                                                                                                                                                return false;
                                                                                                                                                            default:
                                                                                                                                                                switch (cells[idx19.X, idx19.Y])
                                                                                                                                                                {
                                                                                                                                                                    case 1:
                                                                                                                                                                        return false;
                                                                                                                                                                    default:
                                                                                                                                                                        switch (cells[idx20.X, idx20.Y])
                                                                                                                                                                        {
                                                                                                                                                                            case 1:
                                                                                                                                                                                return false;
                                                                                                                                                                            default:
                                                                                                                                                                                switch (cells[idx21.X, idx21.Y])
                                                                                                                                                                                {
                                                                                                                                                                                    case 1:
                                                                                                                                                                                        return false;
                                                                                                                                                                                    default:
                                                                                                                                                                                        switch (cells[idx22.X, idx22.Y])
                                                                                                                                                                                        {
                                                                                                                                                                                            case 1:
                                                                                                                                                                                                switch (cells[idx23.X, idx23.Y])
                                                                                                                                                                                                {
                                                                                                                                                                                                    case 1:
                                                                                                                                                                                                        return false;
                                                                                                                                                                                                    default:
                                                                                                                                                                                                        switch (cells[idx24.X, idx24.Y])
                                                                                                                                                                                                        {
                                                                                                                                                                                                            case 1:
                                                                                                                                                                                                                return false;
                                                                                                                                                                                                            default:
                                                                                                                                                                                                                return true;
                                                                                                                                                                                                        }
                                                                                                                                                                                                }
                                                                                                                                                                                            default:
                                                                                                                                                                                                return false;
                                                                                                                                                                                        }
                                                                                                                                                                                }
                                                                                                                                                                        }
                                                                                                                                                                }
                                                                                                                                                        }
                                                                                                                                                }
                                                                                                                                        }
                                                                                                                                }
                                                                                                                        }
                                                                                                                }
                                                                                                        }
                                                                                                }
                                                                                        }
                                                                                }
                                                                        }
                                                                    default:
                                                                        return false;
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
