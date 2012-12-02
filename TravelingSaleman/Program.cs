using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TravelingSaleman
{
    class Program
    {
        /// <summary>
        /// Extract city names from oregon-mileage.txt
        /// </summary>
        static void GetNames()
        {
            SortedSet<String> cities = new SortedSet<string>();
            System.IO.StreamReader file = new System.IO.StreamReader("..\\..\\oregon-mileage.txt");
            System.IO.StreamWriter outfile = new System.IO.StreamWriter("..\\..\\oregon-cities.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] arr = line.Split(' ');
                cities.Add(arr[0]);
                cities.Add(arr[1]);
            }
            int cnt = 0;
            foreach (var city in cities)
            {
                outfile.WriteLine(String.Format("{{\"{0}\", {1}}},", city, cnt));
                cnt++;
            }
            outfile.Close();
            Console.WriteLine(cnt);
            Console.ReadLine();
        }

        static Dictionary<string, int> cities = new Dictionary<string, int>()
        {
            {"Albany", 0}, {"Ashland", 1}, {"Astoria", 2}, {"Baker.City", 3}, {"Bend", 4}, {"Burns", 5},
            {"Coos.Bay", 6}, {"Corvallis", 7}, {"Eugene", 8}, {"Florence", 9}, {"Forest.Grove", 10},
            {"Grants.Pass", 11}, {"Gresham", 12}, {"Klamath.Falls", 13}, {"La.Grande", 14}, 
            {"McMinnville", 15}, {"Medford", 16}, {"Newberg", 17}, {"Newport", 18}, {"Ontario", 19}, 
            {"Pendleton", 20}, {"Portland", 21}, {"Redmond", 22}, {"Roseburg", 23}, {"Salem", 24}, 
            {"Springfield", 25}, {"The.Dalles", 26}, {"Tillamook", 27}, {"Woodburn", 28},
        };
        public const int total = 29;
        static int[,] mileage = new int[total, total];
        public const int RUNTIME_MAX = 60;

        static int ReadFileFillMileage()
        {
            System.IO.StreamReader file = new System.IO.StreamReader(@"..\..\oregon-mileage.txt");
            string line;
            int cnt = 0;

            while ((line = file.ReadLine()) != null)
            {
                string[] fields = line.Split(' ');
                int x = cities[fields[0]],
                    y = cities[fields[1]],
                    z = Convert.ToInt32(fields[2]);
                mileage[x, y] = z;
                cnt++;
            }

            for (int i = 0; i < total; ++i)
            {
                for (int j = 0; j < total; ++j)
                {
                    Debug.Assert(mileage[i, j] != 0 || mileage[j, i] != 0 || i == j, i.ToString() + " " + j.ToString());
                    if (mileage[i, j] == 0)
                    {
                        mileage[i, j] = mileage[j, i];
                        cnt++;
                    }
                    if (mileage[j, i] == 0)
                    {
                        mileage[j, i] = mileage[i, j];
                        cnt++;
                    }
                }
            }

            System.IO.StreamWriter outfile = new System.IO.StreamWriter(@"..\..\milage-matrix.txt");
            for (int i = 0; i < total; ++i)
            {
                for (int j = 0; j < total; ++j)
                {
                    outfile.Write(mileage[i, j] + " ");
                }
                outfile.WriteLine();
            }
            outfile.Close();
            return cnt;
        }

        static void FillMileage()
        {
            using(System.IO.StreamReader mileageFile = new System.IO.StreamReader(@"..\..\milage-matrix.txt"))
            {
                for (int i = 0; i < total; ++i)
                {
                    string[] fields = mileageFile.ReadLine().Split(new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
                    Debug.Assert(fields.Length == total, "Field length != total");
                    for (int j = 0; j < total; ++j)
                    {
                        mileage[i, j] = Convert.ToInt32(fields[j]);
                    }
                }
            }

            //PrintArr(mileage);
        }
        
        [Conditional("DEBUG")]
        private static void PrintArr(int[,] mileage)
        {
            for (int i = 0; i < total; ++i)
            {
                for (int j = 0; j < total; ++j)
                {
                    Console.Write(mileage[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        static void Shuffle(int[] arr)
        {
            Random prng = new Random();

            for (int i = arr.Length - 2; i > 0; --i)       // start from the second-to-last, end at the second city
            {
                int j = prng.Next(1, i + 1);
                swap(arr, i, j);
            }
            Debug.Assert(arr[0] == arr[arr.Length - 1]);
        }

        static public void swap(int[] arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        static int Score(int[] route)
        {
            int score = 0;
            for (int i = 1; i < route.Length; ++i)
            {
                score += mileage[route[i - 1], route[i]];
            }
            return score;
        }

        static TourResult Search(object Data)
        {
            Debug.Assert(Data is TourData);
            TourData data = (TourData)Data;
            DateTime deadline = DateTime.Now.AddSeconds(RUNTIME_MAX);
            TourResult result = new TourResult(new int[data.CitiesInTour.Length], Int32.MaxValue);
            do
            {
                Shuffle(data.CitiesInTour);
                while (true)
                {
                    int swapScore = Score(data.CitiesInTour);
                    int swapI = -1;
                    for (int i = 2; i < data.CitiesInTour.Length - 1; ++i)
                    {
                        swap(data.CitiesInTour, i - 1, i);
                        if (Score(data.CitiesInTour) < swapScore)
                        {
                            swapScore = Score(data.CitiesInTour);
                            swapI = i;
                        }
                        swap(data.CitiesInTour, i - 1, i);
                    }

                    Debug.Assert(swapI > 1 && swapI < data.CitiesInTour.Length - 1 || swapI == -1, swapI.ToString());
                    if (swapI != -1)
                    {
                        swap(data.CitiesInTour, swapI, swapI - 1);
                    }
                    else        // local maximum reached
                    {
                        int n = Score(data.CitiesInTour);
                        if (n < result.Score)
                        {
                            Array.Copy(data.CitiesInTour, result.TourRoute, data.CitiesInTour.Length);
                            result.Score = n;
                        }
                        break;
                    }
                }
            } while (DateTime.Now <= deadline);
            return result;
        }

        static void Main(string[] args)
        {
            //GetNames();
            //Console.WriteLine(ReadFileFillMileage());
            FillMileage();
            TourData tourInfo = new TourData(ReadTour(args[0]).ToArray(), mileage);
            Task<TourResult>[] searchers = new Task<TourResult>[Environment.ProcessorCount / 2];
            TaskFactory tf = new TaskFactory();
            for (int i = 0; i < searchers.Length; ++i)
            {
                searchers[i] = Task.Factory.StartNew<TourResult>(Search, ((ICloneable)tourInfo).Clone());
            }
            Task.WaitAll(searchers);
            TourResult[] results = new TourResult[searchers.Length];
            for (int i = 0; i < searchers.Length; ++i)
            {
                results[i] = searchers[i].Result;
            }
            Array.Sort(results);
            Console.WriteLine(results[0].toString(cities));
            Console.ReadLine();
        }

        private static List<int> ReadTour(string p)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(p);
            string line;
            List<int> citiesInTour = new List<int>();
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    citiesInTour.Add(cities[line]);
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine(line + " city not founded");
                    return null;
                }
            }
            citiesInTour.Sort();
            citiesInTour.Add(citiesInTour[0]);
            return citiesInTour;
        }
    }
}
