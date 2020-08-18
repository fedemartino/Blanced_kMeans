using System;

namespace KMeans
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSet s = new DataSet();
            s.Data["1"] = new int[]{4,12};
            s.Data["2"] = new int[]{8,13};
            s.Data["3"] = new int[]{6,10};
            s.Data["4"] = new int[]{9,5};
            s.Data["5"] = new int[]{8,2};
            s.Data["6"] = new int[]{10,3};
            s.Data["7"] = new int[]{4,-3};
            s.Data["8"] = new int[]{6,-5};
            s.Data["9"] = new int[]{4,-7};
            s.Data["10"] = new int[]{-8,3};
            s.Data["11"] = new int[]{-4,5};
            s.Data["12"] = new int[]{-6,7};
            KMeans k = new KMeans(4, 2, true, 1000);
            k.CaluclateKMeans(s);
            int i = 0;
            foreach (int[] c in k.Centroids)
            {
                Console.WriteLine($"Centrod {i} is [{c[0]},{c[1]}] with {s.CentroidLabels[i].Count} data points");
                i++;
            }
            Console.WriteLine($"Run in {k.Iterations} iterations");
        }
    }
}
