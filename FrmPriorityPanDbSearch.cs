using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WH_Panel.FrmPriorityAPI;
using static WH_Panel.FrmPriorityBom;
using Action = System.Action;
using ApiResponse = WH_Panel.FrmPriorityAPI.ApiResponse;
using Button = System.Windows.Forms.Button;
using DataTable = System.Data.DataTable;
using GroupBox = System.Windows.Forms.GroupBox;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;
using Timer = System.Threading.Timer;

namespace WH_Panel
{
    public partial class FrmPriorityPanDbSearch : Form
    {
        private DataTable dataTable;
        private DataView dataView;
        private List<Warehouse> loadedWareHouses = new List<Warehouse>();
        public AppSettings settings;
        public string baseUrl = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522";


        private System.Windows.Forms.Timer debounceTimer;
        private readonly Dictionary<string, DataTable> sessionSearchCache = new Dictionary<string, DataTable>(StringComparer.OrdinalIgnoreCase);
        private CancellationTokenSource searchCts;
        private static readonly HttpClient sharedHttpClient = new HttpClient();

        private readonly Dictionary<string, string> balanceCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public FrmPriorityPanDbSearch()
        {
            InitializeComponent();
            SetDarkModeColors(this);
            InitializeDataTable(); // Initialize the DataTable after loading data
            this.Load += FrmPriorityPanDbSearch_Load; // Attach the Load event
            dgwALLDATA.CellFormatting += DataGridView1_CellFormatting; // Attach the CellFormatting event
                                                                       // Attach the TextChanged event for filtering
            txtbWH.TextChanged += FilterData;
            txtbIPN.TextChanged += FilterData;
            txtbMFPN.TextChanged += FilterData;
            txtbDESC.TextChanged += FilterData;
            // Attach the KeyDown event for clearing text on ESC
            txtbWH.KeyDown += TextBox_KeyDown;
            txtbIPN.KeyDown += TextBox_KeyDown;
            txtbMFPN.KeyDown += TextBox_KeyDown;
            txtbDESC.KeyDown += TextBox_KeyDown;
            // Attach the Enter and Leave events for changing background color
            txtbWH.Enter += TextBox_Enter;
            txtbWH.Leave += TextBox_Leave;
            txtbIPN.Enter += TextBox_Enter;
            txtbIPN.Leave += TextBox_Leave;
            txtbMFPN.Enter += TextBox_Enter;
            txtbMFPN.Leave += TextBox_Leave;
            txtbDESC.Enter += TextBox_Enter;
            txtbDESC.Leave += TextBox_Leave;

            // Inside FrmPriorityPanDbSearch() constructor:
            // Keep all your original txtb*.TextChanged += FilterData lines as they are!
            // Just initialize the timer:
            debounceTimer = new System.Windows.Forms.Timer { Interval = 400 };
            debounceTimer.Tick += DebounceTimer_Tick;

        }

        private void FilterData(object sender, EventArgs e)
        {
            // Reset debounce timer on every keystroke
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private async void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            await ExecutePrioritySearchAsync();
        }

        private async Task ExecutePrioritySearchAsync()
        {
            string wh = txtbWH.Text.Trim();
            string ipn = txtbIPN.Text.Trim();
            string mfpn = txtbMFPN.Text.Trim();
            string desc = txtbDESC.Text.Trim();

            // Guard: Require at least 3 characters in at least one input box
            bool hasSearchTerm = wh.Length >= 3 || ipn.Length >= 3 || mfpn.Length >= 3 || desc.Length >= 3;

            if (!hasSearchTerm)
            {
                dataTable.Clear();
                return;
            }

            // Normalized cache key for the session
            string cacheKey = $"{wh}|{ipn}|{mfpn}|{desc}";

            // Check session cache first (0 ms, 0 API transactions)
            if (sessionSearchCache.TryGetValue(cacheKey, out DataTable cachedTable))
            {
                dataTable = cachedTable.Copy();
                dataView = new DataView(dataTable);
                dgwALLDATA.DataSource = dataView;
                AddLogRow($"[Cache Hit] Loaded {dataTable.Rows.Count} rows (0 API calls).", Color.LightGreen);
                return;
            }

            // Cancel previous request if user typed again
            searchCts?.Cancel();
            searchCts = new CancellationTokenSource();

            try
            {
                AddLogRow("Searching Priority server...", Color.Orange);
                DataTable resultTable = await FetchFilteredDataFromPriorityAsync(wh, ipn, mfpn, desc, searchCts.Token);

                // Store result in session cache
                sessionSearchCache[cacheKey] = resultTable.Copy();

                dataTable = resultTable;
                dataView = new DataView(dataTable);
                // Default ascending sort by IPN (PARTNAME)
                dataView.Sort = "PARTNAME ASC";
                dgwALLDATA.DataSource = dataView;

                AddLogRow($"Found {dataTable.Rows.Count} matching records.", Color.Green);
            }
            catch (OperationCanceledException)
            {
                // Suppress cancelled request
            }
            catch (Exception ex)
            {
                AddLogRow($"Search failed: {ex.Message}", Color.Red);
            }
        }

        private async Task<DataTable> FetchFilteredDataFromPriorityAsync(string wh, string ipn, string mfpn, string desc, CancellationToken ct)
        {
            var filterClauses = new List<string>();

            // 1. חיתוך לפי מחסן - קידומת 3 תווים במק"ט
            if (wh.Length >= 1)
            {
                filterClauses.Add($"startswith(PARTNAME, '{EscapeOData(wh)}')");
            }

            // 2. חיפוש לפי מק"ט (IPN)
            if (ipn.Length >= 3)
            {
                filterClauses.Add($"contains(PARTNAME, '{EscapeOData(ipn)}')");
            }

            // 3. חיפוש לפי מק"ט יצרן (MFPN)
            if (mfpn.Length >= 3)
            {
                filterClauses.Add($"contains(MNFPARTNAME, '{EscapeOData(mfpn)}')");
            }

            // 4. חיפוש לפי תיאור פריט (תומך בהפרדת + לחיפוש מילים מרובות)
            if (desc.Length >= 3)
            {
                string[] descTerms = desc.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in descTerms)
                {
                    string clean = term.Trim();
                    if (clean.Length > 0)
                    {
                        filterClauses.Add($"contains(PARTDES, '{EscapeOData(clean)}')");
                    }
                }
            }

            if (filterClauses.Count == 0)
            {
                return dataTable.Clone();
            }

            string filterParam = string.Join(" and ", filterClauses);

            // שליפה ישירה ממסך PARTMNFONE
            string url = $"{baseUrl}/PARTMNFONE?$filter={filterParam}" +
                         $"&$select=PARTNAME,PARTDES,MNFPARTNAME,MNFNAME,PART";

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ApiHelper.AuthenticateClient(request);

                using (var response = await sharedHttpClient.SendAsync(request, ct))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorBody = await response.Content.ReadAsStringAsync();
                        AddLogRow($"Priority Error ({(int)response.StatusCode}): {errorBody}", Color.Red);
                        return dataTable.Clone();
                    }

                    string json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<PartMnfOneResponse>(json);

                    DataTable dt = dataTable.Clone();
                    if (apiResponse?.Value != null)
                    {
                        foreach (var row in apiResponse.Value)
                        {
                            // גזירת קוד המחסן מתוך 3 התווים הראשונים של ה-IPN
                            string derivedWh = row.PARTNAME != null && row.PARTNAME.Length >= 3
                                ? row.PARTNAME.Substring(0, 3)
                                : string.Empty;

                            string balanceKey = $"{derivedWh}|{row.PARTNAME}";
                            string displayBalance = balanceCache.TryGetValue(balanceKey, out string cachedVal)
                                ? cachedVal
                                : "-";

                            dt.Rows.Add(
                                derivedWh,                           // עמודת WH הנגזרת
                                row.PARTNAME,                        // IPN
                                row.MNFPARTNAME ?? string.Empty,     // MFPN
                                row.PARTDES ?? string.Empty,         // Description
                                row.MNFNAME ?? string.Empty,
                                displayBalance,   // Populates cached value or "-" placeholder
                                row.PART                             // Internal ID
                            );
                        }
                    }
                    return dt;
                }
            }
        }

        private string EscapeOData(string input)
        {
            return input.Replace("'", "''");
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).ForeColor = Color.Black;
            ((TextBox)sender).BackColor = Color.LightGreen;
        }
        private void TextBox_Leave(object sender, EventArgs e)
        {
            ((TextBox)sender).ForeColor = Color.White;
            ((TextBox)sender).BackColor = Color.FromArgb(55, 55, 55); // Revert back to the previous color
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ((TextBox)sender).Clear();
                e.SuppressKeyPress = true; // Prevent the beep sound
            }
        }
        private async void FrmPriorityPanDbSearch_Load(object sender, EventArgs e)
        {
            try
            {
                settings = SettingsManager.LoadSettings();
                if (settings == null)
                {
                    AddLogRow("Failed to load settings.", Color.Red);
                    return;
                }
                if (string.IsNullOrEmpty(settings.ApiUsername) || string.IsNullOrEmpty(settings.ApiPassword))
                {
                    AddLogRow("API credentials are missing in the settings.", Color.Red);
                    return;
                }
                else
                {
                    await LoadWarehouseData();
                    //await LoadDataIntoDataTable(); // Load data into the DataTable after initializing
                }
            }
            catch (Exception ex)
            {
                AddLogRow($"An error occurred during initialization: {ex.Message}", Color.Red);
            }
        }
        private void AddLogRow(string errorText, Color textColor)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => AddLogRow(errorText, textColor)));
            }
            else
            {
                txtLog.SelectionStart = txtLog.TextLength;
                txtLog.SelectionLength = 0;
                txtLog.SelectionColor = textColor;
                txtLog.AppendText($"{DateTime.Now}: {errorText}\n");
                txtLog.SelectionColor = txtLog.ForeColor;
                txtLog.ScrollToCaret();
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
        private async Task LoadWarehouseData()
        {
            AddLogRow("Loading warehouse data...", Color.Orange);
            string url = $"{baseUrl}/WAREHOUSES?$select=WARHSNAME,WARHSDES,WARHS";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    string usedUser = ApiHelper.AuthenticateClient(client);
                    // string usedUser = ApiHelper.AuthenticateClient(client);

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    WarehouseApiResponse apiResponse = JsonConvert.DeserializeObject<WarehouseApiResponse>(responseBody);
                    if (apiResponse.value != null && apiResponse.value.Count > 0)
                    {
                        loadedWareHouses.Clear();
                        var excludedWarehouses = new HashSet<string> { "666", "400", "450", "500", "501", "550", "600", "650", "Flr", "Main", "MRB", "Outl", "Trn" };
                        var filteredWarehouses = apiResponse.value.Where(warehouse => !excludedWarehouses.Contains(warehouse.WARHSNAME)).ToList();
                        loadedWareHouses.AddRange(filteredWarehouses);
                        countOFWHs += filteredWarehouses.Count;
                    }
                    else
                    {
                        AddLogRow("No data found for the warehouses.", Color.Orange);
                    }
                }
                catch (HttpRequestException ex)
                {
                    AddLogRow($"Request error: {ex.Message}", Color.Red);
                }
                catch (Exception ex)
                {
                    AddLogRow($"An error occurred: {ex.Message}", Color.Red);
                }
            }
        }
        int countOFWHs = 0;
        private async Task LoadDataIntoDataTable()
        {
            var stopwatch = Stopwatch.StartNew();
            int currentWH = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = countOFWHs;
            progressBar1.Value = 0;
            int totalRowsLoaded = 0;

            foreach (var warehouse in loadedWareHouses)
            {
                AddLogRow($"Loading data for warehouse: {warehouse.WARHSNAME}", Color.Orange);
                // Step 1: Load balances for the warehouse
                //string balanceUrl = $"{baseUrl}/WAREHOUSES?$filter=WARHSNAME eq '{warehouse.WARHSNAME}'&$expand=WARHSBAL_SUBFORM";
                string balanceUrl = $"{baseUrl}/WAREHOUSES?$filter=WARHSNAME eq '{warehouse.WARHSNAME}'&$expand=WARHSBAL_SUBFORM($select=PARTNAME,PARTDES,BALANCE,CDATE,PART)";

                List<WarehouseBalance> warehouseBalances = new List<WarehouseBalance>();
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                        string usedUser = ApiHelper.AuthenticateClient(client);
                        // string usedUser = ApiHelper.AuthenticateClient(client);


                        HttpResponseMessage response = await client.GetAsync(balanceUrl);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<WarehouseApiResponse>(responseBody);
                        if (apiResponse.value != null && apiResponse.value.Count > 0)
                        {
                            warehouseBalances = apiResponse.value.SelectMany(w => w.WARHSBAL_SUBFORM).ToList();
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        AddLogRow($"LoadDataIntoDataTable: Request error: {ex.Message}\n{ex.StackTrace}", Color.Red);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        AddLogRow($"LoadDataIntoDataTable: An error occurred: {ex.Message}\n{ex.StackTrace}", Color.Red);
                        continue;
                    }
                }
                // Step 2: Load MFPNs for the warehouse
                //string mfpnUrl = $"{baseUrl}/PARTMNFONE?$filter=PARTNAME eq '{warehouse.WARHSNAME}_*'";
                string mfpnUrl = $"{baseUrl}/PARTMNFONE?$filter=PARTNAME eq '{warehouse.WARHSNAME}_*'&$select=PARTNAME,MNFPARTNAME";

                Dictionary<string, string> partToMfpnMap = new Dictionary<string, string>();
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                        string usedUser = ApiHelper.AuthenticateClient(client);
                        // string usedUser = ApiHelper.AuthenticateClient(client);

                        HttpResponseMessage response = await client.GetAsync(mfpnUrl);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<PartMnfOneApiResponse>(responseBody);
                        if (apiResponse.value != null && apiResponse.value.Count > 0)
                        {
                            partToMfpnMap = apiResponse.value
                                .GroupBy(p => p.PARTNAME) // Group by PARTNAME to handle duplicates
                                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.MNFPARTNAME); // Use FirstOrDefault for duplicates
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        AddLogRow($"LoadDataIntoDataTable: Request error: {ex.Message}\n{ex.StackTrace}", Color.Red);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        AddLogRow($"LoadDataIntoDataTable: An error occurred: {ex.Message}\n{ex.StackTrace}", Color.Red);
                        continue;
                    }
                }

                int rowsPerWarehouse = 0;
                // Step 3: Merge balances and MFPNs into the dataTable
                foreach (var balance in warehouseBalances)
                {
                    string mfpn = partToMfpnMap.ContainsKey(balance.PARTNAME) ? partToMfpnMap[balance.PARTNAME] : string.Empty;
                    dataTable.Rows.Add(warehouse.WARHSNAME, balance.PARTNAME, mfpn, balance.PARTDES, balance.BALANCE, balance.CDATE.Substring(0, 10), balance.PART);
                    rowsPerWarehouse++;
                }
                currentWH++;
                progressBar1.Value = currentWH; // Update progress bar
                                                // Calculate and display percentage
                int percentage = (int)((currentWH / (double)countOFWHs) * 100);
                lblProgressPercentage.Text = $"{percentage}%";
                lblProgressPercentage.ForeColor = Color.Red;

                AddLogRow($"Data loaded for warehouse: {warehouse.WARHSNAME} ({currentWH}/{countOFWHs}) {rowsPerWarehouse} Rows Loaded", Color.Green);
                totalRowsLoaded += rowsPerWarehouse;
            }
            stopwatch.Stop();
            dataView = new DataView(dataTable);
            dgwALLDATA.DataSource = dataView;
            lblProgressPercentage.ForeColor = Color.Green;
            //AddLogRow($"Total Rows Loaded : {totalRowsLoaded}", Color.Green);
            AddLogRow($"Total Rows Loaded : {totalRowsLoaded} in {stopwatch.Elapsed.TotalSeconds:F1} seconds", Color.Green);
        }
        private void InitializeDataTable()
        {
            dataTable = new DataTable();
            dataTable.Columns.Add("WH", typeof(string));
            dataTable.Columns.Add("PARTNAME", typeof(string));
            dataTable.Columns.Add("MNFPARTNAME", typeof(string));
            dataTable.Columns.Add("PARTDES", typeof(string));
            dataTable.Columns.Add("MNFNAME", typeof(string)); // Replaces BALANCE
            dataTable.Columns.Add("BALANCE", typeof(string)); // String allows "-" or "Click to load"
            dataTable.Columns.Add("PART", typeof(long));       // Replaces CDATE/PART

            dataView = new DataView(dataTable);
            dgwALLDATA.DataSource = dataView;



            // Auto-widen the IPN, MFPN, and Desc columns
            dgwALLDATA.Columns["PARTNAME"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgwALLDATA.Columns["MNFPARTNAME"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgwALLDATA.Columns["PARTDES"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgwALLDATA.Columns[e.ColumnIndex].Name == "BALANCE")
            {
                if (e.Value != null && int.TryParse(e.Value.ToString(), out int balance))
                {
                    if (balance > 0)
                    {
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else if (balance == 0)
                    {
                        e.CellStyle.BackColor = Color.IndianRed;
                        e.CellStyle.ForeColor = Color.White;
                    }
                    else if (balance < 0)
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.ForeColor = Color.White;
                    }
                }
            }
        }
        //private void FilterData(object sender, EventArgs e)
        //{
        //    StringBuilder filter = new StringBuilder();
        //    if (!string.IsNullOrEmpty(txtbWH.Text))
        //    {
        //        filter.Append($"WH LIKE '%{txtbWH.Text}%'");
        //    }
        //    if (!string.IsNullOrEmpty(txtbIPN.Text))
        //    {
        //        if (filter.Length > 0) filter.Append(" AND ");
        //        filter.Append($"PARTNAME LIKE '%{txtbIPN.Text}%'");
        //    }
        //    if (!string.IsNullOrEmpty(txtbMFPN.Text))
        //    {
        //        if (filter.Length > 0) filter.Append(" AND ");
        //        filter.Append($"MNFPARTNAME LIKE '%{txtbMFPN.Text}%'");
        //    }
        //    if (!string.IsNullOrEmpty(txtbDESC.Text))
        //    {
        //        if (filter.Length > 0) filter.Append(" AND ");
        //        string[] descTerms = txtbDESC.Text.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
        //        foreach (string term in descTerms)
        //        {
        //            filter.Append($"PARTDES LIKE '%{term.Trim()}%' AND ");
        //        }
        //        filter.Length -= 5; // Remove the trailing " AND "
        //    }
        //    dataView.RowFilter = filter.ToString();
        //}
        //private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    // Ignore header clicks
        //    if (e.RowIndex < 0 || e.RowIndex >= dgwALLDATA.Rows.Count)
        //        return;

        //    var currentRow = dgwALLDATA.Rows[e.RowIndex];
        //    if (currentRow.DataBoundItem is not DataRowView rowView)
        //        return;

        //    DataRow row = rowView.Row;
        //    string currentBalance = row["BALANCE"]?.ToString();

        //    // If balance is already loaded (not "-" or empty), skip to avoid duplicate API calls
        //    if (!string.IsNullOrEmpty(currentBalance) && currentBalance != "-" && currentBalance != "...")
        //        return;

        //    string wh = row["WH"]?.ToString();
        //    string ipn = row["PARTNAME"]?.ToString();

        //    if (string.IsNullOrEmpty(wh) || string.IsNullOrEmpty(ipn))
        //        return;

        //    try
        //    {
        //        // Visual indicator that it's fetching
        //        row["BALANCE"] = "...";

        //        decimal balance = await FetchSingleBalanceAsync(wh, ipn);
        //        row["BALANCE"] = balance.ToString("0");
        //    }
        //    catch (Exception ex)
        //    {
        //        row["BALANCE"] = "Err";
        //        AddLogRow($"Balance check failed for {ipn}: {ex.Message}", Color.Red);
        //    }
        //}

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgwALLDATA.Rows.Count)
                return;

            var currentRow = dgwALLDATA.Rows[e.RowIndex];
            if (currentRow.DataBoundItem is not DataRowView rowView)
                return;

            DataRow row = rowView.Row;
            string currentBalance = row["BALANCE"]?.ToString();

            // Already resolved on this row
            if (!string.IsNullOrEmpty(currentBalance) && currentBalance != "-" && currentBalance != "...")
                return;

            string wh = row["WH"]?.ToString();
            string ipn = row["PARTNAME"]?.ToString();

            if (string.IsNullOrEmpty(wh) || string.IsNullOrEmpty(ipn))
                return;

            string cacheKey = $"{wh}|{ipn}";

            // Check balance session cache
            if (balanceCache.TryGetValue(cacheKey, out string cachedBalance))
            {
                row["BALANCE"] = cachedBalance;
                return;
            }

            try
            {
                row["BALANCE"] = "...";

                decimal balance = await FetchSingleBalanceAsync(wh, ipn);
                string formattedBalance = balance.ToString("0"); // Pure raw digits (no spaces, no commas)

                // Store into cache and apply to row
                balanceCache[cacheKey] = formattedBalance;
                row["BALANCE"] = formattedBalance;
            }
            catch (Exception ex)
            {
                row["BALANCE"] = "Err";
                AddLogRow($"Balance check failed for {ipn}: {ex.Message}", Color.Red);
            }
        }


        private async Task<decimal> FetchSingleBalanceAsync(string wh, string ipn)
    {
        // Query parent WAREHOUSES filtered by WH, and expand only the specific IPN's balance
        string url = $"{baseUrl}/WAREHOUSES?$filter=WARHSNAME eq '{EscapeOData(wh)}'" +
                     $"&$select=WARHSNAME" +
                     $"&$expand=WARHSBAL_SUBFORM($filter=PARTNAME eq '{EscapeOData(ipn)}';$select=BALANCE)";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        ApiHelper.AuthenticateClient(request);

        using var response = await sharedHttpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<WarehouseExpandBalanceResponse>(json);

        // Sum across subform rows in case the warehouse has multiple storage bins/locations
        var warehouse = apiResponse?.Value?.FirstOrDefault();
        if (warehouse?.Balances != null && warehouse.Balances.Count > 0)
        {
            return warehouse.Balances.Sum(b => b.NumericBalance);
        }

        return 0m;
    }

    public class WarehouseBalanceResponse
        {
            [JsonProperty("value")]
            public List<WarehouseBalanceEntry> Value { get; set; }
        }

        public class WarehouseBalanceEntry
        {
            public decimal BALANCE { get; set; }
        }


        private async Task PopulateInStockData(string partName, int zeroOrHero)
        {
            if (zeroOrHero > 0)
            {
                // Separate data into ROB and notRob lists
                var robList = new List<DataGridViewRow>();
                var notRobList = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in dgwTRANSACTIONS.Rows)
                {
                    if (row.Cells["LOGDOCNO"].Value != null && row.Cells["UDATE"].Value != null && DateTime.TryParse(row.Cells["UDATE"].Value.ToString(), out _))
                    {
                        string docNo = row.Cells["LOGDOCNO"].Value.ToString();
                        if (docNo.StartsWith("ROB") || docNo.StartsWith("IC") || docNo.StartsWith("WR") || docNo.StartsWith("SH"))
                        {
                            // Handle IC documents by converting the quantity to a positive value
                            if (docNo.StartsWith("IC"))
                            {
                                row.Cells["TQUANT"].Value = Math.Abs(Convert.ToInt32(row.Cells["TQUANT"].Value));
                            }
                            robList.Add(row);
                        }
                        else
                        {
                            notRobList.Add(row);
                        }
                    }
                }
                // Sort both lists by transaction date
                robList = robList.OrderBy(row => DateTime.Parse(row.Cells["UDATE"].Value.ToString())).ToList();
                notRobList = notRobList.OrderBy(row => DateTime.Parse(row.Cells["UDATE"].Value.ToString())).ToList();
                // Filter out matching pairs
                var filteredNotRobList = new List<DataGridViewRow>(notRobList);
                foreach (var notRobRow in notRobList)
                {
                    if (robList.Count == 0) break;
                    int notRobQty = Convert.ToInt32(notRobRow.Cells["TQUANT"].Value);
                    var matchingRobRow = robList.FirstOrDefault(robRow => Convert.ToInt32(robRow.Cells["TQUANT"].Value) == notRobQty);
                    if (matchingRobRow != null)
                    {
                        filteredNotRobList.Remove(notRobRow);
                        robList.Remove(matchingRobRow);
                    }
                }
                // Populate the dgwINSTOCK DataGridView with the remaining items from the notRob list
                dgwINSTOCK.AutoGenerateColumns = false;
                dgwINSTOCK.Columns.Clear();
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
                dgwINSTOCK.Columns.AddRange(new DataGridViewColumn[]
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
                dgwINSTOCK.Rows.Clear();
                foreach (var row in filteredNotRobList)
                {
                    dgwINSTOCK.Rows.Add(
                        row.Cells["UDATE"].Value,
                        row.Cells["LOGDOCNO"].Value,
                        row.Cells["DOCDES"].Value,
                        row.Cells["SUPCUSTNAME"].Value,
                        row.Cells["BOOKNUM"].Value,
                        row.Cells["TQUANT"].Value,
                        row.Cells["PACKNAME"].Value
                    );
                }
                dgwINSTOCK.Sort(dgwINSTOCK.Columns[0], ListSortDirection.Descending);
            }
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
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    string usedUser = ApiHelper.AuthenticateClient(client);
                    // string usedUser = ApiHelper.AuthenticateClient(client);


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
                        dgwALLDATA.Refresh();
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
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                    string usedUser = ApiHelper.AuthenticateClient(client);
                    // string usedUser = ApiHelper.AuthenticateClient(client);


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
                //string url = $"{baseUrl}/SERIAL?$filter=SERIALNAME eq '{docNo}'";
                string url = $"{baseUrl}/SERIAL?$filter=SERIALNAME eq '{docNo}'&$select=UDATE";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        string usedUser = ApiHelper.AuthenticateClient(client);
                        // string usedUser = ApiHelper.AuthenticateClient(client);

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
                //string url = $"{baseUrl}/DOCUMENTS_P?$filter=DOCNO eq '{docNo}'";
                string url = $"{baseUrl}/DOCUMENTS_P?$filter=DOCNO eq '{docNo}'&$select=UDATE";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                        string usedUser = ApiHelper.AuthenticateClient(client);
                        // string usedUser = ApiHelper.AuthenticateClient(client);

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
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                        string usedUser = ApiHelper.AuthenticateClient(client);
                        // string usedUser = ApiHelper.AuthenticateClient(client);

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
        //private async void btnGETMFPN_Click(object sender, EventArgs e)
        //{
        //    await FetchMFPNsForAllRows();
        //}
        private async Task FetchMFPNsForAllRows()
        {
            AddLogRow("Fetching MFPNs for all rows\n", Color.Yellow);
            foreach (DataGridViewRow row in dgwALLDATA.Rows)
            {
                await ExtractMFPNForRow(row);
            }
            AddLogRow("MFPN fetching completed\n", Color.Green);
        }
        private void btnClearAllFilters_Click(object sender, EventArgs e)
        {
            txtbDESC.Clear();
            txtbIPN.Clear();
            txtbMFPN.Clear();
            txtbWH.Clear();
            dataView.RowFilter = string.Empty;
            dgwALLDATA.ClearSelection();
        }


        public class PartMnfOneResponse
        {
            [JsonProperty("value")]
            public List<PartMnfOneItem> Value { get; set; }
        }

        public class PartMnfOneItem
        {
            public string PARTNAME { get; set; }
            public string PARTDES { get; set; }
            public string MNFPARTNAME { get; set; }
            public string MNFNAME { get; set; }
            public long PART { get; set; }
        }

        public class WarehouseExpandBalanceResponse
        {
            [JsonProperty("value")]
            public List<WarehouseParentItem> Value { get; set; }
        }

        public class WarehouseParentItem
        {
            public string WARHSNAME { get; set; }

            [JsonProperty("WARHSBAL_SUBFORM")]
            public List<WarehouseBalanceSubformItem> Balances { get; set; }
        }

        public class WarehouseBalanceSubformItem
        {
            // Receive as object/string to safely handle Priority's localized number formatting
            [JsonProperty("BALANCE")]
            public string RawBalance { get; set; }

            public decimal NumericBalance
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(RawBalance))
                        return 0m;

                    // Strip normal spaces, non-breaking spaces (\u00A0), and commas
                    string clean = RawBalance
                        .Replace(" ", "")
                        .Replace("\u00A0", "")
                        .Replace(",", "");

                    return decimal.TryParse(clean, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val)
                        ? val
                        : 0m;
                }
            }
        }

        //private async void dgwALLDATA_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0) // Ensure the row index is valid
        //    {
        //        dgwINSTOCK.Rows.Clear();
        //        var selectedRow = dgwALLDATA.Rows[e.RowIndex];
        //        await ExtractMFPNForRow(selectedRow);
        //        var partName = selectedRow.Cells["PARTNAME"].Value.ToString();
        //        string logPartUrl = $"{baseUrl}/LOGPART?$filter=PARTNAME eq '{partName}'&$expand=PARTTRANSLAST2_SUBFORM";
        //        using (HttpClient client = new HttpClient())
        //        {
        //            try
        //            {
        //                // Set the request headers if needed
        //                client.DefaultRequestHeaders.Accept.Clear();
        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                // Set the Authorization header
        //                //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
        //                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        //                //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


        //                string usedUser = ApiHelper.AuthenticateClient(client);
        //                // string usedUser = ApiHelper.AuthenticateClient(client);


        //                // Measure the time taken for the HTTP POST request
        //                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        //                // Make the HTTP GET request for stock movements
        //                HttpResponseMessage logPartResponse = await client.GetAsync(logPartUrl);
        //                logPartResponse.EnsureSuccessStatusCode();
        //                stopwatch.Stop();
        //                // Update the ping label
        //                //UpdatePing(stopwatch.ElapsedMilliseconds);
        //                // Read the response content
        //                string logPartResponseBody = await logPartResponse.Content.ReadAsStringAsync();
        //                // Parse the JSON response
        //                var logPartApiResponse = JsonConvert.DeserializeObject<LogPartApiResponse>(logPartResponseBody);
        //                // Check if the response contains any data
        //                if (logPartApiResponse.value != null && logPartApiResponse.value.Count > 0)
        //                {
        //                    // Set AutoGenerateColumns to false
        //                    dgwTRANSACTIONS.AutoGenerateColumns = false;
        //                    // Clear existing columns
        //                    dgwTRANSACTIONS.Columns.Clear();
        //                    // Define the columns you want to display
        //                    var curDateColumn = new DataGridViewTextBoxColumn
        //                    {
        //                        DataPropertyName = "UDATE",
        //                        HeaderText = "Transaction Date",
        //                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
        //                        Name = "UDATE"
        //                    };
        //                    var logDocNoColumn = new DataGridViewTextBoxColumn
        //                    {
        //                        DataPropertyName = "LOGDOCNO",
        //                        HeaderText = "Document Number",
        //                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
        //                        Name = "LOGDOCNO"
        //                    };
        //                    var logDOCDESColumn = new DataGridViewTextBoxColumn
        //                    {
        //                        DataPropertyName = "DOCDES",
        //                        HeaderText = "DOCDES",
        //                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
        //                        Name = "DOCDES"
        //                    };
        //                    var SUPCUSTNAMEColumn = new DataGridViewTextBoxColumn
        //                    {
        //                        DataPropertyName = "SUPCUSTNAME",
        //                        HeaderText = "Source_Requester",
        //                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
        //                        Name = "SUPCUSTNAME"
        //                    };
        //                    var tQuantColumn = new DataGridViewTextBoxColumn
        //                    {
        //                        DataPropertyName = "TQUANT",
        //                        HeaderText = "QTY",
        //                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
        //                        Name = "TQUANT"
        //                    };
        //                    var tPACKNAMEColumn = new DataGridViewTextBoxColumn
        //                    {
        //                        DataPropertyName = "PACKNAME",
        //                        HeaderText = "PACK",
        //                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
        //                        Name = "PACKNAME"
        //                    };
        //                    var DocBOOKNUMColumn = new DataGridViewTextBoxColumn
        //                    {
        //                        DataPropertyName = "BOOKNUM",
        //                        HeaderText = "Client`s Document",
        //                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
        //                        Name = "BOOKNUM"
        //                    };
        //                    // Add columns to the DataGridView
        //                    dgwTRANSACTIONS.Columns.AddRange(new DataGridViewColumn[]
        //                    {
        //                curDateColumn,
        //                logDocNoColumn,
        //                logDOCDESColumn,
        //                SUPCUSTNAMEColumn,
        //                DocBOOKNUMColumn,
        //                tQuantColumn,
        //                tPACKNAMEColumn
        //                    });
        //                    // Populate the DataGridView with the data
        //                    dgwTRANSACTIONS.Rows.Clear();
        //                    foreach (var logPart in logPartApiResponse.value)
        //                    {
        //                        foreach (var trans in logPart.PARTTRANSLAST2_SUBFORM)
        //                        {
        //                            if (trans.DOCDES != "קיזוז אוטומטי")
        //                            {
        //                                dgwTRANSACTIONS.Rows.Add("", trans.LOGDOCNO, trans.DOCDES, trans.SUPCUSTNAME, "", trans.TQUANT, "");
        //                            }

        //                        }
        //                    }
        //                    groupBox6.Text = $"TRANSACTIONS for {partName}";
        //                    //ColorTheRows(dataGridView2);
        //                    foreach (DataGridViewRow row in dgwTRANSACTIONS.Rows)
        //                    {
        //                        var logDocNo = row.Cells["LOGDOCNO"].Value?.ToString();
        //                        var partNameCell = partName;
        //                        var quant = int.Parse(row.Cells["TQUANT"].Value?.ToString());
        //                        if (logDocNo != null && partNameCell != null)
        //                        {
        //                            var results = await FetchPackCodeAsync(logDocNo, partNameCell, quant);
        //                            foreach (var result in results)
        //                            {
        //                                if (result.PackCode != null)
        //                                {
        //                                    row.Cells["PACKNAME"].Value = result.PackCode;
        //                                }
        //                                if (result.BookNum != null)
        //                                {
        //                                    row.Cells["BOOKNUM"].Value = result.BookNum;
        //                                }
        //                                if (result.Date != null)
        //                                {
        //                                    row.Cells["UDATE"].Value = result.Date;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    // Sort the DataGridView by the first column in descending order
        //                    dgwTRANSACTIONS.Sort(dgwTRANSACTIONS.Columns[0], ListSortDirection.Descending);
        //                    // Populate dgwINSTOCK with items that are currently in stock
        //                    var actualStock = int.Parse(selectedRow.Cells["BALANCE"].Value.ToString());
        //                    await PopulateInStockData(partName, actualStock);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("No stock movements found for the selected part.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //            }
        //            catch (HttpRequestException ex)
        //            {
        //                txtLog.SelectionColor = Color.Red; // Set the color to acid green
        //                txtLog.AppendText($"Request error: {ex.Message}");
        //                txtLog.ScrollToCaret();
        //            }
        //            catch (Exception ex)
        //            {
        //                if (txtLog != null && !txtLog.IsDisposed)
        //                {
        //                    txtLog.SelectionColor = Color.Red; // Set the color to acid green
        //                    txtLog.AppendText($"Request error: {ex.Message}");
        //                    txtLog.ScrollToCaret();
        //                }
        //            }
        //        }
        //}
        private async void dgwALLDATA_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header clicks or out-of-range clicks
            if (e.RowIndex < 0 || e.RowIndex >= dgwALLDATA.Rows.Count)
                return;

            var selectedRow = dgwALLDATA.Rows[e.RowIndex];
            if (selectedRow.DataBoundItem is not DataRowView rowView)
                return;

            DataRow dr = rowView.Row;
            string partName = dr["PARTNAME"]?.ToString();
            string wh = dr["WH"]?.ToString();

            if (string.IsNullOrEmpty(partName))
                return;

            try
            {
                dgwINSTOCK.Rows.Clear();

                // 1. Ensure BALANCE is resolved and clean
                string currentBalanceStr = dr["BALANCE"]?.ToString();
                int actualStock = 0;

                if (string.IsNullOrEmpty(currentBalanceStr) || currentBalanceStr == "-" || currentBalanceStr == "..." || currentBalanceStr == "Err")
                {
                    // Auto-fetch balance on double click if not clicked yet
                    dr["BALANCE"] = "...";
                    decimal fetchedBalance = await FetchSingleBalanceAsync(wh, partName);
                    string formattedDigits = fetchedBalance.ToString("0");

                    // Update cache and grid
                    balanceCache[$"{wh}|{partName}"] = formattedDigits;
                    dr["BALANCE"] = formattedDigits;
                    actualStock = (int)fetchedBalance;
                }
                else
                {
                    int.TryParse(currentBalanceStr, out actualStock);
                }

                // 2. Fetch Stock Movements from LOGPART
                AddLogRow($"Fetching transactions for {partName}...", Color.Orange);

                string logPartUrl = $"{baseUrl}/LOGPART?$filter=PARTNAME eq '{EscapeOData(partName)}'&$expand=PARTTRANSLAST2_SUBFORM";

                using var request = new HttpRequestMessage(HttpMethod.Get, logPartUrl);
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ApiHelper.AuthenticateClient(request);

                using var response = await sharedHttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var logPartApiResponse = JsonConvert.DeserializeObject<LogPartApiResponse>(responseBody);

                if (logPartApiResponse?.value != null && logPartApiResponse.value.Count > 0)
                {
                    // Configure dgwTRANSACTIONS grid
                    dgwTRANSACTIONS.AutoGenerateColumns = false;
                    dgwTRANSACTIONS.Columns.Clear();

                    dgwTRANSACTIONS.Columns.AddRange(new DataGridViewColumn[]
                    {
                new DataGridViewTextBoxColumn { DataPropertyName = "UDATE", HeaderText = "Transaction Date", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Name = "UDATE" },
                new DataGridViewTextBoxColumn { DataPropertyName = "LOGDOCNO", HeaderText = "Document Number", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Name = "LOGDOCNO" },
                new DataGridViewTextBoxColumn { DataPropertyName = "DOCDES", HeaderText = "DOCDES", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Name = "DOCDES" },
                new DataGridViewTextBoxColumn { DataPropertyName = "SUPCUSTNAME", HeaderText = "Source_Requester", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Name = "SUPCUSTNAME" },
                new DataGridViewTextBoxColumn { DataPropertyName = "BOOKNUM", HeaderText = "Client's Document", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Name = "BOOKNUM" },
                new DataGridViewTextBoxColumn { DataPropertyName = "TQUANT", HeaderText = "QTY", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Name = "TQUANT" },
                new DataGridViewTextBoxColumn { DataPropertyName = "PACKNAME", HeaderText = "PACK", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Name = "PACKNAME" }
                    });

                    dgwTRANSACTIONS.Rows.Clear();

                    foreach (var logPart in logPartApiResponse.value)
                    {
                        if (logPart.PARTTRANSLAST2_SUBFORM == null) continue;

                        foreach (var trans in logPart.PARTTRANSLAST2_SUBFORM)
                        {
                            if (trans.DOCDES != "קיזוז אוטומטי")
                            {
                                dgwTRANSACTIONS.Rows.Add(
                                    "",
                                    trans.LOGDOCNO,
                                    trans.DOCDES,
                                    trans.SUPCUSTNAME,
                                    "",
                                    trans.TQUANT,
                                    ""
                                );
                            }
                        }
                    }

                    groupBox6.Text = $"TRANSACTIONS for {partName}";

                    // 3. Enrich rows with Pack Code / Document info
                    foreach (DataGridViewRow row in dgwTRANSACTIONS.Rows)
                    {
                        string logDocNo = row.Cells["LOGDOCNO"].Value?.ToString();
                        string quantStr = row.Cells["TQUANT"].Value?.ToString();

                        if (!string.IsNullOrEmpty(logDocNo) && int.TryParse(quantStr, out int quant))
                        {
                            var results = await FetchPackCodeAsync(logDocNo, partName, quant);
                            if (results != null)
                            {
                                foreach (var result in results)
                                {
                                    if (result.PackCode != null) row.Cells["PACKNAME"].Value = result.PackCode;
                                    if (result.BookNum != null) row.Cells["BOOKNUM"].Value = result.BookNum;
                                    if (result.Date != null) row.Cells["UDATE"].Value = result.Date;
                                }
                            }
                        }
                    }

                    // Sort by UDATE descending if there are rows
                    if (dgwTRANSACTIONS.Rows.Count > 0)
                    {
                        dgwTRANSACTIONS.Sort(dgwTRANSACTIONS.Columns["UDATE"], ListSortDirection.Descending);
                    }

                    // 4. Populate In-Stock Data with verified numeric stock
                    await PopulateInStockData(partName, actualStock);

                    AddLogRow($"Loaded transactions for {partName}.", Color.Green);
                }
                else
                {
                    MessageBox.Show("No stock movements found for the selected part.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                AddLogRow($"Transaction fetch error for {partName}: {ex.Message}", Color.Red);
            }
        }
    }
    
}
