using System.Drawing;

namespace TerrainMapClientNetFramework.Entities
{
    public struct sColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public sColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public HSV GetHSV()
        {
            Color color = Color.FromArgb(R, G, B);
            return new HSV(color.GetHue(), color.GetSaturation(), color.GetBrightness());
        }

        public Color ToColor()
        {
            return Color.FromArgb(R, G, B);
        }

        public static sColor Black { get { return new sColor(0, 0, 0); } }
        public static sColor White { get { return new sColor(255, 255, 255); } }
    }
}
