using FastMember;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Office.Interop.Excel;
using Seagull.Framework.Extensions;
using Seagull.Framework.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Button = System.Windows.Forms.Button;
using CheckBox = System.Windows.Forms.CheckBox;
using ComboBox = System.Windows.Forms.ComboBox;
using DataTable = System.Data.DataTable;
using GroupBox = System.Windows.Forms.GroupBox;
using Label = System.Windows.Forms.Label;
using ListBox = System.Windows.Forms.ListBox;
using TextBox = System.Windows.Forms.TextBox;
namespace WH_Panel
{
    public partial class FrmLinkSimulator : Form
    {
        //List<ClientWarehouse> warehouses = new List<ClientWarehouse>();
        List<ClientWarehouse> warehouses { get; set; }
        public List<WHitem> stockItems = new List<WHitem>();
        List<string> selectedFileNames = new List<string>();
        public bool isSql = false;
        List<BOMList> loadedBOMs { get; set; }
        List<BOMList> selectedBOMs { get; set; }
        public int selectedBOMscount = 0;
        public string fileName = string.Empty;
        public string theExcelFilePath = string.Empty;
        public DataTable BOM1Dtable = new DataTable();
        public FrmLinkSimulator()
        {
            InitializeComponent();
            UpdateControlColors(this);
            loadedBOMs = new List<BOMList>();
            selectedBOMs = new List<BOMList>();
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
            dataGridView1.CellClick += dataGridView1_CellClick;

        }
        public void InitializeGlobalWarehouses(List<ClientWarehouse> warehousesFromTheMain)
        {
            warehouses = warehousesFromTheMain;
            // Ordering the warehouses list by clName
            warehouses = warehouses.OrderBy(warehouse => warehouse.clName).ToList();
            // Adding clNames to comboBox4
            foreach (ClientWarehouse warehouse in warehouses)
            {
                comboBox6.Items.Add(warehouse.clName);
            }
        }
        private void UpdateControlColors(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                // Skip processing of CheckedListBox controls
                if (control is CheckedListBox)
                    // Update control colors based on your criteria
                    continue;
                // Update control colors based on your criteria
                control.BackColor = Color.LightGray;
                control.ForeColor = Color.Black;
                // Handle Button controls separately
                if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    button.FlatAppearance.BorderColor = Color.DarkGray; // Change border color
                    button.ForeColor = Color.Black;
                }
                // Handle Button controls separately
                if (control is GroupBox groupbox)
                {
                    groupbox.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    groupbox.ForeColor = Color.Black;
                }
                // Handle TextBox controls separately
                if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle; // Set border style to FixedSingle
                    textBox.BackColor = Color.LightGray; // Change background color
                    textBox.ForeColor = Color.Black; // Change text color
                }
                // Handle Label controls separately
                if (control is Label label)
                {
                    label.BorderStyle = BorderStyle.FixedSingle; // Set border style to FixedSingle
                    label.BackColor = Color.Gray; // Change background color
                    label.ForeColor = Color.Black; // Change text color
                }
                // Handle TabControl controls separately
                if (control is TabControl tabControl)
                {
                    //tabControl.BackColor = Color.Black; // Change TabControl background color
                    tabControl.ForeColor = Color.Black;
                    // Handle each TabPage within the TabControl
                    foreach (TabPage tabPage in tabControl.TabPages)
                    {
                        tabPage.BackColor = Color.Gray; // Change TabPage background color
                        tabPage.ForeColor = Color.Black; // Change TabPage text color
                    }
                }
                // Handle DataGridView controls separately
                if (control is DataGridView dataGridView)
                {
                    // Update DataGridView styles
                    dataGridView.EnableHeadersVisualStyles = false;
                    dataGridView.BackgroundColor = Color.DarkGray;
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                    dataGridView.RowHeadersDefaultCellStyle.BackColor = Color.Gray;
                    dataGridView.DefaultCellStyle.BackColor = Color.LightGray;
                    dataGridView.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridView.DefaultCellStyle.SelectionBackColor = Color.Green;
                    dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;
                    // Change the header cell styles for each column
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.HeaderCell.Style.BackColor = Color.DarkGray;
                        column.HeaderCell.Style.ForeColor = Color.Black;
                    }
                }
                // Handle ComboBox controls separately
                if (control is ComboBox comboBox)
                {
                    comboBox.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    comboBox.BackColor = Color.DarkGray; // Change ComboBox background color
                    comboBox.ForeColor = Color.Black; // Change ComboBox text color
                }
                // Handle DateTimePicker controls separately
                if (control is DateTimePicker dateTimePicker)
                {
                    // Change DateTimePicker's custom properties here
                    dateTimePicker.BackColor = Color.DarkGray; // Change DateTimePicker background color
                    dateTimePicker.ForeColor = Color.White; // Change DateTimePicker text color
                                                            // Customize other DateTimePicker properties as needed
                }
                // Recursively update controls within containers
                if (control.Controls.Count > 0)
                {
                    UpdateControlColors(control);
                }
            }
        }
        public class BOMList
        {
            public string Name { get; set; }
            public List<KitHistoryItem> Items { get; set; }
            public BOMList(string name)
            {
                Name = name;
                Items = new List<KitHistoryItem>();
            }
        }
        private void ComboBoxes_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        string[] fileNames { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select BOM File";
            openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.ToString("MM.yyyy");
            openFileDialog1.Filter = "BOM files(*.xlsm) | *.xlsm";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileNames = openFileDialog1.FileNames; // Get all selected file names
                foreach (string fileName in fileNames)
                {
                    string theExcelFilePath = Path.GetFileName(fileName);
                    string Litem = Path.GetFileName(fileName);
                    if (IsFileLoaded(theExcelFilePath))
                    {
                        MessageBox.Show("File already loaded!");
                    }
                    else
                    {
                        DataLoader(fileName, theExcelFilePath);
                        // Add the selected file path to the list
                        selectedFileNames.Add(theExcelFilePath);
                      


                    }
                }
            }
            PopulateDataGridView();
            SetSelectedBoms();
        }
        private void SetSelectedBoms()
        {
            selectedBOMs.Clear(); // Clear the selectedBOMs list before populating
            selectedBOMscount = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Ensure row is not null
                if (row != null)
                {
                    // Check if row index is within range
                    if (row.Index >= 0 && row.Index < dataGridView1.Rows.Count)
                    {
                        DataGridViewCheckBoxCell checkBoxCell = row.Cells[0] as DataGridViewCheckBoxCell;
                        // Ensure checkBoxCell is not null
                        if (checkBoxCell != null && checkBoxCell.Value != null)
                        {
                            bool isChecked = Convert.ToBoolean(checkBoxCell.Value);
                            if (isChecked)
                            {
                                // Ensure column index 1 is within range
                                if (row.Cells.Count > 1)
                                {
                                    string bomName = row.Cells[1].Value?.ToString(); // Safely get cell value
                                    if (!string.IsNullOrEmpty(bomName))
                                    {
                                        if (loadedBOMs != null)
                                        {
                                            var selectedBom = loadedBOMs.FirstOrDefault(b => b.Name == bomName);
                                            if (selectedBom != null)
                                            {
                                                selectedBOMscount++;
                                                selectedBOMs.Add(selectedBom);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            label1.Text = $"Loaded: {dataGridView1.RowCount.ToString()}   Selected: {selectedBOMscount}";
            label1.Parent?.Invalidate();
        }
        private void PopulateDataGridView()
        {
            // Clear existing rows in the DataGridView
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            // Create DataGridView columns
            DataGridViewCheckBoxColumn projectCheckBoxColumn = new DataGridViewCheckBoxColumn();
            projectCheckBoxColumn.HeaderText = "Calculate"; // Header text of the checkbox column
            projectCheckBoxColumn.ReadOnly = false; // Enable editing
            dataGridView1.Columns.Add(projectCheckBoxColumn);
            // Add CellContentClick event handler to the DataGridView

            DataGridViewTextBoxColumn projectColumn = new DataGridViewTextBoxColumn();
            projectColumn.HeaderText = "Project";
            projectColumn.ReadOnly = true;
            dataGridView1.Columns.Add(projectColumn);

            DataGridViewTextBoxColumn ipnsColumn = new DataGridViewTextBoxColumn();
            ipnsColumn.HeaderText = "IPNs in KIT";
            ipnsColumn.ReadOnly = true;
            dataGridView1.Columns.Add(ipnsColumn);

            DataGridViewTextBoxColumn percentageColumn = new DataGridViewTextBoxColumn();
            percentageColumn.HeaderText = "Percentage";
            percentageColumn.ReadOnly = true;
            dataGridView1.Columns.Add(percentageColumn);

            DataGridViewTextBoxColumn folderColumn = new DataGridViewTextBoxColumn();
            folderColumn.HeaderText = "Folder";
            folderColumn.ReadOnly = true;
            dataGridView1.Columns.Add(folderColumn);

            dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2; // E
            foreach (var bom in loadedBOMs)
            {
                int bomPositiveDelta = 0;
                int bomNegativeDelta = 0;
                for (int i = 0; i < bom.Items.Count; i++)
                {
                    if (bom.Items[i].Delta >= 0)
                    {
                        bomPositiveDelta++;
                    }
                    else
                    {
                        bomNegativeDelta++;
                    }
                }
                int bomTot = bomPositiveDelta + bomNegativeDelta;
                double completionPercentage = 0;
                if (bomTot == bomPositiveDelta)
                {
                    completionPercentage = 100;
                }
                else
                {
                    completionPercentage = Math.Round((bomPositiveDelta * 100.0) / bomTot, 2);
                }
                string projectName = bom.Items.Count > 0 ? bom.Items[0].ProjectName : bom.Name;
                string ipnsText = bomPositiveDelta + "/" + bomTot;
                // Create a new row and add it to the DataGridView
                DataGridViewRow row = new DataGridViewRow();
                // Create a DataGridViewCheckBoxCell for the checkbox column
                DataGridViewCheckBoxCell checkBoxCell = new DataGridViewCheckBoxCell();
                checkBoxCell.Value = true; // Set the default value to false
                row.Cells.Add(checkBoxCell); // Add the checkbox cell to the row
                // Add other cells for project name, IPNs in KIT, and percentage
                row.Cells.Add(new DataGridViewTextBoxCell { Value = projectName });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = ipnsText });
                // Create a DataGridViewTextBoxCell for the percentage column
                DataGridViewTextBoxCell percentageCell = new DataGridViewTextBoxCell { Value = completionPercentage };
                // Check if the completion percentage is 100% and set the background color accordingly



                if (completionPercentage == 100)
                {
                    percentageCell.Style.BackColor = Color.LightGreen;
                }
                // Add the percentage cell to the row
                row.Cells.Add(percentageCell);
                // Add the row to the DataGridView




                // Check if loadedfiles is null or empty
                if (loadedfiles != null && loadedfiles.Any())
                {
                    string folderText = loadedfiles
                        .Where(x => x.Contains(projectName))
                        .FirstOrDefault() ?? "DefaultFolderText"; // Provide a fallback value if no match is found

                    // Add folderText to the row in DataGridView
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = folderText });
                }
                else
                {
                    // Handle the case where loadedfiles is null or empty
                    row.Cells.Add(new DataGridViewTextBoxCell { Value = "No files loaded" });
                }






                dataGridView1.Rows.Add(row);
            }
            // Autofit columns
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        // Define the CellContentClick event handler
        //private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (dataGridView1 != null && dataGridView1.Columns.Count > 0)
        //    {
        //        // Display column header text and indices for debugging
        //        //string columnInfo = "Columns in DataGridView:\n\n";
        //        //foreach (DataGridViewColumn column in dataGridView1.Columns)
        //        //{
        //        //    columnInfo += "Header Text: " + column.HeaderText + ", Index: " + column.Index + "\n";
        //        //}

        //        //MessageBox.Show(columnInfo, "DataGridView Columns Information");

        //        // Check if the clicked cell belongs to the checkbox column
        //        //if (dataGridView1.Columns.Contains("Calculate") && e.ColumnIndex == dataGridView1.Columns["Calculate"].Index && e.RowIndex >= 0)
        //        if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].HeaderText == "Calculate")
        //        {
        //            // Handle checkbox click event here
        //            DataGridViewCheckBoxCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
        //            if (cell != null)
        //            {
        //                bool isChecked = (bool)cell.EditedFormattedValue;
        //                // Perform actions based on the checkbox state (isChecked)
        //                // For example:
        //                if (isChecked)
        //                {
        //                    // Checkbox is checked
        //                    SetSelectedBoms();
        //                    //MessageBox.Show("SetSelectedBoms called - Checkbox Checked");
        //                }
        //                else
        //                {
        //                    // Checkbox is unchecked
        //                    SetSelectedBoms();
        //                    //MessageBox.Show("SetSelectedBoms called - Checkbox unchecked");
        //                }
        //            }
        //        }
        //    }
        //}
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsCalculateColumn(e.ColumnIndex) && e.RowIndex >= 0)
            {
                HandleCheckboxClick(e.RowIndex);
            }
        }
        private void HandleCheckboxClick(int rowIndex)
        {
            // Iterate through each column in the DataGridView
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                // Check if the column header text is "Calculate"
                if (column.HeaderText == "Calculate")
                {
                    // Get the cell in the current row corresponding to the "Calculate" column
                    DataGridViewCheckBoxCell checkBoxCell = dataGridView1.Rows[rowIndex].Cells[column.Index] as DataGridViewCheckBoxCell;
                    if (checkBoxCell != null)
                    {
                        bool isChecked = (bool)checkBoxCell.EditedFormattedValue;
                        if (isChecked)
                        {
                            // Checkbox is checked
                            SetSelectedBoms();
                        }
                        else
                        {
                            // Checkbox is unchecked
                            SetSelectedBoms();
                        }
                    }
                }
            }
        }

        private bool IsCalculateColumn(int columnIndex)
        {
            if (dataGridView1.Columns[columnIndex].HeaderText == "Calculate")
            {
                return true;
            }
            return false;
        }

        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{

        //    // Check if the clicked cell belongs to the checkbox column
        //    if (dataGridView1.Columns.Contains("Calculate") && e.ColumnIndex == dataGridView1.Columns["Calculate"].Index)
        //    {
        //        // Handle checkbox click event here
        //        DataGridViewCheckBoxCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
        //        if (cell != null)
        //        {
        //            bool isChecked = (bool)cell.EditedFormattedValue;
        //            if (isChecked)
        //            {
        //                SetSelectedBoms();
        //                //MessageBox.Show("SetSelectedBoms called - Checkbox Checked");
        //            }
        //            else
        //            {
        //                SetSelectedBoms();
        //                //MessageBox.Show("SetSelectedBoms called - Checkbox Unchecked");
        //            }
        //        }
        //    }
        //    //MessageBox.Show("Cell Clicked!");

        //}
        private void button7_Click(object sender, EventArgs e)
        {
            // Iterate through each column in the DataGridView
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                // Check if the column header text is "Percentage"
                if (column.HeaderText == "Percentage")
                {
                    // Iterate through each row in the DataGridView
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Get the cell in the current row corresponding to the "Percentage" column
                        DataGridViewCell cell = row.Cells[column.Index];

                        // Check if the cell value is not null and equals "100"
                        if (cell.Value != null && cell.Value.ToString() == "100")
                        {
                            // Get the checkbox cell in the "Calculate" column for this row
                            string calculateColumnHeaderText = "Calculate";
                            foreach (DataGridViewColumn col in dataGridView1.Columns)
                            {
                                if (col.HeaderText == calculateColumnHeaderText)
                                {
                                    DataGridViewCheckBoxCell checkBoxCell = row.Cells[col.Index] as DataGridViewCheckBoxCell;
                                    if (checkBoxCell != null)
                                    {
                                        // Toggle the checkbox
                                        bool currentValue = (bool)checkBoxCell.Value;
                                        checkBoxCell.Value = !currentValue;
                                    }

                                    // Handle checkbox click event
                                    HandleCheckboxClick(row.Index);
                                    break; // Break the inner loop once we've found the "Calculate" column
                                }
                            }
                        }
                    }
                }
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsCalculateColumn(e.ColumnIndex) && e.RowIndex >= 0)
            {
                HandleCheckboxClick(e.RowIndex);
            }
        }

        private bool IsFileLoaded(string fileName)
        {
            foreach (BOMList bom in loadedBOMs)
            {
                if (bom.Name == fileName)
                {
                    return true;
                }
            }
            return false;
        }
        private void DataLoader(string fp, string excelFIleName)
        {
            BOMList a = new BOMList(excelFIleName);
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=1\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    try
                    {
                        conn.Open();
                        DataTable dbSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dbSchema == null || dbSchema.Rows.Count < 1)
                        {
                            throw new Exception("Error: Could not determine the name of the first worksheet.");
                        }
                        string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();
                        string cleanedUpSheetName = firstSheetName.Substring(1).Substring(0, firstSheetName.Length - 3);
                        OleDbCommand command = new OleDbCommand("Select * from [" + cleanedUpSheetName + "$]", conn);
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            int indIPN = reader.GetOrdinal("IPN");
                            int indMFPN = reader.GetOrdinal("MFPN");
                            int indDescription = reader.GetOrdinal("Description");
                            int indDELTA = reader.GetOrdinal("DELTA");
                            int indQty = reader.GetOrdinal("Qty");
                            int indCalc = reader.GetOrdinal("Calc");
                            int indAlts = indQty + 2;
                            while (reader.Read())
                            {
                                int del = 0;
                                bool delPar = int.TryParse(reader[indDELTA].ToString(), out del);
                                int qtk = 0;
                                bool qtkPar = int.TryParse(reader[indDELTA - 1].ToString(), out qtk);
                                int qpu = 0;
                                bool qpuPar = int.TryParse(reader[indQty].ToString(), out qpu);
                                KitHistoryItem abc = new KitHistoryItem
                                {
                                    DateOfCreation = cleanedUpSheetName,
                                    ProjectName = excelFIleName,
                                    IPN = reader[indIPN].ToString(),
                                    MFPN = reader[indMFPN].ToString(),
                                    Description = reader[indDescription].ToString(),
                                    QtyInKit = qtk,
                                    Delta = del,
                                    QtyPerUnit = qpu,
                                    Calc = reader[indCalc].ToString(),
                                    Alts = reader[indAlts].ToString()
                                };
                                a.Items.Add(abc);
                            }
                        }
                        conn.Dispose();
                        conn.Close();
                        string[] alltheNames = excelFIleName.Split("_");
                    }
                    catch (Exception e)
                    {
                        string er = fp;
                        conn.Dispose();
                        conn.Close();
                    }
                }
                loadedBOMs.Add(a);
                if (a.Items.Count > 0)
                {
                    CheckAndSetWarehouse(a.Items[0].IPN);
                }
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void CheckAndSetWarehouse(string firstIPN)
        {
            foreach (ClientWarehouse warehouse in warehouses)
            {
                if (firstIPN.StartsWith(warehouse.clPrefix))
                {
                    comboBox6.SelectedItem = warehouse.clName;
                    break;
                }
            }
        }
        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (ClientWarehouse w in warehouses)
            {
                if (comboBox6.SelectedItem == w.clName)
                {
                    if (w.sqlStock != null)
                    {
                        isSql = true;
                        StockViewDataLoader(w.sqlStock, "STOCK");
                    }
                    else
                    {
                        isSql = false;
                        StockViewDataLoader(w.clStockFile, "STOCK");
                    }
                }
            }
            if (Control.MouseButtons == MouseButtons.Left)
            {
                // This will be executed only on left-click
                SetSelectedBoms();
                GenerateHtmlReport(false);
            }
            else if (Control.MouseButtons == MouseButtons.Right)
            {
                SetSelectedBoms();
                GenerateHtmlReportWithKitSeparation();
            }
        }
        private void button4_MouseClick(object sender, MouseEventArgs e)
        {
            //
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Right-click logic can be added later
        }
        private void GenerateHtmlReportWithKitSeparation()
        {
            var stockDataDetailed = selectedBOMs
    .SelectMany(bom => bom.Items)
    .GroupBy(item => item.IPN)
    .Select(group => new
    {
        IPN = group.Key,
        BOMs = group.Select(item => new
        {
            Title = item.ProjectName,
            IPN = item.IPN,
            MFPN = item.MFPN,
            Description = item.Description,
            Quantity = item.Delta
        }),
        TotalRequired = group.Sum(item => item.Delta)
    })
            .OrderBy(item => item.IPN);
            string fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            // Generating the HTML content
            // <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css'>
            string htmlContent = @"<!DOCTYPE html>
                        <html style='background-color: gray;'>
                        <head>
                        <style>
                        .lightcoral { background-color: lightcoral; }
                        .lightgreen { background-color: lightgreen; }
                         .sticky {
        position: sticky;
        top: 0;
        background-color: gray;
        z-index: 0;
    }
  #stockTable {
    max-width: 100%; /* Set the maximum width to 100% of the container (screen) */
    margin: 0 auto; /* Center the table within its container */
    border: 1px solid;
    text-align: center;
  }
    table {
        border-collapse: collapse;
        width: 100%;
    }
    th {
        position: sticky;
        top: calc(0rem + 1px);
        border: 1px solid black;
        background-color: gray;
        z-index: 1;
    }
     .wrap-content {
    overflow-wrap: break-word;
    word-wrap: break-word; /* For older browsers */
  }
                        </style>
                        <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
                        </head>";
            htmlContent += @"<body>";
            htmlContent += @"<table style='border: 1px solid; text-align: center; font-weight:bold;'>";
            htmlContent += @"<tr>";
            htmlContent += @"<td style='width: 25%;height: 100%;' id='completion-chart-td'> <div> <canvas id='completion-chart'></canvas> </div></td>";
            htmlContent += @"<td style='width: 50%;'><h2>UPDATED_" + fileTimeStamp + "</h2></td>";
            htmlContent += @"<td style='width: 25%;'></td>";
            htmlContent += @"</tr>";
            htmlContent += @"<tr><td>Multi-BOM simulation for " + selectedBOMs.Count + " kits:</td></tr>";
            foreach (var bom in selectedBOMs)
            {
                int bomPositiveDelta = 0;
                int bomNegativeDelta = 0;
                for (int i = 0; i < bom.Items.Count; i++)
                {
                    if (bom.Items[i].Delta >= 0)
                    {
                        bomPositiveDelta++;
                    }
                    else
                    {
                        bomNegativeDelta++;
                    }
                }
                int bomTot = bomPositiveDelta + bomNegativeDelta;
                double completionPercentage = 0;
                if (bomTot == bomPositiveDelta)
                {
                    completionPercentage = 100;
                }
                else
                {
                    completionPercentage = Math.Round((bomPositiveDelta * 100.0) / bomTot, 2);
                }
                htmlContent += $"<tr{(bomPositiveDelta == bomTot ? " style='background-color: lightgreen;'" : "")}><td>{bom.Name.TrimEnd(".xlsm".ToCharArray())} ({bomPositiveDelta}/{bomTot} IPNs in KIT) {completionPercentage}%</td></tr>";
            }
            htmlContent += @"<tr ><td><div id='completion-perc'> Average completion percentage is  </div></td></tr>";
            htmlContent += @" <tr><td>
<input type='text' id=""searchInput"" placeholder=""Filter IPN or MFPN.."" onkeyup=""filterTable()"" />
<button onclick=""clearFilter()"">Clear Filter</button></td></tr>";
            htmlContent += @"</tbody></table><br>";
            htmlContent += @"
    <style>
        #stockTableMain th {
            position: sticky;
            top: 0;
            background-color: #606060
;
        }
    </style>
    <table id='stockTableMain' class='wrap-content' style='border: 1px solid; text-align: center; width: 100%;'>
         <thead>        
            <tr>
                <th style='width: 28%;'>Project</th>
                <th style='width: 12%;'>IPN</th>
                <th style='width: 12%;'>MFPN</th>
                <th style='width: 12%;'>Description</th>
                <th style='width: 12%;'>WH Qty</th>
                <th style='width: 12%;'>KITs BALANCE</th>
                <th style='width: 12%;'>DELTA</th>
            </tr>
        </thead>";
            List<SIMIPNTABLE> MAINDATASOURCE_LIST = new List<SIMIPNTABLE>();
            foreach (var item in stockDataDetailed)
            {
                SIMIPNTABLE MAINDATASOURCE = new SIMIPNTABLE();
                MAINDATASOURCE.IPN = item.IPN;
                MAINDATASOURCE.WHqty = stockItems.Where(si => si.IPN == item.IPN).Sum(si => si.Stock);
                MAINDATASOURCE.KITsBalance = item.TotalRequired;
                MAINDATASOURCE.DELTA = MAINDATASOURCE.WHqty + MAINDATASOURCE.KITsBalance;
                MAINDATASOURCE.BOMITEMS = new List<BOMitem>();
                foreach (var bomItem in item.BOMs)
                {
                    BOMitem b = new BOMitem();
                    b.ProjectName = bomItem.Title;
                    b.MFPN = bomItem.MFPN;
                    b.Description = bomItem.Description;
                    b.QtyInKit = bomItem.Quantity;
                    MAINDATASOURCE.BOMITEMS.Add(b);
                }
                MAINDATASOURCE_LIST.Add(MAINDATASOURCE);
            }
            // Order the MAINDATASOURCE_LIST by DELTA
            MAINDATASOURCE_LIST = MAINDATASOURCE_LIST.OrderBy(mainDataSource => mainDataSource.DELTA).ToList();
            // Generate HTML
            htmlContent += "<table border='1' style='border-collapse: collapse; width: 100%;'>";
            //htmlContent += "<tr style='background-color: #f2f2f2;'>";
            //htmlContent += "<th style='padding: 10px;'>IPN</th>";
            //htmlContent += "<th style='padding: 10px;'>Warehouse Quantity</th>";
            //htmlContent += "<th style='padding: 10px;'>KITs Balance</th>";
            //htmlContent += "<th style='padding: 10px;'>Delta</th>";
            //htmlContent += "</tr>";
            foreach (var mainDataSource in MAINDATASOURCE_LIST)
            {
                var rowColorClass = mainDataSource.DELTA < 0 ? "lightcoral" : "lightgreen";
                // Add main data row
                htmlContent += $"<tr class='{rowColorClass}' style='text-align:center;border: 1px solid black;'>";
                htmlContent += $"<td style='width:64%;' columnspan='4'>{mainDataSource.IPN}</td>";
                htmlContent += $"<td style='width:12%;'>{mainDataSource.WHqty}</td>";
                htmlContent += $"<td style='width:12%;'>{mainDataSource.KITsBalance}</td>";
                htmlContent += $"<td class='DELTAFIELD' style='width:12%;'>{mainDataSource.DELTA}</td>";
                htmlContent += "</tr>";
                // Add sub-table for BOM items style='border: 1px solid black;'
                htmlContent += "<tr>";
                htmlContent += "<td  colspan='7'>";
                htmlContent += "<table border='1' style='border-collapse: collapse; width: 100%;border: 1px solid black;'>";
                // Add BOM item header
                //htmlContent += "<tr class='{rowColorClass}' style='background-color: #d9edf7;'>";
                //htmlContent += "<th style='padding: 10px;'>Project Name</th>";
                //htmlContent += "<th style='padding: 10px;'>MFPN</th>";
                //htmlContent += "<th style='padding: 10px;'>Description</th>";
                //htmlContent += "<th style='padding: 10px;'>Quantity in Kit</th>";
                //htmlContent += "</tr>";
                // Add BOM item data
                foreach (var bomItem in mainDataSource.BOMITEMS)
                {
                    htmlContent += "<tr style='text-align:center;'>";
                    //Truncate the last 5 characters of Title
                    string truncatedTitle = bomItem.ProjectName.Length > 5
                        ? bomItem.ProjectName.Substring(0, bomItem.ProjectName.Length - 5)
                        : bomItem.ProjectName;
                    htmlContent += $"<td style='width:28%;'>{truncatedTitle}</td>";
                    htmlContent += $"<td class='wrap-content' style='width:12%;'>{bomItem.MFPN}</td>";
                    htmlContent += $"<td class='wrap-content' style='width:36%;' columnspan='3'>{bomItem.Description}</td>";
                    var rowColorClassQ = bomItem.QtyInKit < 0 ? "lightcoral" : "lightgreen";
                    htmlContent += $"<td class='{rowColorClassQ}' style='width:12%;'>{bomItem.QtyInKit}</td>";
                    htmlContent += $"<td style='width:12%;'></td>";
                    htmlContent += "</tr>";
                }
                // Close sub-table
                htmlContent += "</table>";
                htmlContent += "</br>";
                htmlContent += "</td>";
                htmlContent += "</tr>";
            }
            // Close the main table
            htmlContent += "</table>";
            htmlContent += "</div></table>";
            htmlContent += @"<script>
window.onload = function() {
     CalculateCompletion();
document.addEventListener('DOMContentLoaded', function() {
    // Your sorting function here
    sortTablesByDelta();
});
    };
function sortTablesByDelta() {
    var mainTable = document.getElementById('stockTableMain');
    var subTables = mainTable.querySelectorAll('table[id^=""stockTable_""]');
    console.log(subTables);
    subTables = Array.from(subTables).sort(function (a, b) {
        var ipnA = a.id.split('_')[1]; // Extract IPN from the table ID
        var ipnB = b.id.split('_')[1];
        var deltaA = parseFloat(a.querySelector('#delta_' + ipnA).innerText);
        var deltaB = parseFloat(b.querySelector('#delta_' + ipnB).innerText);
        return isNaN(deltaA) || isNaN(deltaB) ? 0 : deltaA - deltaB;
    });
    // Clear the main table
    while (mainTable.firstChild) {
        mainTable.removeChild(mainTable.firstChild);
    }
    // Append sorted sub-tables to the main table
    for (var i = 0; i < subTables.length; i++) {
        mainTable.appendChild(subTables[i]);
    }
}
    function filterTable() {
        var input, filter, table, tr, tdIPN, tdMFPN, txtValueIPN, txtValueMFPN, i;
        input = document.getElementById('searchInput');
        filter = input.value.toUpperCase();
        table = document.getElementById('stockTable');
        tr = table.getElementsByTagName('tr');
        for (i = 0; i < tr.length; i++) {
            tdIPN = tr[i].getElementsByTagName('td')[0];
            tdMFPN = tr[i].getElementsByTagName('td')[1];
            if (tdIPN || tdMFPN) {
                txtValueIPN = tdIPN.textContent || tdIPN.innerText;
                txtValueMFPN = tdMFPN.textContent || tdMFPN.innerText;
                if (txtValueIPN.toUpperCase().indexOf(filter) > -1 || txtValueMFPN.toUpperCase().indexOf(filter) > -1) {
                    tr[i].style.display = '';
                } else {
                    tr[i].style.display = 'none';
                }
            }
        }
    }
    function clearFilter() {
        document.getElementById('searchInput').value = '';
        var table = document.getElementById('stockTable');
        var tr = table.getElementsByTagName('tr');
        for (var i = 0; i < tr.length; i++) {
            tr[i].style.display = '';
        }
        document.getElementById('searchInput').focus();
    }
    function CalculateCompletion() {
        var lightcoralRows = document.querySelectorAll('.lightcoral');
        var lightcoralCount = lightcoralRows.length;
        var lightgreenRows = document.querySelectorAll('.lightgreen');
        var lightgreenCount = lightgreenRows.length;
        var totalRows = lightcoralCount + lightgreenCount;
        var percentage = (lightgreenCount / totalRows) * 100;
        var completionPercDiv = document.getElementById('completion-perc');
        completionPercDiv.textContent = ""Average KIT vs DB simulation is "" + percentage.toFixed(2) + ""%"" + "" ( ""+lightgreenCount+""/""+totalRows+"" rows )"";
var ctx = document.getElementById('completion-chart').getContext('2d');
var myPieChart = new Chart(ctx, {
    type: 'doughnut',
    data: {
        labels: ['Deficient', 'Sufficient'],
        datasets: [{
            data: [lightcoralCount, lightgreenCount],
            backgroundColor: ['#FF6384', '#4CAF50']
        }]
    },
    options: {
        plugins: {
            legend: {
                display: false, // Hide the legend
            },
            tooltip: {
                callbacks: {
                    label: function(context) {
                        var label = context.label || '';
                        var value = context.parsed || 0;
                        return label + ': ' + value;
                    }
                }
            }
        }
    }
});
    }
    document.getElementById('searchInput').focus();
  document.addEventListener('DOMContentLoaded', function() {
    var completionChartTd = document.getElementById('completion-chart-td');
    if (completionChartTd) {
      var numberOfRows = document.querySelectorAll('table tr').length;
      completionChartTd.rowSpan = numberOfRows;
    } else {
      console.error(""Element with ID 'completion-chart-td' not found"");
    }
  });
</script>";
            htmlContent += "</body></html>";
            string filename = @"\\dbr1\Data\WareHouse\2024\WHsearcher\" + fileTimeStamp + "_BOMs_sim" + ".html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(htmlContent);
            }
            // Opening the HTML file in the default browser
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            process.Start();
        }
        private void StockViewDataLoader(string fp, string thesheetName)
        {
            stockItems.Clear();
            try
            {
                if (isSql)
                {
                    // Connection string for SQL Server Express
                    string connectionString = fp;
                    try
                    {
                        // Load STOCK table into dataGridView1
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlDataAdapter adapterStock = new SqlDataAdapter("SELECT * FROM STOCK", connection);
                            DataTable stockTable = new DataTable();
                            adapterStock.Fill(stockTable);
                            foreach (DataRow row in stockTable.Rows)
                            {
                                WHitem item = new WHitem
                                {
                                    IPN = row["IPN"].ToString().Trim(),
                                    Manufacturer = row["Manufacturer"].ToString(),
                                    MFPN = row["MFPN"].ToString(),
                                    Description = row["Description"].ToString(),
                                    Stock = Convert.ToInt32(row["Stock"]), // Assuming Stock is an integer field
                                    Updated_on = row["Updated_on"].ToString(),
                                    Comments = row["Comments"].ToString(),
                                    Source_Requester = row["Source_Requester"].ToString()
                                };
                                stockItems.Add(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show($"Error loading STOCK table: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                    using (OleDbConnection conn = new OleDbConnection(constr))
                    {
                        conn.Open();
                        OleDbCommand command = new OleDbCommand("Select * from [" + thesheetName + "$]", conn);
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    int res = 0;
                                    int toStk;
                                    bool stk = int.TryParse(reader[4].ToString(), out res);
                                    if (stk)
                                    {
                                        toStk = res;
                                    }
                                    else
                                    {
                                        toStk = 0;
                                    }
                                    WHitem abc = new WHitem
                                    {
                                        IPN = reader[0].ToString(),
                                        Manufacturer = reader[1].ToString(),
                                        MFPN = reader[2].ToString(),
                                        Description = reader[3].ToString(),
                                        Stock = toStk,
                                        Updated_on = reader[5].ToString(),
                                        Comments = reader[6].ToString(),
                                        Source_Requester = reader[7].ToString()
                                    };
                                    stockItems.Add(abc);
                                }
                                catch (Exception E)
                                {
                                    MessageBox.Show(E.Message);
                                    throw;
                                }
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error");
            }
        }
        private void GenerateHtmlReport(bool limitOrNot)
        {
            var sumRequiredByIPN = selectedBOMs.SelectMany(bom => bom.Items)
                                      .GroupBy(item => item.IPN)
                                      .Select(group => new
                                      {
                                          IPN = group.Key,
                                          TotalRequired = group.Sum(item => item.Delta)
                                      });
            var stockData = selectedBOMs.SelectMany(bom => bom.Items)
                   .GroupBy(item => item.IPN)
                   .Select(group => new
                   {
                       IPN = group.Key,
                       MFPN = group.Select(x => x.MFPN).FirstOrDefault(),
                       Description = group.Select(x => x.Description).FirstOrDefault(),
                       TotalRequired = group.Sum(item => item.Delta)
                   })
                   .GroupJoin(stockItems,
                              sumItem => sumItem.IPN,
                              stockItem => stockItem.IPN,
                              (sumItem, stockItemGroup) => new
                              {
                                  IPN = sumItem.IPN,
                                  MFPN = sumItem.MFPN,
                                  Description = sumItem.Description,
                                  StockQuantity = stockItemGroup.Sum(item => item.Stock),
                                  TotalRequired = sumItem.TotalRequired
                              })
                   .OrderBy(item => item.TotalRequired);
            string fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            // Generating the HTML content
            // <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css'>
            string htmlContent = @"<!DOCTYPE html>
                        <html style='background-color: gray;'>
                        <head>
                        <style>
                        .lightcoral { background-color: lightcoral; }
                        .lightgreen { background-color: lightgreen; }
                         .sticky {
        position: sticky;
        top: 0;
        background-color: gray;
        z-index: 0;
    }
  #stockTable {
    max-width: 100%; /* Set the maximum width to 100% of the container (screen) */
    margin: 0 auto; /* Center the table within its container */
    border: 1px solid;
    text-align: center;
  }
    table {
        border-collapse: collapse;
        width: 100%;
    }
    th {
        position: sticky;
        top: calc(0rem + 1px);
        border: 1px solid black;
        background-color: gray;
        z-index: 1;
    }
     .wrap-content {
    overflow-wrap: break-word;
    word-wrap: break-word; /* For older browsers */
  }
                        </style>
                        <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
                        </head>";
            htmlContent += @"<body>";
            htmlContent += @"<table style='border: 1px solid; text-align: center; font-weight:bold;'>";
            htmlContent += @"<tr>";
            htmlContent += @"<td style='width: 25%;height: 100%;' id='completion-chart-td'> <div> <canvas id='completion-chart'></canvas> </div></td>";
            htmlContent += @"<td style='width: 50%;'><h2>UPDATED_" + fileTimeStamp + "</h2></td>";
            htmlContent += @"<td style='width: 25%;'></td>";
            htmlContent += @"</tr>";
            htmlContent += @"<tr><td>Multi-BOM simulation for " + selectedBOMs.Count + " kits:</td></tr>";
            if (limitOrNot)
            {
                //
            }
            else
            {
                foreach (var bom in selectedBOMs)
                {
                    int bomPositiveDelta = 0;
                    int bomNegativeDelta = 0;
                    for (int i = 0; i < bom.Items.Count; i++)
                    {
                        if (bom.Items[i].Delta >= 0)
                        {
                            bomPositiveDelta++;
                        }
                        else
                        {
                            bomNegativeDelta++;
                        }
                    }
                    int bomTot = bomPositiveDelta + bomNegativeDelta;
                    double completionPercentage = 0;
                    if (bomTot == bomPositiveDelta)
                    {
                        completionPercentage = 100;
                    }
                    else
                    {
                        completionPercentage = Math.Round((bomPositiveDelta * 100.0) / bomTot, 2);
                    }
                    htmlContent += $"<tr{(bomPositiveDelta == bomTot ? " style='background-color: lightgreen;'" : "")}><td>{bom.Name.TrimEnd(".xlsm".ToCharArray())} ({bomPositiveDelta}/{bomTot} IPNs in KIT) {completionPercentage}%</td></tr>";
                }
            }
            htmlContent += @"<tr ><td><div id='completion-perc'> Average completion percentage is  </div></td></tr>";
            htmlContent += @" <tr ><td>
<input type='text' id=""searchInput"" placeholder=""Filter IPN or MFPN.."" onkeyup=""filterTable()"" />
<button onclick=""clearFilter()"">Clear Filter</button></td></tr>";
            htmlContent += @"</tbody></table><br>";
            htmlContent += @"
                <table id='stockTable' class='wrap-content' style='border: 1px solid; text-align: center;'>
                <tr><th  onclick='sortTable(0)'>IPN</th><th onclick='sortTable(1)'>MFPN</th><th onclick='sortTable(2)'>Description</th><th onclick='sortTable(3)'>WH Qty</th><th onclick='sortTable(4)'>KITs BALANCE</th><th onclick='sortTable(5)'>DELTA</th></tr>";
            foreach (var item in stockData)
            {
                var rowColorClass = item.StockQuantity + item.TotalRequired < 0 ? "lightcoral" : "lightgreen";
                htmlContent += $"<tr class='{rowColorClass}'>";
                htmlContent += $"<td class='wrap-content;white-space: nowrap;'>{item.IPN}</td>";
                htmlContent += $"<td class='wrap-content'>{item.MFPN}</td>";
                htmlContent += $"<td class='wrap-content'>{item.Description}</td>";
                htmlContent += $"<td>{item.StockQuantity}</td>";
                //htmlContent += $"<td>{item.TotalRequired}</td>";
                htmlContent += $"<td style='background-color: {(item.TotalRequired < 0 ? "lightcoral" : "lightgreen")}'>{item.TotalRequired}</td>";

                htmlContent += $"<td>{item.StockQuantity + item.TotalRequired}</td>";
                htmlContent += "</tr>";
            }
            htmlContent += "</table></div>";
            htmlContent += @"<script>
    window.onload = function() {
     CalculateCompletion();
    };
    function sortTable(columnIndex) {
        var table, rows, switching, i, x, y, shouldSwitch;
        table = document.getElementById('stockTable');
        switching = true;
        while (switching) {
            switching = false;
            rows = table.rows;
            for (i = 1; i < (rows.length - 1); i++) {
                x = rows[i].getElementsByTagName('TD')[columnIndex];
                y = rows[i + 1].getElementsByTagName('TD')[columnIndex];
                if (isNaN(x.innerHTML) || isNaN(y.innerHTML)) {
                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                        shouldSwitch = true;
                        break;
                    }
                } else {
                    if (Number(x.innerHTML) > Number(y.innerHTML)) {
                        shouldSwitch = true;
                        break;
                    }
                }
            }
            if (shouldSwitch) {
                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                switching = true;
            }
        }
    }
    function filterTable() {
        var input, filter, table, tr, tdIPN, tdMFPN, txtValueIPN, txtValueMFPN, i;
        input = document.getElementById('searchInput');
        filter = input.value.toUpperCase();
        table = document.getElementById('stockTable');
        tr = table.getElementsByTagName('tr');
        for (i = 0; i < tr.length; i++) {
            tdIPN = tr[i].getElementsByTagName('td')[0];
            tdMFPN = tr[i].getElementsByTagName('td')[1];
            if (tdIPN || tdMFPN) {
                txtValueIPN = tdIPN.textContent || tdIPN.innerText;
                txtValueMFPN = tdMFPN.textContent || tdMFPN.innerText;
                if (txtValueIPN.toUpperCase().indexOf(filter) > -1 || txtValueMFPN.toUpperCase().indexOf(filter) > -1) {
                    tr[i].style.display = '';
                } else {
                    tr[i].style.display = 'none';
                }
            }
        }
    }
    function clearFilter() {
        document.getElementById('searchInput').value = '';
        var table = document.getElementById('stockTable');
        var tr = table.getElementsByTagName('tr');
        for (var i = 0; i < tr.length; i++) {
            tr[i].style.display = '';
        }
        document.getElementById('searchInput').focus();
    }
    function CalculateCompletion() {
        var lightcoralRows = document.querySelectorAll('.lightcoral');
        var lightcoralCount = lightcoralRows.length;
        var lightgreenRows = document.querySelectorAll('.lightgreen');
        var lightgreenCount = lightgreenRows.length;
        var totalRows = lightcoralCount + lightgreenCount;
        var percentage = (lightgreenCount / totalRows) * 100;
        var completionPercDiv = document.getElementById('completion-perc');
        completionPercDiv.textContent = ""Average KIT vs DB simulation is "" + percentage.toFixed(2) + ""%"" + "" ( ""+lightgreenCount+""/""+totalRows+"" unique IPNs )"";
var ctx = document.getElementById('completion-chart').getContext('2d');
var myPieChart = new Chart(ctx, {
    type: 'doughnut',
    data: {
        labels: ['Deficient', 'Sufficient'],
        datasets: [{
            data: [lightcoralCount, lightgreenCount],
            backgroundColor: ['#FF6384', '#4CAF50']
        }]
    },
    options: {
        plugins: {
            legend: {
                display: false, // Hide the legend
            },
            tooltip: {
                callbacks: {
                    label: function(context) {
                        var label = context.label || '';
                        var value = context.parsed || 0;
                        return label + ': ' + value;
                    }
                }
            }
        }
    }
});
    }
    document.getElementById('searchInput').focus();
  document.addEventListener('DOMContentLoaded', function() {
    var completionChartTd = document.getElementById('completion-chart-td');
    if (completionChartTd) {
      var numberOfRows = document.querySelectorAll('table tr').length;
      completionChartTd.rowSpan = numberOfRows;
    } else {
      console.error(""Element with ID 'completion-chart-td' not found"");
    }
  });
</script>";
            htmlContent += "</body></html>";
            string filename = @"\\dbr1\Data\WareHouse\2024\WHsearcher\" + fileTimeStamp + "_BOMs_sim" + ".html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(htmlContent);
            }
            // Opening the HTML file in the default browser
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            process.Start();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            OptimizeBOMOrder();
        }
        //private void OptimizeBOMOrder()
        //{
        //    if (selectedBOMs.Count >= 2)
        //    {
        //        int maxPositivePercentageIndex = -1;
        //        double maxPositivePercentage = 0;
        //        for (int j = 1; j < selectedBOMs.Count; j++)
        //        {
        //            int totalRows = selectedBOMs[j].Items.Count;
        //            int positiveCount = selectedBOMs[j].Items.Count(item => (item.Delta ?? 0) > 0);
        //            double currentPositivePercentage = (double)positiveCount / totalRows;
        //            if (currentPositivePercentage > maxPositivePercentage)
        //            {
        //                maxPositivePercentage = currentPositivePercentage;
        //                maxPositivePercentageIndex = j;
        //            }
        //        }
        //        if (maxPositivePercentageIndex != -1 && maxPositivePercentageIndex != 0)
        //        {
        //            // Swap the BOMLists in the data grid views
        //            BOMList temp = selectedBOMs[0];
        //            selectedBOMs[0] = selectedBOMs[maxPositivePercentageIndex];
        //            selectedBOMs[maxPositivePercentageIndex] = temp;
        //            // Reload the data in the respective ComboBoxes
        //            //ComboBox[] comboBoxes = new ComboBox[] { comboBox1, comboBox2, comboBox3, comboBox4, comboBox5 };
        //            //for (int i = 0; i < Math.Min(BOMs.Count, 5); i++)
        //            //{
        //            //    comboBoxes[i].SelectedItem = BOMs[i].Name;
        //            //}
        //        }
        //    }
        //}
        //private void OptimizeBOMOrder()
        //{
        //    if (selectedBOMs.Count >= 2)
        //    {
        //        // Sort selectedBOMs by completion percentage
        //        selectedBOMs = selectedBOMs.OrderByDescending(bom =>
        //        {
        //            int totalRows = bom.Items.Count;
        //            int positiveCount = bom.Items.Count(item => (item.Delta ?? 0) > 0);
        //            return (double)positiveCount / totalRows;
        //        }).ToList();

        //        // Create a new form for displaying the kits
        //        Form kitsForm = new Form
        //        {
        //            Text = "Kits Sorted by Completion",
        //            Height = 400,
        //            Width = 600
        //        };

        //        // Create a ListBox to display the kits
        //        ListBox listBox = new ListBox
        //        {
        //            Dock = DockStyle.Fill
        //        };

        //        // Add each kit's name and completion percentage to the ListBox
        //        foreach (var kit in selectedBOMs)
        //        {
        //            int totalRows = kit.Items.Count;
        //            int positiveCount = kit.Items.Count(item => (item.Delta ?? 0) > 0);
        //            double percentage = (double)positiveCount / totalRows * 100;

        //            listBox.Items.Add($"{kit.Name} - {percentage:F2}% Completion");
        //        }

        //        // Add the ListBox to the form
        //        kitsForm.Controls.Add(listBox);

        //        // Show the form as a dialog
        //        kitsForm.ShowDialog();
        //    }
        //}

        //private void OptimizeBOMOrder()
        //{
        //    // Step 1: Load stock data based on the selected warehouse
        //    foreach (ClientWarehouse w in warehouses)
        //    {
        //        if (comboBox6.SelectedItem == w.clName)
        //        {
        //            isSql=true;
        //            StockViewDataLoader(w.sqlStock, "STOCK");
        //            break;
        //        }
        //    }

        //    if (selectedBOMs.Count >= 2)
        //    {
        //        // Step 2: Create a new list to store kits with their completion status
        //        var kitCompletionStatus = new List<Tuple<BOMList, double>>();

        //        // Step 3: Simulate each kit's completion based on stock
        //        foreach (var bom in selectedBOMs)
        //        {
        //            // Step 3.1: Calculate completion percentage for the current kit
        //            int totalItems = bom.Items.Count;
        //            int fullyStockedItems = 0;

        //            foreach (var kitItem in bom.Items)
        //            {
        //                // Check if enough stock is available for this item without modifying the stock
        //                var stockItem = stockItems.FirstOrDefault(s => s.IPN == kitItem.IPN);
        //                if (stockItem != null && stockItem.Stock >= kitItem.Delta)
        //                {
        //                    // Stock is available, mark as fully stocked for this item
        //                    fullyStockedItems++;
        //                }
        //            }

        //            // Step 3.2: Calculate completion percentage for the kit
        //            double completionPercentage = ((double)fullyStockedItems / totalItems) * 100;

        //            // Add the kit and its completion percentage to the list
        //            kitCompletionStatus.Add(new Tuple<BOMList, double>(bom, completionPercentage));
        //        }

        //        // Step 4: Sort the kits by completion percentage (descending)
        //        var sortedKits = kitCompletionStatus.OrderByDescending(k => k.Item2).ToList();

        //        // Step 5: Display the sorted kits in a new dynamic form
        //        Form kitsForm = new Form
        //        {
        //            Text = "Kits Sorted by Completion",
        //            Height = 400,
        //            Width = 600
        //        };

        //        ListBox listBox = new ListBox
        //        {
        //            Dock = DockStyle.Fill
        //        };

        //        foreach (var kitStatus in sortedKits)
        //        {
        //            listBox.Items.Add($"{kitStatus.Item1.Name} - {kitStatus.Item2:F2}% Completion");
        //        }

        //        kitsForm.Controls.Add(listBox);
        //        kitsForm.ShowDialog();
        //    }
        //}

        //private void OptimizeBOMOrder()
        //{
        //    // Step 1: Load stock data based on the selected warehouse
        //    foreach (ClientWarehouse w in warehouses)
        //    {
        //        if (comboBox6.SelectedItem == w.clName)
        //        {
        //            isSql = true;
        //            StockViewDataLoader(w.sqlStock, "STOCK");
        //            break;
        //        }
        //    }

        //    if (selectedBOMs.Count >= 2)
        //    {
        //        // Step 2: Create a new list to store kits with their completion status
        //        var kitCompletionStatus = new List<Tuple<BOMList, double>>();

        //        // Step 3: Simulate each kit's completion based on stock
        //        foreach (var bom in selectedBOMs)
        //        {
        //            // Step 3.1: Calculate completion percentage for the current kit
        //            int totalItems = bom.Items.Count;
        //            int fullyStockedItems = 0;

        //            foreach (var kitItem in bom.Items)
        //            {
        //                // Check if the Delta value makes the item fully stocked
        //                if (kitItem.Delta >= 0)
        //                {
        //                    // Item is already fully stocked
        //                    fullyStockedItems++;
        //                }
        //                else
        //                {
        //                    // Check if enough stock is available for this item without modifying the stock
        //                    var stockItem = stockItems.FirstOrDefault(s => s.IPN == kitItem.IPN);
        //                    if (stockItem != null && stockItem.Stock >= Math.Abs((double)kitItem.Delta))
        //                    {
        //                        // Stock is available to fulfill the remaining quantity
        //                        fullyStockedItems++;
        //                    }
        //                }
        //            }

        //            // Step 3.2: Calculate completion percentage for the kit
        //            double completionPercentage = ((double)fullyStockedItems / totalItems) * 100;

        //            // Add the kit and its completion percentage to the list
        //            kitCompletionStatus.Add(new Tuple<BOMList, double>(bom, completionPercentage));
        //        }

        //        // Step 4: Sort the kits by completion percentage (descending)
        //        var sortedKits = kitCompletionStatus.OrderByDescending(k => k.Item2).ToList();

        //        // Step 5: Display the sorted kits in a new dynamic form
        //        Form kitsForm = new Form
        //        {
        //            Text = "Kits Sorted by Completion",
        //            Height = 400,
        //            Width = 600
        //        };

        //        ListBox listBox = new ListBox
        //        {
        //            Dock = DockStyle.Fill
        //        };

        //        foreach (var kitStatus in sortedKits)
        //        {
        //            listBox.Items.Add($"{kitStatus.Item1.Name} - {kitStatus.Item2:F2}% Completion");
        //        }

        //        kitsForm.Controls.Add(listBox);
        //        kitsForm.ShowDialog();
        //    }
        //}

        //private void OptimizeBOMOrder()
        //{
        //    // Step 1: Load stock data based on the selected warehouse
        //    foreach (ClientWarehouse w in warehouses)
        //    {
        //        if (comboBox6.SelectedItem == w.clName)
        //        {
        //            isSql = true;
        //            StockViewDataLoader(w.sqlStock, "STOCK");
        //            break;
        //        }
        //    }

        //    if (selectedBOMs.Count >= 2)
        //    {
        //        // Step 2: Create a new list to store kits with their completion status and delta values
        //        var kitCompletionStatus = new List<Tuple<BOMList, int, int, int, int>>(); // BOMList, currentDelta, totalItems, afterSimulationDelta, fullyStockedItems

        //        // Step 3: Simulate each kit's completion based on stock
        //        foreach (var bom in selectedBOMs)
        //        {
        //            int totalItems = bom.Items.Count;
        //            int currentDeltaTotal = 0;   // Current delta/total before simulation
        //            int afterSimulationDeltaTotal = 0; // After simulation delta/total
        //            int fullyStockedItems = 0;   // Fully stocked items after simulation

        //            foreach (var kitItem in bom.Items)
        //            {
        //                // Current delta calculation
        //                currentDeltaTotal += kitItem.Delta ?? 0; // Use 0 if Delta is null


        //                // Simulate the kit and check stock availability
        //                if (kitItem.Delta >= 0)
        //                {
        //                    fullyStockedItems++;
        //                }
        //                else
        //                {
        //                    var stockItem = stockItems.FirstOrDefault(s => s.IPN == kitItem.IPN);
        //                    if (stockItem != null && stockItem.Stock >= Math.Abs(kitItem.Delta ?? 0))

        //                    {
        //                        fullyStockedItems++;
        //                    }
        //                    else
        //                    {
        //                        // Add to after-simulation delta if stock is insufficient
        //                        afterSimulationDeltaTotal += kitItem.Delta ?? 0; // Use 0 if Delta is null
        //                    }
        //                }
        //            }

        //            // Step 3.2: Add the kit and its statuses (current delta, total, after-simulation delta) to the list
        //            kitCompletionStatus.Add(new Tuple<BOMList, int, int, int, int>(
        //                bom, currentDeltaTotal, totalItems, afterSimulationDeltaTotal, fullyStockedItems));
        //        }

        //        // Step 4: Create a sortable table to display the kits and their statuses
        //        Form kitsForm = new Form
        //        {
        //            Text = "Kits Sorted by Completion",
        //            Height = 400,
        //            Width = 800
        //        };

        //        DataGridView dataGridView = new DataGridView
        //        {
        //            Dock = DockStyle.Fill,
        //            AutoGenerateColumns = false
        //        };

        //        // Add columns to the DataGridView
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kit Name", DataPropertyName = "KitName" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Current Delta/Total", DataPropertyName = "CurrentDeltaTotal" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "After Simulation Delta/Total", DataPropertyName = "AfterSimulationDeltaTotal" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Completion %", DataPropertyName = "CompletionPercentage" });

        //        // Step 5: Populate the DataGridView with data
        //        var dataSource = kitCompletionStatus.Select(k =>
        //            new
        //            {
        //                KitName = k.Item1.Name,
        //                CurrentDeltaTotal = $"{k.Item2} / {k.Item3}", // currentDelta / totalItems
        //                AfterSimulationDeltaTotal = $"{k.Item4} / {k.Item3}", // afterSimulationDelta / totalItems
        //                CompletionPercentage = $"{((double)k.Item5 / k.Item3) * 100:F2}%" // completion %
        //            }).ToList();

        //        dataGridView.DataSource = dataSource;

        //        // Enable sorting
        //        foreach (DataGridViewColumn column in dataGridView.Columns)
        //        {
        //            column.SortMode = DataGridViewColumnSortMode.Automatic;
        //        }

        //        // Step 6: Display the DataGridView in the form
        //        kitsForm.Controls.Add(dataGridView);
        //        kitsForm.ShowDialog();
        //    }
        //}

        //private void OptimizeBOMOrder()
        //{
        //    // Step 1: Load stock data based on the selected warehouse
        //    foreach (ClientWarehouse w in warehouses)
        //    {
        //        if (comboBox6.SelectedItem == w.clName)
        //        {
        //            isSql = true;
        //            StockViewDataLoader(w.sqlStock, "STOCK");
        //            break;
        //        }
        //    }

        //    if (selectedBOMs.Count >= 2)
        //    {
        //        // Step 2: Create a new list to store kits with their completion status
        //        var kitCompletionStatus = new List<Tuple<BOMList, int, int, int, int>>(); // BOMList, fullyStockedItems, totalItems, afterSimulationFullyStockedItems

        //        foreach (var bom in selectedBOMs)
        //        {
        //            int totalItems = bom.Items.Count;
        //            int currentDeltaTotal = 0;   // Current delta/total before simulation
        //            int afterSimulationDeltaTotal = 0; // After simulation delta/total
        //            int fullyStockedItems = 0;   // Fully stocked items after simulation

        //            foreach (var kitItem in bom.Items)
        //            {
        //                // Current delta calculation

        //                if (kitItem.Delta >= 0)
        //                {
        //                    currentDeltaTotal++;
        //                }
        //            }


        //            foreach (var kitItem in bom.Items)
        //            {
        //                //
        //                var stockItem = stockItems.FirstOrDefault(s => s.IPN == kitItem.IPN);
        //                    if (stockItem != null && stockItem.Stock >= Math.Abs(kitItem.Delta ?? 0))
        //                    {
        //                    afterSimulationDeltaTotal++;
        //                    }

        //            }

        //            // Step 3.2: Add the kit and its statuses to the list
        //            // Ensure you're adding all four parameters
        //            kitCompletionStatus.Add(new Tuple<BOMList, int, int, int, int>(
        //                bom,
        //                currentDeltaTotal, // current delta
        //                totalItems,        // total items
        //                afterSimulationDeltaTotal, // after simulation delta
        //                fullyStockedItems)); // fully stocked items
        //        }


        //        // Step 4: Create a sortable table to display the kits and their statuses
        //        Form kitsForm = new Form
        //        {
        //            Text = "Kits Sorted by Completion",
        //            Height = 400,
        //            Width = 800
        //        };

        //        DataGridView dataGridView = new DataGridView
        //        {
        //            Dock = DockStyle.Fill,
        //            AutoGenerateColumns = false
        //        };

        //        // Add columns to the DataGridView
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kit Name", DataPropertyName = "KitName" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fully Stocked Before Simulation", DataPropertyName = "FullyStockedBefore" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total Items", DataPropertyName = "TotalItems" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fully Stocked After Simulation", DataPropertyName = "FullyStockedAfter" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Completion %", DataPropertyName = "CompletionPercentage" });

        //        // Step 5: Populate the DataGridView with data
        //        var dataSource = kitCompletionStatus.Select(k =>
        //            new
        //            {
        //                KitName = k.Item1.Name,
        //                FullyStockedBefore = k.Item2, // Fully stocked before simulation
        //                TotalItems = k.Item3, // Total items
        //                FullyStockedAfter = k.Item4, // Fully stocked after simulation
        //                CompletionPercentage = $"{((double)k.Item2 / k.Item3) * 100:F2}%" // completion % based on fully stocked before
        //            }).ToList();

        //        dataGridView.DataSource = dataSource;

        //        // Enable sorting
        //        foreach (DataGridViewColumn column in dataGridView.Columns)
        //        {
        //            column.SortMode = DataGridViewColumnSortMode.Automatic;
        //        }

        //        // Step 6: Display the DataGridView in the form
        //        kitsForm.Controls.Add(dataGridView);
        //        kitsForm.ShowDialog();
        //    }
        //}
        //private void OptimizeBOMOrder()
        //{
        //    // Step 1: Load stock data based on the selected warehouse
        //    foreach (ClientWarehouse w in warehouses)
        //    {
        //        if (comboBox6.SelectedItem == w.clName)
        //        {
        //            isSql = true;
        //            StockViewDataLoader(w.sqlStock, "STOCK");
        //            break;
        //        }
        //    }

        //    if (selectedBOMs.Count >= 2)
        //    {
        //        // Step 2: Create a new list to store kits with their completion status
        //        var kitCompletionStatus = new List<Tuple<BOMList, int, int, int, int>>(); // BOMList, fullyStockedItemsBefore, totalItems, fullyStockedItemsAfter

        //        foreach (var bom in selectedBOMs)
        //        {
        //            int totalItems = bom.Items.Count;
        //            int fullyStockedItemsBefore = 0;   // Fully stocked items before simulation
        //            int fullyStockedItemsAfter = 0; // Fully stocked items after simulation

        //            // Count items already in the kit
        //            foreach (var kitItem in bom.Items)
        //            {
        //                if (kitItem.Delta >= 0) // Count fully stocked items
        //                {
        //                    fullyStockedItemsBefore++;
        //                }
        //            }

        //            // Simulate the kit and check stock availability
        //            foreach (var kitItem in bom.Items)
        //            {
        //                var stockItem = stockItems.FirstOrDefault(s => s.IPN == kitItem.IPN);
        //                // Check if stock is available
        //                if (stockItem != null && stockItem.Stock >= 0) // Assuming stock exists
        //                {
        //                    fullyStockedItemsAfter++;
        //                }
        //            }

        //            // Add the kit and its statuses to the list
        //            kitCompletionStatus.Add(new Tuple<BOMList, int, int, int, int>(
        //                bom,                              // BOMList
        //                fullyStockedItemsBefore,         // Fully stocked items before simulation
        //                totalItems,                       // Total items
        //                fullyStockedItemsAfter));        // Fully stocked items after simulation
        //        }


        //        // Step 5: Create a sortable table to display the kits and their statuses
        //        Form kitsForm = new Form
        //        {
        //            Text = "Kits Sorted by Completion",
        //            Height = 400,
        //            Width = 800
        //        };

        //        DataGridView dataGridView = new DataGridView
        //        {
        //            Dock = DockStyle.Fill,
        //            AutoGenerateColumns = false
        //        };

        //        // Add columns to the DataGridView
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kit Name", DataPropertyName = "KitName" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fully Stocked Before Simulation", DataPropertyName = "FullyStockedBefore" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total Items", DataPropertyName = "TotalItems" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fully Stocked After Simulation", DataPropertyName = "FullyStockedAfter" });
        //        dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Completion %", DataPropertyName = "CompletionPercentage" });

        //        // Step 6: Populate the DataGridView with data
        //        var dataSource = kitCompletionStatus.Select(k =>
        //            new
        //            {
        //                KitName = k.Item1.Name,
        //                FullyStockedBefore = k.Item2, // Fully stocked before simulation
        //                TotalItems = k.Item3, // Total items
        //                FullyStockedAfter = k.Item4, // Fully stocked after simulation
        //                CompletionPercentage = $"{((double)(k.Item2 + k.Item4) / k.Item3) * 100:F2}%" // Completion % based on fully stocked before and after
        //            }).ToList();

        //        dataGridView.DataSource = dataSource;

        //        // Enable sorting
        //        foreach (DataGridViewColumn column in dataGridView.Columns)
        //        {
        //            column.SortMode = DataGridViewColumnSortMode.Automatic;
        //        }

        //        // Step 7: Display the DataGridView in the form
        //        kitsForm.Controls.Add(dataGridView);
        //        kitsForm.ShowDialog();
        //    }
        //}

        private void OptimizeBOMOrder()
        {
            // Step 1: Load stock data based on the selected warehouse
            foreach (ClientWarehouse w in warehouses)
            {
                if (comboBox6.SelectedItem == w.clName)
                {
                    isSql = true;
                    StockViewDataLoader(w.sqlStock, "STOCK");
                    break;
                }
            }

            if (selectedBOMs.Count >= 2)
            {
                // Step 2: Create a new list to store kits with their completion status
                var kitCompletionStatus = new List<Tuple<BOMList, int, int, int, int>>(); // BOMList, fullyStockedItemsBefore, totalItems, fullyStockedItemsAfter
                foreach (var bom in selectedBOMs)
                {
                    int totalItems = bom.Items.Count;
                    int fullyStockedItemsBefore = 0;   // Fully stocked items before simulation
                    int fullyStockedItemsAfter = 0; // Total fully stocked items after simulation

                    // Count items already in the kit
                    foreach (var kitItem in bom.Items)
                    {
                        if (kitItem.Delta >= 0) // Count fully stocked items
                        {
                            fullyStockedItemsBefore++;
                        }
                        else if (kitItem.Delta < 0)
                        {
                            //var stockItem = stockItems.Sum().(s => s.IPN == kitItem.IPN);

                            var stockItem = stockItems
    .Where(s => s.IPN == kitItem.IPN) // Filter based on the condition
    .Sum(s => s.Stock);
                            // Count items already in kit and available from stock

                            if (stockItem != null && stockItem >= Math.Abs((decimal)kitItem.Delta)) // Check if stock is available
                            {
                                // Add the stock available to the count, up to what is needed
                                fullyStockedItemsAfter++; // Use ?? to ensure it's a non-null value
                            }
                        }
                    }

                   

                    // Add the kit and its statuses to the list
                    kitCompletionStatus.Add(new Tuple<BOMList, int, int, int, int>(
                        bom,                              // BOMList
                        fullyStockedItemsBefore,         // Fully stocked items before simulation
                        totalItems,                       // Total items
                        fullyStockedItemsAfter,          // Fully stocked items after simulation
                        0)); // Add a default value for item5 (you need to replace this with actual logic if needed)
                }

                // Step 5: Create a sortable table to display the kits and their statuses
                Form kitsForm = new Form
                {
                    Text = "Kits Sorted by Completion",
                    Height = 600,
                    Width = 1500
                };

                DataGridView dataGridView = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoGenerateColumns = false
                };

                // Add columns to the DataGridView
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kit Name", DataPropertyName = "KitName" });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total Items", DataPropertyName = "TotalItems" });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "In kit", DataPropertyName = "FullyStockedBefore" });
              
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "In warehouse", DataPropertyName = "FullyStockedAfter" });
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Simulated Completion %", DataPropertyName = "CompletionPercentage" });

                var dataSource = kitCompletionStatus.Select(k =>
       new
       {
           KitName = k.Item1.Name,
           FullyStockedBefore = k.Item2, // Fully stocked before simulation
           TotalItems = k.Item3, // Total items
           FullyStockedAfter = k.Item4, // Fully stocked after simulation
           CompletionPercentage = k.Item3 > 0 ? $"{((double)(k.Item2 + k.Item4) / k.Item3) * 100:F2}%" : "0.00%" // Avoid division by zero
       })
       .OrderByDescending(x => x.CompletionPercentage) // Specify how to order by the CompletionPercentage
       .ToList();

                dataGridView.DataSource = dataSource;



                // Enable sorting and auto-sizing to fit contents
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Fit to all cell contents
                }

                // Step 7: Display the DataGridView in the form
                kitsForm.Controls.Add(dataGridView);

                
                kitsForm.ShowDialog();

                // Sort by CompletionPercentage in descending order
                //dataGridView.Sort(dataGridView.Columns["CompletionPercentage"], System.ComponentModel.ListSortDirection.Descending);

            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            //foreach (ClientWarehouse w in warehouses)
            //{
            //    if (comboBox6.SelectedItem == w.clName)
            //    {
            //        StockViewDataLoader(w.clStockFile, "STOCK");
            //    }
            //}
            //GenerateHtmlReport(false);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            SetSelectedBoms();
            foreach (ClientWarehouse w in warehouses)
            {
                if (comboBox6.SelectedItem == w.clName)
                {
                    if (w.sqlStock != null)
                    {
                        isSql = true;
                        StockViewDataLoader(w.sqlStock, "STOCK");
                    }
                    else
                    {
                        isSql = false;
                        StockViewDataLoader(w.clStockFile, "STOCK");
                    }
                }
            }
            var sumRequiredByIPN = selectedBOMs.SelectMany(bom => bom.Items)
                                   .GroupBy(item => item.IPN)
                                   .Select(group => new
                                   {
                                       IPN = group.Key,
                                       TotalRequired = group.Sum(item => item.Delta)
                                   });
            List<WHitem> stockItemsOriginal = new List<WHitem>();
            stockItemsOriginal.AddRange(stockItems);
            var stockData = selectedBOMs.SelectMany(bom => bom.Items)
                   .GroupBy(item => item.IPN)
                   .Select(group => new
                   {
                       IPN = group.Key,
                       MFPN = group.Select(x => x.MFPN).FirstOrDefault(),
                       Description = group.Select(x => x.Description).FirstOrDefault(),
                       TotalRequired = group.Sum(item => item.Delta)
                   })
                   .GroupJoin(stockItems,
                              sumItem => sumItem.IPN,
                              stockItem => stockItem.IPN,
                              (sumItem, stockItemGroup) => new
                              {
                                  IPN = sumItem.IPN,
                                  MFPN = sumItem.MFPN,
                                  Description = sumItem.Description,
                                  StockQuantity = stockItemGroup.Sum(item => item.Stock),
                                  TotalRequired = sumItem.TotalRequired
                              })
                   .OrderBy(item => item.TotalRequired);
            var stockDataDetailed = selectedBOMs
        .SelectMany(bom => bom.Items)
        .GroupBy(item => item.ProjectName)
        .Select(group => new
        {
            IPN = group.Key,
            BOMs = group.Select(item => new
            {
                Title = item.ProjectName,
                IPN = item.IPN,
                MFPN = item.MFPN,
                Description = item.Description,
                Quantity = item.Delta
            }),
            TotalRequired = group.Sum(item => item.Delta)
        })
                .OrderBy(item => item.BOMs.FirstOrDefault().Title);
            string fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            // Generating the HTML content
            // <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css'>
            string htmlContent = @"<!DOCTYPE html>
                        <html style='background-color: gray;'>
                        <head>
                        <style>
                        .lightcoral { background-color: lightcoral; }
                        .lightgreen { background-color: lightgreen; }
                         .sticky {
        position: sticky;
        top: 0;
        background-color: gray;
        z-index: 0;
    }
  #stockTable {
    max-width: 100%; /* Set the maximum width to 100% of the container (screen) */
    margin: 0 auto; /* Center the table within its container */
    border: 1px solid;
    text-align: center;
  }
    table {
        border-collapse: collapse;
        width: 100%;
    }
    th {
        position: sticky;
        top: calc(0rem + 1px);
        border: 1px solid black;
        background-color: gray;
        z-index: 1;
    }
     .wrap-content {
    overflow-wrap: break-word;
    word-wrap: break-word; /* For older browsers */
  }
                        </style>
                        <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
                        </head>";
            htmlContent += @"<body>";
            htmlContent += @"<table style='border: 1px solid; text-align: center; font-weight:bold;width:100%;'>";
            htmlContent += @"<tr><td style='width: 25%;'></td>";
            //htmlContent += @"<td style='width: 25%;height: 100%;' id='completion-chart-td'> <div> <canvas id='completion-chart'></canvas> </div></td>";
            htmlContent += @"<td style='width: 50%;'><h2>UPDATED_" + fileTimeStamp + "</h2></td>";
            htmlContent += @"<td style='width: 25%;'></td>";
            htmlContent += @"</tr>";
            //htmlContent += @"<tr style='text-align:center;'><td>Multi-BOM simulation for " + selectedBOMs.Count + " kits:</td></tr>";

            htmlContent += @"<tr style='text-align:center;'><td></td><td>Multi-BOM simulation for " + selectedBOMs.Count.ToString() + " kits:</td></tr>";

            foreach (var bom in selectedBOMs)
            {
                int bomPositiveDelta = 0;
                int bomNegativeDelta = 0;
                for (int i = 0; i < bom.Items.Count; i++)
                {
                    if (bom.Items[i].Delta >= 0)
                    {
                        bomPositiveDelta++;
                    }
                    else
                    {
                        bomNegativeDelta++;
                    }
                }
                int bomTot = bomPositiveDelta + bomNegativeDelta;
                double completionPercentage = 0;
                if (bomTot == bomPositiveDelta)
                {
                    completionPercentage = 100;
                }
                else
                {
                    completionPercentage = Math.Round((bomPositiveDelta * 100.0) / bomTot, 2);
                }
                htmlContent += $"<tr{(bomPositiveDelta == bomTot ? " style='background-color: lightgreen;'" : "")}><td></td><td>{bom.Name.TrimEnd(".xlsm".ToCharArray())} ({bomPositiveDelta}/{bomTot} IPNs in KIT) {completionPercentage}%</td></tr>";
            }
            //            htmlContent += @"<tr ><td><div id='completion-perc'> Average completion percentage is  </div></td></tr>";
            //            htmlContent += @" <tr><td>
            //<input type='text' id=""searchInput"" placeholder=""Filter IPN or MFPN.."" onkeyup=""filterTable()"" />
            //<button onclick=""clearFilter()"">Clear Filter</button></td></tr>";
            htmlContent += @"</tbody></table><br>";
            htmlContent += @"
    <style>
        #stockTableMain th {
            position: sticky;
            top: 0;
            background-color: #606060
;
        }
    </style>
    <table id='stockTableMain' class='wrap-content' style='border: 1px solid; text-align: center; width: 100%;'>
         <thead>        
            <tr>
                <th style='width: 28%;'>Project</th>
                <th style='width: 12%;'>IPN</th>
                <th style='width: 12%;'>MFPN</th>
                <th style='width: 12%;'>Description</th>
                <th style='width: 12%;'>WH Qty</th>
                <th style='width: 12%;'>KITs BALANCE</th>
                <th style='width: 12%;'>DELTA</th>
            </tr>
        </thead>";
            List<SIMIPNTABLE> MAINDATASOURCE_LIST = new List<SIMIPNTABLE>();
            foreach (var item in stockDataDetailed)
            {
                SIMIPNTABLE MAINDATASOURCE = new SIMIPNTABLE();
                MAINDATASOURCE.IPN = item.IPN;
                //MAINDATASOURCE.WHqty = stockItems.Where(si => si.IPN == item.IPN).Sum(si => si.Stock);
                //MAINDATASOURCE.KITsBalance = item.TotalRequired;
                //MAINDATASOURCE.DELTA = MAINDATASOURCE.WHqty + MAINDATASOURCE.KITsBalance;
                MAINDATASOURCE.BOMITEMS = new List<BOMitem>();
                foreach (var bomItem in item.BOMs)
                {
                    BOMitem b = new BOMitem();
                    b.ProjectName = bomItem.Title;
                    b.MFPN = bomItem.MFPN;
                    b.IPN = bomItem.IPN;
                    b.Description = bomItem.Description;
                    b.QtyInKit = bomItem.Quantity;
                    MAINDATASOURCE.BOMITEMS.Add(b);
                }
                MAINDATASOURCE_LIST.Add(MAINDATASOURCE);
            }
            // Order the MAINDATASOURCE_LIST by DELTA
            MAINDATASOURCE_LIST = MAINDATASOURCE_LIST.OrderBy(mainDataSource => mainDataSource.DELTA).ToList();
            // Generate HTML
            htmlContent += "<table border='1' style='border-collapse: collapse; width: 100%;'>";
            //htmlContent += "<tr style='background-color: #f2f2f2;'>";
            //htmlContent += "<th style='padding: 10px;'>IPN</th>";
            //htmlContent += "<th style='padding: 10px;'>Warehouse Quantity</th>";
            //htmlContent += "<th style='padding: 10px;'>KITs Balance</th>";
            //htmlContent += "<th style='padding: 10px;'>Delta</th>";
            //htmlContent += "</tr>";
            foreach (var mainDataSource in MAINDATASOURCE_LIST)
            {
                var rowColorClass = mainDataSource.DELTA < 0 ? "lightcoral" : "lightgreen";
                // Add main data row
                htmlContent += $"<tr class='{rowColorClass}' style='text-align:center;border: 1px solid black;'>";
                string truncatedTitle = mainDataSource.IPN.Length > 5
                           ? mainDataSource.IPN.Substring(0, mainDataSource.IPN.Length - 5)
                           : mainDataSource.IPN;
                htmlContent += $"<td style='width:64%;' columnspan='4'>{truncatedTitle}</td>";
                htmlContent += "</tr>";
                // Add sub-table for BOM items style='border: 1px solid black;'
                htmlContent += "<tr>";
                htmlContent += "<td  colspan='7'>";
                htmlContent += "<table border='1' style='border-collapse: collapse; width: 100%;border: 1px solid black;'>";
                foreach (var currentBomItem in mainDataSource.BOMITEMS)
                {
                    int whQty = stockItems.Where(si => si.IPN == currentBomItem.IPN).Sum(si => si.Stock);
                    int? totalReqPerIPN = stockData
     .Where(si => si.IPN == currentBomItem.IPN)
     .Select(w => w.TotalRequired)
     .FirstOrDefault();
                    int totalStockPerIpn = stockItemsOriginal.Where(si => si.IPN == currentBomItem.IPN).Sum(si => si.Stock);
                    int totalDeltaforIPN = totalStockPerIpn + int.Parse(totalReqPerIPN.ToString());
                    if (currentBomItem.QtyInKit < 0 && (currentBomItem.QtyInKit + whQty) < 0 && totalDeltaforIPN < 0)
                    {
                        htmlContent += "<tr style='text-align:center;'>";
                        htmlContent += $"<td class='wrap-content' style='width:12%;white-space: nowrap;'>{currentBomItem.IPN}</td>";
                        htmlContent += $"<td class='wrap-content' style='width:12%;'>{currentBomItem.MFPN}</td>";
                        htmlContent += $"<td class='wrap-content' style='width:36%;' columnspan='3'>{currentBomItem.Description}</td>";
                        if (whQty < 0)
                        {
                            htmlContent += $"<td style='width:12%;'>0 ({whQty})</td>";
                        }
                        else
                        {
                            htmlContent += $"<td style='width:12%;'>{whQty}</td>";
                        }
                        var rowColorClassQ = currentBomItem.QtyInKit < 0 ? "lightcoral" : "lightgreen";
                        htmlContent += $"<td style='width:12%;'>{currentBomItem.QtyInKit}</td>";
                        htmlContent += $"<td class='{rowColorClassQ}' style='width:12%;'>{totalDeltaforIPN}</td>";
                        WHitem deducter = new WHitem();
                        deducter.IPN = currentBomItem.IPN;
                        deducter.Stock = int.Parse(currentBomItem.QtyInKit.ToString());
                        stockItems.Add(deducter);
                        htmlContent += "</tr>";
                    }
                    else
                    {
                        WHitem deducter = new WHitem();
                        deducter.IPN = currentBomItem.IPN;
                        deducter.Stock = int.Parse(currentBomItem.QtyInKit.ToString());
                        stockItems.Add(deducter);
                        // Handle other cases if needed
                    }
                }
                // Close sub-table
                htmlContent += "</table>";
                htmlContent += "</br>";
                htmlContent += "</td>";
                htmlContent += "</tr>";
            }
            // Close the main table
            htmlContent += "</table>";
            htmlContent += "</div></table>";
            htmlContent += @"<script>
window.onload = function() {
     CalculateCompletion();
document.addEventListener('DOMContentLoaded', function() {
    // Your sorting function here
    sortTablesByDelta();
});
    };
function sortTablesByDelta() {
    var mainTable = document.getElementById('stockTableMain');
    var subTables = mainTable.querySelectorAll('table[id^=""stockTable_""]');
    console.log(subTables);
    subTables = Array.from(subTables).sort(function (a, b) {
        var ipnA = a.id.split('_')[1]; // Extract IPN from the table ID
        var ipnB = b.id.split('_')[1];
        var deltaA = parseFloat(a.querySelector('#delta_' + ipnA).innerText);
        var deltaB = parseFloat(b.querySelector('#delta_' + ipnB).innerText);
        return isNaN(deltaA) || isNaN(deltaB) ? 0 : deltaA - deltaB;
    });
    // Clear the main table
    while (mainTable.firstChild) {
        mainTable.removeChild(mainTable.firstChild);
    }
    // Append sorted sub-tables to the main table
    for (var i = 0; i < subTables.length; i++) {
        mainTable.appendChild(subTables[i]);
    }
}
    function filterTable() {
        var input, filter, table, tr, tdIPN, tdMFPN, txtValueIPN, txtValueMFPN, i;
        input = document.getElementById('searchInput');
        filter = input.value.toUpperCase();
        table = document.getElementById('stockTable');
        tr = table.getElementsByTagName('tr');
        for (i = 0; i < tr.length; i++) {
            tdIPN = tr[i].getElementsByTagName('td')[0];
            tdMFPN = tr[i].getElementsByTagName('td')[1];
            if (tdIPN || tdMFPN) {
                txtValueIPN = tdIPN.textContent || tdIPN.innerText;
                txtValueMFPN = tdMFPN.textContent || tdMFPN.innerText;
                if (txtValueIPN.toUpperCase().indexOf(filter) > -1 || txtValueMFPN.toUpperCase().indexOf(filter) > -1) {
                    tr[i].style.display = '';
                } else {
                    tr[i].style.display = 'none';
                }
            }
        }
    }
    function clearFilter() {
        document.getElementById('searchInput').value = '';
        var table = document.getElementById('stockTable');
        var tr = table.getElementsByTagName('tr');
        for (var i = 0; i < tr.length; i++) {
            tr[i].style.display = '';
        }
        document.getElementById('searchInput').focus();
    }
    function CalculateCompletion() {
        var lightcoralRows = document.querySelectorAll('.lightcoral');
        var lightcoralCount = lightcoralRows.length;
        var lightgreenRows = document.querySelectorAll('.lightgreen');
        var lightgreenCount = lightgreenRows.length;
        var totalRows = lightcoralCount + lightgreenCount;
        var percentage = (lightgreenCount / totalRows) * 100;
        var completionPercDiv = document.getElementById('completion-perc');
        completionPercDiv.textContent = ""Average KIT vs DB simulation is "" + percentage.toFixed(2) + ""%"" + "" ( ""+lightgreenCount+""/""+totalRows+"" rows )"";
var ctx = document.getElementById('completion-chart').getContext('2d');
var myPieChart = new Chart(ctx, {
    type: 'doughnut',
    data: {
        labels: ['Deficient', 'Sufficient'],
        datasets: [{
            data: [lightcoralCount],
            backgroundColor: ['#FF6384']
        }]
    },
    options: {
        plugins: {
            legend: {
                display: false, // Hide the legend
            },
            tooltip: {
                callbacks: {
                    label: function(context) {
                        var label = context.label || '';
                        var value = context.parsed || 0;
                        return label + ': ' + value;
                    }
                }
            }
        }
    }
});
    }
    document.getElementById('searchInput').focus();
  document.addEventListener('DOMContentLoaded', function() {
    var completionChartTd = document.getElementById('completion-chart-td');
    if (completionChartTd) {
      var numberOfRows = document.querySelectorAll('table tr').length;
      completionChartTd.rowSpan = numberOfRows;
    } else {
      console.error(""Element with ID 'completion-chart-td' not found"");
    }
  });
</script>";
            htmlContent += "</body></html>";
            string filename = @"\\dbr1\Data\WareHouse\2024\WHsearcher\" + fileTimeStamp + "_BOMs_sim" + ".html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(htmlContent);
            }
            // Opening the HTML file in the default browser
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            process.Start();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            loadedBOMs.Clear();
            selectedBOMs.Clear();
            if (fileNames != null && fileNames.Length > 0)
            {
                foreach (string fileName in fileNames)
                {
                    string theExcelFilePath = Path.GetFileName(fileName);
                    string Litem = Path.GetFileName(fileName);
                    DataLoader(fileName, theExcelFilePath);
                    // Add the selected file path to the list
                    selectedFileNames.Add(theExcelFilePath);
                }
            }
            PopulateDataGridView();
            SetSelectedBoms();
        }

        string[] loadedfiles { get; set; }

        private void button8_Click(object sender, EventArgs e)
        {
            // Get the warehouse name from comboBox6
            string selectedWarehouseName = comboBox6.SelectedItem.ToString();

            // Construct the directory path for the current month
            string directoryPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.ToString("MM.yyyy");

            // Get all files in the directory that start with the selected warehouse name and have the .xlsm extension
            loadedfiles = Directory.GetFiles(directoryPath, $"{selectedWarehouseName}*.xlsm");

            if (loadedfiles.Length == 0)
            {
                MessageBox.Show("No files found for the selected warehouse.");
                return;
            }

            foreach (string fileName in loadedfiles)
            {
                string theExcelFilePath = Path.GetFileName(fileName);
                if (IsFileLoaded(theExcelFilePath))
                {
                    MessageBox.Show($"File {theExcelFilePath} is already loaded!");
                }
                else
                {
                    DataLoader(fileName, theExcelFilePath);
                    // Add the selected file path to the list
                    selectedFileNames.Add(theExcelFilePath);
                }
            }

            // Update the DataGridView and selected BOMs
            PopulateDataGridView();
            SetSelectedBoms();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Check if a warehouse has been selected
            if (comboBox6.SelectedItem == null)
            {
                // If no selection has been made, open the drop-down list
                MessageBox.Show("Please select a warehouse before proceeding.");
                comboBox6.DroppedDown = true; // Opens the drop-down list
                return; // Exit the method until a selection is made
            }

            // Get the warehouse name from comboBox6
            string selectedWarehouseName = comboBox6.SelectedItem.ToString();

            // Construct the directory paths for the current and previous months
            string currentMonthPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.ToString("MM.yyyy");
            string previousMonthPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.AddMonths(-1).ToString("MM.yyyy");

            // Get all files in the directories that start with the selected warehouse name and have the .xlsm extension
            string[] currentMonthFiles = Directory.GetFiles(currentMonthPath, $"{selectedWarehouseName}*.xlsm");
            string[] previousMonthFiles = Directory.GetFiles(previousMonthPath, $"{selectedWarehouseName}*.xlsm");

            // Combine the files from both months
            string[] allFiles = currentMonthFiles.Concat(previousMonthFiles).ToArray();

            if (allFiles.Length == 0)
            {
                MessageBox.Show("No files found for the selected warehouse.");
                return;
            }

            foreach (string fileName in allFiles)
            {
                string theExcelFilePath = Path.GetFileName(fileName);
                if (IsFileLoaded(theExcelFilePath))
                {
                    MessageBox.Show($"File {theExcelFilePath} is already loaded!");
                }
                else
                {
                    DataLoader(fileName, theExcelFilePath);
                    // Add the selected file path to the list
                    selectedFileNames.Add(theExcelFilePath);
                }
            }

            // Update the DataGridView and selected BOMs
            PopulateDataGridView();
            SetSelectedBoms();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Check if a warehouse has been selected
            if (comboBox6.SelectedItem == null)
            {
                // If no selection has been made, open the drop-down list
                MessageBox.Show("Please select a warehouse before proceeding.");
                comboBox6.DroppedDown = true; // Opens the drop-down list
                return; // Exit the method until a selection is made
            }

            // Get the warehouse name from comboBox6
            string selectedWarehouseName = comboBox6.SelectedItem.ToString();

            // Construct the directory paths for the current and previous three months
            string currentMonthPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.ToString("MM.yyyy");
            string previousMonthPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.AddMonths(-1).ToString("MM.yyyy");
            string prepreviousMonthPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.AddMonths(-2).ToString("MM.yyyy");

            // Get all files in the directories that start with the selected warehouse name and have the .xlsm extension
            string[] currentMonthFiles = Directory.GetFiles(currentMonthPath, $"{selectedWarehouseName}*.xlsm");
            string[] previousMonthFiles = Directory.GetFiles(previousMonthPath, $"{selectedWarehouseName}*.xlsm");
            string[] prepreviousMonthFiles = Directory.GetFiles(prepreviousMonthPath, $"{selectedWarehouseName}*.xlsm");

            // Combine the files from all three months
            string[] allFiles = currentMonthFiles
                .Concat(previousMonthFiles)
                .Concat(prepreviousMonthFiles)
                .ToArray();

            if (allFiles.Length == 0)
            {
                MessageBox.Show("No files found for the selected warehouse.");
                return;
            }

            foreach (string fileName in allFiles)
            {
                string theExcelFilePath = Path.GetFileName(fileName);
                if (IsFileLoaded(theExcelFilePath))
                {
                    MessageBox.Show($"File {theExcelFilePath} is already loaded!");
                }
                else
                {
                    DataLoader(fileName, theExcelFilePath);
                    // Add the selected file path to the list
                    selectedFileNames.Add(theExcelFilePath);
                }
            }

            // Update the DataGridView and selected BOMs
            PopulateDataGridView();
            SetSelectedBoms();
        }

        private void button11_Click(object sender, EventArgs e)
        {

            // Check if a warehouse has been selected
            if (comboBox6.SelectedItem == null)
            {
                // If no selection has been made, open the drop-down list
                MessageBox.Show("Please select a warehouse before proceeding.");
                comboBox6.DroppedDown = true; // Opens the drop-down list
                return; // Exit the method until a selection is made
            }

            // Get the warehouse name from comboBox6
            string selectedWarehouseName = comboBox6.SelectedItem.ToString();

            // Construct the directory paths for the current and previous three months
            string currentMonthPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.ToString("MM.yyyy");
            string previousMonthPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.AddMonths(-1).ToString("MM.yyyy");
            string prepreviousMonthPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.AddMonths(-2).ToString("MM.yyyy");
            string preprepreviousMonthPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.AddMonths(-3).ToString("MM.yyyy");

            // Get all files in the directories that start with the selected warehouse name and have the .xlsm extension
            string[] currentMonthFiles = Directory.GetFiles(currentMonthPath, $"{selectedWarehouseName}*.xlsm");
            string[] previousMonthFiles = Directory.GetFiles(previousMonthPath, $"{selectedWarehouseName}*.xlsm");
            string[] prepreviousMonthFiles = Directory.GetFiles(prepreviousMonthPath, $"{selectedWarehouseName}*.xlsm");
            string[] preprepreviousMonthFiles = Directory.GetFiles(preprepreviousMonthPath, $"{selectedWarehouseName}*.xlsm");

            // Combine the files from all three months
            string[] allFiles = currentMonthFiles
                .Concat(previousMonthFiles)
                .Concat(prepreviousMonthFiles)
                .Concat(preprepreviousMonthFiles)
                .ToArray();

            if (allFiles.Length == 0)
            {
                MessageBox.Show("No files found for the selected warehouse.");
                return;
            }

            foreach (string fileName in allFiles)
            {
                string theExcelFilePath = Path.GetFileName(fileName);
                if (IsFileLoaded(theExcelFilePath))
                {
                    MessageBox.Show($"File {theExcelFilePath} is already loaded!");
                }
                else
                {
                    DataLoader(fileName, theExcelFilePath);
                    // Add the selected file path to the list
                    selectedFileNames.Add(theExcelFilePath);
                }
            }

            // Update the DataGridView and selected BOMs
            PopulateDataGridView();
            SetSelectedBoms();
        }
    }
}
