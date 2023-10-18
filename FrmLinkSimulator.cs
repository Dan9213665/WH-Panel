using FastMember;
using System;
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
using ComboBox = System.Windows.Forms.ComboBox;

namespace WH_Panel
{
    public partial class FrmLinkSimulator : Form
    {
        List<ClientWarehouse> warehouses = new List<ClientWarehouse>();
        public FrmLinkSimulator()
        {
            InitializeComponent();
            InitializeComboBoxes();
            IntitializeWarehouses();
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
        clPrefix = "C00",
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
                comboBox4.Items.Add(warehouse.clName);
            }
        }
        private void InitializeComboBoxes()
        {

            // Assuming comboBox1, comboBox2, and comboBox3 are the ComboBoxes in your form
            comboBox1.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
            comboBox3.SelectedIndexChanged += ComboBoxes_SelectedIndexChanged;
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

            // Remove the selected item from other ComboBoxes
            foreach (ComboBox comboBox in new[] { comboBox1, comboBox2, comboBox3 }.Where(c => c != selectedComboBox))
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
            for (int i = 0; i < 3; i++)
            {
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
            var result = openFileDialog1.Title;
            openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\2023\\" + DateTime.Now.ToString("MM") + ".2023";
            openFileDialog1.Filter = "BOM files(*.xlsm) | *.xlsm";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                theExcelFilePath = Path.GetFileName(fileName);
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
            //comboBox1.Items.Add(a.Name);
            //comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            //groupBox6.Text = a.Name;
            comboBox1.Items.Add(a.Name);
            comboBox2.Items.Add(a.Name);
            comboBox3.Items.Add(a.Name);
            richTextBox1.Text += a.Name + "\n";


            // Assuming comboBox1, comboBox2, and comboBox3 are the names of your ComboBox controls

            // Set the SelectedIndex for each ComboBox based on its own count
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
            comboBox3.SelectedIndex = comboBox3.Items.Count - 1;
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
                    comboBox4.SelectedItem = warehouse.clName;
                    break;
                }
            }
        }

        private void PopulateBOMGridView(int selectedIndex, DataGridView dataGridView)
        {
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

            dgw.Columns["IPN"].DisplayIndex = 0;
            dgw.Columns["MFPN"].DisplayIndex = 1;
            dgw.Columns["QtyInKit"].DisplayIndex = 2;
            dgw.Columns["Delta"].DisplayIndex = 3;
            dgw.Columns["QtyPerUnit"].DisplayIndex = 4;

            // Set AutoSizeMode for the displayed columns
            foreach (DataGridViewColumn column in dgw.Columns)
            {
                if (column.Visible)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }

        }
    }
}
