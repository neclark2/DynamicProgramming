using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ClassLibrary1
{
    //http://community.topcoder.com/stat?c=problem_statement&pm=1259&rd=4493
    public static class ZigZag
    {
        public static int LongestZigZag(int[] sequence)
        {

            //this is the O(N) solution, which makes use of the fact that non zig zagging segments can be 'stitched' out safely when we get to the next zig/zag

            if (sequence.Length < 2)
            {
                return sequence.Length;
            }

            if (sequence.Length == 2)
            {
                return sequence[0] == sequence[1] ? 1 : 2;
            }



            var totalDeletionCount = 0;
            var currDeletionCount = 0;
            var inDeletionSegment = false;
            var lastDirection = LastDirection.Unknown;
            var lastValue = sequence[0];

            for (var i = 1; i < sequence.Length; i++)
            {
                

                if (lastValue < sequence[i] && lastDirection != LastDirection.Positive)
                {
                    if (inDeletionSegment)
                    {
                        totalDeletionCount += currDeletionCount;
                        currDeletionCount = 0;
                        inDeletionSegment = false;
                    }
                    lastDirection = LastDirection.Positive;
                }

                else if (lastValue > sequence[i] && lastDirection != LastDirection.Negative)
                {
                    if (inDeletionSegment)
                    {
                        totalDeletionCount += currDeletionCount;
                        currDeletionCount = 0;
                        inDeletionSegment = false;
                    }
                    lastDirection = LastDirection.Negative;
                }

                else
                {
                    inDeletionSegment = true;
                    currDeletionCount++;
                }

                lastValue = sequence[i];
            }

            if (inDeletionSegment)
            {
                totalDeletionCount += currDeletionCount;
            }

            return sequence.Length - totalDeletionCount;
        }


        private enum LastDirection
        {
            Unknown, Positive, Negative
        }

        public static int LongestZigZagDynamic(int[] sequence)
        {
            //O(N^2) DP solution.  Works by creating two lookup tables that store the largest subsequence ending in a zig or zag at each index 'i'.
            
            if (sequence.Count() == 1)
            {
                return 1;
            }

            if (sequence.Count() == 2)
            {
                return 2;
            }

            var largestZig = new int[sequence.Length];
            var largestZag = new int[sequence.Length];

            largestZig[0] = 1;
            largestZag[0] = 1;

            for (var i = 1; i < sequence.Count(); i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (sequence[j] < sequence[i])
                    {
                        largestZig[i] = Math.Max(largestZag[j] + 1, largestZig[i]);
                    }

                    if (sequence[j] > sequence[i])
                    {
                        largestZag[i] = Math.Max(largestZig[j] + 1, largestZag[i]);
                    }
                }
            }

            return Math.Max(largestZig.Last(), largestZag.Last());
        }


    }




    [TestFixture]
    public class ZigZagTests
    {
        [Test]
        public void ZigZagTest()
        {
            var input = new[] {374, 40, 854, 203, 203, 156, 362, 279, 812, 955, 
600, 947, 978, 46, 100, 953, 670, 862, 568, 188, 
67, 669, 810, 704, 52, 861, 49, 640, 370, 908, 
477, 245, 413, 109, 659, 401, 483, 308, 609, 120, 
249, 22, 176, 279, 23, 22, 617, 462, 459, 244};
            var output = ZigZag.LongestZigZagDynamic(input);
            Assert.AreEqual(36, output);
        }
    }
}
