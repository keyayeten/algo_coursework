// BinPackingSolver.cs

using System;
using System.Collections.Generic;
using System.Linq;

namespace algo_coursework
{
    public class BinPackingSolver
    {
        public static int HeuristicBinPacking(int[] weights, int capacity)
        {
            Array.Sort(weights);
            Array.Reverse(weights);

            List<List<int>> bins = new List<List<int>>();

            foreach (var weight in weights)
            {
                bool placed = false;
                for (int i = 0; i < bins.Count; i++)
                {
                    int currentSum = bins[i].Sum();
                    if (currentSum + weight <= capacity)
                    {
                        bins[i].Add(weight);
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    bins.Add(new List<int> { weight });
                }
            }

            // Вывод загруженности каждого контейнера
            for (int i = 0; i < bins.Count; i++)
            {
                Console.WriteLine($"Контейнер {i + 1}: [{string.Join(", ", bins[i])}]  -> Всего: {bins[i].Sum()}");
            }

            return bins.Count;
        }

        public static int BruteForceBinPacking(int[] weights, int capacity)
        {
            int n = weights.Length;
            int minBins = n;
            List<List<int>> bestBins = null;

            void TryBins(List<List<int>> bins, int index)
            {
                if (bins.Count >= minBins)
                    return;

                if (index == n)
                {
                    if (bins.Count < minBins)
                    {
                        minBins = bins.Count;
                        bestBins = bins.Select(bin => new List<int>(bin)).ToList();
                    }
                    return;
                }

                var currentWeight = weights[index];

                for (int i = 0; i < bins.Count; i++)
                {
                    if (bins[i].Sum() + currentWeight <= capacity)
                    {
                        bins[i].Add(currentWeight);
                        TryBins(bins, index + 1);
                        bins[i].RemoveAt(bins[i].Count - 1);
                    }
                }

                bins.Add(new List<int> { currentWeight });
                TryBins(bins, index + 1);
                bins.RemoveAt(bins.Count - 1);
            }

            TryBins(new List<List<int>>(), 0);

            if (bestBins != null)
            {
                for (int i = 0; i < bestBins.Count; i++)
                {
                    Console.WriteLine($"Контейнер {i + 1}: [{string.Join(", ", bestBins[i])}]  -> Всего: {bestBins[i].Sum()}");
                }
            }

            return minBins;
        }

        public static int BruteForceBinPackingIterative(int[] weights, int capacity)
        {
            int n = weights.Length;
            int minBins = n;
            List<List<int>> bestBins = null;

            var initialState = new State
            {
                Index = 0,
                Bins = new List<List<int>>()
            };

            var stack = new Stack<State>();
            stack.Push(initialState);

            while (stack.Count > 0)
            {
                var state = stack.Pop();

                if (state.Bins.Count >= minBins)
                    continue;

                if (state.Index == n)
                {
                    if (state.Bins.Count < minBins)
                    {
                        minBins = state.Bins.Count;
                        bestBins = CloneBins(state.Bins);
                    }
                    continue;
                }

                int currentWeight = weights[state.Index];

                // Пытаемся положить в существующие контейнеры
                for (int i = 0; i < state.Bins.Count; i++)
                {
                    if (state.Bins[i].Sum() + currentWeight <= capacity)
                    {
                        var newBins = CloneBins(state.Bins);
                        newBins[i].Add(currentWeight);
                        stack.Push(new State
                        {
                            Index = state.Index + 1,
                            Bins = newBins
                        });
                    }
                }

                // Создаём новый контейнер
                var newBinList = CloneBins(state.Bins);
                newBinList.Add(new List<int> { currentWeight });
                stack.Push(new State
                {
                    Index = state.Index + 1,
                    Bins = newBinList
                });
            }

            if (bestBins != null)
            {
                for (int i = 0; i < bestBins.Count; i++)
                {
                    Console.WriteLine($"Контейнер {i + 1}: [{string.Join(", ", bestBins[i])}]  -> Всего: {bestBins[i].Sum()}");
                }
            }

            return minBins;
        }

        // Вспомогательный класс состояния
        private class State
        {
            public int Index;
            public List<List<int>> Bins;
        }

        // Клонирование списка контейнеров
        private static List<List<int>> CloneBins(List<List<int>> bins)
        {
            var clone = new List<List<int>>();
            foreach (var bin in bins)
            {
                clone.Add(new List<int>(bin));
            }
            return clone;
        }
    }
}