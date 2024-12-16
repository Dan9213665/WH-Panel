using FastMember;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Office.Interop.Excel;
using Seagull.Framework.Extensions;
using Seagull.Framework.Utility;
using System;
using System.Collections;
using System.Collections.Concurrent;
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
using Application = System.Windows.Forms.Application;
using Button = System.Windows.Forms.Button;
using CheckBox = System.Windows.Forms.CheckBox;
using ComboBox = System.Windows.Forms.ComboBox;
using DataTable = System.Data.DataTable;
using Font = System.Drawing.Font;
using GroupBox = System.Windows.Forms.GroupBox;
using Label = System.Windows.Forms.Label;
using ListBox = System.Windows.Forms.ListBox;
using Point = System.Drawing.Point;
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
        string[] loadedfiles { get; set; }
        string[] allFiles { get; set; }
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
            label1.Update();
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
            folderColumn.Name = "Folder";
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
                if (allFiles != null && allFiles.Any())
                {
                    string folderText = allFiles
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
                        //bool isChecked = (bool)checkBoxCell.EditedFormattedValue;

                        bool isChecked = (checkBoxCell.Value != null && (bool)checkBoxCell.Value);

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


        private void dataGridView1_CurrentCellDirtyStateChanged_1(object sender, EventArgs e)
        {
            // Check if the current cell is a checkbox cell
            if (dataGridView1.CurrentCell is DataGridViewCheckBoxCell)
            {
                // Commit the edit to trigger CellValueChanged event
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the changed cell is in the "Calculate" column
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].HeaderText == "Calculate")
            {
                DataGridViewCheckBoxCell checkBoxCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                if (checkBoxCell != null)
                {
                    bool isChecked = (bool)(checkBoxCell.Value ?? false);
                    SetSelectedBoms(); // Call regardless of the checkbox state
                }
            }
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
                                    SetSelectedBoms();
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

        private bool warehouseSelected = false; // Tracks if the warehouse is already selected
         
        private void CheckAndSetWarehouse(string firstIPN)
        {
            foreach (ClientWarehouse warehouse in warehouses)
            {
                if (firstIPN.StartsWith(warehouse.clPrefix))
                {
                    if (warehouse.clPrefix == "LDT" && !warehouseSelected)
                    {
                        // Create a dynamic form for warehouse selection
                        Form warehouseSelectionForm = new Form
                        {
                            Text = "Select Warehouse",
                            Size = new Size(610, 225),
                            StartPosition = FormStartPosition.CenterScreen
                        };

                        // Ensure that warehouse.clLogo contains a valid path or resource identifier
                        string logoPathAYS = warehouses
                            .Where(x => x.clName == "AYS")
                            .Select(x => x.clLogo)
                            .FirstOrDefault();

                        string logoPathSTXI = warehouses
                            .Where(x => x.clName == "STXI")
                            .Select(x => x.clLogo)
                            .FirstOrDefault();

                        try
                        {
                            // Load the logos dynamically from warehouse.clLogo
                            Image imageAYS = Image.FromFile(logoPathAYS);  // Assuming clLogo is a valid path
                            Image imageSTXI = Image.FromFile(logoPathSTXI); // Assuming clLogo is a valid path

                            // Create AYS button with logo
                            Button buttonAYS = new Button
                            {
                                Text = "AYS",
                                Image = imageAYS,
                                TextAlign = ContentAlignment.TopLeft,
                                Font = new Font("Microsoft Sans Serif", 40),  // Set the font size here
                                ImageAlign = ContentAlignment.MiddleCenter,
                                BackgroundImageLayout = ImageLayout.Zoom,
                                Width = 275,
                                Height = 150,
                                Location = new Point(15, 25),
                                AutoSize = false  // Disable AutoSize
                            };


                            // Set the background image layout to Stretch to fill the button
                            buttonAYS.Click += (sender, e) =>
                            {
                                warehouse.clName = "AYS";
                                comboBox6.SelectedItem = warehouse.clName;
                                warehouseSelected = true; // Mark as selected to avoid re-prompting
                                warehouseSelectionForm.DialogResult = DialogResult.OK;
                                warehouseSelectionForm.Close();
                            };

                            // Create STXI button with logo
                            Button buttonSTXI = new Button
                            {
                                // Text = "STXI",
                                Image = imageSTXI,
                                TextAlign = ContentAlignment.BottomCenter,
                                ImageAlign = ContentAlignment.MiddleCenter,
                                BackgroundImageLayout = ImageLayout.Zoom,
                                Width = 275,
                                Height = 150,
                                Location = new Point(310, 25),
                                AutoSize = false  // Disable AutoSize
                            };

                            // Set the background image layout to Stretch to fill the button
                            buttonSTXI.Click += (sender, e) =>
                            {
                                warehouse.clName = "STXI";
                                comboBox6.SelectedItem = warehouse.clName;
                                warehouseSelected = true; // Mark as selected to avoid re-prompting
                                warehouseSelectionForm.DialogResult = DialogResult.OK;
                                warehouseSelectionForm.Close();
                            };

                            // Add the buttons to the form
                            warehouseSelectionForm.Controls.Add(buttonAYS);
                            warehouseSelectionForm.Controls.Add(buttonSTXI);

                            // Show the form as a dialog
                            warehouseSelectionForm.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error loading warehouse logos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        break;
                    }
                    else if (!warehouse.clPrefix.Equals("LDT"))
                    {
                        comboBox6.SelectedItem = warehouse.clName;
                        warehouseSelected = true; // Mark as selected to break after the first match
                        break;
                    }
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
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    // Right-click logic can be added later
        //}
        private async void button2_Click(object sender, EventArgs e)
        {
            // Create a new form for the popup window
            Form popupForm = new Form
            {
                Width = 800,
                Height = 600,
                StartPosition = FormStartPosition.CenterScreen,
                Text = "IPN Kit Usage Overview"
            };

            // Create and configure the DataGridView
            DataGridView dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            // Add the DataGridView to the popup form
            popupForm.Controls.Add(dataGridView);

            // Populate the DataGridView asynchronously
            await PopulateDataGridViewAsync(dataGridView);

            // Show the popup form
            popupForm.ShowDialog();
        }

        //private async Task PopulateDataGridViewAsync(DataGridView dataGridView)
        //{
        //    dataGridView.Columns.Clear();
        //    dataGridView.Columns.Add("IPN", "IPN");

        //    //// Step 1: Add columns for each kit in selectedBOMs
        //    //foreach (var bom in selectedBOMs)
        //    //{
        //    //    dataGridView.Columns.Add(bom.Name, bom.Name);
        //    //}
        //    // Step 1: Add columns for each kit, displaying only the relevant part of bom.Name
        //    foreach (var bom in selectedBOMs)
        //    {
        //        string[] nameParts = bom.Name.Split('_');
        //        string displayName = nameParts.Length >= 3 ? $"{nameParts[1]}_{nameParts[2].Replace(".xlsm", "")}" : bom.Name;
        //        dataGridView.Columns.Add(displayName, displayName);
        //    }

        //    // Step 2: Prepare a dictionary to track IPNs and their DELTA values across kits
        //    var ipnData = new Dictionary<string, Dictionary<string, int?>>(); // Nullable int for DELTA

        //    foreach (var bom in selectedBOMs)
        //    {
        //        foreach (var item in bom.Items)
        //        {
        //            if (!ipnData.ContainsKey(item.IPN))
        //            {
        //                ipnData[item.IPN] = new Dictionary<string, int?>();
        //            }

        //            ipnData[item.IPN][bom.Name] = item.Delta; // Add DELTA for IPN in this kit
        //        }
        //    }

        //    // Step 3: Populate DataGridView rows with IPN data
        //    foreach (var ipn in ipnData.OrderByDescending(entry => entry.Value.Count(v => v.Value.HasValue)))
        //    {
        //        var row = new DataGridViewRow();
        //        row.CreateCells(dataGridView);

        //        row.Cells[0].Value = ipn.Key; // IPN

        //        foreach (var bom in selectedBOMs)
        //        {
        //            // Extract the relevant part of bom.Name for the column header
        //            string[] nameParts = bom.Name.Split('_');
        //            string displayName = nameParts.Length >= 3 ? $"{nameParts[1]}_{nameParts[2].Replace(".xlsm", "")}" : bom.Name;

        //            // Use the extracted displayName to access the correct column index
        //            if (dataGridView.Columns.Contains(displayName))
        //            {
        //                if (ipn.Value.TryGetValue(bom.Name, out var delta))
        //                {
        //                    row.Cells[dataGridView.Columns[displayName].Index].Value = delta;

        //                    // Apply color based on delta value
        //                    if (delta >= 0)
        //                    {
        //                        row.Cells[dataGridView.Columns[displayName].Index].Style.BackColor = Color.LightGreen;
        //                    }
        //                    else if (delta < 0)
        //                    {
        //                        row.Cells[dataGridView.Columns[displayName].Index].Style.BackColor = Color.IndianRed;
        //                    }
        //                    // No color for delta == 0 or null
        //                }
        //                else
        //                {
        //                    row.Cells[dataGridView.Columns[displayName].Index].Value = string.Empty;
        //                }
        //            }
        //        }

        //        dataGridView.Rows.Add(row);
        //    }


        //}


        private async Task PopulateDataGridViewAsync(DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();

            // Set default styles for DataGridView
            dataGridView.BackgroundColor = Color.LightGray; // Set background color to light gray
            dataGridView.DefaultCellStyle.BackColor = Color.LightGray; // Default cell background color
            dataGridView.DefaultCellStyle.Font = new Font(dataGridView.DefaultCellStyle.Font, FontStyle.Regular); // Default font style


            // Step 1: Add Total Appearances column and IPN column
            dataGridView.Columns.Add("Count", "Count"); // New count column
            var ipnColumn = new DataGridViewTextBoxColumn
            {
                Name = "IPN",
                HeaderText = "IPN",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells // Automatically adjusts the width to fit content
            };
            dataGridView.Columns.Add(ipnColumn);

            // Step 2: Add columns for each kit, displaying only the relevant part of bom.Name
            foreach (var bom in selectedBOMs)
            {
                string[] nameParts = bom.Name.Split('_');
                string displayName = nameParts.Length >= 3 ? $"{nameParts[1]}_{nameParts[2].Replace(".xlsm", "")}" : bom.Name;
                dataGridView.Columns.Add(displayName, displayName);
            }

            // Step 3: Prepare a dictionary to track IPNs and their DELTA values across kits
            var ipnData = new Dictionary<string, Dictionary<string, int?>>(); // Nullable int for DELTA

            foreach (var bom in selectedBOMs)
            {
                foreach (var item in bom.Items)
                {
                    if (!ipnData.ContainsKey(item.IPN))
                    {
                        ipnData[item.IPN] = new Dictionary<string, int?>();
                    }

                    ipnData[item.IPN][bom.Name] = item.Delta; // Add DELTA for IPN in this kit
                }
            }

            // Step 4: Populate DataGridView rows with IPN data
            foreach (var ipn in ipnData.OrderByDescending(entry => entry.Value.Count(v => v.Value.HasValue)))
            {
                var row = new DataGridViewRow();
                row.CreateCells(dataGridView);

                // Calculate the total appearances of the IPN across all kits
                int appearanceCount = ipn.Value.Values.Count(delta => delta.HasValue);

                row.Cells[0].Value = appearanceCount; // Total Appearances
                row.Cells[1].Value = ipn.Key; // IPN

                foreach (var bom in selectedBOMs)
                {
                    // Extract the relevant part of bom.Name for the column header
                    string[] nameParts = bom.Name.Split('_');
                    string displayName = nameParts.Length >= 3 ? $"{nameParts[1]}_{nameParts[2].Replace(".xlsm", "")}" : bom.Name;

                    // Use the extracted displayName to access the correct column index
                    if (dataGridView.Columns.Contains(displayName))
                    {
                        if (ipn.Value.TryGetValue(bom.Name, out var delta))
                        {
                            row.Cells[dataGridView.Columns[displayName].Index].Value = delta;
                            row.Cells[dataGridView.Columns[displayName].Index].Style.Font = new Font(dataGridView.DefaultCellStyle.Font, FontStyle.Bold); // Bold font for delta values

                            // Apply color based on delta value
                            if (delta >= 0)
                            {
                                row.Cells[dataGridView.Columns[displayName].Index].Style.BackColor = Color.LightGreen;
                                row.Cells[dataGridView.Columns[displayName].Index].Style.ForeColor = Color.Black;
                            }
                            else if (delta < 0)
                            {
                                row.Cells[dataGridView.Columns[displayName].Index].Style.BackColor = Color.IndianRed;
                                row.Cells[dataGridView.Columns[displayName].Index].Style.ForeColor = Color.White; // White text for IndianRed background
                            }
                            // No color for delta == 0 or null
                        }
                        else
                        {
                            row.Cells[dataGridView.Columns[displayName].Index].Value = string.Empty;
                        }
                    }
                }

                dataGridView.Rows.Add(row);
            }
        }



        //public class Int32Comparer : IComparer
        //{
        //    public int Compare(object x, object y)
        //    {
        //        if (x == null && y == null) return 0;
        //        if (x == null) return -1;
        //        if (y == null) return 1;

        //        if (int.TryParse(x.ToString(), out int intX) && int.TryParse(y.ToString(), out int intY))
        //            return intX.CompareTo(intY);

        //        return string.Compare(x.ToString(), y.ToString(), StringComparison.Ordinal);
        //    }
        //}

        //private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    var column = dataGridView.Columns[e.ColumnIndex];
        //    if (column.HeaderText != "IPN") // Apply only to kit columns
        //    {
        //        dataGridView.Sort(new Int32Comparer());
        //    }
        //    else
        //    {
        //        dataGridView.Sort(column, ListSortDirection.Ascending); // Default sorting for IPN
        //    }
        //}





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
        private async void button3_Click(object sender, EventArgs e)
        {
            SetSelectedBoms();

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

            //OptimizeBOMOrder();
            await OptimizeBOMOrderAsync();
        }



        private async Task OptimizeBOMOrderAsync()
        {
            // Step 1: Start the stopwatch to measure performance
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // Step 2: Initialize a thread-safe list to store kits with their completion status
            var kitCompletionStatus = new ConcurrentBag<Tuple<BOMList, int, int, int, int>>();

            // Parallelize the calculation for each kit
            await Task.WhenAll(selectedBOMs.Select(bom => Task.Run(() =>
            {
                int totalItems = bom.Items.Count;
                int fullyStockedItemsBefore = 0;
                int fullyStockedItemsAfter = 0;

                foreach (var kitItem in bom.Items)
                {
                    if (kitItem.Delta >= 0)
                    {
                        fullyStockedItemsBefore++;
                    }
                    else if (kitItem.Delta < 0)
                    {
                        var stockItem = stockItems
                            .Where(s => s.IPN == kitItem.IPN)
                            .Sum(s => s.Stock);

                        if (stockItem != null && stockItem >= Math.Abs((decimal)kitItem.Delta))
                        {
                            fullyStockedItemsAfter++;
                        }
                    }
                }

                kitCompletionStatus.Add(new Tuple<BOMList, int, int, int, int>(
                    bom, fullyStockedItemsBefore, totalItems, fullyStockedItemsAfter, 0));
            })));

            stopwatch.Stop(); // Stop the stopwatch once processing is done

            // Step 3: Calculate time taken and threads used
            var timeTaken = stopwatch.ElapsedMilliseconds;
            var threadsUsed = Environment.ProcessorCount;

            // Step 4: Create a form to display the kits sorted by completion
            Form kitsForm = new Form
            {
                Text = $"Kits Sorted by Completion (Time: {timeTaken} ms, Threads: {threadsUsed})",
                Height = 600,
                Width = 1500,
                StartPosition = FormStartPosition.CenterScreen,
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
                    FullyStockedBefore = k.Item2,
                    TotalItems = k.Item3,
                    FullyStockedAfter = k.Item4,
                    CompletionPercentage = k.Item3 > 0 ? $"{((double)(k.Item2 + k.Item4) / k.Item3) * 100:F2}%" : "0.00%"
                })
                .OrderByDescending(x => x.CompletionPercentage)
                .ToList();

            dataGridView.DataSource = dataSource;

            // Enable sorting and auto-sizing to fit contents
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // Step 7: Display the DataGridView in the form
            kitsForm.Controls.Add(dataGridView);
            kitsForm.ShowDialog();
        }


        private void OptimizeBOMOrder()
        {


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
                    Width = 1500,
                    StartPosition = FormStartPosition.CenterScreen, // Ensure you're using the correct enum type
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
        });
            // .OrderBy(item => item.BOMs.FirstOrDefault().Title);
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



        private void button8_Click(object sender, EventArgs e)
        {
            // Get the warehouse name from comboBox6
            string selectedWarehouseName = comboBox6.SelectedItem.ToString();

            // Construct the directory path for the current month
            string directoryPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.ToString("MM.yyyy");

            // Get all files in the directory that start with the selected warehouse name and have the .xlsm extension
            allFiles = Directory.GetFiles(directoryPath, $"{selectedWarehouseName}*.xlsm");

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
            allFiles = currentMonthFiles.Concat(previousMonthFiles).ToArray();

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
            allFiles = currentMonthFiles
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
            allFiles = currentMonthFiles
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

        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Assuming the checkbox column is at index 0 (change the index to match your setup)
                int checkboxColumnIndex = 0;

                bool allChecked = true;

                // First, check if all checkboxes are already checked
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[checkboxColumnIndex].Value != null && !(bool)row.Cells[checkboxColumnIndex].Value)
                    {
                        allChecked = false;
                        break;
                    }
                }

                // If all are checked, uncheck all; otherwise, check all
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Cells[checkboxColumnIndex].Value = !allChecked;
                }
            }
        }

        private async void button2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //// Left-click: Open DataGridView in a popup
                //Form popupForm = new Form
                //{
                //    Width = 800,
                //    Height = 600,
                //    StartPosition = FormStartPosition.CenterScreen,
                //    Text = "IPN Kit Usage Overview"
                //};

                //DataGridView dataGridView = new DataGridView
                //{
                //    Dock = DockStyle.Fill,
                //    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                //    ReadOnly = true,
                //    AllowUserToAddRows = false,
                //    BackgroundColor = Color.LightGray // Set default background color
                //};

                //popupForm.Controls.Add(dataGridView);
                //await PopulateDataGridViewAsync(dataGridView);

                //popupForm.ShowDialog();
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Right-click: Generate HTML report
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmm");
                string filePath = $@"\\dbr1\Data\WareHouse\2024\WHsearcher\qSim_{timestamp}_{comboBox6.SelectedItem.ToString()}.html";

                await GenerateHtmlReportAsync(filePath);
                // MessageBox.Show($"Report saved at: {filePath}", "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open the HTML file in the default web browser
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true // This is required to open with the default associated app
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open the report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        //private async Task GenerateHtmlReportAsync(string filePath)
        //{
        //    // Prepare the dictionary to track IPNs and their DELTA values across kits
        //    var ipnData = new Dictionary<string, Dictionary<string, int?>>(); // Nullable int for DELTA

        //    foreach (var bom in selectedBOMs)
        //    {
        //        foreach (var item in bom.Items)
        //        {
        //            if (!ipnData.ContainsKey(item.IPN))
        //            {
        //                ipnData[item.IPN] = new Dictionary<string, int?>();
        //            }

        //            ipnData[item.IPN][bom.Name] = item.Delta; // Add DELTA for IPN in this kit
        //        }
        //    }


        //    string connectionString = string.Empty;

        //    foreach (ClientWarehouse w in warehouses)
        //    {
        //        if (comboBox6.SelectedItem == w.clName)
        //        {
        //            if (w.sqlStock != null)
        //            {
        //                connectionString = w.sqlStock;
        //            }
        //        }
        //    }


        //    // Pull relevant stock data for IPNs from SQL
        //    var whStockData = new Dictionary<string, int>(); // Store summed stock for each IPN

        //    var ipnList = string.Join("','", ipnData.Keys.Select(ipn => ipn.Replace("'", "''"))); // Sanitize for SQL query

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        string query = $"SELECT IPN, SUM(CAST(Stock AS INT)) AS TotalStock FROM STOCK WHERE IPN IN ('{ipnList}') GROUP BY IPN";
        //        SqlCommand command = new SqlCommand(query, connection);
        //        connection.Open();
        //        using (SqlDataReader reader = await command.ExecuteReaderAsync())
        //        {
        //            while (reader.Read())
        //            {
        //                string ipn = reader["IPN"].ToString().Trim();
        //                int totalStock = Convert.ToInt32(reader["TotalStock"]);
        //                whStockData[ipn] = totalStock;
        //            }
        //        }
        //    }

        //    var htmlContent = new StringBuilder();

        //    // HTML header and CSS styles
        //    htmlContent.AppendLine("<html>");
        //    htmlContent.AppendLine("<head>");
        //    htmlContent.AppendLine("<style>");
        //    htmlContent.AppendLine("table { width: 100%; border-collapse: collapse; }");
        //    htmlContent.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: center; }");
        //    htmlContent.AppendLine("th { background-color: #f2f2f2; color: black; font-weight: bold; white-space: wrap; position: sticky; top: 0; z-index: 1; }");
        //    htmlContent.AppendLine(".rotated-header { transform: rotate(0deg); transform-origin: center center; white-space: wrap; width:20px; vertical-align: center; text-align: center; }");
        //    htmlContent.AppendLine(".positive { background-color: LightGreen; }");
        //    htmlContent.AppendLine(".negative { background-color: IndianRed; color: white; }");
        //    htmlContent.AppendLine(".nowrap { white-space: nowrap; margin:1px;padding:1px;font-weight:bold; }");
        //    htmlContent.AppendLine(".bold { font-weight: bold; }");
        //    htmlContent.AppendLine("</style>");


        //    // Insert this section after your CSS in the HTML content generation
        //    htmlContent.AppendLine("<script>");

        //    htmlContent.AppendLine("document.addEventListener('DOMContentLoaded', function() {");
        //    htmlContent.AppendLine("    let headers = document.querySelectorAll('th');");
        //    htmlContent.AppendLine("    headers.forEach((header, index) => {");
        //    htmlContent.AppendLine("        header.addEventListener('click', () => {");
        //    htmlContent.AppendLine("            sortTable(index);");
        //    htmlContent.AppendLine("        });");
        //    htmlContent.AppendLine("    });");

        //    htmlContent.AppendLine("    let sortDirection = 1;"); // 1 for ascending, -1 for descending

        //    htmlContent.AppendLine("    function sortTable(columnIndex) {");
        //    htmlContent.AppendLine("        let table = document.querySelector('table tbody');");
        //    htmlContent.AppendLine("        let rows = Array.from(table.rows);");

        //    htmlContent.AppendLine("        // Toggle sort direction");
        //    htmlContent.AppendLine("        sortDirection = -sortDirection;");



        //    htmlContent.AppendLine("        rows.sort((rowA, rowB) => {");
        //    htmlContent.AppendLine("            let cellA = rowA.cells[columnIndex]?.textContent.trim() || '';");
        //    htmlContent.AppendLine("            let cellB = rowB.cells[columnIndex]?.textContent.trim() || '';");

        //    // Always place empty cells below non-empty cells
        //    htmlContent.AppendLine("            if (cellA === '' && cellB !== '') return 1;"); // Empty cell comes last
        //    htmlContent.AppendLine("            if (cellA !== '' && cellB === '') return -1;"); // Non-empty cell comes first

        //    // Parse as integers if possible, else as strings
        //    htmlContent.AppendLine("            let a = isNaN(cellA) || cellA === '' ? cellA : parseInt(cellA);");
        //    htmlContent.AppendLine("            let b = isNaN(cellB) || cellB === '' ? cellB : parseInt(cellB);");

        //    htmlContent.AppendLine("            if (a < b) return -sortDirection;");
        //    htmlContent.AppendLine("            if (a > b) return sortDirection;");
        //    htmlContent.AppendLine("            return 0;");
        //    htmlContent.AppendLine("        });");





        //    htmlContent.AppendLine("        // Append sorted rows back to the table");
        //    htmlContent.AppendLine("        rows.forEach(row => table.appendChild(row));");
        //    htmlContent.AppendLine("    }");
        //    htmlContent.AppendLine("});");



        //    htmlContent.AppendLine("</script>");



        //    htmlContent.AppendLine("</head>");
        //    htmlContent.AppendLine("<body style='background-color: grey;'>");

        //    htmlContent.AppendLine("<table>");
        //    htmlContent.AppendLine("<thead>");
        //    htmlContent.AppendLine("<tr>");
        //    htmlContent.AppendLine("<th class='nowrap'>WH</th>"); // Add WH column
        //    htmlContent.AppendLine("<th class='nowrap'>Balance</th>");
        //    htmlContent.AppendLine("<th class='nowrap'>Count</th>");
        //    htmlContent.AppendLine("<th class='nowrap'>IPN</th>");

        //    foreach (var bom in selectedBOMs)
        //    {
        //        string[] nameParts = bom.Name.Split('_');
        //        string displayName = nameParts.Length >= 3 ? $"{nameParts[1]}_{nameParts[2].Replace(".xlsm", "")}" : bom.Name;
        //        htmlContent.AppendLine($"<th class='rotated-header'>{displayName}</th>");
        //    }

        //    htmlContent.AppendLine("</tr>");
        //    htmlContent.AppendLine("</thead>");
        //    htmlContent.AppendLine("<tbody>");

        //    // Data rows with IPN, WH, balance, count, and delta values
        //    foreach (var ipnEntry in ipnData.OrderByDescending(entry => entry.Value.Count(v => v.Value.HasValue)))
        //    {
        //        string ipn = ipnEntry.Key;
        //        int totalAppearances = ipnEntry.Value.Count(v => v.Value.HasValue);
        //        int balance = ipnEntry.Value.Values.Where(v => v.HasValue).Sum(v => v ?? 0);
        //        int whStock = whStockData.ContainsKey(ipn) ? whStockData[ipn] : 0;

        //        //string balanceClass = balance >= 0 ? "positive" : "negative";
        //        //string whClass = whStock >= balance ? "positive" : "negative";

        //        string balanceClass = balance > 0 ? "positive" : (balance < 0 ? "negative" : "positive");
        //        string whClass = ((whStock >= Math.Abs(balance) && whStock > 0) || (whStock > 0 && balance >= 0)) ? "positive" : "negative";






        //        htmlContent.AppendLine("<tr>");
        //        htmlContent.AppendLine($"<td class='{whClass} bold'>{whStock}</td>"); // WH column with coloring
        //        htmlContent.AppendLine($"<td class='{balanceClass} bold'>{balance}</td>");
        //        htmlContent.AppendLine($"<td style='color:white;'>{totalAppearances}</td>");
        //        htmlContent.AppendLine($"<td style='color:white;'>{ipn}</td>");

        //        foreach (var bom in selectedBOMs)
        //        {
        //            if (ipnEntry.Value.TryGetValue(bom.Name, out var delta))
        //            {
        //                string cellClass = delta >= 0 ? "positive bold" : "negative bold";
        //                htmlContent.AppendLine($"<td class='{cellClass}'>{delta}</td>");
        //            }
        //            else
        //            {
        //                htmlContent.AppendLine("<td></td>");
        //            }
        //        }

        //        htmlContent.AppendLine("</tr>");
        //    }

        //    htmlContent.AppendLine("</tbody>");
        //    htmlContent.AppendLine("</table>");
        //    htmlContent.AppendLine("</body>");
        //    htmlContent.AppendLine("</html>");

        //    await File.WriteAllTextAsync(filePath, htmlContent.ToString());

        //    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        //    {
        //        FileName = filePath,
        //        UseShellExecute = true
        //    });
        //}


        private async Task GenerateHtmlReportAsync(string filePath)
        {
            // Prepare the dictionary to track IPNs and their DELTA values across kits
            var ipnData = new Dictionary<string, Dictionary<string, int?>>(); // Nullable int for DELTA

            foreach (var bom in selectedBOMs)
            {
                foreach (var item in bom.Items)
                {
                    if (!ipnData.ContainsKey(item.IPN))
                    {
                        ipnData[item.IPN] = new Dictionary<string, int?>();
                    }

                    ipnData[item.IPN][bom.Name] = item.Delta; // Add DELTA for IPN in this kit
                }
            }

            string connectionString = string.Empty;

            foreach (ClientWarehouse w in warehouses)
            {
                if (comboBox6.SelectedItem == w.clName)
                {
                    if (w.sqlStock != null)
                    {
                        connectionString = w.sqlStock;
                    }
                }
            }

            // Pull relevant stock data for IPNs from SQL
            var whStockData = new Dictionary<string, int>(); // Store summed stock for each IPN

            var ipnList = string.Join("','", ipnData.Keys.Select(ipn => ipn.Replace("'", "''"))); // Sanitize for SQL query

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"SELECT IPN, SUM(CAST(Stock AS INT)) AS TotalStock FROM STOCK WHERE IPN IN ('{ipnList}') GROUP BY IPN";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        string ipn = reader["IPN"].ToString().Trim();
                        int totalStock = Convert.ToInt32(reader["TotalStock"]);
                        whStockData[ipn] = totalStock;
                    }
                }
            }

            var htmlContent = new StringBuilder();

            // HTML header and CSS styles
            htmlContent.AppendLine("<html>");
            htmlContent.AppendLine("<head>");
            htmlContent.AppendLine("<style>");
            htmlContent.AppendLine("table { width: 100%; border-collapse: collapse; }");
            htmlContent.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: center; }");
            htmlContent.AppendLine("th { background-color: #f2f2f2; color: black; font-weight: bold; white-space: wrap; position: sticky; top: 0; z-index: 1; }");
            htmlContent.AppendLine(".rotated-header { transform: rotate(0deg); transform-origin: center center; white-space: wrap; width:20px; vertical-align: center; text-align: center; }");
            htmlContent.AppendLine(".positive { background-color: LightGreen; }");
            htmlContent.AppendLine(".negative { background-color: IndianRed; color: white; }");
            htmlContent.AppendLine(".nowrap { white-space: nowrap; margin:1px;padding:1px;font-weight:bold; }");
            htmlContent.AppendLine(".bold { font-weight: bold; }");
            htmlContent.AppendLine(".ripple { animation: rippleEffect 0.6s ease-out; }");
            htmlContent.AppendLine("@keyframes rippleEffect {");
            htmlContent.AppendLine("    0% { transform: scale(1); opacity: 0.7; }");
            htmlContent.AppendLine("    100% { transform: scale(1.2); opacity: 0; }");
            htmlContent.AppendLine("}");
            htmlContent.AppendLine("</style>");

            // Insert this section after your CSS in the HTML content generation
            htmlContent.AppendLine("<script>");
            htmlContent.AppendLine("document.addEventListener('DOMContentLoaded', function() {");
            htmlContent.AppendLine("    let headers = document.querySelectorAll('th');");
            htmlContent.AppendLine("    headers.forEach((header, index) => {");
            htmlContent.AppendLine("        header.addEventListener('click', () => {");
            htmlContent.AppendLine("            sortTable(index);");
            htmlContent.AppendLine("            header.classList.add('ripple');");  // Add ripple effect on sort
            htmlContent.AppendLine("            setTimeout(() => { header.classList.remove('ripple'); }, 300);"); // Remove ripple effect after animation
            htmlContent.AppendLine("        });");
            htmlContent.AppendLine("    });");

            htmlContent.AppendLine("    let sortDirection = 1;"); // 1 for ascending, -1 for descending

            htmlContent.AppendLine("    function sortTable(columnIndex) {");
            htmlContent.AppendLine("        let table = document.querySelector('table tbody');");
            htmlContent.AppendLine("        let rows = Array.from(table.rows);");

            htmlContent.AppendLine("        // Toggle sort direction");
            htmlContent.AppendLine("        sortDirection = -sortDirection;");

            htmlContent.AppendLine("        rows.sort((rowA, rowB) => {");
            htmlContent.AppendLine("            let cellA = rowA.cells[columnIndex]?.textContent.trim() || '';");
            htmlContent.AppendLine("            let cellB = rowB.cells[columnIndex]?.textContent.trim() || '';");

            htmlContent.AppendLine("            // Always place empty cells below non-empty cells");
            htmlContent.AppendLine("            if (cellA === '' && cellB !== '') return 1;"); // Empty cell comes last
            htmlContent.AppendLine("            if (cellA !== '' && cellB === '') return -1;"); // Non-empty cell comes first

            htmlContent.AppendLine("            let a = isNaN(cellA) || cellA === '' ? cellA : parseInt(cellA);");
            htmlContent.AppendLine("            let b = isNaN(cellB) || cellB === '' ? cellB : parseInt(cellB);");

            htmlContent.AppendLine("            if (a < b) return -sortDirection;");
            htmlContent.AppendLine("            if (a > b) return sortDirection;");
            htmlContent.AppendLine("            return 0;");
            htmlContent.AppendLine("        });");

            htmlContent.AppendLine("        // Append sorted rows back to the table");
            htmlContent.AppendLine("        rows.forEach(row => table.appendChild(row));");
            htmlContent.AppendLine("    }");
            htmlContent.AppendLine("});");
            htmlContent.AppendLine("</script>");

            htmlContent.AppendLine("</head>");
            htmlContent.AppendLine("<body style='background-color: grey;'>");

            htmlContent.AppendLine("<table>");
            htmlContent.AppendLine("<thead>");
            htmlContent.AppendLine("<tr>");
            htmlContent.AppendLine("<th class='nowrap'>WH</th>"); // Add WH column
            htmlContent.AppendLine("<th class='nowrap'>Balance</th>");
            htmlContent.AppendLine("<th class='nowrap'>Count</th>");
            htmlContent.AppendLine("<th class='nowrap'>IPN</th>");

            foreach (var bom in selectedBOMs)
            {
                string[] nameParts = bom.Name.Split('_');
                string displayName = nameParts.Length >= 3 ? $"{nameParts[1]}_{nameParts[2].Replace(".xlsm", "")}" : bom.Name;
                htmlContent.AppendLine($"<th class='rotated-header'>{displayName}</th>");
            }

            htmlContent.AppendLine("</tr>");
            htmlContent.AppendLine("</thead>");
            htmlContent.AppendLine("<tbody>");

            // Data rows with IPN, WH, balance, count, and delta values
            foreach (var ipnEntry in ipnData.OrderByDescending(entry => entry.Value.Count(v => v.Value.HasValue)))
            {
                string ipn = ipnEntry.Key;
                int totalAppearances = ipnEntry.Value.Count(v => v.Value.HasValue);
                int balance = ipnEntry.Value.Values.Where(v => v.HasValue).Sum(v => v ?? 0);
                int whStock = whStockData.ContainsKey(ipn) ? whStockData[ipn] : 0;

                string balanceClass = balance > 0 ? "positive" : (balance < 0 ? "negative" : "positive");
                string whClass = ((whStock >= Math.Abs(balance) && whStock > 0) || (whStock > 0 && balance >= 0)) ? "positive" : "negative";

                htmlContent.AppendLine("<tr>");
                htmlContent.AppendLine($"<td class='{whClass} bold'>{whStock}</td>"); // WH column with coloring
                htmlContent.AppendLine($"<td class='{balanceClass} bold'>{balance}</td>");
                htmlContent.AppendLine($"<td style='color:white;'>{totalAppearances}</td>");
                htmlContent.AppendLine($"<td style='color:white;'>{ipn}</td>");

                foreach (var bom in selectedBOMs)
                {
                    if (ipnEntry.Value.TryGetValue(bom.Name, out var delta))
                    {
                        string cellClass = delta >= 0 ? "positive bold" : "negative bold";
                        htmlContent.AppendLine($"<td class='{cellClass}'>{delta}</td>");
                    }
                    else
                    {
                        htmlContent.AppendLine("<td></td>");
                    }
                }

                htmlContent.AppendLine("</tr>");
            }

            htmlContent.AppendLine("</tbody>");
            htmlContent.AppendLine("</table>");
            htmlContent.AppendLine("</body>");
            htmlContent.AppendLine("</html>");

            await File.WriteAllTextAsync(filePath, htmlContent.ToString());

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }

        private void button12_Click(object sender, EventArgs e)
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
            string prepreprepreviousMonthPath = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.AddMonths(-4).ToString("MM.yyyy");

            // Get all files in the directories that start with the selected warehouse name and have the .xlsm extension
            string[] currentMonthFiles = Directory.GetFiles(currentMonthPath, $"{selectedWarehouseName}*.xlsm");
            string[] previousMonthFiles = Directory.GetFiles(previousMonthPath, $"{selectedWarehouseName}*.xlsm");
            string[] prepreviousMonthFiles = Directory.GetFiles(prepreviousMonthPath, $"{selectedWarehouseName}*.xlsm");
            string[] preprepreviousMonthFiles = Directory.GetFiles(preprepreviousMonthPath, $"{selectedWarehouseName}*.xlsm");
            string[] prepreprepreviousMonthFiles = Directory.GetFiles(prepreprepreviousMonthPath, $"{selectedWarehouseName}*.xlsm");

            // Combine the files from all three months
            allFiles = currentMonthFiles
               .Concat(previousMonthFiles)
               .Concat(prepreviousMonthFiles)
               .Concat(preprepreviousMonthFiles)
               .Concat(prepreprepreviousMonthFiles)
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

    
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the clicked cell is valid
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the column name of the clicked cell
                DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
                //MessageBox.Show($"Clicked Column: {column.Name}, Row: {e.RowIndex}");
                // Check if the column name is "Folder"
                if (column.Name == "Folder")
                {
                    // Get the file path from the clicked cell
                    string filePath = dataGridView1.Rows[e.RowIndex].Cells["Folder"].Value?.ToString();
                    //MessageBox.Show($"FilePath: {filePath}");

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        // Confirm with the user
                        DialogResult result = MessageBox.Show(
                            "Load the BOM : " + filePath + "?",
                            "Load BOM",
                            MessageBoxButtons.OKCancel);

                        if (result == DialogResult.OK)
                        {
                            // Check if Form1 is open
                            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
                            if (form1 != null)
                            {
                                // Initialize FrmBOM and open the file
                                FrmBOM frmBOM = new FrmBOM();
                                frmBOM.InitializeGlobalWarehouses(form1.PopulateWarehouses());
                                frmBOM.ExternalLinktoFile(filePath);
                                frmBOM.Show();
                                frmBOM.ReloadLogic();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No valid file path found in the selected cell.", "Error");
                    }
                }
            }
        }

    }
}
