using System.Data;

namespace TablesAutomation.E2EFramework.Utils
{
    public static class DataRowPartialComparer
    {
        public static void ShouldContainRows(this DataTable actualTable, DataTable expectedTable)
        {
            var expectedTableColumns = expectedTable.Columns;

            foreach (var row in expectedTable.AsEnumerable())
            {
                foreach (DataColumn column in expectedTableColumns)
                {
                    var expectedColumnName = column.ColumnName;
                    var expectedValue = row[expectedColumnName];
                    var rowsFound = actualTable.AsEnumerable().Where(row => row[expectedColumnName].ToString().Equals(expectedValue)); //We are operating with string-type expected values only currently

                    if (!rowsFound.Any())
                    {
                        throw new InvalidOperationException($"Couldn't find following row in Data Table: {Environment.NewLine}{string.Join(", ", row.ItemArray)}. " +
                            $"{Environment.NewLine} Check column: {expectedColumnName}");
                    }
                }

            }
        }

        public static void ShouldNotContainRows(this DataTable actualTable, DataTable expectedTable)
        {
            var expectedTableColumns = expectedTable.Columns;

            foreach (var row in expectedTable.AsEnumerable())
            {

                var rowsFound = actualTable.AsEnumerable();

                foreach (DataColumn column in expectedTableColumns)
                {
                    var expectedColumnName = column.ColumnName;
                    var expectedValue = row[expectedColumnName];
                    rowsFound = rowsFound.Where(row => row[expectedColumnName].ToString().Equals(expectedValue)); //We are operating with string-type expected values only currently                    
                    if (!rowsFound.Any()) break; //If rowsFound is empty at some iteration, no need to continue checking columns, jump to the next row.
                }

                if (rowsFound.Any())
                {
                    throw new InvalidOperationException($"Following row found in Data Table: {Environment.NewLine}{string.Join(", ", row.ItemArray)}.");
                }
            }
        }
    }
}
