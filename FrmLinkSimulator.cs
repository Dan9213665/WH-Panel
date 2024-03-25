using FastMember;
using Microsoft.Office.Interop.Excel;
using Seagull.Framework.Extensions;
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
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Button = System.Windows.Forms.Button;
using ComboBox = System.Windows.Forms.ComboBox;
using DataTable = System.Data.DataTable;
using GroupBox = System.Windows.Forms.GroupBox;
using Label = System.Windows.Forms.Label;
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
        public FrmLinkSimulator()
        {
            InitializeComponent();
            InitializeComboBoxes();
            //IntitializeWarehouses();
            UpdateControlColors(this);
            BOMs = new List<BOMList>();
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
        private void InitializeComboBoxes()
        {
            // Assuming comboBox1, comboBox2, and comboBox3 are the ComboBoxes in your form
            comboBox1.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
            comboBox3.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
            comboBox4.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
            comboBox5.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
        }
        private void UpdateControlColors(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
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
        public List<BOMList> BOMs { get; set; }
        public string fileName = string.Empty;
        public string theExcelFilePath = string.Empty;
        public DataTable BOM1Dtable = new DataTable();
        private void ComboBoxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox selectedComboBox = (ComboBox)sender;
            if (selectedComboBox == comboBox1)
            {
                groupBox6.Text = selectedComboBox.SelectedItem?.ToString() ?? "Select BOM in the combobox above";
                UpdateLabelBasedOnComboBoxSelection(label1, selectedComboBox);
            }
            else if (selectedComboBox == comboBox2)
            {
                groupBox7.Text = selectedComboBox.SelectedItem?.ToString() ?? "Select BOM in the combobox above";
                UpdateLabelBasedOnComboBoxSelection(label2, selectedComboBox);
            }
            else if (selectedComboBox == comboBox3)
            {
                groupBox8.Text = selectedComboBox.SelectedItem?.ToString() ?? "Select BOM in the combobox above";
                UpdateLabelBasedOnComboBoxSelection(label3, selectedComboBox);
            }
            else if (selectedComboBox == comboBox4)
            {
                groupBox10.Text = selectedComboBox.SelectedItem?.ToString() ?? "Select BOM in the combobox above";
                UpdateLabelBasedOnComboBoxSelection(label4, selectedComboBox);
            }
            else if (selectedComboBox == comboBox5)
            {
                groupBox12.Text = selectedComboBox.SelectedItem?.ToString() ?? "Select BOM in the combobox above";
                UpdateLabelBasedOnComboBoxSelection(label5, selectedComboBox);
            }
            // Remove the selected item from other ComboBoxes
            foreach (ComboBox comboBox in new[] { comboBox1, comboBox2, comboBox3, comboBox4, comboBox5 }.Where(c => c != selectedComboBox))
            {
                if (comboBox.SelectedItem == selectedComboBox.SelectedItem)
                {
                    comboBox.SelectedItem = null;
                }
            }
            // Load data based on the selected items in the ComboBoxes
            LoadDataIntoDataGridViews();
        }
        private void UpdateLabelBasedOnComboBoxSelection(Label label, ComboBox selectedComboBox)
        {
            int selectedComboBoxIndex = Array.IndexOf(new[] { comboBox1, comboBox2, comboBox3, comboBox4, comboBox5 }, selectedComboBox);
            if (selectedComboBoxIndex >= 0 && selectedComboBoxIndex < BOMs.Count)
            {
                int totalRows = BOMs[selectedComboBoxIndex].Items.Count;
                int positiveCount = BOMs[selectedComboBoxIndex].Items.Count(item => (item.Delta ?? 0) >= 0);
                double positivePercentage = (double)positiveCount / totalRows * 100;
                label.Text = $"Positive Delta Percentage: {positivePercentage:F2}%";
            }
        }
        private void LoadDataIntoDataGridViews()
        {
            for (int i = 0; i < 5; i++)
            {
                //if (i == 3) continue; // Skip the processing for ComboBox4 and DataGridView4
                ComboBox currentComboBox = Controls.Find($"comboBox{i + 1}", true).FirstOrDefault() as ComboBox;
                DataGridView currentDataGridView = Controls.Find($"dataGridView{i + 1}", true).FirstOrDefault() as DataGridView;
                if (currentComboBox.SelectedItem != null)
                {
                    int selectedIndex = currentComboBox.SelectedIndex;
                    PopulateBOMGridView(selectedIndex, currentDataGridView);
                }
                else
                {
                    currentDataGridView.DataSource = null;
                    currentDataGridView.Rows.Clear();
                    currentDataGridView.Refresh();
                }
            }
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
        }
        private void SetComboBoxItems(List<string> fileNames)
        {
            // Set the ComboBox items in the order of selected file paths
            comboBox1.SelectedItem = fileNames.Count > 0 ? fileNames[0] : null;
            comboBox2.SelectedItem = fileNames.Count > 1 ? fileNames[1] : null;
            comboBox3.SelectedItem = fileNames.Count > 2 ? fileNames[2] : null;
            comboBox4.SelectedItem = fileNames.Count > 3 ? fileNames[3] : null;
            comboBox5.SelectedItem = fileNames.Count > 4 ? fileNames[4] : null;
        }
        private bool IsFileLoaded(string fileName)
        {
            foreach (BOMList bom in BOMs)
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
            comboBox1.Items.Add(a.Name);
            comboBox2.Items.Add(a.Name);
            comboBox3.Items.Add(a.Name);
            comboBox4.Items.Add(a.Name);
            comboBox5.Items.Add(a.Name);
            richTextBox1.Text += a.Name + "\n";
            // Assuming comboBox1, comboBox2, and comboBox3 are the names of your ComboBox controls
            // Set the SelectedIndex for each ComboBox based on its own count
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
            comboBox3.SelectedIndex = comboBox3.Items.Count - 1;
            comboBox4.SelectedIndex = comboBox4.Items.Count - 1;
            comboBox5.SelectedIndex = comboBox5.Items.Count - 1;
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
                BOMs.Add(a);
                if (a.Items.Count > 0)
                {
                    CheckAndSetWarehouse(a.Items[0].IPN);
                }
                //PopulateBOMGridView();
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
        private void PopulateBOMGridView(int selectedIndex, DataGridView dataGridView)
        {
            //if (dataGridView.Name == "dataGridView4") return; // Skip Populating DataGridView4
            dataGridView.DataSource = null;
            dataGridView.Rows.Clear();
            dataGridView.Refresh();
            if (selectedIndex >= 0 && selectedIndex < BOMs.Count)
            {
                DataTable table = new DataTable();
                IEnumerable<KitHistoryItem> data = BOMs[selectedIndex].Items;
                using (var reader = ObjectReader.Create(data))
                {
                    table.Load(reader);
                }
                dataGridView.DataSource = table;
                SetColumsOrder(dataGridView);
            }
        }
        private void SetColumsOrder(DataGridView dgw)
        {
            dgw.Columns["DateOfCreation"].Visible = false;
            dgw.Columns["ProjectName"].Visible = false;
            dgw.Columns["Description"].Visible = false;
            dgw.Columns["Calc"].Visible = false;
            dgw.Columns["Alts"].Visible = false;
            dgw.Columns["MFPN"].Visible = false;
            dgw.Columns["QtyPerUnit"].Visible = false;
            dgw.Columns["IPN"].DisplayIndex = 0;
            dgw.Columns["QtyInKit"].DisplayIndex = 1;
            dgw.Columns["Delta"].DisplayIndex = 2;
            // Set AutoSizeMode for the displayed columns
            foreach (DataGridViewColumn column in dgw.Columns)
            {
                if (column.Visible)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            // Handling the CellFormatting event
            dgw.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == dgw.Columns["Delta"].Index && e.Value != null)
                {
                    // Change the cell color based on the value
                    var deltaValue = Convert.ToDecimal(e.Value);
                    if (deltaValue >= 0)
                    {
                        e.CellStyle.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.IndianRed;
                    }
                }
            };
            // Sorting by Delta column
            dgw.Sort(dgw.Columns["Delta"], ListSortDirection.Ascending);
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
                GenerateHtmlReport(false);
            }
            else if (Control.MouseButtons == MouseButtons.Right)
            {
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
            var stockDataDetailed = BOMs
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
            htmlContent += @"<tr><td>Multi-BOM simulation for " + BOMs.Count + " kits:</td></tr>";
            foreach (var bom in BOMs)
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
                                    IPN = row["IPN"].ToString(),
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
            var sumRequiredByIPN = BOMs.SelectMany(bom => bom.Items)
                                      .GroupBy(item => item.IPN)
                                      .Select(group => new
                                      {
                                          IPN = group.Key,
                                          TotalRequired = group.Sum(item => item.Delta)
                                      });
            var stockData = BOMs.SelectMany(bom => bom.Items)
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
            htmlContent += @"<tr><td>Multi-BOM simulation for " + BOMs.Count + " kits:</td></tr>";
            if (limitOrNot)
            {
                string[] comboBoxItems = new string[] {
                                        comboBox1.SelectedItem?.ToString(),
                                        comboBox2.SelectedItem?.ToString(),
                                        comboBox3.SelectedItem?.ToString(),
                                        comboBox4.SelectedItem?.ToString(),
                                        comboBox5.SelectedItem?.ToString() };
                foreach (string item in comboBoxItems)
                {
                    string selectedText = item?.TrimEnd(".xlsm".ToCharArray());
                    if (!string.IsNullOrEmpty(selectedText))
                    {
                        htmlContent += $"<tr><td>{selectedText}</td></tr>";
                    }
                }
            }
            else
            {
                foreach (var bom in BOMs)
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
                htmlContent += $"<td>{item.TotalRequired}</td>";
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
        private void OptimizeBOMOrder()
        {
            if (BOMs.Count >= 2)
            {
                int maxPositivePercentageIndex = -1;
                double maxPositivePercentage = 0;
                for (int j = 1; j < BOMs.Count; j++)
                {
                    int totalRows = BOMs[j].Items.Count;
                    int positiveCount = BOMs[j].Items.Count(item => (item.Delta ?? 0) > 0);
                    double currentPositivePercentage = (double)positiveCount / totalRows;
                    if (currentPositivePercentage > maxPositivePercentage)
                    {
                        maxPositivePercentage = currentPositivePercentage;
                        maxPositivePercentageIndex = j;
                    }
                }
                if (maxPositivePercentageIndex != -1 && maxPositivePercentageIndex != 0)
                {
                    // Swap the BOMLists in the data grid views
                    BOMList temp = BOMs[0];
                    BOMs[0] = BOMs[maxPositivePercentageIndex];
                    BOMs[maxPositivePercentageIndex] = temp;
                    // Reload the data in the respective ComboBoxes
                    ComboBox[] comboBoxes = new ComboBox[] { comboBox1, comboBox2, comboBox3, comboBox4, comboBox5 };
                    for (int i = 0; i < Math.Min(BOMs.Count, 5); i++)
                    {
                        comboBoxes[i].SelectedItem = BOMs[i].Name;
                    }
                }
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




            var sumRequiredByIPN = BOMs.SelectMany(bom => bom.Items)
                                   .GroupBy(item => item.IPN)
                                   .Select(group => new
                                   {
                                       IPN = group.Key,
                                       TotalRequired = group.Sum(item => item.Delta)
                                   });

            List<WHitem> stockItemsOriginal = new List<WHitem>();
            stockItemsOriginal.AddRange(stockItems);



            var stockData = BOMs.SelectMany(bom => bom.Items)
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






            var stockDataDetailed = BOMs
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
            htmlContent += @"<table style='border: 1px solid; text-align: center; font-weight:bold;'>";
            htmlContent += @"<tr>";
            htmlContent += @"<td style='width: 25%;height: 100%;' id='completion-chart-td'> <div> <canvas id='completion-chart'></canvas> </div></td>";
            htmlContent += @"<td style='width: 50%;'><h2>UPDATED_" + fileTimeStamp + "</h2></td>";
            htmlContent += @"<td style='width: 25%;'></td>";
            htmlContent += @"</tr>";
            htmlContent += @"<tr><td>Multi-BOM simulation for " + BOMs.Count + " kits:</td></tr>";
            foreach (var bom in BOMs)
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
                //htmlContent += $"<td style='width:12%;'>{mainDataSource.WHqty}</td>";
                //htmlContent += $"<td style='width:12%;'>{mainDataSource.KITsBalance}</td>";
                //htmlContent += $"<td class='DELTAFIELD' style='width:12%;'>{mainDataSource.DELTA}</td>";
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

                //List<WHitem> updatedStockList = new List<WHitem>();


                //foreach (var bomItem in mainDataSource.BOMITEMS)
                //{

                //int updatedStockQty = 0;

                ////int whQty = stockItems.Where(si => si.IPN == bomItem.IPN).Sum(si => si.Stock);

                //if(updatedStockList.Count>0)
                //{
                //    updatedStockQty = updatedStockList.FirstOrDefault(w => w.IPN == bomItem.IPN).Stock;
                //}


                //int whQty = stockItems.Where(si => si.IPN == bomItem.IPN).Sum(si => si.Stock) + updatedStockQty;



                //if (bomItem.QtyInKit<0 && (bomItem.QtyInKit+whQty)<0)
                //{
                //    htmlContent += "<tr style='text-align:center;'>";

                //    htmlContent += $"<td class='wrap-content' style='width:12%;white-space: nowrap;'>{bomItem.IPN}</td>";
                //    htmlContent += $"<td class='wrap-content' style='width:12%;'>{bomItem.MFPN}</td>";
                //    htmlContent += $"<td class='wrap-content' style='width:36%;' columnspan='3'>{bomItem.Description}</td>";
                //    htmlContent += $"<td style='width:12%;'>{whQty}</td>";
                //    var rowColorClassQ = bomItem.QtyInKit < 0 ? "lightcoral" : "lightgreen";
                //    htmlContent += $"<td style='width:12%;'>{bomItem.QtyInKit}</td>";
                //    htmlContent += $"<td class='{rowColorClassQ}' style='width:12%;'>{bomItem.QtyInKit + whQty}</td>";

                //    updatedStockList.Add(new WHitem { IPN = bomItem.IPN, Stock = int.Parse(bomItem.QtyInKit.ToString())} );

                //    htmlContent += "</tr>";
                //}
                //else
                //{
                //    //
                //}


                //}


                //Dictionary<string, int> updatedStockDictionary = new Dictionary<string, int>();

                foreach (var currentBomItem in mainDataSource.BOMITEMS)
                {
                    //int updatedStockQty = 0;

                    // Check if the current IPN has an updated stock quantity
                    //if (updatedStockDictionary.ContainsKey(currentBomItem.IPN))
                    //{
                    //    updatedStockQty = updatedStockDictionary[currentBomItem.IPN];
                    //}

                    int whQty = stockItems.Where(si => si.IPN == currentBomItem.IPN).Sum(si => si.Stock);

                    //whQty += updatedStockQty;

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

                        //int totalReqPerIPN =stockData.Where(si => si.IPN==currentBomItem.IPN).Select(w=>w.TotalRequired);




                        //htmlContent += $"<td class='{rowColorClassQ}' style='width:12%;'>{currentBomItem.QtyInKit + whQty}</td>";


                        htmlContent += $"<td class='{rowColorClassQ}' style='width:12%;'>{totalDeltaforIPN}</td>";



                        // Update the dictionary with the new stock quantity for the current IPN
                        //updatedStockDictionary[currentBomItem.IPN] = int.Parse(currentBomItem.QtyInKit.ToString());

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
            BOMs.Clear();

            if (fileNames != null && fileNames.Length > 0)
            {
                foreach (string fileName in fileNames)
                {
                    string theExcelFilePath = Path.GetFileName(fileName);
                    string Litem = Path.GetFileName(fileName);
                    //if (IsFileLoaded(theExcelFilePath))
                    //{
                    //    MessageBox.Show("File already loaded!");
                    //}
                    //else
                    //{
                    DataLoader(fileName, theExcelFilePath);
                    // Add the selected file path to the list
                    selectedFileNames.Add(theExcelFilePath);
                    //}
                }
            }



        }
    }
}
