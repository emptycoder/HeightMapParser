namespace TerrainMapClientNetFramework.Entities
{
    public struct HSV
    {
        public float Hue { get; set; }
        public float Saturation { get; set; }
        public float Brightness { get; set; }

        public HSV(float hue, float saturation, float brightness)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Brightness = brightness;
        }
    }
}
