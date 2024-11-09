using Microsoft.Playwright;
using System.Data;

namespace TablesAutomation.E2EFramework.PageComponents
{
    public class ItemsTable
    {
        private IPage _page;
        private ILocator _tableContainer;

        public ItemsTable(IPage page, ILocator container)
        {
            _page = page;
            _tableContainer = container;
        }
             
        /// <summary>
        /// Read the table from the page, convert to DataTable
        /// </summary>
        /// <returns></returns>
        public async Task<DataTable> AsDataTable()
        {
            var dataTable = new DataTable();

            var headers = await GetHeaders();
            var rows = await GetAllRowValues();

            foreach (var header in headers)
            {
                dataTable.Columns.Add(header);
            }

            foreach (var row in rows)
            {
                dataTable.Rows.Add(row.Select(val => val.ToString()).ToArray());
            }

            return dataTable;
        }

        /// <summary>
        /// Find the header with the specified text and click it
        /// </summary>
        /// <param name="columnHeader"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task ClickColumnHeader(string columnHeaderText)
        {
            var headers = await _tableContainer.Locator($"//th[.//*[text()=' {columnHeaderText} ']]").AllAsync();
            
            if (!headers.Any())
            {
                throw new ArgumentException($"Couldn't find any table headers with text: {columnHeaderText}");
            }

            var tableHeader = _tableContainer.Locator($"//th[.//*[text()=' {columnHeaderText} ']]");            
            await tableHeader.ClickAsync();
        }

        /// <summary>
        /// Get values from the column with specified header
        /// </summary>
        /// <param name="columnHeader"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IReadOnlyList<string>> GetColumnValues(string columnHeader)
        {            
            var columnHeaders = await _tableContainer.Locator($"//th").AllAsync();

            var columnIndex = 1; //We're using xpath for locating and clicking, thus - start counting from 1
            var exists = false;

            //Calculating index of the column
            foreach (var header in columnHeaders)
            {
                if (await header.Locator($"//*[text()=' {columnHeader} ']").IsVisibleAsync())
                {
                    exists = true;
                    break;
                }

                columnIndex++;
            }

            if (!exists) throw new ArgumentException($"Couldn't find any table headers with text: {columnHeader}");

            //Extracting texts from the column using the calculated index:
            var rows = await _tableContainer.Locator($"//tbody//td[{columnIndex}]").AllTextContentsAsync();

            return rows;
        }

        /// <summary>
        /// Return a list of table headers
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<string>> GetHeaders()
        {
            var tableHead = _tableContainer.Locator("thead");
            var headers = await tableHead.Locator($"//th").AllInnerTextsAsync();

            return headers;
        }

        /// <summary>
        /// Simply get row by index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns>ILocator row</returns>
        public ILocator GetRow(int rowIndex)
        {
            return _tableContainer.Locator($"//tbody//tr[{rowIndex}]");
        }

        /// <summary>
        /// Get row by multiple text parameters (each text parameter should be present in the row)
        /// </summary>
        /// <param name="rowValues"></param>
        /// <returns>ILocator row</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ILocator> GetRow(params string[] rowValues)
        {
            var rows = await _tableContainer.Locator($"//tr[.//*[text()='{rowValues.First()}']]").AllAsync();

            for (int i = 1; i < rowValues.Count(); i++)
            {
                //filtering rows using row values params
                rows = rows.Where(r => r.Locator($"//*[text()='{rowValues[i]}']").AllAsync().Result.Any()).ToList();
            }

            if (!rows.Any())
            {
                throw new ArgumentException($"Couldn't find any rows with rowValues: {string.Join("; ", rowValues)}");
            }

            if (rows.Count != 1)
            {
                throw new ArgumentException($"Found {rows.Count} rows with rowValues: {string.Join("; ", rowValues)}");
            }

            return rows.First();
        }

        /// <summary>
        /// Returns values from all rows
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<string[]>> GetAllRowValues()
        {

            var result = new List<string[]>();
            var rows = await _tableContainer.Locator("//tbody//tr").AllAsync();

            foreach (var row in rows)
            {
                var rowData = await row.AllInnerTextsAsync();
                result.Add(rowData.First().Split("\t"));
            }

            return result;
        }

        /// <summary>
        /// Returns values from the specified row
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<string>> GetRowValues(int index)
        {            
            var row = _tableContainer.Locator("//tbody//tr").Nth(index);
            var rowData = await row.AllInnerTextsAsync();

            return rowData.First().Split("\t").ToList();
        }
    }
}
