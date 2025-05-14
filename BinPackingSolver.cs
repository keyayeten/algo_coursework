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
    }
}