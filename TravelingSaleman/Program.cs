using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

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
            {"Albany", 0}, {"Ashland", 1}, {"Astoria", 2}, {"Baker.City", 3}, {"Bend", 4},
            {"Burns", 5}, {"Coos.Bay", 6}, {"Corvallis", 7}, {"Eugene", 8}, {"Florence", 9},
            {"Forest.Grove", 10}, {"Grants.Pass", 11}, {"Gresham", 12}, {"Klamath.Falls", 13},
            {"La.Grande", 14}, {"McMinnville", 15}, {"Medford", 16}, {"Milton-Freewater", 17},
            {"Newberg", 18}, {"Newport", 19}, {"Ontario", 20}, {"Pendleton", 21}, {"Portland", 22},
            {"Redmond", 23}, {"Roseburg", 24}, {"Salem", 25}, {"Springfield", 26},  {"The.Dalles", 27},
            {"Tillamook", 28}, {"Woodburn", 29},
        };
        static int total = 30;
        static int[,] mileage = new int[total, total];
        
        static int ReadFileFillMileage()
        {
            System.IO.StreamReader file = new System.IO.StreamReader("..\\..\\oregon-mileage.txt");
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
                    Debug.Assert(mileage[i, j] != 0 || mileage[j, i] != 0 || i==j, i.ToString() + " " + j.ToString());
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

            System.IO.StreamWriter outfile = new System.IO.StreamWriter("..\\..\\milage-matrix.txt");
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

        }


        static void Main(string[] args)
        {
            Console.WriteLine(ReadFileFillMileage());
            Console.ReadLine();
        }
    }
}
