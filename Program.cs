using System;
using System.Linq;

namespace algo_coursework
{
    class Program
    {
        static void Main(string[] args)
        {
            UserInterface.Run();
        }
    }

    public static class UserInterface
    {
        public static void Run()
        {
            Console.WriteLine("Введите вместимость контейнера (b):");
            if (!int.TryParse(Console.ReadLine(), out int b) || b <= 0)
            {
                Console.WriteLine("Ошибка: некорректное значение вместимости.");
                return;
            }

            Console.WriteLine("Введите количество грузов (n):");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Ошибка: некорректное количество грузов.");
                return;
            }

            Console.WriteLine("Введите массы грузов (через пробел):");
            int[] weights;

            try
            {
                weights = Console.ReadLine().Split().Select(int.Parse).ToArray();
            }
            catch
            {
                Console.WriteLine("Ошибка: не удалось распарсить числа.");
                return;
            }

            if (weights.Length != n)
            {
                Console.WriteLine("Ошибка: количество введённых грузов не совпадает с n.");
                return;
            }

            Console.WriteLine("\n--- Эвристическое решение ---");
            int heuristicResult = BinPackingSolver.HeuristicBinPacking(weights, b);
            Console.WriteLine($"Минимальное число контейнеров (эвристика): {heuristicResult}");

            Console.WriteLine("\n--- Полный перебор ---");
            int bruteForceResult = BinPackingSolver.BruteForceBinPacking(weights, b);
            Console.WriteLine($"Минимальное число контейнеров (перебор): {bruteForceResult}");
        }
    }
}