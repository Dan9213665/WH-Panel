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
        }

        public class WarehouseApiResponse
        {
            public List<Warehouse> value { get; set; }
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
                    cmd.Parameters.AddWithValue("@QTY", int.Parse(textBox5.Text));
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
        }
    }
}
