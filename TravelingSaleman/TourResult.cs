using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSaleman
{
    class TourResult : IComparable<TourResult>        
    {
        private int[] tourRoute;

        public int[] TourRoute
        {
            get { return tourRoute; }
            set { tourRoute = value; }
        }
        private int score;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public TourResult(int[] tourRoute, int score)
        {
            this.tourRoute = tourRoute;
            this.score = score;
        }

        int IComparable<TourResult>.CompareTo(TourResult other)
        {
            return this.Score - other.Score;
        }

        public string toString(Dictionary<string, int> cities)
        {
            string s = score.ToString() + Environment.NewLine;
            if (tourRoute[1] > tourRoute[tourRoute.Length - 2])
            {
                Array.Reverse(tourRoute);
            }
            foreach (var i in tourRoute)
            {
                foreach (var a in cities)
                {
                    if (a.Value == i)
                    {
                        s += a.Key + Environment.NewLine;
                        break;
                    }
                }
            }
            return s;
        }
    }
}
