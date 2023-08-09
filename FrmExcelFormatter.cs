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


namespace WH_Panel
{
    public partial class FrmExcelFormatter : Form
    {
        public FrmExcelFormatter()
        {
            InitializeComponent();
        }

        //private void btnGetSourceFile_Click(object sender, EventArgs e)
        //{
        //    using (OpenFileDialog openFileDialog = new OpenFileDialog())
        //    {
        //        openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
        //        openFileDialog.Title = "Select an Excel File";
        //        openFileDialog.InitialDirectory = "\\\\dbr1\\Data\\Aegis_NPI_Projects";

        //        if (openFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            string filePath = openFileDialog.FileName;

        //            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties=\"Excel 12.0 Xml;HDR=YES;\"";

        //            using (OleDbConnection connection = new OleDbConnection(connectionString))
        //            {
        //                connection.Open();

        //                DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        //                if (schemaTable != null && schemaTable.Rows.Count > 0)
        //                {
        //                    DataRow firstRow = schemaTable.Rows[0];
        //                    string sheetName = firstRow["TABLE_NAME"].ToString();

        //                    DataTable dataTable = new DataTable();
        //                    using (OleDbDataAdapter adapter = new OleDbDataAdapter($"SELECT * FROM [{sheetName}]", connection))
        //                    {
        //                        adapter.Fill(dataTable);
        //                    }

        //                    if (dataTable.Rows.Count == 0)
        //                    {
        //                        MessageBox.Show("No data found in the Excel sheet.");
        //                    }
        //                    else
        //                    {
        //                        dataGridView1.DataSource = dataTable;
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show("No sheets found in the Excel file.");
        //                }
        //            }
        //        }
        //    }
        //}
        public string clientName = string.Empty;
        public string projectName = string.Empty;
        public string filePath = string.Empty;

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void DataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dataGridView = sender as DataGridView;
                DataGridViewCell cell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                List<string> IPNfilter = new List<string>() { "Catalog", "PN" };
                foreach (string filter in IPNfilter)
                {
                    if (cell.Value != null && cell.Value.ToString() == filter)
                    {
                        cell.Value = "IPN";
                    }
                }
                List<string> MFPNfilter = new List<string>() { "Part No", "Part" };
                foreach (string mfilter in MFPNfilter)
                {
                    if (cell.Value != null && cell.Value.ToString() == mfilter)
                    {
                        cell.Value = "MFPN";
                    }
                }
                List<string> qtyfilter = new List<string>() { "Quantity" };
                foreach (string qfilter in qtyfilter)
                {
                    if (cell.Value != null && cell.Value.ToString() == qfilter)
                    {
                        cell.Value = "Qty";
                    }
                }
            }
        }

        private void FrmExcelFormatter_Load(object sender, EventArgs e)
        {
            AttachRightClickHandlerToDataGridViews(this);
        }
        private void AttachRightClickHandlerToDataGridViews(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                if (childControl is DataGridView dataGridView)
                {
                    dataGridView.CellMouseDown += DataGridView_CellMouseDown;
                }
                else
                {
                    AttachRightClickHandlerToDataGridViews(childControl);
                }
            }
        }

        List<string> desiredColumnOrder = new List<string> { "#", "IPN", "MFPN", "Description", "Qty", /* Add more column names here */ };

        private void btnSetDataHeader_Click(object sender, EventArgs e)
        {
            runFormattingLogic();
            // Specify the desired column order


        }
        private void ReorderColumns(DataGridView gridView, List<string> columnOrder)
        {
            // Clone columns from the current DataGridView in the specified order
            DataGridViewColumn[] newColumns = new DataGridViewColumn[columnOrder.Count];
            for (int i = 0; i < columnOrder.Count; i++)
            {
                string columnName = columnOrder[i];
                DataGridViewColumn column = gridView.Columns[columnName].Clone() as DataGridViewColumn;
                newColumns[i] = column;
            }

            gridView.Columns.Clear();
            gridView.Columns.AddRange(newColumns);
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

                    // Locate the header row with specific values and clone it
                    DataGridViewRow headerRow = null;
                    foreach (DataGridViewRow row in currentGridView.Rows)
                    {
                        if (row.Cells.Cast<DataGridViewCell>().Any(cell => cell.Value != null &&
                                                                            (cell.Value.ToString() == "IPN" ||
                                                                             cell.Value.ToString() == "MFPN" ||
                                                                             cell.Value.ToString() == "Qty")))
                        {
                            headerRow = row;
                            break;
                        }
                    }


                    // Copy non-empty rows from startRowIndex to endRowIndex, excluding the header row
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

                        if (!isRowEmpty && row != headerRow) // Exclude the header row and empty rows
                        {
                            DataGridViewRow newRow = (DataGridViewRow)row.Clone();
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                newRow.Cells[cell.ColumnIndex].Value = cell.Value;
                            }
                            newGridView.Rows.Add(newRow);
                        }
                    }

                    // Assign headers from the header row
                    if (headerRow != null)
                    {
                        for (int columnIndex = 0; columnIndex < headerRow.Cells.Count; columnIndex++)
                        {
                            newGridView.Columns[columnIndex].HeaderText = headerRow.Cells[columnIndex].Value.ToString();
                        }
                    }

                    newTab.Controls.Add(newGridView);



                    tabControl1.SelectedTab = newTab;

                    //List<string> desiredColumnOrder = new List<string> { "IPN", "MFPN", "Description", "Qty", /* Add more column names here */ };

                    // Reorder columns in the new DataGridView
                    //ReorderColumns(newGridView, desiredColumnOrder);
                }
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

                if (emptyCellCount < row.Cells.Count / 2) // Adjust this threshold as needed
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
                    File.Copy(originalFilePath, filePathToSave, true);

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
                            }
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

    }
}
