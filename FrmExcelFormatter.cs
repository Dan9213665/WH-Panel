using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OfficeOpenXml;
using OfficeOpenXml.Core.ExcelPackage;
using OfficeOpenXml.Style;
using System.IO;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
namespace WH_Panel
{
    public partial class FrmExcelFormatter : Form
    {
        private ToolTip toolTip; // Declare a single ToolTip instance
        public FrmExcelFormatter()
        {
            InitializeComponent();
            toolTip = new ToolTip();
        }
        private void FrmExcelFormatter_Load(object sender, EventArgs e)
        {
            //AttachRightClickHandlerToDataGridViews(this);
            toolTip.SetToolTip(btnGetSourceFile, "1_Get Source FIle");
            toolTip.SetToolTip(btnSetDataHeader, "2_Process file");
            toolTip.SetToolTip(button1, "3_Set Order");
            toolTip.SetToolTip(btnSaveAs, "4_Save XLSX");
            btnGetSourceFile_Click(btnGetSourceFile, EventArgs.Empty);
        }
        private ContextMenuStrip columnHeaderContextMenu;
        public string clientName = string.Empty;
        public string projectName = string.Empty;
        public string filePath = string.Empty;
        List<string> desiredColumnOrder = new List<string> { "CC", "#", "IPN", "MFPN", "Description", "Qty", "Calc", "Alts", "Manufacturer","Footprint", "RefDes" /*, Add more column names here */ };
        private void btnGetSourceFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Select an Excel File";
                openFileDialog.InitialDirectory = "\\\\dbr1\\Data\\Aegis_NPI_Projects";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    string[] pathParts = filePath.Split('\\'); // Split by directory separator
                    // Assuming the client name is in the third part (index 2) of the path
                    clientName = pathParts[5];
                    projectName = pathParts[6];
                    //MessageBox.Show(clientName+" "+projectName);
                    string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties=\"Excel 12.0 Xml;HDR=YES;\"";
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (schemaTable != null && schemaTable.Rows.Count > 0)
                        {
                            tabControl1.TabPages.Clear(); // Clear existing tab pages
                            foreach (DataRow row in schemaTable.Rows)
                            {
                                string sheetName = row["TABLE_NAME"].ToString();
                                DataTable dataTable = new DataTable();
                                using (OleDbDataAdapter adapter = new OleDbDataAdapter($"SELECT * FROM [{sheetName}]", connection))
                                {
                                    adapter.Fill(dataTable);
                                }
                                if (dataTable.Rows.Count > 0)
                                {
                                    DataGridView dataGridView = new DataGridView();
                                    dataGridView.Dock = DockStyle.Fill; // Set the Dock property to Fill
                                    dataGridView.AllowUserToOrderColumns = true; // Enable column reordering
                                    dataGridView.DataSource = dataTable;
                                    TabPage tabPage = new TabPage(sheetName);
                                    AttachContextMenuToDataGridView(dataGridView);
                                    tabPage.Controls.Add(dataGridView);
                                    tabControl1.TabPages.Add(tabPage); // Add the tab page to the TabControl
                                }
                            }
                            if (tabControl1.TabPages.Count == 0)
                            {
                                MessageBox.Show("No data found in any Excel sheet.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No sheets found in the Excel file.");
                        }
                    }
                }
            }
            AttachRightClickHandlerToDataGridViews(this);
        }
        private void AttachContextMenuToDataGridView(DataGridView dataGridView)
        {
            columnHeaderContextMenu = new ContextMenuStrip();
            // Add items to the context menu
            foreach (string columnName in desiredColumnOrder)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(columnName);
                menuItem.Click += ContextMenuItem_Click; // Attach click event handler
                columnHeaderContextMenu.Items.Add(menuItem);
            }
            // Attach the context menu to the ColumnHeaderMouseClick event
            dataGridView.ColumnHeaderMouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right && e.RowIndex < 0 && e.ColumnIndex >= 0)
                {
                    DataGridViewColumn column = dataGridView.Columns[e.ColumnIndex];
                    columnHeaderContextMenu.Tag = column; // Set the clicked column as the Tag
                    columnHeaderContextMenu.Show(dataGridView, dataGridView.PointToClient(Cursor.Position));
                }
            };
        }
        private void ContextMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            string selectedHeaderText = menuItem.Text;
            // Get the clicked column from the Tag property
            DataGridViewColumn clickedColumn = columnHeaderContextMenu.Tag as DataGridViewColumn;
            if (clickedColumn != null)
            {
                clickedColumn.HeaderText = selectedHeaderText;
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void DataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
        }
        private void AttachRightClickHandlerToDataGridViews(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                if (childControl is DataGridView dataGridView)
                {
                    dataGridView.ColumnHeaderMouseClick += DataGridView_ColumnHeaderMouseClick;
                    AttachContextMenuToDataGridView(dataGridView);
                }
                else
                {
                    AttachRightClickHandlerToDataGridViews(childControl);
                }
            }
        }
        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex < 0 && e.ColumnIndex >= 0)
            {
                DataGridView dataGridView = sender as DataGridView;
                DataGridViewColumn column = dataGridView.Columns[e.ColumnIndex];
                columnHeaderContextMenu.Show(dataGridView, dataGridView.PointToClient(Cursor.Position));
            }
        }
        private void btnSetDataHeader_Click(object sender, EventArgs e)
        {
            runFormattingLogic();
        }
        private void runFormattingLogic()
        {
            if (tabControl1.TabPages.Count > 0)
            {
                TabPage currentTab = tabControl1.SelectedTab;
                DataGridView currentGridView = currentTab.Controls.OfType<DataGridView>().FirstOrDefault();
                if (currentGridView != null)
                {
                    string tabLabel = DateTime.Now.ToString("yyyyMMddHHmm");
                    TabPage newTab = new TabPage(tabLabel);
                    tabControl1.TabPages.Add(newTab);
                    DataGridView newGridView = new DataGridView();
                    newGridView.Dock = DockStyle.Fill;
                    int startRowIndex = FindStartRowIndex(currentGridView);
                    int endRowIndex = FindEndRowIndex(currentGridView);
                    // Clone columns from the current DataGridView
                    foreach (DataGridViewColumn column in currentGridView.Columns)
                    {
                        newGridView.Columns.Add(column.Clone() as DataGridViewColumn);
                    }
                    for (int rowIndex = startRowIndex; rowIndex <= endRowIndex; rowIndex++)
                    {
                        DataGridViewRow row = currentGridView.Rows[rowIndex];
                        // Check if all cells in the row are empty
                        bool isRowEmpty = true;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                isRowEmpty = false;
                                break;
                            }
                        }
                        if (!isRowEmpty) // Exclude the header row and empty rows
                        {
                            DataGridViewRow newRow = (DataGridViewRow)row.Clone();
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                newRow.Cells[cell.ColumnIndex].Value = cell.Value;
                            }
                            newGridView.Rows.Add(newRow);
                        }
                    }
                    RemoveEmptyAndCCColumns(newGridView);
                    newGridView.AllowUserToOrderColumns = true; // Enable column reordering
                    newTab.Controls.Add(newGridView);
                    AttachRightClickHandlerToDataGridViews(this);
                    tabControl1.SelectedTab = newTab;
                }
            }
        }
        private void RemoveEmptyAndCCColumns(DataGridView dataGridView)
        {
            List<DataGridViewColumn> columnsToRemove = new List<DataGridViewColumn>();
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.HeaderText == "CC")
                {
                    columnsToRemove.Add(column);
                }
            }
            foreach (DataGridViewColumn column in columnsToRemove)
            {
                dataGridView.Columns.Remove(column);
            }
        }
        private int FindStartRowIndex(DataGridView gridView)
        {
            for (int rowIndex = 0; rowIndex < gridView.Rows.Count; rowIndex++)
            {
                DataGridViewRow row = gridView.Rows[rowIndex];
                int emptyCellCount = 0;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        emptyCellCount++;
                    }
                }
                if (emptyCellCount < row.Cells.Count / 3) // Adjust this threshold as needed
                {
                    return rowIndex;
                }
            }
            return 0; // Default to starting from the first row
        }
        private int FindEndRowIndex(DataGridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 1; rowIndex >= 0; rowIndex--)
            {
                DataGridViewRow row = gridView.Rows[rowIndex];
                int emptyCellCount = 0;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        emptyCellCount++;
                    }
                }
                if (emptyCellCount < row.Cells.Count / 2) // Adjust this threshold as needed
                {
                    return rowIndex;
                }
            }
            return gridView.Rows.Count - 1; // Default to ending with the last row
        }
        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count > 0)
            {
                TabPage lastTab = tabControl1.TabPages[tabControl1.TabPages.Count - 1];
                DataGridView lastGridView = lastTab.Controls.OfType<DataGridView>().FirstOrDefault();
                lastGridView.Refresh();
                if (lastGridView != null)
                {
                    string tabLabel = DateTime.Now.ToString("yyyyMMddHHmm");
                    // Prompt the user to input quantity
                    string inputQuantity = Microsoft.VisualBasic.Interaction.InputBox("Please enter quantity:", "Quantity", "");
                    if (string.IsNullOrEmpty(inputQuantity))
                    {
                        return; // User cancelled or left the input empty
                    }
                    int quantity;
                    if (!int.TryParse(inputQuantity, out quantity) || quantity <= 0)
                    {
                        MessageBox.Show("Invalid quantity entered. Please enter a positive integer.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Invalid input
                    }
                    // Modify the file name to include the quantity
                    string filePathToSave = $"C:/1/_BOM/{clientName}_{projectName}_{quantity}PCS.xlsx";
                    // Original file path (replace this with the actual path to your template file)
                    string originalFilePath = filePath;
                    // Create a copy of the original file
                    //File.Copy(originalFilePath, filePathToSave, true);
                    // Open the copied Excel package
                    using (var package = new OfficeOpenXml.ExcelPackage(new FileInfo(filePathToSave)))
                    {
                        // Determine the index of the last worksheet
                        int lastWorksheetIndex = package.Workbook.Worksheets.Count;
                        // Add a new worksheet after the last one
                        OfficeOpenXml.ExcelWorksheet newWorksheet = package.Workbook.Worksheets.Add(tabLabel);
                        package.Workbook.Worksheets.MoveAfter(newWorksheet.Name, package.Workbook.Worksheets[lastWorksheetIndex].Name);
                        // Add a new worksheet
                        //OfficeOpenXml.ExcelWorksheet newWorksheet = package.Workbook.Worksheets.Add(tabLabel, package.Workbook.Worksheets[1]);
                        // Copy headers to the new worksheet
                        for (int columnIndex = 0; columnIndex < lastGridView.Columns.Count; columnIndex++)
                        {
                            newWorksheet.Cells[1, columnIndex + 1].Value = lastGridView.Columns[columnIndex].HeaderText;
                        }
                        // Write data rows to the new worksheet
                        for (int rowIndex = 0; rowIndex < lastGridView.Rows.Count; rowIndex++)
                        {
                            DataGridViewRow row = lastGridView.Rows[rowIndex];
                            for (int columnIndex = 0; columnIndex < row.Cells.Count; columnIndex++)
                            {
                                newWorksheet.Cells[rowIndex + 2, columnIndex + 1].Value = row.Cells[columnIndex].Value;
                                ExcelRange cell = newWorksheet.Cells[rowIndex + 2, columnIndex + 1];
                                //cell.Value = row.Cells[columnIndex].Value;
                                // Check if the column is "#" or "Qty"
                                // Convert the value to a number if it's in the "#" column
                                if (lastGridView.Columns[columnIndex].HeaderText == "#")
                                {
                                    if (row.Cells[columnIndex].Value != null && int.TryParse(row.Cells[columnIndex].Value.ToString(), out int numericValue))
                                    {
                                        cell.Value = numericValue;
                                    }
                                    else
                                    {
                                        cell.Value = row.Cells[columnIndex].Value;
                                    }
                                }
                                else
                                {
                                    cell.Value = row.Cells[columnIndex].Value;
                                }
                                if (lastGridView.Columns[columnIndex].HeaderText == "#"|| lastGridView.Columns[columnIndex].HeaderText == "Qty")
                                {
                                    cell.Style.Numberformat.Format = "0"; // Set numeric format
                                }
                            }
                            // Apply autofilters to the header row
                            newWorksheet.Cells[1, 1, 1, lastGridView.Columns.Count].AutoFilter = true;
                        }
                        // Save the Excel package
                        package.Save();
                    }
                    // Open the containing folder
                    string containingFolder = Path.GetDirectoryName(filePathToSave);
                    Process.Start("explorer.exe", containingFolder);
                }
            }
        }
        private DataGridView GetSelectedDataGridView()
        {
            if (tabControl1.SelectedTab != null)
            {
                DataGridView dataGridView = tabControl1.SelectedTab.Controls.OfType<DataGridView>().FirstOrDefault();
                return dataGridView;
            }
            return null;
        }
        private DataGridView GetLastDataGridView()
        {
            if (tabControl1.TabPages.Count > 0)
            {
                TabPage lastTab = tabControl1.TabPages[tabControl1.TabPages.Count - 1];
                DataGridView dataGridView = lastTab.Controls.OfType<DataGridView>().FirstOrDefault();
                return dataGridView;
            }
            return null;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DataGridView selectedDataGridView = GetLastDataGridView();
            if (selectedDataGridView != null)
            {
                CopyAndAddDataGridViewToNewTab(selectedDataGridView, desiredColumnOrder);
            }
        }
        private void CopyAndAddDataGridViewToNewTab(DataGridView originalDataGridView, List<string> desiredColumnOrder)
        {
            DataGridView newGridView = new DataGridView();
            newGridView.Dock = DockStyle.Fill;
            Dictionary<string, int> columnIndices = new Dictionary<string, int>();
            // Store the current display indices of the columns
            for (int columnIndex = 0; columnIndex < originalDataGridView.Columns.Count; columnIndex++)
            {
                columnIndices[originalDataGridView.Columns[columnIndex].HeaderText] = columnIndex;
            }
            foreach (string columnName in desiredColumnOrder)
            {
                if (columnIndices.TryGetValue(columnName, out int columnIndex))
                {
                    DataGridViewColumn newColumn = originalDataGridView.Columns[columnIndex].Clone() as DataGridViewColumn;
                    newGridView.Columns.Add(newColumn);
                    newColumn.DisplayIndex = newGridView.Columns.Count - 1; // Set display index
                    columnIndices.Remove(columnName);
                }
            }
            foreach (int columnIndex in columnIndices.Values)
            {
                DataGridViewColumn newColumn = originalDataGridView.Columns[columnIndex].Clone() as DataGridViewColumn;
                newGridView.Columns.Add(newColumn);
                newColumn.DisplayIndex = newGridView.Columns.Count - 1; // Set display index
            }
            // Copy the data
            foreach (DataGridViewRow row in originalDataGridView.Rows)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(newGridView);
                for (int columnIndex = 0; columnIndex < row.Cells.Count; columnIndex++)
                {
                    newRow.Cells[columnIndex].Value = row.Cells[columnIndex].Value;
                }
                newGridView.Rows.Add(newRow);
            }
            TabPage newTab = new TabPage("OrderedColumns"); // You can set the tab title accordingly
            newTab.Controls.Add(newGridView);
            tabControl1.TabPages.Add(newTab);
        }
        private void btnOpenOriginal_Click(object sender, EventArgs e)
        {
            try
            {
                openWHexcelDB(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void openWHexcelDB(string thePathToFile)
        {
            Process excel = new Process();
            excel.StartInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.exe";
            excel.StartInfo.Arguments = AddQuotesIfRequired(thePathToFile);
            excel.Start();
        }
        public string AddQuotesIfRequired(string path)
        {
            return !string.IsNullOrWhiteSpace(path) ?
                path.Contains(" ") && (!path.StartsWith("\"") && !path.EndsWith("\"")) ?
                    "\"" + path + "\"" : path :
                    string.Empty;
        }
    }
}
