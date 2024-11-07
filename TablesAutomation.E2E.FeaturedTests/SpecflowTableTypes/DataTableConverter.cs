using System.Data;
using TechTalk.SpecFlow;

namespace TablesAutomation.E2EFeaturedTests.SpecflowTableTypes
{
    public static class DataTableConverter
    {
        public static DataTable ToDataTable(this Table table)
        {
            var dataTable = new DataTable();
            foreach (var header in table.Header)
            {
                dataTable.Columns.Add(header, typeof(string));
            }

            foreach (var row in table.Rows)
            {
                var newRow = dataTable.NewRow();
                foreach (var header in table.Header)
                {
                    newRow.SetField(header, row[header]);
                }
                dataTable.Rows.Add(newRow);
            }
            return dataTable;
        }
    }
}
