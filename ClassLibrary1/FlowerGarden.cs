using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ClassLibrary1
{
    public static class FlowerGarden
    {
        //http://community.topcoder.com/stat?c=problem_statement&pm=1918&rd=5006



        public static int[] GetOrdering(int[] height, int[] bloom, int[] wilt)
        {
            var flowers = new List<Flower>();
            for (var i = 0; i < height.Length; i++)
            {
                flowers.Add(new Flower {Height = height[i], Bloom = bloom[i], Wilt = wilt[i]});
            }

            var sorted = flowers.OrderBy(f => f.Height).ToArray();


            for (var i = sorted.Length-1; i > 0; i--)
            {
                for (var j = i; j >= 1 && !sorted[j].IsOverlapping(sorted[j-1]); j--)
                {
                    var temp = sorted[j];
                    sorted[j] = sorted[j - 1];
                    sorted[j - 1] = temp;
                }
               
            }

            return sorted.Select(f => f.Height).ToArray();
        }

        public static int[] GetOrdering2(int[] height, int[] bloom, int[] wilt)
        {
            var flowers = new List<Flower>();
            for (var i = 0; i < height.Length; i++)
            {
                flowers.Add(new Flower { Height = height[i], Bloom = bloom[i], Wilt = wilt[i] });
            }

            var sorted = flowers.OrderBy(f => f.Height).ToArray();


            var linkedList = new DoublyLinkedList<Flower>();

            foreach (var flower in sorted)
            {
                linkedList.Append(flower);
            }

            var currNode = linkedList.Root;
            var linkedListNodes = new List<LinkedListNode<Flower>>();

            while (currNode != null)
            {
                linkedListNodes.Add(currNode);
                currNode = currNode.Next;
            }

            linkedListNodes.Reverse();

            foreach (var node in linkedListNodes)
            {
                var swapNode = node;
                var cursorNode = node.Prev;
                while (cursorNode != null && !cursorNode.Value.IsOverlapping(node.Value) && cursorNode.Value.Height < node.Value.Height)
                {
                    swapNode = swapNode.Prev;
                    cursorNode = cursorNode.Prev;
                }

                linkedList.PlaceBefore(node, swapNode);
            }

            var finalList = new List<Flower>();
            currNode = linkedList.Root;

            while (currNode != null)
            {
                finalList.Add(currNode.Value);
                currNode = currNode.Next;
            }
           

            return finalList.Select(f => f.Height).ToArray();
        }

        public static int[] GetOrdering3(int[] height, int[] bloom, int[] wilt)
        {
            int[] optimal = new int[height.Length];
            int[] optimalBloom = new int[bloom.Length];
            int[] optimalWilt = new int[wilt.Length];

            // init state
            optimal[0] = height[0];
            optimalBloom[0] = bloom[0];
            optimalWilt[0] = wilt[0];

            // run dynamic programming
            for (int i = 1; i < height.Length; i++)
            {
                int currHeight = height[i];
                int currBloom = bloom[i];
                int currWilt = wilt[i];

                int offset = 0; // by default, type i is to be put to 1st row
                for (int j = 0; j < i; j++)
                {
                    if (currWilt >= optimalBloom[j] && currWilt <= optimalWilt[j] ||
                            currBloom >= optimalBloom[j] && currBloom <= optimalWilt[j] ||
                            currWilt >= optimalWilt[j] && currBloom <= optimalBloom[j])
                    {  // life period overlap
                        if (currHeight < optimal[j])
                        {  // life overlap, and type i is shorter than type j
                            offset = j;
                            break;
                        }
                        else
                        {
                            offset = j + 1; // type i overlap with type j, and i is taller than j. Put i after j
                        }
                    }
                    else
                    {  // not overlap with current
                        if (currHeight < optimal[j])
                        {
                            offset = j + 1; // type i not overlap with j, i is shorter than j, put i after j
                        }
                        // else keep offset as is considering offset is smaller than j
                    }
                }

                // shift the types after offset
                for (int k = i - 1; k >= offset; k--)
                {
                    optimal[k + 1] = optimal[k];
                    optimalBloom[k + 1] = optimalBloom[k];
                    optimalWilt[k + 1] = optimalWilt[k];
                }
                // update optimal
                optimal[offset] = currHeight;
                optimalBloom[offset] = currBloom;
                optimalWilt[offset] = currWilt;
            }
            return optimal;
        }

        public static int[] GetOrdering4(int[] height, int[] bloom, int[] wilt)
        {
            var flowers = new List<Flower>();
            for (var i = 0; i < height.Length; i++)
            {
                flowers.Add(new Flower { Height = height[i], Bloom = bloom[i], Wilt = wilt[i] });
            }

            var linkedList = new DoublyLinkedList<Flower>();

            linkedList.Append(flowers.First());

            for (var i = 1; i < flowers.Count; i++)
            {
                LinkedListNode<Flower> position = linkedList.Root;
                var placement = PlacementDirection.Before;
                LinkedListNode<Flower> cursor = linkedList.Root;
                var flower = flowers[i];
                var node = new LinkedListNode<Flower>() {Value = flower};

                while (cursor != null)
                {
                    if (cursor.Value.IsOverlapping(flower))
                    {
                        if (flower.Height < cursor.Value.Height)
                        {
                            placement = PlacementDirection.Before;
                            position = cursor;
                            break;
                        }
                        else
                        {
                            placement = PlacementDirection.After;
                            position = cursor;
                        }
                    }

                    if (flower.Height < cursor.Value.Height)
                    {
                        placement = PlacementDirection.After;
                        position = cursor;
                    }

                    cursor = cursor.Next;
                }

                if (placement == PlacementDirection.After)
                {
                    linkedList.PlaceAfter(node, position);
                }
                else
                {
                    linkedList.PlaceBefore(node, position);
                }

                
            }

            var output = new List<int>();
            var curr = linkedList.Root;

            while (curr != null)
            {
                output.Add(curr.Value.Height);
                curr = curr.Next;
            }

            return output.ToArray();
        }


        public enum PlacementDirection
        {
            Before, After
        }

       



        public class Flower
        {
            public int Height { get; set; }
            public int Bloom { get; set; }
            public int Wilt { get; set; }

            public bool IsOverlapping(Flower other)
            {
                return (other.Bloom <= this.Wilt && other.Bloom >= this.Bloom) || (other.Wilt >= this.Bloom && other.Wilt <= this.Wilt) || other.Bloom <= this.Bloom && other.Wilt >= this.Wilt;
            }
        }

       
    }

    [TestFixture]
    public class FlowerGardenTests
    {

        [Test]
        public void FlowerGardenTest()
        {
            var height = new[] { 3, 2, 5, 4 };
            var bloom = new[] { 1, 2, 11, 10 };
            var wilt = new[] {4, 3, 12, 13};


            var ordered = FlowerGarden.GetOrdering4(height, bloom, wilt);

            Assert.AreEqual(ordered, new[] { 4,  5,  2,  3 });
        }
    }
}
