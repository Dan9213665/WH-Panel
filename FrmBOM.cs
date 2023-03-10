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
using System.IO;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DataTable = System.Data.DataTable;
using TextBox = System.Windows.Forms.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using System.Xml.Serialization;

namespace WH_Panel
{
    public partial class FrmBOM : Form
    {
        public List<KitHistoryItem> MissingItemsList = new List<KitHistoryItem>();
        public List<KitHistoryItem> SufficientItemsList = new List<KitHistoryItem>();
        public DataTable missingUDtable = new DataTable();
        public DataTable sufficientUDtable = new DataTable();
        public int countItems = 0;
        public int countLoadedFIles = 0;
        int i = 0;
        int loadingErrors = 0;
        public static Stopwatch stopWatch = new Stopwatch();
        public int colIpnFoundIndex;
        public int colMFPNFoundIndex;
        public FrmBOM()
        {
            InitializeComponent();
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
            countItems = 0;
            countLoadedFIles = 0;
            i = 0;
            loadingErrors = 0;
            label12.Text = string.Empty;
            MissingItemsList.Clear();
            SufficientItemsList.Clear();
            missingUDtable.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            sufficientUDtable.Clear();
            dataGridView2.DataSource = null;
            dataGridView2.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetViews();
            var result = openFileDialog1.Title;

            openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\2023\\03.2023";
            openFileDialog1.Filter = "LOG files(*.xlsm) | *.xlsm";
            openFileDialog1.Multiselect = false;
            List<KitHistoryItem> BomItemS = new List<KitHistoryItem>();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                string Litem = Path.GetFileName(fileName);
                label12.Text += fileName.ToString() + "\n";
                DataLoader(fileName, Litem);
            }
            PopulateMissingGridView();
            PopulateSufficientGridView();

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
                        string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();
                        string cleanedUpSheetName = firstSheetName.Substring(1).Substring(0, firstSheetName.Length - 3);
                        OleDbCommand command = new OleDbCommand("Select * from [" + cleanedUpSheetName + "$]", conn);
                        OleDbDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                KitHistoryItem abc = new KitHistoryItem
                                {
                                    DateOfCreation = cleanedUpSheetName,
                                    ProjectName = excelFIleName,
                                    IPN = reader[1].ToString(),
                                    MFPN = reader[2].ToString(),
                                    Description = reader[3].ToString(),
                                    QtyInKit = reader[4].ToString(),
                                    Delta = reader[5].ToString(),
                                    QtyPerUnit = reader[7].ToString(),
                                    Notes = reader[8].ToString(),
                                    Alts = reader[9].ToString()
                                };
                                i ++;
                                countItems = i;
                                if(int.Parse(abc.Delta)>=0)
                                {
                                    SufficientItemsList.Add(abc);
                                }
                                else
                                {
                                    MissingItemsList.Add(abc);
                                }
                                
                            }
                        }
                        conn.Dispose();
                        conn.Close();
                        this.Text= excelFIleName;
                        string[] alltheNames=excelFIleName.Split("_");
                        textBox11.Text = alltheNames[1];
                        textBox6.Text= alltheNames[2].Substring(0, alltheNames[2].Length-5);
                        countLoadedFIles++;
                        label12.Text = "Loaded " + (countItems).ToString() + " Rows from " + countLoadedFIles + " files. In " + string.Format("{0:00}.{1:000} Seconds", ts.Seconds, ts.Milliseconds);
                        label12.Update();
                        textBox1.ReadOnly= false;
                        textBox2.ReadOnly= false;
                        textBox3.ReadOnly= false;
                    }
                    catch (Exception)
                    {
                        loadingErrors++;
                        label13.Text = loadingErrors.ToString() + " Loading Errors detected: ";
                        label13.BackColor = Color.IndianRed;
                        label13.Update();
                        string er = fp;
                        listBox1.Items.Add(er);
                        listBox1.Update();
                        conn.Dispose();
                        conn.Close();
                    }
                }
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void PopulateMissingGridView()
        {
            IEnumerable<KitHistoryItem> data = MissingItemsList;
            using (var reader = ObjectReader.Create(data))
            {
                missingUDtable.Load(reader);
            }
            dataGridView1.DataSource = missingUDtable;
            SetColumsOrder(dataGridView1);
            label12.BackColor = Color.LightGreen;
        }
        private void PopulateSufficientGridView()
        {
            IEnumerable<KitHistoryItem> data = SufficientItemsList;
            using (var reader = ObjectReader.Create(data))
            {
                sufficientUDtable.Load(reader);
            }
            dataGridView2.DataSource = sufficientUDtable;
            SetColumsOrder(dataGridView2);
            label12.BackColor = Color.LightGreen;
        }
        private void SetColumsOrder(DataGridView dgw)
        {
            dgw.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns["DateOfCreation"].DisplayIndex = 0;
            dgw.Columns["ProjectName"].DisplayIndex = 1;
            dgw.Columns["IPN"].DisplayIndex = 2;
            dgw.Columns["MFPN"].DisplayIndex = 3;
            dgw.Columns["Description"].DisplayIndex = 4;
            dgw.Columns["QtyInKit"].DisplayIndex = 5;
            dgw.Columns["Delta"].DisplayIndex = 6;
            dgw.Columns["QtyPerUnit"].DisplayIndex = 7;
            dgw.Columns["Notes"].DisplayIndex = 8;
            dgw.Columns["Alts"].DisplayIndex = 9;
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
                DataView dv = missingUDtable.DefaultView;
                dv.RowFilter = "[IPN] LIKE '%" + textBox1.Text.ToString() +
                     "%' AND [ProjectName] LIKE '%" + textBox11.Text.ToString() +
                "%' AND [MFPN] LIKE '%" + textBox2.Text.ToString() +
                "%' AND [Description] LIKE '%" + textBox3.Text.ToString() + "%' ";
                dataGridView1.DataSource = dv;
                SetColumsOrder(dataGridView1);
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect search pattern, remove invalid character and try again !", "Search pattern error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label2.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            label1.BackColor = Color.LightGreen;
            textBox1.Focus();
        }
    }
}
