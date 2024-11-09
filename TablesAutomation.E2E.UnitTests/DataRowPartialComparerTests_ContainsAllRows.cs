using System.Data;
using NUnit.Framework;
using TablesAutomation.E2EFramework.Utils;

namespace TablesAutomation.E2E.UnitTests
{
    [TestFixture]
    public class DataRowPartialComparerTests_ContainsAllRows
    {
        
        [Test]
        public void ShouldThrowErrorIfDataTableDoesNotContainRows()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");
            tableExpected.Rows.Add("1", "Alice");
            tableExpected.Rows.Add("2", "Bob");            
            tableExpected.Rows.Add("4", "James");            

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add("1", "Alice","Cooper");            
            tableActual.Rows.Add("2", "Bob", "Hoover");
            tableActual.Rows.Add("3", "Charlie", "Poole");

            InvalidOperationException invalidOperationException = Assert.Throws<InvalidOperationException>(()=> tableActual.ShouldContainRows(tableExpected));
        }

        [Test]
        public void ShouldThrowErrorIfDataTableHasUnknownColumn()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");
            tableExpected.Columns.Add("Address");
            tableExpected.Rows.Add("1", "Alice", "Wonderland str.");
            tableExpected.Rows.Add("2", "Bob",  "Cloud Kingdom str.");            

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add("1", "Alice", "Cooper");
            tableActual.Rows.Add("2", "Bob", "Hoover");           

            Assert.Throws<ArgumentException>(() => tableActual.ShouldContainRows(tableExpected));
        }

        [Test]
        public void ShouldThrowErrorIfDataTableHasUnexpectedDuplicatesInAnyField()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");            
            tableExpected.Rows.Add("1", "Alice");
            tableExpected.Rows.Add("2", "Bob");
            tableExpected.Rows.Add("3", "Bob");

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add("1", "Alice", "Cooper");
            tableActual.Rows.Add("2", "Bob", "Hoover");

            Assert.Throws<InvalidOperationException>(() => tableActual.ShouldContainRows(tableExpected));
        }

        [Test]
        public void DataTableExactEqualityShouldNotThrowErrors()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");            
            tableExpected.Columns.Add("FirstName");
            tableExpected.Columns.Add("LastName");
            tableExpected.Rows.Add("1", "Alice", "Cooper");
            tableExpected.Rows.Add("2", "Bob", "Hoover");
            tableExpected.Rows.Add("3", "Charlie", "Poole");            

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add("1", "Alice", "Cooper");
            tableActual.Rows.Add("2", "Bob", "Hoover");
            tableActual.Rows.Add("3", "Charlie", "Poole");            

            tableActual.ShouldContainRows(tableExpected);
        }

        [Test]
        public void DataTablePartialEqualityWithStringOnlyRowsShouldNotThrowErrors()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");
            tableExpected.Rows.Add("1", "Alice");
            tableExpected.Rows.Add("2", "Bob");            

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add("1", "Alice", "Cooper");
            tableActual.Rows.Add("2", "Bob", "Hoover");            

            tableActual.ShouldContainRows(tableExpected);
        }

        [Test]
        public void DataTablePartialEqualityWithMixedTypeRowsShouldNotThrowErrors()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");
            tableExpected.Rows.Add("1", "Alice");
            tableExpected.Rows.Add(2, "Bob");
            tableExpected.Rows.Add(3, "Stephen");            

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add(1, "Alice", "Cooper");
            tableActual.Rows.Add("2", "Bob", "Hoover");
            tableActual.Rows.Add(3, "Stephen", "King");            

            tableActual.ShouldContainRows(tableExpected);
        }

        [Test]
        public void DataTablePartialEqualityWithDoubleTypeRowsShouldNotThrowErrors()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");
            tableExpected.Rows.Add("100.01", "Alice");
            tableExpected.Rows.Add("200", "Bob");            

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add(100.01, "Alice", "Cooper");
            tableActual.Rows.Add(200.00, "Bob", "Hoover");            

            tableActual.ShouldContainRows(tableExpected);
        }
    }
}
