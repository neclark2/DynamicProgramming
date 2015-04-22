using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ClassLibrary1
{

    //https://www.topcoder.com/community/data-science/data-science-tutorials/dynamic-programming-from-novice-to-advanced/
    public static class MinCoinSetFinder
    {

        public static IEnumerable<int> MinCoinSet(IEnumerable<int> coins, int sum)
        {

            var sumList = new MinValue[sum+1];
            sumList[0] = new MinValue() {Diff = 0, NumCoins = 0};

            var currSum = 1;
            while (currSum <= sum)
            {
                sumList[currSum] = new MinValue() {Diff = Int32.MaxValue, NumCoins = Int32.MaxValue};
                foreach (var item in coins)
                {
                    if (item <= currSum)
                    {
                        if (sumList[currSum - item].NumCoins + 1 < sumList[currSum].NumCoins)
                        {
                            sumList[currSum] = new MinValue()
                            {
                                NumCoins = sumList[currSum - item].NumCoins + 1,
                                Diff = currSum - item
                            };
                        }
                    }
                }

                currSum++;
            }

            return GetCoinSet(sumList.ToList(), new List<int>(), sum);
        }

        private static IEnumerable<int> GetCoinSet(List<MinValue> CoinSums, List<int> CoinSet, int sum)
        {
            var minValue = CoinSums[sum];

            if (sum == 0)
            {
                
            }
            else if (minValue.Diff == 0)
            {
                CoinSet.Add(sum);
            }
            else
            {
                GetCoinSet(CoinSums, CoinSet, minValue.Diff);
                GetCoinSet(CoinSums, CoinSet, sum - minValue.Diff);
            }

            return CoinSet;
        }  


        public class MinValue
        {
            public int Diff { get; set; }
            public int NumCoins { get; set; }
        }

    }

    [TestFixture]
    public class CoinListTests
    {
        [Test]
        public void CoinTest()
        {
            var coins = new List<int>()
            {
                1,
                5,
                10,
                25,
                100
            };

            var sum = 117;

            var outCoins = MinCoinSetFinder.MinCoinSet(coins, sum);

        }
    }


    public static class LongestNonDecreasingNumberFinder
    {
        public static int FindLengthOfLongestNonDecreasingNumber(IEnumerable<int> sequence)
        {
            
            var startIndex = 0;
            var currIndex = 0;
            var maxSize = 0;
            int? lastItem = null;

            foreach (var item in sequence)
            {
                if (lastItem.HasValue && item < lastItem)
                {
                    maxSize = Math.Max(maxSize, currIndex - startIndex);
                    startIndex = currIndex;
                }

                lastItem = item;
                currIndex++;
            }

            maxSize = Math.Max(maxSize, currIndex - startIndex);
            return maxSize;
        }
    }

    public static class LongestNonDecreasingNonConsecutiveNumberFinder
    {
        public static int FindLengthOfLongestNonDecreasingNumber(IList<int> sequence)
        {   
           
            var optimalSequence = new int[sequence.Count];
            for(var i = 1; i < sequence.Count(); i++)
            {
                optimalSequence[i] = 1;
                for (var j = 0; j < i; j++)
                {
                    if (sequence[j] <= sequence[i])
                    {
                        optimalSequence[i] = Math.Max(optimalSequence[i], optimalSequence[j] + 1);
                    }
                }
            }

            return optimalSequence.Last();
        }
    }

    [TestFixture]
    public class LongestNonDecreasingNumberFinderTests
    {

        [Test]
        public void LongestNonDecreasingNumberTest()
        {
            var seq = new[]
            {10, 2, 3, 4, 3, 3, 3, 3, 4, 5, 6, 1, 2, 1, 2, 1, 2, 3, 4, 3, 4, 3, 3, 3, 4, 4, 4, 4, 4, 5, 6, 10, 10000};

            var num = LongestNonDecreasingNumberFinder.FindLengthOfLongestNonDecreasingNumber(seq);
            var num2 = LongestNonDecreasingNonConsecutiveNumberFinder.FindLengthOfLongestNonDecreasingNumber(seq);
        }
    }




    public class GraphNode
        {
            public List<Edge> Edges { get; set; }
            public int Id { get; set; }
        }

        public class Edge
        {
            public GraphNode Node { get; set; }
            public int Weight { get; set; }
        }

    public class MinDistance
    {
        public int Dist { get; set; }
        public MinDistance Origin { get; set; }
        public GraphNode Node { get; set; }
    }
    public static class ShortestGraphRouteFinder
    {
        

        

        public static MinDistance FindShortestDistance(GraphNode start, GraphNode end)
        {
            var minDistances = new Dictionary<GraphNode, MinDistance>();
            minDistances[start] = new MinDistance() {Dist = 0, Origin = null, Node = start};
            ShortestDistanceHelper(start, 0, minDistances);

            if (!minDistances.ContainsKey(end))
            {
                return null;
            }
            else
            {
                return minDistances[end];
            }
        }

        private static void ShortestDistanceHelper(GraphNode node, int distTraveled, Dictionary<GraphNode, MinDistance> minDistances)
        {
            foreach (var edge in node.Edges)
            {
                var edgeDist = distTraveled + edge.Weight;
                if (!minDistances.ContainsKey(edge.Node) || minDistances[edge.Node].Dist > edgeDist)
                {
                    minDistances[edge.Node] = new MinDistance {Dist = distTraveled + edge.Weight, Origin = minDistances[node], Node = edge.Node};
                    ShortestDistanceHelper(edge.Node, edgeDist, minDistances);
                }
            }
        }
    }

    [TestFixture]
    public class ShortestDistanceHelperTests
    {

        public IEnumerable<GraphNode> GenerateRandomGraph(int numNodes, int maxEdges, int maxWeight)
        {
            var rng = new Random();

            var list = new List<GraphNode>();

            for (var i = 0; i < numNodes; i++)
            {
                list.Add(new GraphNode(){Edges = new List<Edge>(), Id = i});
            }

            foreach (var node in list)
            {
                var edgeCount = rng.Next(1, maxWeight);
                for (var i = 0; i < edgeCount; i++)
                {
                    var randomNode = list[rng.Next(numNodes - 1)];
                    node.Edges.Add(new Edge(){Node = randomNode, Weight = rng.Next(1, maxWeight)});
                }
            }

            return list;
        }
        [Test]
        public void ShortestDistanceHelperTest()
        {
            var randomGraph = GenerateRandomGraph(1000, 500, 10).ToList();

            var shortestDist = ShortestGraphRouteFinder.FindShortestDistance(randomGraph[0], randomGraph[1]);

            var nodes = new List<GraphNode>();
            var currNode = shortestDist;

            while (currNode != null)
            {
                nodes.Add(currNode.Node);
                currNode = currNode.Origin;
            }

            nodes.Reverse();

        }
    }

    public static class TwoDimensionalWeightedPathOptimizer {

        public static IEnumerable<Direction> OptimizePath(int[,] grid)
        {
            var memo = new PathValue[grid.GetLength(0), grid.GetLength(1)];
            VisitCell(grid, memo, 0, 0, new PathValue{DirectionFromParent = Direction.Origin, Value = 0});

            var path = new List<Direction>();
            var workingNode = memo[memo.GetLength(0) - 1, memo.GetLength(1) - 1];

            while (workingNode.DirectionFromParent != Direction.Origin)
            {
                path.Add(workingNode.DirectionFromParent);
                workingNode = workingNode.Parent;
            }

            path.Reverse();
            return path;
        }

        private static void VisitCell(int[,] grid, PathValue[,] memo, int x, int y, PathValue pathValue)
        {
            if (x >= grid.GetLength(0) || y >= grid.GetLength(1))
            {
                //we've passed the edge of the grid
                return;
            }

            var value = grid[x, y] + pathValue.Value;

            if (memo[x, y] == null || value >= memo[x, y].Value)
            {          
                memo[x, y] = new PathValue(){DirectionFromParent = pathValue.DirectionFromParent, Parent = pathValue.Parent, Value = value};    
                VisitCell(grid, memo, x+1, y, new PathValue {DirectionFromParent = Direction.Right, Parent = pathValue, Value = value});
                VisitCell(grid, memo, x, y + 1, new PathValue { DirectionFromParent = Direction.Down, Parent = pathValue, Value = value });
            }
        }


        public enum Direction{
            Down, Right, Origin
        }

        private class PathValue
        {
            public int Value { get; set;}
            public Direction DirectionFromParent { get; set;}
            public PathValue Parent { get; set; }
        }
    }

    [TestFixture]
    public class TwoDimensionalWeightedPathOptimizerTests
    {

        [Test]
        public void TwoDimensionalWeightedPathOptimizerTest()
        {
            var grid = new [,] { { 1, 2, 3, 4, 5}, { 9, 8, 7, 6, 5 }, { 10, 10, 10, 1, 10 }, { 1, 2, 3, 4, 5} };
            var path = TwoDimensionalWeightedPathOptimizer.OptimizePath(grid);
        }

    }

    

}
