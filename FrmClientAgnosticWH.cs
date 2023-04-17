using FastMember;
using Seagull.BarTender.Print;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using DataTable = System.Data.DataTable;
using RadioButton = System.Windows.Forms.RadioButton;
using TextBox = System.Windows.Forms.TextBox;
namespace WH_Panel
{
    public partial class FrmClientAgnosticWH : Form
    {
        public FrmClientAgnosticWH()
        {
            InitializeComponent();
            //comboBox3.SelectedIndex = 0;
            comboBox3.SelectedItem = "ROBOTRON";
            MasterReload(avlROBOTRON, stockROBOTRON);
        }

        public List<WHitem> avlItems = new List<WHitem>();
        public List<WHitem> stockItems = new List<WHitem>();
        public DataTable avlDTable = new DataTable();
        public DataTable stockDTable = new DataTable();
        public int countAVLItems = 0;
        public int countStockItems = 0;
        int iAVL = 0;
        int iStock = 0;
        private object cmd;
        public TextBox LastInputFromUser = new TextBox();
        public List<WHSettings> lstWHSettings = new List<WHSettings>() { };
        public WHSettings net = new WHSettings
        {
            ClientNameWH = "NETLINE",
            avlFP = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_AVL.xlsx",
            stockFP = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_AVL.xlsx"
        };
        public string avlNETLINE = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_AVL.xlsx";
        public string stockNETLINE = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_STOCK.xlsm";
        public string stockLeader_Tech = @"\\dbr1\Data\WareHouse\STOCK_CUSTOMERS\G.I.Leader_Tech\G.I.Leader_Tech_STOCK.xlsm";
        public string avlLeader_Tech = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_AVL.xlsm";
        public string avlVAYAR = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_AVL.xlsx";
        public string stockVAYAR = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_stock.xlsm";
        public string avlVALENS = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_AVL.xlsx";
        public string stockVALENS = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm";
        public string avlROBOTRON = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_AVL.xlsm";
        public string stockROBOTRON = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_STOCK.xlsm";
        public string avlFile;
        public string stockFile;

        private void MasterReload(string avlParam, string stockParam)
        {
            lblSendTo.Enabled = false;
            avlFile = avlParam;
            stockFile = stockParam;
            textBox8.ReadOnly = true;
            radioButton1.Checked = true;
            comboBox1.SelectedIndex = 1;
            button2_Click(this, new EventArgs());
            button3_Click(this, new EventArgs());
            comboBox2.Enabled = false;
            textBox1.Clear();
            textBox2.Clear();
            textBox6.Clear();
            textBox8.Clear();
            textBox9.Clear();
            label1.BackColor = Color.LightGreen;
            label2.BackColor = Color.LightGreen;
            LastInputFromUser = textBox1;
            LastInputFromUser.Focus();
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text == "ROBOTRON")
            {
                MasterReload(avlROBOTRON, stockROBOTRON);
            }
            else if (comboBox3.Text == "LEADER-TECH")
            {
                MasterReload(avlLeader_Tech, stockLeader_Tech);
            }
            else if (comboBox3.Text == "NETLINE")
            {
                MasterReload(avlNETLINE, stockNETLINE);
            }
            else if (comboBox3.Text == "VAYYAR")
            {
                MasterReload(avlVAYAR, stockVAYAR);
            }
            else if (comboBox3.Text == "VALENS")
            {
                MasterReload(avlVALENS, stockVALENS);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            label1.BackColor = Color.IndianRed;
            avlItems.Clear();
            avlDTable.Clear();
            countAVLItems = 0;
            iAVL = 0;
            label1.Text = "RELOAD AVL";
            DataLoaderAVL(avlFile, "AVL");
            PopulateAVLGridView();
        }
        private void DataLoaderAVL(string fp, string thesheetName)
        {
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=NO;IMEX=1\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("Select * from [" + thesheetName + "$]", conn);
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            WHitem abc = new WHitem
                            {
                                IPN = reader[1].ToString(),
                                Manufacturer = reader[2].ToString(),
                                MFPN = reader[3].ToString(),
                                Description = reader[4].ToString(),
                                Stock = 0,
                                UpdatedOn = string.Empty,
                                ReelBagTrayStick = string.Empty,
                                SourceRequester = string.Empty
                            };
                            if (iAVL > 0)
                            {
                                countAVLItems = iAVL;
                                label1.Text = "Rows in AVL: " + (countAVLItems).ToString();
                                if (countAVLItems % 1000 == 0)
                                {
                                    label1.Update();
                                }
                                avlItems.Add(abc);
                            }
                            iAVL++;
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
        private void PopulateAVLGridView()
        {
            IEnumerable<WHitem> data = avlItems;
            using (var reader = ObjectReader.Create(data))
            {
                avlDTable.Load(reader);
            }
            dataGridView2.DataSource = avlDTable;
            SetColumsOrder();
            label1.BackColor = Color.LightGreen;
        }
        private void SetColumsOrder()
        {
            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns["IPN"].DisplayIndex = 0;
            dataGridView2.Columns["Manufacturer"].DisplayIndex = 1;
            dataGridView2.Columns["MFPN"].DisplayIndex = 2;
            dataGridView2.Columns["Description"].DisplayIndex = 3;
        }
        private void FilterAVLDataGridView()
        {
            string searchByIPN = textBox1.Text;
            if (textBox1.Text.Contains("("))
            {
                searchByIPN = textBox1.Text.Substring(0, 15);
            }
            string searchbyMFPN = textBox2.Text;
            if (textBox2.Text.StartsWith("1P") == true)
            {
                searchbyMFPN = textBox2.Text.Substring(2);
            }
            else if (textBox2.Text.Contains("-") == true && textBox2.Text.Length > 6)
            {
                string[] theSplit = textBox2.Text.ToString().Split("-");
                if (theSplit[0].Length == 3 && theSplit.Length == 2)
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
            try
            {
                DataView dv = avlDTable.DefaultView;
                dv.RowFilter = "[IPN] LIKE '%" + searchByIPN.ToString() +
                    "%' AND [MFPN] LIKE '%" + searchbyMFPN.ToString() +
                    "%' AND [DESCRIPTION] LIKE '%" + txtbFiltAVLbyDESCR.Text.ToString() +
                    "%'";
                dataGridView2.DataSource = dv;
                SetColumsOrder();
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect search pattern, remove invalid character and try again !");
                throw;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label2.BackColor = Color.IndianRed;
            FilterAVLDataGridView();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label3.BackColor = Color.IndianRed;
            FilterAVLDataGridView();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox1.Focus();
            label2.BackColor = Color.LightGreen;
        }
        private void label3_Click(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;
            textBox2.Focus();
            label3.BackColor = Color.LightGreen;
        }
        private void label2_DoubleClick(object sender, EventArgs e)
        {
            AvlClearFilters();
        }
        private void label3_DoubleClick(object sender, EventArgs e)
        {
            AvlClearFilters();
        }
        private void AvlClearFilters()
        {
            textBox1.Text = string.Empty;
            label2.BackColor = Color.LightGreen;
            textBox2.Text = string.Empty;
            label3.BackColor = Color.LightGreen;
            txtbFiltAVLbyDESCR.Text = string.Empty;
            label16.BackColor = Color.LightGreen;
        }
        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells.Count > 0)
            {
                int rowindex = dataGridView2.CurrentCell.RowIndex;
                int columnindex = dataGridView2.CurrentCell.ColumnIndex;
                string cellValue = dataGridView2.Rows[rowindex].Cells[columnindex].Value.ToString();
                textBox3.Text = dataGridView2.Rows[rowindex].Cells["IPN"].Value.ToString();
                textBox7.Text = dataGridView2.Rows[rowindex].Cells["Manufacturer"].Value.ToString();
                textBox4.Text = dataGridView2.Rows[rowindex].Cells["MFPN"].Value.ToString();
                textBox5.Text = dataGridView2.Rows[rowindex].Cells["Description"].Value.ToString();
                textBox6.Clear();
                FilterStockDataGridView(textBox3.Text);
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            MoveByRadioColor(sender);
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == true)
            {
                textBox8.Text = string.Empty;
                textBox8.ReadOnly = true;
                textBox9.ReadOnly = true;
            }
        }
        private void MoveByRadioColor(object sender)
        {
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == true && rbtn.Name != "radioButton4")
            {
                btnMove.BackColor = Color.LightGreen;
            }
            else
            {
                btnMove.BackColor = Color.IndianRed;
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            MoveByRadioColor(sender);
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == true)
            {
                comboBox2.Enabled = true;
                textBox8.ReadOnly = false;
                textBox9.ReadOnly = true;
                textBox8.Focus();
            }
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            MoveByRadioColor(sender);
            RadioButton rbtn = sender as RadioButton;
            if (rbtn.Checked == true)
            {
                lblRWK.Text = "_RWK_";
                lblSendTo.Text = "_Sent to_";
                lblSendTo.Enabled = true;
                textBox8.ReadOnly = true;
                textBox9.ReadOnly = false;
                textBox9.Focus();
            }
            else
            {
                lblRWK.ResetText();
                lblSendTo.ResetText();
                lblSendTo.Enabled = false;
                textBox9.ReadOnly = true;
                textBox8.ReadOnly = false;
            }
        }
        private void btnMove_Click(object sender, EventArgs e)
        {
            int qty = 0;
            string sorce_req = string.Empty;
            if (radioButton1.Checked == true)
            {
                bool toPrintMFG = true;
                sorce_req = "MFG";
                if (textBox6.Text != string.Empty)
                {
                    try
                    {
                        string qInqty = (string)textBox6.Text;
                        string inQty = string.Empty;
                        if (qInqty.StartsWith("Q"))
                        {
                            inQty = qInqty.Substring(1);
                            //MessageBox.Show(inQty);
                            int outNumberq;
                            bool successq = int.TryParse(inQty, out outNumberq);
                            if (successq && outNumberq < 15001 && outNumberq > 0)
                            {
                                MoveIntoDATABASE(outNumberq, sorce_req, toPrintMFG);
                                FilterStockDataGridView(textBox10.Text);
                            }
                            else
                            {
                                MessageBox.Show("Input positive numeric values ONLY !");
                                textBox6.Text = string.Empty;
                                textBox6.Focus();
                            }
                        }
                        else
                        {
                            inQty = (string)textBox6.Text;
                            int outNumber;
                            bool success = int.TryParse(inQty, out outNumber);
                            if (success && outNumber < 15001 && outNumber > 0)
                            {
                                MoveIntoDATABASE(outNumber, sorce_req, toPrintMFG);
                                FilterStockDataGridView(textBox10.Text);
                            }
                            else
                            {
                                MessageBox.Show("Input positive numeric values ONLY !");
                                textBox6.Text = string.Empty;
                                textBox6.Focus();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    MessageBox.Show("Input Qty !");
                    textBox6.Text = string.Empty;
                    textBox6.Focus();
                }
            }
            else if (radioButton2.Checked == true)
            {
                bool toPrintGILT = true;
                if (textBox8.Text != string.Empty)
                {
                    sorce_req = comboBox2.Text + textBox8.Text;
                    if (textBox6.Text.ToString().StartsWith("Q"))
                    {
                        int outNumberq;
                        bool successq = int.TryParse(textBox6.Text.ToString().Substring(1), out outNumberq);
                        if (successq && outNumberq < 15001 && outNumberq > 0)
                        {
                            MoveIntoDATABASE(outNumberq, sorce_req, toPrintGILT);
                            FilterStockDataGridView(textBox10.Text);
                        }
                        else
                        {
                            MessageBox.Show("Input positive numeric values ONLY !");
                            textBox6.Text = string.Empty;
                            textBox6.Focus();
                        }
                    }
                    else
                    {
                        int outNumber;
                        bool success = int.TryParse(textBox6.Text.ToString(), out outNumber);
                        if (success && outNumber < 15001 && outNumber > 0)
                        {
                            MoveIntoDATABASE(outNumber, sorce_req, toPrintGILT);
                            FilterStockDataGridView(textBox10.Text);
                        }
                        else
                        {
                            MessageBox.Show("Input positive numeric values ONLY !");
                            textBox6.Text = string.Empty;
                            textBox6.Focus();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Input " + comboBox2.Text + "_XXX ID !");
                    textBox8.Focus();
                }
            }
            else if (radioButton4.Checked == true)
            {
                bool toPrintWO = false;
                if (textBox9.Text != string.Empty)
                {
                    if (textBox9.Text.Contains("_"))
                    {
                        string[] theWOsplit = textBox9.Text.Split("_");
                        sorce_req = theWOsplit[1] + "_" + theWOsplit[2];
                    }
                    else
                    {
                        sorce_req = textBox9.Text;
                    }
                    int outNumber;
                    bool success = int.TryParse(textBox6.Text, out outNumber);
                    if (success && outNumber < 15001 && outNumber > 0)
                    {
                        int negQty = outNumber * (-1);
                        MoveIntoDATABASE(negQty, sorce_req, toPrintWO);
                        FilterStockDataGridView(textBox10.Text);
                    }
                    else
                    {
                        MessageBox.Show("Input Qty !");
                        textBox6.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("INPUT WO !");
                    textBox9.Focus();
                }
            }
        }
        private void ComeBackFromPrint()
        {
            Microsoft.VisualBasic.Interaction.AppActivate("Imperium Tabula Principalis");
            LastInputFromUser.Focus();
        }
        private void MoveIntoDATABASE(int qty, string sorce_req, bool toPrintOrNotToPrint)
        {
            bool toPrint = toPrintOrNotToPrint;
            WHitem inputWHitem = new WHitem
            {
                IPN = textBox3.Text,
                Manufacturer = textBox7.Text,
                MFPN = textBox4.Text,
                Description = textBox5.Text,
                Stock = qty,
                UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"), //tt
                ReelBagTrayStick = comboBox1.Text,
                SourceRequester = sorce_req
            };
            DataInserter(stockFile, "STOCK", inputWHitem, toPrint);
            stockItems.Add(inputWHitem);
            textBox10.Text = inputWHitem.IPN;
            textBox10.BackColor = Color.LightGreen;
            PopulateStockView();
        }
        private void DataInserter(string fp, string thesheetName, WHitem wHitem, bool toPrintOrNotToPrint)
        {
            bool toPrint = toPrintOrNotToPrint;
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
                textBox6.Clear();
                LastInputFromUser.Text = string.Empty;
                label2.BackColor = Color.LightGreen;
                label3.BackColor = Color.LightGreen;
                LastInputFromUser.Focus();
                if (toPrintOrNotToPrint)
                {
                    printSticker(wHitem);
                }
                if (radioButton4.Checked == true)
                {
                    AutoClosingMessageBox.Show(wHitem.IPN + " MOVED to " + textBox9.Text.ToString(), " Item added to " + textBox9.Text.ToString(), 1000);
                }
                else
                {
                    AutoClosingMessageBox.Show(wHitem.Stock.ToString() + " PCS of " + wHitem.IPN + " in a " + wHitem.ReelBagTrayStick + " MOVED to DB ", "Item added to DB", 2000);
                }
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
        private static void printStickerAPI(WHitem wHitem)
        {
            try
            {
                using (Engine btengine = new Engine(true))
                {
                    //Mutex m = new Mutex(false, "MyMutex");
                    btengine.Start();
                    btengine.Window.Visible = true;
                    Messages messages = null;
                    LabelFormatDocument labelFormat =
                        btengine.Documents.Open(@"C:\1\PN_STICKER_2022.btw");
                    labelFormat.SubStrings["Date"].Value = wHitem.UpdatedOn;
                    labelFormat.SubStrings["DESC"].Value = wHitem.Description;
                    labelFormat.SubStrings["MFPN"].Value = wHitem.MFPN;
                    labelFormat.SubStrings["PN"].Value = wHitem.IPN;
                    labelFormat.SubStrings["QTY"].Value = wHitem.Stock.ToString();
                    labelFormat.Print();
                    btengine.Stop();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
        private void textBox8_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LastInputFromUser.Focus();
            }
        }
        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LastInputFromUser.Focus();
            }
        }
        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnMove_Click(this, new EventArgs());
            }
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox6.Focus();
            if (dataGridView2.SelectedCells.Count > 0)
            {
                int rowindex = dataGridView2.CurrentCell.RowIndex;
                int columnindex = dataGridView2.CurrentCell.ColumnIndex;
                string cellValue = dataGridView2.Rows[rowindex].Cells[dataGridView2.Columns["IPN"].Index].Value.ToString();
                FilterStockDataGridView(cellValue);
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            LastInputFromUser = (TextBox)sender;
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView2.Rows.Count == 1)
                {
                    textBox6.Focus();
                    return;
                }
                dataGridView2.Focus();
            }

        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            LastInputFromUser = (TextBox)sender;
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView2.Rows.Count == 1)
                {
                    textBox6.Focus();
                    return;
                }
                dataGridView2.Focus();
            }

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
                                countStockItems = iStock;
                                button3.Text = "Rows in STOCK: " + (countStockItems).ToString();
                                if (countStockItems % 1000 == 0)
                                {
                                    button3.Update();
                                }
                                stockItems.Add(abc);
                                iStock++;
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
        private void button3_Click(object sender, EventArgs e)
        {
            button3.BackColor = Color.IndianRed;
            stockItems.Clear();
            stockDTable.Clear();
            countStockItems = 0;
            iStock = 0;
            dataGridView1.DataSource = null;
            button3.Text = "LOAD STOCK";
            button3.Update();
            StockViewDataLoader(stockFile, "STOCK");
            PopulateStockView();
        }
        private void PopulateStockView()
        {
            IEnumerable<WHitem> data = stockItems;
            stockDTable.Clear();
            using (var reader = ObjectReader.Create(data))
            {
                stockDTable.Load(reader);
            }
            dataGridView1.DataSource = stockDTable;
            button3.BackColor = Color.LightGreen;
            SetSTOCKiewColumsOrder();
        }
        private void SetSTOCKiewColumsOrder()
        {
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["IPN"].DisplayIndex = 0;
            dataGridView1.Columns["Manufacturer"].DisplayIndex = 1;
            dataGridView1.Columns["MFPN"].DisplayIndex = 2;
            dataGridView1.Columns["Description"].DisplayIndex = 3;
            dataGridView1.Columns["Stock"].DisplayIndex = 4;
            dataGridView1.Columns["UpdatedOn"].DisplayIndex = 5;
            dataGridView1.Columns["ReelBagTrayStick"].DisplayIndex = 6;
            dataGridView1.Columns["SourceRequester"].DisplayIndex = 7;
            dataGridView1.Sort(dataGridView1.Columns["UpdatedOn"], ListSortDirection.Descending);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }
        private void FilterStockDataGridView(string IPN)
        {
            if (textBox3.Text != string.Empty)
            {
                try
                {
                    int balance = 0;
                    DataView dv = stockDTable.DefaultView;
                    dv.RowFilter = "[IPN] LIKE '%" + IPN +
                        "%'";
                    dataGridView1.DataSource = dv;
                    dataGridView1.Update();
                    SetSTOCKiewColumsOrder();
                    List<int> qtys = new List<int>();
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        int qty = 0;
                        int result;
                        bool prs = int.TryParse(dataGridView1.Rows[i].Cells[dataGridView1.Columns["Stock"].Index].Value.ToString(), out result);
                        if (prs)
                        {
                            qty = result;
                        }
                        qtys.Add(qty);
                    }
                    foreach (int i in qtys)
                    {
                        balance += i;
                    }
                    label15.Text = "BALANCE: " + balance;
                    if (balance > 0)
                        label15.BackColor = Color.LightGreen;
                    else if (balance == 0)
                        label15.BackColor = Color.IndianRed;
                    label15.Update();
                }
                catch (Exception)
                {
                    MessageBox.Show("Incorrect search pattern, remove invalid character and try again !");
                    throw;
                }
            }
            else
            {
                DataView dv = stockDTable.DefaultView;
                dataGridView1.DataSource = dv;
                dataGridView1.Update();
                SetSTOCKiewColumsOrder();
            }
        }
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.SelectedCells.Count > 0)
            {
                int rowindex = dataGridView2.CurrentCell.RowIndex;
                int columnindex = dataGridView2.CurrentCell.ColumnIndex;
                string cellValue = dataGridView2.Rows[rowindex].Cells[dataGridView2.Columns["IPN"].Index].Value.ToString();
                FilterStockDataGridView(cellValue);
            }
        }
        private void FrmAddItemsToDB_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            List<WHitem> inWHstock = new List<WHitem>();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                int res = 0;
                int toStk;
                bool stk = int.TryParse(dataGridView1.Rows[i].Cells[dataGridView1.Columns["Stock"].Index].Value.ToString(), out res);
                if (stk)
                {
                    toStk = res;
                }
                else
                {
                    toStk = 0;
                }
                WHitem wHitemABC = new WHitem()
                {
                    IPN = dataGridView1.Rows[i].Cells[dataGridView1.Columns["IPN"].Index].Value.ToString(),
                    Manufacturer = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Manufacturer"].Index].Value.ToString(),
                    MFPN = dataGridView1.Rows[i].Cells[dataGridView1.Columns["MFPN"].Index].Value.ToString(),
                    Description = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Description"].Index].Value.ToString(),
                    Stock = toStk,
                    UpdatedOn = dataGridView1.Rows[i].Cells[dataGridView1.Columns["UpdatedOn"].Index].Value.ToString(),
                    ReelBagTrayStick = dataGridView1.Rows[i].Cells[dataGridView1.Columns["ReelBagTrayStick"].Index].Value.ToString(),
                    SourceRequester = dataGridView1.Rows[i].Cells[dataGridView1.Columns["SourceRequester"].Index].Value.ToString()
                };
                inWHstock.Add(wHitemABC);
            }
            List<WHitem> negatiVEQTYs = new List<WHitem>();
            for (int i = 0; i < inWHstock.Count; i++)
            {
                if (inWHstock[i].Stock < 0)
                {
                    negatiVEQTYs.Add(inWHstock[i]);
                }
            }
            List<WHitem> positiveInWH = new List<WHitem>();
            for (int k = 0; k < inWHstock.Count; k++)
            {
                if (inWHstock[k].Stock > 0)
                {
                    positiveInWH.Add(inWHstock[k]);
                }
            }
            for (int i = 0; i < negatiVEQTYs.Count; i++)
            {
                for (int j = 0; j < positiveInWH.Count; j++)
                {
                    if (Math.Abs(negatiVEQTYs[i].Stock) == positiveInWH[j].Stock)
                    {
                        positiveInWH.Remove((WHitem)positiveInWH[j]);
                        break;
                    }
                }
            }
            IEnumerable<WHitem> WHdata = positiveInWH;
            DataTable INWH = new DataTable();
            using (var reader = ObjectReader.Create(WHdata))
            {
                INWH.Load(reader);
            }
            DataView dv = INWH.DefaultView;
            dataGridView1.DataSource = dv;
            dataGridView1.Update();
            SetSTOCKiewColumsOrder();
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
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
        private void textBox1_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private static void txtbColorWhiteOnLeave(object sender)
        {
            TextBox? tb = sender as TextBox;
            tb.BackColor = Color.White;
        }
        private void textBox6_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
            textBox10.Text = textBox3.Text;
            textBox10.BackColor = Color.IndianRed;
        }
        private void textBox6_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void textBox8_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox8_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void txtbFiltAVLbyDESCR_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void textBox9_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void txtbFiltAVLbyDESCR_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox9_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void FrmAddItemsToNetline_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            WHitem wHitemABCD = new WHitem()
            {
                IPN = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["IPN"].Index].Value.ToString(),
                Manufacturer = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Manufacturer"].Index].Value.ToString(),
                MFPN = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["MFPN"].Index].Value.ToString(),
                Description = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Description"].Index].Value.ToString(),
                Stock = int.Parse(dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Stock"].Index].Value.ToString()),
                UpdatedOn = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["UpdatedOn"].Index].Value.ToString(),
                ReelBagTrayStick = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["ReelBagTrayStick"].Index].Value.ToString(),
                SourceRequester = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["SourceRequester"].Index].Value.ToString()
            };
            if (wHitemABCD.Stock > 0)
            {
                printSticker(wHitemABCD);
            }
            else
            {
                MessageBox.Show("Can print only positive quantites !");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            List<WHitem> inWHstock = new List<WHitem>();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                WHitem wHitemABC = new WHitem()
                {
                    IPN = dataGridView1.Rows[i].Cells[dataGridView1.Columns["IPN"].Index].Value.ToString(),
                    Manufacturer = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Manufacturer"].Index].Value.ToString(),
                    MFPN = dataGridView1.Rows[i].Cells[dataGridView1.Columns["MFPN"].Index].Value.ToString(),
                    Description = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Description"].Index].Value.ToString(),
                    Stock = int.Parse(dataGridView1.Rows[i].Cells[dataGridView1.Columns["Stock"].Index].Value.ToString()),
                    UpdatedOn = dataGridView1.Rows[i].Cells[dataGridView1.Columns["UpdatedOn"].Index].Value.ToString(),
                    ReelBagTrayStick = dataGridView1.Rows[i].Cells[dataGridView1.Columns["ReelBagTrayStick"].Index].Value.ToString(),
                    SourceRequester = dataGridView1.Rows[i].Cells[dataGridView1.Columns["SourceRequester"].Index].Value.ToString()
                };
                inWHstock.Add(wHitemABC);
            }
            List<WHitem> negatiVEQTYs = new List<WHitem>();
            for (int i = 0; i < inWHstock.Count; i++)
            {
                if (inWHstock[i].Stock < 0)
                {
                    negatiVEQTYs.Add(inWHstock[i]);
                }
            }
            List<WHitem> positiveInWH = new List<WHitem>();
            for (int k = 0; k < inWHstock.Count; k++)
            {
                if (inWHstock[k].Stock > 0)
                {
                    positiveInWH.Add(inWHstock[k]);
                }
            }
            for (int i = 0; i < negatiVEQTYs.Count; i++)
            {
                for (int j = 0; j < positiveInWH.Count; j++)
                {
                    if (Math.Abs(negatiVEQTYs[i].Stock) == positiveInWH[j].Stock)
                    {
                        positiveInWH.Remove((WHitem)positiveInWH[j]);
                        break;
                    }
                }
            }
            IEnumerable<WHitem> WHdata = positiveInWH;
            DataTable INWH = new DataTable();
            using (var reader = ObjectReader.Create(WHdata))
            {
                INWH.Load(reader);
            }
            DataView dv = INWH.DefaultView;
            dataGridView1.DataSource = dv;
            dataGridView1.Update();
            SetSTOCKiewColumsOrder();
        }
        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (comboBox2.SelectedIndex != (-1))
                {
                    LastInputFromUser.Focus();
                }
                else
                {
                    MessageBox.Show("SELECT GILT/WS/WR/SH/IF source !");
                    comboBox2.DroppedDown = true;
                }
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox8.Focus();
        }
        private void FrmClientAgnosticWH_Load(object sender, EventArgs e)
        {
        }
        private void txtbFiltAVLbyDESCR_TextChanged(object sender, EventArgs e)
        {
            label16.BackColor = Color.IndianRed;
            FilterAVLDataGridView();
        }
        private void label16_Click(object sender, EventArgs e)
        {
            txtbFiltAVLbyDESCR.Text = string.Empty;
            txtbFiltAVLbyDESCR.Focus();
            label16.BackColor = Color.LightGreen;
        }
        private void label16_DoubleClick(object sender, EventArgs e)
        {
            AvlClearFilters();
        }
        private void txtbFiltAVLbyDESCR_KeyDown(object sender, KeyEventArgs e)
        {
            LastInputFromUser = (TextBox)sender;
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView2.Rows.Count == 1)
                {
                    textBox6.Focus();
                }
                else
                {
                    dataGridView2.Focus();
                }
            }
        }
        private void lblSendTo_Click(object sender, EventArgs e)
        {
            lblSendTo.Text += comboBox3.Text.ToString() + " " + DateTime.Now.ToString("yyyy-MM-dd");
            textBox9.Text = lblSendTo.Text;
        }
        private void textBox9_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LastInputFromUser.Text = string.Empty;
                LastInputFromUser.Focus();
            }
        }
        private void lblRWK_Click(object sender, EventArgs e)
        {
            lblRWK.Text += DateTime.Now.ToString("yyyy-MM-dd");
            textBox9.Text = lblRWK.Text;
        }
    }
}
