using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Office.Interop.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static WH_Panel.FrmPriorityAPI;
using static WH_Panel.FrmPriorityBom;
using Microsoft.Extensions.Configuration;

namespace WH_Panel
{

    public partial class FrmPriorityBom : Form
    {
        private AppSettings settings;

        private List<WarehouseBalance> warehouseBalances;
        public FrmPriorityBom()
        {
            InitializeComponent();

            this.Load += FrmPriorityBom_Load;

            this.KeyPreview = true; // Set KeyPreview to true
            
            SetDarkModeColors(this);
            // Set the DrawMode property and handle the DrawItem event
            cmbROBxList.DrawMode = DrawMode.OwnerDrawFixed;
            cmbROBxList.DrawItem += cmbROBxList_DrawItem;
            cmbROBxList.SelectedIndexChanged += cmbROBxList_SelectedIndexChanged;
            // Initialize DataGridView columns
            InitializeDataGridView();
            // Handle the CellFormatting event
            dgwBom.CellFormatting += dgwBom_CellFormatting;
            AttachTextBoxEvents(this);



        }

        private void FrmPriorityBom_Load(object sender, EventArgs e)
        {
            settings = SettingsManager.LoadSettings();

            if (settings == null)
            {
                MessageBox.Show("Failed to load settings.");
                return;
            }
            GetGetRobWosList();
            // Debug output to verify settings are loaded correctly

            // MessageBox.Show("pass:"+settings.ApiPassword);
            // MessageBox.Show($"ApiU: {settings.ApiUsername} -  ApiP: {settings.ApiPassword}");


        }
        public class Serial
        {
            public string PARTNAME { get; set; }
            public string SERIALNAME { get; set; }
            public int QUANT { get; set; }
            public string SERIALSTATUSDES { get; set; }

            public string REVNUM { get; set; }
            public override string ToString()
            {
                return $"{SERIALNAME} - {PARTNAME} - REV({REVNUM}) - {QUANT}PCS - {SERIALSTATUSDES}";
            }
        }
        public class TransOrderKSubform
        {
            public string PARTNAME { get; set; }
            public string PARTDES { get; set; }
            public int WH { get; set; }
            public int PQUANT { get; set; }
            public int CQUANT { get; set; }
            public int DELTA => PQUANT - CQUANT;
            public string CALC { get; set; }
        }


        public class ApiResponse
        {
            public List<Part> value { get; set; }
        }

        public class Part
        {
            public string MNFPARTNAME { get; set; }
        }

        public class WarehouseApiResponse
        {
            public List<Warehouse> value { get; set; }
        }

        public class Warehouse
        {
            public string WARHSNAME { get; set; }
            public List<WarehouseBalance> WARHSBAL_SUBFORM { get; set; }
        }

        public class WarehouseBalance
        {
            public string PARTNAME { get; set; }
            public string LOCNAME { get; set; }
            public string PARTDES { get; set; }
            public int BALANCE { get; set; }
            public int TBALANCE { get; set; }
            public string CDATE { get; set; }
            public int PART { get; set; }
            public string MNFPARTNAME { get; set; }
        }

        //private string username = "api"; // Replace with your actual username
       // private string password = "DdD@12345"; // Replace with your actual password
        private async void GetGetRobWosList()
        {
            string url = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    string un = settings.ApiUsername;

                    //MessageBox.Show(un);
                    string pw = settings.ApiPassword;

                    //MessageBox.Show(pw);
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{un}:{pw}"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var serials = apiResponse["value"].ToObject<List<Serial>>();
                    // Populate the dropdown with the data
                    cmbROBxList.Items.Clear();
                    foreach (var serial in serials)
                    {
                        //cmbROBxList.Items.Add($"{serial.SERIALNAME} - {serial.PARTNAME} - {serial.QUANT}PCS - {serial.SERIALSTATUSDES}");
                        cmbROBxList.Items.Add(serial);
                    }

                    lblLoading.BackColor = Color.Green;
                    lblLoading.Text = "Data Loaded";
                    cmbROBxList.DroppedDown = true;
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
        private void cmbROBxList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var comboBox = sender as System.Windows.Forms.ComboBox;
            var serial = comboBox.Items[e.Index] as Serial;

            // Set the background color based on the SERIALSTATUSDES
            if (serial.SERIALSTATUSDES == "קיט מלא")
            {
                e.Graphics.FillRectangle(Brushes.Green, e.Bounds);
            }
            else if (serial.SERIALSTATUSDES == "הוקפאה")
            {
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(comboBox.BackColor), e.Bounds);
            }

            // Draw the text
            e.Graphics.DrawString(serial.ToString(), e.Font, Brushes.White, e.Bounds);

            // Draw the focus rectangle if the item has focus
            e.DrawFocusRectangle();
        }

        private void SetDarkModeColors(Control parentControl)
        {
            Color backgroundColor = Color.FromArgb(37, 37, 38); // Dark background color
            Color foregroundColor = Color.FromArgb(241, 241, 241); // Light foreground color
            Color borderColor = Color.FromArgb(45, 45, 48); // Border color for controls

            foreach (Control control in parentControl.Controls)
            {
                // Set the background and foreground colors
                control.BackColor = backgroundColor;
                control.ForeColor = foregroundColor;

                // Handle specific control types separately
                if (control is System.Windows.Forms.Button button)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = borderColor;
                    button.ForeColor = foregroundColor;
                }
                else if (control is System.Windows.Forms.GroupBox groupBox)
                {
                    groupBox.ForeColor = foregroundColor;
                }
                else if (control is System.Windows.Forms.TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = backgroundColor;
                    textBox.ForeColor = foregroundColor;
                }
                else if (control is System.Windows.Forms.Label label)
                {
                    if (label.Name != "lblLoading")
                    {
                        label.BackColor = backgroundColor;
                        label.ForeColor = foregroundColor;
                    }

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
                else if (control is System.Windows.Forms.ComboBox comboBox)
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

        private async void cmbROBxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbROBxList.SelectedItem is Serial selectedSerial)
            {
                gbxLoadedWo.Text = selectedSerial.SERIALNAME + " status";
                txtbName.Text = selectedSerial.PARTNAME;
                txtbRob.Text = selectedSerial.SERIALNAME;
                txtbRev.Text = $"REV ( {selectedSerial.REVNUM} )";
                txtbQty.Text = selectedSerial.QUANT.ToString();
                txtbStatus.Text = selectedSerial.SERIALSTATUSDES;

                // Load BOM details
                await LoadBomDetails(selectedSerial.SERIALNAME);
            }
        }


        private void InitializeDataGridView()
        {
            dgwBom.Columns.Clear();
            dgwBom.Columns.Add("PARTNAME", "IPN");
            dgwBom.Columns.Add("MFPN", "MFPN");
            dgwBom.Columns.Add("PARTDES", "Description");
            dgwBom.Columns.Add("WH", "WH"); // Add WH column before KIT
            dgwBom.Columns.Add("PQUANT", "KIT");
            dgwBom.Columns.Add("CQUANT", "Required");
            dgwBom.Columns.Add("DELTA", "Delta");
            dgwBom.Columns.Add("CALC", "CALC");
            dgwBom.Columns.Add("ALT", "ALT"); // Add ALT column after CALC
            dgwBom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private async Task LoadBomDetails(string serialName)
        {
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{serialName}'&$expand=TRANSORDER_K_SUBFORM";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                   // string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var bomDetails = apiResponse["value"].First["TRANSORDER_K_SUBFORM"].ToObject<List<TransOrderKSubform>>();

                    // Aggregate the PQUANT values for each unique PARTNAME
                    var aggregatedDetails = bomDetails
                        .GroupBy(detail => detail.PARTNAME)
                        .Select(group => new TransOrderKSubform
                        {
                            PARTNAME = group.Key,
                            PARTDES = group.First().PARTDES,
                            PQUANT = group.Sum(detail => detail.PQUANT),
                            CQUANT = group.First().CQUANT,
                            CALC = group.Count() > 1 ? string.Join(" + ", group.Select(detail => detail.PQUANT)) : null
                        })
                        .ToList();


                    // Populate the DataGridView with the aggregated data
                    dgwBom.Rows.Clear();
                    foreach (var detail in aggregatedDetails)
                    {
                        if (!detail.CALC.Contains("+ 0"))
                        {
                            dgwBom.Rows.Add(detail.PARTNAME, "", detail.PARTDES, detail.WH, detail.PQUANT, detail.CQUANT, detail.DELTA, detail.CALC);
                        }
                        else
                        {
                            dgwBom.Rows.Add(detail.PARTNAME, "", detail.PARTDES, detail.WH, detail.PQUANT, detail.CQUANT, detail.DELTA);
                        }
                    }


                    // Fetch MFPNs for each row with a delay
                    await FetchMFPNsWithDelay();
                    await FetchBalancesWithDelay();
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
        private async Task FetchBalancesWithDelay()
        {
            string warehouseUrl = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$expand=WARHSBAL_SUBFORM";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    // string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request for warehouse balances
                    HttpResponseMessage response = await client.GetAsync(warehouseUrl);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<WarehouseApiResponse>(responseBody);
                    // Store the warehouse balances in a local list
                    warehouseBalances = apiResponse.value.SelectMany(w => w.WARHSBAL_SUBFORM).ToList();
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

            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                if (row.Cells["PARTNAME"].Value != null)
                {
                    string partName = row.Cells["PARTNAME"].Value.ToString();
                    string warehousePrefix = partName.Split('_')[0];
                    var balance = warehouseBalances.FirstOrDefault(b => b.PARTNAME == partName && b.LOCNAME.StartsWith(warehousePrefix));
                    if (balance != null)
                    {
                        // Update the WH column with the BALANCE value
                        row.Cells["WH"].Value = balance.BALANCE;
                    }

                    // Introduce an artificial delay between each row update
                    await Task.Delay(100); // 100 milliseconds delay
                }
            }

            dgwBom.Refresh();
        }

        private async Task FetchMFPNsWithDelay()
        {
            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                if (row.Cells["PARTNAME"].Value != null)
                {
                    string partName = row.Cells["PARTNAME"].Value.ToString();
                    string partUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PARTNAME eq '{partName}'";

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            // Set the request headers if needed
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            // Set the Authorization header
                            //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
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
                                row.Cells["MFPN"].Value = part.MNFPARTNAME;
                                dgwBom.Refresh();
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            // MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            // MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // Introduce an artificial delay between each API call
                    await Task.Delay(100); // 1 second delay
                }
            }
        }


        private void AttachTextBoxEvents(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                if (control is System.Windows.Forms.TextBox textBox)
                {
                    textBox.Enter += TextBox_Enter;
                    textBox.Leave += TextBox_Leave;
                    textBox.KeyUp += TextBox_KeyUP;
                    textBox.KeyDown += TextBox_KeyDown;

                    // Set the Tag property based on the placeholder text
                    if (textBox.PlaceholderText.Contains("Filter by"))
                    {
                        string displayName = textBox.PlaceholderText.Replace("Filter by ", "").Trim();
                        string columnName = GetColumnNameFromDisplayName(displayName);
                        textBox.Tag = columnName;
                    }
                }
                // Recursively attach events to controls within containers
                if (control.Controls.Count > 0)
                {
                    AttachTextBoxEvents(control);
                }
            }
        }

        // Helper method to map display names to actual column names
        private string GetColumnNameFromDisplayName(string displayName)
        {
            switch (displayName)
            {
                case "IPN":
                    return "PARTNAME";
                case "MFPN":
                    return "MFPN";
                case "Description":
                    return "PARTDES";
                case "ALT":
                    return "ALT";
                default:
                    return displayName;
            }
        }


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //
        }




        private void TextBox_KeyUP(object sender, KeyEventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
            {
                string columnName = textBox.Tag?.ToString();

                if (string.IsNullOrEmpty(columnName))
                {
                    // MessageBox.Show("Column name is null or empty", "Debug");
                    return;
                }

                if (!dgwBom.Columns.Contains(columnName))
                {
                    // MessageBox.Show($"Column named {columnName} cannot be found", "Debug");
                    return;
                }

                if (e.KeyCode == Keys.Escape)
                {
                    ClearFilter(textBox, columnName);
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    //
                }
                else if (e.KeyCode != Keys.Tab && e.KeyCode != Keys.Escape)
                {
                    ApplyFilter(textBox, columnName);
                }
            }
        }

        private void ClearFilter(System.Windows.Forms.TextBox textBox, string columnName)
        {


            textBox.Clear();
            textBox.Text = string.Empty;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();


            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                row.Visible = true;
            }

            dgwBom.Update();
        }

        private void ApplyFilter(System.Windows.Forms.TextBox textBox, string columnName)
        {
            string filterText = textBox.Text.ToLower();

            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                bool isVisible = row.Cells[columnName].Value != null &&
                                 row.Cells[columnName].Value.ToString().ToLower().Contains(filterText);

                row.Visible = isVisible;
            }

            dgwBom.Update();
        }









        private void TextBox_Enter(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
            {
                textBox.BackColor = Color.LightGreen;
                textBox.ForeColor = Color.Black;
            }
        }
        private void TextBox_Leave(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.TextBox textBox)
            {
                textBox.BackColor = Color.FromArgb(30, 30, 30); // Dark background color
                textBox.ForeColor = Color.FromArgb(220, 220, 220); // Light foreground color
            }
        }

        private void dgwBom_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgwBom.Columns[e.ColumnIndex].Name == "DELTA")
            {
                if (e.Value != null && int.TryParse(e.Value.ToString(), out int deltaValue))
                {
                    if (deltaValue >= 0)
                    {
                        e.CellStyle.BackColor = Color.Green;
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.IndianRed;
                    }
                }
            }
        }

        private async void dgwBom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                var selectedRow = dgwBom.Rows[e.RowIndex];
                var partName = selectedRow.Cells["PARTNAME"].Value.ToString();
                string logPartUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/LOGPART?$filter=PARTNAME eq '{partName}'&$expand=PARTTRANSLAST2_SUBFORM";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        // string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        // Measure the time taken for the HTTP GET request
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
                            dgwIPNmoves.AutoGenerateColumns = false;
                            // Clear existing columns
                            dgwIPNmoves.Columns.Clear();
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
                            var tQuantColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "TQUANT",
                                HeaderText = "Quantity",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "TQUANT"
                            };
                            // Add columns to the DataGridView
                            dgwIPNmoves.Columns.AddRange(new DataGridViewColumn[]
                            {
                        curDateColumn,
                        logDocNoColumn,
                        logDOCDESColumn,
                        tQuantColumn
                            });
                            // Populate the DataGridView with the data
                            dgwIPNmoves.Rows.Clear();
                            foreach (var logPart in logPartApiResponse.value)
                            {
                                foreach (var trans in logPart.PARTTRANSLAST2_SUBFORM)
                                {
                                    dgwIPNmoves.Rows.Add(trans.CURDATE, trans.LOGDOCNO, trans.DOCDES, trans.TQUANT);
                                }
                            }
                            gbxIPNstockMovements.Text = $"Stock Movements for {partName}";
                            ColorTheRows(dgwIPNmoves);
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

        private void UpdatePing(long milliseconds)
        {
            // Update the ping label with the elapsed time
            lblPing.Text = $"tProc: {milliseconds} ms";
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

      
    }
}
