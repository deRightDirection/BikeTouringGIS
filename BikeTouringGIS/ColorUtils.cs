using System.Collections.Generic;
using System.Windows.Media;

namespace BikeTouringGIS
{
    public class ColorUtils
    {
        public static List<Color> RgbLinearInterpolate(Color start, Color end, int colorCount)
        {
            List<Color> ret = new List<Color>();

            // linear interpolation lerp (r,a,b) = (1-r)*a + r*b = (1-r)*(ax,ay,az) + r*(bx,by,bz)
            for (int n = 0; n < colorCount; n++)
            {
                double r = (double)n / (double)(colorCount - 1);
                double nr = 1.0 - r;
                double A = (nr * start.A) + (r * end.A);
                double R = (nr * start.R) + (r * end.R);
                double G = (nr * start.G) + (r * end.G);
                double B = (nr * start.B) + (r * end.B);

                ret.Add(Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B));
            }

            return ret;
        }

        public static List<Color> RgbLinearInterpolate(Color start, Color end, int colorCount, Color defaultColorOne, Color defaultColorTwo)
        {
            if (colorCount == 2)
                return new List<Color>() { defaultColorOne, defaultColorTwo };
            if (colorCount == 1)
                return new List<Color>() { defaultColorOne };
            List<Color> ret = new List<Color>();

            if (colorCount == 0)
                return new List<Color>() { defaultColorOne };

            return ColorUtils.RgbLinearInterpolate(start, end, colorCount);
        }
    }
}