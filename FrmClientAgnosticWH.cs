using FastMember;
using Seagull.BarTender.Print;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text;
using OfficeOpenXml;
using System.IO;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Button = System.Windows.Forms.Button;
using ComboBox = System.Windows.Forms.ComboBox;
using DataTable = System.Data.DataTable;
using RadioButton = System.Windows.Forms.RadioButton;
using TextBox = System.Windows.Forms.TextBox;
using Newtonsoft.Json;
using Microsoft.Office.Interop.Excel;
using Label = System.Windows.Forms.Label;
using GroupBox = System.Windows.Forms.GroupBox;
using Application = System.Windows.Forms.Application;
using Seagull.Framework.Extensions;
namespace WH_Panel
{
    public partial class FrmClientAgnosticWH : Form
    {
        public FrmClientAgnosticWH()
        {
            InitializeComponent();
            UpdateControlColors(this);
        }
        public void InitializeGlobalWarehouses(List<ClientWarehouse> warehousesFromTheMain)
        {
            warehouses = warehousesFromTheMain;
            // Ordering the warehouses list by clName
            warehouses = warehouses.OrderBy(warehouse => warehouse.clName).ToList();
            // Adding clNames to comboBox4
            foreach (ClientWarehouse warehouse in warehouses)
            {
                comboBox3.Items.Add(warehouse.clName);
                GroupBox groupBox = new GroupBox();
                groupBox.Name = warehouse.clName;
                groupBox.Text = warehouse.clName;
                groupBox.Width = 150;
                groupBox.Height = 130;
                Button stockButton = new Button();
                //stockButton.Text = "Open Stock File";
                stockButton.Click += (sender, e) => AuthorizedExcelFileOpening(warehouse.clStockFile);
                stockButton.Top = 15; // Adjust the top position as needed
                stockButton.Left = 5; // Adjust the left position as needed
                //stockButton.Width = 66;
                if (File.Exists(warehouse.clLogo))
                {
                    string logoFilePath = Path.Combine("dbr1", "WareHouse", "STOCK_CUSTOMERS", warehouse.clName, warehouse.clLogo);
                    stockButton.BackgroundImage = Image.FromFile(logoFilePath);
                    stockButton.BackgroundImageLayout = ImageLayout.Stretch;
                    stockButton.Width = 140;
                    stockButton.Height = 50;
                }
                Button avlButton = new Button();
                //avlButton.Text = "Open AVL File";
                avlButton.Click += (sender, e) => AuthorizedExcelFileOpening(warehouse.clAvlFile);
                avlButton.Top = stockButton.Bottom + 2; // Adjust the top position as needed
                avlButton.Left = 5;
                string avlImagePath = Path.Combine(Application.StartupPath, "Resources", "AVL.png");
                if (File.Exists(avlImagePath))
                {
                    avlButton.BackgroundImage = Image.FromFile(avlImagePath);
                    avlButton.BackgroundImageLayout = ImageLayout.Stretch;
                    avlButton.Width = 140; // Adjust the button width as needed
                    avlButton.Height = 50;
                }
                flowLayoutPanel1.Controls.Add(groupBox);
                groupBox.Controls.Add(stockButton);
                groupBox.Controls.Add(avlButton);
            }
            comboBox6.Enabled = false;
            button23.Enabled = false;
            comboBox3.SelectedItem = "ROBOTRON";
        }
        List<ClientWarehouse> warehouses { get; set; }
        public WHitem wHitemToSplit = new WHitem();
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
        public string avlFile;
        public string stockFile;
        public void SetComboBoxText(string text)
        {
            comboBox3.Text = text;
        }
        public void MasterReload(string avlParam, string stockParam)
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
            button22.MouseClick += new MouseEventHandler(button22_MouseClick);
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ClientWarehouse w in warehouses)
            {
                if (comboBox3.Text == w.clName)
                {
                    // Set the image in PictureBox based on the selected warehouse
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(w.clLogo);
                    avlFile = w.clAvlFile;
                    stockFile = w.clStockFile;
                    MasterReload(avlFile, stockFile);
                    //if (!string.IsNullOrEmpty(w.claccDBfile))
                    //{
                    //    MessageBox.Show(w.claccDBfile);
                    //}
                }
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
                comboBox6.Enabled = true;
                comboBox6.SelectedIndex = 0;
                lblRWK.Text = "_requested by " + comboBox6.SelectedText;
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
                comboBox6.Enabled = false;
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
                            if (successq && outNumberq < 50001 && outNumberq > 0)
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
                            if (success && outNumber < 50001 && outNumber > 0)
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
                        textBox6.Text = string.Empty;
                        textBox6.Focus();
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
                        if (successq && outNumberq < 50001 && outNumberq > 0)
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
                    else if (textBox6.Text.ToString().Contains(","))
                    {
                        ;
                        int outNumberq;
                        bool successq = int.TryParse(textBox6.Text.Replace(",", ""), out outNumberq);
                        if (successq && outNumberq < 50001 && outNumberq > 0)
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
                        if (success && outNumber < 50001 && outNumber > 0)
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
                    if (success && outNumber < 50001 && outNumber > 0)
                    {
                        int negQty = outNumber * (-1);
                        MoveIntoDATABASE(negQty, sorce_req, toPrintWO);
                        FilterStockDataGridView(textBox10.Text);
                    }
                    else
                    {
                        MessageBox.Show("Input Qty !");
                        textBox6.Text = string.Empty;
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
        private void DataInserterSplitter(string fp, string thesheetName, WHitem wHitem, bool toPrintOrNotToPrint)
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
                if (toPrintOrNotToPrint)
                {
                    printStickerSplitter(wHitem);
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
                //string fp = @"C:\\Users\\lgt\\Desktop";
                string thesheetName = "Sheet1";
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0;HDR=YES;IMEX=0\"";
                OleDbConnection conn = new OleDbConnection(constr);
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE [" + thesheetName + "$] SET PN = @PN, MFPN = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, UPDATEDON = @UPDATEDON";
                //string fp = $@"C:\Users\{userName}\Desktop\Print_Stickers.xlsx";
                //string thesheetName = "Sheet1$"; // Note the '$' at the end to reference the sheet directly
                //string constr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fp};Extended Properties=\"Excel 12.0;HDR=YES;IMEX=0\"";
                //OleDbConnection conn = new OleDbConnection(constr);
                //OleDbCommand cmd = new OleDbCommand();
                //cmd.Connection = conn;
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = $"UPDATE [{thesheetName}] SET PN = @PN, MFPN = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, UPDATEDON = @UPDATEDON";
                //// Rest of your code remains unchanged
                cmd.Parameters.AddWithValue("@PN", wHitem.IPN);
                cmd.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                cmd.Parameters.AddWithValue("@ItemDesc", wHitem.Description);
                cmd.Parameters.AddWithValue("@QTY", wHitem.Stock);
                cmd.Parameters.AddWithValue("@UPDATEDON", wHitem.UpdatedOn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                //string userName = Environment.UserName;
                //string fp = $@"C:\Users\{userName}\Desktop\Print_Stickers.xlsx";
                //string constr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fp};Extended Properties='Excel 12.0 Macro;HDR=YES;IMEX=0'";
                //OleDbConnection conn = new OleDbConnection(constr);
                //conn.Open();
                //DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //conn.Close();
                //if (dtSheet != null && dtSheet.Rows.Count > 0)
                //{
                //    string firstSheetName = dtSheet.Rows[0]["TABLE_NAME"].ToString().Trim('\'');
                //    conn.Open();
                //    OleDbCommand cmd = new OleDbCommand();
                //    cmd.Connection = conn;
                //    cmd.CommandType = CommandType.Text;
                //    cmd.CommandText = $"UPDATE [{firstSheetName}] SET PN = @PN, MFPN = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, UPDATEDON = @UPDATEDON";
                //    cmd.Parameters.AddWithValue("@PN", wHitem.IPN);
                //    cmd.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                //    cmd.Parameters.AddWithValue("@ItemDesc", wHitem.Description);
                //    cmd.Parameters.AddWithValue("@QTY", wHitem.Stock);
                //    cmd.Parameters.AddWithValue("@UPDATEDON", wHitem.UpdatedOn);
                //    cmd.ExecuteNonQuery();
                //    conn.Close();
                //}
                //else
                //{
                //    MessageBox.Show("Print error");
                //    // Handle the case when there are no sheets in the Excel file.
                //}
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
        private void printStickerSplitter(WHitem wHitem)
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
                cmd.CommandText = "UPDATE [" + thesheetName + "$] SET PN = @PN, MFpn = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, UPDATEDON = @UPDATEDON";
                cmd.Parameters.AddWithValue("@PN", wHitem.IPN);
                cmd.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                cmd.Parameters.AddWithValue("@ItemDesc", wHitem.Description);
                cmd.Parameters.AddWithValue("@QTY", wHitem.Stock);
                cmd.Parameters.AddWithValue("@UPDATEDON", wHitem.UpdatedOn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                Microsoft.VisualBasic.Interaction.AppActivate("PN_STICKER_2022.btw - BarTender Designer");
                Thread.Sleep(500);
                SendKeys.SendWait("^p");
                Thread.Sleep(500);
                SendKeys.SendWait("{Enter}");
                Thread.Sleep(500);
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
            label2.BackColor = Color.LightGreen;
        }
        private static void txtbColorGreenOnEnter(object sender)
        {
            TextBox? tb = (TextBox)sender;
            tb.BackColor = Color.LightGreen;
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
            label2.BackColor = Color.Gray;
        }
        private static void txtbColorWhiteOnLeave(object sender)
        {
            TextBox? tb = sender as TextBox;
            tb.BackColor = Color.LightGray;
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
            label3.BackColor = Color.LightGreen;
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
            label3.BackColor = Color.Gray;
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
            label16.BackColor = Color.Gray;
        }
        private void textBox9_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void txtbFiltAVLbyDESCR_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
            label16.BackColor = Color.LightGreen;
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
            if (comboBox2.SelectedItem.ToString() == "FTK2400")
            {
                string todayMonth = DateTime.Now.ToString("MM");
                string todayDay = DateTime.Now.ToString("dd");
                textBox8.Text = todayMonth + todayDay;
            }
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
            lblSendTo.Text = string.Empty;
            lblSendTo.Text += "sent to ";
            lblSendTo.Text += comboBox3.Text.ToString() + " on " + DateTime.Now.ToString("yyyy-MM-dd");
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
            lblRWK.Text = string.Empty;
            lblRWK.Text += "requested by ";
            textBox9.Text = lblRWK.Text + comboBox6.SelectedItem.ToString() + " on " + DateTime.Now.ToString("yyyy-MM-dd");
        }
        private void AuthorizedExcelFileOpening(string fp)
        {
            if (Environment.UserName == "lgt" || Environment.UserName == "rbtwh" || Environment.UserName == "rbtwh2")
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
        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string inputStr = textBox11.Text;
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
                        textBox2.Focus();
                        LastInputFromUser = textBox11;
                    }
                }
            }
        }
        private void textBox11_Click(object sender, EventArgs e)
        {
            textBox11.Clear();
        }
        private void button22_Click(object sender, EventArgs e)
        {
        }
        private void button22_MouseClick(object sender, MouseEventArgs e)
        {
        }
        private void button22_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //MessageBox.Show("left");
                GenerateHTML();
            }
            else if (e.Button == MouseButtons.Right)
            {
                //MessageBox.Show("right");
                GenerateHTMLwareHouseBalance();
            }
        }
        public List<string> GenerateRandomColors(int count)
        {
            var random = new Random();
            var colors = new List<string>();
            for (int i = 0; i < count; i++)
            {
                colors.Add(String.Format("'rgba({0}, {1}, {2}, {3})'",
                    random.Next(0, 255),
                    random.Next(0, 255),
                    random.Next(0, 255),
                    0.2
                ));
            }
            return colors;
        }
        private void GenerateHTMLwareHouseBalance()
        {
            string fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = @"\\dbr1\Data\WareHouse\2024\WHsearcher\" + fileTimeStamp + "_" + comboBox3.SelectedItem.ToString() + "_wh_Balance" + ".html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='background-color: gray;'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<script src='https://cdn.jsdelivr.net/npm/chart.js'></script>");
                writer.WriteLine("<script src='https://cdnjs.cloudflare.com/ajax/libs/json2/20210202/json2.js'></script>");
                writer.WriteLine("<script src='https://cdnjs.cloudflare.com/ajax/libs/newtonsoft.json/13.0.1/json.net.min.js'></script>");
                writer.WriteLine("<h1 id='stickyHeader' style='position: sticky; top: 0; background-color: lightgreen; text-align: center;'>");
                writer.WriteLine($"{fileTimeStamp}_{comboBox3.SelectedItem}");
                writer.WriteLine("<br>");
                writer.WriteLine("<input type='text' id='filterInput' onkeyup='filterItems()' placeholder='Filter IPNs...' style='text-align:center;margin: 10px;'>");
                writer.WriteLine("<button onclick='clearFilter()' style='margin: 10px;'>CLEAR</button>");
                writer.WriteLine("</h1>");
                writer.WriteLine("<style>");
                writer.WriteLine("<button onclick='openAllAccordions()' style='margin: 10px;'>Open All Accordions</button>");
                writer.WriteLine(".accordion {");
                writer.WriteLine("background-color: gray;");
                writer.WriteLine("color: #444;");
                writer.WriteLine("cursor: pointer;");
                writer.WriteLine("padding: 18px;");
                writer.WriteLine("width: 100%;");
                writer.WriteLine("text-align: center;");
                writer.WriteLine("border: 1px;");
                writer.WriteLine("outline: none;");
                writer.WriteLine("transition: 0.4s;");
                writer.WriteLine("}");
                writer.WriteLine(".active, .accordion:hover {");
                writer.WriteLine("background-color: #ccc;");
                writer.WriteLine("}");
                writer.WriteLine(".panel {");
                writer.WriteLine("padding: 0 18px;");
                writer.WriteLine("display: none;");
                writer.WriteLine("background-color: gray;");
                writer.WriteLine("overflow: hidden;");
                writer.WriteLine("}");
                writer.WriteLine("</style>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                //writer.WriteLine("<canvas id='myChart' width='200' height='200'></canvas>");
                //writer.WriteLine("<canvas id='myChart' width='200' height='300' style='max-height: 300px;'></canvas>");
                writer.WriteLine("<div style=\"height: 300px; display: flex;\">");
                writer.WriteLine("<div style =\"flex: 1; text-align: right;\">");
                writer.WriteLine("<canvas id='myChart1' width='100' height='300' style='max-height: 300px; flex: 1;'></canvas>");
                writer.WriteLine("</div>");
                writer.WriteLine("<div style =\"flex: 1; text-align: left;\">");
                writer.WriteLine("<canvas id='myChart2' width='100' height='300' style='max-height: 300px; flex: 2;'></canvas>");
                writer.WriteLine("</div>");
                writer.WriteLine("</div>");
                var orderedStockItems = stockItems.OrderBy(item => item.IPN).ToList();
                // Group items by IPN and calculate the total stock for each unique IPN
                var groupedItems = orderedStockItems.GroupBy(item => item.IPN)
                                            .Select(group => new
                                            {
                                                IPN = group.Key,
                                                TotalStock = group.Sum(item => item.Stock),
                                                Items = group.Select(item => new
                                                {
                                                    item.Manufacturer,
                                                    item.MFPN,
                                                    item.Description,
                                                    item.Stock,
                                                    item.UpdatedOn,
                                                    item.ReelBagTrayStick,
                                                    item.SourceRequester
                                                })
                                            });
                var groupedPositiveBalanceByReelBagTrayStick = orderedStockItems
    .Where(item => item.Stock > 0 && !orderedStockItems.Any(otherItem =>
        otherItem.IPN == item.IPN && otherItem.Stock == -item.Stock))
    .GroupBy(item => new { item.ReelBagTrayStick })
    .Select(group => new
    {
        ReelBagTrayStick = group.Key.ReelBagTrayStick,
        Count = group.Count()
    });
                // Generate the chart data based on the grouped data
                var labels = groupedPositiveBalanceByReelBagTrayStick.Select(item => item.ReelBagTrayStick).ToList();
                var data = groupedPositiveBalanceByReelBagTrayStick.Select(item => item.Count).ToList();
                // Generate random colors for the chart
                var colors = new List<string>();
                var random = new Random();
                for (int i = 0; i < labels.Count; i++)
                {
                    var color = String.Format("'rgba({0}, {1}, {2}, {3})'",
                        random.Next(0, 255),
                        random.Next(0, 255),
                        random.Next(0, 255),
                        0.2
                    );
                    colors.Add(color);
                }
                // Write the script to generate the chart
                writer.WriteLine("<script>");
                writer.WriteLine("var ctx1 = document.getElementById('myChart1').getContext('2d');");
                writer.WriteLine("var myChart1 = new Chart(ctx1, {");
                writer.WriteLine("type: 'doughnut',");
                writer.WriteLine("data: {");
                writer.WriteLine("labels: " + JsonConvert.SerializeObject(labels) + ",");
                writer.WriteLine("datasets: [{");
                writer.WriteLine("label: 'Count:',");
                writer.WriteLine("data: " + JsonConvert.SerializeObject(data) + ",");
                writer.WriteLine("backgroundColor: [");
                // Adjust the alpha value to make the colors more vibrant
                writer.WriteLine("'rgba(255, 99, 132, 1)',");
                writer.WriteLine("'rgba(54, 162, 235, 1)',");
                writer.WriteLine("'rgba(255, 206, 86, 1)',");
                writer.WriteLine("'rgba(75, 192, 192, 1)',");
                writer.WriteLine("'rgba(153, 102, 255, 1)',");
                writer.WriteLine("'rgba(255, 159, 64, 1)'");
                writer.WriteLine("],");
                writer.WriteLine("borderColor: [");
                // Adjust the alpha value for border colors if needed
                writer.WriteLine("'rgba(255, 99, 132, 1)',");
                writer.WriteLine("'rgba(54, 162, 235, 1)',");
                writer.WriteLine("'rgba(255, 206, 86, 1)',");
                writer.WriteLine("'rgba(75, 192, 192, 1)',");
                writer.WriteLine("'rgba(153, 102, 255, 1)',");
                writer.WriteLine("'rgba(255, 159, 64, 1)'");
                writer.WriteLine("],");
                writer.WriteLine("borderWidth: 1");
                writer.WriteLine("}]");
                writer.WriteLine("},");
                //writer.WriteLine("options: {}");
                writer.WriteLine("options: {");
                writer.WriteLine("    indexAxis: 'y',");
                writer.WriteLine("    scales: {");
                writer.WriteLine("        y: {");
                writer.WriteLine("            stacked: true,");
                writer.WriteLine("            beginAtZero: true,");
                writer.WriteLine("            ticks: {");
                writer.WriteLine("                color: 'white'");
                writer.WriteLine("            }");
                writer.WriteLine("        },");
                writer.WriteLine("        x: {");
                writer.WriteLine("            stacked: true,");
                writer.WriteLine("            ticks: {");
                writer.WriteLine("                color: 'white'");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.WriteLine("});");
                writer.WriteLine("</script>");
                writer.WriteLine("<script>");
                writer.WriteLine("var ctx2 = document.getElementById('myChart2').getContext('2d');");
                writer.WriteLine("var myChart2 = new Chart(ctx2, {");
                writer.WriteLine("type: 'bar',");
                writer.WriteLine("data: {");
                writer.WriteLine("labels: " + JsonConvert.SerializeObject(labels) + ",");
                writer.WriteLine("datasets: [{");
                writer.WriteLine("label: 'Count:',");
                writer.WriteLine("data: " + JsonConvert.SerializeObject(data) + ",");
                writer.WriteLine("backgroundColor: [");
                // Adjust the alpha value to make the colors more vibrant
                writer.WriteLine("'rgba(255, 99, 132, 1)',");
                writer.WriteLine("'rgba(54, 162, 235, 1)',");
                writer.WriteLine("'rgba(255, 206, 86, 1)',");
                writer.WriteLine("'rgba(75, 192, 192, 1)',");
                writer.WriteLine("'rgba(153, 102, 255, 1)',");
                writer.WriteLine("'rgba(255, 159, 64, 1)'");
                writer.WriteLine("],");
                writer.WriteLine("borderColor: [");
                // Adjust the alpha value for border colors if needed
                writer.WriteLine("'rgba(255, 99, 132, 1)',");
                writer.WriteLine("'rgba(54, 162, 235, 1)',");
                writer.WriteLine("'rgba(255, 206, 86, 1)',");
                writer.WriteLine("'rgba(75, 192, 192, 1)',");
                writer.WriteLine("'rgba(153, 102, 255, 1)',");
                writer.WriteLine("'rgba(255, 159, 64, 1)'");
                writer.WriteLine("],");
                writer.WriteLine("borderWidth: 1");
                writer.WriteLine("}]");
                writer.WriteLine("},");
                //writer.WriteLine("options: {}");
                writer.WriteLine("options: {");
                writer.WriteLine("    indexAxis: 'y',");
                writer.WriteLine("    scales: {");
                writer.WriteLine("        y: {");
                writer.WriteLine("            stacked: true,");
                writer.WriteLine("            beginAtZero: true,");
                writer.WriteLine("            ticks: {");
                writer.WriteLine("                color: 'white'");
                writer.WriteLine("            }");
                writer.WriteLine("        },");
                writer.WriteLine("        x: {");
                writer.WriteLine("            stacked: true,");
                writer.WriteLine("            ticks: {");
                writer.WriteLine("                color: 'white'");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.WriteLine("});");
                writer.WriteLine("</script>");
                foreach (var group in groupedItems)
                {
                    // Generate the HTML accordion structure for each unique IPN
                    string stockColorBut = group.TotalStock > 0 ? "lightgreen" : "#FF7F7F";
                    writer.WriteLine("<button class='accordion' style='background-color: " + stockColorBut + ";'>");
                    writer.WriteLine($"<strong>{group.IPN}</strong> - Current Balance: <strong>{group.TotalStock}</strong></button>");
                    writer.WriteLine("<div class='panel'><p>");
                    writer.WriteLine("<table style='width:100%; text-align:center;' border='1'>");
                    writer.WriteLine("<tr style='background-color: lightgray;'><th>Manufacturer</th><th>MFPN</th><th>Description</th><th>Stock</th><th>Updated On</th><th>ReelBagTrayStick</th><th>Source Requester</th></tr>");
                    foreach (var item in group.Items)
                    {
                        string stockColor = item.Stock > 0 && !group.Items.Any(otherItem =>
         otherItem.Stock == -item.Stock) ? "lightgreen" : (item.Stock < 0 ? "#FF7F7F" : "");  //#FF7F7F
                        writer.WriteLine("<tr style='background-color: lightgray;'>");
                        writer.WriteLine($"<td>{item.Manufacturer}</td>");
                        writer.WriteLine($"<td>{item.MFPN}</td>");
                        writer.WriteLine($"<td>{item.Description}</td>");
                        writer.WriteLine($"<td style='background-color: {stockColor}'>{item.Stock}</td>");
                        writer.WriteLine($"<td>{item.UpdatedOn}</td>");
                        writer.WriteLine($"<td>{item.ReelBagTrayStick}</td>");
                        writer.WriteLine($"<td>{item.SourceRequester}</td>");
                        writer.WriteLine("</tr>");
                    }
                    writer.WriteLine("</table>");
                    writer.WriteLine("</p></div>");
                }
                writer.WriteLine("<script>");
                writer.WriteLine("var acc = document.getElementsByClassName('accordion');");
                writer.WriteLine("var i;");
                writer.WriteLine("for (i = 0; i < acc.length; i++) {");
                writer.WriteLine("acc[i].addEventListener('click', function() {");
                writer.WriteLine("this.classList.toggle('active');");
                writer.WriteLine("var panel = this.nextElementSibling;");
                writer.WriteLine("if (panel.style.display === 'block') {");
                writer.WriteLine("panel.style.display = 'none';");
                writer.WriteLine("this.style.fontSize = '90%';");
                writer.WriteLine("} else {");
                writer.WriteLine("panel.style.display = 'block';");
                writer.WriteLine("this.style.fontSize = '150%';");
                writer.WriteLine("}");
                writer.WriteLine("});");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                // Add the JavaScript to open all accordions
                writer.WriteLine("<script>");
                writer.WriteLine("function openAllAccordions() {");
                writer.WriteLine("var acc = document.getElementsByClassName('accordion');");
                writer.WriteLine("var i;");
                writer.WriteLine("for (i = 0; i < acc.length; i++) {");
                writer.WriteLine("var panel = acc[i].nextElementSibling;");
                writer.WriteLine("panel.style.display = 'block';");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                writer.WriteLine("<script>");
                writer.WriteLine("function filterItems() {");
                writer.WriteLine("var input, filter, accordions, panels, i, ipn;");
                writer.WriteLine("input = document.getElementById('filterInput');");
                writer.WriteLine("filter = input.value.toUpperCase();");
                writer.WriteLine("accordions = document.getElementsByClassName('accordion');");
                writer.WriteLine("for (i = 0; i < accordions.length; i++) {");
                writer.WriteLine("ipn = accordions[i].getElementsByTagName('strong')[0].innerText;");
                writer.WriteLine("if (ipn.toUpperCase().indexOf(filter) > -1) {");
                writer.WriteLine("accordions[i].style.display = '';");
                writer.WriteLine("} else {");
                writer.WriteLine("accordions[i].style.display = 'none';");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("function clearFilter() {");
                writer.WriteLine("var input, accordions, i;");
                writer.WriteLine("input = document.getElementById('filterInput');");
                writer.WriteLine("input.value = '';");
                writer.WriteLine("accordions = document.getElementsByClassName('accordion');");
                writer.WriteLine("for (i = 0; i < accordions.length; i++) {");
                writer.WriteLine("accordions[i].style.display = '';");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            process.Start();
        }
        private void GenerateHTML()
        {
            SetSTOCKiewColumsOrder();
            string fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = @"\\dbr1\Data\WareHouse\2024\WHsearcher\" + fileTimeStamp + "_" + ".html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='text-align:center'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>" + textBox10.Text + "</title>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body style=\"background-color:#000;\">");
                writer.WriteLine("<table border='1' style=\"background-color:  #D3D3D3;\">");
                writer.WriteLine("<tr style='text-align:center'>");
                // Assuming you have a reference to the selected DataGridViewCell
                DataGridViewCell selectedCell = dataGridView1.SelectedCells[0];
                int ipnColumnIndex = dataGridView1.Columns["IPN"].Index; // Replace "IPN" with the actual column name
                string cellValue = string.Empty;
                if (dataGridView1.Rows.Count > 0 && ipnColumnIndex >= 0)
                {
                    // Getting the value of the cell in the first row and "IPN" column
                    cellValue = dataGridView1.Rows[0].Cells[ipnColumnIndex].Value != null
                       ? dataGridView1.Rows[0].Cells[ipnColumnIndex].Value.ToString()
                       : "";
                }
                writer.WriteLine("<td>" + "WAREHOUSE STOCK STATUS for : <b>" + cellValue + "</b> UPDATED " + fileTimeStamp + "</td>");
                if (label15.Text.Contains("BALANCE: 0"))
                {
                    writer.WriteLine("<td style=\"background-color:  #FF7F7F;\">" + label15.Text + "</td>");
                }
                else
                {
                    writer.WriteLine("<td style=\"background-color: lightgreen;\">" + label15.Text + "</td>");
                }
                writer.WriteLine("</tr>");
                writer.WriteLine("<tr style='text-align:center'>");
                // Set column order and autosize mode
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                // Add header from DataGridView with specified column order
                writer.WriteLine("<th>" + dataGridView1.Columns["IPN"].HeaderText + "</th>");
                writer.WriteLine("<th>" + dataGridView1.Columns["Manufacturer"].HeaderText + "</th>");
                writer.WriteLine("<th>" + dataGridView1.Columns["MFPN"].HeaderText + "</th>");
                writer.WriteLine("<th>" + dataGridView1.Columns["Description"].HeaderText + "</th>");
                writer.WriteLine("<th>" + dataGridView1.Columns["Stock"].HeaderText + "</th>");
                writer.WriteLine("<th>" + dataGridView1.Columns["UpdatedOn"].HeaderText + "</th>");
                writer.WriteLine("<th>" + dataGridView1.Columns["ReelBagTrayStick"].HeaderText + "</th>");
                writer.WriteLine("<th>" + dataGridView1.Columns["SourceRequester"].HeaderText + "</th>");
                writer.WriteLine("</tr>");
                // Iterate through the rows
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    writer.WriteLine("<tr style='text-align:center'>");
                    // Iterate through the cells in the specified column order
                    writer.WriteLine("<td>" + row.Cells["IPN"].Value.ToString() + "</td>");
                    writer.WriteLine("<td>" + row.Cells["Manufacturer"].Value.ToString() + "</td>");
                    writer.WriteLine("<td>" + row.Cells["MFPN"].Value.ToString() + "</td>");
                    writer.WriteLine("<td>" + row.Cells["Description"].Value.ToString() + "</td>");
                    if (row.Cells["Stock"].Value.ToString().Contains("-"))
                    {
                        writer.WriteLine("<td style=\"background-color:  #FF7F7F;\">" + row.Cells["Stock"].Value.ToString() + "</td>");
                    }
                    else
                    {
                        writer.WriteLine("<td style=\"background-color: lightgreen;\">" + row.Cells["Stock"].Value.ToString() + "</td>");
                    }
                    writer.WriteLine("<td>" + row.Cells["UpdatedOn"].Value.ToString() + "</td>");
                    writer.WriteLine("<td>" + row.Cells["ReelBagTrayStick"].Value.ToString() + "</td>");
                    writer.WriteLine("<td>" + row.Cells["SourceRequester"].Value.ToString() + "</td>");
                    writer.WriteLine("</tr>");
                }
                writer.WriteLine("</table>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            process.Start();
        }
        private void button23_Click(object sender, EventArgs e)
        {
            using (FrmSplit fs = new FrmSplit())
            {
                fs.wHitemToSplitFromTheMainForm = wHitemToSplit;
                // Calculate the difference in width
                int widthDifference = Screen.PrimaryScreen.WorkingArea.Width - fs.Width;
                // Adjust the form's width without changing the height
                fs.Width += widthDifference;
                // Subscribe to the AdjustmentCompleted event
                fs.AdjustmentCompleted += SubForm_AdjustmentCompleted;
                // Handle the Load event to set focus on textbox1
                fs.Load += (s, eventArgs) => { fs.TextBox1.Focus(); };
                // Show the subform
                fs.ShowDialog();
            }
        }
        private void SubForm_AdjustmentCompleted(object sender, AdjustmentEventArgs e)
        {
            e.OriginalItem.SourceRequester = "SPLIT";
            e.OriginalItem.Stock = e.OriginalItem.Stock * (-1);
            e.OriginalItem.UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            DataInserterSplitter(stockFile, "STOCK", e.OriginalItem, false);
            stockItems.Add(e.OriginalItem);
            Thread.Sleep(1000);
            e.AdjustedItemA.UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            DataInserterSplitter(stockFile, "STOCK", e.AdjustedItemA, true);
            stockItems.Add(e.AdjustedItemA);
            e.AdjustedItemB.UpdatedOn = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            DataInserterSplitter(stockFile, "STOCK", e.AdjustedItemB, true);
            stockItems.Add(e.AdjustedItemB);
            PopulateStockView();
        }
        private string GetObjectPropertiesAsString(WHitem item)
        {
            // Get all properties of the WHitem object and format them as a string
            string properties =
                $"IPN: {item.IPN}\n" +
                $"Manufacturer: {item.Manufacturer}\n" +
                $"MFPN: {item.MFPN}\n" +
                $"Description: {item.Description}\n" +
                $"Stock: {item.Stock}\n" +
                $"UpdatedOn: {item.UpdatedOn}\n" +
                $"ReelBagTrayStick: {item.ReelBagTrayStick}\n" +
                $"SourceRequester: {item.SourceRequester}";
            return properties;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button23.Enabled = true;
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            WHitem whi = new WHitem()
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
            wHitemToSplit = whi;
        }
        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchbyMFPN = string.Empty;
                if (textBox12.Text.Contains("-") == true && textBox12.Text.Length > 6)
                {
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
                //lastTxtbInputFromUser = textBox13;
                textBox2.Focus();
                textBox2_KeyDown(sender, e);
            }
        }
        private void textBox12_Click(object sender, EventArgs e)
        {
            textBox12.Clear();
        }
        private void textBox12_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(textBox12);
        }
        private void textBox11_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(textBox11);
        }
        private void textBox12_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(textBox12);
        }
        private void textBox11_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(textBox11);
        }
        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string searchbyMFPN = textBox13.Text.Trim(); // Get the text from textBox14
                // Remove [)> characters from the search string
                searchbyMFPN = searchbyMFPN.Replace("[)>", "");
                if (!string.IsNullOrEmpty(searchbyMFPN))
                {
                    // Loop through the DataGridView rows and filter based on MFPN
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (row.Cells["MFPN"].Value != null)
                        {
                            string cellValue = row.Cells["MFPN"].Value.ToString();
                            // Check if the search text contains the cell value
                            if (searchbyMFPN.Contains(cellValue))
                            {
                                textBox2.Text = cellValue;
                                // Extract the quantity value from the input string
                                int qtyIndex1 = searchbyMFPN.IndexOf("qty:", StringComparison.OrdinalIgnoreCase);
                                int qtyIndex2 = searchbyMFPN.IndexOf("11ZPICK", StringComparison.OrdinalIgnoreCase);
                                int qtyIndex3 = searchbyMFPN.IndexOf("V003331", StringComparison.OrdinalIgnoreCase);
                                if (qtyIndex1 != -1)
                                {
                                    int commaIndex = searchbyMFPN.IndexOf(",", qtyIndex1);
                                    if (commaIndex != -1)
                                    {
                                        string qtyValue = searchbyMFPN.Substring(qtyIndex1 + "qty:".Length, commaIndex - qtyIndex1 - "qty:".Length).Trim();
                                        textBox6.Text = qtyValue;
                                        BrandNewItemAutoInsertionToDB();
                                    }
                                    else
                                    {
                                        // If there is no comma after "qty:", take the remaining string
                                        string qtyValue = searchbyMFPN.Substring(qtyIndex1 + "qty:".Length).Trim();
                                        textBox6.Text = qtyValue;
                                        BrandNewItemAutoInsertionToDB();
                                    }
                                }
                                else if (qtyIndex2 != -1)
                                {
                                    int qtyStartIndex = searchbyMFPN.LastIndexOf("Q", qtyIndex2);
                                    if (qtyStartIndex != -1)
                                    {
                                        string qtyValue = searchbyMFPN.Substring(qtyStartIndex + 1, qtyIndex2 - qtyStartIndex - 1).Trim();
                                        textBox6.Text = qtyValue;
                                        BrandNewItemAutoInsertionToDB();
                                    }
                                    else
                                    {
                                        // Handle the case where "Q" is not found before "11ZPICK"
                                        textBox6.Text = "";
                                    }
                                }
                                else if (qtyIndex3 != -1)
                                {
                                    int qtyStartIndex = searchbyMFPN.LastIndexOf("Q", qtyIndex3);
                                    if (qtyStartIndex != -1)
                                    {
                                        string qtyValue = searchbyMFPN.Substring(qtyStartIndex + 1, qtyIndex3 - qtyStartIndex - 1).Trim();
                                        textBox6.Text = qtyValue;
                                        BrandNewItemAutoInsertionToDB();
                                    }
                                    else
                                    {
                                        // Handle the case where "Q" is not found before "V003331"
                                        textBox6.Text = "";
                                    }
                                }
                                else
                                {
                                    // Handle the case where neither "qty:", "11ZPICK," nor "V003331" is found in the input string
                                    textBox6.Text = "";
                                }
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
                }
                LastInputFromUser = textBox13;
                textBox2.Focus();
                textBox2_KeyDown(sender, e);
            }
        }
        private void BrandNewItemAutoInsertionToDB()
        {
            if (checkBox1.Checked)
            {
                SendKeys.Send("{ENTER}");
                // Now, you can handle the KeyPress event if needed
                //textBox6_KeyPress(sender, new KeyPressEventArgs((char)Keys.Enter));
            }
        }
        private void textBox13_Click(object sender, EventArgs e)
        {
            textBox13.Clear();
            textBox2.Clear();
        }
        private void textBox13_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(textBox13);
            textBox2.Clear();
        }
        private void textBox13_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(textBox13);
        }
        private void button30_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "ROBOTRON")
            {
                string prefix = warehouses.FirstOrDefault(x => x.clName == comboBox3.SelectedItem.ToString()).clPrefix;
                try
                {
                    //if (avlItems[0].IPN.Contains("_"))
                    //{
                    //    List<string> sp = avlItems[0].IPN.Split('_').ToList();
                    //    prefix = sp[0];
                    //}
                    //else if (avlItems[0].IPN.Contains("-"))
                    //{
                    //    List<string> sp = avlItems[0].IPN.Split('-').ToList();
                    //    prefix = sp[0];
                    //}
                    FrmIPNgenerator gen = new FrmIPNgenerator(avlItems, prefix);
                    gen.FormClosed += (s, args) =>
                    {
                        button2.PerformClick(); // Replace button2 with your actual button name
                    };
                    gen.Show();
                }
                catch
                {
                }
            }
        }
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string currentReelBagTrayStick = selectedRow.Cells["ReelBagTrayStick"].Value.ToString();
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
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                foreach (string option in comboBox1.Items)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(option);
                    item.Click += (sender, args) =>
                    {
                        ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
                        string newReelBagTrayStick = clickedItem.Text;
                        // Show a confirmation message box before applying the changes
                        DialogResult dialogResult = MessageBox.Show($"Apply changes to Warehouse Item? Change from {currentReelBagTrayStick} to {newReelBagTrayStick} ?", "Confirmation", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            selectedRow.Cells["ReelBagTrayStick"].Value = newReelBagTrayStick;
                            DataUpdater(stockFile, "STOCK", currentReelBagTrayStick, wHitemABCD, newReelBagTrayStick);
                        }
                    };
                    contextMenu.Items.Add(item);
                }
                // Display the context menu at the current mouse position
                contextMenu.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
            }
        }
        private void DataUpdater(string fp, string thesheetName, string currentReelBagTrayStick, WHitem wHitem, string newReelBagTrayStick)
        {
            //AND Manufacturer = @Manufacturer
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand($"UPDATE [{thesheetName}$] SET Comments = @NewReelBagTrayStick WHERE Comments = @currentReelBagTrayStick AND IPN = @IPN AND MFPN = @MFPN AND Description = @Description AND Stock = @Stock AND Updated_on = @UpdatedOn AND Source_Requester = @SourceRequester", conn);
                    command.Parameters.AddWithValue("@NewReelBagTrayStick", newReelBagTrayStick);
                    command.Parameters.AddWithValue("@currentReelBagTrayStick", currentReelBagTrayStick);
                    command.Parameters.AddWithValue("@IPN", wHitem.IPN);
                    //command.Parameters.AddWithValue("@Manufacturer", wHitem.Manufacturer);
                    command.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                    command.Parameters.AddWithValue("@Description", wHitem.Description);
                    command.Parameters.AddWithValue("@Stock", wHitem.Stock);
                    command.Parameters.AddWithValue("@UpdatedOn", wHitem.UpdatedOn);
                    command.Parameters.AddWithValue("@SourceRequester", wHitem.SourceRequester);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                // Update the UI or perform any necessary actions after the update
            }
            catch (IOException)
            {
                MessageBox.Show("Error");
            }
        }
        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
        }
        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                openFileDialog1.Title = "Select BOM File";
                openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\2024\\" + DateTime.Now.ToString("MM") + ".2024";
                openFileDialog1.Filter = "BOM files(*.xlsm) | *.xlsm";
                openFileDialog1.Multiselect = false;
                string currentPrefix = string.Empty;
                string currentAvl = string.Empty;
                foreach (ClientWarehouse w in warehouses)
                {
                    if (comboBox3.Text == w.clName)
                    {
                        currentPrefix = w.clPrefix;
                        currentAvl = w.clAvlFile;
                    }
                }
                List<WHitem> ItemsToAddToAvl = new List<WHitem>();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog1.FileName;
                    string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1';";
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        // Get the first sheet name from the schema
                        string sheetName = GetFirstSheetName(connection);
                        if (IsValidSheet(sheetName))
                        {
                            // Check if the required columns exist
                            List<string> missingColumns = CheckColumnsExist(connection, sheetName, "IPN", "Manufacturer", "MFPN", "Description");
                            // Check if the required columns exist
                            // if (CheckColumnsExist(connection, sheetName, "IPN", "Manufacturer", "MFPN", "Description"))
                            if (missingColumns.Count == 0)
                            {
                                // Columns exist, proceed with data retrieval
                                DataTable dtExcelData = RetrieveExcelData(connection, sheetName);
                                // Check if the first IPN starts with currentPrefix
                                if (IsValidPrefix(dtExcelData))
                                {
                                    // Process data and add to ItemsToAddToAvl
                                    ProcessExcelData(dtExcelData);
                                    // Check for unique MFPN items
                                    CheckAndHandleUniqueItems();
                                }
                                else
                                {
                                    MessageBox.Show("INCORRECT CLIENT BOM!");
                                    // Let the user select a suitable file again
                                    button2_MouseDown(sender, e);
                                    return;
                                }
                            }
                            else
                            {
                                //MessageBox.Show("Error: The required columns do not exist in the Excel sheet.");
                                // Display the missing columns in the error message
                                MessageBox.Show($"Error: The following columns do not exist in the Excel sheet: {string.Join(", ", missingColumns)}");
                                // Open the Excel file for user to make necessary changes
                                //OpenExcelFile(openFileDialog1.FileName);
                                AuthorizedExcelFileOpening(openFileDialog1.FileName);
                            }
                        }
                    }
                    void OpenExcelFile(string filePath)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(filePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error opening Excel file: {ex.Message}");
                        }
                    }
                    // Helper method to check if the sheet name is valid
                    bool IsValidSheet(string sheetName)
                    {
                        return !string.IsNullOrEmpty(sheetName) && !sheetName.EndsWith("_");
                    }
                    // Helper method to retrieve Excel data
                    DataTable RetrieveExcelData(OleDbConnection connection, string sheetName)
                    {
                        OleDbDataAdapter dataAdapter = new OleDbDataAdapter($"SELECT IPN, Manufacturer, MFPN, Description FROM [{sheetName}]", connection);
                        DataTable dtExcelData = new DataTable();
                        dataAdapter.Fill(dtExcelData);
                        return dtExcelData;
                    }
                    // Helper method to check if the first IPN starts with the current prefix
                    bool IsValidPrefix(DataTable dtExcelData)
                    {
                        if (dtExcelData.Rows.Count > 0)
                        {
                            string firstIPN = dtExcelData.Rows[0]["IPN"]?.ToString();
                            return !string.IsNullOrEmpty(firstIPN) && firstIPN.StartsWith(currentPrefix);
                        }
                        return false;
                    }
                    // Helper method to process Excel data and add to ItemsToAddToAvl
                    void ProcessExcelData(DataTable dtExcelData)
                    {
                        foreach (DataRow item in dtExcelData.Rows)
                        {
                            ItemsToAddToAvl.Add(new WHitem
                            {
                                IPN = item["IPN"]?.ToString()?.Trim(),
                                Manufacturer = item["Manufacturer"]?.ToString(),
                                MFPN = item["MFPN"]?.ToString()?.Trim(),
                                Description = item["Description"]?.ToString()
                            });
                        }
                    }
                    // Helper method to check for unique MFPN items and handle accordingly
                    void CheckAndHandleUniqueItems()
                    {
                        var uniqueMFPNItems = ItemsToAddToAvl
                            .Where(newItem => !avlItems.Any(existingItem => existingItem.MFPN == newItem.MFPN))
                            .ToList();
                        if (uniqueMFPNItems.Count > 0)
                        {
                            string message = $"{uniqueMFPNItems.Count} new ITEMS found:\n\n";
                            foreach (var item in uniqueMFPNItems)
                            {
                                message += $"IPN: {item.IPN}, Manufacturer: {item.Manufacturer}, MFPN: {item.MFPN}, Description: {item.Description}\n\n";
                            }
                            MessageBox.Show(message);
                            // Call the function to add new items to the database
                            AddNewItemsToAVL(currentAvl, uniqueMFPNItems);
                        }
                        else
                        {
                            MessageBox.Show("Nothing new here");
                        }
                    }
                }
            }
        }
        // Helper method to get the first sheet name from the schema
        string GetFirstSheetName(OleDbConnection connection)
        {
            DataTable dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["TABLE_NAME"].ToString();
            }
            return null;
        }
        List<string> CheckColumnsExist(OleDbConnection connection, string sheetName, params string[] columnNames)
        {
            DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, sheetName, null });
            List<string> missingColumns = new List<string>();
            foreach (string columnName in columnNames)
            {
                // Check if the column exists in the schema table
                DataRow[] rows = schemaTable.Select("COLUMN_NAME = '" + columnName + "'");
                if (rows.Length == 0)
                {
                    missingColumns.Add(columnName);
                }
            }
            return missingColumns;
        }
        private void AddNewItemsToAVL(string currentAvl, List<WHitem> newItems)
        {
            //MessageBox.Show(currentAvl);
            try
            {
                OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
                builder.Provider = "Microsoft.ACE.OLEDB.12.0";
                builder.DataSource = currentAvl;
                builder["Extended Properties"] = "Excel 12.0 Xml;HDR=YES;IMEX=0;Mode=ReadWrite";
                using (OleDbConnection connection = new OleDbConnection(builder.ConnectionString))
                {
                    connection.Open();
                    foreach (var item in newItems)
                    {
                        // Assuming you have a sheet named 'YourSheetName' in your Excel file
                        string query = "INSERT INTO [AVL$] (IPN, Manufacturer, MFPN, Description) VALUES (@IPN, @Manufacturer, @MFPN, @Description)";
                        using (OleDbCommand command = new OleDbCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@IPN", item.IPN);
                            command.Parameters.AddWithValue("@Manufacturer", item.Manufacturer);
                            command.Parameters.AddWithValue("@MFPN", item.MFPN);
                            command.Parameters.AddWithValue("@Description", item.Description);
                            command.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show(newItems.Count.ToString() + " New items added to the AVL file successfully.");
                    button2.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new items to the AVL file: {ex.Message}");
                // Handle the exception appropriately (log, show a user-friendly message, etc.)
            }
        }
        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //MessageBox.Show("right");
                GenerateHTMLwareHouseBalanceListByIPN();
            }
            else
            {
                //
            }
        }
        void GenerateHTMLwareHouseBalanceListByIPN()
        {
            string fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = @"\\dbr1\Data\WareHouse\2024\WHsearcher\" + fileTimeStamp + "_" + comboBox3.SelectedItem.ToString() + "_wh_Balance" + ".html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='background-color: gray;'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<script src='https://cdn.jsdelivr.net/npm/chart.js'></script>");
                writer.WriteLine("<script src='https://cdnjs.cloudflare.com/ajax/libs/json2/20210202/json2.js'></script>");
                writer.WriteLine("<script src='https://cdnjs.cloudflare.com/ajax/libs/newtonsoft.json/13.0.1/json.net.min.js'></script>");
                writer.WriteLine("<h1 id='stickyHeader' style='position: sticky; top: 0; background-color: lightgreen; text-align: center;'>");
                writer.WriteLine($"{fileTimeStamp}_{comboBox3.SelectedItem}");
                writer.WriteLine("<br>");
                writer.WriteLine("<input type='text' id='filterInput' onkeyup='filterItems()' placeholder='Type to filter...' style='text-align:center;margin: 10px;'>");
                writer.WriteLine("<button onclick='clearFilter()' style='margin: 10px;'>CLEAR</button>");
                writer.WriteLine("</h1>");
                writer.WriteLine("<script>");
                writer.WriteLine("function filterItems() {");
                writer.WriteLine("  var input, filter, table, tr, td, i, txtValue;");
                writer.WriteLine("  input = document.getElementById('filterInput');");
                writer.WriteLine("  filter = input.value.toUpperCase();");
                writer.WriteLine("  table = document.querySelector('table');");
                writer.WriteLine("  tr = table.getElementsByTagName('tr');");
                writer.WriteLine("  for (i = 1; i < tr.length; i++) {");  // Start from 1 to skip the header row
                writer.WriteLine("    var display = 'none';");
                writer.WriteLine("    td = tr[i].getElementsByTagName('td');");
                writer.WriteLine("    for (var j = 0; j < td.length; j++) {");
                writer.WriteLine("      txtValue = td[j].textContent || td[j].innerText;");
                writer.WriteLine("      if (txtValue.toUpperCase().indexOf(filter) > -1) {");
                writer.WriteLine("        display = '';");
                writer.WriteLine("        break;");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("    tr[i].style.display = display;");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("function clearFilter() {");
                writer.WriteLine("  document.getElementById('filterInput').value = '';");
                writer.WriteLine("  filterItems();");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                var orderedStockItems = stockItems.OrderBy(item => item.IPN).ToList();
                // Group items by IPN and calculate the total stock for each unique IPN
                var groupedItems = orderedStockItems.GroupBy(item => item.IPN)
                                            .Select(group => new
                                            {
                                                IPN = group.Key,
                                                TotalStock = group.Sum(item => item.Stock),
                                                Items = group.Select(item => new
                                                {
                                                    item.Manufacturer,
                                                    item.MFPN,
                                                    item.Description,
                                                    item.Stock,
                                                    item.UpdatedOn,
                                                    item.ReelBagTrayStick,
                                                    item.SourceRequester
                                                })
                                            });
                writer.WriteLine("<table style='width:100%; text-align:center;' border='1'>");
                writer.WriteLine($"<tr style='background-color: lightgray;'><th>IPN</th><th>MFPN</th><th>Description</th><th>Stock</th></tr>");
                var totalQtyOfItemsInWarehouse = groupedItems.Sum(group => group.TotalStock);
                writer.WriteLine($"<tr style='background-color: lightgray;'><td>{groupedItems.Count()} IPNs found</td><td></td><td></td><td>{totalQtyOfItemsInWarehouse}</td></tr>");
                foreach (var group in groupedItems)
                {
                    string backgroundColor;
                    if (group.TotalStock > 0)
                    {
                        backgroundColor = "lightgreen";
                    }
                    else if (group.TotalStock == 0 || group.TotalStock < 0)
                    {
                        backgroundColor = "#FF7F7F";
                    }
                    else
                    {
                        backgroundColor = ""; // Default color or any other desired color
                    }
                    writer.WriteLine($"<tr style='background-color: {backgroundColor};'><td>{group.IPN}</td><td>{group.Items.FirstOrDefault()?.MFPN}</td><td>{group.Items.FirstOrDefault()?.Description}</td><td>{group.TotalStock}</td></tr>");
                }
                writer.WriteLine("</table>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            process.Start();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.BackColor = Color.Red;
            }
            else
            {
                // Set the background color to the default color (you may replace this with the actual default color)
                checkBox1.BackColor = Color.LightGray;
            }
        }
    }
}
