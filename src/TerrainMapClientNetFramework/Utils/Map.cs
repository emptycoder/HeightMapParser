using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TerrainMapClientNetFramework.Entities;

namespace TerrainMapClientNetFramework.Utils
{
    public static class Map
    {
        private delegate MapTile StrategyDelegate(string path);

        public static void Merge(Strategy strategy, MapResponse mapResponse)
        {
            int countY = mapResponse.mapPathes.Count / mapResponse.countX;
            int y = 0;
            int x = 0;

            Bitmap bitmap = new Bitmap(mapResponse.countX * 1081, countY * 1081);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                foreach (string path in mapResponse.mapPathes)
                {
                    MapTile mapTile = getMapTile[strategy](path);
                    g.DrawImage(mapTile.map, x * 1081, y * 1081);

                    y++;
                    if (y == countY)
                    {
                        x++;
                        y = 0;
                    }
                    mapTile.Dispose();
                }
            }
            bitmap.Save($"{Environment.CurrentDirectory}\\maps-merging {Enum.GetName(typeof(Strategy), strategy)}.png", ImageFormat.Png);
            bitmap.Dispose();
        }

        private readonly static Dictionary<Strategy, StrategyDelegate> getMapTile = new Dictionary<Strategy, StrategyDelegate>()
        {
            { Strategy.OnlyAster, delegate(string path) { return SingleType(path, Types.Aster); } },
            { Strategy.OnlyMerged, delegate(string path) { return SingleType(path, Types.Merged); } },
            { Strategy.OnlySRTM, delegate(string path) { return SingleType(path, Types.SRTM); } },
            { Strategy.OnlySRTMPlus, delegate(string path) { return SingleType(path, Types.SRTMPlus); } },
            { Strategy.MergedAster, delegate(string path) { return ManyTypes(path, Types.Merged, Types.Aster); } },
            { Strategy.AsterMerged, delegate(string path) { return ManyTypes(path, Types.Aster, Types.Merged); } }
        };

        private static MapTile TryToGetMapTile(string path, Types type)
        {
            string file = $"{path}\\map Height map {typeToMapName[type]}.png";
            return File.Exists(file) ? new MapTile(file) : MapTile.Null;
        }

        private static MapTile SingleType(string path, Types type)
        {
            MapTile mapTile = TryToGetMapTile(path, type);
            if (!mapTile.isNull())
            {
                return mapTile;
            }
            else
            {
                throw new Exception("Error merging: OnlyAster!");
            }
        }

        private static MapTile ManyTypes(string path, params Types[] types)
        {
            foreach (Types type in types)
            {
                MapTile mapTile = TryToGetMapTile(path, type);
                if (!mapTile.isNull())
                {
                    return mapTile;
                }
            }

            throw new Exception("Error merging: OnlyAster!");
        }

        private static Dictionary<Types, string> typeToMapName = new Dictionary<Types, string>()
        {
            { Types.Aster, "(ASTER 30m)" },
            { Types.Merged, "(Merged)" },
            { Types.SRTM, "(SRTM3 v4.1)" },
            { Types.SRTMPlus, "(SRTM30 Plus)" }
        };
    }
}
