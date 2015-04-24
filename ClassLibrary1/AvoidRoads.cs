

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DynamicProgramming
{
    public static class AvoidRoads
    {
        //http://community.topcoder.com/stat?c=problem_statement&pm=1889&rd=4709

        public static long NumWays(int width, int height, string[] bad)
        {
            return BinomialCoefficient((width + height) * 2, width + height);

            var badRoads = bad.Select(b => new Road
            {
                X0 = int.Parse(b[0].ToString()),
                Y0 = int.Parse(b[2].ToString()),
                X1 = int.Parse(b[4].ToString()),
                Y1 = int.Parse(b[6].ToString()),
            });

            return NumPaths(width, height, 0, 0, -1, -1, badRoads, 0);

        }

        public static long NumPaths(int width, int height, int x, int y, int sourceX, int sourceY, IEnumerable<Road> badRoads, int count)
        {
            if (x > width || y > height)
            {
                return count;
            }

            if (badRoads.Any(r => (r.X0 == sourceX && r.X1 == x && r.Y0 == sourceY && r.Y1 == y) || r.X1 == sourceX && r.X0 == x && r.Y1 == sourceY && r.Y0 == y))
            {
                return count;
            }

            if (x == width && y == height)
            {
                return count + 1;
            }

            else
            {
                return count + NumPaths(width, height, x + 1, y, x, y, badRoads, count) +
                       NumPaths(width, height, x, y + 1, x, y, badRoads, count);
            }
        }

        private static long BinomialCoefficient(long n, long k)
        {
            return Factorial(n)/(Factorial(k)*Factorial(n - k));
        }

        private static long Factorial(long n)
        {
            var sum = n;
            var i = n - 1;
            while (i > 0)
            {
                sum *= i;
                i--;
            }
            return sum;
        }
     

        public class Road
        {
            public int X0 { get; set; }
            public int Y0 { get; set; }
            public int X1 { get; set; }
            public int Y1 { get; set; }
        }
    }

    [TestFixture]
    public class AvoidRoadsTests
    {
        [Test]
        public void AvoidRoads()
        {
            var numRoads = DynamicProgramming.AvoidRoads.NumWays(6, 6, new string[] { });
            Assert.AreEqual(252, numRoads);
        }
    }
}