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

            List<int> bins = new List<int>();

            foreach (var weight in weights)
            {
                bool placed = false;
                for (int i = 0; i < bins.Count; i++)
                {
                    if (bins[i] + weight <= capacity)
                    {
                        bins[i] += weight;
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    bins.Add(weight);
                }
            }

            return bins.Count;
        }

        public static int BruteForceBinPacking(int[] weights, int capacity)
        {
            int n = weights.Length;
            int minBins = n;

            void TryBins(List<List<int>> bins, int index)
            {
                if (index == n)
                {
                    minBins = Math.Min(minBins, bins.Count);
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
            return minBins;
        }

        public static int BruteForceBinPackingIterative(int[] weights, int capacity)
        {
            int n = weights.Length;
            int minBins = n;

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

                if (state.Index == n)
                {
                    minBins = Math.Min(minBins, state.Bins.Count);
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