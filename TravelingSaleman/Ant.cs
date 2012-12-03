using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TravelingSaleman
{
    class Ant : IComparable<Ant>
    {

        public int currCity;
        public int nextCity;
        public int[] tabu;
        public int pathIndex;
        public int[] path;
        public int tourLength;
        public int start;

        public Ant(int cityCount, int startingCity)
        {
            start = startingCity;
            tabu = new int[cityCount];
            path = new int[cityCount];

            Restart();
        }

        public void Restart()
        {
            nextCity = -1;
            tourLength = 0;
            for (int i = 0; i < tabu.Length; ++i)
            {
                tabu[i] = 0;
                path[i] = -1;
            }
            currCity = start;
            pathIndex = 1;
            path[0] = currCity;
            tabu[currCity] = 1;
        }

        int IComparable<Ant>.CompareTo(Ant other)
        {
            return this.tourLength - other.tourLength;
        }

        //double antProduct(int from, int to)
        //{
        //    Debug.Assert(pheromone != null && distance != null, "distance and pheromone array is not initialized");
        //    return (Math.Pow(pheromone[from, to], ALPHA) * Math.Pow((1.0 / distance[from, to]), BETA));
        //}

        //void selectNextCity()
        //{
        //    int from = currCity, to;
        //    double denom = 0.0;

        //    for (to = 0; to < tabu.Length; ++to)
        //    {
        //        if (tabu[to] == 0)
        //        {
        //            denom += antProduct(from, to);
        //        }
        //    }

        //    Debug.Assert(denom != 0, "Denom is zero");

        //    while (true)
        //    {
        //        double p;

        //        ++to;
        //        if (to >= tabu.Length)
        //            to = 0;

        //        if (tabu[to] == 0)
        //        {
        //            p = antProduct(from, to) / denom;
        //            if (prng.NextDouble() < p)
        //            {
        //                nextCity = to;
        //                return;
        //            }
        //        }
        //    }
        //}

        //bool Step()
        //{
        //    bool moved = false;
        //    if (pathIndex < tabu.Length)
        //    {
        //        selectNextCity();
        //        tabu[nextCity] = 1;
        //        path[pathIndex++] = nextCity;
        //        tourLength += distance[currCity, nextCity];

        //        if (pathIndex == tabu.Length)
        //        {
        //            tourLength += distance[path[tabu.Length - 1], path[0]];
        //        }

        //        currCity = nextCity;
        //        moved = true;
        //    }

        //    return moved;
        //}

    }
}
