using FastMember;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using TextBox = System.Windows.Forms.TextBox;
using Seagull.Framework.OS;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using DataTable = System.Data.DataTable;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using System.Xml.Serialization;
using System.Xml;
namespace WH_Panel
{
    public partial class FrmPackingSlip : Form
    {
        public List<KitHistoryItem> KitHistoryItemsList = new List<KitHistoryItem>();
        public List<WHitem> PackedItemsList = new List<WHitem>();
        public DataTable UDtable = new DataTable();
        public DataTable PackedItemsDtable = new DataTable();
        public TextBox LastInputFromUser = new TextBox();
        public int countItems = 0;
        public int countLoadedFIles = 0;
        int i = 0;
        int loadingErrors = 0;
        public static Stopwatch stopWatch = new Stopwatch();
        public int colIpnFoundIndex;
        public int colMFPNFoundIndex;
        public List<string> listOfPaths = new List<string>()
            {
                "\\\\dbr1\\Data\\WareHouse\\2022\\10.2022",
                "\\\\dbr1\\Data\\WareHouse\\2022\\11.2022",
                "\\\\dbr1\\Data\\WareHouse\\2022\\12.2022",
                "\\\\dbr1\\Data\\WareHouse\\2023"
            };
        public FrmPackingSlip()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            stopWatch.Reset();
            ResetViews();
            startUpLogic();
            SetColumsOrder(dataGridView1);
            textBox1.Focus();
            button2.Enabled = true;
        }
        private void ResetViews()
        {
            LastInputFromUser = textBox1;
            LastInputFromUser.Focus();
            listBox1.Items.Clear();
            listBox1.Update();
            label13.Text = "No Errors detected.";
            label13.BackColor = Color.LightGreen;
            label13.Update();
            label1.BackColor = Color.LightGreen;
            label11.BackColor = Color.LightGreen;
            label2.BackColor = Color.LightGreen;
            label3.BackColor = Color.LightGreen;
            KitHistoryItemsList.Clear();
            PackedItemsList.Clear();
            countItems = 0;
            countLoadedFIles = 0;
            i = 0;
            loadingErrors = 0;
            label12.Text = string.Empty;
            UDtable.Clear();
            PackedItemsDtable.Clear();  
            dataGridView1.DataSource = null;
            dataGridView2.DataSource= null;
            dataGridView1.Refresh();
            dataGridView2.Refresh();
            checkBox1.BackColor = Color.LightGreen;
            checkBox1.Checked = true;
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
                    if(FileIsLocked(Litem))
                    {
                        DataLoader(file, Litem);
                    }
                    else
                    {
                        AddErrorousFilesToListOfErrors(Litem);
                    }
                }
            }
            PopulateGridView();
            SetColumsOrder(dataGridView1);
            stopWatch.Stop();
            textBox1.ReadOnly= false;
            textBox11.ReadOnly= false;
            textBox2.ReadOnly= false;
            textBox3.ReadOnly= false;
        }
        public bool FileIsLocked(string strFullFileName)
        {
            bool blnReturn = false;
            System.IO.FileStream fs;
            try
            {
                fs = System.IO.File.Open(strFullFileName, System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.None);
                fs.Close();
                fs.Dispose();
            }
            catch (System.IO.IOException ex)
            {
                blnReturn = true;
            }
            return blnReturn;
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
        private void SetColumsOrderPackedItems(DataGridView dgw)
        {
            dgw.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgw.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgw.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgw.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgw.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns["IPN"].DisplayIndex = 0;
            dgw.Columns["Manufacturer"].Visible = false;
            dgw.Columns["MFPN"].DisplayIndex = 1;
            dgw.Columns["Description"].DisplayIndex = 2;
            dgw.Columns["Stock"].DisplayIndex = 3;
            dgw.Columns["UpdatedOn"].Visible = false;
            dgw.Columns["CommentsWHitem"].Visible = false;
            dgw.Columns["SourceRequester"].Visible = false;
            dgw.AutoResizeColumns();
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
                                int del = 0;
                                bool delPar = int.TryParse(reader[5].ToString(), out del);
                                int qtk = 0;
                                bool qtkPar = int.TryParse(reader[4].ToString(), out qtk);
                                int qpu = 0;
                                bool qpuPar = int.TryParse(reader[7].ToString(), out qpu);
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
                                countItems = i;
                                label12.Text = "Loaded " + (countItems).ToString() + " Rows from " + countLoadedFIles + " files. In " + string.Format("{0:00}.{1:000} Seconds", ts.Seconds, ts.Milliseconds);
                               if(countItems%1000==0)
                                { label12.Update(); }
                                KitHistoryItemsList.Add(abc);
                                i++;
                            }
                        }
                        conn.Dispose();
                        conn.Close();
                    }
                    catch (Exception)
                    {
                        AddErrorousFilesToListOfErrors(fp);
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
        private void AddErrorousFilesToListOfErrors(string fp)
        {
            loadingErrors++;
            label13.Text = loadingErrors.ToString() + " Loading Errors detected: ";
            label13.BackColor = Color.IndianRed;
            label13.Update();
            string er = fp;
            listBox1.Items.Add(er);
            listBox1.Update();
        }
        private void PopulateGridView()
        {
            IEnumerable<KitHistoryItem> data = KitHistoryItemsList;
            using (var reader = ObjectReader.Create(data))
            {
                UDtable.Load(reader);
            }
            dataGridView1.DataSource = UDtable;
            SetColumsOrder(dataGridView1);
            label12.BackColor = Color.LightGreen;
        }
        private void PopulatePackedItemsGridView()
        {
            PackedItemsDtable.Clear();
            IEnumerable<WHitem> data = PackedItemsList;
            using (var reader = ObjectReader.Create(data))
            {
                PackedItemsDtable.Load(reader);
            }
            dataGridView2.DataSource = PackedItemsDtable;
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            filterViewAndJump2Qty(sender,e); ;
        }
        private void filterViewAndJump2Qty(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LastInputFromUser = (TextBox)sender;
                if (dataGridView1.Rows.Count == 1)
                {
                    txtbQty.Focus();
                }
                else
                {
                    dataGridView1.Focus();
                }
            }
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            filterViewAndJump2Qty(sender,e);
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
                DataView dv = UDtable.DefaultView;
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
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                string cellValue = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();
                txtbIPN.Text = dataGridView1.Rows[rowindex].Cells["IPN"].Value.ToString();
                txtbMFPN.Text = dataGridView1.Rows[rowindex].Cells["MFPN"].Value.ToString();
                txtbDescription.Text = dataGridView1.Rows[rowindex].Cells["Description"].Value.ToString();
                txtbQty.Clear();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            label1.BackColor = Color.LightGreen;
            textBox1.Focus();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;
            label2.BackColor = Color.LightGreen;
            textBox2.Focus();
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            label11.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label3.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void label3_Click(object sender, EventArgs e)
        {
            textBox3.Text = string.Empty;
            label3.BackColor = Color.LightGreen;
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private static void txtbColorGreenOnEnter(object sender)
        {
            TextBox? tb = (TextBox)sender;
            tb.BackColor = Color.LightGreen;
        }
        private void textBox11_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox3_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox11_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private static void txtbColorWhiteOnLeave(object sender)
        {
            TextBox? tb = sender as TextBox;
            tb.BackColor = Color.White;
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void label11_Click(object sender, EventArgs e)
        {
            textBox11.Text = string.Empty;
            label11.BackColor = Color.LightGreen;
            textBox11.Focus();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label2.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void txtbQty_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void txtbQty_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void txtbQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnPrintSticker_Click(this, new EventArgs());
            }
        }
        private void btnPrintSticker_Click(object sender, EventArgs e)
        {
            int outNumber;
            bool success = int.TryParse(txtbQty.Text, out outNumber);
            if (success && outNumber < 15001 && outNumber > 0)
            {
                WHitem w = new WHitem() { IPN = txtbIPN.Text, MFPN = txtbMFPN.Text, Description = txtbDescription.Text, Stock = outNumber, UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss") };
                if(checkBox1.Checked)
                {
                    printSticker(w);
                }
                PackedItemsList.Add(w);
                addToXML();
                PopulatePackedItemsGridView();
                SetColumsOrderPackedItems(dataGridView2);
                ResetAllTexboxes(LastInputFromUser);
            }
            else
            {
                MessageBox.Show("Input valid QTY !");
                txtbQty.Clear();
                txtbQty.Focus();
            }
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
                LastInputFromUser.Focus();
            }
            catch (Exception e)
            {
                MessageBox.Show("Sticker printing failed : " + e.Message);
            }
        }
        private void ResetAllTexboxes(TextBox txtb)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            txtbQty.Clear();
            label1.BackColor = Color.LightGreen;
            label2.BackColor = Color.LightGreen;
            label3.BackColor = Color.LightGreen;
            txtb.Focus();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           if( checkBox1.Checked == true)
            {
                checkBox1.BackColor = Color.LightGreen;
                checkBox1.Text="Print Sticker";
            }
            else
            {
                checkBox1.BackColor = Color.IndianRed;
                checkBox1.Text = "No sticker needed";
            }
            LastInputFromUser.Focus();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtbQty.Focus();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Close the shipment ?", "Complete the shipment procedure", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //MessageBox.Show("Shipment sent");
                //string fp = "\\\\dbr1\\Data\\WareHouse\\PACKING_SLIPS\\_template.xlsm";
                //string thesheetName = "PACKING_SLIP";
                    //DataInserter(fp,thesheetName, PackedItemsList);
                EXCELinserter(PackedItemsList);
            }
            else if (dialogResult == DialogResult.No)
            {
                LastInputFromUser.Focus();
            }
        }
        private void EXCELinserter(List<WHitem> lst)
        {
            try
            {
                lst.Sort((x, y) => string.Compare(x.IPN, y.IPN));
                string? v = KitHistoryItemsList.FirstOrDefault(r => lst[0].IPN == r.IPN).ProjectName;
                string[]? client = v.Split("_");
                string fp = "\\\\dbr1\\Data\\WareHouse\\PACKING_SLIPS\\_templateITPc.xlsm";
                _Application docExcel = new Microsoft.Office.Interop.Excel.Application();
                docExcel.Visible = false;
                docExcel.DisplayAlerts = false;
                _Workbook workbooksExcel = docExcel.Workbooks.Open(@fp, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //_Worksheet worksheetExcel = (_Worksheet)workbooksExcel;
                _Worksheet worksheetExcel = (_Worksheet)workbooksExcel.Worksheets["PACKING_SLIP"];
                int startRow = 12;
                string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                ((Range)worksheetExcel.Cells[1, "A"]).Value2 = "RPS_" + _fileTimeStamp;
                ((Range)worksheetExcel.Cells[2, "D"]).Value2 = DateTime.Now.ToString("yyyy-MM-dd");
                ((Range)worksheetExcel.Cells[8, "B"]).Value2 = client?[0];
                for (int i = 0; i < lst.Count; i++)
                {
                    ((Range)worksheetExcel.Cells[startRow + i, "A"]).Value2 = lst[i].IPN.ToString();
                    ((Range)worksheetExcel.Cells[startRow + i, "A"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                    ((Range)worksheetExcel.Cells[startRow + i, "B"]).Value2 = lst[i].MFPN.ToString();
                    ((Range)worksheetExcel.Cells[startRow + i, "B"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                    ((Range)worksheetExcel.Cells[startRow + i, "C"]).Value2 = lst[i].Description.ToString();
                    ((Range)worksheetExcel.Cells[startRow + i, "C"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                    ((Range)worksheetExcel.Cells[startRow + i, "D"]).Value2 = lst[i].Stock;
                    ((Range)worksheetExcel.Cells[startRow + i, "D"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                }
                ((Range)worksheetExcel.Cells[startRow + lst.Count+1, "A"]).Value2 = "Comments:                                ";
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 1, 1], worksheetExcel.Cells[startRow + lst.Count + 1, 4]].Merge();
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 1, 1], worksheetExcel.Cells[startRow + lst.Count + 1, 4]].BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 2, "A"]).Value2 = "Signature_______________________ חתימה     DATE ______/______/2023  תאריך      NAME ________________________________  שם";
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 2, "A"]).WrapText = true;
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 2, 1], worksheetExcel.Cells[startRow + lst.Count + 2, 4]].Merge();
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 2, 1], worksheetExcel.Cells[startRow + lst.Count + 2, 4]].BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 3, "A"]).Value2 = "Thank You";
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 3, 1], worksheetExcel.Cells[startRow + lst.Count + 3, 4]].Merge();
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 3, 1], worksheetExcel.Cells[startRow + lst.Count + 3, 4]].BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 4, "A"]).Value2 = "if you have any questions or concerns, please contact  Vlad Berezin, (972) 525118807, vlad@robotron.co.il";
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 4, "A"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 4, 1], worksheetExcel.Cells[startRow + lst.Count + 4, 4]].Merge();
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 4, 1], worksheetExcel.Cells[startRow + lst.Count + 4, 4]].BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                workbooksExcel.SaveAs("\\\\dbr1\\Data\\WareHouse\\PACKING_SLIPS\\" + DateTime.Now.ToString("yyyy") + "\\" + DateTime.Now.ToString("yyyy.MM") + "\\" + _fileTimeStamp + "_" + client?[0] + ".xlsm");
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "\\\\dbr1\\Data\\WareHouse\\PACKING_SLIPS\\" + DateTime.Now.ToString("yyyy") + "\\" + DateTime.Now.ToString("yyyy.MM") + "",
                    UseShellExecute = true,
                    Verb = "open"
                });
                workbooksExcel.Close(false, Type.Missing, Type.Missing);
                docExcel.Application.DisplayAlerts = false;
                docExcel.Application.Quit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void DataInserter(string fp, string thesheetName,List<WHitem>lst)
        {
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=NO;IMEX=0\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    int startRow = 12;
                    for ( int i =0; i< lst.Count;i++)
                    {
                        string stRowID = "A" + startRow+i + ":C" + startRow+i;
                        //OleDbCommand command = new OleDbCommand("INSERT INTO [" + thesheetName + "$" + stRowID +"] (IPN,MFPN,Description,Stock) values('" + lst[i].IPN + "','" + lst[i].MFPN + "','" + lst[i].Description + "','" + lst[i].Stock + "')", conn);
                        //OleDbCommand command = new OleDbCommand("INSERT INTO [" + thesheetName + "$A12] (F1) VALUES"+lst[i].IPN +")", conn);
                        OleDbCommand command=new OleDbCommand();
                        command.Connection= conn;
                        string sql= "INSERT INTO ["+ thesheetName + "$] (IPN,MFPN,Description,Qty) values('" + lst[i].IPN + "','" + lst[i].MFPN + "','" + lst[i].Description + "','" + lst[i].Stock + "')";
                        command.CommandText= sql;   
                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                }
         }
            catch (IOException)
            {
                MessageBox.Show("Error");
            }
        }
        private void btnPrintSticker_Click_1(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            loadFromXML();
        }
        private void addToXML()
        {
            //List<WHitem> allData = new List<WHitem>();
            //allData.AddRange(PackedItemsList);
            string s = SerializeToXml(PackedItemsList);
            XmlDocument xdoc = new XmlDocument();
            string theTimeStamp = DateTime.Now.ToString("_yyMMdd");
            string theLogFileName = "\\\\dbr1\\Data\\WareHouse\\PACKING_SLIPS\\PackedItemsInProgress" + theTimeStamp + ".log";
            try
            {
                xdoc.Load(theLogFileName);
                xdoc.LoadXml(s);
                xdoc.Save(theLogFileName);
            }
            catch (Exception)
            {
                xdoc.LoadXml(s);
                xdoc.Save(theLogFileName);
            }
        }
        private void loadFromXML()
        {
            openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\PACKING_SLIPS";
            openFileDialog1.Filter = "LOG files(*.log) | *.log";
            openFileDialog1.Multiselect = false;
            List<WHitem> BomItemS = new List<WHitem>();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string foldefileName = openFileDialog1.FileName;
                //label1.Text += foldefileName.ToString() + "\n";
                //groupBox2.Text += foldefileName.ToString() + " ";
                XmlSerializer serializer = new XmlSerializer(typeof(List<WHitem>));
                using (StreamReader reader = new StreamReader(openFileDialog1.FileName))
                {
                    BomItemS = (List<WHitem>)serializer.Deserialize(reader);
                }
            }
            if (BomItemS != null && BomItemS.Count > 0)
            {
                for (int i = 0; i < BomItemS.Count; i++)
                {
                    PackedItemsList.Add(BomItemS[i]);
                }
                PopulatePackedItemsGridView();
                SetColumsOrderPackedItems(dataGridView2);
                ResetAllTexboxes(LastInputFromUser);
            }
        }
        public string SerializeToXml(object input)
        {
            XmlSerializer ser = new XmlSerializer(input.GetType(), "");
            string result = string.Empty;
            using (MemoryStream memStm = new MemoryStream())
            {
                ser.Serialize(memStm, input);
                memStm.Position = 0;
                result = new StreamReader(memStm).ReadToEnd();
            }
            return result;
        }
    }
}
