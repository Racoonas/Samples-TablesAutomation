using NUnit.Framework;
using NUnit.Framework.Internal;
using TablesAutomation.E2EFramework.Utils;

namespace TablesAutomation.E2E.UnitTests
{
    [TestFixture]
    public class CustomCollectionAssertTests
    {
        [Test]
        public void SortAscendingListOfIntegersTest()
        {
            var collection = new List<int> { 1, 2, 4, 7 };
            CustomCollectionAssert.AssertCollectionSorted(collection, "ascending", Comparer<int>.Default);
        }

        [Test]
        public void SortDescendingListOfIntegersTest()
        {
            var collection = new List<int> { 10, 7, 4, 1 };
            CustomCollectionAssert.AssertCollectionSorted(collection, "descending", Comparer<int>.Default);
        }

        [Test]
        public void SortAscendingListOfStringsWithOrdinalIgnoreCaseTest()
        {
            var collection = new List<string> { " ", "#", "-", "1", "2", "Bob", "Dylan" };
            CustomCollectionAssert.AssertCollectionSorted(collection, "ascending", StringComparer.OrdinalIgnoreCase);
        }
        
        [Test]
        public void SortDescendingListOfStringsWithInvariantCultureIgnoreCaseTest()
        {
            var collection = new List<string> { "Dylan", "Bob", "2", "1", "#", "-", " " };
            CustomCollectionAssert.AssertCollectionSorted(collection, "descending", StringComparer.InvariantCultureIgnoreCase);
        }

        [Test]
        public void SortWithIncorrectOrderThrowsExceptionTest()
        {
            var collection = new List<int> { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>(
                () => CustomCollectionAssert.AssertCollectionSorted(collection, "abracadabra", Comparer<int>.Default)
             );
        }

        /// <summary>
        /// An example of how String comparer may change the expected sorting behavior. Notice the special symbols placement
        /// </summary>
        [Test]
        public void SortDescendingListOfStringsWithOrdinalIgnoreCaseTest()
        {
            var collection = new List<string> { "Dylan", "Bob", "2", "1", "-", "#", " " };
            CustomCollectionAssert.AssertCollectionSortedDescending(collection, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// An example of how String comparer may change the expected sorting behavior. Notice the special symbols placement
        /// </summary>
        [Test]
        public void SortAscendingListOfStringsWithInvariantCultureIgnoreCaseTest()
        {
            var collection = new List<string> { " ", "-", "#", "1", "2", "Bob", "Dylan" };
            CustomCollectionAssert.AssertCollectionSortedAscending(collection, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
