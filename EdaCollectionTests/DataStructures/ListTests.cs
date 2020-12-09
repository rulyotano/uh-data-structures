using Bogus;
using EdaCollection.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdaCollectionTests.DataStructures
{
    [TestClass]
    public class ListTests
    {
        [TestMethod("Should create the list empty")]
        public void ShouldCreateEmpty()
        {
            var list = new List<int>();

            Assert.AreEqual(0, list.Count);
        }

        [TestMethod("Should add items (add method)")]
        public void ShouldAddItems()
        {
            var fakeItems = new int[] { 1, 2, 3 };
            var list = new List<int>();

            for (int i = 0; i < fakeItems.Length; i++)
            {
                list.Add(fakeItems[i]);
                Assert.AreEqual(i + 1, list.Count);
                Assert.AreEqual(fakeItems[i], list[i]);
            }
        }

        [TestMethod("Should delete items (remove method) last item")]
        public void ShouldDeleteItems()
        {
            const string insertedItem = "fake-item";
            var list = new List<string>();

            list.Add(insertedItem);
            Assert.AreEqual(1, list.Count);

            list.Remove(insertedItem);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod("Should delete items (remove method) no last item")]
        public void ShouldDeleteItemsNoLatest()
        {
            const string insertedItem1 = "fake-item-1";
            const string insertedItem2 = "fake-item-2";
            var list = new List<string>();

            list.Add(insertedItem1);
            list.Add(insertedItem2);
            Assert.AreEqual(2, list.Count);

            list.Remove(insertedItem2);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(insertedItem1, list[0]);
        }
    }
}
