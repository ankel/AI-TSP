using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace TravelingSaleman
{
    class AntColony
    {
        public int citiesCount;
        public int[,] distance;
        public double[,] pheromone;
        public Ant[] ants;
        public int best = Int32.MaxValue;
        public int[] bestRoute;

        public const double ALPHA = 0.5;
        public const double BETA = 4;
        public const double RHO = 0.5;
        public const double QVAL = 100;
        double INIT_PHEROMONE;

        Random prng;


        public AntColony(int citiesCount, int[,] distance)
        {
            this.citiesCount = citiesCount;
            this.distance = distance;
            INIT_PHEROMONE = 1.0 / citiesCount;
            prng = new Random();

            ants = new Ant[this.citiesCount];
            for (int i = 0; i < citiesCount; ++i)
            {
                ants[i] = new Ant(citiesCount, i);
            }

            pheromone = new double[citiesCount, citiesCount];
            for (int i = 0; i < citiesCount; ++i)
            {
                for (int j = 0; j < citiesCount; ++j)
                {
                    pheromone[i, j] = INIT_PHEROMONE;
                }
            }

            bestRoute = new int[citiesCount];
        }

        public double antProduct(int from, int to)
        {
            return (Math.Pow(pheromone[from, to], ALPHA) * Math.Pow((1.0 / distance[from, to]), BETA));
        }

        public int selectNextCity(Ant ant)
        {
            int from, to;
            double denom = 0.0;

            from = ant.currCity;

            for (to = 0; to < citiesCount; ++to)
            {
                if (ant.tabu[to] == 0)
                {
                    denom += antProduct(from, to);
                }
            }

            Debug.Assert(denom != 0, "Denom is zero");

            while (true)
            {
                double p;
                to++;
                if (to >= citiesCount)
                {
                    to = 0;
                }
                if (ant.tabu[to] == 0)
                {
                    p = antProduct(from, to) / denom;
                    if (prng.NextDouble() < p)
                    {
                        return to;
                    }
                }
            }
        }

        public int simulateAnts()
        {
            int k;
            int moving = 0;

            foreach (var ant in ants)
            {
                if (ant.pathIndex < citiesCount)
                {
                    ant.nextCity = selectNextCity(ant);
                    ant.tabu[ant.nextCity] = 1;
                    ant.path[ant.pathIndex++] = ant.nextCity;

                    ant.tourLength += distance[ant.currCity, ant.nextCity];

                    if (ant.pathIndex == citiesCount)
                    {
                        ant.tourLength += distance[ant.path[citiesCount - 1], ant.path[0]];
                    }
                    ant.currCity = ant.nextCity;
                    ++moving;
                }
            }

            return moving;
        }

        public void updateTrail()
        {
            int from, to, i, ant;

            for (from = 0; from < citiesCount; ++from)
            {
                for (to = 0; to < citiesCount; ++to)
                {
                    if (from != to)
                    {
                        pheromone[from, to] *= (1.0 - RHO);

                        if (pheromone[from, to] < 0.0)
                            pheromone[from, to] = INIT_PHEROMONE;
                    }
                }
            }

            for (ant = 0; ant < ants.Length; ++ant)
            {
                for (i = 0; i < citiesCount; ++i)
                {
                    if (i < citiesCount - 1)
                    {
                        from = ants[ant].path[i];
                        to = ants[ant].path[i + 1];
                    }
                    else
                    {
                        from = ants[ant].path[i];
                        to = ants[ant].path[0];
                    }

                    pheromone[from, to] += QVAL / ants[ant].tourLength * RHO;
                    pheromone[to, from] = pheromone[from, to];

                }
            }

        }

        public void Restart()
        {
            if (best > ants.Min<Ant>().tourLength)
            {
                best = ants.Min<Ant>().tourLength;
                Array.Copy(ants.Min<Ant>().path, bestRoute, citiesCount);
            }

            foreach (var ant in ants)
            {
                ant.Restart();
            }
        }

        public IEnumerable<int> ResultTour()
        {
            int start = Array.IndexOf(bestRoute, 0);
            int next = 0;
            if (0 < start && start < citiesCount - 1)
            {
                next = bestRoute[start - 1] < bestRoute[start + 1] ? start - 1 : start + 1;
            }
            int current = start;
            do
            {
                yield return bestRoute[current];
                if (next > start)
                {
                    current = (current + 1) % citiesCount;
                }
                else
                {
                    current = (current + citiesCount - 1) % citiesCount;
                }
            } while (current != start);
            yield return 0;
        }
    }
}
