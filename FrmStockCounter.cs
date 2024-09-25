using FastMember;
using Microsoft.Office.Interop.Excel;
using System;
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
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;
using ComboBox = System.Windows.Forms.ComboBox;
using DataTable = System.Data.DataTable;
using GroupBox = System.Windows.Forms.GroupBox;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace WH_Panel
{
    public partial class FrmStockCounter : Form
    {
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
            button1.BackColor = Color.IndianRed;
            //StartCountingProcess();
        }
        List<ClientWarehouse> warehouses { get; set; }
        string selectedWHconnstring { get; set; }
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
                groupBox.Width = 140;
                groupBox.Height = 130;
            }
            comboBox3.SelectedItem = "LEADER-TECH";

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

                            // Merge the data from the XML log with the DataTable
                            if (!string.IsNullOrEmpty(selectedXmlFilePath))
                            {
                                MergeXmlData(dataTable);
                            }

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
        }

        private void ColorRowsBasedOnUserColumn()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue; // Skip the new row placeholder

                // Check if the "User" column has a value
                string userValue = row.Cells["User"].Value?.ToString();
                if (!string.IsNullOrEmpty(userValue))
                {
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
            decimal stockSum = 0;

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
            if (string.IsNullOrEmpty(selectedXmlFilePath))
            {
                MessageBox.Show("Please start the counting process by selecting an XML file first.");
                return;
            }

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

            // Step 3: Search through DataGridView for matching items
            List<DataGridViewRow> matchingRows = new List<DataGridViewRow>(); // Store matching rows

            bool alreadyCounted = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue; // Skip new row placeholder

                // Get values from the current row
                string rowIPN = row.Cells["IPN"].Value?.ToString();
                string rowMFPN = row.Cells["MFPN"].Value?.ToString();
                int rowStock = Convert.ToInt32(row.Cells["Stock"].Value);
                string rowComments = row.Cells["Comments"].Value?.ToString();
                string user = row.Cells["User"].Value?.ToString();

                // Check if this row matches the input item
                if (rowIPN == ipn && rowMFPN == mfpn && rowStock == stock)
                {
                    if ( user == string.Empty)
                    {
                        matchingRows.Add(row);
                    }
                    else
                    {
                        MessageBox.Show("Already counted on " +  row.Cells["Counted"].Value?.ToString(),"Already counted !!!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        alreadyCounted = true;
                        break;
                    }
                    
                }
            }

            // Step 4: Handle match cases
            if (matchingRows.Count == 0 )
            {
                if(alreadyCounted)
                {
                    //
                }
                else
                {
                    MessageBox.Show("No matching items found in the DataGridView.");
                }
               
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
            tb.BackColor = Color.LightGray;
        }

        private static void txtbColorGreenOnEnter(object sender)
        {
            TextBox? tb = (TextBox)sender;
            tb.BackColor = Color.LightGreen;
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

            // Iterate through negative stock items and match them with positive stock items
            foreach (var negativeItem in negativeQTYs)
            {
                // Find the first matching positive item with the same absolute stock value
                var matchingPositive = positiveInWH.FirstOrDefault(positiveItem => Math.Abs(negativeItem.Stock) == positiveItem.Stock);

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


        private void button1_Click(object sender, EventArgs e)
        {

        }


        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Left-click logic: Select an existing XML file
                if (comboBox3.SelectedItem == null)
                {
                    MessageBox.Show("Please select a valid item from the dropdown.");
                    return;
                }

                // Get the currently selected item from comboBox3
                string selectedFolder = comboBox3.SelectedItem.ToString();

                // Build the start folder path using the selected item
                string startFolder = $@"\\dbr1\Data\WareHouse\STOCK_CUSTOMERS\{selectedFolder}";

                // Create and configure OpenFileDialog
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    InitialDirectory = startFolder,
                    Filter = "XML files (*.xml)|*.xml",
                    Title = "Select an XML File"
                };

                // Show the dialog and check if the user selected a file
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog1.FileName;
                    //MessageBox.Show($"You selected: {selectedFilePath}");
                    button1.Text = selectedFilePath;
                    button1.BackColor = Color.LightGreen;
                    selectedXmlFilePath = openFileDialog1.FileName;
                    // You can now process the selected file path further as needed
                }
                textBox1.Focus();
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Right-click logic: Prompt to create a new XML file
                if (comboBox3.SelectedItem == null)
                {
                    MessageBox.Show("Please select a valid item from the dropdown.");
                    return;
                }

                // Get the currently selected item from comboBox3
                string selectedItem = comboBox3.SelectedItem.ToString();

                // Generate the filename based on the current date and selected item
                string currentDate = DateTime.Now.ToString("yyyyMMdd");
                string newFileName = $"{currentDate}_{selectedItem}_count.xml";

                // Build the folder path using the selected item
                string startFolder = $@"\\dbr1\Data\WareHouse\STOCK_CUSTOMERS\{selectedItem}";

                // Create and configure SaveFileDialog
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    InitialDirectory = startFolder,
                    FileName = newFileName, // Pre-fill the file name
                    Filter = "XML files (*.xml)|*.xml",
                    Title = "Create a new XML File for Counting"
                };

                // Show the dialog and check if the user selected a file
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string newFilePath = saveFileDialog1.FileName;

                    try
                    {
                        var testItem = new
                        {
                            Id = 1,
                            Counted = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Current date and time
                            User = Environment.UserName // Current logged-in Windows user
                        };

                        // Create the XML file and write the test object
                        using (XmlWriter writer = XmlWriter.Create(newFilePath))
                        {
                            writer.WriteStartDocument();
                            writer.WriteStartElement("CountedItems"); // Root element for your data

                            writer.WriteStartElement("Item"); // Individual item element
                            writer.WriteElementString("Id", testItem.Id.ToString());
                            writer.WriteElementString("Counted", testItem.Counted);
                            writer.WriteElementString("User", testItem.User);
                            writer.WriteEndElement(); // Close Item

                            writer.WriteEndElement(); // Close CountedItems
                            writer.WriteEndDocument();
                        }

                        MessageBox.Show($"New file created: {newFilePath}");

                        // Set this as the file for further saving
                        selectedXmlFilePath = newFilePath;
                        button1.Text = newFilePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while creating the file: {ex.Message}");
                    }
                }

                textBox1.Focus();
            }

        }
        private void ShowMatchSelectionForm(List<DataGridViewRow> matchingRows)
        {
            // Create a new form dynamically
            Form selectionForm = new Form();
            selectionForm.Text = "Select a Matching Item";
            selectionForm.Size = new Size(800, 400);
            selectionForm.StartPosition = FormStartPosition.CenterParent;

            // Create a DataGridView dynamically
            DataGridView dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Top;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.Height = 300;

            // Create columns for DataGridView
            dataGridView.Columns.Add("IPN", "IPN");
            dataGridView.Columns.Add("MFPN", "MFPN");
            dataGridView.Columns.Add("Stock", "Stock");
            dataGridView.Columns.Add("Comments", "Comments");
            dataGridView.Columns.Add("Id", "Id");
            dataGridView.Columns.Add("Updated_on", "Updated On");

            // Sort matching rows by Updated_on date
            matchingRows = matchingRows.OrderBy(row => DateTime.Parse(row.Cells["Updated_on"].Value.ToString())).ToList();

            // Populate DataGridView with matching rows
            foreach (var row in matchingRows)
            {
                dataGridView.Rows.Add(
                    row.Cells["IPN"].Value.ToString(),
                    row.Cells["MFPN"].Value.ToString(),
                    row.Cells["Stock"].Value.ToString(),
                    row.Cells["Comments"].Value.ToString(),
                    row.Cells["Id"].Value.ToString(),
                    row.Cells["Updated_on"].Value.ToString()
                );
            }

            // Create a Select button dynamically
            Button btnSelect = new Button();
            btnSelect.Text = "Select";
            btnSelect.Dock = DockStyle.Bottom;
            btnSelect.Click += (sender, e) =>
            {
                if (dataGridView.SelectedRows.Count > 0)
                {
                    // Get the selected row from the dynamically created DataGridView
                    int selectedIndex = dataGridView.SelectedRows[0].Index;
                    DataGridViewRow selectedRow = matchingRows[selectedIndex];

                    // Update the selected row
                    string currentUser = Environment.UserName; // Get the current user
                    UpdateRow(selectedRow, currentUser);

                    // Close the form after selection
                    selectionForm.DialogResult = DialogResult.OK;
                    selectionForm.Close();
                }
                else
                {
                    MessageBox.Show("Please select a row.");
                }
            };

            // Add DataGridView and Select button to the form
            selectionForm.Controls.Add(dataGridView);
            selectionForm.Controls.Add(btnSelect);

            // Show the form as a dialog
            selectionForm.ShowDialog();
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
                Stock = Convert.ToInt32(row.Cells["Stock"].Value),
                Id = int.Parse(row.Cells["Id"].Value.ToString()),
                Comments = row.Cells["Comments"].Value?.ToString(),
                Counted = row.Cells["Counted"].Value?.ToString(),
                User = row.Cells["User"].Value?.ToString()
            };

            // Save the counted item to the selected XML file
            SaveCountedItemsToSelectedFile(countedItem);
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
    }
}
