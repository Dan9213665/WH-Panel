using FastMember;
using Seagull.Framework.Extensions;
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
        //List<ClientWarehouse> warehouses = new List<ClientWarehouse>();
        List<ClientWarehouse> warehouses { get; set; }
        public List<WHitem> stockItems = new List<WHitem>();
        List<string> selectedFileNames = new List<string>();
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
//        public void IntitializeWarehouses()
//        {
//            warehouses = new List<ClientWarehouse>
//{
//    new ClientWarehouse
//    {
//        clName = "NETLINE",
//        clPrefix = "NET",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_STOCK.xlsm"
//    },
//    new ClientWarehouse
//    {
//        clName = "LEADER-TECH",
//        clPrefix = "C100",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_AVL.xlsm",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_STOCK.xlsm"
//    },
//    // Add more warehouses following the same format
//    new ClientWarehouse
//    {
//        clName = "VAYYAR",
//        clPrefix = "VAY",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_stock.xlsm"
//    },
//     new ClientWarehouse
//    {
//        clName = "CIS",
//        clPrefix = "CIS",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\CIS\\CIS_AVL.xlsm",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\CIS\\CIS_STOCK.xlsm"
//    },
//       new ClientWarehouse
//    {
//        clName = "VALENS",
//        clPrefix = "VAL",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm"
//    },
//          new ClientWarehouse
//    {
//        clName = "ROBOTRON",
//        clPrefix = "ROB",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_STOCK.xlsm"
//    },
//             new ClientWarehouse
//    {
//        clName = "ENERCON",
//        clPrefix = "ENE",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ENERCON\\ENERCON_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ENERCON\\ENERCON_STOCK.xlsm"
//    },
//               new ClientWarehouse
//    {
//        clName = "DIGITRONIX",
//        clPrefix = "DIG",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\DIGITRONIX\\DIGITRONIX_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\DIGITRONIX\\DIGITRONIX_STOCK.xlsm"
//    },
//                 new ClientWarehouse
//    {
//        clName = "HEPTAGON",
//        clPrefix = "HEP",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\HEPTAGON\\HEPTAGON_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\HEPTAGON\\HEPTAGON_STOCK.xlsm"
//    },
//                   new ClientWarehouse
//    {
//        clName = "EPS",
//        clPrefix = "EPS",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\EPS\\EPS_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\EPS\\EPS_STOCK.xlsm"
//    },
//                   new ClientWarehouse
//    {
//        clName = "SOS",
//        clPrefix = "SOS",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOS\\SOS_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOS\\SOS_STOCK.xlsm"
//    },
//                    new ClientWarehouse
//    {
//        clName = "ARAN",
//        clPrefix = "ARN",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ARAN\\ARAN_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ARAN\\ARAN_STOCK.xlsm"
//    },
//                    new ClientWarehouse
//    {
//        clName = "SOLANIUM",
//        clPrefix = "BAN",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOLANIUM\\SOLANIUM_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOLANIUM\\SOLANIUM_STOCK.xlsm"
//    },
//                    new ClientWarehouse
//    {
//        clName = "SONOTRON",
//        clPrefix = "SON",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SONOTRON\\SONOTRON_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SONOTRON\\SONOTRON_STOCK.xlsm"
//    },
//                    new ClientWarehouse
//    {
//        clName = "ASIO",
//        clPrefix = "ASO",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ASIO\\ASIO_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ASIO\\ASIO_STOCK.xlsm"
//    },
//                    new ClientWarehouse
//    {
//        clName = "SHILAT",
//        clPrefix = "SHT",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SHILAT\\SHILAT_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SHILAT\\SHILAT_STOCK.xlsm"
//    }
//                    ,
//                    new ClientWarehouse
//    {
//        clName = "TRILOGICAL",
//        clPrefix = "UTR",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\TRILOGICAL\\TRILOGICAL_AVL.xlsx",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\TRILOGICAL\\TRILOGICAL_STOCK.xlsm"
//    }
//                      ,
//                    new ClientWarehouse
//    {
//        clName = "QUANTUM-MACHINES",
//        clPrefix = "QNT",
//        clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\QUANTUM-MACHINES\\QUANTUM-MACHINES_AVL.xlsm",
//        clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\QUANTUM-MACHINES\\QUANTUM-MACHINES_STOCK.xlsm"
//    }  ,
//                new ClientWarehouse
//                {
//                    clName = "GASNGO",
//                    clPrefix = "GNG",
//                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\GASNGO\\GASNGO_AVL.xlsm",
//                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\GASNGO\\GASNGO_STOCK.xlsm"
//                }
//                    ,
//                new ClientWarehouse
//                {
//                    clName = "MS-TECH",
//                    clPrefix = "MST",
//                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\MS-TECH\\MS-TECH_AVL.xlsm",
//                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\MS-TECH\\MS-TECH_STOCK.xlsm"
//                }
//                     ,
//                new ClientWarehouse
//                {
//                    clName = "RP-OPTICAL",
//                    clPrefix = "RPO",
//                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\RP-OPTICAL\\RP-OPTICAL_AVL.xlsm",
//                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\RP-OPTICAL\\RP-OPTICAL_STOCK.xlsm"
//                },
//                new ClientWarehouse
//                {
//                    clName = "ROBOTEAM",
//                    clPrefix = "RBM",
//                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTEAM\\ROBOTEAM_AVL.xlsm",
//                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTEAM\\ROBOTEAM_STOCK.xlsm"
//                }
//    // Add more entries for each warehouse as needed
//};

//            // Ordering the warehouses list by clName
//            warehouses = warehouses.OrderBy(warehouse => warehouse.clName).ToList();

//            // Adding clNames to comboBox of warehouse name
//            foreach (ClientWarehouse warehouse in warehouses)
//            {
//                comboBox6.Items.Add(warehouse.clName);
//            }
//        }
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

            //// Update the GroupBox texts based on the selected items in the ComboBoxes
            //if (selectedComboBox == comboBox1)
            //{
            //    groupBox6.Text = selectedComboBox.SelectedItem?.ToString() ?? "GroupBox6";
            //}
            //else if (selectedComboBox == comboBox2)
            //{
            //    groupBox7.Text = selectedComboBox.SelectedItem?.ToString() ?? "GroupBox7";
            //}
            //else if (selectedComboBox == comboBox3)
            //{
            //    groupBox8.Text = selectedComboBox.SelectedItem?.ToString() ?? "GroupBox8";
            //}
            //else if (selectedComboBox == comboBox4)
            //{
            //    groupBox10.Text = selectedComboBox.SelectedItem?.ToString() ?? "GroupBox10";
            //}
            //else if (selectedComboBox == comboBox5)
            //{
            //    groupBox12.Text = selectedComboBox.SelectedItem?.ToString() ?? "GroupBox12";
            //}

            // Update the GroupBox texts based on the selected items in the ComboBoxes
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

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ClientWarehouse w in warehouses)
            {
                if (comboBox6.SelectedItem == w.clName)
                {

                    StockViewDataLoader(w.clStockFile, "STOCK");
                }
            }
            GenerateHtmlReport(true);
        }


        private void StockViewDataLoader(string fp, string thesheetName)
        {
            stockItems.Clear();
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
                            <h2>UPDATED_" + fileTimeStamp+"<br> Multi-BOM simulation for:";

            htmlContent += "<br>";

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
                        htmlContent += $"{selectedText}<br>";
                    }
                }
                // Removing the last comma
                htmlContent = htmlContent.TrimEnd(',');
            }
            else
            {
                foreach (var bom in BOMs)
                {
                    htmlContent += $"{bom.Name.TrimEnd(".xlsm".ToCharArray())}<br>";
                }
                // Removing the last <br>
                if (!string.IsNullOrEmpty(htmlContent))
                {
                    htmlContent = htmlContent.Substring(0, htmlContent.Length - 4); // Assuming <br> is of length 4
                }
                htmlContent = htmlContent.TrimEnd(',');
            }
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

            //htmlContent += "<div class='container'><table class='table table-striped'>";

            //foreach (var item in stockData)
            //{
            //    var rowColorClass = item.StockQuantity + item.TotalRequired < 0 ? "lightcoral" : "lightgreen";
            //    htmlContent += $"<tr class='{rowColorClass}'><td><a href='#' data-toggle='modal' data-target='#modal-{item.IPN}'>{item.IPN}</a></td><td>{item.MFPN}</td><td>{item.Description}</td><td>{item.StockQuantity}</td><td>{item.TotalRequired}</td><td>{item.StockQuantity + item.TotalRequired}</td></tr>";

            //    // Find all the BOMs containing the specific IPN
            //    var relevantBOMs = BOMs.Where(bom => bom.Items.Any(bomItem => bomItem.IPN == item.IPN));

            //    // Modal content
            //    htmlContent += $"<div class='modal fade' id='modal-{item.IPN}' tabindex='-1' role='dialog' aria-labelledby='modal-{item.IPN}-label' aria-hidden='true'>";
            //    htmlContent += "<div class='modal-dialog' role='document'><div class='modal-content'>";
            //    htmlContent += "<div class='modal-header'><h5 class='modal-title' id='modal-{item.IPN}-label'>BOMs Information</h5>";
            //    htmlContent += "<button type='button' class='close' data-dismiss='modal' aria-label='Close'>";
            //    htmlContent += "<span aria-hidden='true'>&times;</span></button></div><div class='modal-body'><ul>";

            //    foreach (var bom in relevantBOMs)
            //    {
            //        var quantityInBOM = bom.Items.Where(bomItem => bomItem.IPN == item.IPN).Sum(bomItem => bomItem.QtyInKit);
            //        htmlContent += $"<li>{bom.Name} - Quantity in BOM: {quantityInBOM}</li>";
            //    }

            //    htmlContent += "</ul></div><div class='modal-footer'>";
            //    htmlContent += "<button type='button' class='btn btn-secondary' data-dismiss='modal'>Close</button></div></div></div></div>";
            //}

            //htmlContent += "</table></div></body></html>";

            //htmlContent += "<div class='container'><table class='table table-striped'>";

            //foreach (var item in stockData)
            //{
            //    var rowColorClass = item.StockQuantity + item.TotalRequired < 0 ? "lightcoral" : "lightgreen";
            //    htmlContent += $"<tr class='{rowColorClass}'><td>{item.IPN}</td><td>{item.MFPN}</td><td>{item.Description}</td><td>{item.StockQuantity}</td><td>{item.TotalRequired}</td><td>{item.StockQuantity + item.TotalRequired}</td></tr>";

            //    // Find all the BOMs containing the specific IPN
            //    var relevantBOMs = BOMs.Where(bom => bom.Items.Any(bomItem => bomItem.IPN == item.IPN));

            //    htmlContent += "<tr><td colspan='6'><ul>";
            //    foreach (var bom in relevantBOMs)
            //    {
            //        var quantityInBOM = bom.Items.Where(bomItem => bomItem.IPN == item.IPN).Sum(bomItem => bomItem.QtyInKit);
            //        htmlContent += $"<li>{bom.Name} - Quantity in BOM: {quantityInBOM}</li>";
            //    }
            //    htmlContent += "</ul></td></tr>";
            //}

            //htmlContent += "</table></div></body></html>";





            string filename = @"\\dbr1\Data\WareHouse\2023\WHsearcher\" + fileTimeStamp + "_BOMs_sim" + ".html";

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
            foreach (ClientWarehouse w in warehouses)
            {
                if (comboBox6.SelectedItem == w.clName)
                {

                    StockViewDataLoader(w.clStockFile, "STOCK");
                }
            }
            GenerateHtmlReport(false);
        }
    }
}
