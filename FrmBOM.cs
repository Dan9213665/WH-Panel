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
using Label = System.Windows.Forms.Label;
using System.Web;
using Seagull.BarTender.Print;
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
        public string fileName = string.Empty;
        public int orderQty = 0;
        public int validQty= 0;
        int i = 0;
        int loadingErrors = 0;
        public static Stopwatch stopWatch = new Stopwatch();
        public int colIpnFoundIndex;
        public int colMFPNFoundIndex;
        public TextBox lastTxtbInputFromUser = new TextBox();
        public FrmBOM()
        {
            InitializeComponent();
            ResetViews();
        }
        private void ResetViews()
        {
            checkBox1.Checked= true;
            checkBox1.BackColor= Color.LightGreen;
            listBox1.Items.Clear();
            listBox1.Update();
            label13.Text = "No Errors detected.";
            label13.BackColor = Color.LightGreen;
            label13.Update();
            label1.BackColor = Color.LightGreen;
            label11.BackColor = Color.LightGreen;
            label2.BackColor = Color.LightGreen;
            label3.BackColor = Color.LightGreen;
            label9.BackColor = Color.LightGreen;
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
                fileName = openFileDialog1.FileName;
                string Litem = Path.GetFileName(fileName);
                label12.Text += fileName.ToString() + "\n";
                DataLoader(fileName, Litem);
                KitProgressUpdate(fileName);
                button2.Enabled = true;
                PopulateMissingGridView();
                PopulateSufficientGridView();
            }
        }
        private void KitProgressUpdate(string fileName)
        {
            double percentage = double.Parse(sufficientCount.ToString())/ (countItems / 100.00) ;
            percentageComplete = Math.Round(percentage, 2);
            string text = $"{fileName} MIS:{missingCount} / SUF:{sufficientCount} of TOT:{countItems} ({percentageComplete}%)";
            this.Text = text;
            groupBox3.Text = $"Missing Items {missingCount} / {countItems}";
            groupBox5.Text = $"Sufficient Items {sufficientCount} / {countItems}";
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
                                bool qtkPar= int.TryParse(reader[indDELTA-1].ToString(), out qtk);
                                int qpu = 0;
                                bool qpuPar= int.TryParse(reader[indQty].ToString(), out qpu);
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
                                    i++;
                                    countItems++;
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
                        orderQty = int.Parse(alltheNames[2].Substring(0, alltheNames[2].Length - 8));
                        countLoadedFIles++;
                        label12.Text = "Loaded " + (countItems).ToString() + " Rows from " + countLoadedFIles + " files. In " + string.Format("{0:00}.{1:000} Seconds", ts.Seconds, ts.Milliseconds);
                        label12.Update();
                        textBox1.ReadOnly= false;
                        textBox2.ReadOnly= false;
                        textBox3.ReadOnly= false;
                        textBox9.ReadOnly= false;
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
            missingUDtable.Clear();
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
            sufficientUDtable.Clear();
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
            dgw.Columns["Calc"].DisplayIndex = 8;
            dgw.Columns["Alts"].DisplayIndex = 9;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label1.BackColor = Color.IndianRed;
            FilterTheMissingDataGridView();
            FilterTheFoundDataGridView();
        }
        public static string ExtractBetween(string inputString, string startString, string endString)
        {
            int startIndex = inputString.IndexOf(startString);
            if (startIndex < 0)
            {
                return null;
            }
            startIndex += startString.Length;

            int endIndex = inputString.IndexOf(endString, startIndex);
            if (endIndex < 0)
            {
                return null;
            }

            return inputString.Substring(startIndex, endIndex - startIndex);
        }
        private void FilterTheMissingDataGridView()
        {
            try
            {
                string searchbyMFPN = textBox2.Text;
                if (textBox2.Text.StartsWith("1P"))
                {
                    searchbyMFPN = textBox2.Text.Substring(2);
                }
                //LCLS QR decoder
                else if (textBox2.Text.StartsWith("{pbn:"))
                {
                    searchbyMFPN = ExtractBetween((textBox2.Text), "pm:", ",qty");
                }
                else if (textBox2.Text.Contains("-") == true && textBox2.Text.Length > 6)
                {
                    string[] theSplit = textBox2.Text.ToString().Split("-");
                    if (theSplit[0].Length == 3 || theSplit[0].Length >= 2|| textBox2.Text.Length>5)
                    {
                        searchbyMFPN = theSplit[1];
                    }
                    else
                    {
                        searchbyMFPN = textBox2.Text;
                    }
                }
                else if (textBox2.Text.StartsWith("P") == true)
                {
                    searchbyMFPN = textBox2.Text.Substring(1);
                }
                DataView dv = missingUDtable.DefaultView;
                dv.RowFilter = "[IPN] LIKE '%" + textBox1.Text.ToString() +
                "%' AND [ProjectName] LIKE '%" + textBox11.Text.ToString() +
                "%' AND [MFPN] LIKE '%" + searchbyMFPN +
                "%' AND [Alts] LIKE '%" + textBox9.Text.ToString() +
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
        private void FilterTheFoundDataGridView()
        {
            try
            {
                DataView dv2 = sufficientUDtable.DefaultView;
                dv2.RowFilter = "[IPN] LIKE '%" + textBox1.Text.ToString() +
                "%' AND [ProjectName] LIKE '%" + textBox11.Text.ToString() +
                "%' AND [MFPN] LIKE '%" + textBox2.Text.ToString() +
                "%' AND [Alts] LIKE '%" + textBox9.Text.ToString() +
                "%' AND [Description] LIKE '%" + textBox3.Text.ToString() + "%' ";
                dataGridView2.DataSource = dv2;
                SetColumsOrder(dataGridView2);
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
            FilterTheMissingDataGridView();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label3.BackColor = Color.IndianRed;
            FilterTheMissingDataGridView();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            clearTextboxesOnSingleLabelClick(sender, textBox1);
        }
        private void label1_DoubleClick(object sender, EventArgs e)
        {
            clearAllTextBoxesOnDoubleClick();
        }
        private void clearAllTextBoxesOnDoubleClick()
        {
            clearTextboxesOnSingleLabelClick(label1, textBox1);
            clearTextboxesOnSingleLabelClick(label2, textBox2);
            clearTextboxesOnSingleLabelClick(label3, textBox3);
            clearTextboxesOnSingleLabelClick(label9, textBox9);
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count >0)
            {
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                txtbSelIPN.Text = dataGridView1.Rows[rowindex].Cells["IPN"].Value.ToString();
                txtbSelMFPN.Text = dataGridView1.Rows[rowindex].Cells["MFPN"].Value.ToString();
                txtbSelDes.Text = dataGridView1.Rows[rowindex].Cells["Description"].Value.ToString();
                txtbQtyToAdd.Clear();
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            JumpToQtyInput((TextBox)sender,e);
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            JumpToQtyInput((TextBox)sender, e);
        }
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            JumpToQtyInput((TextBox)sender, e);
        }
        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            JumpToQtyInput((TextBox)sender, e);
        }
        private void JumpToQtyInput(TextBox t, KeyEventArgs e)
        {
            lastTxtbInputFromUser = t;
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView1.Rows.Count == 1)
                {
                    txtbQtyToAdd.Focus();
                }
                else
                {
                    dataGridView1.Focus();
                }
            }
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
        private void textBox9_Enter(object sender, EventArgs e)
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
        private void textBox9_Leave(object sender, EventArgs e)
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
            string inputQty = txtbQtyToAdd.Text.ToString();
            if (inputQty.StartsWith("Q"))
             {
                inputQty= txtbQtyToAdd.Text.Substring(1);
            }
            validQty = 0;
            bool qtyOK = int.TryParse(inputQty, out validQty);
            if(qtyOK)
            {
                    updateQtyInBomFile(w, validQty);
                if(checkBox1.Checked)
                {
                    WHitem itemToPrint = new WHitem();
                    itemToPrint.IPN = w.IPN;
                    itemToPrint.MFPN = w.MFPN;
                    itemToPrint.Description = w.Description;
                    itemToPrint.Stock = validQty;
                    itemToPrint.UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
                    printSticker(itemToPrint);
                }
            }
        }
        private void updateQtyInBomFile(KitHistoryItem w,int qtyToAdd)
        {
            KitHistoryItem itemToUpdate = MissingItemsList.FirstOrDefault(r => r.IPN == w.IPN && r.QtyPerUnit == w.QtyPerUnit);
            if (itemToUpdate != null)
            {
                if (itemToUpdate.QtyInKit > 0 && itemToUpdate.Calc == string.Empty)
                {
                    itemToUpdate.Calc = $"{w.QtyInKit}+{qtyToAdd}";
                    itemToUpdate.QtyInKit += qtyToAdd;
                    itemToUpdate.Delta = itemToUpdate.QtyInKit - (itemToUpdate.QtyPerUnit * orderQty);
                }
                else if (itemToUpdate.QtyInKit > 0 && itemToUpdate.Calc != string.Empty)
                {
                    itemToUpdate.Calc += $"+{qtyToAdd}";
                    itemToUpdate.QtyInKit += qtyToAdd;
                    itemToUpdate.Delta = itemToUpdate.QtyInKit - (itemToUpdate.QtyPerUnit * orderQty);
                }
                else if (itemToUpdate.QtyInKit == 0 && itemToUpdate.Calc == string.Empty)
                {
                    itemToUpdate.QtyInKit = qtyToAdd;
                    itemToUpdate.Delta = itemToUpdate.QtyInKit - (itemToUpdate.QtyPerUnit * orderQty);
                }
                if (itemToUpdate.Delta >= 0)
                {
                    MissingItemsList.Remove(itemToUpdate);
                    PopulateMissingGridView();
                    SufficientItemsList.Add(itemToUpdate);
                    sufficientCount++;
                    missingCount--;
                    PopulateSufficientGridView();
                    KitProgressUpdate(fileName);
                    UpdateKitHistoryItem(fileName,itemToUpdate);
                }
                else
                {
                    PopulateMissingGridView();
                    UpdateKitHistoryItem( fileName, itemToUpdate);
                }
            }
            else
            {
            }
        }
        private void txtbQtyToAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnPrintSticker_Click(sender, e);
                txtbQtyToAdd.Clear();
                clearAllTextBoxesOnDoubleClick();
                lastTxtbInputFromUser.Clear();
                lastTxtbInputFromUser.Focus();
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
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
                string fp = $@"C:\Users\{userName}\Desktop\Print_Stickers.xlsx"; // Use string interpolation instead of concatenation
                string thesheetName = "Sheet1";
                string constr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fp}; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand($"UPDATE [{thesheetName}$] SET PN = @PN, MFPN = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, UPDATEDON = @UPDATEDON", conn))
                    {
                        cmd.Parameters.AddWithValue("@PN", wHitem.IPN);
                        cmd.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                        cmd.Parameters.AddWithValue("@ItemDesc", wHitem.Description);
                        cmd.Parameters.AddWithValue("@QTY", wHitem.Stock);
                        cmd.Parameters.AddWithValue("@UPDATEDON", wHitem.UpdatedOn);
                        cmd.ExecuteNonQuery();
                    }
                }
                // Launch BarTender Designer and print the sticker
                Microsoft.VisualBasic.Interaction.AppActivate("PN_STICKER_2022.btw - BarTender Designer");
                SendKeys.SendWait("^p");
                SendKeys.SendWait("{Enter}");
                // Bring back the focus to the main application
                Microsoft.VisualBasic.Interaction.AppActivate("Imperium Tabula Principalis");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sticker printing failed : {ex.Message}");
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {
            clearTextboxesOnSingleLabelClick(sender, textBox3);
        }
        private void label2_Click(object sender, EventArgs e)
        {
            clearTextboxesOnSingleLabelClick(sender,textBox2);
        }
        private void clearTextboxesOnSingleLabelClick(object sender, TextBox txtb)
        {
            Label l = new Label();
            l = (Label)sender;
            txtb.Text = string.Empty;
            l.BackColor = Color.LightGreen;
            txtb.Focus();
        }
        private void label2_DoubleClick(object sender, EventArgs e)
        {
            clearAllTextBoxesOnDoubleClick();
        }
        private void label3_DoubleClick(object sender, EventArgs e)
        {
            clearAllTextBoxesOnDoubleClick();
        }
        private void button2_Click(object sender, EventArgs e)
        {
           AuthorizedExcelFileOpening(AddQuotesIfRequired(fileName));
        }
        private void AuthorizedExcelFileOpening(string fp)
        {
            if (Environment.UserName == "lgt")
            {
                openWHexcelDB(fp);
            }
            else
            {
                MessageBox.Show("Unauthorized ! Access denied !", "Unauthorized ! Access denied !", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void openWHexcelDB(string thePathToFile)
        {
            Process excel = new Process();
            excel.StartInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.exe";
            excel.StartInfo.Arguments = thePathToFile;
            excel.Start();
        }
        public string AddQuotesIfRequired(string path)
        {
            return !string.IsNullOrWhiteSpace(path) ?
                path.Contains(" ") && (!path.StartsWith("\"") && !path.EndsWith("\"")) ?
                    "\"" + path + "\"" : path :
                    string.Empty;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\DocumentsForProduction\\WORK_PROGRAM.xlsm";
            openWHexcelDB(fp);
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            label9.BackColor = Color.IndianRed;
            FilterTheMissingDataGridView();
        }
        private void label9_Click(object sender, EventArgs e)
        {
            clearTextboxesOnSingleLabelClick(sender, textBox9);
        }
        private void label9_DoubleClick(object sender, EventArgs e)
        {
            clearAllTextBoxesOnDoubleClick();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtbQtyToAdd.Focus();
        }
        private void UpdateKitHistoryItem(string fp,KitHistoryItem itemToUpdate)
        {
            try
            {
                string normalizedPath = AddQuotesIfRequired(fp);
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + normalizedPath + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    DataTable dbSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                  if (dbSchema == null || dbSchema.Rows.Count < 1)
                    {
                        throw new Exception("Error: Could not determine the name of the first worksheet.");
                    }
                    string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();
                    string cleanedUpSheetName = firstSheetName.Substring(1).Substring(0, firstSheetName.Length - 3);
                    string kitColumnName = getKitColIndex(fp);
                    OleDbCommand command = new OleDbCommand("UPDATE [" + cleanedUpSheetName + "$] SET ["+ kitColumnName + "] = @QtyInKit,[Calc] = @Calc WHERE [IPN] = @IPN AND [MFPN] = @MFPN", conn);
                    command.Parameters.AddWithValue("@QtyInKit", itemToUpdate.QtyInKit);
                    command.Parameters.AddWithValue("@Calc", itemToUpdate.Calc);
                    command.Parameters.AddWithValue("@IPN", itemToUpdate.IPN);
                    command.Parameters.AddWithValue("@MFPN", itemToUpdate.MFPN);
                    int rowsAffected = command.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected > 0)
                    {
                        //MessageBox.Show("BOM item was updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No rows were updated.");
                    }
                }
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private string getKitColIndex(string fp)
        {
            string columnName = string.Empty;
            string normalizedPath = AddQuotesIfRequired(fp);
            string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + normalizedPath + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
            using (OleDbConnection conn = new OleDbConnection(constr))
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
                command.Connection = conn;
                OleDbDataReader reader = command.ExecuteReader();
                DataTable schemaTable = reader.GetSchemaTable();
                foreach (DataRow row in schemaTable.Rows)
                {
                    string currentColumnName = (string)row["ColumnName"];
                    if (currentColumnName.StartsWith("KIT"))
                    {
                        columnName = currentColumnName;
                        break;
                    }
                }
                reader.Close();
                conn.Close();
            }
            return columnName;
        }
        private static void txtbColorGreenOnEnter(object sender)
        {
            if (sender is TextBox tb)
            {
                tb.BackColor = Color.LightGreen;
            }
        }
        private static void txtbColorWhiteOnLeave(object sender)
        {
            if (sender is TextBox tb)
            {
                tb.BackColor = Color.White;
            }
        }
    }
}
