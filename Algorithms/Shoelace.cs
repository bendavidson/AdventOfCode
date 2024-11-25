using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Algorithms
{
    internal class Shoelace
    {
        public static double PolygonArea(double[] x, double[] y, int n)
        {
            double area = 0.0;

            int j = n - 1;

            for(int i = 0; i < n; i++)
            {
                area += (x[j] + x[i]) * (y[j] - y[i]);

                j = i;
            }

            return Math.Abs(area / 2.0);
        }
    }
}
