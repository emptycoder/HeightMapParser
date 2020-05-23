using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TerrainMapClientNetFramework.Entities
{
    public struct MapTile : IDisposable
    {
        public readonly Bitmap map;
        public readonly string path;

        public MapTile(string path)
        {
            this.path = path;
            this.map = (Bitmap)Image.FromFile(path);
            // AverageValues();
        }

        public unsafe void AverageValues()
        {
            sColor maxColor = sColor.Black,
                minColor = sColor.White;

            BitmapData mapData = map.LockBits(new Rectangle(0, 0, map.Width, map.Height), ImageLockMode.ReadWrite, map.PixelFormat);

            int bitsPerPixel = Image.GetPixelFormatSize(mapData.PixelFormat);
            byte* scan0 = (byte*)mapData.Scan0.ToPointer();

            float minBrightness = float.MaxValue,
                maxBrightness = float.MinValue;

            // Find max and min colors
            for (int i = 0; i < mapData.Height; ++i)
            {
                for (int j = 0; j < mapData.Width; ++j)
                {
                    byte* data = scan0 + i * mapData.Stride + j * bitsPerPixel / 8;
                    Color color = Color.FromArgb(data[2], data[1], data[0]);

                    float brightness = color.GetBrightness();
                    if (brightness > maxBrightness)
                    {
                        maxBrightness = brightness;
                    }
                    
                    if (brightness < minBrightness)
                    {
                        minBrightness = brightness;
                    }

                    //data is a pointer to the first byte of the 3-byte color data
                    //data[0] = blueComponent;
                    //data[1] = greenComponent;
                    //data[2] = redComponent;
                }
            }

            float brightnessRange = maxBrightness - minBrightness;
            // Set new values
            for (int i = 0; i < mapData.Height; ++i)
            {
                for (int j = 0; j < mapData.Width; ++j)
                {
                    byte* data = scan0 + i * mapData.Stride + j * bitsPerPixel / 8;
                    Color color = Color.FromArgb(data[2], data[1], data[0]);

                    double newBrightness = brightnessRange * color.GetBrightness() * 4;
                    //Console.WriteLine("BrightnessRange: " + brightnessRange + ", Color brightness: " + color.GetBrightness() + ", New brightness: " + newBrightness);
                    color = ColorFromHSV(color.GetHue(), color.GetSaturation(), newBrightness);

                    data[0] = color.B;
                    data[1] = color.G;
                    data[2] = color.R;
                }
            }

            map.UnlockBits(mapData);

            int pixelsCount = mapData.Width * mapData.Height;
        }

        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public static MapTile Null { get { return new MapTile(); } }

        public bool isNull()
        {
            return map == null;
        }

        public void Dispose()
        {
            map.Dispose();
        }
    }
}
