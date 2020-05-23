using System;
using System.IO;
using TerrainMapClientNetFramework.Entities;

namespace TerrainMapClientNetFramework.Utils
{
    public static class Config
    {
        /// <summary>
        /// Save config to file.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mapResponse"></param>
        public static void SaveConfig(Data data, MapResponse mapResponse)
        {
            using (StreamWriter writer = new StreamWriter($"{data.dirPath}\\config.txt"))
            {
                writer.WriteLine(data.scale);
                writer.WriteLine(mapResponse.countX);
                foreach (string path in mapResponse.mapPathes)
                {
                    writer.WriteLine(path);
                }
                writer.Close();
            }
        }

        /// <summary>
        /// Load config from file.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static MapResponse LoadConfig(string dirPath, out double scale)
        {
            MapResponse mapResponse = new MapResponse();
            if (File.Exists($"{dirPath}\\config.txt"))
            {
                using (StreamReader reader = new StreamReader($"{dirPath}\\config.txt"))
                {
                    scale = Convert.ToDouble(reader.ReadLine());
                    mapResponse.countX = Convert.ToInt32(reader.ReadLine());
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        mapResponse.mapPathes.Add(line);
                    }
                }
            }
            else
            {
                throw new Exception("Config not found!");
            }

            return mapResponse;
        }
    }
}
