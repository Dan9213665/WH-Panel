using FastMember;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DataTable = System.Data.DataTable;
using Label = System.Windows.Forms.Label;
namespace WH_Panel
{
    public partial class FrmPackingSlips : Form
    {
        public List<PackingSlipItem> PSItems  = new List<PackingSlipItem>();
        public DataTable PSIDtable = new DataTable();
        public int countItems = 0;
        public int countLoadedFIles = 0;
        int i = 0;
        int loadingErrors = 0;
        public static Stopwatch stopWatch = new Stopwatch();
        public int colIpnFoundIndex;
        public int colMFPNFoundIndex;
        public List<string> listOfPaths = new List<string>()
            {
                "\\\\dbr1\\Data\\WareHouse\\PACKING_SLIPS\\2023"
            };
        public FrmPackingSlips()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            stopWatch.Reset();
            ResetViews();
            startUpLogic();
            SetColumsOrderPS();
        }
        private void ResetViews()
        {
            listBox1.Items.Clear();
            listBox1.Update();
            label13.Text = "No Errors detected.";
            label13.BackColor = Color.LightGreen;
            label13.Update();
            label1.BackColor = Color.LightGreen;
            label11.BackColor = Color.LightGreen;
            label2.BackColor = Color.LightGreen;
            label3.BackColor = Color.LightGreen;
            PSItems.Clear();
            countItems = 0;
            countLoadedFIles = 0;
            i = 0;
            loadingErrors = 0;
            label12.Text = string.Empty;
            PSIDtable.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }
        private void startUpLogic()
        {
            stopWatch.Start();
            label12.BackColor = Color.IndianRed;
            foreach (string path in listOfPaths)
            {
                foreach (string file in Directory.EnumerateFiles(path, "*.xlsm", SearchOption.AllDirectories))
                {
                    countLoadedFIles++;
                    string Litem = Path.GetFileName(file);
                    DataLoader(file, Litem);
                }
            }
            PopulateGridView();
            SetColumsOrderPS();
            stopWatch.Stop();
        }
        private void SetColumsOrderPS()
        {
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["ShipmentDate"].DisplayIndex = 0;
            dataGridView1.Columns["ClientName"].DisplayIndex = 1;
            dataGridView1.Columns["IPN"].DisplayIndex = 2;
            dataGridView1.Columns["MFPN"].DisplayIndex = 3;
            dataGridView1.Columns["Description"].DisplayIndex = 4;
            dataGridView1.Columns["QtySent"].DisplayIndex = 5;
        }
        private void PopulateGridView()
        {
            IEnumerable<PackingSlipItem> data = PSItems;
            using (var reader = ObjectReader.Create(data))
            {
                PSIDtable.Load(reader);
            }
            dataGridView1.DataSource = PSIDtable;
            SetColumsOrderPS();
            label12.BackColor = Color.LightGreen;
        }
        private void DataLoader(string fp, string excelFIleName)
        {
            TimeSpan ts = stopWatch.Elapsed;
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
                        string firstSheetName = excelFIleName;
                        string cleanedUpSheetName = "PACKING SLIP";

                        OleDbCommand command = new OleDbCommand("Select * from [" + cleanedUpSheetName + "$]", conn);
                        //OleDbCommand command = new OleDbCommand("Select * from [Sheet1$]", conn);
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            int j = 0;
                            while (reader.Read())
                            {
                                

                                if (j>11)
                                {
                                    int qtyS = 0;
                                    bool parseOk = int.TryParse(reader[3].ToString(), out qtyS);
                                    string _ClientName = excelFIleName.Substring(13);
                                    string thName = _ClientName.Substring(0, _ClientName.Length - 5);
                                    PackingSlipItem abc = new PackingSlipItem
                                    {
                                        ShipmentDate = excelFIleName.Substring(0, 12),
                                        ClientName = thName,
                                        IPN = reader[0].ToString(),
                                        MFPN = reader[1].ToString(),
                                        Description = reader[2].ToString(),
                                        QtySent = qtyS,
                                    };
                                    countItems = i;
                                    label12.Text = "Loaded " + (countItems).ToString() + " Rows from " + countLoadedFIles + " files. In " + string.Format("{0:00}.{1:000} Seconds", ts.Seconds, ts.Milliseconds);
                                    if (countItems % 5000 == 0)
                                    { label12.Update(); }
                                    if (abc.IPN != string.Empty && !abc.IPN.StartsWith("Comments") && abc.IPN != "Thank You" && !abc.IPN.StartsWith("Signature") && !abc.IPN.StartsWith("if you") && j > 11)
                                    {
                                        PSItems.Add(abc);
                                        i++;
                                    }
                                }
                                
                                j++;
                            }
                        }
                        conn.Dispose();
                        conn.Close();
                    }
                    catch (Exception e)
                    {
                        loadingErrors++;
                        label13.Text = loadingErrors.ToString() + " Loading Errors detected: ";
                        MessageBox.Show(e.Message);
                        label13.BackColor = Color.IndianRed;
                        label13.Update();

                        string er = fp;
                        listBox1.Items.Add(er);
                        listBox1.Update();
                        conn.Close();
                    }
                }
            }
            catch (IOException)
            {
                throw;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label1.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void FilterTheDataGridView()
        {
            try
            {
                DataView dv = PSIDtable.DefaultView;
                dv.RowFilter = "[IPN] LIKE '%" + textBox1.Text.ToString() +
                     "%' AND [ClientName] LIKE '%" + textBox11.Text.ToString() +
                "%' AND [MFPN] LIKE '%" + textBox2.Text.ToString() +
                "%' AND [Description] LIKE '%" + textBox3.Text.ToString() + "%' ";
                dataGridView1.DataSource = dv;
                SetColumsOrderPS();
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect search pattern, remove invalid character and try again !", "Search pattern error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            label11.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label2.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label3.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            Label? lbl = sender as Label;
            textBox1.Clear();
            lbl.BackColor = Color.LightGreen;
        }
        private void label11_Click(object sender, EventArgs e)
        {
            Label? lbl = sender as Label;
            textBox11.Clear();
            lbl.BackColor = Color.LightGreen;
        }
        private void label2_Click(object sender, EventArgs e)
        {
            Label? lbl = sender as Label;
            textBox2.Clear();
            lbl.BackColor = Color.LightGreen;
        }
        private void label3_Click(object sender, EventArgs e)
        {
            Label? lbl = sender as Label;
            textBox3.Clear();
            lbl.BackColor = Color.LightGreen;
        }
    }
}
