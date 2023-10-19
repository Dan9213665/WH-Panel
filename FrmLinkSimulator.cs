using FastMember;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Button = System.Windows.Forms.Button;
using ComboBox = System.Windows.Forms.ComboBox;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace WH_Panel
{
    public partial class FrmLinkSimulator : Form
    {
        List<ClientWarehouse> warehouses = new List<ClientWarehouse>();
        public List<WHitem> stockItems = new List<WHitem>();
        public FrmLinkSimulator()
        {
            InitializeComponent();
            InitializeComboBoxes();
            IntitializeWarehouses();
            UpdateControlColors(this);
            BOMs = new List<BOMList>();



        }

        public void IntitializeWarehouses()
        {
            warehouses = new List<ClientWarehouse>
{
    new ClientWarehouse
    {
        clName = "NETLINE",
        clPrefix = "NET",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_STOCK.xlsm"
    },
    new ClientWarehouse
    {
        clName = "Leader_Tech",
        clPrefix = "C100",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_AVL.xlsm",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_STOCK.xlsm"
    },
    // Add more warehouses following the same format
    new ClientWarehouse
    {
        clName = "VAYYAR",
        clPrefix = "VAY",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_stock.xlsm"
    },
     new ClientWarehouse
    {
        clName = "CIS",
        clPrefix = "CIS",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\CIS\\CIS_AVL.xlsm",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\CIS\\CIS_STOCK.xlsm"
    },
       new ClientWarehouse
    {
        clName = "VALENS",
        clPrefix = "VAL",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm"
    },
          new ClientWarehouse
    {
        clName = "ROBOTRON",
        clPrefix = "ROB",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_STOCK.xlsm"
    },
             new ClientWarehouse
    {
        clName = "ENERCON",
        clPrefix = "ENE",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ENERCON\\ENERCON_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ENERCON\\ENERCON_STOCK.xlsm"
    },
               new ClientWarehouse
    {
        clName = "DIGITRONIX",
        clPrefix = "DIG",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\DIGITRONIX\\DIGITRONIX_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\DIGITRONIX\\DIGITRONIX_STOCK.xlsm"
    },
                 new ClientWarehouse
    {
        clName = "HEPTAGON",
        clPrefix = "HEP",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\HEPTAGON\\HEPTAGON_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\HEPTAGON\\HEPTAGON_STOCK.xlsm"
    },
                   new ClientWarehouse
    {
        clName = "EPS",
        clPrefix = "EPS",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\EPS\\EPS_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\EPS\\EPS_STOCK.xlsm"
    },
                   new ClientWarehouse
    {
        clName = "SOS",
        clPrefix = "SOS",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOS\\SOS_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOS\\SOS_STOCK.xlsm"
    },
                    new ClientWarehouse
    {
        clName = "ARAN",
        clPrefix = "ARN",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ARAN\\ARAN_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ARAN\\ARAN_STOCK.xlsm"
    },
                    new ClientWarehouse
    {
        clName = "SOLANIUM",
        clPrefix = "BAN",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOLANIUM\\SOLANIUM_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOLANIUM\\SOLANIUM_STOCK.xlsm"
    },
                    new ClientWarehouse
    {
        clName = "SONOTRON",
        clPrefix = "SON",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SONOTRON\\SONOTRON_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SONOTRON\\SONOTRON_STOCK.xlsm"
    },
                    new ClientWarehouse
    {
        clName = "ASIO",
        clPrefix = "ASO",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ASIO\\ASIO_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ASIO\\ASIO_STOCK.xlsm"
    },
                    new ClientWarehouse
    {
        clName = "SHILAT",
        clPrefix = "SHT",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SHILAT\\SHILAT_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SHILAT\\SHILAT_STOCK.xlsm"
    }
                    ,
                    new ClientWarehouse
    {
        clName = "TRILOGICAL",
        clPrefix = "UTR",
        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\TRILOGICAL\\TRILOGICAL_AVL.xlsx",
        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\TRILOGICAL\\TRILOGICAL_STOCK.xlsm"
    }
    // Add more entries for each warehouse as needed
};


            // Ordering the warehouses list by clName
            warehouses = warehouses.OrderBy(warehouse => warehouse.clName).ToList();

            // Adding clNames to comboBox4
            foreach (ClientWarehouse warehouse in warehouses)
            {
                comboBox5.Items.Add(warehouse.clName);
            }
        }
        private void InitializeComboBoxes()
        {

            // Assuming comboBox1, comboBox2, and comboBox3 are the ComboBoxes in your form
            comboBox1.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
            comboBox3.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
            comboBox4.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
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

            // Update the GroupBox texts based on the selected items in the ComboBoxes
            if (selectedComboBox == comboBox1)
            {
                groupBox6.Text = selectedComboBox.SelectedItem?.ToString() ?? "GroupBox6";
            }
            else if (selectedComboBox == comboBox2)
            {
                groupBox7.Text = selectedComboBox.SelectedItem?.ToString() ?? "GroupBox7";
            }
            else if (selectedComboBox == comboBox3)
            {
                groupBox8.Text = selectedComboBox.SelectedItem?.ToString() ?? "GroupBox8";
            }
            else if (selectedComboBox == comboBox4)
            {
                groupBox10.Text = selectedComboBox.SelectedItem?.ToString() ?? "GroupBox10";
            }

            // Remove the selected item from other ComboBoxes
            foreach (ComboBox comboBox in new[] { comboBox1, comboBox2, comboBox3, comboBox4 }.Where(c => c != selectedComboBox))
            {
                if (comboBox.SelectedItem == selectedComboBox.SelectedItem)
                {
                    comboBox.SelectedItem = null;
                }
            }

            // Load data based on the selected items in the ComboBoxes
            LoadDataIntoDataGridViews();
        }
        private void LoadDataIntoDataGridViews()
        {

            for (int i = 0; i < 4; i++)
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

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    var result = openFileDialog1.Title;
        //    openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\2023\\" + DateTime.Now.ToString("MM") + ".2023";
        //    openFileDialog1.Filter = "BOM files(*.xlsm) | *.xlsm";
        //    openFileDialog1.Multiselect = true;

        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        fileName = openFileDialog1.FileName;
        //        theExcelFilePath = Path.GetFileName(fileName);
        //        string Litem = Path.GetFileName(fileName);

        //        if (IsFileLoaded(theExcelFilePath))
        //        {
        //            MessageBox.Show("File already loaded!");
        //        }
        //        else
        //        {
        //            DataLoader(fileName, Litem);
        //        }
        //    }
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select BOM File";
            openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\2023\\" + DateTime.Now.ToString("MM.yyyy");
            openFileDialog1.Filter = "BOM files(*.xlsm) | *.xlsm";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] fileNames = openFileDialog1.FileNames; // Get all selected file names

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
                        DataLoader(fileName, Litem);
                    }
                }
            }
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
            richTextBox1.Text += a.Name + "\n";


            // Assuming comboBox1, comboBox2, and comboBox3 are the names of your ComboBox controls

            // Set the SelectedIndex for each ComboBox based on its own count
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
            comboBox3.SelectedIndex = comboBox3.Items.Count - 1;
            comboBox4.SelectedIndex = comboBox4.Items.Count - 1;
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
                    comboBox5.SelectedItem = warehouse.clName;
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

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ClientWarehouse w in warehouses)
            {
                if (comboBox5.SelectedItem == w.clName)
                {

                    StockViewDataLoader(w.clStockFile, "STOCK");
                }
            }
            PopulateStockView();
        }


        private void StockViewDataLoader(string fp, string thesheetName)
        {
            try
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
                                    UpdatedOn = reader[5].ToString(),
                                    ReelBagTrayStick = reader[6].ToString(),
                                    SourceRequester = reader[7].ToString()
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
            catch (IOException)
            {
                MessageBox.Show("Error");
            }


        }


        private void PopulateStockView()
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
                   .OrderBy(item => item.IPN);

            // Generating the HTML content
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
                        </style>
                     <script>
                         window.onload = function() {
        var headerElement = document.getElementById('myHeader');
        var tableElement = document.getElementById('stockTable');

        var headerBottom = headerElement.offsetTop + headerElement.offsetHeight;
        tableElement.style.marginTop = Math.max(headerBottom, headerElement.offsetHeight) + 'px';
    };
                    function sortTable(columnIndex) {{
        var table, rows, switching, i, x, y, shouldSwitch;
        table = document.getElementById('stockTable');
        switching = true;
        while (switching) {{
            switching = false;
            rows = table.rows;
            for (i = 1; i < (rows.length - 1); i++) {{
                x = rows[i].getElementsByTagName('TD')[columnIndex];
                y = rows[i + 1].getElementsByTagName('TD')[columnIndex];
                if (isNaN(x.innerHTML) || isNaN(y.innerHTML)) {{
                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {{
                        shouldSwitch = true;
                        break;
                    }}
                }} else {{
                    if (Number(x.innerHTML) > Number(y.innerHTML)) {{
                        shouldSwitch = true;
                        break;
                    }}
                }}
            }}
            if (shouldSwitch) {{
                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                switching = true;
            }}
        }}
    }}

      function filterTable() {{
        var input, filter, table, tr, td, i, txtValue;
        input = document.getElementById('searchInput');
        filter = input.value.toUpperCase();
        table = document.getElementById('stockTable');
        tr = table.getElementsByTagName('tr');

        for (i = 0; i < tr.length; i++) {{
            tdIPN = tr[i].getElementsByTagName('td')[0];
            tdMFPN = tr[i].getElementsByTagName('td')[1];
            if (tdIPN || tdMFPN) {{
                txtValueIPN = tdIPN.textContent || tdIPN.innerText;
                txtValueMFPN = tdMFPN.textContent || tdMFPN.innerText;
                if (txtValueIPN.toUpperCase().indexOf(filter) > -1 || txtValueMFPN.toUpperCase().indexOf(filter) > -1) {{
                    tr[i].style.display = '';
                }} else {{
                    tr[i].style.display = 'none';
                }}
            }}
        }}
    }}

     function clearFilter() {
    document.getElementById('searchInput').value = '';
    var table = document.getElementById('stockTable');
    var tr = table.getElementsByTagName('tr');
    for (var i = 0; i < tr.length; i++) {
        tr[i].style.display = '';
    }
    document.getElementById('searchInput').focus();
}

                    </script>
                        </head>
                        <body>
                        <div style='text-align: center;'>
                            <h2>Warehouse stock simulation for";

            htmlContent += "<br>";

            string selectedText1 = comboBox1.SelectedItem?.ToString()?.TrimEnd(".xlsm".ToCharArray());
            string selectedText2 = comboBox2.SelectedItem?.ToString()?.TrimEnd(".xlsm".ToCharArray());
            string selectedText3 = comboBox3.SelectedItem?.ToString()?.TrimEnd(".xlsm".ToCharArray());
            string selectedText4 = comboBox4.SelectedItem?.ToString()?.TrimEnd(".xlsm".ToCharArray());

            if (!string.IsNullOrEmpty(selectedText1))
            {
                htmlContent += $"{selectedText1}<br>";
            }

            if (!string.IsNullOrEmpty(selectedText2))
            {
                htmlContent += $"{selectedText2}<br>";
            }

            if (!string.IsNullOrEmpty(selectedText3))
            {
                htmlContent += $"{selectedText3}<br>";
            }
            if (!string.IsNullOrEmpty(selectedText4))
            {
                htmlContent += $"{selectedText4}<br>";
            }

            // Removing the last comma
            htmlContent = htmlContent.TrimEnd(',');

            // Continuing the HTML content
            htmlContent += @"</h2>
         
<input type='text' id=""searchInput"" placeholder=""Search for IPN or MFPN.."" onkeyup=""filterTable()"" />


<button onclick=""clearFilter()"">Clear Filter</button>
                <table id='stockTable' border='1'>
                <tr><th onclick='sortTable(0)'>IPN</th><th onclick='sortTable(1)'>MFPN</th><th onclick='sortTable(2)'>Description</th><th onclick='sortTable(3)'>WH Qty</th><th onclick='sortTable(4)'>KITs BALANCE</th><th onclick='sortTable(5)'>DELTA</th></tr>";


            foreach (var item in stockData)
            {
                var rowColorClass = item.StockQuantity + item.TotalRequired < 0 ? "lightcoral" : "lightgreen";
                htmlContent += $"<tr class='{rowColorClass}'><td>{item.IPN}</td><td>{item.MFPN}</td><td>{item.Description}</td><td>{item.StockQuantity}</td><td>{item.TotalRequired}</td><td>{item.StockQuantity + item.TotalRequired}</td></tr>";
            }
            htmlContent += "</table></div></body></html>";

            // Writing the HTML content to a file
            string fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = @"\\dbr1\Data\WareHouse\2023\WHsearcher\" + fileTimeStamp + "_" + ".html";

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

    }
}
