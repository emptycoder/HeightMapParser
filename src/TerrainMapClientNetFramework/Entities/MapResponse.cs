using System.Collections.Generic;

namespace TerrainMapClientNetFramework.Entities
{
    public class MapResponse
    {
        public List<string> mapPathes;
        public int countX;

        public MapResponse()
        {
            this.mapPathes = new List<string>();
            this.countX = 0;
        }
    }
}
