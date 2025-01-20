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
using System.Diagnostics;
using System.Reflection; // Add this using directive if not already present
using System.Drawing;
using System.Security.Principal; // Add this using directive if not already present
using System.IO;
using OfficeOpenXml; // Add the EPPlus NuGet package for reading Excel files

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
            txtbFilterIPN.KeyUp += textBox6_KeyUp_1;
            textBox5.KeyPress += textBox5_KeyPress;
            textBox5.TextChanged += textBox5_TextChanged;
            //textBox6.KeyDown += textBox6_KeyDown;
            // Simulate button3 click on form load
            button3_Click(this, EventArgs.Empty);
            // Enable or disable gbxINSERT based on the current user
            if (Environment.UserName == "lgt")
            {
                gbxINSERT.Visible = true;
            }
            else
            {
                gbxINSERT.Visible = false;
            }
            // Attach CheckedChanged event handlers to radio buttons
            rbtMFG.CheckedChanged += RadioButton_CheckedChanged;
            rbtIN.CheckedChanged += RadioButton_CheckedChanged;
            tbtOUT.CheckedChanged += RadioButton_CheckedChanged;
            rbtMFG.Checked = true;
            //this.RightToLeft = RightToLeft.Yes;
            //this.RightToLeftLayout = true;
            //SetRightToLeftForControls(this);



        }

        private async void FrmPriorityAPI_Load(object sender, EventArgs e)
        {
            await PopulatePackCombobox();
        }

        private void SetRightToLeftForControls(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                control.RightToLeft = RightToLeft.Yes;
                if (control.Controls.Count > 0)
                {
                    SetRightToLeftForControls(control);
                }
            }
        }

        public string username = "api"; // Replace with your actual username
        public string password = "DdD@12345"; // Replace with your actual password
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

            public int WARHS { get; set; }
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
            // public int TBALANCE { get; set; }
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


            public DateTimeOffset UDATE { get; set; }
            public string SUPCUSTNAME { get; set; }

            public string BOOKNUM { get; set; }

            public int TQUANT { get; set; }
            public string DOCDES { get; set; }
        }
        public class WarehouseBalanceApiResponse
        {
            public List<WarehouseBalance> value { get; set; }
        }

        public class WarehouseBalancePayload
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
            //public int TBALANCE { get; set; }
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
        }
        public class Document
        {
            public string DOCNO { get; set; }
            public string TYPE { get; set; }
            public DateTimeOffset CURDATE { get; set; }
            public string ORDNAME { get; set; }
            public string SUPNAME { get; set; }
            public string CDES { get; set; }
            public string NAME { get; set; }
            public string PRIVTYPE { get; set; }
            public string BOOKNUM { get; set; }
            public DateTimeOffset? EDATE { get; set; }
            public string TOWARHSNAME { get; set; }
            public string TOLOCNAME { get; set; }
            public string TOWARHSDES { get; set; }
            public string STATDES { get; set; }
            public string OWNERLOGIN { get; set; }
            public string FLAG { get; set; }
            public string IVALL { get; set; }
            public string STCODE { get; set; }
            public string STDES { get; set; }
            public string PDOCNO { get; set; }
            public string PFDOCNO { get; set; }
            public string PWDOCNO { get; set; }
            public string PJDOCNO { get; set; }
            public string FROMWARHSNAME { get; set; }
            public string FROMLOCNAME { get; set; }
            public string FROMWARHSDES { get; set; }
            public string SHIPPERNAME { get; set; }
            public string SHIPPERDES { get; set; }
            public string PROJDOCNO { get; set; }
            public string PROJDES { get; set; }
            public string MUSERLOGIN { get; set; }
            public string IVBOODNUM { get; set; }
            public string IVNUM { get; set; }
            public string RMADOCNO { get; set; }
            public string DETAILS { get; set; }
            public decimal QPRICE { get; set; }
            public decimal PERCENT { get; set; }
            public decimal DISPRICE { get; set; }
            public decimal VAT { get; set; }
            public decimal TOTPRICE { get; set; }
            public string CODE { get; set; }
            public decimal TOTQUANT { get; set; }
            public string TAXCODE { get; set; }
            public string ADJPRICEFLAG { get; set; }
            public string BRANCHNAME { get; set; }
            public string BRANCHDES { get; set; }
            public string EXTFILEFLAGB { get; set; }
            public string USERLOGIN { get; set; }
            public DateTimeOffset UDATE { get; set; }
            public string Y_11663_5_ESH { get; set; }
            public long DOC { get; set; }
            public List<TransOrder> TRANSORDER_P_SUBFORM { get; set; }
        }

        public class TransOrder
        {
            public string PARTNAME { get; set; }
            public int TQUANT { get; set; }

            public string PACKCODE { get; set; }
            public string UNITNAME { get; set; }
            //public DateTime CURDATE { get; set; } // Add UDATE property
            public string SERIALNAME { get; set; }
        }


        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton.Checked)
                {
                    radioButton.BackColor = Color.Green;
                }
                else
                {
                    radioButton.BackColor = Color.FromArgb(50, 50, 50); // Reset to dark background color
                }
            }
        }




        public async Task PopulatePackCombobox()
        {
            string url = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PACK";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var packCodes = apiResponse["value"].Select(p => p["PACKCODE"].ToString()).ToList();
                    // Populate the ComboBox with the PACKCODE values
                    cmbPackCode.Items.Clear();
                    foreach (var packCode in packCodes)
                    {
                        cmbPackCode.Items.Add(packCode);
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
            cmbPackCode.SelectedIndex = 0; // Select the first item by default

        }

        public class WarehouseService
        {
            private static readonly string baseUrl = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522";
            private static readonly string username = "api"; // Replace with your actual username
            private static readonly string password = "DdD@12345"; // Replace with your actual password




            public static async Task InsertDocumentAsync(Document document, FrmPriorityAPI formInstance)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        // Construct the API URL
                        string apiUrl = $"{baseUrl}/DOCUMENTS_P";

                        // Serialize the document to JSON
                        string jsonPayload = JsonConvert.SerializeObject(document);
                        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                        // Make the HTTP POST request
                        HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                        // Check if the response indicates success
                        if (response.IsSuccessStatusCode)
                        {
                            // Read the response content
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var responseJson = JObject.Parse(responseBody);
                            // MessageBox.Show(responseJson.ToString());
                            string docNo = responseJson["DOCNO"]?.ToString();

                            //MessageBox.Show(docNo);
                            string insertedIpn = formInstance.txtbInputIPN.Text;

                            formInstance.comboBox1_SelectedIndexChanged(formInstance.comboBox1, EventArgs.Empty);
                            formInstance.txtbInputIPN.Clear();
                            formInstance.textBox2.Clear();
                            formInstance.textBox3.Clear();
                            formInstance.textBox4.Clear();
                            formInstance.textBox5.Clear();
                            formInstance.txtbPART.Clear();



                            // Update the document status
                            //await WarehouseService.UpdateDocumentStatusAsync(docNo, "סופית", "Y", formInstance);


                            //MessageBox.Show("Item successfully inserted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            formInstance.txtLog.AppendText($"{insertedIpn} successfully inserted. Document number: {docNo}\n");
                            formInstance.txtLog.ScrollToCaret();
                        }
                        else
                        {
                            // Read the error content
                            string errorContent = await response.Content.ReadAsStringAsync();
                            Clipboard.SetText($"Error: {response.StatusCode}\n{errorContent}");
                            //MessageBox.Show($"Error: {response.StatusCode}\n{errorContent}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            formInstance.txtLog.AppendText($"Error: {response.StatusCode}\n{errorContent}\n");
                            formInstance.txtLog.ScrollToCaret();
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        //MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        formInstance.txtLog.AppendText($"Request error: {ex.Message}\n");
                        formInstance.txtLog.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        formInstance.txtLog.AppendText($"An error occurred: {ex.Message}\n");
                        formInstance.txtLog.ScrollToCaret();
                    }
                }
            }


            public static async Task UpdateDocumentStatusAsync(string docNo, string statDes, string flag, FrmPriorityAPI formInstance)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        // Construct the API URL
                        string apiUrl = $"{baseUrl}/DOCUMENTS_P('{docNo}')";

                        // Create the update payload
                        var updatePayload = new
                        {
                            STATDES = statDes,
                            FLAG = flag
                        };

                        // Serialize the update payload to JSON
                        string jsonPayload = JsonConvert.SerializeObject(updatePayload);
                        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                        // Make the HTTP PATCH request
                        HttpResponseMessage response = await client.PatchAsync(apiUrl, content);

                        // Check if the response indicates success
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Document status successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Read the error content
                            string errorContent = await response.Content.ReadAsStringAsync();
                            Clipboard.SetText($"Error: {response.StatusCode}\n{errorContent}");
                            MessageBox.Show($"Error: {response.StatusCode}\n{errorContent}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string partName = txtbInputIPN.Text;
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PARTNAME eq '{partName}'";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
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
                            txtbPART.Text = part.PART.ToString();
                            textBox5.Focus();
                        }
                        else
                        {
                            txtbInputIPN.Clear();
                            txtbInputIPN.Focus();
                            //MessageBox.Show("No data found for the specified part name.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AutoClosingMessageBox.Show("No data found for the specified part name.", 1000, Color.Red);


                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        //MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        AutoClosingMessageBox.Show($"Request error: {ex.Message}", 1000, Color.Red);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        AutoClosingMessageBox.Show($"Request error: {ex.Message}", 1000, Color.Red);
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                txtbInputIPN.Clear(); textBox2.Clear(); textBox3.Clear(); textBox4.Clear(); txtbPART.Clear();

            }
        }
        private void printSticker(PR_PART wHitem)
        {
            try
            {
                string userName = Environment.UserName;
                string fpst = @"C:\\Users\\" + userName + "\\Desktop\\Print_Stickers.xlsx";
                if (userName == "lgt")
                {
                    string message = $"PN:\t{wHitem.PARTNAME}\n" +
                                     $"MFPN:\t{wHitem.MNFPARTNAME}\n" +
                                     $"ItemDesc:\t{wHitem.PARTDES}\n" +
                                     $"QTY:\t{wHitem.QTY}\n" +
                                     $"Updated_on:\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                    MessageBox.Show(message, "Printed sticker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
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
                    txtbInputIPN.Focus();
                }
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
                btnPrintSticker_Click(sender, e);
                btnMFG_Click(sender, e);
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control keys (like backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //
        }
        private void btnPrintSticker_Click(object sender, EventArgs e)
        {
            // Validate the required fields
            if (string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) ||
                string.IsNullOrEmpty(textBox4.Text) ||
                string.IsNullOrEmpty(textBox5.Text) ||
                !int.TryParse(textBox5.Text, out int qty) ||
                qty <= 0)
            {
                MessageBox.Show("Please ensure all fields are filled in correctly before printing.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create a PR_PART object with the data from the textboxes
            PR_PART part = new PR_PART
            {
                PARTNAME = txtbInputIPN.Text, // Assuming PART is an integer and is in textBox1
                MNFPARTNAME = textBox2.Text,
                PARTDES = textBox3.Text,
                MNFNAME = textBox4.Text,
                QTY = qty // Set the QTY from textBox5
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
                            txtbInputIPN.Text = part.PARTNAME;
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
            txtbInputIPN.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            txtbPART.Clear();
        }

        List<Warehouse> loadedWareHouses = new List<Warehouse>();


        private async void button3_Click(object sender, EventArgs e)
        {
            string url = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$select=WARHSNAME,WARHSDES,WARHS";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
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
                        loadedWareHouses.Clear(); // Clear the list before adding new items
                        foreach (var warehouse in apiResponse.value)
                        {
                            comboBox1.Items.Add($"{warehouse.WARHSNAME} - {warehouse.WARHSDES}");
                            loadedWareHouses.Add(warehouse);
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


        public async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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
                            //var locNameColumn = new DataGridViewTextBoxColumn
                            //{
                            //    DataPropertyName = "LOCNAME",
                            //    HeaderText = "Location Name",
                            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                            //    Name = "LOCNAME"
                            //};
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
                            //var tBalanceColumn = new DataGridViewTextBoxColumn
                            //{
                            //    DataPropertyName = "TBALANCE",
                            //    HeaderText = "Total Balance",
                            //    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                            //    Name = "TBALANCE"
                            //};
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
                        //locNameColumn,
                        partNameColumn,
                        mfpnColumn,
                        partDesColumn,
                        balanceColumn,
                        //tBalanceColumn,
                        cDateColumn,
                        partIdColumn
                            });
                            // Populate the DataGridView with the data
                            foreach (var balance in warehouseBalances)
                            {
                                dataGridView1.Rows.Add(balance.PARTNAME, balance.MNFPARTNAME, balance.PARTDES, balance.BALANCE, balance.CDATE, balance.PART); //balance.TBALANCE  //balance.LOCNAME
                            }
                            groupBox3.Text = $"Warehouse  {selectedWarehouse} {selectedWarehouseDesc}";
                            ColorTheRows(dataGridView1);
                            txtbInputIPN.Focus();
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
                        // Measure the time taken for the HTTP POST request
                        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                        // Make the HTTP GET request for stock movements
                        HttpResponseMessage logPartResponse = await client.GetAsync(logPartUrl);
                        logPartResponse.EnsureSuccessStatusCode();
                        stopwatch.Stop();
                        // Update the ping label
                        UpdatePing(stopwatch.ElapsedMilliseconds);
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
                                HeaderText = "DOCDES",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "DOCDES"
                            };
                            var SUPCUSTNAMEColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "SUPCUSTNAME",
                                HeaderText = "Source_Req",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "SUPCUSTNAME"
                            };
                            var tQuantColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "TQUANT",
                                HeaderText = "Quantity",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "TQUANT"
                            };
                            var tPACKNAMEColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "PACKNAME",
                                HeaderText = "PACK",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "PACKNAME"
                            };
                            // Add columns to the DataGridView
                            dataGridView2.Columns.AddRange(new DataGridViewColumn[]
                            {
                        curDateColumn,
                        logDocNoColumn,
                        logDOCDESColumn,
                        SUPCUSTNAMEColumn,
                        tQuantColumn,
                        tPACKNAMEColumn
                            });
                            // Populate the DataGridView with the data
                            dataGridView2.Rows.Clear();
                            foreach (var logPart in logPartApiResponse.value)
                            {
                                foreach (var trans in logPart.PARTTRANSLAST2_SUBFORM)
                                {
                                    dataGridView2.Rows.Add(trans.CURDATE, trans.LOGDOCNO, trans.DOCDES, trans.SUPCUSTNAME, trans.TQUANT, ""); //trans.BOOKNUM
                                }
                            }
                            groupBox4.Text = $"Stock Movements for {partName}";
                            ColorTheRows(dataGridView2);

                            // Fetch PACKCODE data and update the PACK column
                            foreach (DataGridViewRow row in dataGridView2.Rows)
                            {
                                var logDocNo = row.Cells["LOGDOCNO"].Value?.ToString();
                                var partNameCell = partName;
                                var quant = int.Parse(row.Cells["TQUANT"].Value?.ToString());
                                if (logDocNo != null && partNameCell != null)
                                {
                                    var packCode = await FetchPackCodeAsync(logDocNo, partNameCell, quant);
                                    if (packCode != null)
                                    {
                                        row.Cells["PACKNAME"].Value = packCode;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No stock movements found for the selected part.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        //MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtLog.SelectionColor = Color.Red; // Set the color to acid green
                        txtLog.AppendText($"Request error: {ex.Message}");
                        txtLog.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtLog.SelectionColor = Color.Red; // Set the color to acid green
                        txtLog.AppendText($"Request error: {ex.Message}");
                        txtLog.ScrollToCaret();
                    }
                }
            }
        }

        public async Task<string> FetchPackCodeAsync(string logDocNo, string partName, int quant)
        {
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_P?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_P_SUBFORM";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var transOrders = apiResponse["value"].SelectMany(d => d["TRANSORDER_P_SUBFORM"]).ToList();
                    // Find the matching PARTNAME and QUANT
                    var matchingOrder = transOrders.FirstOrDefault(t => t["PARTNAME"].ToString() == partName && int.Parse(t["TQUANT"].ToString()) == quant);
                    if (matchingOrder != null)
                    {
                        return matchingOrder["PACKCODE"].ToString();
                    }
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    // MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLog.SelectionColor = Color.Red; // Set the color to acid green
                    txtLog.AppendText($"Request error: {ex.Message}");
                    txtLog.ScrollToCaret();
                    return null;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtLog.SelectionColor = Color.Red; // Set the color to acid green
                    txtLog.AppendText($"Request error: {ex.Message}");
                    txtLog.ScrollToCaret();
                    return null;
                }
            }
        }



        private void textBox6_KeyUp_1(object sender, KeyEventArgs e)
        {
            string filterText = txtbFilterIPN.Text.Trim().ToLower();
            if (e.KeyCode == Keys.Escape)
            {
                txtbFilterIPN.Clear();
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
            //if (e.RowIndex >= 0) // Ensure the row index is valid
            //{
            //    var selectedRow = dataGridView1.Rows[e.RowIndex];
            //    // Extract values from the selected row's cells
            //    string partName = selectedRow.Cells["PARTNAME"].Value.ToString();
            //    string mfpn = selectedRow.Cells["MNFPARTNAME"].Value.ToString();
            //    string partDes = selectedRow.Cells["PARTDES"].Value.ToString();
            //    int balance = int.Parse(selectedRow.Cells["BALANCE"].Value.ToString());
            //    // Create a PR_PART object with the extracted data
            //    PR_PART part = new PR_PART
            //    {
            //        PARTNAME = partName,
            //        MNFPARTNAME = mfpn,
            //        PARTDES = partDes,
            //        QTY = balance
            //    };
            //    // Call the printSticker method
            //    printSticker(part);
            //}
            MessageBox.Show("Print stickers from Stock Movements list  >>>>");
        }
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
                            else if (docDesValue.Contains("העברה"))
                            {
                                cell.Style.BackColor = Color.IndianRed;
                            }
                        }
                    }
                }
            }
        }
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                // Get the selected row from dataGridView2
                var selectedRow2 = dataGridView2.Rows[e.RowIndex];
                // Get the selected row from dataGridView1
                var selectedRow1 = dataGridView1.CurrentRow;
                if (selectedRow1 != null)
                {
                    // Extract values from the selected row's cells in dataGridView1
                    string partName = selectedRow1.Cells["PARTNAME"].Value.ToString();
                    string mfpn = selectedRow1.Cells["MNFPARTNAME"].Value.ToString();
                    string partDes = selectedRow1.Cells["PARTDES"].Value.ToString();
                    // Extract the balance value from the selected row's cell in dataGridView2
                    int balance = int.Parse(selectedRow2.Cells["TQUANT"].Value.ToString());
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
        }
        private async void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0) // Ensure the row index is valid and right mouse button is clicked
            {
                var selectedRow = dataGridView2.Rows[e.RowIndex];
                var docDesCell = selectedRow.Cells["DOCDES"];
                var serialNameCell = selectedRow.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.OwningColumn.Name == "LOGDOCNO");
                if (docDesCell != null && docDesCell.Value != null && docDesCell.Value.ToString().Contains("נפוק"))
                {
                    if (serialNameCell != null && serialNameCell.Value != null)
                    {
                        string serialName = serialNameCell.Value.ToString();
                        //MessageBox.Show(serialName);
                        ShowSerialDetails(serialName);
                    }
                }
            }
        }

        private async void ShowSerialDetails(string serialName)
        {
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{serialName}'";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var serialDetails = apiResponse["value"].FirstOrDefault();
                    if (serialDetails != null)
                    {

                        // Create a new form to display the data
                        Form popupForm = new Form
                        {
                            Text = $"{serialName} Details",
                            Size = new Size(500, 300),
                            StartPosition = FormStartPosition.CenterScreen,
                            BackColor = Color.FromArgb(50, 50, 50),
                            ForeColor = Color.FromArgb(220, 220, 220)
                        };
                        DataGridView dataGridView = new DataGridView
                        {
                            Dock = DockStyle.Fill,
                            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                            BackgroundColor = Color.FromArgb(50, 50, 50),
                            ForeColor = Color.FromArgb(220, 220, 220),
                            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                            {
                                BackColor = Color.FromArgb(50, 50, 50),
                                ForeColor = Color.FromArgb(220, 220, 220)
                            },
                            DefaultCellStyle = new DataGridViewCellStyle
                            {
                                BackColor = Color.FromArgb(50, 50, 50),
                                ForeColor = Color.FromArgb(220, 220, 220)
                            },
                            EnableHeadersVisualStyles = false,
                            RowHeadersVisible = false,
                            ColumnCount = 2,
                            AllowUserToAddRows = false,
                            AllowUserToDeleteRows = false,
                            ReadOnly = true
                        };
                        dataGridView.Columns[0].Name = "Field";
                        dataGridView.Columns[1].Name = "Value";
                        // Add rows to the DataGridView
                        dataGridView.Rows.Add("PARTNAME", serialDetails["PARTNAME"].ToString());
                        dataGridView.Rows.Add("PARTDES", serialDetails["PARTDES"].ToString());
                        dataGridView.Rows.Add("SERIALNAME", serialDetails["SERIALNAME"].ToString());
                        dataGridView.Rows.Add("REVNAME", serialDetails["REVNAME"]?.ToString());
                        dataGridView.Rows.Add("REVNUM", serialDetails["REVNUM"].ToString());
                        dataGridView.Rows.Add("SERIALSTATUSDES", serialDetails["SERIALSTATUSDES"].ToString());
                        dataGridView.Rows.Add("OWNERLOGIN", serialDetails["OWNERLOGIN"].ToString());
                        dataGridView.Rows.Add("ORDNAME", serialDetails["ORDNAME"]?.ToString());
                        dataGridView.Rows.Add("QUANT", serialDetails["QUANT"].ToString());
                        popupForm.Controls.Add(dataGridView);
                        popupForm.Show();
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

        private async void btnINSERTlogpart_Click(object sender, EventArgs e)
        {
            string partName = txtbIPN.Text.Trim();
            string partDes = txtbDESC.Text.Trim();
            string partMFPN = txtbMFPN.Text.Trim().ToUpper();
            string partMNFDes = txtbMNF.Text.Trim().ToUpper();
            // Validate the required fields
            if (string.IsNullOrEmpty(partName) || string.IsNullOrEmpty(partDes) || string.IsNullOrEmpty(partMFPN) || string.IsNullOrEmpty(partMNFDes))
            {
                MessageBox.Show("Please ensure all fields are filled in before inserting.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Truncate MNFDES to fit within the 32-character limit
            if (partMNFDes.Length > 32)
            {
                partMNFDes = partMNFDes.Substring(0, 32);
            }
            // Generate MNFNAME by truncating MNFDES to fit within the 10-character limit
            string partMNFName = partMNFDes.Length > 10 ? partMNFDes.Substring(0, 10) : partMNFDes;
            try
            {
                // Measure the time taken for the HTTP POST request
                var stopwatch = Stopwatch.StartNew();
                // Insert into LOGPART and get the generated PART ID
                int partId = await InsertLogPart(partName, partDes);
                // Check if the manufacturer exists, if not, insert it and get the MNF ID
                int mnfId = await GetOrInsertManufacturer(partMNFName, partMNFDes);
                // Insert into PARTMNFONE
                await InsertPartMnfOne(partId, partMFPN, mnfId, partDes);
                // Fetch and display the inserted data
                await DisplayInsertedData(partId);
                stopwatch.Stop();
                // Update the ping label
                UpdatePing(stopwatch.ElapsedMilliseconds);
                btnClear.PerformClick();
                //MessageBox.Show("Item successfully inserted into LOGPART and PARTMNFONE.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private async Task<int> InsertLogPart(string partName, string partDes)
        {
            var logPartData = new
            {
                PARTNAME = partName,
                PARTDES = partDes,
                TYPE = "R"
            };
            string url = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/LOGPART";
            using (HttpClient client = new HttpClient())
            {
                // Set the request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Set the Authorization header
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                // Serialize the logPartData to JSON
                string jsonLogPartData = JsonConvert.SerializeObject(logPartData);
                var content = new StringContent(jsonLogPartData, Encoding.UTF8, "application/json");
                // Make the HTTP POST request
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                // Read the response content
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<JObject>(responseBody);
                return responseData["PART"].Value<int>(); // Assuming the response contains the generated PART ID
            }
        }
        private async Task<int> GetOrInsertManufacturer(string partMNFName, string partMNFDes)
        {
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/MNFCTR?$filter=MNFNAME eq '{partMNFName}'";
            using (HttpClient client = new HttpClient())
            {
                // Set the request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Set the Authorization header
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                // Make the HTTP GET request
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                // Read the response content
                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                var manufacturer = apiResponse["value"].FirstOrDefault();
                if (manufacturer != null)
                {
                    return manufacturer["MNF"].Value<int>();
                }
                else
                {
                    // Insert the manufacturer if it does not exist
                    var mnfData = new
                    {
                        MNFNAME = partMNFName,
                        MNFDES = partMNFDes
                    };
                    string insertUrl = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/MNFCTR";
                    string jsonMnfData = JsonConvert.SerializeObject(mnfData);
                    var content = new StringContent(jsonMnfData, Encoding.UTF8, "application/json");
                    HttpResponseMessage insertResponse = await client.PostAsync(insertUrl, content);
                    insertResponse.EnsureSuccessStatusCode();
                    // Read the response content
                    string insertResponseBody = await insertResponse.Content.ReadAsStringAsync();
                    var insertResponseData = JsonConvert.DeserializeObject<JObject>(insertResponseBody);
                    return insertResponseData["MNF"].Value<int>(); // Assuming the response contains the generated MNF ID
                }
            }
        }
        private async Task InsertPartMnfOne(int partId, string partMFPN, int mnfId, string partDes)
        {
            var partMnfOneData = new
            {
                PART = partId,
                MNFPARTNAME = partMFPN,
                MNFPARTDES = partDes,
                MNF = mnfId
            };
            string url = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE";
            using (HttpClient client = new HttpClient())
            {
                // Set the request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Set the Authorization header
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                // Serialize the partMnfOneData to JSON
                string jsonPartMnfOneData = JsonConvert.SerializeObject(partMnfOneData);
                var content = new StringContent(jsonPartMnfOneData, Encoding.UTF8, "application/json");
                // Make the HTTP POST request
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
        }
        private async Task DisplayInsertedData(int partId)
        {
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PART eq {partId}";
            using (HttpClient client = new HttpClient())
            {
                // Set the request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Set the Authorization header
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                // Make the HTTP GET request
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                // Read the response content
                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                // Create a new form to display the data
                Form popupForm = new Form
                {
                    Text = "Inserted Data",
                    Size = new Size(1500, 100),
                    StartPosition = FormStartPosition.CenterScreen,
                    BackColor = Color.FromArgb(60, 60, 60),
                    ForeColor = Color.FromArgb(220, 220, 220)
                };
                DataGridView dataGridView = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.FromArgb(50, 50, 50),
                    ForeColor = Color.FromArgb(220, 220, 220),
                    ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                    {
                        BackColor = Color.FromArgb(50, 50, 50),
                        ForeColor = Color.FromArgb(220, 220, 220)
                    },
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        BackColor = Color.FromArgb(50, 50, 50),
                        ForeColor = Color.FromArgb(220, 220, 220)
                    },
                    EnableHeadersVisualStyles = false,
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    ReadOnly = true
                };
                dataGridView.Columns.Add("PARTNAME", "PARTNAME");
                dataGridView.Columns.Add("PARTDES", "PARTDES");
                dataGridView.Columns.Add("MNFPARTNAME", "MNFPARTNAME");
                dataGridView.Columns.Add("MNFPARTDES", "MNFPARTDES");
                dataGridView.Columns.Add("MNFNAME", "MNFNAME");
                dataGridView.Columns.Add("MNFDES", "MNFDES");
                foreach (var part in apiResponse.value)
                {
                    dataGridView.Rows.Add(part.PARTNAME, part.PARTDES, part.MNFPARTNAME, part.MNFPARTDES, part.MNFNAME, part.MNFDES);
                }
                popupForm.Controls.Add(dataGridView);
                popupForm.Show();
            }
        }
        private void UpdatePing(long elapsedMilliseconds)
        {
            txtbPing.Text = $"{elapsedMilliseconds} ms";
            if (elapsedMilliseconds < 200)
            {
                txtbPing.BackColor = Color.LightGreen;
                txtbPing.ForeColor = Color.Black;
            }
            else if (elapsedMilliseconds < 500)
            {
                txtbPing.BackColor = Color.Yellow;
                txtbPing.ForeColor = Color.Black;
            }
            else
            {
                txtbPing.BackColor = Color.IndianRed;
                txtbPing.ForeColor = Color.White;
            }
            txtbPing.Update();
        }
        private void txtbBuffer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                // Get the clipboard text
                string clipboardText = Clipboard.GetText();
                // Check if the clipboard text contains tab characters
                if (clipboardText.Contains("\t"))
                {
                    // Split the clipboard text by tab characters
                    string[] values = clipboardText.Split('\t');
                    // Distribute the values across the textboxes
                    if (values.Length > 0) txtbIPN.Text = values[0].Trim();
                    if (values.Length > 1) txtbMNF.Text = values[1].Trim();
                    if (values.Length > 2) txtbMFPN.Text = values[2].Trim();
                    if (values.Length > 3) txtbDESC.Text = values[3].Trim();
                    // Clear the buffer textbox
                    txtbBuffer.Clear();
                    // Prevent the default paste operation
                    e.Handled = true;
                }
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtbIPN.Clear();
            txtbMNF.Clear();
            txtbMFPN.Clear();
            txtbDESC.Clear();
            txtbBuffer.Clear();
        }




        private async void btnMFG_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && txtbInputIPN.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty && textBox4.Text != string.Empty && int.Parse(textBox5.Text) > 0 && int.Parse(textBox5.Text) <= 50000)
            {
                string selectedWarehouseName = comboBox1.SelectedItem.ToString().Split(' ')[0];

                var selectedWarehouse = loadedWareHouses.FirstOrDefault(w => w.WARHSNAME == selectedWarehouseName);

                if (selectedWarehouse != null)
                {

                    string _BOOKNUM = string.Empty;
                    string _OWNERLOGIN = "Yuri_G";
                    string _SUPNAME = string.Empty;

                    if (rbtIN.Checked)
                    {
                        if (txtbIN.Text == string.Empty)
                        {
                            MessageBox.Show("Please enter the supplier document description", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtbIN.Focus();

                            return;
                        }
                        else
                        {
                            _BOOKNUM = txtbIN.Text;
                            _SUPNAME = "CLIENT";
                        }
                    }
                    else if (tbtOUT.Checked)
                    {
                        if (txtbOUT.Text == string.Empty)
                        {
                            MessageBox.Show("Please enter the supplier name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtbOUT.Focus();
                            return;
                        }
                        else
                        {
                            _BOOKNUM = txtbOUT.Text;
                            _SUPNAME = "***";
                        }
                    }
                    else if (rbtMFG.Checked)
                    {
                        _BOOKNUM = "MFG";
                        _SUPNAME = "MFG";
                    }
                    else if (rbtFTK.Checked)
                    {
                        _BOOKNUM = "FTK";
                        _SUPNAME = "FTK";
                    }



                    // Create a new Document object
                    Document document = new Document
                    {
                        //DOCNO = "GR25000123", // Set the document number
                        //STATDES= "סופית",
                        USERLOGIN = _OWNERLOGIN,
                        //FLAG= "Y",
                        TYPE = "P", // Set the document type
                        CURDATE = DateTimeOffset.UtcNow,
                        SUPNAME = _SUPNAME, // Set the supplier name
                        BOOKNUM = _BOOKNUM, // Set the supplier number
                        TOWARHSNAME = selectedWarehouse.WARHSNAME,
                        TRANSORDER_P_SUBFORM = new List<TransOrder>
                {
                    new TransOrder
                    {
                        PARTNAME = txtbInputIPN.Text,
                        TQUANT = int.Parse(textBox5.Text),
                        PACKCODE = cmbPackCode.SelectedItem != null ? cmbPackCode.SelectedItem.ToString() : "Bag",
                        UNITNAME = "יח'",
                        //SERIALNAME = "0",
                        //CURDATE = DateTime.Now // Set UDATE to the current date and time
                    }
                }
                    };

                    // Insert the document
                    await WarehouseService.InsertDocumentAsync(document, this);


                }
                else
                {
                    MessageBox.Show("Selected warehouse not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please check data fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbtIN_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtIN.Checked)
            {
                btnMFG.Text = "INCOMING";
                btnMFG.Update();
                txtbIN.ReadOnly = false;
                txtbIN.Focus();
            }
            else
            {
                txtbIN.ReadOnly = true;
                // btnMFG.Text = "";
            }
        }

        private void tbtOUT_CheckedChanged(object sender, EventArgs e)
        {
            if (tbtOUT.Checked)
            {
                btnMFG.Text = "OUTGOING";
                btnMFG.Update();
                txtbOUT.ReadOnly = false;
                txtbOUT.Focus();
            }
            else
            {
                txtbOUT.ReadOnly = true;
                // btnMFG.Text = "";
            }
        }

        private void rbtMFG_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtMFG.Checked)
            {
                btnMFG.Text = "MFG";
                btnMFG.Update();
                txtbOUT.ReadOnly = true;
                txtbIN.ReadOnly = true;

            }
            else
            {

                // btnMFG.Text = "";
            }
        }

        private void rbtFTK_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtFTK.Checked)
            {
                btnMFG.Text = "FTK";
                btnMFG.Update();
                txtbOUT.ReadOnly = true;
                txtbIN.ReadOnly = true;

            }
            else
            {

                // btnMFG.Text = "";
            }
        }




        private async void btnBULKinsert_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xlsm;*.xlsx";
                openFileDialog.Title = "Select an Excel File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    await BulkInsertIntoDB(filePath);
                }
            }
        }

        private async Task BulkInsertIntoDB(string filePath)
        {
            int InsertedrowsCount = 0;
            int totalRowsCount = 0;
            try
            {
                var fileInfo = new FileInfo(filePath);
                using (var package = new ExcelPackage(fileInfo))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // Read the header row to get the column names
                    Dictionary<string, int> columnIndices = new Dictionary<string, int>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        string columnName = worksheet.Cells[1, col].Text.Trim();
                        columnIndices[columnName] = col;
                    }

                    // Get the column indices for the required fields
                    int partNameCol = columnIndices["IPN"];
                    int partDesCol = columnIndices["Description"];
                    int partMFPNCol = columnIndices["MFPN"];
                    int partMNFDesCol = columnIndices["Manufacturer"];

                    for (int row = 2; row <= rowCount; row++) // Start from row 2 to skip the header
                    {
                        totalRowsCount++;
                        string partName = worksheet.Cells[row, partNameCol].Text.Trim();
                        string partDes = worksheet.Cells[row, partDesCol].Text.Trim();
                        string partMFPN = worksheet.Cells[row, partMFPNCol].Text.Trim().ToUpper();
                        string partMNFDes = worksheet.Cells[row, partMNFDesCol].Text.Trim().ToUpper();


                        // Validate the required fields
                        if (string.IsNullOrEmpty(partName) || string.IsNullOrEmpty(partDes) || string.IsNullOrEmpty(partMFPN) || string.IsNullOrEmpty(partMNFDes))
                        {
                            MessageBox.Show($"Row {row}: Please ensure all fields are filled in before inserting.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        // Truncate MNFDES to fit within the 32-character limit
                        if (partMNFDes.Length > 32)
                        {
                            partMNFDes = partMNFDes.Substring(0, 32);
                        }

                        // Generate MNFNAME by truncating MNFDES to fit within the 10-character limit
                        string partMNFName = partMNFDes.Length > 10 ? partMNFDes.Substring(0, 10) : partMNFDes;

                        try
                        {
                            // Measure the time taken for the HTTP POST request
                            var stopwatch = Stopwatch.StartNew();

                            // Insert into LOGPART and get the generated PART ID
                            int partId = await InsertLogPart(partName, partDes);

                            // Check if the manufacturer exists, if not, insert it and get the MNF ID
                            int mnfId = await GetOrInsertManufacturer(partMNFName, partMNFDes);

                            // Insert into PARTMNFONE
                            await InsertPartMnfOne(partId, partMFPN, mnfId, partDes);

                            stopwatch.Stop();
                            // Update the ping label
                            UpdatePing(stopwatch.ElapsedMilliseconds);

                            InsertedrowsCount++;
                            // Log the extracted values
                            txtLog.SelectionColor = Color.LimeGreen; // Set the color to acid green
                            txtLog.AppendText($"Row {row}: Part Name = {partName}, Part Description = {partDes}, Manufacturer Part Number = {partMFPN}, Manufacturer Description = {partMNFDes}\n");
                            txtLog.ScrollToCaret();

                            //await DisplayInsertedData(partId);
                        }
                        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            // Log the extracted values for duplicate entries
                            txtLog.SelectionColor = Color.Yellow; // Set the color to yellow for duplicate entries
                            txtLog.AppendText($"Row {row}: Part Name = {partName}, Part Description = {partDes}, Manufacturer Part Number = {partMFPN}, Manufacturer Description = {partMNFDes} - Already exists (409) \n");
                            txtLog.ScrollToCaret();
                        }
                        catch (HttpRequestException ex)
                        {
                            // Log the extracted values
                            txtLog.SelectionColor = Color.Red; // Set the color to acid green
                            txtLog.AppendText($"Row {row}: Part Name = {partName}, Part Description = {partDes}, Manufacturer Part Number = {partMFPN}, Manufacturer Description = {partMNFDes}\n");
                            txtLog.ScrollToCaret();
                            //MessageBox.Show($"Row {row}: Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            // Log the extracted values
                            txtLog.SelectionColor = Color.Red; // Set the color to acid green
                            txtLog.AppendText($"Row {row}: Part Name = {partName}, Part Description = {partDes}, Manufacturer Part Number = {partMFPN}, Manufacturer Description = {partMNFDes}\n");
                            txtLog.ScrollToCaret();
                            // MessageBox.Show($"Row {row}: An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    MessageBox.Show($"Bulk insert completed. {InsertedrowsCount} rows inserted of total {totalRowsCount} rows in file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while reading the Excel file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearIpnFilter_Click(object sender, EventArgs e)
        {
            txtbFilterIPN.Clear();
            txtbFilterIPN.Focus();
        }
    }
}
