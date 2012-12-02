using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSaleman
{
    class State
    {
        int[] route;
        int score;

        public State(int size)
        {
            route = new int[size];
            for (int i = 0; i < size; ++i)
            {
                route[i] = i;
            }
        }

        public int Score(int[,] milage)
        {
            score = 0;
            for (int i = 1; i < route.Length; ++i)
            {
                int x = route[i - 1], y = route[i];
                score += milage[x,y];
            }

            return score;
        }

    }
}
