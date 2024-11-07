using NUnit.Framework;

namespace TablesAutomation.E2EFramework.Utils
{
    public class CustomCollectionAssert
    {
        public static void AssertCollectionSortedAscending<T>(IEnumerable<T> collection, IComparer<T> comparer)
        {
            AssertCollectionSorted(collection, "ascending", comparer);
        }

        public static void AssertCollectionSortedDescending<T>(IEnumerable<T> collection, IComparer<T> comparer)
        {
            AssertCollectionSorted(collection, "descending", comparer);
        }

        public static void AssertCollectionSorted<T>(IEnumerable<T> collection, string order, IComparer<T> comparer)
        {
            switch (order)
            {
                case "ascending":
                    Assert.That(collection, Is.Ordered.Ascending.Using(comparer));
                    break;
                case "descending":
                    Assert.That(collection, Is.Ordered.Descending.Using(comparer));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(order), $"Unkown order: {order}. Please use 'ascending' or 'descending'");
            }
        }
    }
}