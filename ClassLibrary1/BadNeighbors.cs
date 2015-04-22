using System;
using System.Linq;
using System.Net.NetworkInformation;
using NUnit.Framework;

namespace ClassLibrary1
{
    public static class BadNeighbors
    {
        //http://community.topcoder.com/stat?c=problem_statement&pm=2402&rd=5009


        private static int MaxDonationsHelper(int[] donations)
        {
            var maxDonations = new int[donations.Length];

            for (var i = 0; i < donations.Length; i++)
            {
                if (i == 0)
                {
                    maxDonations[i] = donations[i];
                }
                else if (i == 1)
                {
                    maxDonations[i] = Math.Max(donations[i], donations[i - 1]);
                }
                else if (i == 2)
                {
                    maxDonations[i] = Math.Max(maxDonations[i - 1], donations[i] + maxDonations[i - 2]);
                }

                else
                {
                    maxDonations[i] = Math.Max(Math.Max(donations[i] + maxDonations[i-3], donations[i] + maxDonations[i-2]), maxDonations[i-1]);
                }
                
            }

            return maxDonations.Last();
        }

        public static int MaxDonations(int[] donations)
        {
            var withoutFirst = donations.ToList();
            withoutFirst.RemoveAt(0);

            var withoutLast = donations.ToList();
            withoutLast.RemoveAt(donations.Count() - 1);

            return Math.Max(MaxDonationsHelper(withoutFirst.ToArray()), MaxDonationsHelper(withoutLast.ToArray()));
        }


        [TestFixture]
        public class BadNeighborsTests
        {
            [Test]
            public void BadNeighborTest()
            {
                var input = new[]  { 94, 40, 49, 65, 21, 21, 106, 80, 92, 81, 679, 4, 61,  
  6, 237, 12, 72, 74, 29, 95, 265, 35, 47, 1, 61, 397,
  52, 72, 37, 51, 1, 81, 45, 435, 7, 36, 57, 86, 81, 72 }
 ;
                var output = BadNeighbors.MaxDonations(input);
                Assert.AreEqual(output, 2926);
            }
        }
    }
}