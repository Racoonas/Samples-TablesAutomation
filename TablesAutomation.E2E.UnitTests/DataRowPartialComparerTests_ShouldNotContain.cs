using System.Data;
using NUnit.Framework;
using TablesAutomation.E2EFramework.Utils;

namespace TablesAutomation.E2E.UnitTests
{
    [TestFixture]
    public class DataRowPartialComparerTests_ShouldNotContain
    {
        [Test]
        public void DataTableDoesNotContainsRows_FullColumnsMatch()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");
            tableExpected.Columns.Add("LastName");
            tableExpected.Rows.Add("100", "Alice", "Coopa");
            tableExpected.Rows.Add("200", "Bob", "Hoova");

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add("100", "Alice", "Cooper");
            tableActual.Rows.Add("200", "Bob", "Hoover");

            tableActual.ShouldNotContainRows(tableExpected);
        }

        [Test]
        public void DataTableDoesNotContainsRows_PartialColumnsMatch()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");            
            tableExpected.Rows.Add("100", "Alise");
            tableExpected.Rows.Add("200", "Bub");

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add("100", "Alice", "Cooper");
            tableActual.Rows.Add("200", "Bob", "Hoover");

            tableActual.ShouldNotContainRows(tableExpected);
        }

        [Test]
        public void DataTableDoesNotContainsRows_UnknownColumn()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");
            tableExpected.Columns.Add("Address");
            tableExpected.Rows.Add("100", "Alice", "DreamLand str.");
            tableExpected.Rows.Add("200", "Bob", "Nightmares str.");

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add("100", "Alice", "Cooper");
            tableActual.Rows.Add("200", "Bob", "Hoover");

            Assert.Throws<ArgumentException>(() => tableActual.ShouldNotContainRows(tableExpected));
        }

        [Test]
        public void ShouldThrowErrorIfDataTableContainsRows_IntToString_PartialColumnsMatch()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");
            tableExpected.Rows.Add(100, "Alice");

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add("100", "Alice", "Cooper");
            tableActual.Rows.Add("200", "Bob", "Hoover");

            Assert.Throws<InvalidOperationException>(() => tableActual.ShouldNotContainRows(tableExpected));
        }

        [Test]
        public void ShouldThrowErrorIfDataTableContainsRows_StringToString_FullColumnsMatch()
        {
            DataTable tableExpected = new DataTable();
            tableExpected.Columns.Add("Id");
            tableExpected.Columns.Add("FirstName");
            tableExpected.Columns.Add("LastName");
            tableExpected.Rows.Add("100", "Alice", "Cooper");

            DataTable tableActual = new DataTable();
            tableActual.Columns.Add("Id");
            tableActual.Columns.Add("FirstName");
            tableActual.Columns.Add("LastName");
            tableActual.Rows.Add("100", "Alice", "Cooper");
            tableActual.Rows.Add("200", "Bob", "Hoover");

            Assert.Throws<InvalidOperationException>(() => tableActual.ShouldNotContainRows(tableExpected));            
        }


        
    }
}
