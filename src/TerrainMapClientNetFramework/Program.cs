using System;
using System.IO;
using TerrainMapClientNetFramework.Entities;
using TerrainMapClientNetFramework.Utils;

namespace TerrainMapClientNetFramework
{
    class Program
    {
        public static void Main(string[] args)
        {
            Data data = new Data(
                // Path for map files
                $"{Environment.CurrentDirectory}\\maps",
                // Area from openstreetmap: https://www.openstreetmap.org/export#map=4/48.17/87.36
                new Area(34.3927f, 48.7852f, 35.6726f, 48.1294f)
            );
            Console.WriteLine("-------------Terrain party client-------------");

            
            try
            {
                data.strategy = (Strategy) Input.EnumInput<Strategy>("Set up type", "Can't parse strategy!");
                MapResponse mapResponse;
                if (Input.BooleanInput("Check for config?", "Can't parse check config!"))
                {
                    Console.Write("--->Loading config: ");
                    mapResponse = Config.LoadConfig(data.dirPath, out data.scale);
                    Console.WriteLine("Complete!");
                }
                else
                {
                    data.scale = Input.FloatInput("Set up scale", "Can't parse scale!");
                    Console.Write("--->Creating directory for download: ");
                    if (!Directory.Exists(data.dirPath))
                    {
                        Directory.CreateDirectory(data.dirPath);
                    }
                    Console.WriteLine("Complete!");
                    Console.WriteLine("--->Downloading with extracting...");
                    mapResponse = TerrainParty.DownloadWithExtract(data);
                    Console.WriteLine("Complete!");
                    Console.Write("--->Save config: ");
                    Config.SaveConfig(data, mapResponse);
                    Console.WriteLine("Complete!");
                }
                
                Console.WriteLine("--->Merging images...");
                if (data.strategy == Strategy.All)
                {
                    foreach (int value in Enum.GetValues(typeof(Strategy)))
                    {
                        Strategy strategy = (Strategy)value;
                        Console.Write($"Map ({Enum.GetName(typeof(Strategy), value)}): ");
                        try
                        {
                            Map.Merge(strategy, mapResponse);
                        }
                        catch
                        {
                            Console.WriteLine("Error!");
                        }
                    }
                }
                else
                {
                    Console.Write($"Map ({Enum.GetName(typeof(Strategy), data.strategy)}): ");
                    Map.Merge(data.strategy, mapResponse);
                }
                Console.WriteLine("Complete!");
            }
            catch (Exception ex) { Console.WriteLine($"!--->{ex.Message}"); }

            Console.Write("Press any key to close program...");
            Console.ReadKey();
        }
    }
}
