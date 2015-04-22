using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    [TestFixture]
    public static class CountingSort
    {
        public static IEnumerable<int> SortIntegers(int[] input)
        {
            
            var count = new int[input.Length];
            var timer = Stopwatch.StartNew();
            for(var i = 0; i < input.Length; i++)
            {
                var val = input[i];
                count[val] = count[val] + 1;
            }
            Console.WriteLine("Created histogram: {0}", timer.Elapsed);
            timer = Stopwatch.StartNew();

            var total = 0;
            for(var i = 0; i < input.Length; i++){
                var oldCount = count[i];
                count[i] = total;
                total += oldCount;
            }
            Console.WriteLine("Identified indexes: {0}", timer.Elapsed);

            timer = Stopwatch.StartNew();

            var output = new int[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                
                var j = input[i];
                var currCount = count[j];
                output[currCount] = i;
                count[j] = currCount + 1;
            }

            Console.WriteLine("Generated output: {0}", timer.Elapsed);
            return output;
        }

        [Test]
        public static void Test1()
        {
            var rng = new Random();
            const int size = 100000000;
            var input = new int[size];
            foreach (var i in Enumerable.Range(0, size))
            {
                input[i] = rng.Next(size);
            }
            var inputList = input.ToList();

            var timer = Stopwatch.StartNew();

            var o1 = SortIntegers(input);

            timer.Stop();

            Console.WriteLine("Total Elapsed for count sort: {0}", timer.Elapsed);

            timer = Stopwatch.StartNew();

            inputList.Sort();

            timer.Stop();

            Console.WriteLine("Total Elapsed for comparison sort: {0}", timer.Elapsed);
        }
    }


}