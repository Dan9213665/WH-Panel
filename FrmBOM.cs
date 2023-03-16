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
        public int sufficientCount = 0;
        public int missingCount = 0;
        public double percentageComplete = 0.0;
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
            sufficientCount = 0;
            missingCount = 0;
            percentageComplete = 0.0;
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
            openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\2023\\"+ DateTime.Now.ToString("MM") + ".2023";
            openFileDialog1.Filter = "BOM files(*.xlsm) | *.xlsm";
            openFileDialog1.Multiselect = false;
            List<KitHistoryItem> BomItemS = new List<KitHistoryItem>();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                string Litem = Path.GetFileName(fileName);
                label12.Text += fileName.ToString() + "\n";
                DataLoader(fileName, Litem);
                double pers = double.Parse(countItems.ToString()) * 0.01;
                percentageComplete = Math.Round((sufficientCount / pers),2);
                this.Text = fileName + " MIS:" + missingCount.ToString() + " / SUF:" + sufficientCount.ToString() + " of TOT:" + countItems + " (" + percentageComplete + "%)";
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
                        //using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                        //{
                        //    var table = reader.GetSchemaTable();
                        //    var nameCol = table.Columns["ColumnName"];
                        //    foreach (DataRow row in table.Rows)
                        //    {
                        //        Console.WriteLine(row[nameCol]);
                        //    }
                        //}
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
                                //reader.GetSchemaTable().Rows[i][nameCol].index
                                var table = reader.GetSchemaTable();
                                var nameCol = table.Columns["ColumnName"];
                                //int conInd = nameCol.
                                //for (int i=0;i<table.Columns.Count;i++)
                                //{
                                //    DataRow dataRowrow = table.Rows[i];
                                //   MessageBox.Show((dataRowrow[nameCol]).ToString());
                                //}
                                //MessageBox.Show(reader[reader.GetSchemaTable().Rows[reader.GetSchemaTable().Columns["ColumnName"]]]);
                                int del = 0;
                                bool delPar = int.TryParse(reader[5].ToString(), out del);
                                int qtk = 0;
                                bool qtkPar= int.TryParse(reader[4].ToString(), out qtk);
                                int qpu = 0;
                                bool qpuPar= int.TryParse(reader[7].ToString(), out qpu);
                                    KitHistoryItem abc = new KitHistoryItem
                                    {
                                        DateOfCreation = cleanedUpSheetName,
                                        ProjectName = excelFIleName,
                                        IPN = reader[1].ToString(),
                                        MFPN = reader[2].ToString(),
                                        Description = reader[3].ToString(),
                                        QtyInKit = qtk,
                                        Delta = del,
                                        QtyPerUnit = qpu,
                                        Notes = reader[8].ToString(),
                                        Alts = reader[9].ToString()
                                    };
                                    i++;
                                    countItems = i;
                                    if (abc.Delta >= 0)
                                    {
                                        SufficientItemsList.Add(abc);
                                       sufficientCount++;
                                    }
                                    else
                                    {
                                        MissingItemsList.Add(abc);
                                    missingCount++;
                                    }
                            }
                        }
                        conn.Dispose();
                        conn.Close();
                     string[] alltheNames=excelFIleName.Split("_");
                        textBox11.Text = alltheNames[1];
                        textBox6.Text= alltheNames[2].Substring(0, alltheNames[2].Length-5);
                        countLoadedFIles++;
                        label12.Text = "Loaded " + (countItems).ToString() + " Rows from " + countLoadedFIles + " files. In " + string.Format("{0:00}.{1:000} Seconds", ts.Seconds, ts.Milliseconds);
                        label12.Update();
                        textBox1.ReadOnly= false;
                        textBox2.ReadOnly= false;
                        textBox3.ReadOnly= false;
                        textBox1.Focus();
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
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                //string cellValue = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();
                txtbSelIPN.Text = dataGridView1.Rows[rowindex].Cells["IPN"].Value.ToString();
                txtbSelMFPN.Text = dataGridView1.Rows[rowindex].Cells["MFPN"].Value.ToString();
                txtbSelDes.Text = dataGridView1.Rows[rowindex].Cells["Description"].Value.ToString();
                txtbQtyToAdd.Clear();
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView1.SelectedCells.Count == 1)
                {
                    txtbQtyToAdd.Focus();
                }
                else
                {
                    dataGridView1.Focus();
                }
            }
        }
        private static void txtbColorGreenOnEnter(object sender)
        {
            TextBox? tb = (TextBox)sender;
            tb.BackColor = Color.LightGreen;
        }
        private static void txtbColorWhiteOnLeave(object sender)
        {
            TextBox? tb = sender as TextBox;
            tb.BackColor = Color.White;
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter((TextBox)sender);
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter((TextBox)sender);
        }
        private void textBox3_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter((TextBox)sender);
        }
        private void txtbQtyToAdd_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter((TextBox)sender);
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave((TextBox)sender);
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave((TextBox)sender);
        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave((TextBox)sender);
        }
        private void txtbQtyToAdd_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave((TextBox)sender);
        }
        private void btnPrintSticker_Click(object sender, EventArgs e)
        {
            KitHistoryItem w = MissingItemsList.FirstOrDefault(r => r.IPN == txtbSelIPN.Text);
            updateQtyInBomFile(w) ;
            MessageBox.Show("Test");
        }
        private void updateQtyInBomFile(KitHistoryItem w)
        {
            if(w.QtyInKit>w.QtyPerUnit)
            {
            }
        }
        private void txtbQtyToAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnPrintSticker_Click(sender, e);
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox1.BackColor = Color.LightGreen;
                checkBox1.Text = "Print Sticker";
            }
            else
            {
                checkBox1.BackColor = Color.IndianRed;
                checkBox1.Text = "No sticker needed";
            }
            txtbQtyToAdd.Focus();
        }
        private void printSticker(WHitem wHitem)
        {
            try
            {
                string userName = Environment.UserName;
                string fp = @"C:\\Users\\" + userName + "\\Desktop\\Print_Stickers.xlsx"; // //////Print_StickersWH.xlsm
                string thesheetName = "Sheet1";
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                OleDbConnection conn = new OleDbConnection(constr);
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE [" + thesheetName + "$] SET PN = @PN, MFPN = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, UPDATEDON = @UPDATEDON";
                cmd.Parameters.AddWithValue("@PN", wHitem.IPN);
                cmd.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                cmd.Parameters.AddWithValue("@ItemDesc", wHitem.Description);
                cmd.Parameters.AddWithValue("@QTY", wHitem.Stock);
                cmd.Parameters.AddWithValue("@UPDATEDON", wHitem.UpdatedOn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                Microsoft.VisualBasic.Interaction.AppActivate("PN_STICKER_2022.btw - BarTender Designer");
                SendKeys.SendWait("^p");
                SendKeys.SendWait("{Enter}");
                //ComeBackFromPrint();
                Microsoft.VisualBasic.Interaction.AppActivate("Imperium Tabula Principalis");
                //LastInputFromUser.Focus();
            }
            catch (Exception e)
            {
                MessageBox.Show("Sticker printing failed : " + e.Message);
            }
        }
    }
}
