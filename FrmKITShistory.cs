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
using File = System.IO.File;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;
using Label = System.Windows.Forms.Label;
using Button = System.Windows.Forms.Button;
using GroupBox = System.Windows.Forms.GroupBox;

namespace WH_Panel
{
    public partial class FrmKITShistory : Form
    {
        public List<KitHistoryItem> KitHistoryItemsList = new List<KitHistoryItem>();
        public DataTable UDtable = new DataTable();
        public int countItems = 0;
        public int countLoadedFIles = 0;
        int i = 0;
        int loadingErrors = 0;
        public static Stopwatch stopWatch = new Stopwatch();
        public int colIpnFoundIndex;
        public int colMFPNFoundIndex;
        List<string> listOfPaths = new List<string>() { };
        //public List<string> listOfPaths = new List<string>()
        //    {
        //       // "\\\\dbr1\\Data\\WareHouse\\2022\\09.2022",
        //       // "\\\\dbr1\\Data\\WareHouse\\2022\\10.2022",
        //       // "\\\\dbr1\\Data\\WareHouse\\2022\\11.2022",
        //        //"\\\\dbr1\\Data\\WareHouse\\2022\\12.2022",
        //        "\\\\dbr1\\Data\\WareHouse\\2023"
        //    };
        public FrmKITShistory()
        {
            InitializeComponent();
            UpdateControlColors(this);
        }
        private void UpdateControlColors(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                // Update control colors based on your criteria
                control.BackColor = Color.LightGray;
                control.ForeColor = Color.White;

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
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView.RowHeadersDefaultCellStyle.BackColor = Color.Gray;
                    dataGridView.DefaultCellStyle.BackColor = Color.Gray;
                    dataGridView.DefaultCellStyle.ForeColor = Color.White;
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
        private void button1_Click(object sender, EventArgs e)
        {
            stopWatch.Reset();
            ResetViews();
            startUpLogic(2);
            SetColumsOrder();
            textBox1.Focus();
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
            label9.BackColor = Color.LightGreen;
            KitHistoryItemsList.Clear();
            countItems = 0;
            countLoadedFIles = 0;
            i = 0;
            loadingErrors = 0;
            label12.Text = string.Empty;
            UDtable.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }
        private void startUpLogic(int timeSpan)
        {
            stopWatch.Start();
            label12.BackColor = Color.IndianRed;


            if (timeSpan == 2)
            {
                listOfPaths = listOfPathsAggregator(1);
            }
            else if (timeSpan == 6)
            {
                listOfPaths = listOfPathsAggregator(5);
            }
            else if (timeSpan == 12)
            {
                listOfPaths = listOfPathsAggregator(11);
            }


            foreach (string path in listOfPaths)
            {
                foreach (string file in Directory.EnumerateFiles(path, "*.xlsm", SearchOption.AllDirectories))
                {
                    countLoadedFIles++;
                    string Litem = Path.GetFileName(file);
                    string fileName = Path.GetFileName(file);
                    if (FileIsLocked(Litem))
                    {
                        string copyFilePath = CreateCopyOfFile(file);
                        DataLoader(copyFilePath, fileName);
                        DeleteFile(copyFilePath);
                        //DataLoader(file, Litem);
                    }
                    else
                    {
                        AddErrorousFilesToListOfErrors(Litem);
                    }
                }
            }
            PopulateGridView();
            SetColumsOrder();
            stopWatch.Stop();
        }

        //private List<string> listOfPathsAggregator(int numMonths)
        //{
        //    List<string> list = new List<string>();

        //    string main = "\\\\dbr1\\Data\\WareHouse\\";
        //    DateTime d = DateTime.Now;

        //    for (int i = 0; i < numMonths; i++)
        //    {
        //        string year = d.Year.ToString("D4");
        //        int month = d.Month;

        //        if (month == 1) // If it's January, adjust year and month accordingly
        //        {
        //            year = (d.Year - 1).ToString("D4");
        //            month = 12; // Set month to December
        //        }
        //        else
        //        {
        //            //month--;
        //        }

        //        string previousMonthPath = $"{main}{year}\\{month:D2}.{year}";
        //        list.Add(previousMonthPath);

        //        d = d.AddMonths(-1); // Move to the previous month
        //    }

        //    list.Reverse(); // Since we're adding paths in reverse order, reverse the list

        //    return list;
        //}
        private List<string> listOfPathsAggregator(int numMonths)
        {
            List<string> list = new List<string>();

            string main = "\\\\dbr1\\Data\\WareHouse\\";
            DateTime d = DateTime.Now;

            string cyear = d.Year.ToString("D4");
            int cmonth = d.Month;
            string currentMonthPath = $"{main}{cyear}\\{cmonth:D2}.{cyear}";
            list.Add(currentMonthPath);

            for (int i = 0; i < numMonths; i++)
            {
                string year = d.Year.ToString("D4");
                int month = d.Month;



                if (month == 1) // If it's January, adjust year and month accordingly
                {
                    year = (d.Year - 1).ToString("D4");
                    month = 12; // Set month to December
                }
                else
                {
                    month--;
                }

                string previousMonthPath = $"{main}{year}\\{month:D2}.{year}";
                list.Add(previousMonthPath);

                d = d.AddMonths(-1); // Move to the previous month
            }

            list.Reverse(); // Since we're adding paths in reverse order, reverse the list

            return list;
        }

        private string CreateCopyOfFile(string filePath)
        {
            string copyFilePath = Path.Combine(Path.GetDirectoryName(filePath), "Copy_" + Path.GetFileName(filePath));
            File.Copy(filePath, copyFilePath, true);
            return copyFilePath;
        }
        private void DeleteFile(string filePath)
        {
            File.Delete(filePath);
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
        public bool FileIsLocked(string strFullFileName)
        {
            bool blnReturn;
            System.IO.FileStream fs;
            try
            {
                fs = System.IO.File.Open(strFullFileName, System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.None);
                fs.Close();
                fs.Dispose();
                blnReturn = false;
            }
            catch (System.IO.IOException ex)
            {
                blnReturn = true;
            }
            return blnReturn;
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
                                int indIPN = reader.GetOrdinal("IPN");
                                int indMFPN = reader.GetOrdinal("MFPN");
                                int indDescription = reader.GetOrdinal("Description");
                                int indDELTA = reader.GetOrdinal("DELTA");
                                int indQty = indDELTA + 2;//reader.GetOrdinal("Qty");
                                int indCalc = indDELTA + 3;//reader.GetOrdinal("Calc");
                                int indAlts = indDELTA + 4;

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
                                    Calc = reader[indQty + 1].ToString(),
                                    Alts = reader[indQty + 2].ToString()
                                };
                                countItems = i;
                                label12.Text = "Loaded " + (countItems).ToString() + " Rows from " + countLoadedFIles + " files. In " + string.Format("{0:00}.{1:000} Seconds", ts.Seconds, ts.Milliseconds);
                                if (countItems % 1000 == 0)
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
        private void PopulateGridView()
        {
            IEnumerable<KitHistoryItem> data = KitHistoryItemsList;
            using (var reader = ObjectReader.Create(data))
            {
                UDtable.Load(reader);
            }
            dataGridView1.DataSource = UDtable;
            SetColumsOrder();
            label12.BackColor = Color.LightGreen;
        }
        private void SetColumsOrder()
        {
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["DateOfCreation"].DisplayIndex = 0;
            dataGridView1.Columns["ProjectName"].DisplayIndex = 1;
            dataGridView1.Columns["IPN"].DisplayIndex = 2;
            dataGridView1.Columns["MFPN"].DisplayIndex = 3;
            dataGridView1.Columns["Description"].DisplayIndex = 4;
            dataGridView1.Columns["QtyInKit"].DisplayIndex = 5;
            dataGridView1.Columns["Delta"].DisplayIndex = 6;
            dataGridView1.Columns["QtyPerUnit"].DisplayIndex = 7;
            dataGridView1.Columns["Calc"].DisplayIndex = 8;
            dataGridView1.Columns["Alts"].DisplayIndex = 9;
        }
        private void FilterTheDataGridView()
        {
            try
            {
                DataView dv = UDtable.DefaultView;
                dv.RowFilter = "[IPN] LIKE '%" + textBox1.Text.ToString() +
                     "%' AND [ProjectName] LIKE '%" + textBox11.Text.ToString() +
                "%' AND [MFPN] LIKE '%" + textBox2.Text.ToString() +
                "%' AND [Alts] LIKE '%" + textBox9.Text.ToString() +
                "%' AND [Description] LIKE '%" + textBox3.Text.ToString() + "%' ";
                dataGridView1.DataSource = dv;
                SetColumsOrder();
                SetColumsOrder();
                ColorTheDelta(dataGridView1);
                //dataGridView1.Update();
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect search pattern, remove invalid character and try again !", "Search pattern error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current column is "DELTA" and the cell is not a header cell.
            if (e.ColumnIndex == dataGridView1.Columns["DELTA"].Index && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value != null && cell.Value != DBNull.Value)
                {
                    double deltaValue = Convert.ToDouble(cell.Value);

                    // Set the background color based on the delta value.
                    if (deltaValue < 0)
                    {
                        cell.Style.BackColor = Color.IndianRed;
                    }
                    else
                    {
                        //cell.Style.BackColor = Color.LightGreen;
                    }
                }
            }
        }

        private void ColorTheDelta(DataGridView dw)
        {
            // This method can be used to initially color the cells.
            // You can call it with your DataGridView object.
            foreach (DataGridViewRow row in dw.Rows)
            {
                DataGridViewCell cell = row.Cells["DELTA"];
                if (cell.Value != null && cell.Value != DBNull.Value)
                {
                    double deltaValue = Convert.ToDouble(cell.Value);

                    // Set the background color based on the delta value.
                    if (deltaValue < 0)
                    {
                        cell.Style.BackColor = Color.IndianRed;
                    }
                    else
                    {
                        //cell.Style.BackColor = Color.LightGreen;
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label1.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            label1.BackColor = Color.LightGreen;
            textBox1.Focus();
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            label11.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void label11_Click(object sender, EventArgs e)
        {
            textBox11.Text = string.Empty;
            label11.BackColor = Color.LightGreen;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label2.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;
            label2.BackColor = Color.LightGreen;
            textBox2.Focus();
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
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void openWHexcelDB(string thePathToFile)
        {
            Process excel = new Process();
            excel.StartInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.exe";
            excel.StartInfo.Arguments = AddQuotesIfRequired(thePathToFile);
            excel.Start();
        }
        public string AddQuotesIfRequired(string path)
        {
            return !string.IsNullOrWhiteSpace(path) ?
                path.Contains(" ") && (!path.StartsWith("\"") && !path.EndsWith("\"")) ?
                    "\"" + path + "\"" : path :
                    string.Empty;
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            string fp = dataGridView1.Rows[rowindex].Cells["ProjectName"].Value.ToString();
            string fullpath = string.Empty;
            foreach (string path in listOfPaths)
            {
                foreach (string file in Directory.EnumerateFiles(path, "*.xlsm", SearchOption.AllDirectories))
                {
                    string Litem = Path.GetFileName(file);
                    if (Litem == fp)
                    {
                        string str = @file.ToString();
                        DialogResult result = MessageBox.Show("Open the file : " + str + " ?", "Open file", MessageBoxButtons.OKCancel);
                        if (result == DialogResult.OK)
                        {
                            openWHexcelDB(@str);
                        }
                        else
                        {
                        }
                    }
                }
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
        private void txtbQty_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void txtbQty_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private static void txtbColorGreenOnEnter(object sender)
        {
            TextBox? tb = (TextBox)sender;
            tb.BackColor = Color.LightGreen;
        }
        private static void txtbColorWhiteOnLeave(object sender)
        {
            TextBox? tb = sender as TextBox;
            tb.BackColor = Color.Gray;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtbQty.Focus();
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            filterViewAndJump2Qty(e);
        }
        private void filterViewAndJump2Qty(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
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
        private void textBox2_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            filterViewAndJump2Qty(e);
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void btnPrintSticker_Click(object sender, EventArgs e)
        {
            int outNumber;
            bool success = int.TryParse(txtbQty.Text, out outNumber);
            if (success && outNumber < 50001 && outNumber > 0)
            {
                WHitem w = new WHitem() { IPN = txtbIPN.Text, MFPN = txtbMFPN.Text, Description = txtbDescription.Text, Stock = outNumber, UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss") };
                printSticker(w);
                ResetAllTexboxes(textBox1);
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
                textBox1.Focus();
            }
            catch (Exception e)
            {
                MessageBox.Show("Sticker printing failed : " + e.Message);
            }
        }
        private void txtbQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnPrintSticker_Click(this, new EventArgs());
            }
        }
        private void label1_DoubleClick(object sender, EventArgs e)
        {
            ResetAllTexboxes(textBox1);
        }
        private void ResetAllTexboxes(TextBox txtb)
        {
            textBox1.Clear();
            textBox2.Clear();
            txtbQty.Clear();
            label1.BackColor = Color.LightGreen;
            label2.BackColor = Color.LightGreen;
            txtb.Focus();
        }
        private void label2_DoubleClick(object sender, EventArgs e)
        {
            ResetAllTexboxes(textBox2);
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            label9.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void label9_Click(object sender, EventArgs e)
        {
        }
        private void label9_DoubleClick(object sender, EventArgs e)
        {
            ResetAllTexboxes(textBox9);
        }
        private void textBox9_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox9_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stopWatch.Reset();
            ResetViews();
            startUpLogic(6);
            SetColumsOrder();
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stopWatch.Reset();
            ResetViews();
            startUpLogic(12);
            SetColumsOrder();
            textBox1.Focus();
        }


        private void textBox11_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string inputStr = textBox13.Text;
                string startStr = comboBox1.Text.ToString();
                string endStr = comboBox2.Text.ToString();

                int startIndex = inputStr.IndexOf(startStr);
                if (startIndex != -1)
                {
                    startIndex += startStr.Length;
                    int endIndex = inputStr.IndexOf(endStr, startIndex);
                    if (endIndex != -1)
                    {
                        string extractedStr = inputStr.Substring(startIndex, endIndex - startIndex);
                        textBox2.Text = extractedStr;
                        textBox2.Focus();
                        textBox2_KeyDown(sender, e);
                    }
                }

            }
        }

        private void textBox13_Click(object sender, EventArgs e)
        {
            textBox13.Clear();
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchbyMFPN = string.Empty;

                if (textBox12.Text.Contains("-") == true && textBox12.Text.Length > 6)
                {
                    //string[] theSplit = textBox13.Text.ToString().Split("-");
                    ////if (theSplit[0].Length == 3 || theSplit[0].Length >= 2 || textBox13.Text.Length > 5)
                    //if (theSplit[0].Length >= 3)
                    //{
                    //    searchbyMFPN = theSplit[1];

                    //}
                    string[] theSplit = textBox12.Text.Split("-");
                    if (theSplit.Length > 1)
                    {
                        searchbyMFPN = string.Join("-", theSplit, 1, theSplit.Length - 1);
                    }
                    else
                    {
                        searchbyMFPN = textBox12.Text;
                    }

                    textBox2.Text = searchbyMFPN;
                }
                else
                {

                }
                textBox2.Focus();
                textBox2_KeyDown(sender, e);
            }
        }

        private void textBox12_Click(object sender, EventArgs e)
        {
            textBox12.Clear();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Left mouse click logic
                txtbQty.Focus();
                // Additional left-click logic
            }
            else if (e.Button == MouseButtons.Right)
            {
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                string fp = dataGridView1.Rows[rowindex].Cells["ProjectName"].Value.ToString();
                string fullpath = string.Empty;
                foreach (string path in listOfPaths)
                {
                    foreach (string file in Directory.EnumerateFiles(path, "*.xlsm", SearchOption.AllDirectories))
                    {
                        string Litem = Path.GetFileName(file);
                        if (Litem == fp)
                        {
                            string str = @file.ToString();
                            DialogResult result = MessageBox.Show("Open the file : " + str + " ?", "Open file", MessageBoxButtons.OKCancel);
                            if (result == DialogResult.OK)
                            {
                                FrmBOM frmBOM = new FrmBOM();
                                frmBOM.ExternalLinktoFile(@str);
                                frmBOM.Show();
                                frmBOM.ReloadLogic();
                                //openWHexcelDB(@str);
                            }
                            else
                            {
                                //
                            }
                        }
                    }
                }
                // Right mouse click logic
                // Additional right-click logic
                //MessageBox.Show("Test");
            }
        }


    }
}
