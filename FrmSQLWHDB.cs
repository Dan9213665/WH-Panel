using FastMember;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Button = System.Windows.Forms.Button;
using DataTable = System.Data.DataTable;
using GroupBox = System.Windows.Forms.GroupBox;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;
#pragma warning disable CS0618
namespace WH_Panel
{
    public partial class FrmSQLWHDB : Form
    {
        private string selectedDatabase; // To store the selected database name
        public FrmSQLWHDB()
        {
            InitializeComponent();
            UpdateControlColors(this);
            LoadDatabases();
            // Subscribe to the SelectedIndexChanged event of the ComboBox
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;
            UpdateControlColors(this);
            comboBox1.SelectedIndex = 1;
            comboBox3.SelectedIndex = 0;
        }
        public TextBox LastInputFromUser = new TextBox();
        public WHitem wHitemToSplit = new WHitem();
        public DataTable dtAVL = new DataTable();
        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected database name
            selectedDatabase = comboBox3.SelectedItem as string;
            // Load the tables into the DataGridViews
            LoadAVLTable();
        }
        private void LoadAVLTable()
        {
            if (string.IsNullOrEmpty(selectedDatabase))
                return; // No selected database
            // Connection string for SQL Server Express
            string connectionString = $"Data Source=RT12\\SQLEXPRESS;Initial Catalog={selectedDatabase};Integrated Security=True;";
            try
            {
                // Load AVL table into dataGridView2
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    dtAVL.Clear();
                    SqlDataAdapter adapterAVL = new SqlDataAdapter("SELECT * FROM AVL", connection);
                    adapterAVL.Fill(dtAVL);
                    dataGridView2.DataSource = dtAVL;
                    // Hide the "Id" column
                    dataGridView2.Columns["Id"].Visible = false;
                    // Autosize columns to fill the available width
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    // Display the number of rows loaded in label
                    label1.Text = $"{dtAVL.Rows.Count} rows loaded";
                    label1.BackColor = Color.LightGreen;
                }
                textBox1.Focus();
                LastInputFromUser = textBox1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
        }
        private void LoadDatabases()
        {
            // Connection string for SQL Server Express
            string connectionString = "Data Source=RT12\\SQLEXPRESS;Integrated Security=True;";
            // Clear existing items in ComboBox
            comboBox3.Items.Clear();
            try
            {
                // Establish connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Retrieve the list of databases
                    DataTable databases = connection.GetSchema("Databases");
                    // Add each database name to the ComboBox, excluding system databases
                    foreach (DataRow database in databases.Rows)
                    {
                        string dbName = database.Field<string>("database_name");
                        // Exclude system databases
                        if (!IsSystemDatabase(dbName))
                        {
                            comboBox3.Items.Add(dbName);
                        }
                    }
                    // Close the connection
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading databases: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Function to check if a database is a system database
        private bool IsSystemDatabase(string databaseName)
        {
            return databaseName.Equals("master", StringComparison.OrdinalIgnoreCase) ||
                   databaseName.Equals("model", StringComparison.OrdinalIgnoreCase) ||
                   databaseName.Equals("msdb", StringComparison.OrdinalIgnoreCase) ||
                   databaseName.Equals("tempdb", StringComparison.OrdinalIgnoreCase);
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
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox6.Focus();
            // Set MultiSelect property to false in your form's constructor or Load event
            dataGridView2.MultiSelect = false;
            // Check if the clicked cell is not a header cell
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];
                // Access the value of the "IPN" column in the selected row
                string selectedIPN = selectedRow.Cells["IPN"].Value.ToString();
                // Load data into dataGridView1 based on the selected IPN
                LoadDataIntoDataGridView1(selectedIPN);
            }
        }
        private void UpdateWarehouseBalanceLabel(string selectedIPN)
        {
            // Connection string for SQL Server Express
            string connectionString = $"Data Source=RT12\\SQLEXPRESS;Initial Catalog={selectedDatabase};Integrated Security=True;";
            try
            {
                // Calculate the sum of the STOCK column values for the selected IPN
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Use a parameterized query to prevent SQL injection
                    string query = "SELECT SUM(STOCK) AS WarehouseBalance FROM STOCK WHERE IPN = @SelectedIPN";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameter to the query
                        command.Parameters.AddWithValue("@SelectedIPN", selectedIPN);
                        // Execute the query and get the sum value
                        object sumValue = command.ExecuteScalar();
                        // Update label15 with the sum value
                        label15.Text = $"Balance: {sumValue ?? 0}"; // Default to 0 if sumValue is null
                                                                    // Set background color based on the sum value
                        label15.BackColor = (sumValue != null && Convert.ToInt32(sumValue) > 0) ? Color.LightGreen : Color.IndianRed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating Warehouse Balance label: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label15.Text = $"Balance: {0}";
            }
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
            dataGridView1.Columns["Updated_on"].DisplayIndex = 5;
            dataGridView1.Columns["Comments"].DisplayIndex = 6;
            dataGridView1.Columns["Source_Requester"].DisplayIndex = 7;
        }
        private void LoadDataIntoDataGridView1(string selectedIPN)
        {
            // Connection string for SQL Server Express
            string connectionString = $"Data Source=RT12\\SQLEXPRESS;Initial Catalog={selectedDatabase};Integrated Security=True;";
            try
            {
                // Load data into dataGridView1 based on the selected IPN
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Use a parameterized query to prevent SQL injection
                    string query = "SELECT * FROM STOCK WHERE IPN = @SelectedIPN ORDER BY Updated_On DESC";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameter to the query
                        command.Parameters.AddWithValue("@SelectedIPN", selectedIPN);
                        SqlDataAdapter adapterStock = new SqlDataAdapter(command);
                        DataTable dtStock = new DataTable();
                        adapterStock.Fill(dtStock);
                        // Bind the data to dataGridView1
                        dataGridView1.DataSource = dtStock;
                        // Hide the "Id" column
                        dataGridView1.Columns["Id"].Visible = false;
                        SetSTOCKiewColumsOrder();
                        // Optionally, you can autosize columns in dataGridView1
                        //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        // Update label15 with the warehouse balance for the selected IPN
                        UpdateWarehouseBalanceLabel(selectedIPN);
                        isFiltered = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data into dataGridView1: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool isFiltered = false;
        private void button4_Click(object sender, EventArgs e)
        {
            isFiltered = !isFiltered;
            if (isFiltered)
            {
                FilterActualItemsOnly();
                button4.BackColor = Color.LightGreen;
                isFiltered = true;
                button4.Text = "FIlter Current WH stock ONLY(FILTERED)";
            }
            else
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex];
                // Access the value of the "IPN" column in the selected row
                string selectedIPN = selectedRow.Cells["IPN"].Value.ToString();
                LoadDataIntoDataGridView1(selectedIPN);
                button4.BackColor = Color.LightGray;
                isFiltered = false;
                button4.Text = "FIlter Current WH stock ONLY(UNFILTERED)";
            }
        }
        private void FilterActualItemsOnly()
        {
            List<WHitem> inWHstock = new List<WHitem>();
            // Convert dataGridView1 data to List<WHitem>
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
                    Updated_on = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Updated_on"].Index].Value.ToString(),
                    Comments = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Comments"].Index].Value.ToString(),
                    Source_Requester = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Source_Requester"].Index].Value.ToString()
                };
                inWHstock.Add(wHitemABC);
            }
            // Apply logic to filter out negative quantities
            List<WHitem> negativeQTYs = inWHstock.Where(item => item.Stock < 0).ToList();
            List<WHitem> positiveInWH = inWHstock.Where(item => item.Stock > 0).ToList();
            // Remove matching pairs (positive and negative quantities)
            for (int i = 0; i < negativeQTYs.Count; i++)
            {
                for (int j = 0; j < positiveInWH.Count; j++)
                {
                    if (Math.Abs(negativeQTYs[i].Stock) == positiveInWH[j].Stock)
                    {
                        positiveInWH.Remove((WHitem)positiveInWH[j]);
                        break;
                    }
                }
            }
            // Update dataGridView1 with filtered data
            IEnumerable<WHitem> WHdata = positiveInWH;
            DataTable filteredData = new DataTable();
            using (var reader = ObjectReader.Create(WHdata))
            {
                filteredData.Load(reader);
            }
            // Display filtered data in dataGridView1
            DataView dv = filteredData.DefaultView;
            dataGridView1.DataSource = dv;
            SetSTOCKiewColumsOrder();
            dataGridView1.Update();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label2.BackColor = Color.IndianRed;
            FilterAVLDataGridView();
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
                DataView dv = dtAVL.DefaultView;
                dv.RowFilter = "[IPN] LIKE '%" + searchByIPN.ToString() +
                    "%' AND [MFPN] LIKE '%" + searchbyMFPN.ToString() +
                    "%' AND [DESCRIPTION] LIKE '%" + txtbFiltAVLbyDESCR.Text.ToString() +
                    "%'";
                dataGridView2.DataSource = dv;
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect search pattern, remove invalid character and try again !");
                throw;
            }
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
        private void AvlClearFilters()
        {
            textBox1.Text = string.Empty;
            label2.BackColor = Color.LightGreen;
            textBox2.Text = string.Empty;
            label3.BackColor = Color.LightGreen;
            txtbFiltAVLbyDESCR.Text = string.Empty;
            label16.BackColor = Color.LightGreen;
        }
        private void label2_DoubleClick(object sender, EventArgs e)
        {
            AvlClearFilters();
        }
        private void label3_DoubleClick(object sender, EventArgs e)
        {
            AvlClearFilters();
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
        private void txtbFiltAVLbyDESCR_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
            label16.BackColor = Color.Gray;
        }
        private void txtbFiltAVLbyDESCR_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
            label16.BackColor = Color.LightGreen;
        }
        private void txtbFiltAVLbyDESCR_TextChanged(object sender, EventArgs e)
        {
            label16.BackColor = Color.IndianRed;
            FilterAVLDataGridView();
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
        private void lblRWK_Click(object sender, EventArgs e)
        {
            lblRWK.Text = string.Empty;
            lblRWK.Text += "requested by ";
            textBox9.Text = lblRWK.Text + comboBox6.SelectedItem.ToString() + " on " + DateTime.Now.ToString("yyyy-MM-dd");
        }
        private void lblSendTo_Click(object sender, EventArgs e)
        {
            lblSendTo.Text = string.Empty;
            lblSendTo.Text += "sent to ";
            lblSendTo.Text += comboBox3.Text.ToString() + " on " + DateTime.Now.ToString("yyyy-MM-dd");
            textBox9.Text = lblSendTo.Text;
        }
        private void btnMove_Click(object sender, EventArgs e)
        {
            //int qty = 0;
            string sourceReq = string.Empty;
            if (radioButton1.Checked)
            {
                bool toPrintMFG = true;
                sourceReq = "MFG";
                if (!string.IsNullOrEmpty(textBox6.Text))
                {
                    try
                    {
                        string inputQty = textBox6.Text.Trim();
                        int parsedQty;
                        if (inputQty.StartsWith("Q"))
                        {
                            inputQty = inputQty.Substring(1);
                        }
                        if (int.TryParse(inputQty, out parsedQty) && parsedQty > 0 && parsedQty < 50001)
                        {
                            MoveIntoDATABASE(parsedQty, sourceReq, toPrintMFG);
                            LoadDataIntoDataGridView1(textBox10.Text);
                        }
                        else
                        {
                            ShowErrorMessage("Input positive numeric values ONLY !");
                        }
                    }
                    catch (Exception)
                    {
                        ResetTextBoxAndFocus(textBox6);
                        throw;
                    }
                }
                else
                {
                    ShowErrorMessage("Input Qty !");
                }
            }
            else if (radioButton2.Checked)
            {
                bool toPrintGILT = true;
                if (!string.IsNullOrEmpty(textBox8.Text))
                {
                    sourceReq = comboBox2.Text + textBox8.Text;
                    int parsedQty;
                    if (textBox6.Text.StartsWith("Q") && int.TryParse(textBox6.Text.Substring(1), out parsedQty)
                        || textBox6.Text.Contains(",") && int.TryParse(textBox6.Text.Replace(",", ""), out parsedQty)
                        || int.TryParse(textBox6.Text, out parsedQty))
                    {
                        if (parsedQty > 0 && parsedQty < 50001)
                        {
                            MoveIntoDATABASE(parsedQty, sourceReq, toPrintGILT);
                            LoadDataIntoDataGridView1(textBox10.Text);
                        }
                        else
                        {
                            ShowErrorMessage("Input positive numeric values ONLY !");
                        }
                    }
                    else
                    {
                        ResetTextBoxAndFocus(textBox6);
                    }
                }
                else
                {
                    ShowErrorMessage("Input " + comboBox2.Text + "_XXX ID !");
                    textBox8.Focus();
                }
            }
            else if (radioButton4.Checked)
            {
                bool toPrintWO = false;
                if (!string.IsNullOrEmpty(textBox9.Text))
                {
                    string woValue = textBox9.Text.Contains("_") ? textBox9.Text.Split("_")[1] + "_" + textBox9.Text.Split("_")[2] : textBox9.Text;
                    int parsedQty;
                    if (int.TryParse(textBox6.Text, out parsedQty) && parsedQty > 0 && parsedQty < 50001)
                    {
                        int negQty = parsedQty * (-1);
                        MoveIntoDATABASE(negQty, woValue, toPrintWO);
                        LoadDataIntoDataGridView1(textBox10.Text);
                    }
                    else
                    {
                        ShowErrorMessage("Input Qty !");
                    }
                }
                else
                {
                    ShowErrorMessage("INPUT WO !");
                    textBox9.Focus();
                }
            }
        }
        private void ShowErrorMessage(string message)
        {
            //////////////MessageBox.Show(message);
            ResetTextBoxAndFocus(textBox6);
        }
        private void ResetTextBoxAndFocus(TextBox textBox)
        {
            textBox.Text = string.Empty;
            textBox.Focus();
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
                Updated_on = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"), //tt
                Comments = comboBox1.Text,
                Source_Requester = sorce_req
            };
            DataInserter(selectedDatabase, "STOCK", inputWHitem, toPrint);
            textBox10.Text = inputWHitem.IPN;
            textBox10.BackColor = Color.LightGreen;
            LoadDataIntoDataGridView1(inputWHitem.IPN);
        }
        private void DataInserter(string fp, string thesheetName, WHitem wHitem, bool toPrintOrNotToPrint)
        {
            bool toPrint = toPrintOrNotToPrint;
            try
            {
                // Connection string for SQL Server Express
                string connectionString = $"Data Source=RT12\\SQLEXPRESS;Initial Catalog={selectedDatabase};Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [STOCK] (IPN, Manufacturer, MFPN, Description, Stock, Updated_on, Comments, Source_Requester) VALUES (@IPN, @Manufacturer, @MFPN, @Description, @Stock, @Updated_on, @Comments, @Source_Requester)", conn);
                    // Assuming wHitem is an instance of some class with properties like IPN, Manufacturer, etc.
                    command.Parameters.AddWithValue("@IPN", wHitem.IPN);
                    command.Parameters.AddWithValue("@Manufacturer", wHitem.Manufacturer);
                    command.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                    command.Parameters.AddWithValue("@Description", wHitem.Description);
                    command.Parameters.AddWithValue("@Stock", wHitem.Stock);
                    command.Parameters.AddWithValue("@Updated_on", wHitem.Updated_on);
                    command.Parameters.AddWithValue("@Comments", wHitem.Comments);
                    command.Parameters.AddWithValue("@Source_Requester", wHitem.Source_Requester);
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
                    AutoClosingMessageBox.Show(wHitem.Stock.ToString() + " PCS of " + wHitem.IPN + " in a " + wHitem.Comments + " MOVED to DB ", "Item added to DB", 2000);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error");
            }
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
                cmd.CommandText = "UPDATE [" + thesheetName + "$] SET PN = @PN, MFPN = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, UPDATEDON = @Updated_on";
                cmd.Parameters.AddWithValue("@PN", wHitem.IPN);
                cmd.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                cmd.Parameters.AddWithValue("@ItemDesc", wHitem.Description);
                cmd.Parameters.AddWithValue("@QTY", wHitem.Stock);
                cmd.Parameters.AddWithValue("@Updated_on", wHitem.Updated_on);
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
                LoadDataIntoDataGridView1(textBox3.Text);
            }
        }
        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnMove_Click(this, new EventArgs());
            }
        }
        private void textBox6_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox6_Leave(object sender, EventArgs e)
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
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
        }
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string currentComments = selectedRow.Cells["Comments"].Value.ToString();
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                WHitem wHitemABCD = new WHitem()
                {
                    IPN = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["IPN"].Index].Value.ToString(),
                    Manufacturer = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Manufacturer"].Index].Value.ToString(),
                    MFPN = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["MFPN"].Index].Value.ToString(),
                    Description = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Description"].Index].Value.ToString(),
                    Stock = int.Parse(dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Stock"].Index].Value.ToString()),
                    Updated_on = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Updated_on"].Index].Value.ToString(),
                    Comments = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Comments"].Index].Value.ToString(),
                    Source_Requester = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Source_Requester"].Index].Value.ToString()
                };
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                foreach (string option in comboBox1.Items)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(option);
                    item.Click += (sender, args) =>
                    {
                        ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
                        string newComments = clickedItem.Text;
                        // Show a confirmation message box before applying the changes
                        DialogResult dialogResult = MessageBox.Show($"Apply changes to Warehouse Item? Change from {currentComments} to {newComments} ?", "Confirmation", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            selectedRow.Cells["Comments"].Value = newComments;
                            DataUpdater(selectedDatabase, "STOCK", currentComments, wHitemABCD, newComments);
                        }
                    };
                    contextMenu.Items.Add(item);
                }
                // Display the context menu at the current mouse position
                contextMenu.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
            }
        }
        private void DataUpdater(string fp, string thesheetName, string currentComments, WHitem wHitem, string newComments)
        {
            //AND Manufacturer = @Manufacturer
            try
            {
                // Connection string for SQL Server Express
                string connectionString = $"Data Source=RT12\\SQLEXPRESS;Initial Catalog={selectedDatabase};Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand($"UPDATE [STOCK] SET Comments = @NewComments WHERE Comments = @currentComments AND IPN = @IPN AND MFPN = @MFPN AND Description = @Description AND Stock = @Stock AND Updated_on = @Updated_on AND Source_Requester = @Source_Requester", conn);
                    command.Parameters.AddWithValue("@NewComments", newComments);
                    command.Parameters.AddWithValue("@currentComments", currentComments);
                    command.Parameters.AddWithValue("@IPN", wHitem.IPN);
                    //command.Parameters.AddWithValue("@Manufacturer", wHitem.Manufacturer);
                    command.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                    command.Parameters.AddWithValue("@Description", wHitem.Description);
                    command.Parameters.AddWithValue("@Stock", wHitem.Stock);
                    command.Parameters.AddWithValue("@Updated_on", wHitem.Updated_on);
                    command.Parameters.AddWithValue("@Source_Requester", wHitem.Source_Requester);
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
        private void textBox13_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
            textBox13.Clear();
        }
        private void textBox13_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
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
        private void textBox11_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
            textBox11.Clear();
        }
        private void textBox11_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
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
            e.OriginalItem.Source_Requester = "SPLIT";
            e.OriginalItem.Stock = e.OriginalItem.Stock * (-1);
            e.OriginalItem.Updated_on = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            DataInserterSplitter(selectedDatabase, "STOCK", e.OriginalItem, false);
            //stockItems.Add(e.OriginalItem);
            Thread.Sleep(1000);
            e.AdjustedItemA.Updated_on = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            DataInserterSplitter(selectedDatabase, "STOCK", e.AdjustedItemA, true);
            //stockItems.Add(e.AdjustedItemA);
            e.AdjustedItemB.Updated_on = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            DataInserterSplitter(selectedDatabase, "STOCK", e.AdjustedItemB, true);
            //stockItems.Add(e.AdjustedItemB);
            LoadDataIntoDataGridView1(e.OriginalItem.IPN);
        }
        private void DataInserterSplitter(string fp, string thesheetName, WHitem wHitem, bool toPrintOrNotToPrint)
        {
            bool toPrint = toPrintOrNotToPrint;
            try
            {
                // Connection string for SQL Server Express
                string connectionString = $"Data Source=RT12\\SQLEXPRESS;Initial Catalog={selectedDatabase};Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [STOCK] (IPN,Manufacturer,MFPN,Description,Stock,Updated_on,Comments,Source_Requester) values('" + wHitem.IPN + "','" + wHitem.Manufacturer + "','" + wHitem.MFPN + "','" + wHitem.Description + "','" + wHitem.Stock + "','" + wHitem.Updated_on + "','" + wHitem.Comments + "','" + wHitem.Source_Requester + "')", conn);
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
        private void printStickerSplitter(WHitem wHitem)
        {
            try
            {
                //string userName = Environment.UserName;
                //string fp = @"C:\\Users\\" + userName + "\\Desktop\\Print_Stickers.xlsx"; // //////Print_StickersWH.xlsm
                //string thesheetName = "Sheet1";
                //string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                //OleDbConnection conn = new OleDbConnection(constr);
                //OleDbCommand cmd = new OleDbCommand();
                //cmd.Connection = conn;
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "UPDATE [" + thesheetName + "$] SET PN = @PN, MFpn = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, Updated_on = @Updated_on";
                //cmd.Parameters.AddWithValue("@PN", wHitem.IPN);
                //cmd.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                //cmd.Parameters.AddWithValue("@ItemDesc", wHitem.Description);
                //cmd.Parameters.AddWithValue("@QTY", wHitem.Stock);
                //cmd.Parameters.AddWithValue("@Updated_on", wHitem.Updated_on);
                //conn.Open();
                //cmd.ExecuteNonQuery();
                //conn.Close();
                string userName = Environment.UserName;
                string fp = @"C:\\Users\\" + userName + "\\Desktop\\Print_Stickers.xlsx"; // //////Print_StickersWH.xlsm
                //string fp = @"C:\\Users\\lgt\\Desktop";
                string thesheetName = "Sheet1";
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0;HDR=YES;IMEX=0\"";
                OleDbConnection conn = new OleDbConnection(constr);
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE [" + thesheetName + "$] SET PN = @PN, MFPN = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, UPDATEDON = @Updated_on";
                cmd.Parameters.AddWithValue("@PN", wHitem.IPN);
                cmd.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                cmd.Parameters.AddWithValue("@ItemDesc", wHitem.Description);
                cmd.Parameters.AddWithValue("@QTY", wHitem.Stock);
                cmd.Parameters.AddWithValue("@Updated_on", wHitem.Updated_on);
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
                Updated_on = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Updated_on"].Index].Value.ToString(),
                Comments = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Comments"].Index].Value.ToString(),
                Source_Requester = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["Source_Requester"].Index].Value.ToString()
            };
            wHitemToSplit = whi;
        }
    }
}
