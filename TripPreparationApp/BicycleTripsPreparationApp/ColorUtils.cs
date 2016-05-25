using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BicycleTripsPreparationApp
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
        public static List<Color> RgbLinearInterpolate(Color start, Color middle, Color end, int colorCount)
        {
            if (colorCount % 2 == 0)
                return new List<Color>() { Colors.Gold, Colors.LimeGreen };
            if (colorCount == 1)
                return new List<Color>() { Colors.Gold };
            List<Color> ret = new List<Color>();

            if (colorCount == 0)
                return ret;

            int size = (colorCount + 1) / 2;

            List<Color> res = ColorUtils.RgbLinearInterpolate(start, middle, size);
            if (res.Count > 0)
                res.RemoveAt(res.Count - 1);

            ret.AddRange(res);
            ret.AddRange(ColorUtils.RgbLinearInterpolate(middle, end, size));

            return ret;
        }
        public static Color HsvToRgb(double h, double s, double v)
        {
            int hi = (int)Math.Floor(h / 60.0) % 6;
            double f = (h / 60.0) - Math.Floor(h / 60.0);

            double p = v * (1.0 - s);
            double q = v * (1.0 - (f * s));
            double t = v * (1.0 - ((1.0 - f) * s));

            Color ret;

            switch (hi)
            {
                case 0:
                    ret = ColorUtils.GetRgb(v, t, p);
                    break;
                case 1:
                    ret = ColorUtils.GetRgb(q, v, p);
                    break;
                case 2:
                    ret = ColorUtils.GetRgb(p, v, t);
                    break;
                case 3:
                    ret = ColorUtils.GetRgb(p, q, v);
                    break;
                case 4:
                    ret = ColorUtils.GetRgb(t, p, v);
                    break;
                case 5:
                    ret = ColorUtils.GetRgb(v, p, q);
                    break;
                default:
                    ret = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
                    break;
            }
            return ret;
        }
        public static Color GetRgb(double r, double g, double b)
        {
            return Color.FromArgb(255, (byte)(r * 255.0), (byte)(g * 255.0), (byte)(b * 255.0));
        }
    }
}
