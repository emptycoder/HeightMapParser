using System;

namespace TerrainMapClientNetFramework.Utils
{
    public class Input
    {
        public static float FloatInput(string message, string errorMessage = "Can't parse!")
        {
            Console.Write($"?--->{message} (float type): ");
            if (float.TryParse(Console.ReadLine(), out float value))
            {
                return value;
            }

            throw new Exception(errorMessage);
        }

        public static int EnumInput<T>(string message, string errorMessage = "Can't parse!") where T : Enum
        {
            string[] strategyVariants = Enum.GetNames(typeof(T));
            int i = 0;
            for (; i < strategyVariants.Length; i++)
            {
                Console.WriteLine($"{i}. {strategyVariants[i]}");
            }
            Console.Write($"?--->{message} (0-{i - 1}): ");
            int result = Convert.ToInt32(Console.ReadLine());
            if (!Enum.IsDefined(typeof(T), result))
            {
                throw new Exception(errorMessage);
            }

            return result;
        }

        public static bool BooleanInput(string message, string errorMessage = "Can't parse!")
        {
            Console.Write($"?--->{message} (true, false): ");
            if (bool.TryParse(Console.ReadLine(), out bool flag))
            {
                return flag;
            }

            throw new Exception(errorMessage);
        }
    }
}
