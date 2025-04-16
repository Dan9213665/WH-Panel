using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
using System.Security.Principal; // Add this using directive if not already present
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Slicer.Style; // Add the EPPlus NuGet package for reading Excel files
#pragma warning disable CS0618
namespace WH_Panel
{
    public partial class FrmPriorityAPI : Form
    {
        public AppSettings settings;
        private DataTable dataTable;
        private DataView dataView;
        private ContextMenuStrip contextMenuStrip;
        private DataGridViewRow selectedRowForContextMenu; // Class-level variable to store the selected row
        //private System.Windows.Forms.Timer breathingTimer;
        //private int opacityStep = 5;
        //private int currentOpacity = 100;
        //private bool increasing = false;
        public static string baseUrl = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522";
        //private DataTable dataTable;
        //private DataView dataView;
        public FrmPriorityAPI()
        {
            InitializeComponent();
            SetDarkModeColors(this);
            AttachTextBoxEvents(this);
            InitializeDataTable();
            // Attach event handlers
            txtbFilterIPN.KeyUp += textBox6_KeyUp_1;
            txtbInputQty.KeyPress += textBox5_KeyPress;
            txtbInputQty.TextChanged += textBox5_TextChanged;
            // Attach Sorted event handlers to DataGridViews
            dataGridView1.Sorted += DataGridView_Sorted;
            dataGridView2.Sorted += DataGridView_Sorted;
            //textBox6.KeyDown += textBox6_KeyDown;
            // Simulate button3 click on form load
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
            InitializeGifButton();
        }
        private void InitializeGifButton()
        {
            // Load GIF from Resources folder
            string resourcesFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources");
            string gifFilePath = Path.Combine(resourcesFolder, "mfgGif.gif");
            // Check if the GIF file exists
            if (File.Exists(gifFilePath))
            {
                // Initialize PictureBox
                var pictureBox = new PictureBox
                {
                    Image = Image.FromFile(gifFilePath), // Load the GIF from the file path
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Dock = DockStyle.Fill // Fill the cell
                };
                // Add PictureBox to the same cell as btnMFG in tableLayoutPanel2
                int column = tableLayoutPanel2.GetColumn(btnMFG);
                int row = tableLayoutPanel2.GetRow(btnMFG);
                tableLayoutPanel2.Controls.Add(pictureBox, column, row);
                tableLayoutPanel2.SetRowSpan(pictureBox, 2); // Set the RowSpan to 2
                // Set the button's parent to the PictureBox to ensure proper layering
                btnMFG.Parent = pictureBox;
                btnMFG.BackColor = Color.Transparent;
                // Bring the button to the front
                btnMFG.BringToFront();
            }
            else
            {
                MessageBox.Show("GIF file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void InitializeDataTable()
        {
            dataTable = new DataTable();
            dataTable.Columns.Add("PARTNAME", typeof(string));
            dataTable.Columns.Add("MNFPARTNAME", typeof(string));
            dataTable.Columns.Add("PARTDES", typeof(string));
            dataTable.Columns.Add("BALANCE", typeof(int));
            dataTable.Columns.Add("CDATE", typeof(string));
            dataTable.Columns.Add("PART", typeof(int));
            dataView = new DataView(dataTable);
            dataGridView1.DataSource = dataView;
        }
        private async void FrmPriorityAPI_Load(object sender, EventArgs e)
        {
            try
            {
                settings = SettingsManager.LoadSettings();
                if (settings == null)
                {
                    MessageBox.Show("Failed to load settings.");
                    return;
                }
                if (string.IsNullOrEmpty(settings.ApiUsername) || string.IsNullOrEmpty(settings.ApiPassword))
                {
                    MessageBox.Show("API credentials are missing in the settings.");
                    return;
                }
                else
                {
                    await LoadWarehouseData();
                    InitializeDataTable(); // Initialize the DataTable after loading data
                    await PopulatePackCombobox();
                }
                InitializeContextMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during initialization: {ex.Message}");
            }
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
        private void DataGridView_Sorted(object sender, EventArgs e)
        {
            if (sender is DataGridView dataGridView)
            {
                ColorTheRows(dataGridView);
            }
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
        public class TDocument
        {
            public string WARHSNAME { get; set; }
            public string TOWARHSNAME { get; set; }
            public string USERLOGIN { get; set; }
            public DateTimeOffset CURDATE { get; set; }
            public string BOOKNUM { get; set; }
            public string DOCNO { get; set; }
            public string TYPE { get; set; }
            public List<TransOrder> TRANSORDER_T_SUBFORM { get; set; }
        }
        public class TransOrder
        {
            public string PARTNAME { get; set; }
            public int TQUANT { get; set; }
            public int QUANT { get; set; }
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
            string url = $"{baseUrl}/PACK";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
           // private static readonly string baseUrl = $"{baseUrl}";
            public static async Task InsertDocumentAsync(Document document, FrmPriorityAPI formInstance, AppSettings settings)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
                            formInstance.comboBox1_SelectedIndexChanged(formInstance.cmbWarehouseList, EventArgs.Empty);
                            formInstance.txtbInputIPN.Clear();
                            formInstance.txtbInputMFPN.Clear();
                            formInstance.txtbPartDescription.Clear();
                            formInstance.txtbManufacturer.Clear();
                            formInstance.txtbInputQty.Clear();
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
            public static async Task TransfertDocumentAsync(TDocument document, FrmPriorityAPI formInstance, AppSettings settings)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        // Construct the API URL
                        string apiUrl = $"{baseUrl}/DOCUMENTS_T";
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
                            formInstance.comboBox1_SelectedIndexChanged(formInstance.cmbWarehouseList, EventArgs.Empty);
                            formInstance.txtbInputIPN.Clear();
                            formInstance.txtbInputMFPN.Clear();
                            formInstance.txtbPartDescription.Clear();
                            formInstance.txtbManufacturer.Clear();
                            formInstance.txtbInputQty.Clear();
                            formInstance.txtbPART.Clear();
                            // Update the document status
                            //await WarehouseService.UpdateDocumentStatusAsync(docNo, "סופית", "Y", formInstance);
                            //MessageBox.Show("Item successfully inserted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            formInstance.txtLog.AppendText($"{insertedIpn} successfully transferred. Document number: {docNo}\n");
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
            public static async Task UpdateDocumentStatusAsync(string docNo, string statDes, string flag, FrmPriorityAPI formInstance, AppSettings settings)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
            Color backgroundColor = Color.FromArgb(55, 55, 55); // Dark background color
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
        private async void txtbInputIPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string partName = txtbInputIPN.Text;
                string url = $"{baseUrl}/PARTMNFONE?$filter=PARTNAME eq '{partName}'";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
                            txtbInputMFPN.Text = part.MNFPARTNAME;
                            txtbPartDescription.Text = part.PARTDES;
                            txtbManufacturer.Text = part.MNFNAME;
                            txtbPART.Text = part.PART.ToString();
                            txtbInputQty.Focus();
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
                txtbInputIPN.Clear(); txtbInputMFPN.Clear(); txtbPartDescription.Clear(); txtbManufacturer.Clear(); txtbPART.Clear();
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
                    //txtbInputIPN.Focus();
                    if (lastUserInput != null)
                    {
                        lastUserInput.Focus();
                    }
                    else
                    {
                        txtbInputIPN.Focus();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Sticker printing failed: " + e.Message);
            }
        }
        private void txtbInputQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Call the button1_Click method programmatically
                if (chkbNoSticker.Checked)
                {
                    btnMFG_Click(sender, e);
                }
                else
                {
                    btnMFG_Click(sender, e);
                    if (!tbtOUT.Checked)
                    {
                        btnPrintSticker_Click(sender, e);
                    }
                }
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
            if (string.IsNullOrEmpty(txtbInputMFPN.Text) ||
                string.IsNullOrEmpty(txtbPartDescription.Text) ||
                string.IsNullOrEmpty(txtbManufacturer.Text) ||
                string.IsNullOrEmpty(txtbInputQty.Text) ||
                !int.TryParse(txtbInputQty.Text, out int qty) ||
                qty <= 0)
            {
                MessageBox.Show("Please ensure all fields are filled in correctly before printing.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Create a PR_PART object with the data from the textboxes
            PR_PART part = new PR_PART
            {
                PARTNAME = txtbInputIPN.Text,
                MNFPARTNAME = txtbInputMFPN.Text,
                PARTDES = txtbPartDescription.Text,
                MNFNAME = txtbManufacturer.Text,
                QTY = qty // Set the QTY from textBox5
            };
            // Call the printSticker method
            printSticker(part);
        }
        private async void txtbInputMFPN_KeyDown(object sender, KeyEventArgs e, TextBox lastInput)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lastUserInput = lastInput;
                string mnfPartName = txtbInputMFPN.Text;
                string encodedMnfPartName = Uri.EscapeDataString(mnfPartName); // URL-encode the MNFPARTNAME
                string url = $"{baseUrl}/PARTMNFONE?$filter=MNFPARTNAME eq '{encodedMnfPartName}'";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
                            // Filter the response to get the IPN that starts with "SGE"
                            var part = apiResponse.value.FirstOrDefault(p => p.PARTNAME.StartsWith(txtbPrefix.Text));
                            if (part != null)
                            {
                                // Populate the textboxes with the data
                                txtbInputIPN.Text = part.PARTNAME;
                                txtbPartDescription.Text = part.PARTDES;
                                txtbManufacturer.Text = part.MNFNAME;
                                txtbInputQty.Focus();
                            }
                            else
                            {
                                MessageBox.Show($"No IPN found that starts with {txtbPrefix.Text}.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
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
        private void btnClearFields_Click(object sender, EventArgs e)
        {
            txtbInputIPN.Clear();
            txtbInputMFPN.Clear();
            txtbPartDescription.Clear();
            txtbManufacturer.Clear();
            txtbPART.Clear();
        }
        List<Warehouse> loadedWareHouses = new List<Warehouse>();
        private async void button3_Click(object sender, EventArgs e)
        {
            await LoadWarehouseData();
        }
        private async Task LoadWarehouseData()
        {
            txtLog.AppendText($"Loading warehouses list...\n");
            string url = $"{baseUrl}/WAREHOUSES?$select=WARHSNAME,WARHSDES,WARHS";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
                        cmbWarehouseList.Items.Clear();
                        loadedWareHouses.Clear(); // Clear the list before adding new items
                        foreach (var warehouse in apiResponse.value)
                        {
                            cmbWarehouseList.Items.Add($"{warehouse.WARHSNAME} - {warehouse.WARHSDES}");
                            loadedWareHouses.Add(warehouse);
                        }
                        // Set the autocomplete source
                        cmbWarehouseList.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        cmbWarehouseList.AutoCompleteSource = AutoCompleteSource.ListItems;
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
            cmbWarehouseList.DroppedDown = true; // Open the dropdown list
        }
        private Dictionary<string, string> avlDictionary = new Dictionary<string, string>();
        public async void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWarehouseList.SelectedItem != null)
            {
                txtLog.AppendText($"Loading {cmbWarehouseList.SelectedItem} stock data ...\n");
                string selectedWarehouse = cmbWarehouseList.SelectedItem.ToString().Split(' ')[0];
                string selectedWarehouseDesc = cmbWarehouseList.SelectedItem.ToString().Substring(selectedWarehouse.Length).Trim();
                string avlUrl = $"{baseUrl}/PARTMNFONE?$filter=PARTNAME eq '{selectedWarehouse}_*'";
                string balanceUrl = $"{baseUrl}/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouse}'&$expand=WARHSBAL_SUBFORM";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        // Make the HTTP GET request for AVL list
                        HttpResponseMessage avlResponse = await client.GetAsync(avlUrl);
                        avlResponse.EnsureSuccessStatusCode();
                        string avlResponseBody = await avlResponse.Content.ReadAsStringAsync();
                        var avlApiResponse = JsonConvert.DeserializeObject<PartMnfOneApiResponse>(avlResponseBody);
                        // Populate the AVL dictionary
                        avlDictionary = new Dictionary<string, string>();
                        foreach (var part in avlApiResponse.value)
                        {
                            if (!avlDictionary.ContainsKey(part.PARTNAME))
                            {
                                avlDictionary.Add(part.PARTNAME, part.MNFPARTNAME);
                            }
                            else
                            {
                                // Handle duplicate keys (e.g., log a warning or update the value)
                               // txtLog.AppendText($"Duplicate PARTNAME found: {part.PARTNAME}. Skipping...\n");
                            }
                        }
                        // Make the HTTP GET request for warehouse balance
                        HttpResponseMessage balanceResponse = await client.GetAsync(balanceUrl);
                        balanceResponse.EnsureSuccessStatusCode();
                        string balanceResponseBody = await balanceResponse.Content.ReadAsStringAsync();
                        var balanceApiResponse = JsonConvert.DeserializeObject<WarehouseApiResponse>(balanceResponseBody);
                        // Check if the response contains any data
                        if (balanceApiResponse.value != null && balanceApiResponse.value.Count > 0)
                        {
                            // Extract the WARHSBAL_SUBFORM data
                            var warehouseBalances = balanceApiResponse.value.SelectMany(w => w.WARHSBAL_SUBFORM).ToList();
                            // Clear existing rows in the DataTable
                            dataTable.Rows.Clear();
                            // Add rows to the DataTable
                            foreach (var balance in warehouseBalances)
                            {
                                try
                                {
                                    string partName = balance.PARTNAME ?? string.Empty;
                                    string partDes = balance.PARTDES ?? string.Empty;
                                    int balanceValue = balance.BALANCE;
                                    string cDate = balance.CDATE?.Substring(0, 10) ?? string.Empty;
                                    int partId = balance.PART;
                                    string mfpn = avlDictionary.ContainsKey(partName) ? avlDictionary[partName] : string.Empty;
                                    dataTable.Rows.Add(partName, mfpn, partDes, balanceValue, cDate, partId);
                                }
                                catch (Exception ex)
                                {
                                    txtLog.AppendText($"Error adding row: {ex.Message}\n");
                                }
                            }
                            groupBox3.Text = $"Warehouse  {selectedWarehouse} {selectedWarehouseDesc}";
                            ColorTheRows(dataGridView1);
                            if (lastUserInput != null)
                            {
                                lastUserInput.Focus();
                            }
                            else
                            {
                                txtbInputIPN.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No data found for the selected warehouse balance.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        txtLog.AppendText($"Request error: {ex.Message}\n");
                    }
                    catch (Exception ex)
                    {
                        txtLog.AppendText($"An error occurred: {ex.Message}\n");
                    }
                }
            }
            txtbPrefix.Text = cmbWarehouseList.SelectedItem.ToString().Split(' ')[0];
        }
        private async Task ExtractMFPNForRow(DataGridViewRow row)
        {
            var partId = (int)row.Cells["PART"].Value;
            var partName = row.Cells["PARTNAME"].Value.ToString();
            string partUrl = $"{baseUrl}/PARTMNFONE?$filter=PART eq {partId}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
                        row.Cells["MNFPARTNAME"].Value = part.MNFPARTNAME;
                        dataGridView1.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("No data found for the selected part.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            await Task.Delay(100); // Delay for 1 second
        }
        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                var selectedRow = dataGridView1.Rows[e.RowIndex];
                await ExtractMFPNForRow(selectedRow);
                var partName = selectedRow.Cells["PARTNAME"].Value.ToString();
                string logPartUrl = $"{baseUrl}/LOGPART?$filter=PARTNAME eq '{partName}'&$expand=PARTTRANSLAST2_SUBFORM";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
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
                                DataPropertyName = "UDATE",
                                HeaderText = "Transaction Date",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "UDATE"
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
                                HeaderText = "Source_Requester",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "SUPCUSTNAME"
                            };
                            var tQuantColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "TQUANT",
                                HeaderText = "QTY",
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
                            var DocBOOKNUMColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "BOOKNUM",
                                HeaderText = "Client`s Document",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "BOOKNUM"
                            };
                            // Add columns to the DataGridView
                            dataGridView2.Columns.AddRange(new DataGridViewColumn[]
                            {
                        curDateColumn,
                        logDocNoColumn,
                        logDOCDESColumn,
                        SUPCUSTNAMEColumn,
                        DocBOOKNUMColumn,
                        tQuantColumn,
                        tPACKNAMEColumn
                            });
                            // Populate the DataGridView with the data
                            dataGridView2.Rows.Clear();
                            foreach (var logPart in logPartApiResponse.value)
                            {
                                foreach (var trans in logPart.PARTTRANSLAST2_SUBFORM)
                                {
                                    dataGridView2.Rows.Add("", trans.LOGDOCNO, trans.DOCDES, trans.SUPCUSTNAME, "", trans.TQUANT, "");
                                }
                            }
                            groupBox4.Text = $"Stock Movements for {partName}";
                            ColorTheRows(dataGridView2);
                            foreach (DataGridViewRow row in dataGridView2.Rows)
                            {
                                var logDocNo = row.Cells["LOGDOCNO"].Value?.ToString();
                                var partNameCell = partName;
                                var quant = int.Parse(row.Cells["TQUANT"].Value?.ToString());
                                if (logDocNo != null && partNameCell != null)
                                {
                                    var results = await FetchPackCodeAsync(logDocNo, partNameCell, quant);
                                    foreach (var result in results)
                                    {
                                        if (result.PackCode != null)
                                        {
                                            row.Cells["PACKNAME"].Value = result.PackCode;
                                        }
                                        if (result.BookNum != null)
                                        {
                                            row.Cells["BOOKNUM"].Value = result.BookNum;
                                        }
                                        if (result.Date != null)
                                        {
                                            row.Cells["UDATE"].Value = result.Date;
                                        }
                                    }
                                }
                            }
                            // Sort the DataGridView by the first column in descending order
                            dataGridView2.Sort(dataGridView2.Columns[0], ListSortDirection.Descending);
                        }
                        else
                        {
                            MessageBox.Show("No stock movements found for the selected part.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        txtLog.SelectionColor = Color.Red; // Set the color to acid green
                        txtLog.AppendText($"Request error: {ex.Message}");
                        txtLog.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        if (txtLog != null && !txtLog.IsDisposed)
                        {
                            txtLog.SelectionColor = Color.Red; // Set the color to acid green
                            txtLog.AppendText($"Request error: {ex.Message}");
                            txtLog.ScrollToCaret();
                        }
                    }
                }
            }
        }
        public async Task<List<(string PackCode, string BookNum, string Date)>> FetchPackCodeAsync(string logDocNo, string partName, int quant)
        {
            List<(string PackCode, string BookNum, string Date)> results = new List<(string PackCode, string BookNum, string Date)>();
            string url;
            if (logDocNo.StartsWith("GR"))
            {
                // Handle GR documents
                url = $"{baseUrl}/DOCUMENTS_P?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_P_SUBFORM";
            }
            else if (logDocNo.StartsWith("WR"))
            {
                // Handle GR documents
                url = $"{baseUrl}/DOCUMENTS_T?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_T_SUBFORM";
            }
            else if (logDocNo.StartsWith("SH"))
            {
                // Handle GR documents
                url = $"{baseUrl}/DOCUMENTS_D?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_D_SUBFORM";
            }
            else if (logDocNo.StartsWith("ROB"))
            {
                url = $"{baseUrl}/SERIAL?$filter=SERIALNAME eq '{logDocNo}'";
            }
            else if (logDocNo.StartsWith("IC"))
            {
                url = $"{baseUrl}/DOCUMENTS_C?$filter=DOCNO eq '{logDocNo}'";
            }
            else
            {
                // Handle other document types if needed
                url = $"{baseUrl}/DOCUMENTS_P?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_P_SUBFORM";
            }
            results = await FetchPackCodeFromUrlAsync(url, logDocNo, partName, quant, logDocNo.StartsWith("ROB"));
            return results;
        }
        private async Task<List<(string PackCode, string BookNum, string Date)>> FetchPackCodeFromUrlAsync(string url, string logDocNo, string partName, int quant, bool isRobDocument)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    if (apiResponse == null || apiResponse["value"] == null || !apiResponse["value"].Any())
                    {
                        return new List<(string PackCode, string BookNum, string Date)>();
                    }
                    var document = apiResponse["value"].FirstOrDefault();
                    if (document == null)
                    {
                        return new List<(string PackCode, string BookNum, string Date)>();
                    }
                    var results = new List<(string PackCode, string BookNum, string Date)>();
                    if (isRobDocument)
                    {
                        // Handle ROB document logic
                        string packCode = document["PACKCODE"]?.ToString();
                        string bookNum = document["BOOKNUM"]?.ToString();
                        string date = document["UDATE"]?.ToString();
                        results.Add((packCode, bookNum, date));
                    }
                    else if (logDocNo.StartsWith("WR"))
                    {
                        // Handle WR document logic
                        var transOrders = document["TRANSORDER_T_SUBFORM"]?.ToList();
                        if (transOrders == null)
                        {
                            return new List<(string PackCode, string BookNum, string Date)>();
                        }
                        // Find all matching PARTNAME and QUANT
                        var matchingOrders = transOrders.Where(t => t["PARTNAME"].ToString() == partName && int.Parse(t["QUANT"].ToString()) == quant).ToList();
                        foreach (var matchingOrder in matchingOrders)
                        {
                            string packCode = matchingOrder["PACKCODE"]?.ToString();
                            string bookNum = document["BOOKNUM"]?.ToString();
                            string date = await FetchUDateAsync(logDocNo);
                            results.Add((packCode, bookNum, date));
                        }
                    }
                    else if (logDocNo.StartsWith("SH"))
                    {
                        // Handle SH document logic
                        string bookNum = document["CDES"]?.ToString();
                        string date = document["UDATE"]?.ToString();
                        results.Add((null, bookNum, date));
                    }
                    else if (logDocNo.StartsWith("IC"))
                    {
                        // Handle SH document logic
                        string bookNum = document["CDES"]?.ToString();
                        string date = document["UDATE"]?.ToString();
                        results.Add((null, bookNum, date));
                    }
                    else
                    {
                        // Handle GR document logic
                        var transOrders = document["TRANSORDER_P_SUBFORM"]?.ToList();
                        if (transOrders == null)
                        {
                            return new List<(string PackCode, string BookNum, string Date)>();
                        }
                        // Find all matching PARTNAME and QUANT
                        var matchingOrders = transOrders.Where(t => t["PARTNAME"].ToString() == partName && int.Parse(t["TQUANT"].ToString()) == quant).ToList();
                        foreach (var matchingOrder in matchingOrders)
                        {
                            string packCode = matchingOrder["PACKCODE"]?.ToString();
                            string bookNum = document["BOOKNUM"]?.ToString();
                            string date = await FetchUDateAsync(logDocNo);
                            results.Add((packCode, bookNum, date));
                        }
                    }
                    return results;
                }
                catch (HttpRequestException ex)
                {
                    txtLog.SelectionColor = Color.Red; // Set the color to red
                    txtLog.AppendText($"Request error: {ex.Message}\n");
                    txtLog.ScrollToCaret();
                    return new List<(string PackCode, string BookNum, string Date)>();
                }
                catch (Exception ex)
                {
                    txtLog.SelectionColor = Color.Red; // Set the color to red
                    txtLog.AppendText($"Request error: {ex.Message}\n");
                    txtLog.ScrollToCaret();
                    return new List<(string PackCode, string BookNum, string Date)>();
                }
            }
        }
        public async Task<string> FetchUDateAsync(string docNo)
        {
            string uDate = null;
            // Log the document number for debugging
            //txtLog.SelectionColor = Color.Blue; // Set the color to blue
            //txtLog.AppendText($"Document Number: '{docNo}'\n");
            //txtLog.ScrollToCaret();
            if (docNo.StartsWith("ROB"))
            {
                // Fetch UDATE from SERIAL
                string url = $"{baseUrl}/SERIAL?$filter=SERIALNAME eq '{docNo}'";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        // Make the HTTP GET request
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        // Read the response content
                        string responseBody = await response.Content.ReadAsStringAsync();
                        // Parse the JSON response
                        var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                        var serial = apiResponse["value"].FirstOrDefault();
                        if (serial != null)
                        {
                            txtLog.AppendText($"Data for SERIALNAME: {serial}\n");
                            uDate = serial["UDATE"]?.ToString();
                            if (uDate == null)
                            {
                                txtLog.SelectionColor = Color.Red; // Set the color to red
                                txtLog.AppendText($"UDATE is null for SERIALNAME: {docNo}\n");
                                txtLog.ScrollToCaret();
                            }
                        }
                        else
                        {
                            txtLog.SelectionColor = Color.Red; // Set the color to red
                            txtLog.AppendText($"No serial found for SERIALNAME: {docNo}\n");
                            txtLog.ScrollToCaret();
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        txtLog.SelectionColor = Color.Red; // Set the color to red
                        txtLog.AppendText($"Request error: {ex.Message}\n");
                        txtLog.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        txtLog.SelectionColor = Color.Red; // Set the color to red
                        txtLog.AppendText($"Request error: {ex.Message}\n");
                        txtLog.ScrollToCaret();
                    }
                }
            }
            else if (docNo.StartsWith("GR"))
            {
                // Fetch UDATE from DOCUMENTS_P
                string url = $"{baseUrl}/DOCUMENTS_P?$filter=DOCNO eq '{docNo}'";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        // Make the HTTP GET request
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        // Read the response content
                        string responseBody = await response.Content.ReadAsStringAsync();
                        // Parse the JSON response
                        var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                        var document = apiResponse["value"].FirstOrDefault();
                        if (document != null)
                        {
                            uDate = document["UDATE"]?.ToString();
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        txtLog.SelectionColor = Color.Red; // Set the color to red
                        txtLog.AppendText($"Request error: {ex.Message}\n");
                        txtLog.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        txtLog.SelectionColor = Color.Red; // Set the color to red
                        txtLog.AppendText($"Request error: {ex.Message}\n");
                        txtLog.ScrollToCaret();
                    }
                }
            }
            else if (docNo.StartsWith("WR"))
            {
                // Fetch UDATE from DOCUMENTS_P
                string url = $"{baseUrl}/DOCUMENTS_T?$filter=DOCNO eq '{docNo}'";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        // Make the HTTP GET request
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        // Read the response content
                        string responseBody = await response.Content.ReadAsStringAsync();
                        // Parse the JSON response
                        var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                        var document = apiResponse["value"].FirstOrDefault();
                        if (document != null)
                        {
                            uDate = document["UDATE"]?.ToString();
                            //txtLog.AppendText($"Data for DOCNO: {document} UDATE: {uDate} \n");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        txtLog.SelectionColor = Color.Red; // Set the color to red
                        txtLog.AppendText($"Request error: {ex.Message}\n");
                        txtLog.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        txtLog.SelectionColor = Color.Red; // Set the color to red
                        txtLog.AppendText($"Request error: {ex.Message}\n");
                        txtLog.ScrollToCaret();
                    }
                }
            }
            else
            {
                // Handle other document types if needed
                txtLog.SelectionColor = Color.Orange; // Set the color to orange
                txtLog.AppendText($"Unhandled document type for DOCNO: {docNo}\n");
                txtLog.ScrollToCaret();
            }
            return uDate;
        }
        private void textBox6_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                string filterText = txtbFilterIPN.Text.Trim().ToLower();
                if (e.KeyCode == Keys.Escape)
                {
                    txtbFilterIPN.Clear();
                    filterText = string.Empty;
                }
                if (string.IsNullOrEmpty(filterText))
                {
                    dataView.RowFilter = string.Empty;
                    ColorTheRows(dataGridView1);
                }
                else
                {
                    dataView.RowFilter = $"PARTNAME LIKE '%{filterText}%'";
                    ColorTheRows(dataGridView1);
                }
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
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
                            else if (docDesValue.Contains("ללקוח"))
                            {
                                cell.Style.BackColor = Color.BlueViolet;
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
        private void InitializeContextMenu()
        {
            contextMenuStrip = new ContextMenuStrip();
            foreach (var item in cmbPackCode.Items)
            {
                contextMenuStrip.Items.Add(item.ToString(), null, ContextMenuItem_Click);
                //txtLog.AppendText($"Added context menu item: {item}\n");
            }
        }
        private async void ContextMenuItem_Click(object sender, EventArgs e)
        {
            txtLog.AppendText("Context menu item clicked\n");
            if (sender is ToolStripMenuItem menuItem && contextMenuStrip.Tag is DataGridViewRow selectedRow)
            {
                //txtLog.AppendText("Context menu item is ToolStripMenuItem\n");
                string selectedPackCode = menuItem.Text;
                string docNo = selectedRow.Cells["LOGDOCNO"].Value.ToString();
                // Extract PARTNAME from groupBox4.Text
                string partName = groupBox4.Text.Replace("Stock Movements for ", "").Trim();
                // Confirm the update
                DialogResult result = MessageBox.Show($"Do you want to update the package to '{selectedPackCode}' for part '{partName}'?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //txtLog.AppendText($"Updating package to '{selectedPackCode}' for part '{partName}'\n");
                    //string docType = "P";
                    //await UpdatePackage(docNo, docType, partName, selectedPackCode);
                    txtLog.SelectionColor = Color.Red; // Set the color to green
                    txtLog.AppendText($"Only through web interface! ☹️\n");
                    txtLog.ScrollToCaret();
                }
            }
        }
        private string statusUrl { get; set; }
        private async Task UpdatePackage(string docNo, string docType, string partName, string packCode)
        {
            string url = $"{baseUrl}/DOCUMENTS_P?$filter=DOCNO eq '{docNo}' and TYPE eq '{docType}'&$expand=TRANSORDER_P_SUBFORM($filter=PARTNAME eq '{partName}')";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    if (apiResponse == null)
                    {
                        txtLog.AppendText("apiResponse is null\n");
                        return;
                    }
                    var document = apiResponse["value"]?.FirstOrDefault();
                    if (document == null)
                    {
                        txtLog.AppendText("No document found in apiResponse\n");
                        return;
                    }
                    var transOrderToken = document["TRANSORDER_P_SUBFORM"];
                    if (transOrderToken == null)
                    {
                        txtLog.AppendText("document['TRANSORDER_P_SUBFORM'] is null\n");
                        return;
                    }
                    var transOrder = transOrderToken.FirstOrDefault();
                    if (transOrder == null)
                    {
                        txtLog.AppendText("No transOrder found in document['TRANSORDER_P_SUBFORM']\n");
                        return;
                    }
                    string kline = transOrder["KLINE"].ToString();
                    string type = transOrder["TYPE"].ToString();
                    string trans = transOrder["TRANS"].ToString();
                    string transOrderUrl = $"{baseUrl}/DOCUMENTS_P(DOCNO='{docNo}',TYPE='{docType}')/TRANSORDER_P_SUBFORM(KLINE={kline},TYPE='{type}',TRANS={trans})";
                    txtLog.AppendText($"PATCH URL: {transOrderUrl}\n"); // Log the PATCH URL
                    // Check if the document is finalized
                    string originalStatus = document["STATDES"]?.ToString();
                    if (originalStatus == "סופית")
                    {
                        // Update the document status to allow modifications
                        var updateStatusPayload = new { STATDES = "טיוטא" };
                        string statusPayload = JsonConvert.SerializeObject(updateStatusPayload);
                        var statusContent = new StringContent(statusPayload, Encoding.UTF8, "application/json");
                        string statusUrl = $"{baseUrl}/DOCUMENTS_P(DOCNO='{docNo}',TYPE='{docType}')";
                        HttpResponseMessage statusResponse = await client.PatchAsync(statusUrl, statusContent);
                        if (!statusResponse.IsSuccessStatusCode)
                        {
                            string errorContent = await statusResponse.Content.ReadAsStringAsync();
                            txtLog.AppendText($"Error updating document status: {statusResponse.StatusCode}\n{errorContent}\n");
                            return;
                        }
                    }
                    var updatePayload = new { PACKCODE = packCode };
                    string jsonPayload = JsonConvert.SerializeObject(updatePayload);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    // Make the HTTP PATCH request
                    HttpResponseMessage patchResponse = await client.PatchAsync(transOrderUrl, content);
                    if (patchResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Package updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectedRowForContextMenu.Cells["PACKNAME"].Value = packCode; // Update the DataGridView cell
                        // Revert the document status back to its original state if it was finalized
                        if (originalStatus == "סופית")
                        {
                            var revertStatusPayload = new { STATDES = originalStatus };
                            string revertPayload = JsonConvert.SerializeObject(revertStatusPayload);
                            var revertContent = new StringContent(revertPayload, Encoding.UTF8, "application/json");
                            HttpResponseMessage revertResponse = await client.PatchAsync(statusUrl, revertContent);
                            if (!revertResponse.IsSuccessStatusCode)
                            {
                                string errorContent = await revertResponse.Content.ReadAsStringAsync();
                                txtLog.AppendText($"Error reverting document status: {revertResponse.StatusCode}\n{errorContent}\n");
                            }
                        }
                    }
                    else
                    {
                        string errorContent = await patchResponse.Content.ReadAsStringAsync();
                        txtLog.AppendText($"Error updating package: {patchResponse.StatusCode}\n{errorContent}\n");
                    }
                }
                catch (HttpRequestException ex)
                {
                    txtLog.AppendText($"Request error: {ex.Message}\n");
                }
                catch (Exception ex)
                {
                    txtLog.AppendText($"An error occurred: {ex.Message}\n");
                }
            }
        }
        //private async void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0) // Ensure the row and column indices are valid and right mouse button is clicked
        //    {
        //        var selectedRow = dataGridView2.Rows[e.RowIndex];
        //        var clickedCell = selectedRow.Cells[e.ColumnIndex];
        //        // Check if the clicked cell is in the DOCDES column
        //        if (clickedCell.OwningColumn.Name == "DOCDES")
        //        {
        //            var docDesCell = clickedCell;
        //            var serialNameCell = selectedRow.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.OwningColumn.Name == "LOGDOCNO");
        //            if (docDesCell != null && docDesCell.Value != null && docDesCell.Value.ToString().Contains("נפוק"))
        //            {
        //                if (serialNameCell != null && serialNameCell.Value != null)
        //                {
        //                    string serialName = serialNameCell.Value.ToString();
        //                    //MessageBox.Show(serialName);
        //                    ShowSerialDetails(serialName);
        //                }
        //            }
        //        }
        //    }
        //}
        private async void dataGridView2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0) // Ensure the row and column indices are valid and right mouse button is clicked
            {
                var selectedRow = dataGridView2.Rows[e.RowIndex];
                var clickedCell = selectedRow.Cells[e.ColumnIndex];
                // Check if the clicked cell is in the DOCDES column
                if (clickedCell.OwningColumn.Name == "DOCDES")
                {
                    var docDesCell = clickedCell;
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
                // Check if the clicked cell is in the PACK column
                else if (clickedCell.OwningColumn.Name == "PACKNAME")
                {
                    //txtLog.AppendText($"Right-clicked on PACK cell\n");
                    contextMenuStrip.Tag = selectedRow; // Store the selected row in the context menu's Tag property
                    contextMenuStrip.Show(Cursor.Position);
                }
            }
            await Task.Delay(10); // Optional delay to allow the context menu to show
        }
        private async void ShowSerialDetails(string serialName)
        {
            string url = $"{baseUrl}/SERIAL?$filter=SERIALNAME eq '{serialName}'";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
                // Check if the IPN already exists
                int existingPartId = await CheckIfIPNExists(partName);
                if (existingPartId > 0)
                {
                    // IPN exists, check if the MFPN is different
                    //bool isMFPNDifferent = await CheckIfMFPNIsDifferent(existingPartId, partMFPN);
                    //if (isMFPNDifferent)
                    //{
                    //    // Patch the existing IPN to add the new MFPN
                    //   // await PatchExistingIPN(existingPartId, partMFPN, partDes);
                    //    MessageBox.Show("MFPN added to the existing IPN.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                    //else
                    //{
                        MessageBox.Show("IPN and MFPN already exist.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   // }
                }

                //if (existingPartId > 0)
                //{
                //    // IPN exists, check if the MFPN is different
                //    bool isMFPNDifferent = await CheckIfMFPNIsDifferent(existingPartId, partMFPN);
                //    if (isMFPNDifferent)
                //    {
                //        // Retrieve the manufacturer ID
                //        int? mnfId = await GetManufacturerId(existingPartId, partMNFName);
                //        if (mnfId.HasValue)
                //        {
                //            // Patch the existing IPN with the new MFPN
                //            //await PatchExistingIPN(existingPartId, mnfId.Value, partMFPN, partDes);

                //            await AddToPartMnfSubform(existingPartId, partMFPN, partDes, partMNFName, partMNFDes);
                //            MessageBox.Show("MFPN added to the existing IPN.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        }
                //        else
                //        {
                //            MessageBox.Show($"Manufacturer ID not found for {partMNFName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //        }
                //    }
                //    else
                //    {
                //        MessageBox.Show("IPN and MFPN already exist.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //}

                else
                {
                    // Insert into LOGPART and get the generated PART ID
                    int partId = await InsertLogPart(partName, partDes);
                    // Check if the manufacturer exists, if not, insert it and get the MNF ID
                    int mnfId = await GetOrInsertManufacturer(partMNFName, partMNFDes);
                    // Insert into PARTMNFONE
                    await InsertPartMnfOne(partId, partMFPN, mnfId, partDes);
                    // Fetch and display the inserted data
                    await DisplayInsertedData(partId);
                }
                stopwatch.Stop();
                // Update the ping label
                UpdatePing(stopwatch.ElapsedMilliseconds);
                btnClear.PerformClick();
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

        //private async void btnINSERTlogpart_Click(object sender, EventArgs e)
        //{
        //    string partName = txtbIPN.Text.Trim();
        //    string partDes = txtbDESC.Text.Trim();
        //    string partMFPN = txtbMFPN.Text.Trim().ToUpper();
        //    string partMNFDes = txtbMNF.Text.Trim().ToUpper();

        //    // Validate the required fields
        //    if (string.IsNullOrEmpty(partName) || string.IsNullOrEmpty(partDes) || string.IsNullOrEmpty(partMFPN) || string.IsNullOrEmpty(partMNFDes))
        //    {
        //        MessageBox.Show("Please ensure all fields are filled in before inserting.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    // Truncate MNFDES to fit within the 32-character limit
        //    if (partMNFDes.Length > 32)
        //    {
        //        partMNFDes = partMNFDes.Substring(0, 32);
        //    }

        //    // Generate MNFNAME by truncating MNFDES to fit within the 10-character limit
        //    string partMNFName = partMNFDes.Length > 10 ? partMNFDes.Substring(0, 10) : partMNFDes;

        //    try
        //    {
        //        // Measure the time taken for the HTTP POST request
        //        var stopwatch = Stopwatch.StartNew();

        //        // Check if the IPN already exists
        //        int existingPartId = await CheckIfIPNExists(partName);

        //        if (existingPartId > 0)
        //        {
        //            // IPN exists, check if the MFPN is different
        //            bool isMFPNDifferent = await CheckIfMFPNIsDifferent(existingPartId, partMFPN);
        //            if (isMFPNDifferent)
        //            {
        //                // Retrieve or insert the manufacturer ID
        //                //int mnfId = await GetOrInsertManufacturer(partMNFName, partMNFDes);

        //                // Add the new MFPN to the existing IPN
        //                await AddToPartMnfSubform(existingPartId, partMFPN, partDes, partMNFName, partMNFDes);
        //                MessageBox.Show("MFPN added to the existing IPN.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            else
        //            {
        //                MessageBox.Show("IPN and MFPN already exist.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //        }
        //        else
        //        {
        //            // Insert into LOGPART and get the generated PART ID
        //            int partId = await InsertLogPart(partName, partDes);

        //            // Retrieve or insert the manufacturer ID
        //            int mnfId = await GetOrInsertManufacturer(partMNFName, partMNFDes);

        //            // Insert into PARTMNFONE
        //            await InsertPartMnfOne(partId, partMFPN, mnfId, partDes);

        //            // Fetch and display the inserted data
        //            await DisplayInsertedData(partId);
        //        }

        //        stopwatch.Stop();

        //        // Update the ping label
        //        UpdatePing(stopwatch.ElapsedMilliseconds);

        //        // Clear the form
        //        btnClear.PerformClick();
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        //private async Task AddToPartMnfSubform(int partId, string partMFPN, string partDes, string mnfName, string mnfDes)
        //{
        //    // Construct the payload for the new PARTMNF_SUBFORM item
        //    var newPartMnfSubformItem = new
        //    {
        //        PART = partId,
        //        MNFPARTNAME = partMFPN,
        //        MNFPARTDES = partDes,
        //        MNFNAME = mnfName,
        //        MNFDES = mnfDes
        //    };

        //    // Construct the URL for the PARTMNFONE endpoint
        //    string url = $"{baseUrl}/PARTMNFONE";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        //        // Serialize the payload to JSON
        //        string jsonPayload = JsonConvert.SerializeObject(newPartMnfSubformItem);
        //        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        //        // Send the POST request
        //        HttpResponseMessage response = await client.PostAsync(url, content);

        //        // Handle the response
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            string errorContent = await response.Content.ReadAsStringAsync();
        //            throw new HttpRequestException($"Error adding to PARTMNF_SUBFORM: {response.StatusCode}\n{errorContent}");
        //        }
        //    }
        //}

        private async Task AddToPartMnfSubform(int partId, string partMFPN, string partDes, string mnfName, string mnfDes)
        {
            // Ensure the manufacturer name is unique
            string uniqueMnfName = await GetUniqueManufacturerName(mnfName);

            // Check if the manufacturer exists, if not, create it
            int mnfId = await EnsureManufacturerExists(uniqueMnfName, mnfDes);

            // Construct the payload for the new PARTMNF_SUBFORM item
            var newPartMnfSubformItem = new
            {
                PART = partId,
                MNFPARTNAME = partMFPN,
                MNFPARTDES = partDes,
                MNF = mnfId // Use the manufacturer ID
            };

            // Construct the URL for the PARTMNFONE endpoint
            string url = $"{baseUrl}/PARTMNFONE";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                // Serialize the payload to JSON
                string jsonPayload = JsonConvert.SerializeObject(newPartMnfSubformItem);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send the POST request
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Handle the response
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error adding to PARTMNF_SUBFORM: {response.StatusCode}\n{errorContent}");
                }
            }
        }

        private async Task<string> GetUniqueManufacturerName(string baseMnfName)
        {
            string uniqueMnfName = baseMnfName;
            int counter = 1;

            while (true)
            {
                // Check if the manufacturer exists
                string url = $"{baseUrl}/MNFCTR?$filter=MNFNAME eq '{uniqueMnfName}'";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);

                    // If no manufacturer is found, return the unique name
                    if (!apiResponse["value"].Any())
                    {
                        return uniqueMnfName;
                    }
                }

                // If the manufacturer exists, append a number and try again
                uniqueMnfName = $"{baseMnfName}{counter}";
                counter++;
            }
        }

        private async Task<int> EnsureManufacturerExists(string mnfName, string mnfDes)
        {
            // Check if the manufacturer exists
            string url = $"{baseUrl}/MNFCTR?$filter=MNFNAME eq '{mnfName}'";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);

                var manufacturer = apiResponse["value"].FirstOrDefault();
                if (manufacturer != null)
                {
                    // Return the existing manufacturer ID
                    return manufacturer["MNF"].Value<int>();
                }
            }

            // If the manufacturer does not exist, create it
            var mnfData = new
            {
                MNFNAME = mnfName,
                MNFDES = mnfDes
            };
            string insertUrl = $"{baseUrl}/MNFCTR";
            using (HttpClient client = new HttpClient())
            {
                string jsonMnfData = JsonConvert.SerializeObject(mnfData);
                var content = new StringContent(jsonMnfData, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                HttpResponseMessage response = await client.PostAsync(insertUrl, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<JObject>(responseBody);

                // Return the newly created manufacturer ID
                return responseData["MNF"].Value<int>();
            }
        }


        private async Task<int> CheckIfIPNExists(string partName)
        {
            string url = $"{baseUrl}/LOGPART?$filter=PARTNAME eq '{partName}'";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                var part = apiResponse["value"].FirstOrDefault();
                if (part != null)
                {
                    return part["PART"].Value<int>();
                }
                return 0;
            }
        }
        private async Task<bool> CheckIfMFPNIsDifferent(int partId, string partMFPN)
        {
            string url = $"{baseUrl}/PARTMNFONE?$filter=PART eq {partId}";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                var mfpn = apiResponse["value"].FirstOrDefault(p => p["MNFPARTNAME"].ToString() == partMFPN);
                return mfpn == null;
            }
        }
        //private async Task PatchExistingIPN(int partId, string partMFPN, string partDes)
        //{
        //    var partMnfOneData = new
        //    {
        //        PART = partId,
        //        MNFPARTNAME = partMFPN,
        //        MNFPARTDES = partDes
        //    };
        //    string url = $"{baseUrl}/PARTMNFONE";
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        //        string jsonPartMnfOneData = JsonConvert.SerializeObject(partMnfOneData);
        //        var content = new StringContent(jsonPartMnfOneData, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PatchAsync(url, content);
        //        response.EnsureSuccessStatusCode();
        //    }
        //}
        //private async Task PatchExistingIPN(int partId, string partMFPN, string partDes)
        //{
        //    // Construct the URL for the specific PARTMNFONE resource
        //    string url = $"{baseUrl}/PARTMNFONE(PART={partId},MNFPARTNAME='{partMFPN}')";

        //    // Construct the payload for the PATCH request
        //    var partMnfOneData = new
        //    {
        //        MNFPARTDES = partDes // Update only the description for the existing MFPN
        //    };

        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        //        // Serialize the payload to JSON
        //        string jsonPartMnfOneData = JsonConvert.SerializeObject(partMnfOneData);
        //        var content = new StringContent(jsonPartMnfOneData, Encoding.UTF8, "application/json");

        //        // Send the PATCH request
        //        HttpResponseMessage response = await client.PatchAsync(url, content);

        //        // Handle the response
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            string errorContent = await response.Content.ReadAsStringAsync();
        //            throw new HttpRequestException($"Error updating IPN: {response.StatusCode}\n{errorContent}");
        //        }
        //    }
        //}

        //private async Task PatchExistingIPN(int partId, int mnfId, string partMFPN, string partDes)
        //{
        //    // Construct the URL for the specific PARTMNFONE resource
        //    string url = $"{baseUrl}/PARTMNFONE(PART={partId},MNF={mnfId})";

        //    // Construct the payload for the PATCH request
        //    var partMnfOneData = new
        //    {
        //        MNFPARTNAME = partMFPN, // Update the MFPN if needed
        //        MNFPARTDES = partDes    // Update the description
        //    };

        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        //        // Serialize the payload to JSON
        //        string jsonPartMnfOneData = JsonConvert.SerializeObject(partMnfOneData);
        //        var content = new StringContent(jsonPartMnfOneData, Encoding.UTF8, "application/json");

        //        // Send the PATCH request
        //        HttpResponseMessage response = await client.PatchAsync(url, content);

        //        // Handle the response
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            string errorContent = await response.Content.ReadAsStringAsync();
        //            throw new HttpRequestException($"Error updating IPN: {response.StatusCode}\n{errorContent}");
        //        }
        //    }
        //}
        private async Task<int?> GetManufacturerId(int partid, string partMFPN)
        {
            // Construct the URL to query the PART entity and expand the PARTMNF_SUBFORM
            string url = $"{baseUrl}/PART?$filter=PART eq '{partid}'&$expand=PARTMNF_SUBFORM";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                // Send the GET request
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                var part = apiResponse["value"]?.FirstOrDefault();

                if (part != null)
                {
                    // Look for the matching MFPN in the PARTMNF_SUBFORM
                    var manufacturer = part["PARTMNF_SUBFORM"]?
                        .FirstOrDefault(m => m["MNFPARTNAME"]?.ToString() == partMFPN);

                    if (manufacturer != null)
                    {
                        // Return the MNF (manufacturer ID)
                        return manufacturer["MNF"]?.Value<int>();
                    }
                }

                // Return null if no matching manufacturer is found
                return null;
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
            string url = $"{baseUrl}/LOGPART";
            using (HttpClient client = new HttpClient())
            {
                // Set the request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Set the Authorization header
                //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
            string url = $"{baseUrl}/MNFCTR?$filter=MNFNAME eq '{partMNFName}'";
            using (HttpClient client = new HttpClient())
            {
                // Set the request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Set the Authorization header
                //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
                    string insertUrl = $"{baseUrl}/MNFCTR";
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
            string url = $"{baseUrl}/PARTMNFONE";
            using (HttpClient client = new HttpClient())
            {
                // Set the request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Set the Authorization header
                //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
            string url = $"{baseUrl}/PARTMNFONE?$filter=PART eq {partId}";
            using (HttpClient client = new HttpClient())
            {
                // Set the request headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Set the Authorization header
                //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
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
            if (cmbWarehouseList.SelectedItem != null && txtbInputIPN.Text != string.Empty && txtbInputMFPN.Text != string.Empty && txtbPartDescription.Text != string.Empty && txtbManufacturer.Text != string.Empty && int.Parse(txtbInputQty.Text) > 0 && int.Parse(txtbInputQty.Text) <= 50000)
            {
                string selectedWarehouseName = cmbWarehouseList.SelectedItem.ToString().Split(' ')[0];
                var selectedWarehouse = loadedWareHouses.FirstOrDefault(w => w.WARHSNAME == selectedWarehouseName);
                if (selectedWarehouse != null)
                {
                    string _BOOKNUM = string.Empty;
                    string _OWNERLOGIN = "Yuri_G";
                    string _SUPNAME = string.Empty;
                    if (rbtIN.Checked)
                    {
                        if (txtbINdoc.Text == string.Empty)
                        {
                            MessageBox.Show("Please enter the supplier document description", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtbINdoc.Focus();
                            return;
                        }
                        else
                        {
                            _BOOKNUM = txtbINdoc.Text;
                            _SUPNAME = "CLIENT";
                        }
                    }
                    else if (tbtOUT.Checked)
                    {
                        if (txtbOUT.Text == string.Empty)
                        {
                            MessageBox.Show("Please enter the requester", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        _BOOKNUM = txtbINdoc.Text;
                        _SUPNAME = "FTK";
                    }
                    if (tbtOUT.Checked)
                    {
                        // Create a new TDocument object for outgoing transactions
                        TDocument documentT = new TDocument
                        {
                            USERLOGIN = _OWNERLOGIN,
                            TYPE = "T", // Set the document type
                            CURDATE = DateTimeOffset.UtcNow,
                            WARHSNAME = selectedWarehouse.WARHSNAME,
                            BOOKNUM = _BOOKNUM, // Set the supplier number
                            TOWARHSNAME = "Flr",
                            TRANSORDER_T_SUBFORM = new List<TransOrder>
                    {
                        new TransOrder
                        {
                            PARTNAME = txtbInputIPN.Text,
                            QUANT = int.Parse(txtbInputQty.Text),
                            PACKCODE = cmbPackCode.SelectedItem != null ? cmbPackCode.SelectedItem.ToString() : "Bag",
                            UNITNAME = "יח'"
                        }
                    }
                        };
                        await WarehouseService.TransfertDocumentAsync(documentT, this, settings);
                    }
                    else
                    {
                        // Create a new Document object for incoming and manufacturing transactions
                        Document Pdocument = new Document
                        {
                            USERLOGIN = _OWNERLOGIN,
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
                            QUANT = int.Parse(txtbInputQty.Text),
                            PACKCODE = cmbPackCode.SelectedItem != null ? cmbPackCode.SelectedItem.ToString() : "Bag",
                            UNITNAME = "יח'"
                        }
                    }
                        };
                        await WarehouseService.InsertDocumentAsync(Pdocument, this, settings);
                    }
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
                string generatedDate = DateTime.Now.ToString("yy00MMdd");
                txtbINdoc.Text = ($"WR{generatedDate}");
                btnMFG.Text = "INCOMING";
                btnMFG.Update();
                txtbINdoc.ReadOnly = false;
                txtbINdoc.Focus();
            }
            else
            {
                txtbINdoc.ReadOnly = true;
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
                txtbINdoc.Text = string.Empty;
                cbmOUT.DroppedDown = true;
                txtbOUT.Focus();
            }
            else
            {
                txtbOUT.Text = string.Empty;
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
                txtbOUT.Text = string.Empty;
                txtbOUT.ReadOnly = true;
                txtbINdoc.Text = string.Empty;
                txtbINdoc.ReadOnly = true;
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
                string generatedDate = DateTime.Now.ToString("yy00MMdd");
                txtbINdoc.Text = ($"FTK{generatedDate}");
                btnMFG.Text = "FTK";
                btnMFG.Update();
                txtbOUT.ReadOnly = true;
                txtbINdoc.ReadOnly = true;
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
                            txtLog.AppendText($"Row {row}: Part Name = {partName}, Part Description = {partDes}, Manufacturer Part Number = {partMFPN}, Manufacturer Description = {partMNFDes} , Exception = {ex.Message}\n");
                            txtLog.ScrollToCaret();
                            //MessageBox.Show($"Row {row}: Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            // Log the extracted values
                            txtLog.SelectionColor = Color.Red; // Set the color to acid green
                            txtLog.AppendText($"Row {row}: Part Name = {partName}, Part Description = {partDes}, Manufacturer Part Number = {partMFPN}, Manufacturer Description = {partMNFDes}, Exception = {ex.Message}\n");
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
            dataView.RowFilter = string.Empty; // Clear the filter
            txtbFilterIPN.Focus();
            ColorTheRows(dataGridView1);
        }
        private async void btnGetMFPNs_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                await ExtractMFPNForRow(row);
            }
        }
        TextBox lastUserInput = null;
        private void txtbDecoder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lastUserInput = txtbDecoder;
                string decoderText = txtbDecoder.Text;
                string preCode = cmbPreCode.Text;
                string postCode = cmbPostCode.Text;
                if (!string.IsNullOrEmpty(preCode) && !string.IsNullOrEmpty(postCode))
                {
                    int startIndex = decoderText.IndexOf(preCode) + preCode.Length;
                    int endIndex = decoderText.IndexOf(postCode, startIndex);
                    if (startIndex >= preCode.Length && endIndex > startIndex)
                    {
                        string extractedText = decoderText.Substring(startIndex, endIndex - startIndex);
                        txtbInputMFPN.Text = extractedText;
                        txtbInputMFPN.Focus();
                        // Simulate ENTER key press on txtbInputMFPN
                        txtbInputMFPN_KeyDown(txtbInputMFPN, new KeyEventArgs(Keys.Enter), txtbDecoder);
                        txtbDecoder.Clear();
                        lastUserInput.Focus();
                    }
                    else
                    {
                        MessageBox.Show("PreCode or PostCode not found in the input text.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (!string.IsNullOrEmpty(preCode))
                {
                    int startIndex = decoderText.IndexOf(preCode) + preCode.Length;
                    if (startIndex >= preCode.Length)
                    {
                        string extractedText = decoderText.Substring(startIndex);
                        txtbInputMFPN.Text = extractedText;
                        txtbInputMFPN.Focus();
                        // Simulate ENTER key press on txtbInputMFPN
                        txtbInputMFPN_KeyDown(txtbInputMFPN, new KeyEventArgs(Keys.Enter), txtbDecoder);
                        txtbDecoder.Clear();
                        lastUserInput.Focus();
                    }
                    else
                    {
                        MessageBox.Show("PreCode not found in the input text.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (!string.IsNullOrEmpty(postCode))
                {
                    int endIndex = decoderText.IndexOf(postCode);
                    if (endIndex > 0)
                    {
                        string extractedText = decoderText.Substring(0, endIndex);
                        txtbInputMFPN.Text = extractedText;
                        txtbInputMFPN.Focus();
                        // Simulate ENTER key press on txtbInputMFPN
                        txtbInputMFPN_KeyDown(txtbInputMFPN, new KeyEventArgs(Keys.Enter), txtbDecoder);
                        txtbDecoder.Clear();
                        lastUserInput.Focus();
                    }
                    else
                    {
                        MessageBox.Show("PostCode not found in the input text.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select at least one of PreCode or PostCode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void txtbDecodeIPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string unprefixedIPN = txtbDecodeIPN.Text;
                string prefix = txtbPrefix.Text;
                // Determine the appropriate separator based on the first row of dataGridView1
                string separator = "_"; // Default separator
                if (dataGridView1.Rows.Count > 0)
                {
                    var firstRow = dataGridView1.Rows[0];
                    if (firstRow.Cells["PARTNAME"].Value != null)
                    {
                        string firstRowPartName = firstRow.Cells["PARTNAME"].Value.ToString();
                        if (firstRowPartName.Length > 3 && firstRowPartName[3] == '-')
                        {
                            separator = "-";
                        }
                    }
                }
                // Construct the valid IPN using the prefix and the unprefixed IPN
                string validIPN = $"{prefix}{separator}{unprefixedIPN}";
                // Copy the constructed IPN into the txtbInputIPN textbox
                txtbInputIPN.Text = validIPN;
                // Simulate ENTER key press on txtbInputIPN to trigger the original logic
                txtbInputIPN_KeyDown(txtbInputIPN, new KeyEventArgs(Keys.Enter));
                // Clear the txtbDecodeIPN textbox
                txtbDecodeIPN.Clear();
                lastUserInput = txtbDecodeIPN;
            }
        }
        private void txtbINdoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtbINdoc.Text != string.Empty)
            {
                txtbInputQty.Focus();
            }
        }
        private void chkbNoSticker_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbNoSticker.Checked)
            {
                chkbNoSticker.BackColor = Color.IndianRed;
            }
        }
        //private void cbmOUT_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if(cbmOUT.SelectedItem.Equals("SENT TO"))
        //    {
        //        string clientName = cmbWarehouseList.SelectedItem.ToString().Substring(10).Trim();
        //        txtbOUT.Text = cbmOUT.SelectedItem + " "+ clientName;
        //    }
        //    else
        //    {
        //        txtbOUT.Text = cbmOUT.SelectedItem.ToString();
        //    }
        //    txtbInputIPN.Focus();
        //}
        private void cbmOUT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbmOUT.SelectedItem.Equals("SENT TO"))
            {
                // Open the provided link in the default browser
                string url = "https://p.priority-connect.online/webui/openmail.htm?tenant=zad51&priority:priform@DOCUMENTS_D:NEW:a020522:tabzad51.ini:1";
                try
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true // Ensures the URL is opened in the default browser
                        }
                    };
                    process.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open the link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                txtbOUT.Text = cbmOUT.SelectedItem.ToString();
            }
            txtbInputIPN.Focus();
        }
        private void txtbInputMFPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtbInputMFPN_KeyDown(txtbInputMFPN, new KeyEventArgs(Keys.Enter), txtbInputMFPN);
            }
        }
        private void btnPrintStock_Click(object sender, EventArgs e)
        {
            // Get the selected warehouse name
            string selectedWarehouseName = GetSelectedWarehouseName();
            if (string.IsNullOrEmpty(selectedWarehouseName))
            {
                MessageBox.Show("Please select a warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Generate HTML report
            txtLog.AppendText($"Generating HTML report for {selectedWarehouseName} \n");
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\{selectedWarehouseName}_StockReport_{_fileTimeStamp}.html";
            GenerateHTMLFromDataGridView(filename, dataGridView1, $"{selectedWarehouseName} Stock Report {_fileTimeStamp}");
            // Open the file in default browser
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void GenerateHTMLFromDataGridView(string filename, DataGridView dataGridView, string reportTitle)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='text-align:center;background-color:gray;color:white;'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>Stock Report</title>");
                writer.WriteLine("<style>");
                writer.WriteLine("table { border-collapse: collapse; width: 100%; border: solid 1px; }");
                writer.WriteLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
                writer.WriteLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
                writer.WriteLine(".green { background-color: lightgreen; color: black; }");
                writer.WriteLine(".zero { background-color: indianred; color: white; }");
                writer.WriteLine(".negative { background-color: red; color: white; }");
                writer.WriteLine(".header-table td { font-size: 2em; font-weight: bold; }");
                writer.WriteLine("</style>");
                writer.WriteLine("<script>");
                writer.WriteLine("function filterTable() {");
                writer.WriteLine("  var input, filter, table, tr, td, i, j, txtValue;");
                writer.WriteLine("  input = document.getElementById('searchInput');");
                writer.WriteLine("  filter = input.value.toLowerCase();");
                writer.WriteLine("  table = document.getElementById('stockTable');");
                writer.WriteLine("  tr = table.getElementsByTagName('tr');");
                writer.WriteLine("  for (i = 1; i < tr.length; i++) {");
                writer.WriteLine("    tr[i].style.display = 'none';");
                writer.WriteLine("    td = tr[i].getElementsByTagName('td');");
                writer.WriteLine("    for (j = 0; j < td.length; j++) {");
                writer.WriteLine("      if (td[j]) {");
                writer.WriteLine("        txtValue = td[j].textContent || td[j].innerText;");
                writer.WriteLine("        if (txtValue.toLowerCase().indexOf(filter) > -1) {");
                writer.WriteLine("          tr[i].style.display = '';");
                writer.WriteLine("          break;");
                writer.WriteLine("        }");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("function clearSearch() {");
                writer.WriteLine("  document.getElementById('searchInput').value = '';");
                writer.WriteLine("  filterTable();");
                writer.WriteLine("}");
                writer.WriteLine("function sortTable(n, isNumeric) {");
                writer.WriteLine("  var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;");
                writer.WriteLine("  table = document.getElementById('stockTable');");
                writer.WriteLine("  switching = true;");
                writer.WriteLine("  dir = 'asc';");
                writer.WriteLine("  while (switching) {");
                writer.WriteLine("    switching = false;");
                writer.WriteLine("    rows = table.rows;");
                writer.WriteLine("    for (i = 1; i < (rows.length - 1); i++) {");
                writer.WriteLine("      shouldSwitch = false;");
                writer.WriteLine("      x = rows[i].getElementsByTagName('TD')[n];");
                writer.WriteLine("      y = rows[i + 1].getElementsByTagName('TD')[n];");
                writer.WriteLine("      if (isNumeric) {");
                writer.WriteLine("        if (dir == 'asc') {");
                writer.WriteLine("          if (parseFloat(x.innerHTML) > parseFloat(y.innerHTML)) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        } else if (dir == 'desc') {");
                writer.WriteLine("          if (parseFloat(x.innerHTML) < parseFloat(y.innerHTML)) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        }");
                writer.WriteLine("      } else {");
                writer.WriteLine("        if (dir == 'asc') {");
                writer.WriteLine("          if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        } else if (dir == 'desc') {");
                writer.WriteLine("          if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        }");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("    if (shouldSwitch) {");
                writer.WriteLine("      rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);");
                writer.WriteLine("      switching = true;");
                writer.WriteLine("      switchcount ++;");
                writer.WriteLine("    } else {");
                writer.WriteLine("      if (switchcount == 0 && dir == 'asc') {");
                writer.WriteLine("        dir = 'desc';");
                writer.WriteLine("        switching = true;");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine($"<h1>{reportTitle}</h1>");
                // Calculate the total balance
                int totalBalance = dataGridView.Rows.Cast<DataGridViewRow>()
                    .Sum(row => Convert.ToInt32(row.Cells["BALANCE"].Value));
                writer.WriteLine($"<div>Displaying {dataGridView.RowCount} rows. Total Balance:{totalBalance} items</div>");
                writer.WriteLine("<input type='text' id='searchInput' onkeyup='filterTable()' placeholder='Search for keywords..' style='margin-bottom: 10px;'>");
                writer.WriteLine("<button onclick='clearSearch()'>Clear</button>");
                writer.WriteLine("<table id='stockTable'>");
                writer.WriteLine("<tr>");
                // Add table headers
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    bool isNumeric = column.Name == "BALANCE" || column.Name == "PART";
                    writer.WriteLine($"<th onclick='sortTable({column.Index}, {isNumeric.ToString().ToLower()})'>{column.HeaderText}</th>");
                }
                writer.WriteLine("</tr>");
                // Order the rows by BALANCE column values in descending order
                var orderedRows = dataGridView.Rows.Cast<DataGridViewRow>()
                    .OrderByDescending(r => Convert.ToInt32(r.Cells["BALANCE"].Value))
                    .ToList();
                // Add table rows
                foreach (DataGridViewRow row in orderedRows)
                {
                    writer.WriteLine("<tr>");
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string cellValue = cell.Value?.ToString() ?? string.Empty;
                        string cellClass = string.Empty;
                        if (cell.OwningColumn.Name == "BALANCE" && int.TryParse(cellValue, out int balanceValue))
                        {
                            if (balanceValue > 0)
                            {
                                cellClass = "green";
                            }
                            else if (balanceValue == 0)
                            {
                                cellClass = "zero";
                            }
                            else
                            {
                                cellClass = "negative";
                            }
                        }
                        writer.WriteLine($"<td class='{cellClass}'>{cellValue}</td>");
                    }
                    writer.WriteLine("</tr>");
                }
                writer.WriteLine("</table>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
        }
        private string GetSelectedWarehouseName()
        {
            if (cmbWarehouseList.SelectedIndex >= 0)
            {
                string selectedItem = cmbWarehouseList.SelectedItem.ToString();
                return selectedItem.Split('-')[0].Trim();
            }
            return null;
        }
        private void btnPrintIPNmoves_Click(object sender, EventArgs e)
        {
            // Get the IPN from groupBox4.Text
            string ipn = groupBox4.Text.Replace("Stock Movements for ", "").Trim();
            if (string.IsNullOrEmpty(ipn))
            {
                MessageBox.Show("No IPN found in the group box title.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Get the balance for the IPN from dataGridView1
            int balance = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["PARTNAME"].Value.ToString() == ipn)
                {
                    balance = Convert.ToInt32(row.Cells["BALANCE"].Value);
                    break;
                }
            }
            // Generate HTML report
            txtLog.AppendText($"Generating HTML report for stock movements of {ipn} \n");
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\{ipn}_StockMovements_{_fileTimeStamp}.html";
            GenerateHTMLFromDataGridView(filename, dataGridView2, $"STOCK MOVEMENTS FOR {ipn}", balance);
            // Open the file in default browser
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void GenerateHTMLFromDataGridView(string filename, DataGridView dataGridView, string reportTitle, int balance)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='text-align:center;background-color:gray;color:white;'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>Stock Movements Report</title>");
                writer.WriteLine("<style>");
                writer.WriteLine("table { border-collapse: collapse; width: 100%; border: solid 1px; }");
                writer.WriteLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
                writer.WriteLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
                writer.WriteLine(".green { background-color: lightgreen; color: black; }");
                writer.WriteLine(".zero { background-color: indianred; color: white; }");
                writer.WriteLine(".negative { background-color: red; color: white; }");
                writer.WriteLine(".header-table td { font-size: 2em; font-weight: bold; }");
                writer.WriteLine("</style>");
                writer.WriteLine("<script>");
                writer.WriteLine("function filterTable() {");
                writer.WriteLine("  var input, filter, table, tr, td, i, j, txtValue;");
                writer.WriteLine("  input = document.getElementById('searchInput');");
                writer.WriteLine("  filter = input.value.toLowerCase();");
                writer.WriteLine("  table = document.getElementById('stockTable');");
                writer.WriteLine("  tr = table.getElementsByTagName('tr');");
                writer.WriteLine("  for (i = 1; i < tr.length; i++) {");
                writer.WriteLine("    tr[i].style.display = 'none';");
                writer.WriteLine("    td = tr[i].getElementsByTagName('td');");
                writer.WriteLine("    for (j = 0; j < td.length; j++) {");
                writer.WriteLine("      if (td[j]) {");
                writer.WriteLine("        txtValue = td[j].textContent || td[j].innerText;");
                writer.WriteLine("        if (txtValue.toLowerCase().indexOf(filter) > -1) {");
                writer.WriteLine("          tr[i].style.display = '';");
                writer.WriteLine("          break;");
                writer.WriteLine("        }");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("function clearSearch() {");
                writer.WriteLine("  document.getElementById('searchInput').value = '';");
                writer.WriteLine("  filterTable();");
                writer.WriteLine("}");
                writer.WriteLine("function sortTable(n, isNumeric) {");
                writer.WriteLine("  var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;");
                writer.WriteLine("  table = document.getElementById('stockTable');");
                writer.WriteLine("  switching = true;");
                writer.WriteLine("  dir = 'asc';");
                writer.WriteLine("  while (switching) {");
                writer.WriteLine("    switching = false;");
                writer.WriteLine("    rows = table.rows;");
                writer.WriteLine("    for (i = 1; i < (rows.length - 1); i++) {");
                writer.WriteLine("      shouldSwitch = false;");
                writer.WriteLine("      x = rows[i].getElementsByTagName('TD')[n];");
                writer.WriteLine("      y = rows[i + 1].getElementsByTagName('TD')[n];");
                writer.WriteLine("      if (isNumeric) {");
                writer.WriteLine("        if (dir == 'asc') {");
                writer.WriteLine("          if (parseFloat(x.innerHTML) > parseFloat(y.innerHTML)) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        } else if (dir == 'desc') {");
                writer.WriteLine("          if (parseFloat(x.innerHTML) < parseFloat(y.innerHTML)) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        }");
                writer.WriteLine("      } else {");
                writer.WriteLine("        if (dir == 'asc') {");
                writer.WriteLine("          if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        } else if (dir == 'desc') {");
                writer.WriteLine("          if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        }");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("    if (shouldSwitch) {");
                writer.WriteLine("      rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);");
                writer.WriteLine("      switching = true;");
                writer.WriteLine("      switchcount ++;");
                writer.WriteLine("    } else {");
                writer.WriteLine("      if (switchcount == 0 && dir == 'asc') {");
                writer.WriteLine("        dir = 'desc';");
                writer.WriteLine("        switching = true;");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine($"<h1>{reportTitle}</h1>");
                int intRowsDisplayed = 0;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["DOCDES"].Value.ToString() != "קיזוז אוטומטי")
                    {
                        intRowsDisplayed++;
                    }
                }
                writer.WriteLine($"<div>Displaying {intRowsDisplayed} rows</div>");
                writer.WriteLine("<input type='text' id='searchInput' onkeyup='filterTable()' placeholder='Search for keywords..' style='margin-bottom: 10px;'>");
                writer.WriteLine("<button onclick='clearSearch()'>Clear</button>");
                // Generate the "CURRENT STOCK TABLE"
                writer.WriteLine($"<h2>Current Stock : {balance}</h2>");
                writer.WriteLine("<table id='currentStockTable'>");
                writer.WriteLine("<tr>");
                // Add table headers
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    bool isNumeric = column.Name == "TQUANT";
                    writer.WriteLine($"<th onclick='sortTable({column.Index}, {isNumeric.ToString().ToLower()})'>{column.HeaderText}</th>");
                }
                writer.WriteLine("</tr>");
                // Add table rows for current stock
                var currentStockRows = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["DOCDES"].Value.ToString() == "קיזוז אוטומטי")
                    {
                        continue;
                    }
                    else
                    {
                        string rowDocType = row.Cells["LOGDOCNO"].Value?.ToString() ?? string.Empty;
                        if (rowDocType.StartsWith("GR") && int.TryParse(row.Cells["TQUANT"].Value?.ToString(), out int balanceValue) && balanceValue > 0)
                        {
                            bool hasOutgoingMovement = dataGridView.Rows.Cast<DataGridViewRow>().Any(r =>
        (r.Cells["LOGDOCNO"].Value?.ToString().StartsWith("ROB") == true ||
         r.Cells["LOGDOCNO"].Value?.ToString().StartsWith("RD") == true ||
         r.Cells["LOGDOCNO"].Value?.ToString().StartsWith("SH") == true ||
         r.Cells["LOGDOCNO"].Value?.ToString().StartsWith("IC") == true) &&
        (r.Cells["LOGDOCNO"].Value?.ToString().StartsWith("IC") == true
            ? Math.Abs(Convert.ToInt32(r.Cells["TQUANT"].Value)) == Math.Abs(Convert.ToInt32(row.Cells["TQUANT"].Value))
            : r.Cells["TQUANT"].Value?.ToString() == row.Cells["TQUANT"].Value?.ToString())
    );
                            if (!hasOutgoingMovement)
                            {
                                currentStockRows.Add(row);
                            }
                        }
                    }
                }
                foreach (var row in currentStockRows)
                {
                    if (row.Cells["DOCDES"].Value.ToString() == "קיזוז אוטומטי")
                    {
                        continue;
                    }
                    else
                    {
                        writer.WriteLine("<tr>");
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            string cellValue = cell.Value?.ToString() ?? string.Empty;
                            string cellClass = string.Empty;
                            if (cell.OwningColumn.Name == "TQUANT" && int.TryParse(cellValue, out int balanceValue))
                            {
                                if (balanceValue > 0)
                                {
                                    cellClass = "green";
                                }
                                else if (balanceValue == 0)
                                {
                                    cellClass = "zero";
                                }
                                else if (balanceValue < 0)
                                {
                                    cellClass = "negative";
                                }
                            }
                            writer.WriteLine($"<td class='{cellClass}'>{cellValue}</td>");
                        }
                        writer.WriteLine("</tr>");
                    }
                }
                writer.WriteLine("</table>");
                // Generate the existing stock movements table
                writer.WriteLine("<h2>Stock Movements</h2>");
                writer.WriteLine("<table id='stockTable'>");
                writer.WriteLine("<tr>");
                // Add table headers
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    bool isNumeric = column.Name == "TQUANT";
                    writer.WriteLine($"<th onclick='sortTable({column.Index}, {isNumeric.ToString().ToLower()})'>{column.HeaderText}</th>");
                }
                writer.WriteLine("</tr>");
                // Add table rows for stock movements
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["DOCDES"].Value.ToString() == "קיזוז אוטומטי")
                    {
                        continue;
                    }
                    else
                    {
                        writer.WriteLine("<tr>");
                        string rowDocType = row.Cells["LOGDOCNO"].Value?.ToString() ?? string.Empty;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            string cellValue = cell.Value?.ToString() ?? string.Empty;
                            string cellClass = string.Empty;
                            if (cell.OwningColumn.Name == "TQUANT" && int.TryParse(cellValue, out int balanceValue))
                            {
                                if (balanceValue > 0 && rowDocType.StartsWith("GR"))
                                {
                                    cellClass = "green";
                                }
                                else if (balanceValue == 0)
                                {
                                    cellClass = "zero";
                                }
                                else if (balanceValue > 0 && !rowDocType.StartsWith("GR") || balanceValue < 0)
                                {
                                    cellClass = "negative";
                                }
                            }
                            writer.WriteLine($"<td class='{cellClass}'>{cellValue}</td>");
                        }
                        writer.WriteLine("</tr>");
                    }
                }
                writer.WriteLine("</table>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
        }
        private void btnPandatabaseSearch_Click(object sender, EventArgs e)
        {
            FrmPriorityPanDbSearch frmPriorityPanDbSearch = new FrmPriorityPanDbSearch();
            frmPriorityPanDbSearch.Show();
        }
        private async void btnDIGIAPI_Click(object sender, EventArgs e)
        {
            string apiUrl = "https://api.digikey.com/products/v4/search/keyword";
            string clientId = "1V0C9rxhmIcEf28EC6ADmF9avL74IDF0"; // Replace with your actual client ID
            string clientSecret = "bbNRuLqxaxjN87AQ"; // Replace with your actual client secret
            string keyword = txtbMFPN.Text; // Get the keyword from the txtbMFPN
            txtLog.AppendText("Getting access token...\n");
            string accessTokenReceived = string.Empty;
            try
            {
                accessTokenReceived = await GetAccessTokenAsync(clientId, clientSecret);
                txtLog.AppendText($"Access token obtained successfully: {accessTokenReceived}\n");
            }
            catch (Exception ex)
            {
                txtLog.AppendText($"Error obtaining access token: {ex.Message}\n");
                return;
            }
            var requestData = new
            {
                Keywords = keyword,
                Limit = 10,
                Offset = 0,
                FilterOptionsRequest = new
                {
                    ManufacturerFilter = new List<object>(),
                    CategoryFilter = new List<object>(),
                    StatusFilter = new List<object>(),
                    PackagingFilter = new List<object>(),
                    MarketPlaceFilter = "NoFilter",
                    SeriesFilter = new List<object>(),
                    MinimumQuantityAvailable = 0,
                    ParameterFilterRequest = new
                    {
                        CategoryFilter = new { Id = "string" },
                        ParameterFilters = new List<object>()
                    },
                    SearchOptions = new List<string> { }
                },
                SortOptions = new
                {
                    Field = "None",
                    SortOrder = "Ascending"
                }
            };
            var jsonRequest = System.Text.Json.JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenReceived);
                client.DefaultRequestHeaders.Add("X-DIGIKEY-Client-Id", clientId);
                txtLog.AppendText("Sending search request...\n");
                txtLog.AppendText($"Request URL: {apiUrl}\n");
                txtLog.AppendText($"Request Data: {jsonRequest}\n");
                HttpResponseMessage response = null;
                try
                {
                    response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();
                    txtLog.AppendText("Search request successful.\n");
                }
                catch (HttpRequestException ex)
                {
                    string errorContent = response != null ? await response.Content.ReadAsStringAsync() : "No response content";
                    txtLog.AppendText($"Error sending search request: {ex.Message}\n");
                    txtLog.AppendText($"Response Content: {errorContent}\n");
                    return;
                }
                string jsonResponse;
                try
                {
                    jsonResponse = await response.Content.ReadAsStringAsync();
                    txtLog.AppendText("Response received successfully.\n");
                    txtLog.AppendText($"JSON Response: {jsonResponse}\n"); // Log the JSON response contents
                }
                catch (Exception ex)
                {
                    txtLog.AppendText($"Error reading response: {ex.Message}\n");
                    return;
                }
                KeywordResponse keywordResponse;
                try
                {
                    keywordResponse = System.Text.Json.JsonSerializer.Deserialize<KeywordResponse>(jsonResponse);
                    txtLog.AppendText("Response deserialized successfully.\n");
                }
                catch (Exception ex)
                {
                    txtLog.AppendText($"Error deserializing response: {ex.Message}\n");
                    return;
                }
                // Map to simplified products
                var simplifiedProducts = keywordResponse.ExactMatches.Select(p => new
                {
                    Description = p.Description.ProductDescription,
                    Manufacturer = p.Manufacturer.Name
                }).ToList();
                // Log the contents of the simplified products list
                txtLog.AppendText($"Products Count: {simplifiedProducts.Count}\n");
                foreach (var product in simplifiedProducts)
                {
                    txtLog.AppendText($"Manufacturer: {product.Manufacturer}, Description: {product.Description}\n");
                    txtbDESC.Text = product.Description;
                    txtbMNF.Text = product.Manufacturer.ToUpper();
                }
            }
        }
        private async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
        {
            string tokenUrl = "https://api.digikey.com/v1/oauth2/token";
            var requestBody = new Dictionary<string, string>
    {
        { "client_id", clientId },
        { "client_secret", clientSecret },
        { "grant_type", "client_credentials" }
    };
            using (HttpClient client = new HttpClient())
            {
                var requestContent = new FormUrlEncodedContent(requestBody);
                HttpResponseMessage response = await client.PostAsync(tokenUrl, requestContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(responseBody);
                return tokenResponse.access_token;
            }
        }
        public class KeywordResponse
        {
            public List<Product> ExactMatches { get; set; }
        }
        public class Product
        {
            public Description Description { get; set; }
            public Manufacturer Manufacturer { get; set; }
        }
        public class Description
        {
            public string ProductDescription { get; set; }
            public string DetailedDescription { get; set; }
        }
        public class Manufacturer
        {
            public string Name { get; set; }
        }
        public class TokenResponse
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }
        }
        private async void btnAVL_Click(object sender, EventArgs e)
        {
            string selectedPrefix = txtbPrefix.Text.Trim();
            var prefixExceptions = new List<string> { "Flr", "666", "400", "450", "500", "501", "550", "600", "650", "Main", "Outl", "Trn","MRB" };
            // Check if the selected prefix is valid
            if (prefixExceptions.Contains(selectedPrefix))
            {
                MessageBox.Show($"The prefix '{selectedPrefix}' is not valid for AVL.", "Invalid Prefix", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Change the button text to include the prefix
            btnAVL.Text = $"{selectedPrefix} AVL";
            // Make the API call to fetch the data
            string apiUrl = $"{baseUrl}/PARTMNFONE?$filter=PARTNAME eq '{selectedPrefix}_*'";
            List<PartMnfOne> partMnfOnes = new List<PartMnfOne>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<PartMnfOneApiResponse>(responseBody);
                    partMnfOnes = apiResponse.value;
                }
                catch (HttpRequestException ex)
                {
                    txtLog.AppendText($"Request error: {ex.Message}\n");
                    return;
                }
                catch (Exception ex)
                {
                    txtLog.AppendText($"An error occurred: {ex.Message}\n");
                    return;
                }
            }
            // Generate HTML report
            txtLog.AppendText($"Generating HTML report for {selectedPrefix} AVL\n");
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\{selectedPrefix}_AVLReport_{_fileTimeStamp}.html";
            List<PartMnfOne> orderedbyIPN = partMnfOnes.OrderBy(x => x.PARTNAME).ToList();
            GenerateHTMLFromPartMnfOneList(filename, orderedbyIPN, $"{selectedPrefix} AVL Report {_fileTimeStamp}");
            // Open the file in default browser
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void GenerateHTMLFromPartMnfOneList(string filename, List<PartMnfOne> partMnfOnes, string reportTitle)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='text-align:center;background-color:gray;color:white;'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>AVL Report</title>");
                writer.WriteLine("<style>");
                writer.WriteLine("table { border-collapse: collapse; width: 100%; border: solid 1px; }");
                writer.WriteLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
                writer.WriteLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
                writer.WriteLine(".green { background-color: lightgreen; color: black; }");
                writer.WriteLine(".zero { background-color: indianred; color: white; }");
                writer.WriteLine(".negative { background-color: red; color: white; }");
                writer.WriteLine(".header-table td { font-size: 2em; font-weight: bold; }");
                writer.WriteLine("</style>");
                writer.WriteLine("<script>");
                writer.WriteLine("function filterTable() {");
                writer.WriteLine("  var input, filter, table, tr, td, i, j, txtValue;");
                writer.WriteLine("  input = document.getElementById('searchInput');");
                writer.WriteLine("  filter = input.value.toLowerCase();");
                writer.WriteLine("  table = document.getElementById('stockTable');");
                writer.WriteLine("  tr = table.getElementsByTagName('tr');");
                writer.WriteLine("  for (i = 1; i < tr.length; i++) {");
                writer.WriteLine("    tr[i].style.display = 'none';");
                writer.WriteLine("    td = tr[i].getElementsByTagName('td');");
                writer.WriteLine("    for (j = 0; j < td.length; j++) {");
                writer.WriteLine("      if (td[j]) {");
                writer.WriteLine("        txtValue = td[j].textContent || td[j].innerText;");
                writer.WriteLine("        if (txtValue.toLowerCase().indexOf(filter) > -1) {");
                writer.WriteLine("          tr[i].style.display = '';");
                writer.WriteLine("          break;");
                writer.WriteLine("        }");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("function clearSearch() {");
                writer.WriteLine("  document.getElementById('searchInput').value = '';");
                writer.WriteLine("  filterTable();");
                writer.WriteLine("}");
                writer.WriteLine("function sortTable(n, isNumeric) {");
                writer.WriteLine("  var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;");
                writer.WriteLine("  table = document.getElementById('stockTable');");
                writer.WriteLine("  switching = true;");
                writer.WriteLine("  dir = 'asc';");
                writer.WriteLine("  while (switching) {");
                writer.WriteLine("    switching = false;");
                writer.WriteLine("    rows = table.rows;");
                writer.WriteLine("    for (i = 1; i < (rows.length - 1); i++) {");
                writer.WriteLine("      shouldSwitch = false;");
                writer.WriteLine("      x = rows[i].getElementsByTagName('TD')[n];");
                writer.WriteLine("      y = rows[i + 1].getElementsByTagName('TD')[n];");
                writer.WriteLine("      if (isNumeric) {");
                writer.WriteLine("        if (dir == 'asc') {");
                writer.WriteLine("          if (parseFloat(x.innerHTML) > parseFloat(y.innerHTML)) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        } else if (dir == 'desc') {");
                writer.WriteLine("          if (parseFloat(x.innerHTML) < parseFloat(y.innerHTML)) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        }");
                writer.WriteLine("      } else {");
                writer.WriteLine("        if (dir == 'asc') {");
                writer.WriteLine("          if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        } else if (dir == 'desc') {");
                writer.WriteLine("          if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        }");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("    if (shouldSwitch) {");
                writer.WriteLine("      rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);");
                writer.WriteLine("      switching = true;");
                writer.WriteLine("      switchcount ++;");
                writer.WriteLine("    } else {");
                writer.WriteLine("      if (switchcount == 0 && dir == 'asc') {");
                writer.WriteLine("        dir = 'desc';");
                writer.WriteLine("        switching = true;");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine($"<h1>{reportTitle}</h1>");
                writer.WriteLine($"<div>Displaying {partMnfOnes.Count} rows</div>");
                writer.WriteLine("<input type='text' id='searchInput' onkeyup='filterTable()' placeholder='Search for keywords..' style='margin-bottom: 10px;'>");
                writer.WriteLine("<button onclick='clearSearch()'>Clear</button>");
                writer.WriteLine("<table id='stockTable'>");
                writer.WriteLine("<tr>");
                writer.WriteLine("<th onclick='sortTable(0, false)'>IPN</th>");
                writer.WriteLine("<th onclick='sortTable(1, false)'>MFPN</th>");
                writer.WriteLine("<th onclick='sortTable(2, false)'>Description</th>");
                writer.WriteLine("<th onclick='sortTable(4, false)'>Manufacturer</th>");
                writer.WriteLine("</tr>");
                // Add table rows
                foreach (var partMnfOne in partMnfOnes)
                {
                    writer.WriteLine("<tr>");
                    writer.WriteLine($"<td>{partMnfOne.PARTNAME}</td>");
                    writer.WriteLine($"<td>{partMnfOne.MNFPARTNAME}</td>");
                    writer.WriteLine($"<td>{partMnfOne.PARTDES}</td>");
                    writer.WriteLine($"<td>{partMnfOne.MNFDES}</td>");
                    writer.WriteLine("</tr>");
                }
                writer.WriteLine("</table>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
        }
        public class PartMnfOneApiResponse
        {
            public List<PartMnfOne> value { get; set; }
        }
        public class PartMnfOne
        {
            public string PARTNAME { get; set; }
            public string MNFPARTNAME { get; set; }
            public string PARTDES { get; set; }
            public string MNFDES { get; set; }
        }
        private void txtbPrefix_TextChanged(object sender, EventArgs e)
        {
            string selectedPrefix = txtbPrefix.Text.Trim();
            var prefixExceptions = new List<string> { "Flr", "666", "400", "450", "500", "501", "550", "600", "650", "Main", "Outl", "Trn" ,"MRB"};
            // Check if the selected prefix is valid
            if (prefixExceptions.Contains(selectedPrefix))
            {
                btnAVL.Text = "AVL";
            }
            else {
                // Change the button text to include the prefix
                btnAVL.Text = $"{selectedPrefix} AVL";
            }
        }
    }
}
