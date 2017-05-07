using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace craxx_plus.Controls.rainbow
{
    public static class rainbowColors
    {
        public static Color r1 = Color.FromRgb(255, 0, 0);
        public static Color r2 = Color.FromRgb(255, 165, 0);
        public static Color r3 = Color.FromRgb(255, 255, 0);
        public static Color r4 = Color.FromRgb(0, 128, 0);
        public static Color r5 = Color.FromRgb(0, 255, 255);
        public static Color r6 = Color.FromRgb(0, 0, 255);
        public static Color r7 = Color.FromRgb(128, 0, 128);
        public static Color getRainbow(int index)
        {
            switch (index)
            {
                case (1):
                    {
                        return r2;
                    }
                case (2):
                    {
                        return r3;
                    }
                case (3):
                    {
                        return r4;
                    }
                case (4):
                    {
                        return r5;
                    }
                case (5):
                    {
                        return r6;
                    }
                case (6):
                    {
                        return r7;
                    }
                case (7):
                    {
                        return r1;
                    }
                default:
                    {

                        return r1;
                    }
            }

        }
    }
}