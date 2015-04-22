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

            var linkedList = new LinkedList<Flower>();
            

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

       

        public class Flower
        {
            public int Height { get; set; }
            public int Bloom { get; set; }
            public int Wilt { get; set; }

            public bool IsOverlapping(Flower other)
            {
                return (other.Bloom <= this.Wilt && other.Bloom >= this.Bloom) || (other.Wilt >= this.Bloom && other.Wilt <= this.Wilt);
            }
        }

       
    }

    [TestFixture]
    public class FlowerGardenTests
    {

        [Test]
        public void FlowerGardenTest()
        {
            var height = new[] {5, 4, 3, 2, 1};
            var bloom = new[] { 1, 5, 10, 15, 20 };
            var wilt = new[] { 5, 10, 14, 20, 25 };

            var ordered = FlowerGarden.GetOrdering(height, bloom, wilt);

            Assert.AreEqual(ordered, new[] { 3, 4, 5, 1, 2 });
        }
    }
}
