using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.OleDb;
using Button = System.Windows.Forms.Button;
using TextBox = System.Windows.Forms.TextBox;
using ComboBox = System.Windows.Forms.ComboBox;

namespace WH_Panel
{
    public partial class FrmPriorityAPI : Form
    {
        public FrmPriorityAPI()
        {
            InitializeComponent();
            SetDarkModeColors(this);
            AttachTextBoxEvents(this);
            // Attach event handlers
            textBox6.KeyUp += textBox6_KeyUp_1;
            //textBox6.KeyDown += textBox6_KeyDown;
            // Simulate button3 click on form load
            button3_Click(this, EventArgs.Empty);
        }
        public class PR_PART
        {
            public string PARTNAME { get; set; }
            public string TYPE { get; set; }
            public string PARTDES { get; set; }
            public string MNFPARTNAME { get; set; }
            public string MNFPARTDES { get; set; }
            public string MNFNAME { get; set; }
            public string MNFDES { get; set; }
            public string STATDES { get; set; }
            public string ECOFLAG { get; set; }
            public string ECONUM { get; set; }
            public int PART { get; set; }
            public int MNF { get; set; }

            public int QTY { get; set; }
        }

        public class ApiResponse
        {
            public List<PR_PART> value { get; set; }
        }
        public class Warehouse
        {
            public string WARHSNAME { get; set; }
            public string WARHSDES { get; set; }
            public List<WarehouseBalance> WARHSBAL_SUBFORM { get; set; }
        }

        public class WarehouseApiResponse
        {
            public List<Warehouse> value { get; set; }
        }

        public class WarehouseBalance
        {
            public string LOCNAME { get; set; }
            public string PARTNAME { get; set; }
            public string PARTDES { get; set; }
            public string SERIALNAME { get; set; }
            public string SERIALDES { get; set; }
            public string EXPIRYDATE { get; set; }
            public string DOCNO { get; set; }
            public string PROJDES { get; set; }
            public string ACTNAME { get; set; }
            public string CUSTNAME { get; set; }
            public int TBALANCE { get; set; }
            public string TUNITNAME { get; set; }
            public string SUPNAME { get; set; }
            public string SUPDES { get; set; }
            public int BALANCE { get; set; }
            public string UNITNAME { get; set; }
            public string CDATE { get; set; }
            public int NUMPACK { get; set; }
            public int WARHS { get; set; }
            public int PART { get; set; }
            public int CUST { get; set; }
            public int ACT { get; set; }
            public int SERIAL { get; set; }

            public string MNFPARTNAME { get; set; } // Add this property
        }
        // Define the LogPartApiResponse class
        public class LogPartApiResponse
        {
            public List<LogPart> value { get; set; }
        }

        public class LogPart
        {
            public string PARTNAME { get; set; }
            public List<PartTransLast2> PARTTRANSLAST2_SUBFORM { get; set; }
        }

        public class PartTransLast2
        {
            public string CURDATE { get; set; }
            public string LOGDOCNO { get; set; }
            public int TQUANT { get; set; }

            public string DOCDES { get; set; }
        }
        public class WarehouseBalanceApiResponse
        {
            public List<WarehouseBalance> value { get; set; }
        }

        private void SetDarkModeColors(Control parentControl)
        {
            Color backgroundColor = Color.FromArgb(50, 50, 50); // Dark background color
            Color foregroundColor = Color.FromArgb(220, 220, 220); // Light foreground color
            Color borderColor = Color.FromArgb(45, 45, 48); // Border color for controls

            foreach (Control control in parentControl.Controls)
            {
                // Set the background and foreground colors
                control.BackColor = backgroundColor;
                control.ForeColor = foregroundColor;

                // Handle specific control types separately
                if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = borderColor;
                    button.ForeColor = foregroundColor;
                }
                else if (control is GroupBox groupBox)
                {
                    groupBox.ForeColor = foregroundColor;
                }
                else if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = backgroundColor;
                    textBox.ForeColor = foregroundColor;
                }
                else if (control is Label label)
                {
                    label.BackColor = backgroundColor;
                    label.ForeColor = foregroundColor;
                }
                else if (control is TabControl tabControl)
                {
                    tabControl.BackColor = backgroundColor;
                    tabControl.ForeColor = foregroundColor;
                    foreach (TabPage tabPage in tabControl.TabPages)
                    {
                        tabPage.BackColor = backgroundColor;
                        tabPage.ForeColor = foregroundColor;
                    }
                }
                else if (control is DataGridView dataGridView)
                {
                    dataGridView.EnableHeadersVisualStyles = false;
                    dataGridView.BackgroundColor = backgroundColor;
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = borderColor;
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = foregroundColor;
                    dataGridView.RowHeadersDefaultCellStyle.BackColor = borderColor;
                    dataGridView.DefaultCellStyle.BackColor = backgroundColor;
                    dataGridView.DefaultCellStyle.ForeColor = foregroundColor;
                    dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255);
                    dataGridView.DefaultCellStyle.SelectionForeColor = foregroundColor;
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.HeaderCell.Style.BackColor = borderColor;
                        column.HeaderCell.Style.ForeColor = foregroundColor;
                    }
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.BackColor = backgroundColor;
                    comboBox.ForeColor = foregroundColor;
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.BackColor = backgroundColor;
                    dateTimePicker.ForeColor = foregroundColor;
                }

                // Recursively update controls within containers
                if (control.Controls.Count > 0)
                {
                    SetDarkModeColors(control);
                }
            }
        }
        private void AttachTextBoxEvents(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Enter += TextBox_Enter;
                    textBox.Leave += TextBox_Leave;
                }

                // Recursively attach events to controls within containers
                if (control.Controls.Count > 0)
                {
                    AttachTextBoxEvents(control);
                }
            }
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.BackColor = Color.LightGreen;
                textBox.ForeColor = Color.Black;
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.BackColor = Color.FromArgb(30, 30, 30); // Dark background color
                textBox.ForeColor = Color.FromArgb(220, 220, 220); // Light foreground color
            }
        }
        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string partName = textBox1.Text;
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PARTNAME eq '{partName}'";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Set the Authorization header
                        string username = "api"; // Replace with your actual username
                        string password = "Ddd@123456"; // Replace with your actual password
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        // Make the HTTP GET request
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        // Read the response content
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Parse the JSON response
                        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseBody);

                        // Check if the response contains any data
                        if (apiResponse.value != null && apiResponse.value.Count > 0)
                        {
                            PR_PART part = apiResponse.value[0];

                            // Populate the textboxes with the data
                            textBox2.Text = part.MNFPARTNAME;
                            textBox3.Text = part.PARTDES;
                            textBox4.Text = part.MNFNAME;
                        }
                        else
                        {
                            MessageBox.Show("No data found for the specified part name.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void printSticker(PR_PART wHitem)
        {
            try
            {
                string userName = Environment.UserName;
                string fpst = @"C:\\Users\\" + userName + "\\Desktop\\Print_Stickers.xlsx";

                string thesheetName = "Sheet1";
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fpst + "; Extended Properties=\"Excel 12.0;HDR=YES;IMEX=0\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE [" + thesheetName + "$] SET PN = @PN, MFPN = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, UPDATEDON = @Updated_on";

                    cmd.Parameters.AddWithValue("@PN", wHitem.PARTNAME);
                    cmd.Parameters.AddWithValue("@MFPN", wHitem.MNFPARTNAME);
                    cmd.Parameters.AddWithValue("@ItemDesc", wHitem.PARTDES);
                    cmd.Parameters.AddWithValue("@QTY", wHitem.QTY);
                    cmd.Parameters.AddWithValue("@Updated_on", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                // Switch to English keyboard layout (you can adjust the culture code as needed)
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US"));

                Microsoft.VisualBasic.Interaction.AppActivate("PN_STICKER_2022.btw - BarTender Designer");
                SendKeys.SendWait("^p");
                SendKeys.SendWait("{Enter}");
                //ComeBackFromPrint();
                Microsoft.VisualBasic.Interaction.AppActivate("Imperium Tabula Principalis");
                textBox1.Focus();
            }
            catch (Exception e)
            {
                MessageBox.Show("Sticker printing failed: " + e.Message);
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Call the button1_Click method programmatically
                button1_Click(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate the required fields
            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Please ensure all fields are filled in before printing.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create a PR_PART object with the data from the textboxes
            PR_PART part = new PR_PART
            {
                PARTNAME = textBox1.Text, // Assuming PART is an integer and is in textBox1
                MNFPARTNAME = textBox2.Text,
                PARTDES = textBox3.Text,
                MNFNAME = textBox4.Text,
                QTY = int.Parse(textBox5.Text) // Set the QTY from textBox5
            };

            // Call the printSticker method
            printSticker(part);
        }

        private async void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string mnfPartName = textBox2.Text;
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=MNFPARTNAME eq '{mnfPartName}'";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Set the Authorization header
                        string username = "api"; // Replace with your actual username
                        string password = "Ddd@123456"; // Replace with your actual password
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        // Make the HTTP GET request
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        // Read the response content
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Parse the JSON response
                        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseBody);

                        // Check if the response contains any data
                        if (apiResponse.value != null && apiResponse.value.Count > 0)
                        {
                            PR_PART part = apiResponse.value[0];

                            // Populate the textboxes with the data
                            textBox1.Text = part.PARTNAME;
                            textBox3.Text = part.PARTDES;
                            textBox4.Text = part.MNFNAME;
                        }
                        else
                        {
                            MessageBox.Show("No data found for the specified manufacturer part name.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string url = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$select=WARHSNAME,WARHSDES";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Set the Authorization header
                    string username = "api"; // Replace with your actual username
                    string password = "Ddd@123456"; // Replace with your actual password
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response
                    WarehouseApiResponse apiResponse = JsonConvert.DeserializeObject<WarehouseApiResponse>(responseBody);

                    // Check if the response contains any data
                    if (apiResponse.value != null && apiResponse.value.Count > 0)
                    {
                        // Populate the combobox with the data
                        comboBox1.Items.Clear();
                        foreach (var warehouse in apiResponse.value)
                        {
                            comboBox1.Items.Add($"{warehouse.WARHSNAME} - {warehouse.WARHSDES}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No data found for the warehouses.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            comboBox1.DroppedDown = true; // Open the dropdown list
        }


        private async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedWarehouse = comboBox1.SelectedItem.ToString().Split(' ')[0];
                string selectedWarehouseDesc = comboBox1.SelectedItem.ToString().Substring(selectedWarehouse.Length).Trim();

                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouse}'&$expand=WARHSBAL_SUBFORM";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Set the Authorization header
                        string username = "api"; // Replace with your actual username
                        string password = "Ddd@123456"; // Replace with your actual password
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        // Make the HTTP GET request
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        // Read the response content
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Parse the JSON response
                        var apiResponse = JsonConvert.DeserializeObject<WarehouseApiResponse>(responseBody);

                        // Check if the response contains any data
                        if (apiResponse.value != null && apiResponse.value.Count > 0)
                        {
                            // Extract the WARHSBAL_SUBFORM data
                            var warehouseBalances = apiResponse.value.SelectMany(w => w.WARHSBAL_SUBFORM).ToList();

                            // Set AutoGenerateColumns to false
                            dataGridView1.AutoGenerateColumns = false;

                            // Clear existing columns
                            dataGridView1.Columns.Clear();

                            // Define the columns you want to display
                            var locNameColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "LOCNAME",
                                HeaderText = "Location Name",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "LOCNAME"
                            };
                            var partNameColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "PARTNAME",
                                HeaderText = "IPN",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "PARTNAME"
                            };
                            var partDesColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "PARTDES",
                                HeaderText = "Description",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "PARTDES"
                            };
                            var balanceColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "BALANCE",
                                HeaderText = "Balance",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "BALANCE"
                            };
                            var tBalanceColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "TBALANCE",
                                HeaderText = "Total Balance",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "TBALANCE"
                            };
                            var cDateColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "CDATE",
                                HeaderText = "DATE",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "CDATE"
                            };
                            var partIdColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "PART",
                                HeaderText = "PART",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "PART"
                            };
                            var mfpnColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "MNFPARTNAME",
                                HeaderText = "MFPN",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "MNFPARTNAME"
                            };

                            // Add columns to the DataGridView
                            dataGridView1.Columns.AddRange(new DataGridViewColumn[]
                            {
                        locNameColumn,
                        partNameColumn,
                        mfpnColumn,
                        partDesColumn,
                        balanceColumn,
                        tBalanceColumn,
                        cDateColumn,
                        partIdColumn
                            });

                            // Populate the DataGridView with the data
                            foreach (var balance in warehouseBalances)
                            {
                                dataGridView1.Rows.Add(balance.LOCNAME, balance.PARTNAME, balance.MNFPARTNAME, balance.PARTDES, balance.BALANCE, balance.TBALANCE, balance.CDATE, balance.PART);
                            }
                            groupBox3.Text = $"Warehouse  {selectedWarehouse} {selectedWarehouseDesc}";
                            ColorTheRows(dataGridView1);
                        }
                        else
                        {
                            MessageBox.Show("No data found for the selected warehouse balance.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0) // Ensure the row index is valid
        //    {
        //        var selectedRow = dataGridView1.Rows[e.RowIndex];
        //        var partId = (int)selectedRow.Cells["PART"].Value;

        //        string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PART eq {partId}";

        //        using (HttpClient client = new HttpClient())
        //        {
        //            try
        //            {
        //                // Set the request headers if needed
        //                client.DefaultRequestHeaders.Accept.Clear();
        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //                // Set the Authorization header
        //                string username = "api"; // Replace with your actual username
        //                string password = "Ddd@123456"; // Replace with your actual password
        //                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        //                // Make the HTTP GET request
        //                HttpResponseMessage response = await client.GetAsync(url);
        //                response.EnsureSuccessStatusCode();

        //                // Read the response content
        //                string responseBody = await response.Content.ReadAsStringAsync();

        //                // Parse the JSON response
        //                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseBody);

        //                // Check if the response contains any data
        //                if (apiResponse.value != null && apiResponse.value.Count > 0)
        //                {
        //                    var part = apiResponse.value[0];

        //                    // Directly update the DataGridView cell
        //                    selectedRow.Cells["MNFPARTNAME"].Value = part.MNFPARTNAME;
        //                    dataGridView1.Refresh();
        //                }
        //                else
        //                {
        //                    MessageBox.Show("No data found for the selected part.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //            }
        //            catch (HttpRequestException ex)
        //            {
        //                MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //    }
        //}


        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                var selectedRow = dataGridView1.Rows[e.RowIndex];
                var partId = (int)selectedRow.Cells["PART"].Value;
                var partName = selectedRow.Cells["PARTNAME"].Value.ToString();

                string partUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PART eq {partId}";
                string logPartUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/LOGPART?$filter=PARTNAME eq '{partName}'&$expand=PARTTRANSLAST2_SUBFORM";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        // Set the Authorization header
                        string username = "api"; // Replace with your actual username
                        string password = "Ddd@123456"; // Replace with your actual password
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        // Make the HTTP GET request for part details
                        HttpResponseMessage partResponse = await client.GetAsync(partUrl);
                        partResponse.EnsureSuccessStatusCode();

                        // Read the response content
                        string partResponseBody = await partResponse.Content.ReadAsStringAsync();

                        // Parse the JSON response
                        var partApiResponse = JsonConvert.DeserializeObject<ApiResponse>(partResponseBody);

                        // Check if the response contains any data
                        if (partApiResponse.value != null && partApiResponse.value.Count > 0)
                        {
                            var part = partApiResponse.value[0];

                            // Directly update the DataGridView cell
                            selectedRow.Cells["MNFPARTNAME"].Value = part.MNFPARTNAME;
                            dataGridView1.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("No data found for the selected part.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        // Make the HTTP GET request for stock movements
                        HttpResponseMessage logPartResponse = await client.GetAsync(logPartUrl);
                        logPartResponse.EnsureSuccessStatusCode();

                        // Read the response content
                        string logPartResponseBody = await logPartResponse.Content.ReadAsStringAsync();

                        // Parse the JSON response
                        var logPartApiResponse = JsonConvert.DeserializeObject<LogPartApiResponse>(logPartResponseBody);

                        // Check if the response contains any data
                        if (logPartApiResponse.value != null && logPartApiResponse.value.Count > 0)
                        {
                            // Set AutoGenerateColumns to false
                            dataGridView2.AutoGenerateColumns = false;

                            // Clear existing columns
                            dataGridView2.Columns.Clear();

                            // Define the columns you want to display
                            var curDateColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "CURDATE",
                                HeaderText = "Transaction Date",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "CURDATE"
                            };
                            var logDocNoColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "LOGDOCNO",
                                HeaderText = "Document Number",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "LOGDOCNO"
                            };
                            var logDOCDESColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "DOCDES",
                                HeaderText = "Source_requester",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "DOCDES"
                            };
                            var tQuantColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "TQUANT",
                                HeaderText = "Quantity",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "TQUANT"
                            };

                            // Add columns to the DataGridView
                            dataGridView2.Columns.AddRange(new DataGridViewColumn[]
                            {
                        curDateColumn,
                        logDocNoColumn,
                        logDOCDESColumn,
                        tQuantColumn
                            });

                            // Populate the DataGridView with the data
                            dataGridView2.Rows.Clear();
                            foreach (var logPart in logPartApiResponse.value)
                            {
                                foreach (var trans in logPart.PARTTRANSLAST2_SUBFORM)
                                {
                                    dataGridView2.Rows.Add(trans.CURDATE, trans.LOGDOCNO,trans.DOCDES, trans.TQUANT);
                                }
                            }
                            groupBox4.Text = $"Stock Movements for {partName}";
                            ColorTheRows(dataGridView2);
                        }
                        else
                        {
                            MessageBox.Show("No stock movements found for the selected part.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



        private void textBox6_KeyUp_1(object sender, KeyEventArgs e)
        {
            string filterText = textBox6.Text.Trim().ToLower();

            if (e.KeyCode == Keys.Escape)
            {
                textBox6.Clear();
                filterText = string.Empty;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["PARTNAME"].Value != null)
                {
                    row.Visible = row.Cells["PARTNAME"].Value.ToString().ToLower().Contains(filterText);
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                var selectedRow = dataGridView1.Rows[e.RowIndex];

                // Extract values from the selected row's cells
                string partName = selectedRow.Cells["PARTNAME"].Value.ToString();
                string mfpn = selectedRow.Cells["MNFPARTNAME"].Value.ToString();
                string partDes = selectedRow.Cells["PARTDES"].Value.ToString();
                int balance = int.Parse(selectedRow.Cells["BALANCE"].Value.ToString());

                // Create a PR_PART object with the extracted data
                PR_PART part = new PR_PART
                {
                    PARTNAME = partName,
                    MNFPARTNAME = mfpn,
                    PARTDES = partDes,
                    QTY = balance
                };

                // Call the printSticker method
                printSticker(part);
            }
        }
        //private void ColorTheRows(DataGridView dataGridView)
        //{
        //    foreach (DataGridViewRow row in dataGridView.Rows)
        //    {
        //        foreach (DataGridViewCell cell in row.Cells)
        //        {
        //            if (cell.OwningColumn.Name == "BALANCE" || cell.OwningColumn.Name == "TQUANT")
        //            {
        //                if (int.TryParse(cell.Value?.ToString(), out int value))
        //                {
        //                    if (value > 0)
        //                    {
        //                        cell.Style.BackColor = Color.LightGreen;
        //                        cell.Style.ForeColor = Color.Black;
        //                    }
        //                    else
        //                    {
        //                        cell.Style.BackColor = Color.IndianRed;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        private void ColorTheRows(DataGridView dataGridView)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.OwningColumn.Name == "BALANCE")
                    {
                        if (int.TryParse(cell.Value?.ToString(), out int balanceValue))
                        {
                            if (balanceValue > 0)
                            {
                                cell.Style.BackColor = Color.LightGreen;
                                cell.Style.ForeColor = Color.Black;
                            }
                            else
                            {
                                cell.Style.BackColor = Color.IndianRed;
                            }
                        }
                    }
                    else if (cell.OwningColumn.Name == "TQUANT")
                    {
                        var docDesCell = row.Cells["DOCDES"];
                        if (docDesCell != null && docDesCell.Value != null)
                        {
                            string docDesValue = docDesCell.Value.ToString();
                            if (docDesValue.Contains("קבלות"))
                            {
                                cell.Style.BackColor = Color.LightGreen;
                                cell.Style.ForeColor = Color.Black;
                            }
                            else if (docDesValue.Contains("נפוק"))
                            {
                                cell.Style.BackColor = Color.IndianRed;
                            }
                        }
                    }
                }
            }
        }

    }
}
