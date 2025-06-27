using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;
using Seagull.Framework.Yaml;
using Seagull.Framework.Utility;
using System.Linq;
namespace WH_Panel
{
    public partial class FrmPrioritySearchRob : Form
    {
        private AppSettings settings;
        string baseUrl = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/";
        public FrmPrioritySearchRob()
        {
            InitializeComponent();
            SetDarkModeColors(this);
            InitializeDataGridView();
        }
        private void FrmPrioritySearchRob_Load(object sender, EventArgs e)
        {
            settings = SettingsManager.LoadSettings();
            if (settings == null)
            {
                //MessageBox.Show("Failed to load settings.");
                txtbLog.SelectionColor = Color.Red;
                txtbLog.AppendText("Failed to load settings.");
                txtbLog.ScrollToCaret();
                return;
            }
        }
        private void InitializeDataGridView()
        {
            // Set the AutoSizeColumnsMode property to Fill
            dgwSerials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Add columns if not already added
            if (dgwSerials.Columns.Count == 0)
            {
                var IpnColumn = new DataGridViewTextBoxColumn
                {
                    Name = "IPN",
                    HeaderText = "IPN",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 25f // Set the fill weight to 33.33%
                };
                dgwSerials.Columns.Add(IpnColumn);
                var serialNameColumn = new DataGridViewButtonColumn
                {
                    Name = "SERIALNAME",
                    HeaderText = "ROB work order",
                    UseColumnTextForButtonValue = false, // Set to false to use cell value as button text
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 25f // Set the fill weight to 33.33%
                };
                dgwSerials.Columns.Add(serialNameColumn);
                var parentIpnColumn = new DataGridViewTextBoxColumn
                {
                    Name = "PARENTIPN",
                    HeaderText = "Parent IPN",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 25f // Set the fill weight to 33.33%
                };
                dgwSerials.Columns.Add(parentIpnColumn);
                var serialDesColumn = new DataGridViewTextBoxColumn
                {
                    Name = "SERIALDES",
                    HeaderText = "Description",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 25f // Set the fill weight to 33.33%
                };
                dgwSerials.Columns.Add(serialDesColumn);
            }
        }
        public class SerialInfo
        {
            public string IPN { get; set; }
            public string SERIALNAME { get; set; }
            public string PARENTIPN { get; set; }
            public string SERIALDES { get; set; }
            public string REVNUM { get; set; }
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
                if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = borderColor;
                    button.ForeColor = foregroundColor;
                    button.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    button.FlatAppearance.MouseOverBackColor = Color.FromArgb(51, 153, 255);
                }
                else if (control is GroupBox groupBox)
                {
                    groupBox.ForeColor = foregroundColor;
                    groupBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }
                else if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = backgroundColor;
                    textBox.ForeColor = foregroundColor;
                    textBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }
                else if (control is Label label)
                {
                    if (label.Name != "lblLoading")
                    {
                        label.BackColor = backgroundColor;
                        label.ForeColor = foregroundColor;
                        label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    }
                }
                else if (control is TabControl tabControl)
                {
                    tabControl.BackColor = backgroundColor;
                    tabControl.ForeColor = foregroundColor;
                    tabControl.Font = new Font("Segoe UI", 10, FontStyle.Regular);
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
                    dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
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
                    comboBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.BackColor = backgroundColor;
                    dateTimePicker.ForeColor = foregroundColor;
                    dateTimePicker.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }
                // Recursively update controls within containers
                if (control.Controls.Count > 0)
                {
                    SetDarkModeColors(control);
                }
            }
        }
        private async void txtbSearchIPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtbSearchMFPN.Clear();
                string partName = txtbSearchIPN.Text.Trim();
                if (string.IsNullOrEmpty(partName))
                {
                    MessageBox.Show("Please enter a valid IPN.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                dgwSerials.Rows.Clear();
                //string partUrl = $"{baseUrl}PART?$filter=PARTNAME eq '{partName}'&$expand=PARTPARENT_SUBFORM";
                string partUrl = $"{baseUrl}PART?$filter=PARTNAME eq '{partName}'&$select=PARTNAME&$expand=PARTPARENT_SUBFORM($select=PARENTNAME)";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        // Make the HTTP GET request for part details
                        HttpResponseMessage partResponse = await client.GetAsync(partUrl);
                        partResponse.EnsureSuccessStatusCode();
                        string partResponseBody = await partResponse.Content.ReadAsStringAsync();
                        var partApiResponse = JsonConvert.DeserializeObject<JObject>(partResponseBody);
                        if (partApiResponse == null)
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("partApiResponse is null\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }
                        if (partApiResponse["value"] == null)
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("partApiResponse[\"value\"] is null\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }
                        if (!partApiResponse["value"].Any())
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("partApiResponse[\"value\"] is empty\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }
                        var part = partApiResponse["value"].FirstOrDefault();
                        if (part == null)
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("part is null\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }
                        txtbLog.AppendText($"IPN: {partName}\n");
                        txtbLog.ScrollToCaret();
                        var partParentSubform = part["PARTPARENT_SUBFORM"];
                        if (partParentSubform == null || !partParentSubform.Any())
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("partParentSubform is null or empty\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }
                        foreach (var parent in partParentSubform)
                        {
                            string parentName = parent["PARENTNAME"]?.ToString();
                            if (parentName == null)
                            {
                                txtbLog.SelectionColor = Color.Red;
                                txtbLog.AppendText("parentName is null\n");
                                txtbLog.ScrollToCaret();
                                continue;
                            }
                            txtbLog.AppendText($"Parent IPN: {parentName}\n");
                            txtbLog.ScrollToCaret();
                            await FetchAndDisplaySerials(parentName, partName);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        txtbLog.SelectionColor = Color.Red;
                        txtbLog.AppendText($"HttpRequestException txtbSearchIPN_KeyDown error: {ex.Message}\n");
                        txtbLog.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        txtbLog.SelectionColor = Color.Red;
                        txtbLog.AppendText($"Exception txtbSearchIPN_KeyDown error: {ex.Message}\n");
                        txtbLog.AppendText($"Stack Trace: {ex.StackTrace}\n");
                        txtbLog.ScrollToCaret();
                    }
                }
            }
        }
        private async Task FetchAndDisplaySerials(string parentName,string originIPN)
        {
            //string serialUrl = $"{baseUrl}SERIAL?$filter=PARTNAME eq '{parentName}'";
            string serialUrl = $"{baseUrl}SERIAL?$filter=PARTNAME eq '{parentName}'&$select=SERIALNAME,SERIALDES,SERIALSTATUSDES,REVNUM";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Log the start of the method
                    txtbLog.SelectionColor = Color.Yellow;
                    txtbLog.AppendText($"Fetching serials for parent: {parentName}\n");
                    txtbLog.ScrollToCaret();
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request for serial details
                    HttpResponseMessage serialResponse = await client.GetAsync(serialUrl);
                    // Check for 429 Too Many Requests
                    if (serialResponse.StatusCode == (System.Net.HttpStatusCode)429)
                    {
                        txtbLog.SelectionColor = Color.Red;
                        txtbLog.AppendText("Error: Too Many Requests (429). The server is rate-limiting requests.\n");
                        // Check if the Retry-After header is present
                        if (serialResponse.Headers.TryGetValues("Retry-After", out var retryAfterValues))
                        {
                            string retryAfter = retryAfterValues.FirstOrDefault();
                            txtbLog.AppendText($"Retry-After: {retryAfter} seconds\n");
                        }
                        else
                        {
                            txtbLog.AppendText("Retry-After header not provided by the server.\n");
                        }
                        txtbLog.ScrollToCaret();
                        return;
                    }
                    // Ensure the response is successful
                    serialResponse.EnsureSuccessStatusCode();
                    // Read and process the response
                    string serialResponseBody = await serialResponse.Content.ReadAsStringAsync();
                    var serialApiResponse = JsonConvert.DeserializeObject<JObject>(serialResponseBody);
                    // Log the response
                    txtbLog.SelectionColor = Color.Yellow;
                    txtbLog.AppendText($"Received response for parent: {parentName}\n");
                    txtbLog.ScrollToCaret();
                    if (serialApiResponse["value"] != null && serialApiResponse["value"].Any())
                    {
                        var serials = serialApiResponse["value"]
                            .Where(s => s["SERIALSTATUSDES"]?.ToString() != "נסגרה" && s["SERIALSTATUSDES"]?.ToString() != "קיט מלא" && s["SERIALSTATUSDES"] != null && s["REVNUM"] != null)
                            .Select(s => new SerialInfo
                            {
                                IPN = originIPN,
                                SERIALNAME = s["SERIALNAME"]?.ToString(),
                                PARENTIPN = parentName,
                                SERIALDES = s["SERIALDES"]?.ToString(),
                                REVNUM = s["REVNUM"]?.ToString()
                            })
                            .ToList();
                        if (serials.Count == 0)
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("No open Work Orders found!\n");
                            txtbLog.ScrollToCaret();
                        }
                        else
                        {
                            txtbLog.SelectionColor = Color.Green;
                            txtbLog.AppendText($"Found {serials.Count} open Work Orders for parent IPN: {parentName}\n");
                            txtbLog.ScrollToCaret();
                            DisplaySerials(serials);
                        }
                    }
                    else
                    {
                        txtbLog.SelectionColor = Color.Red;
                        txtbLog.AppendText($"No Work Orders found for the IPN: {parentName}.\n");
                        txtbLog.ScrollToCaret();
                    }
                }
                catch (HttpRequestException ex)
                {
                    txtbLog.SelectionColor = Color.Red;
                    txtbLog.AppendText($"HttpRequestException FetchAndDisplaySerials error: {ex.Message}\n");
                    txtbLog.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    txtbLog.SelectionColor = Color.Red;
                    txtbLog.AppendText($"Exception FetchAndDisplaySerials error: {ex.Message}\n");
                    txtbLog.AppendText($"Stack Trace: {ex.StackTrace}\n");
                    txtbLog.ScrollToCaret();
                }
            }
        }
        private void DisplaySerials(List<SerialInfo> serials)
        {
            try
            {
                // Add rows
                foreach (var serial in serials)
                {
                    int rowIndex = dgwSerials.Rows.Add(serial.IPN,serial.SERIALNAME, serial.PARENTIPN, serial.SERIALDES);
                    dgwSerials.Rows[rowIndex].Cells["SERIALNAME"].Value = $"{serial.SERIALNAME} ({serial.REVNUM})"; // Set the button text to SERIALNAME with REVNUM
                }
            }
            catch (Exception ex)
            {
                txtbLog.SelectionColor = Color.Red;
                txtbLog.AppendText($"Error in DisplaySerials: {ex.Message}\n");
                txtbLog.ScrollToCaret();
            }
        }
        private void OpenFrmPriorityBom(string serialName)
        {
            try
            {
                FrmPriorityBom frmPriorityBom = new FrmPriorityBom
                {
                    SelectedSerialName = serialName
                };
                frmPriorityBom.Show();
                frmPriorityBom.SelectComboBoxItem(serialName);
            }
            catch (Exception ex)
            {
                txtbLog.SelectionColor = Color.Red;
                txtbLog.AppendText($"Error in OpenFrmPriorityBom: {ex.Message}\n");
                txtbLog.ScrollToCaret();
            }
        }
        private void SelectComboBoxItem(ComboBox comboBox, string serialName)
        {
            foreach (var item in comboBox.Items)
            {
                if (item is FrmPriorityBom.Serial serial && serial.SERIALNAME == serialName)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }
        private void dgwSerials_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgwSerials.Columns["SERIALNAME"].Index)
                {
                    string serialName = dgwSerials.Rows[e.RowIndex].Cells["SERIALNAME"].Value.ToString();
                    OpenFrmPriorityBom(serialName);
                }
            }
            catch (Exception ex)
            {
                txtbLog.SelectionColor = Color.Red;
                txtbLog.AppendText($"Error in DgwSerials_CellClick: {ex.Message}\n");
                txtbLog.ScrollToCaret();
            }
        }
        private void txtbSearchIPN_DoubleClick(object sender, EventArgs e)
        {
            txtbSearchIPN.Clear();
        }
        private void txtbSearchIPN_Enter(object sender, EventArgs e)
        {
            OnEnterColorGreen(sender, e);
        }
        private void txtbSearchIPN_Leave(object sender, EventArgs e)
        {
            //txtbSearchIPN.BackColor = Color.Gray;
            OnLeavingBlack(sender as TextBox, e);
        }
        private void txtbSearchIMFPN_Enter(object sender, EventArgs e)
        {
            OnEnterColorGreen(sender as TextBox, e);
        }
        private void txtbSearchIMFPN_Leave(object sender, EventArgs e)
        {
            OnLeavingBlack(sender as TextBox, e);
        }
        private void OnEnterColorGreen(object sender, EventArgs e)
        {
            TextBox txtb = (TextBox)sender;
            txtb.BackColor = Color.LightGreen;
            txtb.ForeColor = Color.Black;
        }
        private void OnLeavingBlack(object sender, EventArgs e)
        {
            TextBox txtb = (TextBox)sender;
            txtb.BackColor = Color.Gray;
        }
        private async void txtbSearchMFPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtbSearchIPN.Clear();
                dgwSerials.Rows.Clear();
                string mfpn = txtbSearchMFPN.Text.Trim();
                if (string.IsNullOrEmpty(mfpn))
                {
                    MessageBox.Show("Please enter a valid MFPN.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Retrieve IPNs based on the MFPN
                var ipns = await RetrieveIPNs(mfpn);
                if (ipns.Count == 0)
                {
                    txtbLog.SelectionColor = Color.Red;
                    txtbLog.AppendText("No IPNs found for the provided MFPN.\n");
                    txtbLog.ScrollToCaret();
                    return;
                }
                foreach (string ipn in ipns)
                {
                    txtbLog.SelectionColor = Color.Yellow;
                    txtbLog.AppendText($"Fetching parent names for IPN: {ipn}\n");
                    txtbLog.ScrollToCaret();
                    // Retrieve all parent names for the IPN
                    var parentNames = await RetrieveParentNames(ipn);
                    if (parentNames.Count == 0)
                    {
                        // Removed duplicate logging here
                        continue;
                    }
                    foreach (string parentName in parentNames)
                    {
                        txtbLog.SelectionColor = Color.Yellow;
                        txtbLog.AppendText($"Fetching details for parent: {parentName}\n");
                        txtbLog.ScrollToCaret();
                        // Fetch and display serials for each parent IPN
                        await FetchAndDisplaySerials(parentName, ipn);
                        // Add a delay to avoid overloading the API
                        await Task.Delay(100); // 500 milliseconds delay
                    }
                }
            }
        }
        private async Task<string> RetrieveParentName(string partName)
        {
            string partUrl = $"{baseUrl}PART?$filter=PARTNAME eq '{partName}'&$expand=PARTPARENT_SUBFORM";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request for part details
                    HttpResponseMessage partResponse = await client.GetAsync(partUrl);
                    partResponse.EnsureSuccessStatusCode();
                    string partResponseBody = await partResponse.Content.ReadAsStringAsync();
                    var partApiResponse = JsonConvert.DeserializeObject<JObject>(partResponseBody);
                    if (partApiResponse == null || partApiResponse["value"] == null || !partApiResponse["value"].Any())
                    {
                        txtbLog.SelectionColor = Color.Red;
                        txtbLog.AppendText($"No parent name found for IPN: {partName}\n");
                        txtbLog.ScrollToCaret();
                        return null;
                    }
                    var part = partApiResponse["value"].FirstOrDefault();
                    var partParentSubform = part["PARTPARENT_SUBFORM"]?.FirstOrDefault();
                    string parentName = partParentSubform?["PARENTNAME"]?.ToString();
                    if (string.IsNullOrEmpty(parentName))
                    {
                        txtbLog.SelectionColor = Color.Red;
                        txtbLog.AppendText($"No parent name found for IPN: {partName}\n");
                        txtbLog.ScrollToCaret();
                        return null;
                    }
                    txtbLog.SelectionColor = Color.Green;
                    txtbLog.AppendText($"Parent for IPN {partName}: {parentName}\n");
                    txtbLog.ScrollToCaret();
                    return parentName;
                }
                catch (HttpRequestException ex)
                {
                    txtbLog.SelectionColor = Color.Red;
                    txtbLog.AppendText($"HttpRequestException RetrieveParentName error: {ex.Message}\n");
                    txtbLog.ScrollToCaret();
                    return null;
                }
                catch (Exception ex)
                {
                    txtbLog.SelectionColor = Color.Red;
                    txtbLog.AppendText($"Exception RetrieveParentName error: {ex.Message}\n");
                    txtbLog.AppendText($"Stack Trace: {ex.StackTrace}\n");
                    txtbLog.ScrollToCaret();
                    return null;
                }
            }
        }
        private async Task<List<string>> RetrieveParentNames(string partName)
        {
            //string partUrl = $"{baseUrl}PART?$filter=PARTNAME eq '{partName}'&$expand=PARTPARENT_SUBFORM";
            string partUrl = $"{baseUrl}PART?$filter=PARTNAME eq '{partName}'&$select=PARTNAME&$expand=PARTPARENT_SUBFORM($select=PARENTNAME)";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request for part details
                    HttpResponseMessage partResponse = await client.GetAsync(partUrl);
                    partResponse.EnsureSuccessStatusCode();
                    string partResponseBody = await partResponse.Content.ReadAsStringAsync();
                    var partApiResponse = JsonConvert.DeserializeObject<JObject>(partResponseBody);
                    // Check if the response or required data is null or empty
                    if (partApiResponse?["value"] == null || !partApiResponse["value"].Any())
                    {
                        LogErrorOnce($"No parent names found for IPN: {partName}");
                        return new List<string>();
                    }
                    var part = partApiResponse["value"].FirstOrDefault();
                    var partParentSubform = part?["PARTPARENT_SUBFORM"];
                    if (partParentSubform == null || !partParentSubform.Any())
                    {
                        LogErrorOnce($"No parent names found for IPN: {partName}");
                        return new List<string>();
                    }
                    // Extract all parent names
                    var parentNames = partParentSubform
                        .Select(p => p["PARENTNAME"]?.ToString())
                        .Where(pn => !string.IsNullOrEmpty(pn))
                        .ToList();
                    LogSuccess($"Found parent names for IPN {partName}:", parentNames);
                    return parentNames;
                }
                catch (HttpRequestException ex)
                {
                    LogErrorOnce($"HttpRequestException RetrieveParentNames error: {ex.Message}");
                    return new List<string>();
                }
                catch (Exception ex)
                {
                    LogErrorOnce($"Exception RetrieveParentNames error: {ex.Message}\nStack Trace: {ex.StackTrace}");
                    return new List<string>();
                }
            }
        }
        private readonly HashSet<string> loggedErrors = new HashSet<string>();
        private void LogErrorOnce(string message)
        {
            if (!loggedErrors.Contains(message))
            {
                loggedErrors.Add(message);
                txtbLog.SelectionColor = Color.Red;
                txtbLog.AppendText(message + "\n");
                txtbLog.ScrollToCaret();
            }
        }
        private void LogSuccess(string message, List<string> parentNames)
        {
            txtbLog.SelectionColor = Color.Green;
            txtbLog.AppendText(message + "\n");
            foreach (var parentName in parentNames)
            {
                txtbLog.AppendText($"- {parentName}\n");
            }
            txtbLog.ScrollToCaret();
        }
        private async Task<List<string>> RetrieveIPNs(string mfpn)
        {
            // URL-encode the MFPN to handle special characters
            string encodedMfpn = Uri.EscapeDataString(mfpn);
            //string url = $"{baseUrl}PARTMNFONE?$filter=MNFPARTNAME eq '{encodedMfpn}'";
            string url = $"{baseUrl}PARTMNFONE?$filter=MNFPARTNAME eq '{encodedMfpn}'&$select=PARTNAME";

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
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    // Extract the IPNs from the response
                    var ipns = apiResponse["value"]
                        .Select(p => p["PARTNAME"].ToString())
                        .ToList();
                    // Log the found IPNs
                    if (ipns.Count > 0)
                    {
                        txtbLog.SelectionColor = Color.Green;
                        txtbLog.AppendText($"Found IPNs for MFPN '{mfpn}':\n");
                        foreach (var ipn in ipns)
                        {
                            txtbLog.AppendText($"- {ipn}\n");
                        }
                    }
                    else
                    {
                        txtbLog.SelectionColor = Color.Red;
                        txtbLog.AppendText($"No IPNs found for MFPN '{mfpn}'.\n");
                    }
                    txtbLog.ScrollToCaret();
                    return ipns;
                }
                catch (HttpRequestException ex)
                {
                    txtbLog.SelectionColor = Color.Red;
                    txtbLog.AppendText($"HttpRequestException RetrieveIPNs error: {ex.Message}\n");
                    txtbLog.ScrollToCaret();
                    return new List<string>();
                }
                catch (Exception ex)
                {
                    txtbLog.SelectionColor = Color.Red;
                    txtbLog.AppendText($"Exception RetrieveIPNs error: {ex.Message}\n");
                    txtbLog.AppendText($"Stack Trace: {ex.StackTrace}\n");
                    txtbLog.ScrollToCaret();
                    return new List<string>();
                }
            }
        }
        private void txtbSearchIMFPN_DoubleClick(object sender, EventArgs e)
        {
            txtbSearchIPN.Clear();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtbSearchMFPN.Text = string.Empty;
            txtbSearchIPN.Text = string.Empty;
        }
    }
}
