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
            InitializeMovementsGrid();
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

                        //foreach (var trans in movements)
                        //{
                        //    // Adding rows to your live display grid
                        //    // We leave Date and Pack empty for the Parallel Enrichment to fill
                        //    dgvStockMovements.Rows.Add("", trans.LOGDOCNO, trans.DOCDES, trans.SUPCUSTNAME, "", trans.TQUANT, "");
                        //}
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



        //private void PopulateInStockByLogic()
        //{
        //    // Ensure the IN STOCK grid has a skeleton
        //    if (dgwINSTOCK.Columns.Count == 0)
        //    {
        //        dgwINSTOCK.Columns.Add("UDATE", "Date");
        //        dgwINSTOCK.Columns.Add("LOGDOCNO", "Doc No");
        //        dgwINSTOCK.Columns.Add("BOOKNUM", "Client Doc");
        //        dgwINSTOCK.Columns.Add("TQUANT", "Qty");
        //        dgwINSTOCK.Columns.Add("SUPCUSTNAME", "Source");
        //        dgwINSTOCK.Columns.Add("PACKNAME", "Pack Code");
        //        dgwINSTOCK.Columns.Add("CountDate", "CountDate");
        //        dgwINSTOCK.Columns.Add("UserCounted", "UserCounted");
        //        ApplyDarkThemeToGrid(dgwINSTOCK);
        //    }

        //    dgwINSTOCK.Rows.Clear();
        //    dgwINSTOCK.Rows.Clear();

        //    // 1. Get all movements from your grid
        //    var rows = dgvStockMovements.Rows.Cast<DataGridViewRow>()
        //        .Where(r => r.Cells["TQUANT"].Value != null)
        //        .Select(r => new {
        //            DocNo = r.Cells["LOGDOCNO"].Value.ToString(),
        //            Qty = Math.Abs(Convert.ToDecimal(r.Cells["TQUANT"].Value)),
        //            Date = DateTime.TryParse(r.Cells["UDATE"].Value?.ToString(), out var d) ? d : DateTime.MinValue,
        //            Pack = r.Cells["PACKNAME"].Value?.ToString(),
        //            Supplier = r.Cells["SUPCUSTNAME"].Value?.ToString(),
        //            BookNum = r.Cells["BOOKNUM"].Value?.ToString()
        //        }).ToList();

        //    // 2. Separate into In and Out
        //    var incoming = rows.Where(r => r.DocNo.StartsWith("GR")).OrderBy(r => r.Date).ToList();
        //    var outgoing = rows.Where(r => r.DocNo.StartsWith("ROB") || r.DocNo.StartsWith("SH")).OrderBy(r => r.Date).ToList();

        //    // 3. Match and Remove
        //    // Note: We use a list we can modify
        //    var remainingInStock = incoming.ToList();

        //    foreach (var outMove in outgoing)
        //    {
        //        // Find the oldest 'In' that matches this quantity exactly
        //        var match = remainingInStock.FirstOrDefault(i => i.Qty == outMove.Qty);
        //        if (match != null)
        //        {
        //            remainingInStock.Remove(match); // It's been issued, remove from "In Stock"
        //        }
        //    }

        //    int countedTotalStr = 0;
        //    // 4. Populate dgwINSTOCK with what's left
        //    foreach (var item in remainingInStock)
        //    {
        //        dgwINSTOCK.Rows.Add(item.Date, item.DocNo,item.BookNum, item.Qty, item.Supplier, item.Pack);
        //        countedTotalStr += (int)item.Qty;
        //    }
        //    lblBalance.Text = $"0/{countedTotalStr}";

        //    Log($"Heuristic reconciliation: {remainingInStock.Count} reels likely in stock.", Color.Yellow);
        //}


        private void PopulateInStockByLogic()
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
                // Status columns for later SQL sync
                dgwINSTOCK.Columns.Add("CountDate", "Count Date");
                dgwINSTOCK.Columns.Add("UserCounted", "User");
                ApplyDarkThemeToGrid(dgwINSTOCK);
            }

            dgwINSTOCK.Rows.Clear();

            // 2. Extract and sanitize movements from the main grid
            var rows = dgvStockMovements.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["TQUANT"].Value != null && !string.IsNullOrEmpty(r.Cells["TQUANT"].Value.ToString()))
                .Select(r => new
                {
                    DocNo = r.Cells["LOGDOCNO"].Value?.ToString() ?? "",
                    // Use decimal.TryParse for safety; Priority quantities are treated as absolute for matching
                    Qty = decimal.TryParse(r.Cells["TQUANT"].Value.ToString(), out decimal q) ? Math.Abs(q) : 0,
                    Date = DateTime.TryParse(r.Cells["UDATE"].Value?.ToString(), out var d) ? d : DateTime.MinValue,
                    Pack = r.Cells["PACKNAME"].Value?.ToString() ?? "N/A",
                    Supplier = r.Cells["SUPCUSTNAME"].Value?.ToString() ?? "",
                    BookNum = r.Cells["BOOKNUM"].Value?.ToString() ?? ""
                })
                .Where(x => x.Qty > 0)
                .ToList();

            // 3. Robust Reconciliation: Segregate by flow, not just prefix
            // INCOMING: Includes Goods Receipts (GR) and Warehouse Receipts (WR)
            var incoming = rows.Where(r => r.DocNo.StartsWith("GR")).ToList();

            // OUTGOING: Includes Issues (ROB), Shipping (SH), and Transfers Out (WR/SH logic)
            var outgoing = rows.Where(r => r.DocNo.StartsWith("ROB") || r.DocNo.StartsWith("SH") || r.DocNo.StartsWith("WR")).ToList();

            // 4. THE BUG WORKAROUND: Partner Matching
            // We remove pairs that match in quantity to handle incorrect timestamps
            var remainingInStock = incoming.OrderBy(r => r.Date).ToList();
            var unmatchedOut = outgoing.OrderBy(r => r.Date).ToList();

            foreach (var outMove in unmatchedOut)
            {
                // Search for a matching quantity in the 'In' bucket
                // We look for the closest date match, but allow the 'In' to be slightly later 
                // than the 'Out' to solve your WR/GR bug.
                var match = remainingInStock.FirstOrDefault(i => i.Qty == outMove.Qty);

                if (match != null)
                {
                    remainingInStock.Remove(match);
                }
            }

            // 5. Populate dgwINSTOCK and update the Balance Label
            decimal totalQtyInStock = 0;

            foreach (var item in remainingInStock)
            {
                dgwINSTOCK.Rows.Add(
                    item.Date == DateTime.MinValue ? "" : item.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    item.DocNo,
                    item.BookNum,
                    item.Qty,
                    item.Supplier,
                    item.Pack
                );
                totalQtyInStock += item.Qty;
            }

            // Update the balance label: Counted (0 for now) / Calculated Stock Total
            lblBalance.Text = $"0/{totalQtyInStock:N0}";

            Log($"Heuristic reconciliation: {remainingInStock.Count} reels identified ({totalQtyInStock:N0} total pcs).", Color.Yellow);
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

        private async void txtbQTY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (!decimal.TryParse(txtbQTY.Text.Trim(), out decimal scannedQty)) return;

                string operatorSelection = cmbPackage.Text;
                string currentIPN = txtSearchIPN.Text.Trim().ToUpper();

                // 1. Search dgwINSTOCK for a matching quantity
                var matchingRow = dgwINSTOCK.Rows.Cast<DataGridViewRow>()
                    .FirstOrDefault(r => Convert.ToDecimal(r.Cells["TQUANT"].Value) == scannedQty);

                if (matchingRow != null)
                {
                    string dbPackageType = matchingRow.Cells["PACKNAME"].Value?.ToString();

                    // 2. Scenario A: User selection doesn't match the DB record
                    if (dbPackageType != operatorSelection)
                    {
                        MessageBox.Show($"Incorrect Package Selection!\n\n" +
                                        $"The system record for Qty {scannedQty} specifies: {dbPackageType}.\n" +
                                        $"Please correct your selection in the dropdown.",
                                        "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        // Drop down the correct item for selection
                        cmbPackage.SelectedItem = dbPackageType;
                        txtbQTY.Clear();
                        txtbQTY.Focus();
                        return; // Do not save yet
                    }

                    // 3. Scenario B: User realizes Physical != DB
                    // (This is handled by the operator stopping here since they cannot 
                    // 'Verify' a mismatch without manager intervention in Priority).

                    // Get the domain and username
                    string user = WindowsIdentity.GetCurrent().Name;

                    // Capture the current UTC time for the database (best practice for SQL)
                    DateTime countTime = DateTime.UtcNow;

                    // 4. Success: Write to SQL
                    //await SaveCountToSql(matchingRow, user, countTime);

                    // UI Feedback
                    matchingRow.DefaultCellStyle.BackColor = Color.FromArgb(45, 65, 45);
                    
                    Log($"VERIFIED: {scannedQty} on {dbPackageType} saved to SQL.", Color.LimeGreen);
                    UpdateBalanceLabel();

                    txtbQTY.Clear();
                    txtbQTY.Focus();
                }
                else
                {
                    Log($"!!! UNKNOWN REEL !!! No record found with Qty: {scannedQty}.", Color.Red);
                }
            }
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