using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace TerrainMapClientNetFramework
{
    class Program
    {
        const string URL = "http://terrain.party/api/export";
        const double left = 34.3927;
        const double top = 48.7852;
        const double right = 35.6726;
        const double bottom = 48.1294;

        static readonly string DirPath = $"{Environment.CurrentDirectory}\\maps";
        static List<string> mapPathes = new List<string>();
        static Types type;
        static int countX = 0;
        static double scale;

        enum Types
        {
            Aster = 0,
            Merged = 1,
            SRTM = 2,
            SRTMPlus = 3,
            All = 4
        }

        static Dictionary<Types, string> typeToMapName = new Dictionary<Types, string>()
        {
            { Types.Aster, "(ASTER 30m)" },
            { Types.Merged, "(Merged)" },
            { Types.SRTM, "(SRTM3 v4.1)" },
            { Types.SRTMPlus, "(SRTM30 Plus)" }
        };

        static void Main(string[] args)
        {
            int i = 0;
            foreach (string name in Enum.GetNames(typeof(Types)))
            {
                Console.WriteLine($"{i}. {name}");
                i++;
            }
            Console.Write("Set up type (0-4): ");
            try
            {
                type = (Types)Convert.ToInt32(Console.ReadLine());

                Console.Write("Check for config? (true, false): ");
                if (bool.TryParse(Console.ReadLine(), out bool check))
                {
                    if (!check)
                    {
                        Console.Write("Set up scale: ");
                        if (double.TryParse(Console.ReadLine(), out scale))
                        {
                            Console.Write("!--->Creating directory for download: ");
                            if (!Directory.Exists(DirPath))
                            {
                                Directory.CreateDirectory(DirPath);
                            }
                            Console.WriteLine("Complete!");
                            Console.WriteLine("!--->Downloading...");
                            Downloading();
                            Console.WriteLine("!--->Download complete!");
                        }
                        else
                        {
                            Console.WriteLine("Can't parse scale!");
                        }
                    }
                    else
                    {
                        Console.Write("Loading config: ");
                        LoadConfig();
                        Console.WriteLine("Complete!");
                    }
                }
                Console.Write("Save config: ");
                SaveConfig();
                Console.WriteLine("--->Save config: ");
                Console.WriteLine("!--->Extracting...");
                Extracting();
                Console.WriteLine("!--->Extracting complete!");
                Console.WriteLine("!--->Merging images...");
                if (type == Types.All)
                {
                    Merging(Types.Aster);
                    Merging(Types.Merged);
                    Merging(Types.SRTM);
                    Merging(Types.SRTMPlus);
                }
                else
                {
                    Merging(type);
                }
                Console.WriteLine("!--->Merging complete!");
            }
            catch { Console.WriteLine("Can't parse type!"); }

            Console.Write("Press any key to close program...");
            Console.ReadKey();
        }

        static void SaveConfig()
        {
            using (StreamWriter writer = new StreamWriter($"{DirPath}\\config.txt"))
            {
                writer.WriteLine(scale);
                writer.WriteLine(countX);
                foreach (string path in mapPathes)
                {
                    writer.WriteLine(path);
                }
                writer.Close();
            }
        }

        static void LoadConfig()
        {
            if (File.Exists($"{DirPath}\\config.txt"))
            {
                using (StreamReader reader = new StreamReader($"{DirPath}\\config.txt"))
                {
                    scale = Convert.ToDouble(reader.ReadLine());
                    countX = Convert.ToInt32(reader.ReadLine());
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        mapPathes.Add(line);
                    }
                }
            }
            else
            {
                throw new Exception("Config not found!");
            }
        }

        static void Downloading()
        {
            using (WebClient client = new WebClient())
            {
                for (double x = left; x < right + scale; x += scale)
                {
                    for (double y = top; y >= bottom - scale; y -= scale)
                    {
                        string path = $"{DirPath}\\map-{x}-{y}-{x + scale}-{y - scale}.zip";
                        Console.Write($"{path}: ");
                        mapPathes.Add(path);
                        client.DownloadFile(new Uri($"{URL}?name=map&box={x},{y},{x + scale},{y - scale}"), path);
                        Console.WriteLine("Complete!");
                    }
                    countX++;
                }
            }
        }

        static void Extracting()
        {
            for (int i = 0; i < mapPathes.Count; i++)
            {
                string dest = mapPathes[i].Replace(".zip", "");
                if (File.Exists(mapPathes[i]))
                {
                    ZipFile.ExtractToDirectory(mapPathes[i], dest);
                    File.Delete(mapPathes[i]);
                }
                mapPathes[i] = dest;
            }
        }

        static void Merging(Types type)
        {
            try
            {
                int countY = mapPathes.Count / countX;
                int y = 0;
                int x = 0;
                Bitmap bitmap = new Bitmap(countX * 1081, countY * 1081);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    foreach (string path in mapPathes)
                    {
                        Image map = Image.FromFile($"{path}\\map Height map {typeToMapName[type]}.png");

                        g.DrawImage(map, x * 1081, y * 1081);

                        map.Dispose();

                        y++;
                        if (y == countY)
                        {
                            x++;
                            y = 0;
                        }
                    }
                }
                bitmap.Save($"{Environment.CurrentDirectory}\\maps-merging {typeToMapName[type]}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error merging! Error: {ex.Message}");
            }
        }
    }
}
