﻿using FastMember;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using ComboBox = System.Windows.Forms.ComboBox;
using DataTable = System.Data.DataTable;
using GroupBox = System.Windows.Forms.GroupBox;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;
using QRCoder;
using Point = System.Drawing.Point;
using Font = System.Drawing.Font;
using System.Diagnostics;
#pragma warning disable CS0618
namespace WH_Panel
{
    public partial class FrmStockCounter : Form
    {
        // Declare the contextMenuStrip as a class-level variable
        public ContextMenuStrip contextMenuStrip;
        public FrmStockCounter()
        {
            InitializeComponent();
            UpdateControlColors(this);
    
        }
        public class whItemStockCounter : WHitem
        {
            public int Id { get; set; }
            public string? Counted { get; set; }
            public string? User { get; set; }
        }
        List<whItemStockCounter> inWHstock { get; set; }
        int actualSum { get; set; }
        public decimal stockSum { get; set; }
        private string selectedXmlFilePath = null; // To store the user-selected XML file path
        private void StartCountingProcess()
        {
            // Step 1: Open a file dialog to select an XML file
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "XML files (*.xml)|*.xml";
                saveFileDialog.Title = "Select or Create an XML File for Counting";
                saveFileDialog.InitialDirectory = $@"\\dbr1\Data\WareHouse\STOCK_CUSTOMERS\{comboBox3.SelectedItem?.ToString()}"; // Set the folder path
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedXmlFilePath = saveFileDialog.FileName;
                    MessageBox.Show($"Counting progress will be saved to: {selectedXmlFilePath}");
                }
                else
                {
                    MessageBox.Show("No file selected. Counting cannot proceed.");
                    return;
                }
            }
        }
            private void UpdateControlColors(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                // Update control colors based on your criteria
                control.BackColor = Color.FromArgb(45, 45, 48); // Dark background
                control.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                // Handle Button controls separately
                if (control is Button button)
                {
                    if (button.Name != "btnLoadReport")
                    {
                        button.BackColor = Color.FromArgb(63, 63, 70); // Darker button background
                        button.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                        button.FlatAppearance.BorderColor = Color.FromArgb(104, 104, 104); // Change border color
                        button.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                    }
                }
                // Handle GroupBox controls separately
                if (control is GroupBox groupbox)
                {
                    groupbox.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    groupbox.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                }
                // Handle TextBox controls separately
                if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle; // Set border style to FixedSingle
                    textBox.BackColor = Color.FromArgb(30, 30, 30); // Darker background
                    textBox.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                }
                // Handle Label controls separately
                if (control is Label label)
                {
                    label.BorderStyle = BorderStyle.FixedSingle; // Set border style to FixedSingle
                    label.BackColor = Color.FromArgb(45, 45, 48); // Dark background
                    label.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                }
                // Handle TabControl controls separately
                if (control is TabControl tabControl)
                {
                    tabControl.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                                                                          // Handle each TabPage within the TabControl
                    foreach (TabPage tabPage in tabControl.TabPages)
                    {
                        tabPage.BackColor = Color.FromArgb(45, 45, 48); // Dark background
                        tabPage.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                    }
                }
                // Handle DataGridView controls separately
                if (control is DataGridView dataGridView)
                {
                    // Update DataGridView styles
                    dataGridView.EnableHeadersVisualStyles = false;
                    dataGridView.BackgroundColor = Color.FromArgb(55, 55, 55); // Darker background
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(55, 55, 55); // Dark background
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                    dataGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(55, 55, 55); // Dark background
                    dataGridView.DefaultCellStyle.BackColor = Color.FromArgb(55, 55, 55); // Darker background
                    dataGridView.DefaultCellStyle.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                    dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255); // Selection background
                    dataGridView.DefaultCellStyle.SelectionForeColor = Color.FromArgb(241, 241, 241); // Selection foreground
                                                                                                      // Change the header cell styles for each column
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.HeaderCell.Style.BackColor = Color.FromArgb(55, 55, 55); // Dark background
                        column.HeaderCell.Style.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                    }
                }
                // Handle ComboBox controls separately
                if (control is ComboBox comboBox)
                {
                    comboBox.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    comboBox.BackColor = Color.FromArgb(30, 30, 30); // Darker background
                    comboBox.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                }
                // Handle DateTimePicker controls separately
                if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.BackColor = Color.FromArgb(30, 30, 30); // Darker background
                    dateTimePicker.ForeColor = Color.FromArgb(241, 241, 241); // Light foreground
                }
                // Recursively update controls within containers
                if (control.Controls.Count > 0)
                {
                    UpdateControlColors(control);
                }
            }
        }
        private void FrmStockCounter_Load(object sender, EventArgs e)
        {
            // Create a SoundPlayer instance
            // Create a SoundPlayer instance and use the resource stream
            SoundPlayer player = new SoundPlayer(Properties.Resources.fromStockCounterLoad);
            try
            {
                // Load and play the audio file
                player.Load();
                player.Play(); // Use PlayLooping() if you want to loop the sound
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while playing the audio: " + ex.Message);
            }
            textBox1.Focus();
            comboBox1.SelectedIndex = 5;
            lblCalc.BackColor = Color.IndianRed;
            //StartCountingProcess();
        }
        List<ClientWarehouse> warehouses { get; set; }
        string selectedWHconnstring { get; set; }
        public async void InitializeGlobalWarehouses(List<ClientWarehouse> warehousesFromTheMain)
        {
            // Create and show the loading panel
            Panel loadingPanel = new Panel
            {
                Size = new Size(700, 200),
                BackColor = Color.Yellow,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - 700) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - 200) / 2)
            };
            Label loadingLabel = new Label
            {
                Text = "Loading data, please wait......טוען בסיסי נתונים , נא להמתין",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 16, FontStyle.Bold)
            };
            loadingPanel.Controls.Add(loadingLabel);
            this.Controls.Add(loadingPanel);
            loadingPanel.BringToFront();
            loadingPanel.Show();
            loadingPanel.Refresh();
            try
            {
                await Task.Run(() =>
                {
                    warehouses = new List<ClientWarehouse>();
                    foreach (ClientWarehouse warehouse in warehousesFromTheMain)
                    {
                        if (HasCountTable(warehouse.clName))
                        {
                            warehouses.Add(warehouse);
                        }
                    }
                    // Ordering the warehouses list by clName
                    warehouses = warehouses.OrderBy(warehouse => warehouse.clName).ToList();
                });
                // Adding clNames to comboBox4
                foreach (ClientWarehouse warehouse in warehouses)
                {
                    comboBox3.Items.Add(warehouse.clName);
                    GroupBox groupBox = new GroupBox
                    {
                        Name = warehouse.clName,
                        Text = warehouse.clName,
                        Width = 140,
                        Height = 130
                    };
                }
                comboBox3.SelectedItem = "ARAN";
            }
            finally
            {
                // Hide the loading panel
                loadingPanel.Hide();
                this.Controls.Remove(loadingPanel);
            }
        }
        private bool HasCountTable(string databaseName)
        {
            string connectionString = "Data Source=DBR3\\SQLEXPRESS;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT COUNT(*) FROM [{databaseName}].INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'COUNT'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox2.Focus(); // Switches focus to textBox2
                e.Handled = true;  // Mark event as handled
                e.SuppressKeyPress = true; // Suppress the Enter key sound
                textBox2.Clear();
                textBox3.Clear();
                getDataFromStock();
                preRecalc();
            }
        }
        private void getDataFromStock()
        {
            // Get IPN from textBox1
            string ipn = textBox1.Text;
            // Get the selected table name from comboBox3
            string selectedTable = comboBox3.SelectedItem?.ToString();
            // Retrieve the connection string for the selected warehouse
            foreach (ClientWarehouse w in warehouses)
            {
                if (w.clName == selectedTable)
                {
                    selectedWHconnstring = w.sqlStock;
                    break;
                }
            }
            // Ensure the selected table and IPN are not null or empty
            if (string.IsNullOrEmpty(selectedTable) || string.IsNullOrEmpty(ipn))
            {
                MessageBox.Show("Please enter a valid IPN and select a table.");
                return;
            }
            // SQL query string with the table name wrapped in square brackets
            string query = $"SELECT * FROM [{selectedTable}].dbo.STOCK WHERE IPN = @IPN ORDER BY UPDATED_ON DESC";
            // Connection string for the selected warehouse
            string connectionString = selectedWHconnstring;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Create SQL command with parameterized query to prevent SQL injection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add the IPN parameter to the query
                        command.Parameters.AddWithValue("@IPN", ipn);
                        // Fill a DataTable with the query result
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            // Bind the DataTable to the DataGridView
                            dataGridView1.DataSource = dataTable;
                            // Adjust column width based on contents
                            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            // Ensure the DataTable has the "Counted" and "User" columns
                            if (!dataTable.Columns.Contains("Counted"))
                            {
                                dataTable.Columns.Add("Counted", typeof(string)); // Add "Counted" column
                            }
                            if (!dataTable.Columns.Contains("User"))
                            {
                                dataTable.Columns.Add("User", typeof(string)); // Add "User" column
                            }
                            //// Merge the data from the XML log with the DataTable
                            //if (!string.IsNullOrEmpty(selectedXmlFilePath))
                            //{
                            //    MergeXmlData(dataTable);
                            //}
                            MergeCountDataFromSQL(dataTable);
                            // After the while loop in MergeCountDataFromSQL
                            dataTable.AcceptChanges(); // Optional: Mark all rows as unchanged
                            dataGridView1.DataSource = null; // Reset the DataSource
                            dataGridView1.DataSource = dataTable; // Rebind the updated DataTable
                            dataGridView1.Refresh(); // Refresh the DataGridView
                            // Update GroupBox text with the IPN and stock sum after loading data
                            UpdateGroupBoxText(ipn, dataTable);
                            // Check if checkBox1 is checked to filter for in-stock items
                            if (checkBox1.Checked)
                            {
                                showOnlyInStock();
                            }
                            // Set the column order for STOCK view
                            SetSTOCKiewColumsOrder();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Display SQL exception message
                    MessageBox.Show("An error occurred while querying the database: " + ex.Message);
                }
            }
        }
        private void MergeCountDataFromSQL(DataTable dataTable)
        {
            // Step 1: Define your SQL connection string
            string connectionString = selectedWHconnstring; // Update with your actual connection string
            // Create a list of Ids to use in the query
            var idList = dataTable.Rows.Cast<DataRow>()
                .Where(row => row["Id"] != DBNull.Value) // Ensure Id is not null
                .Select(row => (int)row["Id"]) // Cast to int
                .Distinct()
                .ToList();
            // Check if there are any Ids to query
            if (idList.Count == 0)
            {
                MessageBox.Show("No valid IDs found to merge data.");
                return;
            }
            // Step 2: Create a SQL query to retrieve Counted and User data from the COUNT table
            string query = "SELECT Id, Counted, [User] FROM [COUNT] WHERE Id IN (" + string.Join(",", idList) + ")";
            // Step 3: Use a using statement for the SQL connection and command
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Step 4: Create a command to execute the query
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Step 5: Open the connection and execute the command
                    try
                    {
                        connection.Open();
                        // Execute the command and read the data
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Step 6: Loop through each row in the DataTable from the database
                            while (reader.Read())
                            {
                                int stockId = (int)reader["Id"]; // Cast to int directly
                                string countedValue = reader["Counted"]?.ToString();
                                string userValue = reader["User"]?.ToString();
                                // Find the matching row in the dataTable by Id
                                DataRow[] matchingRows = dataTable.Select($"Id = {stockId}");
                                if (matchingRows.Length > 0)
                                {
                                    // If a match is found, update the Counted and User fields
                                    matchingRows[0]["Counted"] = countedValue;
                                    matchingRows[0]["User"] = userValue;
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"An error occurred while accessing the database: {ex.Message}");
                    }
                }
            }
        }
        private void MergeXmlData(DataTable dataTable)
        {
            // Load the XML file
            DataSet xmlDataSet = new DataSet();
            // Try loading the XML data
            try
            {
                xmlDataSet.ReadXml(selectedXmlFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading XML file: {ex.Message}");
                return;
            }
            // Check if the XML file contains any tables
            if (xmlDataSet.Tables.Count == 0 || xmlDataSet.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("The XML file is empty or doesn't contain the expected data.");
                return;
            }
            // Assuming the XML structure has Id, Counted, and User columns
            DataTable xmlTable = xmlDataSet.Tables[0]; // This assumes your XML file has at least one table
            // Loop through each row in the DataTable from the database
            foreach (DataRow row in dataTable.Rows)
            {
                string stockId = row["Id"].ToString();
                // Find the matching row in the XML data by Id
                DataRow[] xmlRows = xmlTable.Select($"Id = '{stockId}'");
                if (xmlRows.Length > 0)
                {
                    // If a match is found, update the Counted and User fields
                    row["Counted"] = xmlRows[0]["Counted"];
                    row["User"] = xmlRows[0]["User"];
                }
            }
        }
        private void SetSTOCKiewColumsOrder()
        {
            // Set the auto size mode for columns
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            // Set the display index for specific columns
            dataGridView1.Columns["IPN"].DisplayIndex = 0;
            dataGridView1.Columns["Manufacturer"].DisplayIndex = 1;
            dataGridView1.Columns["MFPN"].DisplayIndex = 2;
            dataGridView1.Columns["Description"].DisplayIndex = 3;
            dataGridView1.Columns["Stock"].DisplayIndex = 4;
            dataGridView1.Columns["Updated_on"].DisplayIndex = 5;
            dataGridView1.Columns["Comments"].DisplayIndex = 6;
            dataGridView1.Columns["Source_Requester"].DisplayIndex = 7;
            dataGridView1.Columns["Id"].DisplayIndex = 8;
            dataGridView1.Columns["Counted"].DisplayIndex = 9;
            dataGridView1.Columns["User"].DisplayIndex = 10;
            //// Sort by Updated_on column
            //dataGridView1.Sort(dataGridView1.Columns["Updated_on"], ListSortDirection.Descending);
            // Check if DataSource is DataTable before sorting
            if (dataGridView1.DataSource is DataTable dataTable)
            {
                // Sort by Updated_on column
                dataTable.DefaultView.Sort = "Updated_on DESC"; // This applies sorting to the DefaultView
            }
            // Step to color the rows based on the "User" column
            ColorRowsBasedOnUserColumn();
            lblCalc.Text = actualSum + "/" + stockSum + " ( "+  (stockSum-actualSum) +" )";
            if (actualSum == stockSum)
            {
                lblCalc.BackColor = Color.DarkGreen;
            }
            else if (actualSum < stockSum)
            {
                lblCalc.BackColor = Color.IndianRed;
            }
            else if (actualSum > stockSum)
            {
                lblCalc.BackColor = Color.YellowGreen;
            }
        }
        private void ColorRowsBasedOnUserColumn()
        {
            actualSum = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue; // Skip the new row placeholder
                // Check if the "User" column has a value
                string userValue = row.Cells["User"].Value?.ToString();
                if (!string.IsNullOrEmpty(userValue))
                {
                    actualSum += int.Parse(row.Cells["Stock"].Value?.ToString());
                    // Set the entire row's background color to LightGreen
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        // Method to update GroupBox text with IPN and current stock balance
        private void UpdateGroupBoxText(string ipn, DataTable dataTable)
        {
            // Assuming the Stock column is named "Stock"
            stockSum = 0;
            // Calculate the sum of the stock values
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Stock"] != DBNull.Value)
                {
                    stockSum += Convert.ToDecimal(row["Stock"]);
                }
            }
            // Update the GroupBox text with IPN and stock sum
            groupBox5.Text = $"IPN: {ipn}, Total Stock Balance: {stockSum}";
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox3.Focus(); // Switches focus to textBox2
                e.Handled = true;  // Mark event as handled
                e.SuppressKeyPress = true; // Suppress the Enter key sound
            }
        }
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox1.Focus(); // Switches focus to textBox2
                e.Handled = true;  // Mark event as handled
                e.SuppressKeyPress = true; // Suppress the Enter key sound
                countLogic();
            }
        }
        private void countLogic()
        {
            // Step 1: Ensure XML file path is set
            //if (string.IsNullOrEmpty(selectedXmlFilePath))
            //{
            //    MessageBox.Show("Please start the counting process by selecting an XML file first.");
            //    return;
            //}
            // Step 2: Collect data from UI
            string ipn = textBox1.Text; // IPN from textbox1
            string mfpn = textBox2.Text; // MFPN from textbox2
            int stock;
            bool isStockValid = int.TryParse(textBox3.Text, out stock); // Stock from textbox3
            string comments = comboBox1.SelectedItem?.ToString() ?? string.Empty; // Comments from combobox1
            string currentUser = Environment.UserName; // Get current Windows user
            // Validate input fields
            if (string.IsNullOrEmpty(ipn) || string.IsNullOrEmpty(mfpn) || !isStockValid)
            {
                MessageBox.Show("Please fill out all the fields correctly.");
                return;
            }
            bool alreadyCounted = false;
            List<DataGridViewRow> matchingRows = new List<DataGridViewRow>();
            // First, check how many rows have the same IPN, MFPN, and Stock values
            var similarRows = dataGridView1.Rows.Cast<DataGridViewRow>()
                               .Where(row => !row.IsNewRow && row.Cells["IPN"].Value?.ToString() == ipn
                                             && row.Cells["MFPN"].Value?.ToString() == mfpn
                                             && Convert.ToInt32(row.Cells["Stock"].Value) == stock)
                               .ToList();
            // Loop through the similar rows
            foreach (var row in similarRows)
            {
                string user = row.Cells["User"].Value?.ToString();
                // If User is empty, the item hasn't been counted
                if (string.IsNullOrEmpty(user))
                {
                    matchingRows.Add(row); // Add to the list for further processing
                }
                else
                {
                    // Mark as already counted, but only show the message if all similar rows have been counted
                    alreadyCounted = true;
                }
            }
            // Show the message only if all similar rows have been counted
            if (alreadyCounted && matchingRows.Count == 0)
            {
                MessageBox.Show("Already counted on " + similarRows[0].Cells["Counted"].Value?.ToString() + " by " + similarRows[0].Cells["User"].Value?.ToString(),
                                "Already counted !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            // If no rows are found for counting, display a message
            else if (matchingRows.Count == 0 && !alreadyCounted)
            {
                MessageBox.Show("No items found that haven't been counted yet.", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (matchingRows.Count == 1)
            {
                // Only one match, update it directly
                string rowComments = matchingRows[0].Cells["Comments"].Value?.ToString();
                if (rowComments == comments)
                {
                    // Update the matched row
                    UpdateRow(matchingRows[0], currentUser);
                }
                else
                {
                    // Prompt user to select correct comments
                    MessageBox.Show("Selected package not found!");
                    comboBox1.Focus();
                    comboBox1.DroppedDown = true;
                }
            }
            else
            {
                // Step 5: Multiple matches, prompt user to select correct one by showing dynamic form
                ShowMatchSelectionForm(matchingRows);
            }
        }
        //private void SaveCountedItemsToDatabase(whItemStockCounter countedItem)
        //{
        //    // Step 1: Define your SQL connection string
        //    string connectionString = selectedWHconnstring; // Update with your actual connection string
        //    string selectedTable = comboBox3.SelectedItem?.ToString();
        //    // Step 2: Create an SQL INSERT statement
        //    string insertQuery = $@"
        //INSERT INTO [{selectedTable}].dbo.COUNT (IPN, MFPN, Stock, Comments, Id, Counted, [User])
        //VALUES (@IPN, @MFPN, @Stock, @Comments, @Id, @Counted, @User)";
        //    // Step 3: Use a using statement for the SQL connection and command
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        // Step 4: Create a command to execute the query
        //        using (SqlCommand command = new SqlCommand(insertQuery, connection))
        //        {
        //            // Step 5: Add parameters to prevent SQL injection
        //            command.Parameters.AddWithValue("@IPN", countedItem.IPN);
        //            command.Parameters.AddWithValue("@MFPN", countedItem.MFPN);
        //            command.Parameters.AddWithValue("@Stock", countedItem.Stock);
        //            command.Parameters.AddWithValue("@Comments", countedItem.Comments);
        //            command.Parameters.AddWithValue("@Id", countedItem.Id);
        //            command.Parameters.AddWithValue("@Counted", countedItem.Counted);
        //            command.Parameters.AddWithValue("@User", countedItem.User);
        //            // Step 6: Open the connection and execute the command
        //            try
        //            {
        //                connection.Open();
        //                command.ExecuteNonQuery();
        //                ShowMessageWithAutoClose($"Counting progress has been saved to the database.", 2000); // 2000 milliseconds = 2 seconds
        //            }
        //            catch (SqlException ex)
        //            {
        //                MessageBox.Show($"An error occurred while saving to the database: {ex.Message}");
        //            }
        //        }
        //    }
        //}
        private void SaveCountedItemsToDatabase(whItemStockCounter countedItem)
        {
            // Step 1: Define your SQL connection string
            string connectionString = selectedWHconnstring; // Update with your actual connection string
            string selectedTable = comboBox3.SelectedItem?.ToString();
            // Step 2: Create an SQL INSERT statement
            string insertQuery = $@"
        INSERT INTO [{selectedTable}].dbo.COUNT 
        (IPN, MFPN, Stock, Comments, Id, Counted, [User], Manufacturer, Description, Updated_on, Source_Requester)
        VALUES 
        (@IPN, @MFPN, @Stock, @Comments, @Id, @Counted, @User, @Manufacturer, @Description, @Updated_on, @Source_Requester)";
            // Step 3: Use a using statement for the SQL connection and command
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Step 4: Create a command to execute the query
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Step 5: Add parameters to prevent SQL injection and handle nulls
                    command.Parameters.AddWithValue("@IPN", (object)countedItem.IPN ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MFPN", (object)countedItem.MFPN ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Stock", countedItem.Stock); // Assuming Stock is an int, it can't be null
                    command.Parameters.AddWithValue("@Comments", (object)countedItem.Comments ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Id", countedItem.Id);
                    command.Parameters.AddWithValue("@Counted", (object)countedItem.Counted ?? DBNull.Value);
                    command.Parameters.AddWithValue("@User", (object)countedItem.User ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Manufacturer", (object)countedItem.Manufacturer ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Description", (object)countedItem.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Updated_on", (object)countedItem.Updated_on ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Source_Requester", (object)countedItem.Source_Requester ?? DBNull.Value);
                    // Step 6: Open the connection and execute the command
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        ShowMessageWithAutoClose($"Counting progress has been saved to the database.", 2000); // 2000 milliseconds = 2 seconds
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"An error occurred while saving to the database: {ex.Message}");
                    }
                }
            }
        }
        private void SaveCountedItemsToSelectedFile(whItemStockCounter countedItem)
        {
            // Step 1: Ensure we have a valid XML file path
            if (string.IsNullOrEmpty(selectedXmlFilePath))
            {
                MessageBox.Show("No file selected to save the counted items.");
                return;
            }
            // Step 2: Load the existing XML document if it exists
            XDocument xdoc;
            if (File.Exists(selectedXmlFilePath))
            {
                xdoc = XDocument.Load(selectedXmlFilePath);
            }
            else
            {
                // Create a new XML document if the file doesn't exist
                xdoc = new XDocument(new XElement("ArrayOfWhItemStockCounter"));
            }
            // Step 3: Create a new XElement for the counted item
            XElement newItem = new XElement("whItemStockCounter",
                new XElement("IPN", countedItem.IPN),
                new XElement("MFPN", countedItem.MFPN),
                new XElement("Stock", countedItem.Stock),
                new XElement("Comments", countedItem.Comments),
                new XElement("Id", countedItem.Id),
                new XElement("Counted", countedItem.Counted),
                new XElement("User", countedItem.User)
            );
            // Step 4: Add the new item to the existing root
            xdoc.Root.Add(newItem);
            // Step 5: Save the document back to the XML file
            xdoc.Save(selectedXmlFilePath);
            //MessageBox.Show($"Counting progress has been saved to {selectedXmlFilePath}");
            ShowMessageWithAutoClose($"Counting progress has been saved to {selectedXmlFilePath}", 2000); // 2000 milliseconds = 2 seconds
        }
        private void ShowMessageWithAutoClose(string message, int delay)
        {
            // Create a new form to display the message
            Form messageForm = new Form()
            {
                StartPosition = FormStartPosition.CenterScreen,
                Size = new Size(600, 200),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false
            };
            Label messageLabel = new Label()
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightGreen
            };
            messageForm.Controls.Add(messageLabel);
            messageForm.Show();
            // Create a timer to close the message form after the specified delay
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = delay; // Set the interval to the delay time
            timer.Tick += (s, e) =>
            {
                messageForm.Close(); // Close the message form
                timer.Stop(); // Stop the timer
            };
            timer.Start();
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
            textBox1.SelectAll();
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private static void txtbColorWhiteOnLeave(object sender)
        {
            TextBox? tb = sender as TextBox;
            tb.BackColor = Color.FromArgb(55, 55, 55);
        }
        private static void txtbColorGreenOnEnter(object sender)
        {
            TextBox? tb = (TextBox)sender;
            tb.BackColor = Color.DarkGreen;
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void textBox3_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox1.Focus();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                showOnlyInStock();
            }
            else
            {
                dataGridView1.DataSource = inWHstock; // Ensure inWHstock is a DataTable
                SetSTOCKiewColumsOrder();
            }
            // Set the column order and potentially sort again
            // Add sorting logic if you want to sort the DataGridView after changing the data source
            if (dataGridView1.DataSource is DataTable)
            {
                var dataTable = (DataTable)dataGridView1.DataSource;
                dataTable.DefaultView.Sort = "Updated_on DESC"; // Adjust to your desired sort column
            }
        }
        public void showOnlyInStock()
        {
            inWHstock = new List<whItemStockCounter>();
            // Populate inWHstock with non-null values
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].IsNewRow) continue; // Skip the new row placeholder
                // Read the stock value, ensuring we handle nulls
                int res = 0;
                bool stk = int.TryParse(dataGridView1.Rows[i].Cells[dataGridView1.Columns["Stock"].Index]?.Value?.ToString(), out res);
                int toStk = stk ? res : 0;
                // Create whItemStockCounter instance, handling potential nulls
                whItemStockCounter wHitemABC = new whItemStockCounter()
                {
                    IPN = dataGridView1.Rows[i].Cells[dataGridView1.Columns["IPN"].Index]?.Value?.ToString() ?? string.Empty,
                    Manufacturer = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Manufacturer"].Index]?.Value?.ToString() ?? string.Empty,
                    MFPN = dataGridView1.Rows[i].Cells[dataGridView1.Columns["MFPN"].Index]?.Value?.ToString() ?? string.Empty,
                    Description = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Description"].Index]?.Value?.ToString() ?? string.Empty,
                    Stock = toStk,
                    Updated_on = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Updated_on"].Index]?.Value?.ToString() ?? string.Empty,
                    Comments = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Comments"].Index]?.Value?.ToString() ?? string.Empty,
                    Source_Requester = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Source_Requester"].Index]?.Value?.ToString() ?? string.Empty,
                    Id = int.Parse(dataGridView1.Rows[i].Cells[dataGridView1.Columns["Id"].Index]?.Value?.ToString() ?? "0"),
                    Counted = dataGridView1.Rows[i].Cells[dataGridView1.Columns["Counted"].Index]?.Value?.ToString() ?? string.Empty,
                    User = dataGridView1.Rows[i].Cells[dataGridView1.Columns["User"].Index]?.Value?.ToString() ?? string.Empty
                };
                inWHstock.Add(wHitemABC);
            }
            // Filter out negative stock quantities
            List<whItemStockCounter> negativeQTYs = inWHstock.Where(item => item.Stock < 0).ToList();
            List<whItemStockCounter> positiveInWH = inWHstock.Where(item => item.Stock > 0).ToList();
            foreach (var negativeItem in negativeQTYs)
            {
                // Find the oldest matching positive item (same absolute stock value, ordered by Updated_on)
                var matchingPositive = positiveInWH
                    .Where(positiveItem => Math.Abs(negativeItem.Stock) == positiveItem.Stock)
                    .OrderBy(positiveItem => positiveItem.Updated_on) // Order by Updated_on ascending (oldest first)
                    .FirstOrDefault();
                // If a matching positive item is found, remove it from the positive list
                if (matchingPositive != null)
                {
                    positiveInWH.Remove(matchingPositive);
                }
            }
            // Load data into DataTable for DataGridView
            DataTable INWH = new DataTable();
            using (var reader = ObjectReader.Create(positiveInWH))
            {
                INWH.Load(reader);
            }
            // Remove the existing "Counted" column from DataGridView before re-binding
            if (dataGridView1.Columns.Contains("Counted"))
            {
                dataGridView1.Columns.Remove("Counted");
            }
            if (dataGridView1.Columns.Contains("User"))
            {
                dataGridView1.Columns.Remove("User");
            }
            // Bind the DataTable to the DataGridView
            DataView dv = INWH.DefaultView;
            dataGridView1.DataSource = dv;
            dv.Sort = "Updated_on DESC";
            // Ensure the "Counted" column is added again if it's needed
            if (!dataGridView1.Columns.Contains("Counted"))
            {
                dataGridView1.Columns.Add("Counted", "Counted");
            }
            if (!dataGridView1.Columns.Contains("User"))
            {
                dataGridView1.Columns.Add("User", "User");
            }
            // Set the correct column order for STOCK view
            SetSTOCKiewColumsOrder();
        }
      

        private async void generateReport()
        {
            // Get the current warehouse name from comboBox3
            string currentWarehouseName = comboBox3.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(currentWarehouseName))
            {
                MessageBox.Show("Please select a warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Define the SQL query
            string query = $@"
        DECLARE @DatabaseName NVARCHAR(100) = '{currentWarehouseName}';
        DECLARE @SQL NVARCHAR(MAX);
        SET @SQL = '
        SELECT 
            COALESCE(s.IPN, c.IPN) AS IPN,
            ISNULL(s.TotalStock, 0) AS TotalStock,
            ISNULL(c.TotalCount, 0) AS TotalCount
        FROM 
            (SELECT IPN, SUM(CAST(stock AS FLOAT)) AS TotalStock
             FROM [' + @DatabaseName + '].dbo.STOCK
             GROUP BY IPN) s
        FULL OUTER JOIN 
            (SELECT IPN, SUM(stock) AS TotalCount
             FROM [' + @DatabaseName + '].dbo.COUNT
             GROUP BY IPN) c
        ON s.IPN = c.IPN
        WHERE 
            ISNULL(s.TotalStock, 0) <> ISNULL(c.TotalCount, 0)
        ORDER BY 
            COALESCE(s.IPN, c.IPN);';
        EXEC sp_executesql @SQL;";
            // Execute the SQL query and get the data
            DataTable dataTable = await ExecuteSqlQueryAsync(query, currentWarehouseName);
            // Generate the HTML content from the data
            string htmlContent = GenerateHtmlReport(dataTable, $"{currentWarehouseName} Stock Report");
            // Save the HTML content to a file
            string fileName = $"{currentWarehouseName}_StockReport_{DateTime.Now:yyyyMMddHHmm}.html";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            File.WriteAllText(filePath, htmlContent);
            // Open the HTML file in the default browser
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
        private async Task<DataTable> ExecuteSqlQueryAsync(string query, string databaseName)
        {
            DataTable dataTable = new DataTable();
            string connectionString = $"Data Source=DBR3\\SQLEXPRESS;Initial Catalog={databaseName};Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while executing the SQL query: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return dataTable;
        }
        private string GenerateHtmlReport(DataTable dataTable, string reportTitle)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html style='text-align:center;background-color:gray;color:white;'>");
            html.AppendLine("<head>");
            html.AppendLine("<title>Stock Report</title>");
            html.AppendLine("<style>");
            html.AppendLine("table { border-collapse: collapse; width: 100%; border: solid 1px; }");
            html.AppendLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
            html.AppendLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
            html.AppendLine(".green { background-color: lightgreen; color: black; }");
            html.AppendLine(".zero { background-color: indianred; color: white; }");
            html.AppendLine(".negative { background-color: red; color: white; }");
            html.AppendLine(".header-table td { font-size: 2em; font-weight: bold; }");
            html.AppendLine("</style>");
            html.AppendLine("<script>");
            html.AppendLine("function filterTable() {");
            html.AppendLine("  var input, filter, table, tr, td, i, j, txtValue;");
            html.AppendLine("  input = document.getElementById('searchInput');");
            html.AppendLine("  filter = input.value.toLowerCase();");
            html.AppendLine("  table = document.getElementById('stockTable');");
            html.AppendLine("  tr = table.getElementsByTagName('tr');");
            html.AppendLine("  for (i = 1; i < tr.length; i++) {");
            html.AppendLine("    tr[i].style.display = 'none';");
            html.AppendLine("    td = tr[i].getElementsByTagName('td');");
            html.AppendLine("    for (j = 0; j < td.length; j++) {");
            html.AppendLine("      if (td[j]) {");
            html.AppendLine("        txtValue = td[j].textContent || td[j].innerText;");
            html.AppendLine("        if (txtValue.toLowerCase().indexOf(filter) > -1) {");
            html.AppendLine("          tr[i].style.display = '';");
            html.AppendLine("          break;");
            html.AppendLine("        }");
            html.AppendLine("      }");
            html.AppendLine("    }");
            html.AppendLine("  }");
            html.AppendLine("}");
            html.AppendLine("function clearSearch() {");
            html.AppendLine("  document.getElementById('searchInput').value = '';");
            html.AppendLine("  filterTable();");
            html.AppendLine("}");
            html.AppendLine("function sortTable(n, isNumeric) {");
            html.AppendLine("  var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;");
            html.AppendLine("  table = document.getElementById('stockTable');");
            html.AppendLine("  switching = true;");
            html.AppendLine("  dir = 'asc';");
            html.AppendLine("  while (switching) {");
            html.AppendLine("    switching = false;");
            html.AppendLine("    rows = table.rows;");
            html.AppendLine("    for (i = 1; i < (rows.length - 1); i++) {");
            html.AppendLine("      shouldSwitch = false;");
            html.AppendLine("      x = rows[i].getElementsByTagName('TD')[n];");
            html.AppendLine("      y = rows[i + 1].getElementsByTagName('TD')[n];");
            html.AppendLine("      if (isNumeric) {");
            html.AppendLine("        if (dir == 'asc') {");
            html.AppendLine("          if (parseFloat(x.innerHTML) > parseFloat(y.innerHTML)) {");
            html.AppendLine("            shouldSwitch = true;");
            html.AppendLine("            break;");
            html.AppendLine("          }");
            html.AppendLine("        } else if (dir == 'desc') {");
            html.AppendLine("          if (parseFloat(x.innerHTML) < parseFloat(y.innerHTML)) {");
            html.AppendLine("            shouldSwitch = true;");
            html.AppendLine("            break;");
            html.AppendLine("          }");
            html.AppendLine("        }");
            html.AppendLine("      } else {");
            html.AppendLine("        if (dir == 'asc') {");
            html.AppendLine("          if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {");
            html.AppendLine("            shouldSwitch = true;");
            html.AppendLine("            break;");
            html.AppendLine("          }");
            html.AppendLine("        } else if (dir == 'desc') {");
            html.AppendLine("          if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {");
            html.AppendLine("            shouldSwitch = true;");
            html.AppendLine("            break;");
            html.AppendLine("          }");
            html.AppendLine("        }");
            html.AppendLine("      }");
            html.AppendLine("    }");
            html.AppendLine("    if (shouldSwitch) {");
            html.AppendLine("      rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);");
            html.AppendLine("      switching = true;");
            html.AppendLine("      switchcount ++;");
            html.AppendLine("    } else {");
            html.AppendLine("      if (switchcount == 0 && dir == 'asc') {");
            html.AppendLine("        dir = 'desc';");
            html.AppendLine("        switching = true;");
            html.AppendLine("      }");
            html.AppendLine("    }");
            html.AppendLine("  }");
            html.AppendLine("}");
            html.AppendLine("</script>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine($"<h1>{reportTitle}</h1>");
            html.AppendLine($"<div>Displaying {dataTable.Rows.Count} rows</div>");
            html.AppendLine("<input type='text' id='searchInput' onkeyup='filterTable()' placeholder='Search for keywords..' style='margin-bottom: 10px;'>");
            html.AppendLine("<button onclick='clearSearch()'>Clear</button>");
            html.AppendLine("<table id='stockTable'>");
            html.AppendLine("<tr>");
            // Add table headers
            foreach (DataColumn column in dataTable.Columns)
            {
                bool isNumeric = column.ColumnName == "TotalStock" || column.ColumnName == "TotalCount";
                html.AppendLine($"<th onclick='sortTable({dataTable.Columns.IndexOf(column)}, {isNumeric.ToString().ToLower()})'>{column.ColumnName}</th>");
            }
            html.AppendLine("</tr>");
            // Add table rows
            foreach (DataRow row in dataTable.Rows)
            {
                html.AppendLine("<tr>");
                foreach (DataColumn column in dataTable.Columns)
                {
                    string cellValue = row[column]?.ToString() ?? string.Empty;
                    string cellClass = string.Empty;
                    if (column.ColumnName == "TotalStock" || column.ColumnName == "TotalCount")
                    {
                        if (decimal.TryParse(cellValue, out decimal value))
                        {
                            if (value > 0)
                            {
                                cellClass = "green";
                            }
                            else if (value == 0)
                            {
                                cellClass = "zero";
                            }
                            else
                            {
                                cellClass = "negative";
                            }
                        }
                    }
                    html.AppendLine($"<td class='{cellClass}'>{cellValue}</td>");
                }
                html.AppendLine("</tr>");
            }
            html.AppendLine("</table>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
               generateReport();
            }
            else if (e.Button == MouseButtons.Right)
            {
                displayCOUNTdatabase();
            }
        }

        private async void displayCOUNTdatabase()
        {
            string currentWarehouseName = comboBox3.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(currentWarehouseName))
            {
                MessageBox.Show("Please select a warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Define the SQL query
            string query = $@"
    SELECT * 
    FROM [{currentWarehouseName}].dbo.COUNT";

            try
            {
                // Execute the SQL query and get the data
                DataTable dataTable = await ExecuteSqlQueryAsync(query, currentWarehouseName);

                // Generate the HTML content from the data
                string htmlContent = GenerateHtmlReport(dataTable, $"{currentWarehouseName} COUNT Report");

                // Save the HTML content to a file
                string fileName = $"{currentWarehouseName}_COUNT_Report_{DateTime.Now:yyyyMMddHHmm}.html";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
                File.WriteAllText(filePath, htmlContent);

                // Open the HTML file in the default browser
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ShowMatchSelectionForm(List<DataGridViewRow> matchingRows)
        {
            int heightMultiplier = 160;
            // Create a new form dynamically
            Form selectionForm = new Form
            {
                Text = "Select a Matching Item",
                Size = new Size(1200, matchingRows.Count * heightMultiplier),
                StartPosition = FormStartPosition.CenterParent
            };
            // Create a Panel to enable scrolling
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true // Enable vertical scroll if content overflows
            };
            // Create a DataGridView dynamically
            DataGridView dataGridView = new DataGridView
            {
                Dock = DockStyle.Top,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Height = (matchingRows.Count) * heightMultiplier
            };
            // Set the height of the rows to 70 pixels
            dataGridView.RowTemplate.Height = 90;
            // Create columns for DataGridView
            dataGridView.Columns.Add("IPN", "IPN");
            dataGridView.Columns.Add("MFPN", "MFPN");
            dataGridView.Columns.Add("Stock", "Stock");
            dataGridView.Columns.Add("Comments", "Comments");
            // Create a column specifically for the QR code image
            DataGridViewImageColumn qrCodeColumn = new DataGridViewImageColumn
            {
                Name = "QRCode",
                HeaderText = "QR Code"
            };
            dataGridView.Columns.Add(qrCodeColumn);
            dataGridView.Columns.Add("Updated_on", "Updated On");
            // Sort matching rows by Updated_on date
            matchingRows = matchingRows.OrderBy(row => DateTime.Parse(row.Cells["Updated_on"].Value.ToString())).ToList();
            // Populate DataGridView with matching rows
            foreach (var row in matchingRows)
            {
                // Generate the QR code for the Id
                string idValue = row.Cells["Id"].Value.ToString();
                byte[] qrCodeImage = GenerateQrCodeImage(idValue);
                // Create an Image object from the byte array
                using (var ms = new MemoryStream(qrCodeImage))
                {
                    Image qrCodeBitmap = Image.FromStream(ms);
                    // Add a new row to the DataGridView, including the QR code as an image
                    int rowIndex = dataGridView.Rows.Add(
                        row.Cells["IPN"].Value.ToString(),
                        row.Cells["MFPN"].Value.ToString(),
                        row.Cells["Stock"].Value.ToString(),
                        row.Cells["Comments"].Value.ToString(),
                        qrCodeBitmap, // Set the QR code image in the image column
                        row.Cells["Updated_on"].Value.ToString()
                    );
                    // Set the ToolTipText for the QR Code image cell to display the Id
                    dataGridView.Rows[rowIndex].Cells["QRCode"].ToolTipText = idValue;
                }
            }
            // Create a TextBox for scanning input
            TextBox scanTextBox = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = HorizontalAlignment.Center,
                PlaceholderText = "Scan or enter ID here..."
            };
            // Handle the scanTextBox ENTER key event for scanning
            scanTextBox.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    // Get the scanned Id from the scanTextBox
                    string scannedId = scanTextBox.Text;
                    // Find the row that matches the scanned Id in the DataGridView
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (row.Cells["QRCode"].ToolTipText == scannedId) // Compare with the tooltip (Id)
                        {
                            row.Selected = true; // Select the row that matches
                            // Simulate the same behavior as ENTER on the selected row
                            int selectedIndex = row.Index;
                            if (selectedIndex >= 0 && selectedIndex < matchingRows.Count)
                            {
                                DataGridViewRow selectedRow = matchingRows[selectedIndex];
                                // Update the selected row
                                string currentUser = Environment.UserName; // Get the current user
                                UpdateRow(selectedRow, currentUser);
                                // Close the form after selection
                                selectionForm.DialogResult = DialogResult.OK;
                                selectionForm.Close();
                            }
                            break; // Exit loop after finding the match
                        }
                    }
                }
            };
            // Handle double-click event on DataGridView
            dataGridView.CellDoubleClick += (sender, e) =>
        {
            if (e.RowIndex >= 0 && e.RowIndex < matchingRows.Count)
            {
                // Get the selected row based on the double-clicked cell
                DataGridViewRow selectedRow = matchingRows[e.RowIndex];
                // Update the selected row
                string currentUser = Environment.UserName; // Get the current user
                UpdateRow(selectedRow, currentUser);
                // Close the form after selection
                selectionForm.DialogResult = DialogResult.OK;
                selectionForm.Close();
            }
        };
            // Handle ENTER key event
            dataGridView.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter && dataGridView.SelectedRows.Count > 0)
                {
                    // Prevent the 'ding' sound from happening when ENTER is pressed
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    // Get the selected row based on the currently selected row in the DataGridView
                    int selectedIndex = dataGridView.SelectedRows[0].Index;
                    if (selectedIndex >= 0 && selectedIndex < matchingRows.Count)
                    {
                        DataGridViewRow selectedRow = matchingRows[selectedIndex];
                        // Update the selected row
                        string currentUser = Environment.UserName; // Get the current user
                        UpdateRow(selectedRow, currentUser);
                        // Close the form after selection
                        selectionForm.DialogResult = DialogResult.OK;
                        selectionForm.Close();
                    }
                }
            };
            // Handle the form's 'Shown' event to focus the scanTextBox after the form has loaded
            selectionForm.Shown += (sender, e) =>
            {
                scanTextBox.Focus(); // Set focus to the scanTextBox
            };
            // Add DataGridView and TextBox to the form
            //selectionForm.Controls.Add(dataGridView);
            // Add the Panel to the form
            // Add the DataGridView to the Panel
            panel.Controls.Add(dataGridView);
            selectionForm.Controls.Add(panel);
            selectionForm.Controls.Add(scanTextBox);
            // Show the form as a dialog
            selectionForm.ShowDialog();
        }
        private byte[] GenerateQrCodeImage(string text)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] originalQrCode = qrCode.GetGraphic(20); // Get the QR code as a byte array
                // Resize the QR code to 50x50 pixels
                using (var ms = new MemoryStream(originalQrCode))
                {
                    using (Image originalImage = Image.FromStream(ms))
                    {
                        Bitmap resizedImage = new Bitmap(50, 50); // Create a new bitmap for resizing
                        using (Graphics graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.DrawImage(originalImage, 0, 0, 50, 50); // Draw the original image on the resized bitmap
                        }
                        // Convert resized bitmap to byte array
                        using (MemoryStream resizedStream = new MemoryStream())
                        {
                            resizedImage.Save(resizedStream, System.Drawing.Imaging.ImageFormat.Png); // Save the resized image
                            return resizedStream.ToArray(); // Return the byte array of the resized image
                        }
                    }
                }
            }
        }
        private void UpdateRow(DataGridViewRow row, string currentUser)
        {
            // Update "Counted" and "User" properties
            row.Cells["Counted"].Value = DateTime.Now.ToString(); // Set "Counted" to current date and time
            row.Cells["User"].Value = currentUser; // Set "User" to current Windows user
            // Create a counted item and save it to the XML file
            whItemStockCounter countedItem = new whItemStockCounter()
            {
                IPN = row.Cells["IPN"].Value?.ToString(),
                MFPN = row.Cells["MFPN"].Value?.ToString(),
                Manufacturer = row.Cells["Manufacturer"].Value?.ToString(),
                Description = row.Cells["Description"].Value?.ToString(),
                Stock = Convert.ToInt32(row.Cells["Stock"].Value),
                Id = int.Parse(row.Cells["Id"].Value.ToString()),
                Updated_on = row.Cells["Updated_on"].Value?.ToString(),
                Comments = row.Cells["Comments"].Value?.ToString(),
                Source_Requester = row.Cells["Source_Requester"].Value?.ToString(),
                Counted = row.Cells["Counted"].Value?.ToString(),
                User = row.Cells["User"].Value?.ToString()
            };
            // Save the counted item to the selected XML file
            //SaveCountedItemsToSelectedFile(countedItem);
            SaveCountedItemsToDatabase(countedItem);
            SetSTOCKiewColumsOrder();
        }
        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.SelectAll();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Focus();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            RecalculateBalance();
        }
        private void preRecalc()
        {
            // A list to hold unmatched movements
            List<DataGridViewRow> unmatchedMovements = new List<DataGridViewRow>();
            // Step 1: Get all movements from the DataGridView and sort by Updated_on
            var rows = dataGridView1.Rows.Cast<DataGridViewRow>()
                           .Where(r => r.Cells["Stock"].Value != null && r.Cells["Updated_on"].Value != null)
                           .OrderByDescending(r => Convert.ToDateTime(r.Cells["Updated_on"].Value))
                           .ToList();
            // Step 2: Separate incoming and outgoing movements
            var incomingMovements = new List<DataGridViewRow>();
            var outgoingMovements = new List<DataGridViewRow>();
            foreach (var row in rows)
            {
                int quantity = Convert.ToInt32(row.Cells["Stock"].Value);
                if (quantity > 0)
                {
                    incomingMovements.Add(row); // Positive quantity means incoming
                }
                else if (quantity < 0)
                {
                    outgoingMovements.Add(row); // Negative quantity means outgoing
                }
            }
            foreach (var outgoing in outgoingMovements)
            {
                int outgoingQuantity = Math.Abs(Convert.ToInt32(outgoing.Cells["Stock"].Value)); // Make it positive
                // Try to find a matching incoming where the User property is filled
                var matchingIncoming = incomingMovements
                    .FirstOrDefault(incoming =>
                        Convert.ToInt32(incoming.Cells["Stock"].Value) == outgoingQuantity &&
                        string.IsNullOrEmpty(Convert.ToString(incoming.Cells["User"].Value))); // Prioritize User-filled
                // If no such incoming exists, fall back to any matching incoming
                if (matchingIncoming == null)
                {
                    matchingIncoming = incomingMovements
                        .FirstOrDefault(incoming =>
                            Convert.ToInt32(incoming.Cells["Stock"].Value) == outgoingQuantity);
                }
                if (matchingIncoming != null)
                {
                    // Remove matched movements
                    incomingMovements.Remove(matchingIncoming);
                }
                else
                {
                    // Add to unmatched if no match is found
                    unmatchedMovements.Add(outgoing);
                }
            }
            // Add any remaining unmatched incoming movements
            unmatchedMovements.AddRange(incomingMovements);
            int countNeg = 0;
            foreach (var incoming in unmatchedMovements)
            {
                if (int.Parse(incoming.Cells["Stock"].Value.ToString()) < 0)
                {
                    countNeg += 1;
                }
            }
            if (countNeg > 0)
            {
                button2.Text = string.Format("Recalculate balance ({0})", countNeg);
                button2.BackColor = Color.DarkRed;
                // Step 4: Create a new window and display the unmatched movements
                Form popupForm = new Form();
                popupForm.Text = "Unmatched Movements";
                popupForm.Size = new Size(1666, 666);
                popupForm.StartPosition = FormStartPosition.CenterScreen;
                DataGridView popupDataGridView = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ContextMenuStrip = new ContextMenuStrip(), // Right-click menu
                    ReadOnly = true, // Make the DataGridView uneditable
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect, // Select entire row on cell click
                    MultiSelect = false // Allow only single row selection
                };
                // Create the right-click menu item for deletion
                var deleteMenuItem = new ToolStripMenuItem("Delete");
                popupDataGridView.ContextMenuStrip.Items.Add(deleteMenuItem);
                // Handle right-click and delete operation
                deleteMenuItem.Click += (s, e) =>
                {
                    if (popupDataGridView.SelectedRows.Count > 0)
                    {
                        var selectedRow = popupDataGridView.SelectedRows[0]; // Now use [0] as it's the only selected row
                        int itemId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                        var result = MessageBox.Show($"Are you sure you want to delete item with Id {itemId}?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            DeleteFromDatabase(itemId, false);
                            popupDataGridView.Rows.Remove(selectedRow);
                        }
                    }
                };
                // Step 5: Add columns from the original DataGridView to the popup DataGridView
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    popupDataGridView.Columns.Add((DataGridViewColumn)col.Clone()); // Clone the structure of the original DataGridView
                }
                // Step 6: Add unmatched rows to the popup DataGridView and color them
                foreach (var unmatched in unmatchedMovements)
                {
                    int index = popupDataGridView.Rows.Add();
                    for (int i = 0; i < unmatched.Cells.Count; i++)
                    {
                        popupDataGridView.Rows[index].Cells[i].Value = unmatched.Cells[i].Value;
                    }
                    int quantity = Convert.ToInt32(unmatched.Cells["Stock"].Value);
                    if (quantity > 0)
                    {
                        popupDataGridView.Rows[index].DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        popupDataGridView.Rows[index].DefaultCellStyle.BackColor = Color.IndianRed;
                    }
                    if (popupDataGridView.Rows.Count > 0)
                    {
                        int lastRowIndex = popupDataGridView.Rows.Count - 1;
                        popupDataGridView.ClearSelection();
                        popupDataGridView.Rows[lastRowIndex].Selected = true;
                        popupDataGridView.CurrentCell = popupDataGridView.Rows[lastRowIndex].Cells[0];
                    }
                }
                popupDataGridView.CellMouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                    {
                        popupDataGridView.ClearSelection();
                        popupDataGridView.Rows[e.RowIndex].Selected = true;
                        popupDataGridView.CurrentCell = popupDataGridView.Rows[e.RowIndex].Cells[0];
                    }
                };
                popupForm.Controls.Add(popupDataGridView);
                //popupForm.ShowDialog();
                popupForm.Show();
                // Ask the user if they want to auto-remove negative quantities
                var autoRemoveResult = MessageBox.Show("Do you want to automatically remove all negative quantities?", "Auto-Remove Negative Quantities", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (autoRemoveResult == DialogResult.Yes)
                {
                    // Automatically remove all rows with negative quantities
                    foreach (DataGridViewRow row in popupDataGridView.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["Stock"].Value) < 0)
                        {
                            int itemId = Convert.ToInt32(row.Cells["Id"].Value);
                            DeleteFromDatabase(itemId, true); // Delete from the database
                            //popupDataGridView.Rows.Remove(row); // Remove from DataGridView
                        }
                    }
                    popupForm.Close(); // Close the popup form completely
                }
                textBox1.Clear();
                textBox1.Focus();
            }
            else
            {
                button2.Text = "Recalculate balance";
                button2.BackColor = Color.DarkGreen;
            }
        }
        private void RecalculateBalance()
        {
            // A list to hold unmatched movements
            List<DataGridViewRow> unmatchedMovements = new List<DataGridViewRow>();
            // Step 1: Get all movements from the DataGridView and sort by Updated_on
            var rows = dataGridView1.Rows.Cast<DataGridViewRow>()
                           .Where(r => r.Cells["Stock"].Value != null && r.Cells["Updated_on"].Value != null)
                           .OrderByDescending(r => Convert.ToDateTime(r.Cells["Updated_on"].Value))
                           .ToList();
            // Step 2: Separate incoming and outgoing movements
            var incomingMovements = new List<DataGridViewRow>();
            var outgoingMovements = new List<DataGridViewRow>();
            foreach (var row in rows)
            {
                int quantity = Convert.ToInt32(row.Cells["Stock"].Value);
                if (quantity > 0)
                {
                    incomingMovements.Add(row); // Positive quantity means incoming
                }
                else if (quantity < 0)
                {
                    outgoingMovements.Add(row); // Negative quantity means outgoing
                }
            }
            foreach (var outgoing in outgoingMovements)
            {
                int outgoingQuantity = Math.Abs(Convert.ToInt32(outgoing.Cells["Stock"].Value)); // Make it positive
                // Try to find a matching incoming where the User property is filled
                var matchingIncoming = incomingMovements
                    .FirstOrDefault(incoming =>
                        Convert.ToInt32(incoming.Cells["Stock"].Value) == outgoingQuantity &&
                        string.IsNullOrEmpty(Convert.ToString(incoming.Cells["User"].Value))); // Prioritize User-filled
                // If no such incoming exists, fall back to any matching incoming
                if (matchingIncoming == null)
                {
                    matchingIncoming = incomingMovements
                        .FirstOrDefault(incoming =>
                            Convert.ToInt32(incoming.Cells["Stock"].Value) == outgoingQuantity);
                }
                if (matchingIncoming != null)
                {
                    // Remove matched movements
                    incomingMovements.Remove(matchingIncoming);
                }
                else
                {
                    // Add to unmatched if no match is found
                    unmatchedMovements.Add(outgoing);
                }
            }
            // Add any remaining unmatched incoming movements
            unmatchedMovements.AddRange(incomingMovements);
            // Step 4: Create a new window and display the unmatched movements
            Form popupForm = new Form();
            popupForm.Text = "Unmatched Movements";
            popupForm.Size = new Size(1666, 666);
            popupForm.StartPosition = FormStartPosition.CenterScreen;
            DataGridView popupDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ContextMenuStrip = new ContextMenuStrip(), // Right-click menu
                ReadOnly = true, // Make the DataGridView uneditable
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, // Select entire row on cell click
                MultiSelect = false // Allow only single row selection
            };
            // Create the right-click menu item for deletion
            var deleteMenuItem = new ToolStripMenuItem("Delete");
            popupDataGridView.ContextMenuStrip.Items.Add(deleteMenuItem);
            // Handle right-click and delete operation
            deleteMenuItem.Click += (s, e) =>
            {
                // Ensure a row is selected
                if (popupDataGridView.SelectedRows.Count > 0)
                {
                    // Get the selected row (right-clicked)
                    var selectedRow = popupDataGridView.SelectedRows[0]; // Now use [0] as it's the only selected row
                    // Assuming "Id" is the column with the primary key
                    int itemId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                    // Confirm deletion
                    var result = MessageBox.Show($"Are you sure you want to delete item with Id {itemId}?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        // Call the delete function
                        DeleteFromDatabase(itemId, false);
                        //MessageBox.Show($"Item with Id {itemId} has been deleted from the database.");
                        // Remove the row from the DataGridView
                        popupDataGridView.Rows.Remove(selectedRow);
                    }
                }
            };
            // Step 5: Add columns from the original DataGridView to the popup DataGridView
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                popupDataGridView.Columns.Add((DataGridViewColumn)col.Clone()); // Clone the structure of the original DataGridView
            }
            // Step 6: Add unmatched rows to the popup DataGridView and color them
            foreach (var unmatched in unmatchedMovements)
            {
                int index = popupDataGridView.Rows.Add();
                for (int i = 0; i < unmatched.Cells.Count; i++)
                {
                    popupDataGridView.Rows[index].Cells[i].Value = unmatched.Cells[i].Value;
                }
                // Check the stock balance (assuming 'Stock' is the relevant column)
                int quantity = Convert.ToInt32(unmatched.Cells["Stock"].Value);
                // Apply row color based on stock balance
                if (quantity > 0)
                {
                    popupDataGridView.Rows[index].DefaultCellStyle.BackColor = Color.LightGreen; // Positive balance
                }
                else
                {
                    popupDataGridView.Rows[index].DefaultCellStyle.BackColor = Color.IndianRed;  // Negative or zero balance
                }
                // After the dialog is closed, set selection to the last row of popupDataGridView
                if (popupDataGridView.Rows.Count > 0)
                {
                    int lastRowIndex = popupDataGridView.Rows.Count - 1; // Get the last row index
                    popupDataGridView.ClearSelection(); // Clear any previous selection
                    popupDataGridView.Rows[lastRowIndex].Selected = true; // Select the last row
                    popupDataGridView.CurrentCell = popupDataGridView.Rows[lastRowIndex].Cells[0]; // Set CurrentCell to the first cell of the last row
                }
            }
            // Handle row right-click to select the row
            popupDataGridView.CellMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                {
                    // Select the clicked row
                    popupDataGridView.ClearSelection();
                    popupDataGridView.Rows[e.RowIndex].Selected = true;
                    popupDataGridView.CurrentCell = popupDataGridView.Rows[e.RowIndex].Cells[0]; // Ensure CurrentCell is set to this row
                }
            };
            // Add the DataGridView to the form and show it as a popup
            popupForm.Controls.Add(popupDataGridView);
            popupForm.ShowDialog(); // Show the form as a modal dialog
        }
        private void DeleteFromDatabase(int itemId, bool Auto)
        {
            // Get the selected table name from the ComboBox
            string selectedTable = comboBox3.SelectedItem?.ToString();
            // Check if selectedTable is valid
            if (string.IsNullOrEmpty(selectedTable))
            {
                MessageBox.Show("Please select a valid warehouse.");
                return;
            }
            // Connection string for the selected warehouse
            string connectionString = selectedWHconnstring; // Ensure this is set previously
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the connection
                    // Define the DELETE command using the selected table
                    string deleteCommandText = $"DELETE FROM [{selectedTable}].dbo.STOCK WHERE Id = @ItemId"; // Adjust if necessary
                    using (SqlCommand command = new SqlCommand(deleteCommandText, connection))
                    {
                        command.Parameters.AddWithValue("@ItemId", itemId); // Use parameterized query to prevent SQL injection
                        int rowsAffected = command.ExecuteNonQuery(); // Execute the command
                        if (rowsAffected > 0)
                        {
                            if (!Auto)
                            {
                                MessageBox.Show($"Item with Id {itemId} has been successfully deleted from the {selectedTable} table.");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"No item found with Id {itemId} in the {selectedTable} table. Deletion failed.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., log the error, show a message to the user)
                    MessageBox.Show($"An error occurred while trying to delete the item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
        }
        private void button3_Click(object sender, EventArgs e)
        {
            FrmClientAgnosticWH h = new FrmClientAgnosticWH();
            h.InitializeGlobalWarehouses(warehouses);
            foreach (ClientWarehouse cw in warehouses)
            {
                if (cw != null && comboBox3.Text == cw.clName)
                {
                    h.SetComboBoxText(cw.clName);
                    h.MasterReload(cw.sqlAvl, cw.sqlStock);
                    h.Show();
                }
            }
        }
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if it is a right-click
            if (e.Button == MouseButtons.Right)
            {
                // Get the clicked row using HitTest
                var hitTestInfo = dataGridView1.HitTest(e.X, e.Y);
                if (hitTestInfo.RowIndex >= 0) // Ensure a row was clicked
                {
                    // Select the clicked row
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[hitTestInfo.RowIndex].Selected = true;
                    // Retrieve the selected row
                    DataGridViewRow selectedRow = dataGridView1.Rows[hitTestInfo.RowIndex];
                    // Get the Id and other properties from the selected row
                    int id = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                    string ipn = selectedRow.Cells["IPN"].Value?.ToString();
                    string manufacturer = selectedRow.Cells["Manufacturer"].Value?.ToString();
                    string description = selectedRow.Cells["Description"].Value?.ToString();
                    int stock = Convert.ToInt32(selectedRow.Cells["Stock"].Value);
                    string updatedOn = selectedRow.Cells["Updated_on"].Value?.ToString();
                    string comments = selectedRow.Cells["Comments"].Value?.ToString();
                    string user = selectedRow.Cells["User"].Value?.ToString();
                    // Prepare the confirmation message with item details
                    string message = $"Select an action for the item:\n\n" +
                                     $"Id: {id}\n" +
                                     $"IPN: {ipn}\n" +
                                     $"Manufacturer: {manufacturer}\n" +
                                     $"Description: {description}\n" +
                                     $"Stock: {stock}\n" +
                                     $"Updated On: {updatedOn}\n" +
                                     $"Comments: {comments}\n" +
                                     $"User: {user}";
                    // Create and show the custom message box
                    using (CustomMessageBox customMessageBox = new CustomMessageBox(message))
                    {
                        customMessageBox.ShowDialog(); // Show the custom message box
                        // Check the result based on the button clicked
                        if (customMessageBox.Result == DialogResult.Yes)
                        {
                            // If user confirms UNCOUNT, delete from the COUNT table
                            DeleteFromCountTable(id);
                        }
                        else if (customMessageBox.Result == DialogResult.No)
                        {
                            // If user confirms DELETE, handle the delete operation here
                            DeleteFromSqlDataBase(id); // Placeholder for the actual delete logic
                        }
                        else if (customMessageBox.Result == DialogResult.OK)
                        {
                            // If user confirms DELETE, handle the delete operation here
                            UpdatePackage(id, @"7""");
                        }
                        else if (customMessageBox.Result == DialogResult.Abort)
                        {
                            // If user confirms DELETE, handle the delete operation here
                            UpdatePackage(id, @"Bag");
                        }
                        else if (customMessageBox.Result == DialogResult.Continue)
                        {
                            // If user confirms DELETE, handle the delete operation here
                            UpdatePackage(id, @"13""");
                        }
                        else if (customMessageBox.Result == DialogResult.Ignore)
                        {
                            // If user confirms DELETE, handle the delete operation here
                            UpdatePackage(id, @"Stick");
                        }
                        else if (customMessageBox.Result == DialogResult.Retry)
                        {
                            // If user confirms DELETE, handle the delete operation here
                            UpdatePackage(id, @"Tray");
                        }
                        else if (customMessageBox.Result == DialogResult.TryAgain)
                        {
                            // If user confirms DELETE, handle the delete operation here
                            UpdatePackage(id, @"10""");
                        }
                        // Cancel does nothing
                    }
                }
            }
        }
        private void UpdatePackage(int id, string newComment)
        {
            // Prepare the confirmation message
            string message = $"Are you sure you want to UPDATE the item in STOCK:\n\n" +
                             $"Id: {id}\n" +
                             $"New Comments: {newComment}";
            // Ask for user confirmation
            DialogResult dialogResult = MessageBox.Show(message, "Confirm UPDATE in STOCK", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // If user confirms, perform the update operation
                string connectionString = selectedWHconnstring; // Update with your actual connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string selectedTable = comboBox3.SelectedItem?.ToString();
                    string query = $"UPDATE [{selectedTable}].dbo.STOCK SET comments = @comments WHERE Id = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@comments", newComment);
                        command.Parameters.AddWithValue("@id", id);
                        // Execute the update command
                        int rowsAffected = command.ExecuteNonQuery();
                        // Optionally, check if the update was successful
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Comments updated successfully.", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox1.Focus();
                            SendKeys.Send("{ENTER}"); // Simulate pressing Enter
                        }
                        else
                        {
                            MessageBox.Show("No records were updated. Please check the ID.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            else
            {
                // Optionally, handle the case where the user cancels the operation
                MessageBox.Show("Update operation was canceled.", "Update Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void DeleteFromSqlDataBase(int id)
        {
            //MessageBox.Show($"DELETE operation fired for ID: {id}", "DELETE Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Retrieve the Id from the context menu's Tag
            // Prepare the confirmation message
            string message = $"Are you sure you want to DELETE the item from STOCK:\n\n" +
                             $"Id: {id}";
            // Ask for user confirmation
            DialogResult dialogResult = MessageBox.Show(message, "Confirm DELETE from STOCK", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                // If user confirms, delete the row from the STOCK table
                DeleteFromStockTable(id);
            }
        }
        // Method to handle the UNCOUNT operation
        private void UncountSelectedRow()
    {
        // Retrieve the Id from the context menu's Tag
        var menuData = (dynamic)contextMenuStrip.Tag;
        int id = menuData.Id;
        DataGridViewRow selectedRow = menuData.SelectedRow;
        // Prepare the confirmation message with item details
        string message = $"Are you sure you want to UNCOUNT the item:\n\n" +
                         $"Id: {id}\n" +
                         $"IPN: {selectedRow.Cells["IPN"].Value}\n" +
                         $"Manufacturer: {selectedRow.Cells["Manufacturer"].Value}\n" +
                         $"Description: {selectedRow.Cells["Description"].Value}\n" +
                         $"Stock: {selectedRow.Cells["Stock"].Value}\n" +
                         $"Updated On: {selectedRow.Cells["Updated_on"].Value}\n" +
                         $"Comments: {selectedRow.Cells["Comments"].Value}\n" +
                         $"User: {selectedRow.Cells["User"].Value}";
        // Ask for user confirmation
        DialogResult dialogResult = MessageBox.Show(message, "Confirm UNCOUNT", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (dialogResult == DialogResult.Yes)
        {
            // If user confirms, delete the row from the COUNT table
            DeleteFromCountTable(id);
        }
    }
    // Method to handle the DELETE from STOCK operation
    //private void DeleteFromStockTableSelectedRow()
    //{
    //}
    private void DeleteFromStockTable(int id)
    {
            //// Logic to delete from STOCK table
            //MessageBox.Show($"TEST Deleted item with Id {id} from STOCK table.");
            // Step 1: Define your SQL connection string
            string connectionString = selectedWHconnstring; // Update with your actual connection string
            string selectedTable = comboBox3.SelectedItem?.ToString();
            // Step 2: Create a SQL DELETE statement
            string deleteQuery = $"DELETE FROM [{selectedTable}].dbo.STOCK WHERE Id = @Id";
            // Step 3: Use a using statement for the SQL connection and command
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Step 4: Create a command to execute the query
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    // Step 5: Add the Id parameter
                    command.Parameters.AddWithValue("@Id", id);
                    // Step 6: Open the connection and execute the command
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        // Optional: Notify the user of successful deletion
                        MessageBox.Show($"Item with Id {id} has been successfully DELETEed from the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox1.Focus();
                        SendKeys.Send("{ENTER}"); // Simulate pressing Enter
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"An error occurred while deleting from the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    private void DeleteFromCountTable(int id)
        {
            // Step 1: Define your SQL connection string
            string connectionString = selectedWHconnstring; // Update with your actual connection string
            string selectedTable = comboBox3.SelectedItem?.ToString();
            // Step 2: Create a SQL DELETE statement
            string deleteQuery = $"DELETE FROM [{selectedTable}].dbo.COUNT WHERE Id = @Id";
            // Step 3: Use a using statement for the SQL connection and command
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Step 4: Create a command to execute the query
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    // Step 5: Add the Id parameter
                    command.Parameters.AddWithValue("@Id", id);
                    // Step 6: Open the connection and execute the command
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        // Optional: Notify the user of successful deletion
                        MessageBox.Show($"Item with Id {id} has been successfully UNCOUNTED and removed from the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox1.Focus();
                        SendKeys.Send("{ENTER}"); // Simulate pressing Enter
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"An error occurred while deleting from the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
