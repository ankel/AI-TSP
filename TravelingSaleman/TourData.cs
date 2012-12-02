using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSaleman
{
    class TourData : ICloneable
    {
        private int[] citiesInTour;

        public int[] CitiesInTour
        {
            get { return citiesInTour; }
            set { citiesInTour = value; }
        }
        private int[,] mileage;

        public int[,] Mileage
        {
            get { return mileage; }
            set { mileage = value; }
        }

        public TourData(int[] citiesInTour, int[,] mileage)
        {
            this.citiesInTour = citiesInTour;
            this.mileage = mileage;
        }

        object ICloneable.Clone()
        {
            int[] newTour = new int[this.CitiesInTour.Length] ;
            Array.Copy(this.CitiesInTour, newTour, this.CitiesInTour.Length);
            return new TourData(newTour, this.Mileage);
        }
    }
}
