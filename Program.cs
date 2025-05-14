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
            Console.WriteLine("Вы хотите:");
            Console.WriteLine("1. Ввести новые данные");
            Console.WriteLine("2. Загрузить сохранённые данные");
            Console.WriteLine("3. Сгенерировать случайные данные");

            string choice = Console.ReadLine();
            UserData data = null;

            if (choice == "2")
            {
                Console.Write("Введите имя файла (без расширения или с .json): ");
                string filename = Console.ReadLine().Trim();
                if (!filename.EndsWith(".json"))
                    filename += ".json";

                if (!DataStorage.Exists(filename))
                {
                    Console.WriteLine("Файл не найден.");
                    return;
                }

                data = DataStorage.Load(filename);
                Console.WriteLine("\nЗагруженные данные:");
                Console.WriteLine($"Вместимость контейнера: {data.Capacity}");
                Console.WriteLine($"Грузы: {string.Join(", ", data.Weights)}");
            }
            else if (choice == "3")
            {
                data = GenerateRandomData();
                if (data == null) return;

                Console.Write("Введите имя файла для сохранения (без расширения или с .json): ");
                string filename = Console.ReadLine().Trim();
                if (!filename.EndsWith(".json"))
                    filename += ".json";

                DataStorage.Save(data, filename);
                Console.WriteLine($"Случайные данные сохранены в файл {filename}");
            }
            else
            {
                data = InputNewData();
                if (data == null) return;

                Console.Write("Сохранить эти данные? (y/n): ");
                if (Console.ReadLine().Trim().ToLower() == "y")
                {
                    Console.Write("Введите имя файла для сохранения (без расширения или с .json): ");
                    string filename = Console.ReadLine().Trim();
                    if (!filename.EndsWith(".json"))
                        filename += ".json";

                    DataStorage.Save(data, filename);
                    Console.WriteLine($"Данные сохранены в файл {filename}");
                }
            }

            Console.WriteLine("\n--- Эвристическое решение ---");
            int heuristicResult = BinPackingSolver.HeuristicBinPacking(data.Weights, data.Capacity);
            Console.WriteLine($"Минимальное число контейнеров (эвристика): {heuristicResult}");

            Console.WriteLine("\n--- Полный перебор ---");
            int bruteForceRecursiveResult = BinPackingSolver.BruteForceBinPacking(data.Weights, data.Capacity);
            Console.WriteLine($"Минимальное число контейнеров (рекрсивный перебор): {bruteForceRecursiveResult}");
            int bruteForceIterativeResult = BinPackingSolver.BruteForceBinPackingIterative(data.Weights, data.Capacity);
            Console.WriteLine($"Минимальное число контейнеров (итеративный перебор): {bruteForceIterativeResult}");
        }

        private static UserData InputNewData()
        {
            Console.WriteLine("Введите вместимость контейнера (b):");
            if (!int.TryParse(Console.ReadLine(), out int b) || b <= 0)
            {
                Console.WriteLine("Ошибка: некорректное значение вместимости.");
                return null;
            }

            Console.WriteLine("Введите количество грузов (n):");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Ошибка: некорректное количество грузов.");
                return null;
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
                return null;
            }

            if (weights.Length != n)
            {
                Console.WriteLine("Ошибка: количество введённых грузов не совпадает с n.");
                return null;
            }

            return new UserData { Capacity = b, Weights = weights };
        }

        private static UserData GenerateRandomData()
        {
            Console.WriteLine("Введите количество грузов (n):");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Ошибка: некорректное количество грузов.");
                return null;
            }

            Console.WriteLine("Введите максимальную массу одного груза:");
            if (!int.TryParse(Console.ReadLine(), out int maxWeight) || maxWeight <= 0)
            {
                Console.WriteLine("Ошибка: некорректная масса.");
                return null;
            }

            Console.WriteLine("Введите вместимость контейнера (b):");
            if (!int.TryParse(Console.ReadLine(), out int b) || b <= 0)
            {
                Console.WriteLine("Ошибка: некорректная вместимость.");
                return null;
            }

            var rnd = new Random();
            var weights = new int[n];
            for (int i = 0; i < n; i++)
            {
                weights[i] = rnd.Next(1, maxWeight + 1);
            }

            Console.WriteLine("\nСгенерированные массы грузов:");
            Console.WriteLine(string.Join(", ", weights));

            return new UserData { Capacity = b, Weights = weights };
        }
    }
}