namespace TerrainMapClientNetFramework.Entities
{
    public struct Data
    {
        public readonly string dirPath;
        public Strategy strategy;
        public Area area;
        public double scale;

        public Data(string dirPath, Area area)
        {
            this.dirPath = dirPath;
            this.strategy = Strategy.All;
            this.scale = 0.1;
            this.area = area;
        }
    }
}
