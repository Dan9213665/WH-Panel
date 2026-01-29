using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Seagull.BarTender.Print.LabelFormat;

namespace WH_Panel
{
    public partial class FrmPriorityCount : Form
    {
        // Settings and Data Storage
        private AppSettings settings;
        private List<PartMnfSubform> fullAvlList = new List<PartMnfSubform>();
        private List<Warehouse> loadedWareHouses = new List<Warehouse>();
        public static string baseUrl = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522";
        public FrmPriorityCount()
        {
            InitializeComponent();
        }

        private async void FrmPriorityCount_Load(object sender, EventArgs e)
        {
            // 1. Style the UI immediately
            ApplyDarkTheme(this);
            SetupTextBoxStyles(txtSearchIPN, txtbMFPN, txtbQTY);
            // 2.Load "Workable" Snapshots from SQL(For all users)
            // This allows non-LGT users to select a DB created by LGT
            LoadExistingSnapshots();
            // 2. Load Application Settings (Crucial for API credentials)
            try
            {
                settings = SettingsManager.LoadSettings();
                if (settings == null)
                {
                    MessageBox.Show("Failed to load settings file.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error accessing settings: {ex.Message}", "Init Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Identify User and Set Permissions
            string currentUsername = GetLoggedUser().ToLower();
            bool isLgtUser = (currentUsername == "lgt");

            gbCreateCountDB.Visible = isLgtUser;
            gbAllWhs.Visible = isLgtUser;

            // 4. If Authorized, validate credentials and load data
            if (isLgtUser)
            {
                if (string.IsNullOrEmpty(settings.ApiUsername) || string.IsNullOrEmpty(settings.ApiPassword))
                {
                    MessageBox.Show("API credentials are missing in settings. Please check configuration.", "Auth Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    await LoadWarehouseData();
                }
            }
        }

        private void LoadExistingSnapshots()
        {
            string serverAddress = "DBR3\\SQLEXPRESS";
            string masterConnString = $"Server={serverAddress};Integrated Security=True;Database=master;";

            // We only want databases that look like our snapshots: e.g., DGT_2026...
            // The pattern searches for names containing an underscore followed by numbers
            string sqlQuery = @"
        SELECT name 
        FROM sys.databases 
        WHERE name LIKE '%_202[0-9]%' 
        AND name NOT IN ('master', 'model', 'msdb', 'tempdb')
        ORDER BY create_date DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(masterConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            cmbSelectedWH.Items.Clear();
                            while (reader.Read())
                            {
                                cmbSelectedWH.Items.Add(reader["name"].ToString());
                            }
                        }
                    }
                }

                if (cmbSelectedWH.Items.Count > 0)
                {
                    cmbSelectedWH.SelectedIndex = 0; // Default to newest
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading snapshots: {ex.Message}");
            }
        }


        private async Task LoadWarehouseData()
        {
            // Update this URL if your baseUrl is stored in settings as well
            string url = $"{baseUrl}/WAREHOUSES?$select=WARHSNAME,WARHSDES,WARHS";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Auth using the freshly loaded settings
                    string authInfo = $"{settings.ApiUsername}:{settings.ApiPassword}";
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(authInfo));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<WarehouseApiResponse>(responseBody);

                    if (apiResponse?.value != null)
                    {
                        cmbAllWhs.Items.Clear();
                        loadedWareHouses.Clear();

                        foreach (var warehouse in apiResponse.value)
                        {
                            cmbAllWhs.Items.Add($"{warehouse.WARHSNAME} - {warehouse.WARHSDES}");
                            loadedWareHouses.Add(warehouse);
                        }

                        cmbAllWhs.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        cmbAllWhs.AutoCompleteSource = AutoCompleteSource.ListItems;
                        cmbAllWhs.DroppedDown = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Warehouse Load Error: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetLoggedUser() => Environment.UserName;

        private void ApplyDarkTheme(Control parent)
        {
            this.BackColor = VSDarkColors.Background;
            this.ForeColor = VSDarkColors.Foreground;

            foreach (Control c in parent.Controls)
            {
                c.BackColor = VSDarkColors.Background;
                c.ForeColor = VSDarkColors.Foreground;

                if (c is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = VSDarkColors.Border;
                    btn.BackColor = VSDarkColors.Accent;
                }
                else if (c is TextBox txt)
                {
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.BackColor = VSDarkColors.Accent;
                }
                else if (c is ComboBox cmb)
                {
                    cmb.FlatStyle = FlatStyle.Flat;
                    cmb.BackColor = VSDarkColors.Accent;
                    cmb.ForeColor = VSDarkColors.Foreground;
                }

                if (c.HasChildren) ApplyDarkTheme(c);
            }
        }

        private void cmbAllWhs_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCreateCountDB.Text = "Create Count DB for " + cmbAllWhs.SelectedItem.ToString();
        }

        private async void btnCreateCountDB_Click(object sender, EventArgs e)
        {
            if (cmbAllWhs.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a warehouse first.");
                return;
            }

            // 1. Setup Variables
            string warehouseName = loadedWareHouses[cmbAllWhs.SelectedIndex].WARHSNAME;
            string dateTimeNow = DateTime.Now.ToString("yyyyMMddHHmm");
            string dbWHName = $"{warehouseName}_{dateTimeNow}";
            string serverAddress = "DBR3\\SQLEXPRESS";
            string masterConnString = $"Server={serverAddress};Integrated Security=True;Database=master;";
            string newDbConnString = $"Server={serverAddress};Integrated Security=True;Database={dbWHName};";

            btnCreateCountDB.Enabled = false;
            btnCreateCountDB.Text = "Fetching Priority Data...";

            try
            {
                // STEP 1: Fetch Data from Priority API
                var subformItems = await FetchPriorityInventoryAsync(warehouseName);

                if (subformItems != null && subformItems.Count > 0)
                {
                    btnCreateCountDB.Text = "Creating SQL Database...";

                    // STEP 2: Create the physical Database on the server
                    CreatePhysicalDatabase(masterConnString, dbWHName);

                    // STEP 3: Create the Schema (STOCK, COUNT, AVL) inside the new DB
                    btnCreateCountDB.Text = "Building Tables...";
                    CreateDatabaseSchema(newDbConnString);

                    // STEP 4: Bulk Insert the Priority Snapshot into the STOCK table
                    btnCreateCountDB.Text = "Populating STOCK...";
                    await BulkInsertToStockAsync(newDbConnString, subformItems);

                    MessageBox.Show($"Success! Database '{dbWHName}' created and populated with {subformItems.Count} items.",
                                    "ITP System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCreateCountDB.Enabled = true;
                btnCreateCountDB.Text = "Create Count DB for " + cmbAllWhs.SelectedItem?.ToString();
            }
        }

        // --- SUB FUNCTIONS ---

        private async Task<List<WarehouseBalance>> FetchPriorityInventoryAsync(string whName)
        {
            string url = $"{baseUrl}/WAREHOUSES?$filter=WARHSNAME eq '{whName}'&$expand=WARHSBAL_SUBFORM($select=PARTNAME,PARTDES,TBALANCE;$filter=BALANCE gt 0)";

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                string authInfo = $"{settings.ApiUsername}:{settings.ApiPassword}";
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(authInfo));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<WarehouseApiResponse>(json);

                return apiResponse?.value.FirstOrDefault()?.WARHSBAL_SUBFORM;
            }
        }

        private void CreatePhysicalDatabase(string masterConn, string dbName)
        {
            using (SqlConnection conn = new SqlConnection(masterConn))
            {
                conn.Open();
                // SQL Server does not allow variables for DB names in CREATE DATABASE, so we use string interpolation
                string sql = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{dbName}') CREATE DATABASE [{dbName}]";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void CreateDatabaseSchema(string dbConn)
        {
            using (SqlConnection conn = new SqlConnection(dbConn))
            {
                conn.Open();
                string sql = @"
            CREATE TABLE STOCK (
                IPN NVARCHAR(50) PRIMARY KEY,
                Description NVARCHAR(MAX),
                PriorityQty DECIMAL(18,4),
                IsInitialized BIT DEFAULT 0,
                IsCounted BIT DEFAULT 0,
                SnapshotDate DATETIME
            );

            CREATE TABLE COUNT (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                IPN NVARCHAR(50),
                PackageID NVARCHAR(100),
                ExpectedQty DECIMAL(18,4),
                ActualQty DECIMAL(18,4) NULL,
                Status INT DEFAULT 0,
                CountDate DATETIME NULL,
                UserCounted NVARCHAR(50)
            );

            CREATE TABLE AVL (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                IPN NVARCHAR(50),
                Manufacturer NVARCHAR(100),
                MPN NVARCHAR(100),
                Preference INT
            );";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private async Task BulkInsertToStockAsync(string dbConn, List<WarehouseBalance> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("IPN", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("PriorityQty", typeof(decimal));
            dt.Columns.Add("IsInitialized", typeof(bool));
            dt.Columns.Add("IsCounted", typeof(bool));
            dt.Columns.Add("SnapshotDate", typeof(DateTime));

            foreach (var item in items)
            {
                dt.Rows.Add(item.PARTNAME, item.PARTDES, item.TBALANCE, false, false, DateTime.Now);
            }

            using (SqlConnection conn = new SqlConnection(dbConn))
            {
                await conn.OpenAsync();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = "STOCK";
                    bulkCopy.ColumnMappings.Add("IPN", "IPN");
                    bulkCopy.ColumnMappings.Add("Description", "Description");
                    bulkCopy.ColumnMappings.Add("PriorityQty", "PriorityQty");
                    bulkCopy.ColumnMappings.Add("IsInitialized", "IsInitialized");
                    bulkCopy.ColumnMappings.Add("IsCounted", "IsCounted");
                    bulkCopy.ColumnMappings.Add("SnapshotDate", "SnapshotDate");

                    await bulkCopy.WriteToServerAsync(dt);
                }
            }
        }

        private void btnStockReport_Click(object sender, EventArgs e)
        {
            if (cmbSelectedWH.SelectedItem == null)
            {
                MessageBox.Show("Please select a warehouse snapshot first.");
                return;
            }

            string selectedDb = cmbSelectedWH.SelectedItem.ToString();
            string selectedPrefix = selectedDb.Split('_')[0]; // Extract "DGT" from "DGT_2026..."
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");

            // Your provided network path
            string directoryPath = $@"\\dbr1\Data\WareHouse\2025\WHsearcher\";
            string filename = Path.Combine(directoryPath, $"{selectedPrefix}_StockReport_{_fileTimeStamp}.html");

            string connectionString = $"Server=DBR3\\SQLEXPRESS;Integrated Security=True;Database={selectedDb};";

            try
            {
                StringBuilder html = new StringBuilder();

                // Basic CSS for a "High Russian" / Professional look
                html.Append("<html><head><style>");
                html.Append("body { font-family: Calibri, sans-serif; background-color: #1e1e1e; color: #dcdcdc; padding: 20px; }");
                html.Append("h2 { color: #569cd6; border-bottom: 2px solid #3e3e42; padding-bottom: 10px; }");
                html.Append("table { width: 100%; border-collapse: collapse; margin-top: 20px; background-color: #252526; }");
                html.Append("th { background-color: #2d2d30; color: #9cdcfe; padding: 12px; text-align: left; border: 1px solid #3e3e42; }");
                html.Append("td { padding: 10px; border: 1px solid #3e3e42; }");
                html.Append("tr:nth-child(even) { background-color: #2a2a2b; }");
                html.Append("tr:hover { background-color: #3e3e42; }");
                html.Append("</style></head><body>");

                html.Append($"<h2>Inventory Snapshot: {selectedDb}</h2>");
                html.Append($"<p>Report Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");

                html.Append("<table><tr><th>IPN</th><th>Description</th><th>Priority Qty</th><th>Initialized</th><th>Counted</th><th>Snapshot Date</th></tr>");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT IPN, Description, PriorityQty, IsInitialized, IsCounted, SnapshotDate FROM STOCK ORDER BY IPN ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            html.Append("<tr>");
                            html.Append($"<td>{reader["IPN"]}</td>");
                            html.Append($"<td>{reader["Description"]}</td>");
                            html.Append($"<td>{Convert.ToDecimal(reader["PriorityQty"]):F0}</td>");
                            html.Append($"<td>{(Convert.ToBoolean(reader["IsInitialized"]) ? "Yes" : "No")}</td>");
                            html.Append($"<td>{(Convert.ToBoolean(reader["IsCounted"]) ? "Yes" : "No")}</td>");
                            html.Append($"<td>{Convert.ToDateTime(reader["SnapshotDate"]):yyyy-MM-dd HH:mm}</td>");
                            html.Append("</tr>");
                        }
                    }
                }

                html.Append("</table></body></html>");

                // Ensure directory exists
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                // Write file
                File.WriteAllText(filename, html.ToString(), Encoding.UTF8);

                // Open the report automatically
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filename) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeltaReport_Click(object sender, EventArgs e)
        {
            if (cmbSelectedWH.SelectedItem == null) return;

            string selectedDb = cmbSelectedWH.SelectedItem.ToString();
            string selectedPrefix = selectedDb.Split('_')[0];
            string connectionString = $"Server=DBR3\\SQLEXPRESS;Integrated Security=True;Database={selectedDb};";
            string filename = Path.Combine($@"\\dbr1\Data\WareHouse\2025\WHsearcher\", $"{selectedPrefix}_DeltaReport_{DateTime.Now:yyyyMMddHHmm}.html");

            try
            {
                StringBuilder html = new StringBuilder();
                // 
                html.Append("<html><head><meta charset='UTF-8'><style>");
                html.Append("body { font-family: 'Segoe UI', sans-serif; background-color: #1e1e1e; color: #dcdcdc; padding: 20px; }");
                html.Append("table { width: 100%; border-collapse: collapse; margin-top: 20px; }");
                html.Append("th, td { border: 1px solid #3e3e42; padding: 10px; text-align: left; }");
                html.Append("th { background-color: #2d2d30; color: #569cd6; }");
                html.Append(".delta-pos { color: #4ec9b0; } .delta-neg { color: #f44747; } .detail { font-size: 0.85em; color: #858585; }");
                html.Append("</style></head><body>");
                html.Append($"<h2>Delta Report: {selectedPrefix}</h2>");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // This query joins STOCK with a grouped SUM from COUNT
                    string query = @"
                SELECT 
                    S.IPN, 
                    S.Description, 
                    S.PriorityQty, 
                    ISNULL(C.TotalCounted, 0) as TotalCounted,
                    C.CountDetails
                FROM STOCK S
                LEFT JOIN (
                    SELECT 
                        IPN, 
                        SUM(ISNULL(ActualQty, 0)) as TotalCounted,
                        STRING_AGG(CAST(ISNULL(ActualQty, 0) AS VARCHAR), ' + ') as CountDetails
                    FROM COUNT
                    GROUP BY IPN
                ) C ON S.IPN = C.IPN
                ORDER BY S.IPN";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        html.Append("<table><tr><th>IPN</th><th>Description</th><th>Priority</th><th>Counted</th><th>Delta</th><th>Details</th></tr>");

                        while (reader.Read())
                        {
                            decimal pQty = Convert.ToDecimal(reader["PriorityQty"]);
                            decimal cQty = Convert.ToDecimal(reader["TotalCounted"]);
                            decimal delta = cQty - pQty;
                            string details = reader["CountDetails"]?.ToString() ?? "-";

                            // Only highlighting rows that are actually counted or have a discrepancy
                            string deltaClass = delta >= 0 ? "delta-pos" : "delta-neg";

                            html.Append("<tr>");
                            html.Append($"<td>{reader["IPN"]}</td>");
                            html.Append($"<td>{reader["Description"]}</td>");
                            html.Append($"<td>{pQty:F0}</td>");
                            html.Append($"<td>{cQty:F0}</td>");
                            html.Append($"<td class='{deltaClass}'>{delta:F0}</td>");
                            html.Append($"<td class='detail'>{details}</td>");
                            html.Append("</tr>");
                        }
                    }
                }

                html.Append("</table></body></html>");
                File.WriteAllText(filename, html.ToString(), Encoding.UTF8);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filename) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delta Error: {ex.Message}");
            }
        }

        private async void txtSearchIPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string inputIPN = txtSearchIPN.Text.Trim().ToUpper();

                if (cmbSelectedWH.SelectedItem == null)
                {
                    MessageBox.Show("Please select an active Snapshot first.");
                    return;
                }

                // 1. Fetch AVL data from Priority
                bool success = await getMFPNSfromPRIORITY(inputIPN);

                if (success)
                {
                    // 2. Prepare for the next scan
                    txtbMFPN.Clear();
                    txtbMFPN.Focus();
                }
            }
        }

        private async Task<bool> getMFPNSfromPRIORITY(string ipn)
        {
            string url = $"{baseUrl}/PART?$filter=PARTNAME eq '{ipn}'&$expand=PARTMNF_SUBFORM($select=MNFPARTNAME,MNFPARTDES,MNFNAME)";

            Log($"Querying Priority AVL for: {ipn}...", Color.Cyan);

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);

                    string authInfo = $"{settings.ApiUsername}:{settings.ApiPassword}";
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(authInfo));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();

                    var partData = JsonConvert.DeserializeObject<PartApiResponse>(json);

                    if (partData?.value != null && partData.value.Count > 0)
                    {
                        var avlList = partData.value[0].PARTMNF_SUBFORM;
                        int avlCount = avlList?.Count ?? 0;

                        fullAvlList = partData.value[0].PARTMNF_SUBFORM; // Save the full list here
                        // 1. Bind Data
                        dgwAVL.DataSource = avlList;

                        // 2. Apply VS Dark Mode Styling
                        ApplyDarkThemeToGrid(dgwAVL);

                        // 3. Header Text & Autosizing
                        if (dgwAVL.Columns["MNFPARTNAME"] != null) dgwAVL.Columns["MNFPARTNAME"].HeaderText = "MFPN";
                        if (dgwAVL.Columns["MNFNAME"] != null) dgwAVL.Columns["MNFNAME"].HeaderText = "Manufacturer";
                        if (dgwAVL.Columns["MNFPARTDES"] != null) dgwAVL.Columns["MNFPARTDES"].HeaderText = "MFPN Description";

                        dgwAVL.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        if (dgwAVL.Columns.Count > 0)
                            dgwAVL.Columns[dgwAVL.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                        Log($"SUCCESS: Found {avlCount} Authorized Vendors for {ipn}.", Color.LimeGreen);
                        return true;
                    }
                    else
                    {
                        Log($"WARNING: No AVL data found in Priority for {ipn}.", Color.Yellow);
                        MessageBox.Show("No AVL data found for this IPN in Priority.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"API ERROR: {ex.Message}", Color.Red);
                MessageBox.Show($"AVL Load Error: {ex.Message}");
                return false;
            }
        }

        // --- Helper for Grid Styling ---
        private void ApplyDarkThemeToGrid(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false; // Required to change header colors
            dgv.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgv.ForeColor = Color.FromArgb(220, 220, 220);
            dgv.GridColor = Color.FromArgb(60, 60, 60);

            // Row Styles
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 79, 120);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            // Header Styles
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(86, 156, 214); // Light Blue
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            dgv.RowHeadersVisible = false; // Cleaner look for lists
            dgv.BorderStyle = BorderStyle.None;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
        }
        private async Task ProcessIPNScan(string ipn)
        {

        }

        private void SetupTextBoxStyles(params TextBox[] textBoxes)
        {
            foreach (var tb in textBoxes)
            {
                // Set initial state
                tb.BackColor = VSDarkColors.Background;
                tb.ForeColor = VSDarkColors.Foreground;

                tb.Enter += (s, e) =>
                {
                    tb.BackColor = VSDarkColors.FocusBackground;
                    tb.ForeColor = VSDarkColors.FocusForeground;
                };

                tb.Leave += (s, e) =>
                {
                    tb.BackColor = VSDarkColors.Background;
                    tb.ForeColor = VSDarkColors.Foreground;
                };
            }
        }

        //private void txtbMFPN_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        e.SuppressKeyPress = true;
        //        string scannedMFPN = txtbMFPN.Text.Trim().ToUpper();

        //        if (string.IsNullOrEmpty(scannedMFPN)) return;

        //        // 1. Check if AVL data exists
        //        var avlList = dgwAVL.DataSource as List<PartMnfSubform>;
        //        if (avlList == null || avlList.Count == 0)
        //        {
        //            Log("WARNING: No AVL data loaded. Please scan an IPN first.", Color.Orange);
        //            txtSearchIPN.Focus();
        //            return;
        //        }

        //        // 2. Perform the Filter
        //        // Note: Using .Equals for strict verification
        //        var filteredMatch = avlList.Where(x => x.MNFPARTNAME.ToUpper().Equals(scannedMFPN)).ToList();

        //        if (filteredMatch.Count == 1)
        //        {
        //            // SUCCESS
        //            dgwAVL.DataSource = filteredMatch;
        //            Log($"VERIFIED MFPN: {scannedMFPN} | Mfr: {filteredMatch[0].MNFNAME}", Color.LimeGreen);

        //            // Move focus to Qty
        //            txtbQTY.Focus();
        //        }
        //        else if (filteredMatch.Count > 1)
        //        {
        //            // AMBIGUITY
        //            dgwAVL.DataSource = filteredMatch;
        //            Log($"AMBIGUITY: {filteredMatch.Count} items match '{scannedMFPN}'. Select manually.", Color.Yellow);
        //        }
        //        else
        //        {
        //            // THE MORTAL SIN
        //            Log($"!!! MORTAL SIN !!!: Invalid MFPN [{scannedMFPN}] for this IPN.", Color.Red);

        //            MessageBox.Show(
        //                $"MORTAL SIN DETECTED!\n\nThe scanned MFPN '{scannedMFPN}' does not belong to this IPN's AVL.\n\n" +
        //                "Check the reel again immediately!",
        //                "VALIDATION FAILED",
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Error);

        //            txtbMFPN.SelectAll();
        //            txtbMFPN.Focus();
        //        }
        //    }
        //}

        private void txtbMFPN_KeyDown(object sender, KeyEventArgs e)
        {
            // --- ESCAPE KEY LOGIC ---
            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                txtbMFPN.Clear();

                // Restore the full list to the grid
                dgwAVL.DataSource = null; // Reset binding
                dgwAVL.DataSource = fullAvlList;

                Log("MFPN filter cleared. Full AVL list restored.", Color.White);
                txtbMFPN.Focus();
                return;
            }

            // --- ENTER KEY LOGIC ---
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string scannedMFPN = txtbMFPN.Text.Trim().ToUpper();

                if (string.IsNullOrEmpty(scannedMFPN)) return;

                if (fullAvlList == null || fullAvlList.Count == 0)
                {
                    Log("WARNING: No AVL data loaded. Please scan an IPN first.", Color.Orange);
                    txtSearchIPN.Focus();
                    return;
                }

                // Use the fullAvlList for filtering so we always check the original data
                var filteredMatch = fullAvlList.Where(x => x.MNFPARTNAME.ToUpper().Equals(scannedMFPN)).ToList();

                if (filteredMatch.Count == 1)
                {
                    dgwAVL.DataSource = filteredMatch;
                    Log($"VERIFIED MFPN: {scannedMFPN} | Mfr: {filteredMatch[0].MNFNAME}", Color.LimeGreen);
                    txtbQTY.Focus();
                }
                else if (filteredMatch.Count > 1)
                {
                    dgwAVL.DataSource = filteredMatch;
                    Log($"AMBIGUITY: {filteredMatch.Count} items match '{scannedMFPN}'. Select manually.", Color.Yellow);
                }
                else
                {
                    Log($"!!! MORTAL SIN !!!: Invalid MFPN [{scannedMFPN}] for this IPN.", Color.Red);
                    MessageBox.Show($"MORTAL SIN DETECTED!\n\nThe scanned MFPN '{scannedMFPN}' does not belong to this IPN's AVL.", "VALIDATION FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtbMFPN.SelectAll();
                }
            }
        }

        private void Log(string message, Color color)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => Log(message, color)));
                return;
            }

            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor = color;

            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            rtbLog.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
            rtbLog.ScrollToCaret();
        }


    }



    public static class VSDarkColors
    {
        public static Color Background = Color.FromArgb(30, 30, 30);
        public static Color Foreground = Color.FromArgb(220, 220, 220);
        public static Color Accent = Color.FromArgb(45, 45, 48);
        public static Color Border = Color.FromArgb(63, 63, 70);

        // New Focus Colors
        public static Color FocusBackground = Color.LightGreen;
        public static Color FocusForeground = Color.Black;
    }
  
 }