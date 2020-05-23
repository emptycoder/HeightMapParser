using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using TerrainMapClientNetFramework.Entities;

namespace TerrainMapClientNetFramework.Utils
{
    public static class TerrainParty
    {
        const string Url = "http://terrain.party/api/export";

        /// <summary>
        /// Method download and extract data to folder.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>MapResponse that has tile folder pathes and count tile in columns.</returns>
        public static MapResponse DownloadWithExtract(Data data)
        {
            MapResponse mapResponse = new MapResponse();
            using (WebClient client = new WebClient())
            {
                for (double x = data.area.Left; x < data.area.Right + data.scale; x += data.scale)
                {
                    for (double y = data.area.Top; y >= data.area.Bottom - data.scale; y -= data.scale)
                    {
                        string path = $"{data.dirPath}\\map-{x}-{y}-{x + data.scale}-{y - data.scale}.zip";
                        Console.Write($"{path}: ");
                        client.DownloadFile(new Uri($"{Url}?name=map&box={x},{y},{x + data.scale},{y - data.scale}"), path);
                        Console.WriteLine("Complete!");
                        // Extract
                        string dest = path.Replace(".zip", "");
                        if (File.Exists(path))
                        {
                            ZipFile.ExtractToDirectory(path, dest);
                            File.Delete(path);
                        }

                        mapResponse.mapPathes.Add(dest);
                    }
                    mapResponse.countX++;
                }
            }

            return mapResponse;
        }
    }
}
