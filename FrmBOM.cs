using FastMember;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Outlook;
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
using Range = Microsoft.Office.Interop.Excel.Range;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Application = Microsoft.Office.Interop.Excel.Application;
using static Seagull.Framework.OS.ServiceControlManager;
using System;
using System.Windows.Forms;
using Exception = System.Exception;
using _Application = Microsoft.Office.Interop.Excel._Application;
using OfficeOpenXml;
using System.Drawing.Printing;
using Button = System.Windows.Forms.Button;
using GroupBox = System.Windows.Forms.GroupBox;

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
        public int validQty = 0;
        int i = 0;
        int loadingErrors = 0;
        public static Stopwatch stopWatch = new Stopwatch();
        public int colIpnFoundIndex;
        public int colMFPNFoundIndex;
        public TextBox lastTxtbInputFromUser = new TextBox();
        public string theExcelFilePath = string.Empty;
        public FrmBOM()
        {
            InitializeComponent();
            UpdateControlColors(this);
            ResetViews();
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
                    dataGridView.DefaultCellStyle.BackColor = Color.Gray;
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
        private void ResetViews()
        {
            checkBox1.Checked = false;
            checkBox1.BackColor = Color.IndianRed;
            checkBox1.Text = "No sticker needed";
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
            cmbReelSelector.SelectedIndex = 1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            btn1ClickLogic();
        }

        private void btn1ClickLogic()
        {
            ResetViews();
            var result = openFileDialog1.Title;
            openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\2023\\" + DateTime.Now.ToString("MM") + ".2023";
            openFileDialog1.Filter = "BOM files(*.xlsm) | *.xlsm";
            openFileDialog1.Multiselect = false;
            List<KitHistoryItem> BomItemS = new List<KitHistoryItem>();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Environment.UserName != "lgt")
                {
                    Application excel = new Application();
                    Workbook workbook = excel.Workbooks.Open(openFileDialog1.FileName);
                    Worksheet firstSheet = (Worksheet)workbook.Worksheets[1]; // Cast the object to a Worksheet type
                    firstSheet.Calculate(); // Calculate only the first sheet
                                            // Save the changes
                    workbook.Save();
                    workbook.Close();
                    excel.Quit();
                }
                else
                {
                    //
                }

                fileName = openFileDialog1.FileName;
                theExcelFilePath = Path.GetFileName(fileName);
                string Litem = Path.GetFileName(fileName);
                label12.Text += fileName.ToString() + "\n";
                DataLoader(fileName, Litem);
                KitProgressUpdate(fileName);
                button2.Enabled = true;
                PopulateMissingGridView();
                PopulateSufficientGridView();
            }
        }

        public void ExternalLinktoFile(string externalPathToExcelFIle)
        {
            ResetViews();
            List<KitHistoryItem> BomItemS = new List<KitHistoryItem>();
            if (externalPathToExcelFIle != null)
            {
                fileName = externalPathToExcelFIle;
                theExcelFilePath = Path.GetFileName(fileName);
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
            double percentage = double.Parse(sufficientCount.ToString()) / (countItems / 100.00);
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
                        string[] alltheNames = excelFIleName.Split("_");
                        textBox11.Text = alltheNames[1];
                        textBox6.Text = alltheNames[2].Substring(0, alltheNames[2].Length - 5);
                        orderQty = int.Parse(alltheNames[2].Substring(0, alltheNames[2].Length - 8));
                        countLoadedFIles++;
                        label12.Text = "Loaded " + (countItems).ToString() + " Rows from " + countLoadedFIles + " files. In " + string.Format("{0:00}.{1:000} Seconds", ts.Seconds, ts.Milliseconds);
                        label12.Update();
                        textBox1.ReadOnly = false;
                        textBox2.ReadOnly = false;
                        textBox3.ReadOnly = false;
                        textBox9.ReadOnly = false;
                        textBox1.Focus();
                    }
                    catch (Exception e)
                    {
                        loadingErrors++;
                        label13.Text = loadingErrors.ToString() + " Loading Errors detected: " + e.Message;
                        listBox1.Items.Add(e.Message.ToString());
                        listBox1.Height = 50;
                        listBox1.Update();
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
            FilterTheFoundDataGridView();
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
            if (dataGridView1.SelectedCells.Count > 0)
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
            JumpToQtyInput((TextBox)sender, e);
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
                else if (dataGridView1.Rows.Count == 0)
                {
                    AutoClosingMessageBox.Show(lastTxtbInputFromUser.Text + " NOT FOUND !", "item not FOUND !", 1000);
                    lastTxtbInputFromUser.Text = string.Empty;
                    lastTxtbInputFromUser.Focus();
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
                inputQty = txtbQtyToAdd.Text.Substring(1);
            }
            validQty = 0;
            bool qtyOK = int.TryParse(inputQty, out validQty);
            if (qtyOK)
            {
                updateQtyInBomFile(w, validQty);
                transferFromDatabaseToKit(w, validQty, theExcelFilePath.Substring(0, theExcelFilePath.Length - 5));
                if (checkBox1.Checked)
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
        private void updateQtyInBomFile(KitHistoryItem w, int qtyToAdd)
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
                    SufficientItemsList.Insert(0, itemToUpdate);
                    sufficientCount++;
                    missingCount--;
                    PopulateSufficientGridView();
                    KitProgressUpdate(fileName);
                    UpdateKitHistoryItem(fileName, itemToUpdate);
                }
                else
                {
                    PopulateMissingGridView();
                    UpdateKitHistoryItem(fileName, itemToUpdate);
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
            clearTextboxesOnSingleLabelClick(sender, textBox2);
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
            if (Environment.UserName == "lgt" || Environment.UserName == "rbtwh")
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
        private void UpdateKitHistoryItem(string fp, KitHistoryItem itemToUpdate)
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
                    OleDbCommand command = new OleDbCommand("UPDATE [" + cleanedUpSheetName + "$] SET [" + kitColumnName + "] = @QtyInKit,[Calc] = @Calc WHERE [IPN] = @IPN AND [MFPN] = @MFPN", conn);
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
        private void button4_Click(object sender, EventArgs e)
        {
            if (MissingItemsList.Count > 0)
            {
                FrmBomWHS wh = new FrmBomWHS();
                wh.fromTheMainBom = new List<KitHistoryItem>();
                wh.fromTheMainBom.Clear();
                wh.fromTheMainBom = MissingItemsList;
                wh.Show();
            }
            else
            {
                MessageBox.Show("No missing items to search for !");
            }
        }
        private void btnPrintKitLabel_Click(object sender, EventArgs e)
        {
            EXCELinserter(theExcelFilePath.Substring(0, theExcelFilePath.Length - 5));
            //ExcelInserterUsingEPPlus(theExcelFilePath.Substring(0, theExcelFilePath.Length - 5));
        }
        //private void ExcelInserterUsingOleDb(string kitName)
        //{
        //    try
        //    {
        //        string filePath = @"\\dbr1\Data\WareHouse\KitLabelAuto.xlsx";
        //        string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties='Excel 12.0 Macro;HDR=NO;IMEX=1;'";

        //        using (OleDbConnection connection = new OleDbConnection(connectionString))
        //        {
        //            connection.Open();

        //            // Open the first worksheet
        //            string sheetName = "Sheet1$"; // Update this based on your Excel sheet name
        //            string updateQuery = $"UPDATE [{sheetName}] SET [ColumnB] = @kitName";

        //            using (OleDbCommand command = new OleDbCommand(updateQuery, connection))
        //            {
        //                command.Parameters.AddWithValue("@kitName", kitName);
        //                command.ExecuteNonQuery();
        //            }

        //            MessageBox.Show("Data Updated in .xlsx file");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }
        //}

        private void ExcelInserterUsingEPPlus(string kitName)
        {
            try
            {
                string filePath = @"\\dbr1\Data\WareHouse\KitLabelAuto.xlsx";

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Assuming first worksheet

                    // Update cell value
                    worksheet.Cells["B1"].Value = kitName;

                    // Set column width to a specific value (e.g., 20)
                    worksheet.Column(2).Width = 51; // Column B

                    // Wrap text within the cell
                    worksheet.Cells["B1"].Style.WrapText = true;

                    // Print the entire worksheet
                    worksheet.PrinterSettings.FitToPage = true; // Fit to a single page
                    worksheet.PrinterSettings.Orientation = eOrientation.Landscape; // Set to eOrientation.Portrait if needed

                    // Print the worksheet to the default printer
                    //worksheet.PrintOut();

                    // Save the changes to the Excel file
                    package.Save();

                    MessageBox.Show("Data Updated and Printed");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void EXCELinserter(string kitName)
        {
            try
            {
                string fp = "\\\\dbr1\\Data\\WareHouse\\KitLabel.xlsm";
                _Application docExcel = new Microsoft.Office.Interop.Excel.Application();
                docExcel.Visible = false;
                docExcel.DisplayAlerts = false;
                _Workbook workbooksExcel = docExcel.Workbooks.Open(@fp, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                _Worksheet worksheetExcel = (_Worksheet)workbooksExcel.Worksheets[1];
                ((Range)worksheetExcel.Cells[1, "B"]).Value2 = kitName;
                ((Range)worksheetExcel.Columns["B"]).ColumnWidth = 51;
                ((Range)worksheetExcel.Cells[3, "B"]).WrapText = true;
                ((Range)worksheetExcel.Cells[6, "B"]).WrapText = true;
                ((Range)worksheetExcel.Cells[9, "B"]).WrapText = true;
                workbooksExcel.PrintOutEx(1, 1, 1);
                workbooksExcel.Close(false, Type.Missing, Type.Missing);
                docExcel.Application.DisplayAlerts = false;
                docExcel.Application.Quit();
                MessageBox.Show("Label Sent to printer");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void transferFromDatabaseToKit(KitHistoryItem w, int qtyToMove, string kitName)
        {
            try
            {
                WHitem itemToTransfer = new WHitem()
                {
                    IPN = w.IPN,
                    Manufacturer = "",
                    MFPN = w.MFPN,
                    Description = w.Description,
                    Stock = qtyToMove * (-1),
                    UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"),
                    ReelBagTrayStick = cmbReelSelector.SelectedItem.ToString(),
                    SourceRequester = kitName
                };
                if (!System.String.IsNullOrEmpty(warehouseSelectorBasedOnItem(w)))
                {
                    itemToTransfer.Manufacturer = getTheManufacturerFromTheStock(warehouseSelectorBasedOnItem(w), itemToTransfer);
                    itemToTransfer.ReelBagTrayStick = getTheReelBagTrayStickFromTheStock(warehouseSelectorBasedOnItem(w), itemToTransfer);
                    DataInserter(warehouseSelectorBasedOnItem(w), "STOCK", itemToTransfer);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
        private string getTheManufacturerFromTheStock(string fp, WHitem itemTolookby)
        {
            string manufacturerFromStock = string.Empty;
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("SELECT * FROM [STOCK$] WHERE IPN=? AND MFPN=? AND Stock=?", conn);
                    command.Parameters.AddWithValue("@IPN", itemTolookby.IPN);
                    command.Parameters.AddWithValue("@MFPN", itemTolookby.MFPN);
                    command.Parameters.AddWithValue("@Stock", Math.Abs(itemTolookby.Stock));
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //MessageBox.Show("Manufacturer from stock: " + reader["Manufacturer"].ToString());
                        manufacturerFromStock = reader["Manufacturer"].ToString();
                        break;
                        // Add more code here to display other columns or perform other actions on each row
                    }
                    reader.Close();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return manufacturerFromStock;
        }
        private string getTheReelBagTrayStickFromTheStock(string fp, WHitem itemTolookby)
        {
            string ReelBagTrayStickFromStock = string.Empty;
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("SELECT * FROM [STOCK$] WHERE IPN=? AND MFPN=? AND Stock=?", conn);
                    command.Parameters.AddWithValue("@IPN", itemTolookby.IPN);
                    command.Parameters.AddWithValue("@MFPN", itemTolookby.MFPN);
                    command.Parameters.AddWithValue("@Stock", Math.Abs(itemTolookby.Stock));
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //MessageBox.Show("ReelBagTrayStick from stock: " + reader["Comments"].ToString());
                        ReelBagTrayStickFromStock = reader["Comments"].ToString();
                        break;
                        // Add more code here to display other columns or perform other actions on each row
                    }
                    reader.Close();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return ReelBagTrayStickFromStock;
        }

        private string warehouseSelectorBasedOnItem(KitHistoryItem w)
        {
            string selection = string.Empty;
            if (w.IPN.StartsWith("C100") || w.IPN.StartsWith("A00"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_STOCK.xlsm";
            }
            else if (w.IPN.StartsWith("NET"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_STOCK.xlsm";
            }
            else if (w.IPN.StartsWith("VAY"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_stock.xlsm";
            }
            else if (w.IPN.StartsWith("VAL"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm";
            }
            else if (w.IPN.StartsWith("ROB"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_STOCK.xlsm";
            }
            else if (w.IPN.StartsWith("ENE"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ENERCON\\ENERCON_STOCK.xlsm";
            }
            else if (w.IPN.StartsWith("DGT"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\DIGITRONIX\\DIGITRONIX_STOCK.xlsm";
            }
            else if (w.IPN.StartsWith("HEP"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\HEPTAGON\\HEPTAGON_STOCK.xlsm";
            }
            else if (w.IPN.StartsWith("EPS"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\EPS\\EPS_STOCK.xlsm";
            }
            else if (w.IPN.StartsWith("SOS"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOS\\SOS_STOCK.xlsm";
            }
            else if (w.IPN.StartsWith("ARN"))
            {
                selection = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ARAN\\ARAN_STOCK.xlsm";
            }
            else
            {
                selection = string.Empty;
            }
            return selection;
        }
        private void DataInserter(string fp, string thesheetName, WHitem wHitem)
        {
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("INSERT INTO [" + thesheetName + "$] (IPN,Manufacturer,MFPN,Description,Stock,Updated_on,Comments,Source_Requester) values('" + wHitem.IPN + "','" + wHitem.Manufacturer + "','" + wHitem.MFPN + "','" + wHitem.Description + "','" + wHitem.Stock + "','" + wHitem.UpdatedOn + "','" + wHitem.ReelBagTrayStick + "','" + wHitem.SourceRequester + "')", conn);
                    command.ExecuteNonQuery();

                    conn.Close();
                }
                txtbQtyToAdd.Clear();
                lastTxtbInputFromUser.Clear();
                label2.BackColor = Color.LightGreen;
                label3.BackColor = Color.LightGreen;
                lastTxtbInputFromUser.Focus();
                AutoClosingMessageBox.Show(wHitem.IPN + " Transferred to " + wHitem.SourceRequester, " Item Transferred to " + wHitem.SourceRequester, 1000);
            }
            catch (IOException)
            {
                MessageBox.Show("Error");
            }
        }
        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                using (_timeoutTimer)
                    MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            WHitem itemToPrint = new WHitem();
            if (dataGridView2.SelectedCells.Count == 1)
            {
                int rowindex = dataGridView2.CurrentCell.RowIndex;
                itemToPrint.IPN = dataGridView2.Rows[rowindex].Cells["IPN"].Value.ToString();
                itemToPrint.MFPN = dataGridView2.Rows[rowindex].Cells["MFPN"].Value.ToString();
                itemToPrint.Description = dataGridView2.Rows[rowindex].Cells["Description"].Value.ToString();
                itemToPrint.Stock = int.Parse(dataGridView2.Rows[rowindex].Cells["QtyInKit"].Value.ToString());
                itemToPrint.UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");

            }
            printSticker(itemToPrint);
            //printStickerCopy(itemToPrint);
        }

        private void printStickerCopy(WHitem itToPrint)
        {
            //MessageBox.Show("Test");
        }


        private void button5_Click(object sender, EventArgs e)
        {
            ReloadLogic();

        }

        public void ReloadLogic()
        {
            if (fileName != string.Empty)
            {
                ResetViews();
                theExcelFilePath = Path.GetFileName(fileName);
                string Litem = Path.GetFileName(fileName);
                label12.Text += fileName.ToString() + "\n";
                DataLoader(fileName, Litem);
                KitProgressUpdate(fileName);
                button2.Enabled = true;
                PopulateMissingGridView();
                PopulateSufficientGridView();
            }
            else
            {
                //
            }
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string inputStr = textBox12.Text;
                string startStr = comboBox4.Text.ToString();
                string endStr = comboBox5.Text.ToString();

                int startIndex = inputStr.IndexOf(startStr);
                if (startIndex != -1)
                {
                    startIndex += startStr.Length;
                    int endIndex = inputStr.IndexOf(endStr, startIndex);
                    if (endIndex != -1)
                    {
                        string extractedStr = inputStr.Substring(startIndex, endIndex - startIndex);
                        textBox2.Text = extractedStr;
                        lastTxtbInputFromUser = textBox12;
                        textBox2.Focus();
                        textBox2_KeyDown(sender, e);
                    }
                }

            }
        }

        private void textBox12_Click(object sender, EventArgs e)
        {
            textBox12.Clear();
        }





        // Event handler for the button click
        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            //// Get the contents of the DataGridView
            //string contents = GetDataGridViewContents(dataGridView1); // Replace 'dataGridView1' with the name of your DataGridView control



            //// Focus on the current instance of Outlook
            //FocusOnOutlook();

            //// Create a new email and paste the contents into the body
            //CreateNewEmail();
            //AndPaste(contents);
        }

        private void textBox13_Click(object sender, EventArgs e)
        {
            textBox13.Clear();
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchbyMFPN = string.Empty;

                if (textBox13.Text.Contains("-") == true && textBox13.Text.Length > 6)
                {
                    //string[] theSplit = textBox13.Text.ToString().Split("-");
                    ////if (theSplit[0].Length == 3 || theSplit[0].Length >= 2 || textBox13.Text.Length > 5)
                    //if (theSplit[0].Length >= 3)
                    //{
                    //    searchbyMFPN = theSplit[1];

                    //}
                    string[] theSplit = textBox13.Text.Split("-");
                    if (theSplit.Length > 1)
                    {
                        searchbyMFPN = string.Join("-", theSplit, 1, theSplit.Length - 1);
                    }
                    else
                    {
                        searchbyMFPN = textBox13.Text;
                    }

                    textBox2.Text = searchbyMFPN;
                }
                else
                {

                }
                lastTxtbInputFromUser = textBox13;
                textBox2.Focus();
                textBox2_KeyDown(sender, e);
            }
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter((TextBox)sender);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter((TextBox)sender);
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave((TextBox)sender);
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave((TextBox)sender);
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchbyMFPN = textBox14.Text.Trim(); // Get the text from textBox14

                // Remove [)> characters from the search string
                searchbyMFPN = searchbyMFPN.Replace("[)>", "");

                if (!string.IsNullOrEmpty(searchbyMFPN))
                {
                    // Loop through the DataGridView rows and filter based on MFPN
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["MFPN"].Value != null)
                        {
                            string cellValue = row.Cells["MFPN"].Value.ToString();
                            // Check if the search text contains the cell value
                            if (searchbyMFPN.Contains(cellValue))
                            {
                                textBox2.Text = cellValue;
                            }
                            else
                            {
                                //row.Visible = false;
                            }
                        }
                    }
                }
                else
                {
                    // If the search text is empty, show all rows
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.Visible = true;
                    }
                }

                lastTxtbInputFromUser = textBox14;
                textBox2.Focus();
                textBox2_KeyDown(sender, e);
            }
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter((TextBox)sender);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave((TextBox)sender);
        }

        private void textBox14_Click(object sender, EventArgs e)
        {
            textBox14.Clear();
        }
    }
}
