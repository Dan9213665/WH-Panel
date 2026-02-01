using Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Seagull.BarTender.Print.LabelFormat;
using static WH_Panel.FrmPriorityAPI;
using Action = System.Action;
using Exception = System.Exception;

namespace WH_Panel
{
    public partial class FrmPriorityCount : Form
    {
        // Settings and Data Storage
        private AppSettings settings;
        private List<PartMnfSubform> fullAvlList = new List<PartMnfSubform>();
        private List<Warehouse> loadedWareHouses = new List<Warehouse>();
        private Dictionary<string, ReelState> currentIPNState = new Dictionary<string, ReelState>();
        public static string baseUrl = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522";
        public FrmPriorityCount()
        {
            InitializeComponent();
        }

        private void SetupCountedLog()
        {
            dgwCountedLog.Columns.Clear();
            dgwCountedLog.AutoGenerateColumns = false;

            // 1. Hardcode columns
            dgwCountedLog.Columns.Add("LOG_IPN", "IPN");
            dgwCountedLog.Columns.Add("UDATE", "Date");
            dgwCountedLog.Columns.Add("LOGDOCNO", "Doc No");
            dgwCountedLog.Columns.Add("BOOKNUM", "Client Doc");
            dgwCountedLog.Columns.Add("TQUANT", "Qty");
            dgwCountedLog.Columns.Add("SUPCUSTNAME", "Source");
            dgwCountedLog.Columns.Add("PACKNAME", "Pack Code");
            dgwCountedLog.Columns.Add("CountDate", "Count Date");
            dgwCountedLog.Columns.Add("UserCounted", "User");

            // 2. Set Auto-Fit Logic
            // AllCells ensures it looks at the headers and the new data you just scanned
            dgwCountedLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // 3. Optional: Specific constraints for wider columns
            dgwCountedLog.Columns["SUPCUSTNAME"].MinimumWidth = 120;
            dgwCountedLog.Columns["LOG_IPN"].MinimumWidth = 100;

            ApplyDarkThemeToGrid(dgwCountedLog);

            // 4. Optimization: Disable resizing for the user to prevent accidental clicks
            dgwCountedLog.AllowUserToResizeColumns = false;
        }

        private async void FrmPriorityCount_Load(object sender, EventArgs e)
        {
            // 1. Style the UI immediately
            ApplyDarkTheme(this);
            SetupTextBoxStyles(txtSearchIPN, txtbMFPN, txtbQTY);
            // 2.Load "Workable" Snapshots from SQL(For all users)
            // This allows non-LGT users to select a DB created by LGT
            LoadExistingSnapshots();
            InitializeMovementsGrid();
            SetupCountedLog();
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
            -- 1. Updated Snapshot Table (Book Value)
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'STOCK')
            CREATE TABLE STOCK (
                IPN NVARCHAR(50) PRIMARY KEY,
                Description NVARCHAR(MAX),
                PriorityQty INT, -- Changed to INT for discrete reel counts
                IsInitialized BIT DEFAULT 0,
                IsCounted BIT DEFAULT 0,
                SnapshotDate DATETIME
            );

            -- 2. Updated Transactional Count Table (Physical Truth)
            -- Aligned with: Id, IPN, PackageID, ActualQty, CountDate, UserCounted, 
            -- OriginalDoc, PackageType, BookNum, Supplier, PriorityDate
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'COUNT')
            CREATE TABLE [COUNT] (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                IPN NVARCHAR(50) NULL,
                PackageID NVARCHAR(100) NULL,
                ActualQty INT NULL, -- Discrete integer count
                CountDate DATETIME NULL,
                UserCounted NVARCHAR(50) NULL,
                OriginalDoc NVARCHAR(50) NULL, -- Unique Anchor (e.g., GR26000200)
                PackageType NVARCHAR(50) NULL, -- User-selected packaging
                BookNum NVARCHAR(50) NULL,      -- Priority Reference
                Supplier NVARCHAR(100) NULL,
                PriorityDate DATETIME NULL      -- ERP Transaction Timestamp
            );

            -- 3. AVL Table (Manufacturer matching)
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AVL')
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

            if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                txtSearchIPN.Clear();
                // 1. Detach DataSources to avoid the InvalidOperationException
                dgwAVL.DataSource = null;
                dgvStockMovements.DataSource = null;
                dgwINSTOCK.DataSource = null;

                // 2. Now it is safe to manually clear rows/columns if they aren't auto-generated
                dgwAVL.Rows.Clear();
                dgvStockMovements.Rows.Clear();
                dgwINSTOCK.Rows.Clear();
                lblBalance.Text = "0/0";

                Log("IPN filter cleared.", Color.White);
                txtSearchIPN.Focus();
                return;
            }

            else if (e.KeyCode == Keys.Enter)
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

                getAllMovementsForIPN(inputIPN);




                if (success)
                {
                    // 2. Prepare for the next scan
                    txtbMFPN.Clear();
                    txtbMFPN.Focus();
                }
            }
        }

        private async Task getAllMovementsForIPN(string ipn)
        {
            Log($"Fetching live stock movements for {ipn}...", Color.Cyan);

            // We target the LOGPART endpoint using your established pattern
            string logPartUrl = $"{baseUrl}/LOGPART?$filter=PARTNAME eq '{ipn}'&$expand=PARTTRANSLAST2_SUBFORM";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    ApiHelper.AuthenticateClient(client); // Uses your existing Auth logic

                    var response = await client.GetAsync(logPartUrl);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    var logPartApiResponse = JsonConvert.DeserializeObject<LogPartApiResponse>(json);

                    if (logPartApiResponse.value != null && logPartApiResponse.value.Count > 0)
                    {
                        // Prepare the grid for live display
                        dgvStockMovements.Rows.Clear();

                        // Extract and filter the subform data
                        var movements = logPartApiResponse.value[0].PARTTRANSLAST2_SUBFORM
                            .Where(t => t.DOCDES != "קיזוז אוטומטי" && t.TOWARHSNAME != "666")
                            .ToList();

                     
                        foreach (var trans in movements)
                        {
                            int rowIndex = dgvStockMovements.Rows.Add(
                                "",               // UDATE
                                trans.LOGDOCNO,
                                trans.DOCDES,
                                trans.SUPCUSTNAME,
                                "",               // BOOKNUM
                                trans.TQUANT,
                                ""                // PACKNAME
                            );

                            var row = dgvStockMovements.Rows[rowIndex];
                            var qtyCell = row.Cells["TQUANT"];
                            string docNo = trans.LOGDOCNO.ToUpper();

                            // Segregate coloring by Document Type
                            if (docNo.StartsWith("GR"))
                            {
                                // INCOMING
                                qtyCell.Style.BackColor = Color.LightGreen;
                                qtyCell.Style.ForeColor = Color.Black;
                            }
                            else if (docNo.StartsWith("ROB") || docNo.StartsWith("IC") || docNo.StartsWith("WR"))
                            {
                                // OUTGOING / ADJUSTMENT (Inventory Count 'IC' is often a reduction in this view)
                                qtyCell.Style.BackColor = Color.IndianRed;
                                qtyCell.Style.ForeColor = Color.Black;
                            }
                            else if (docNo.StartsWith("SH"))
                            {
                                // WAREHOUSE TRANSFER - Often depends on the DOCDES or Warehouse name
                                // For now, let's mark it as Neutral/Cyan or check direction
                                qtyCell.Style.BackColor = Color.DarkViolet;
                                qtyCell.Style.ForeColor = Color.White;
                            }
                            else
                            {
                                // Default Dark Mode
                                qtyCell.Style.BackColor = VSDarkColors.Background;
                                qtyCell.Style.ForeColor = VSDarkColors.Foreground;
                            }
                        }

                        Log($"Displaying {movements.Count} potential reels. Starting background enrichment...", Color.Yellow);

                        // Now run your parallel enrichment logic to fetch PACKCODE and BOOKNUM
                        await EnrichLiveGridAsync(ipn);
                    }
                    else
                    {
                        Log($"No transaction history found for {ipn}.", Color.Orange);
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Movements Load Error: {ex.Message}", Color.Red);
            }



        }





        private async Task EnrichLiveGridAsync(string partName)
        {
            var rows = dgvStockMovements.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["LOGDOCNO"].Value != null)
                .ToList();

            if (rows.Count == 0) return;

            // --- Start Timing ---
            var sw = System.Diagnostics.Stopwatch.StartNew();

            var clients = Enumerable.Range(0, ApiUserPool.Count)
                .Select(_ =>
                {
                    var c = new HttpClient();
                    c.DefaultRequestHeaders.Accept.Clear();
                    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string user = ApiHelper.AuthenticateClient(c);
                    return (User: user, Client: c);
                }).ToList();

            int maxConcurrencyPerUser = 2;
            var clientSemaphores = clients.ToDictionary(c => c.User, c => new SemaphoreSlim(maxConcurrencyPerUser));
            int userIndex = -1;
            object userLock = new object();

            (string User, HttpClient Client) GetNextClient()
            {
                lock (userLock)
                {
                    userIndex = (userIndex + 1) % clients.Count;
                    return clients[userIndex];
                }
            }

            var tasks = rows.Select(async row =>
            {
                string logDocNo = row.Cells["LOGDOCNO"].Value.ToString();
                var (usedUser, client) = GetNextClient();
                var semaphore = clientSemaphores[usedUser];

                await semaphore.WaitAsync();
                try
                {
                    string url = logDocNo switch
                    {
                        var s when s.StartsWith("GR") => $"{baseUrl}/DOCUMENTS_P?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_P_SUBFORM",
                        var s when s.StartsWith("WR") => $"{baseUrl}/DOCUMENTS_T?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_T_SUBFORM",
                        var s when s.StartsWith("SH") => $"{baseUrl}/DOCUMENTS_D?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_D_SUBFORM",
                        var s when s.StartsWith("IC") => $"{baseUrl}/DOCUMENTS_C?$filter=DOCNO eq '{logDocNo}'",
                        var s when s.StartsWith("ROB") => $"{baseUrl}/SERIAL?$filter=SERIALNAME eq '{logDocNo}'&$expand=TRANSORDER_K_SUBFORM",
                        _ => $"{baseUrl}/DOCUMENTS_P?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_P_SUBFORM"
                    };

                    HttpResponseMessage response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode) return;

                    string body = await response.Content.ReadAsStringAsync();
                    var doc = JsonConvert.DeserializeObject<JObject>(body)?["value"]?.FirstOrDefault();

                    if (doc != null)
                    {
                        string uDate = doc["UDATE"]?.ToString();
                        string bookNum = doc["BOOKNUM"]?.ToString()
                                         ?? doc["CDES"]?.ToString()
                                         ?? doc["REFERENCE"]?.ToString();

                        string packCode = null;

                        var subForm = doc["TRANSORDER_P_SUBFORM"]
                                      ?? doc["TRANSORDER_T_SUBFORM"]
                                      ?? doc["TRANSORDER_D_SUBFORM"]
                                      ?? doc["TRANSORDER_K_SUBFORM"];

                        if (subForm != null)
                        {
                            var line = subForm.FirstOrDefault(l => l["PARTNAME"]?.ToString() == partName);
                            if (line != null)
                            {
                                packCode = line["PACKCODE"]?.ToString();
                                if (string.IsNullOrEmpty(bookNum))
                                {
                                    bookNum = line["BOOKNUM"]?.ToString();
                                }
                            }
                        }

                        this.Invoke((Action)(() =>
                        {
                            row.Cells["UDATE"].Value = uDate;
                            row.Cells["PACKNAME"].Value = packCode;
                            row.Cells["BOOKNUM"].Value = bookNum;
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Log($"Enrichment error for {logDocNo}: {ex.Message}", Color.Gray);
                }
                finally
                {
                    semaphore.Release();
                }
            });


            // AT THE END OF EnrichLiveGridAsync
            await Task.WhenAll(tasks);
            sw.Stop();

            // Now that we ARE sure data is there, calculate stock
            this.Invoke((Action)(() =>
            {
                dgvStockMovements.Sort(dgvStockMovements.Columns["UDATE"], ListSortDirection.Descending);
                PopulateInStockByLogic(); // Call it here!
            }));


            Log($"Reel data enrichment complete in {sw.ElapsedMilliseconds} ms.", Color.LimeGreen);
        }



        private async Task PopulateInStockByLogic()
        {
            // 1. Ensure the IN STOCK grid has a skeleton
            if (dgwINSTOCK.Columns.Count == 0)
            {
                dgwINSTOCK.Columns.Add("UDATE", "Date");
                dgwINSTOCK.Columns.Add("LOGDOCNO", "Doc No");
                dgwINSTOCK.Columns.Add("BOOKNUM", "Client Doc");
                dgwINSTOCK.Columns.Add("TQUANT", "Qty");
                dgwINSTOCK.Columns.Add("SUPCUSTNAME", "Source");
                dgwINSTOCK.Columns.Add("PACKNAME", "Pack Code");
                dgwINSTOCK.Columns.Add("CountDate", "Count Date");
                dgwINSTOCK.Columns.Add("UserCounted", "User");
                ApplyDarkThemeToGrid(dgwINSTOCK);
            }

            dgwINSTOCK.Rows.Clear();

            // CRITICAL: Clear the memory state before rebuilding
            currentIPNState.Clear();

            // 2. Extract movements
            var rows = dgvStockMovements.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["TQUANT"].Value != null && !string.IsNullOrEmpty(r.Cells["TQUANT"].Value.ToString()))
                .Select(r => new {
                    DocNo = r.Cells["LOGDOCNO"].Value?.ToString() ?? "",
                    Qty = int.TryParse(r.Cells["TQUANT"].Value.ToString(), out int q) ? Math.Abs(q) : 0,
                    Date = DateTime.TryParse(r.Cells["UDATE"].Value?.ToString(), out var d) ? d : DateTime.MinValue,
                    Pack = r.Cells["PACKNAME"].Value?.ToString() ?? "N/A",
                    Supplier = r.Cells["SUPCUSTNAME"].Value?.ToString() ?? "",
                    BookNum = r.Cells["BOOKNUM"].Value?.ToString() ?? ""
                })
                .Where(x => x.Qty > 0)
                .ToList();

            // 3. Heuristic Reconciliation
            var incoming = rows.Where(r => r.DocNo.StartsWith("GR")).ToList();
            var outgoing = rows.Where(r => r.DocNo.StartsWith("ROB") || r.DocNo.StartsWith("SH") || r.DocNo.StartsWith("WR")).ToList();

            var remainingInStock = incoming.OrderBy(r => r.Date).ToList();
            var unmatchedOut = outgoing.OrderBy(r => r.Date).ToList();

            foreach (var outMove in unmatchedOut)
            {
                var match = remainingInStock.FirstOrDefault(i => i.Qty == outMove.Qty);
                if (match != null) remainingInStock.Remove(match);
            }

            // 4. POPULATE THE DICTIONARY (State Management)
            // We do this BEFORE the SQL sync so the dictionary exists to be updated
            foreach (var item in remainingInStock)
            {
                currentIPNState[item.DocNo] = new ReelState
                {
                    DocNo = item.DocNo,
                    Qty = item.Qty,
                    PriorityDate = item.Date,
                    PackageID = item.Pack,
                    Supplier = item.Supplier,
                    BookNum = item.BookNum,
                    User = null,
                    CountDate = null
                };
            }

            // 5. SQL SYNC: Update the dictionary with physical truth
            string currentIPN = txtSearchIPN.Text.Trim().ToUpper();
            string dbName = cmbSelectedWH.SelectedItem?.ToString() ?? "";
            string connString = $"Server=DBR3\\SQLEXPRESS;Integrated Security=True;Database={dbName};";

            if (!string.IsNullOrEmpty(dbName) && currentIPNState.Count > 0)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        await conn.OpenAsync();
                        string sql = "SELECT OriginalDoc, UserCounted, CountDate FROM [COUNT] WHERE IPN = @ipn";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ipn", currentIPN);
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    string docKey = reader["OriginalDoc"].ToString();
                                    if (currentIPNState.ContainsKey(docKey))
                                    {
                                        currentIPNState[docKey].User = reader["UserCounted"].ToString();
                                        currentIPNState[docKey].CountDate = Convert.ToDateTime(reader["CountDate"]);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { Log($"SQL Sync Error: {ex.Message}", Color.Red); }
            }

            // 6. Refresh UI from Dictionary
            // Use the function we defined earlier to rebuild the grid
            RefreshInStockGrid();

            Log($"Heuristic reconciliation complete: {currentIPNState.Count} active reels in memory.", Color.Yellow);
        }

        private void InitializeMovementsGrid()
        {
            dgvStockMovements.Columns.Clear();
            dgvStockMovements.AutoGenerateColumns = false;

            // Define the columns to match your legacy logic
            dgvStockMovements.Columns.Add(new DataGridViewTextBoxColumn { Name = "UDATE", HeaderText = "Date", Width = 110 });
            dgvStockMovements.Columns.Add(new DataGridViewTextBoxColumn { Name = "LOGDOCNO", HeaderText = "Doc No", Width = 100 });
            dgvStockMovements.Columns.Add(new DataGridViewTextBoxColumn { Name = "DOCDES", HeaderText = "Description", Width = 130 });
            dgvStockMovements.Columns.Add(new DataGridViewTextBoxColumn { Name = "SUPCUSTNAME", HeaderText = "Source", Width = 150 });
            dgvStockMovements.Columns.Add(new DataGridViewTextBoxColumn { Name = "BOOKNUM", HeaderText = "Client Doc", Width = 100 });
            dgvStockMovements.Columns.Add(new DataGridViewTextBoxColumn { Name = "TQUANT", HeaderText = "Qty", Width = 80 });
            dgvStockMovements.Columns.Add(new DataGridViewTextBoxColumn { Name = "PACKNAME", HeaderText = "Pack Code", Width = 120 });

            // Apply your Dark Mode styling immediately
            ApplyDarkThemeToGrid(dgvStockMovements);
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

            dgv.RowHeadersVisible = true; // Cleaner look for lists
            dgv.BorderStyle = BorderStyle.None;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.MultiSelect = true;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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

        private void UpdateBalanceLabel()
        {
            decimal totalCalculatedInStock = 0;
            decimal totalPhysicallyCounted = 0;

            foreach (DataGridViewRow row in dgwINSTOCK.Rows)
            {
                if (decimal.TryParse(row.Cells["TQUANT"].Value?.ToString(), out decimal qty))
                {
                    totalCalculatedInStock += qty;

                    // DATA-DRIVEN FLAG: Check if the audit fields are populated
                    bool isCounted = row.Cells["CountDate"].Value != null &&
                                     !string.IsNullOrEmpty(row.Cells["CountDate"].Value.ToString()) &&
                                     row.Cells["UserCounted"].Value != null;

                    if (isCounted)
                    {
                        totalPhysicallyCounted += qty;
                        // Apply the visual style as a consequence of the data, not as the source of truth
                        row.DefaultCellStyle.BackColor = Color.FromArgb(45, 65, 45);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = dgwINSTOCK.DefaultCellStyle.BackColor;
                    }
                }
            }

            lblBalance.Text = $"{totalPhysicallyCounted:N0} / {totalCalculatedInStock:N0}";

            if (totalPhysicallyCounted == totalCalculatedInStock && totalCalculatedInStock > 0)
            {
                lblBalance.ForeColor = Color.LimeGreen;
            }
        }

        private async Task LoadAndSyncState(string ipn, List<ReelState> priorityReels)
        {
            currentIPNState.Clear();
            string dbName = cmbSelectedWH.SelectedItem.ToString();
            string connString = $"Server=DBR3\\SQLEXPRESS;Integrated Security=True;Database={dbName};";

            // 1. Initialize dictionary with unique DocNo from Priority
            foreach (var reel in priorityReels)
            {
                currentIPNState[reel.DocNo] = reel;
            }

            // 2. Cross-reference with your SQL COUNT table using DocNo
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                // We match on OriginalDoc (which stores the DocNo)
                string sql = "SELECT OriginalDoc, UserCounted, CountDate FROM [COUNT] WHERE IPN = @ipn";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ipn", ipn);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string docKey = reader["OriginalDoc"].ToString();
                            if (currentIPNState.ContainsKey(docKey))
                            {
                                currentIPNState[docKey].User = reader["UserCounted"].ToString();
                                currentIPNState[docKey].CountDate = Convert.ToDateTime(reader["CountDate"]);
                            }
                        }
                    }
                }
            }

            RefreshInStockGrid();
        }
        private async void ProcessCount(decimal scannedQty, string packType)
        {
            // 1. Find all uncounted reels with this exact quantity
            var potentialMatches = currentIPNState.Values
                .Where(r => r.Qty == scannedQty && !r.IsCounted)
                .OrderBy(r => r.PriorityDate)
                .ToList();

            ReelState targetReel = null;

            if (potentialMatches.Count == 0)
            {
                // Check if the quantity was already counted to give a specific warning
                bool alreadyCounted = currentIPNState.Values.Any(r => r.Qty == scannedQty && r.IsCounted);
                string msg = alreadyCounted
                    ? $"Quantity {scannedQty} has already been counted. Check for duplicate labels."
                    : $"No records found for {scannedQty} pcs.";

                Log(msg, Color.Red);
                return;
            }
            else if (potentialMatches.Count == 1)
            {
                targetReel = potentialMatches[0];
            }
            else
            {
                // 2. Ambiguity: Multiple reels with same Qty. Let the user choose.
                targetReel = ShowReelSelectionDialog(potentialMatches);
                if (targetReel == null) return; // User cancelled
            }

            // 3. Final Verification and Save
            targetReel.User = Environment.UserName;
            targetReel.CountDate = DateTime.Now;

            await SaveToSql(targetReel, packType);

            RefreshInStockGrid();
            UpdateBalanceLabel();
        }

        private ReelState ShowReelSelectionDialog(List<ReelState> matches)
        {
            using (Form selectionForm = new Form())
            {
                selectionForm.Text = "Multiple Matches Found - Select Correct Reel";
                selectionForm.Size = new Size(500, 300);
                selectionForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                selectionForm.StartPosition = FormStartPosition.CenterParent;
                selectionForm.BackColor = Color.FromArgb(30, 30, 30); // Dark theme match
                selectionForm.ForeColor = Color.White;

                Label lblHeader = new Label()
                {
                    Text = $"Select the correct record for Qty: {matches[0].Qty:N0}",
                    Dock = DockStyle.Top,
                    Height = 30,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                ListBox lbChoices = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.FromArgb(45, 45, 45),
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 10)
                };

                // Populate with identifying metadata
                foreach (var m in matches)
                {
                    string display = $"{m.DocNo} | Date: {m.PriorityDate:yyyy-MM-dd} | Source: {m.Supplier}";
                    lbChoices.Items.Add(display);
                }

                Button btnSelect = new Button()
                {
                    Text = "Confirm Selection (Enter)",
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.DarkSlateBlue
                };

                // Map the selection back to the object
                ReelState selectedReel = null;
                btnSelect.Click += (s, e) =>
                {
                    if (lbChoices.SelectedIndex >= 0)
                    {
                        selectedReel = matches[lbChoices.SelectedIndex];
                        selectionForm.DialogResult = DialogResult.OK;
                    }
                };

                // Keyboard support for speed
                selectionForm.AcceptButton = btnSelect;
                lbChoices.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) btnSelect.PerformClick(); };

                selectionForm.Controls.Add(lbChoices);
                selectionForm.Controls.Add(lblHeader);
                selectionForm.Controls.Add(btnSelect);

                if (selectionForm.ShowDialog() == DialogResult.OK)
                {
                    return selectedReel;
                }
                return null;
            }
        }


        private async Task SaveToSql(ReelState reel, string userPackageType)
        {
            // Database name is dynamic based on your selected warehouse snapshot
            string dbName = cmbSelectedWH.SelectedItem.ToString();
            string connString = $"Server=DBR3\\SQLEXPRESS;Integrated Security=True;Database={dbName};";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    await conn.OpenAsync();

         
                    // Inside SaveToSql - Change the logic to be "Insert Only" for counts
                    string sql = @"
                        IF EXISTS (SELECT 1 FROM [COUNT] WHERE OriginalDoc = @doc)
                        BEGIN
                            -- Throw an error back to C# to be caught in the catch block
                            RAISERROR('This specific Document (DocNo) has already been recorded in the count.', 16, 1);
                        END
                        ELSE
                        BEGIN
                            INSERT INTO [COUNT] (IPN, PackageID, ActualQty, CountDate, UserCounted, OriginalDoc, BookNum, Supplier, PackageType, PriorityDate)
                            VALUES (@ipn, @pkg, @qty, @cDate, @user, @doc, @book, @supp, @pType, @pDate)
                        END";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Core Identification
                        cmd.Parameters.AddWithValue("@ipn", txtSearchIPN.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@doc", reel.DocNo); // Primary unique link
                        cmd.Parameters.AddWithValue("@pkg", reel.PackageID ?? "N/A");

                        // Count Data
                        cmd.Parameters.AddWithValue("@qty", reel.Qty);
                        cmd.Parameters.AddWithValue("@cDate", reel.CountDate ?? DateTime.Now);
                        cmd.Parameters.AddWithValue("@user", reel.User); // Environment.UserName
                        cmd.Parameters.AddWithValue("@pType", userPackageType);

                        // Priority Metadata for Audit Trail
                        cmd.Parameters.AddWithValue("@book", reel.BookNum ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@supp", reel.Supplier ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pDate", reel.PriorityDate);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }


            catch (Exception ex)
            {
                Log($"SQL Save Error: {ex.Message}", Color.Red);
                // Throw or handle as needed for your industrial environment
            }
        }
        private void RefreshInStockGrid()
        {
            // 1. Prevent UI flickering during bulk updates
            dgwINSTOCK.Rows.Clear();

            // 2. Iterate through the dictionary values (ordered by date for operator convenience)
            var sortedReels = currentIPNState.Values.OrderBy(r => r.PriorityDate).ToList();

            foreach (var reel in sortedReels)
            {
                int rowIndex = dgwINSTOCK.Rows.Add(
                    reel.PriorityDate.ToString("yyyy-MM-dd HH:mm"),
                    reel.DocNo,
                    reel.BookNum,
                    reel.Qty,
                    reel.Supplier,
                    reel.PackageID,
                    reel.CountDate?.ToString("yyyy-MM-dd HH:mm") ?? "", // Data-driven flag
                    reel.User ?? ""                                    // Data-driven flag
                );

                // 3. Apply visual styling based on the data state
                if (reel.IsCounted)
                {
                    dgwINSTOCK.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(45, 65, 45); // Muted Forest Green
                    dgwINSTOCK.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
            }

            // 4. Update the Balance Label once the grid is refreshed
            UpdateBalanceLabel();
        }


        //private void AddRowToCountedLog(DataGridViewRow sourceRow)
        //{
        //    // 1. Ensure the Log Grid has the same column structure if not already initialized
        //    if (dgwCountedLog.Columns.Count == 0)
        //    {
        //        foreach (DataGridViewColumn col in dgwINSTOCK.Columns)
        //        {
        //            dgwCountedLog.Columns.Add((DataGridViewColumn)col.Clone());
        //        }
        //        ApplyDarkThemeToGrid(dgwCountedLog);
        //    }

        //    // 2. Create the new row values array
        //    object[] rowValues = new object[sourceRow.Cells.Count];
        //    for (int i = 0; i < sourceRow.Cells.Count; i++)
        //    {
        //        rowValues[i] = sourceRow.Cells[i].Value;
        //    }

        //    // 3. Insert at the TOP (Index 0)
        //    dgwCountedLog.Rows.Insert(0, rowValues);

        //    // 4. Optional: Keep the green highlight to signify it was a successful count
        //    dgwCountedLog.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(45, 65, 45);
        //    dgwCountedLog.Rows[0].DefaultCellStyle.ForeColor = Color.White;

        //    // 5. Visual cue: Auto-scroll to ensure the latest is visible
        //    dgwCountedLog.FirstDisplayedScrollingRowIndex = 0;
        //}

        //private void AddRowToCountedLog(DataGridViewRow sourceRow, string ipn)
        //{
        //    // Insert at index 0 (top of the list)
        //    // capture the index to get the newly created row
        //    int rowIndex = dgwCountedLog.Rows.Insert(0, 1);
        //    DataGridViewRow newRow = dgwCountedLog.Rows[rowIndex];

        //    // 1. Map the IPN to the custom first column
        //    newRow.Cells["LOG_IPN"].Value = ipn;

        //    // 2. Loop through and map by column name to avoid alignment bugs
        //    foreach (DataGridViewColumn col in dgwINSTOCK.Columns)
        //    {
        //        if (dgwCountedLog.Columns.Contains(col.Name))
        //        {
        //            newRow.Cells[col.Name].Value = sourceRow.Cells[col.Name].Value;
        //        }
        //    }

        //    // 3. Styling for that "Successful Transaction" look
        //    newRow.DefaultCellStyle.BackColor = Color.FromArgb(45, 65, 45); // Forest Green
        //    newRow.DefaultCellStyle.ForeColor = Color.White;

        //    // 4. Auto-scroll so Daniel sees the latest scan immediately
        //    dgwCountedLog.FirstDisplayedScrollingRowIndex = 0;
        //}

        //private void AddRowToCountedLog(DataGridViewRow sourceRow, string ipn)
        //{
        //    // 1. Insert 1 row at index 0. This method returns void.
        //    dgwCountedLog.Rows.Insert(0, 1);

        //    // 2. Since we inserted at 0, the new row is at index 0
        //    DataGridViewRow newRow = dgwCountedLog.Rows[0];

        //    // 3. Map the IPN to your custom first column
        //    newRow.Cells["LOG_IPN"].Value = ipn;

        //    // 4. Map the rest of the data by column name
        //    foreach (DataGridViewColumn col in dgwINSTOCK.Columns)
        //    {
        //        if (dgwCountedLog.Columns.Contains(col.Name))
        //        {
        //            newRow.Cells[col.Name].Value = sourceRow.Cells[col.Name].Value;
        //        }
        //    }

        //    // 5. Styling: Apply your industrial "Verified" look
        //    newRow.DefaultCellStyle.BackColor = Color.FromArgb(45, 65, 45); // Forest Green
        //    newRow.DefaultCellStyle.ForeColor = Color.White;

        //    // 6. Auto-scroll so Daniel sees the latest scan immediately
        //    dgwCountedLog.FirstDisplayedScrollingRowIndex = 0;
        //}

        private void AddRowToCountedLog(DataGridViewRow sourceRow, string ipn)
        {
            // 1. Insert blank row at top (returns void)
            dgwCountedLog.Rows.Insert(0, 1);
            DataGridViewRow newRow = dgwCountedLog.Rows[0];

            // 2. Set the IPN in the first cell (Index 0)
            newRow.Cells[0].Value = ipn;

            // 3. Copy every cell from the sourceRow into the log, offset by 1
            // This handles the copy-paste perfectly regardless of column names
            for (int i = 0; i < sourceRow.Cells.Count; i++)
            {
                // source index i maps to log index i + 1
                newRow.Cells[i + 1].Value = sourceRow.Cells[i].Value;
            }

            // 4. Styling (Forest Green for verified items)
            newRow.DefaultCellStyle.BackColor = Color.FromArgb(45, 65, 45);
            newRow.DefaultCellStyle.ForeColor = Color.White;

            // 5. Ensure the latest scan is visible at the top
            if (dgwCountedLog.Rows.Count > 0)
                dgwCountedLog.FirstDisplayedScrollingRowIndex = 0;
        }

        private async void txtbQTY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                // 1. Transactional Input (Int for clean inventory tracking)
                if (!int.TryParse(txtbQTY.Text.Trim(), out int scannedQty)) return;

                string operatorPackageSelection = cmbPackage.Text;
                string currentIPN = txtSearchIPN.Text.Trim().ToUpper();

                // 2. Locate the specific match in memory
                var potentialMatches = currentIPNState.Values
                    .Where(r => r.Qty == scannedQty && !r.IsCounted)
                    .OrderBy(r => r.PriorityDate)
                    .ToList();

                ReelState targetReel = null;

                // 3. Match Identification
                if (potentialMatches.Count == 0)
                {
                    bool alreadyDone = currentIPNState.Values.Any(r => r.Qty == scannedQty && r.IsCounted);
                    Log(alreadyDone
                        ? $"Qty {scannedQty} already verified for this IPN."
                        : $"!!! UNKNOWN REEL !!! No record found for Qty: {scannedQty}.", Color.Red);

                    txtbQTY.Clear();
                    return;
                }
                else if (potentialMatches.Count == 1)
                {
                    targetReel = potentialMatches[0];
                }
                else
                {
                    // Selection dialog for ambiguous quantities
                    targetReel = ShowReelSelectionDialog(potentialMatches);
                    if (targetReel == null) return;
                }

                // --- RE-INTEGRATED PACKAGING VALIDATION ---
                // 4. Validation: Packaging Type Check
                if (targetReel.PackageID != operatorPackageSelection)
                {
                    MessageBox.Show($"Incorrect Package Selection!\n\n" +
                                    $"Priority Record {targetReel.DocNo} is defined as: {targetReel.PackageID}.\n" +
                                    $"Your current selection is: {operatorPackageSelection}.\n\n" +
                                    $"Please correct the dropdown to match the system record.",
                                    "Packaging Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Auto-correct the dropdown to help the user and reset focus
                    cmbPackage.SelectedItem = targetReel.PackageID;
                    txtbQTY.Clear();
                    txtbQTY.Focus();
                    return; // Block save until user confirms correction
                }

                // 5. Verification & Audit Trail
                targetReel.User = Environment.UserName;
                targetReel.CountDate = DateTime.Now;

                try
                {
                    // Commit individual transaction to SQL
                    await SaveToSql(targetReel, operatorPackageSelection);

                    // 6. Update UI for the current item
                    RefreshInStockGrid();
                    UpdateBalanceLabel();

                    //// 7. Push to the Persistent Session Log (LIFO)
                    //var newlyCountedRow = dgwINSTOCK.Rows.Cast<DataGridViewRow>()
                    //    .FirstOrDefault(r => r.Cells["LOGDOCNO"].Value?.ToString() == targetReel.DocNo);

                    //if (newlyCountedRow != null)
                    //{
                    //    AddRowToCountedLog(newlyCountedRow);
                    //}

                    // Find the row we just updated to clone it into the persistent log
                    var rowToLog = dgwINSTOCK.Rows.Cast<DataGridViewRow>()
                        .FirstOrDefault(r => r.Cells["LOGDOCNO"].Value?.ToString() == targetReel.DocNo);

                    if (rowToLog != null)
                    {
                        // Pass BOTH the row and the current IPN string
                        string currentIPNtouseForLog = txtSearchIPN.Text.Trim().ToUpper();
                        AddRowToCountedLog(rowToLog, currentIPNtouseForLog);
                    }

                    Log($"VERIFIED: {scannedQty} pcs (Doc: {targetReel.DocNo})", Color.LimeGreen);

                    // 8. Reset inputs only - ready for next reel
                    ResetSessionForNextIPN();
                }
                catch (Exception ex)
                {
                    targetReel.CountDate = null;
                    targetReel.User = null;
                    Log($"DATABASE ERROR: {ex.Message}", Color.Red);
                }
            }
        }
        private void ResetSessionForNextIPN()
        {
            // 1. Clear Memory State
            currentIPNState.Clear();

            // 2. Clear all TextBoxes
            txtSearchIPN.Clear();
            txtbQTY.Clear();
            txtbMFPN.Clear();
            // Clear any other specific info boxes you might have (e.g., lblMFPNInfo)

            // 3. Reset DataGridViews
            // Safety check: if bound to DataSource, set to null first
            dgwAVL.DataSource = null;
            dgwAVL.Rows.Clear();

            dgvStockMovements.DataSource = null;
            dgvStockMovements.Rows.Clear();

            dgwINSTOCK.DataSource = null;
            dgwINSTOCK.Rows.Clear();

            // 4. Reset Status Labels
            lblBalance.Text = "0 / 0";
            lblBalance.ForeColor = Color.White;

            // 5. Final UI Action
            Log("Session reset. Ready for next IPN scan.", Color.Gray);
            txtSearchIPN.Focus();
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
}