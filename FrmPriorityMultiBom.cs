using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WH_Panel.FrmPriorityBom;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using System.Data.SqlClient;

namespace WH_Panel
{

    public partial class FrmPriorityMultiBom : Form
    {
        private AppSettings settings;
        List<Warehouse> loadedWareHouses = new List<Warehouse>();
        private Dictionary<string, ListSortDirection> columnSortDirections = new Dictionary<string, ListSortDirection>();

        public FrmPriorityMultiBom()
        {
            InitializeComponent();
            SetDarkModeColors(this);

            settings = SettingsManager.LoadSettings();
            if (settings == null)
            {
                //MessageBox.Show("Failed to load settings.");
                txtbLog.ForeColor = Color.Red;
                txtbLog.AppendText("Failed to load settings.");
                txtbLog.ScrollToCaret();
                return;
            }
            PopulateWarehouses();
            InitializeDataGridView();
            //GetGetRobWosList();
            //chkbBomsList.DrawMode = DrawMode.OwnerDrawFixed;
            //chkbBomsList.DrawItem += chkbBomsList_DrawItem;
        }

        private void InitializeDataGridView()
        {
            dgvBomsList.Columns.Clear();
            dgvBomsList.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Selected", HeaderText = "Selected" });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "SerialName", HeaderText = "Serial Name" });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "PartName", HeaderText = "Part Name" });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "SerialStatusDes", HeaderText = "Status" });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quant", HeaderText = "Quantity" });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "RevNum", HeaderText = "Revision" });

            dgvBomsList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBomsList.AllowUserToAddRows = false;
            dgvBomsList.AllowUserToDeleteRows = false;
            dgvBomsList.ReadOnly = false;
            dgvBomsList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBomsList.MultiSelect = false;

            // Handle the ColumnHeaderMouseClick event for sorting
            dgvBomsList.ColumnHeaderMouseClick += dgvBomsList_ColumnHeaderMouseClick;
            // Handle the CellValueChanged and CurrentCellDirtyStateChanged events for updating the selected label
            dgvBomsList.CellValueChanged += dgvBomsList_CellValueChanged;
            dgvBomsList.CurrentCellDirtyStateChanged += dgvBomsList_CurrentCellDirtyStateChanged;
        }
        private void dgvBomsList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvBomsList.Columns[e.ColumnIndex].Name == "Selected")
            {
                UpdateSelectedLabel();
            }
        }

        private void dgvBomsList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvBomsList.IsCurrentCellDirty)
            {
                dgvBomsList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }



        private void dgvBomsList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = dgvBomsList.Columns[e.ColumnIndex].Name;

            if (columnName != "Selected")
            {
                ListSortDirection sortDirection;
                if (columnSortDirections.ContainsKey(columnName))
                {
                    sortDirection = columnSortDirections[columnName] == ListSortDirection.Ascending
                        ? ListSortDirection.Descending
                        : ListSortDirection.Ascending;
                }
                else
                {
                    sortDirection = ListSortDirection.Ascending;
                }

                dgvBomsList.Sort(dgvBomsList.Columns[e.ColumnIndex], sortDirection);
                columnSortDirections[columnName] = sortDirection;
            }
        }






        private async void GetGetRobWosList(string warehouseName, string warehouseDesc)
        {
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=PARTNAME eq '{warehouseName}*'";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var serials = apiResponse["value"].ToObject<List<Serial>>();

                    dgvBomsList.Rows.Clear();
                    if (serials.Count == 0)
                    {
                        AppendLogMessage($"No data found for {warehouseName} - {warehouseDesc} \n", Color.Red);
                        return;
                    }
                    else
                    {
                        foreach (var serial in serials)
                        {
                            int rowIndex = dgvBomsList.Rows.Add(false, serial.SERIALNAME, serial.PARTNAME, serial.SERIALSTATUSDES, serial.QUANT, serial.REVNUM);
                            if (serial.SERIALSTATUSDES != "נסגרה")
                            {
                                dgvBomsList.Rows[rowIndex].Cells["Selected"].Value = true;
                            }
                        }

                        lblLoading.BackColor = Color.Green;
                        lblLoading.Text = "Data Loaded";
                        UpdateSelectedLabel();

                        AppendLogMessage($"{serials.Count} Work Orders loaded for {warehouseName} - {warehouseDesc} \n", Color.Green);
                    }
                }
                catch (HttpRequestException ex)
                {
                    AppendLogMessage($"Request error: {ex.Message} \n", Color.Red);
                }
                catch (Exception ex)
                {
                    AppendLogMessage($"Request error: {ex.Message}\n", Color.Red);
                }
            }
        }




        private void AppendLogMessage(string message, Color color)
        {
            txtbLog.SelectionStart = txtbLog.TextLength;
            txtbLog.SelectionLength = 0;
            txtbLog.SelectionColor = color;
            txtbLog.AppendText(message);
            txtbLog.SelectionColor = txtbLog.ForeColor; // Reset the color to default
            txtbLog.ScrollToCaret();
        }


        private async void PopulateWarehouses()
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
                        cmbWarehouses.Items.Clear();
                        loadedWareHouses.Clear(); // Clear the list before adding new items
                        foreach (var warehouse in apiResponse.value)
                        {
                            cmbWarehouses.Items.Add($"{warehouse.WARHSNAME} - {warehouse.WARHSDES}");
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
            cmbWarehouses.DroppedDown = true; // Open the dropdown list
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
                else if (control is CheckedListBox checkedListBox)
                {
                    checkedListBox.BackColor = backgroundColor;
                    checkedListBox.ForeColor = foregroundColor;
                    checkedListBox.DrawMode = DrawMode.OwnerDrawFixed;
                    checkedListBox.DrawItem += CheckedListBox_DrawItem;
                }
                // Recursively update controls within containers
                if (control.Controls.Count > 0)
                {
                    SetDarkModeColors(control);
                }
            }
        }


        private void CheckedListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var checkedListBox = sender as CheckedListBox;
            var serial = checkedListBox.Items[e.Index] as Serial;
            if (serial == null) return;

            // Set the background color based on the SERIALSTATUSDES
            Color backgroundColor;
            Color textColor = Color.Black;

            switch (serial.SERIALSTATUSDES)
            {
                case "קיט מלא":
                    backgroundColor = Color.Green;
                    textColor = Color.White;
                    break;
                case "שוחררה":
                    backgroundColor = Color.Orange;
                    textColor = Color.Black;
                    break;
                case "נסגרה":
                    backgroundColor = Color.Black;
                    textColor = Color.Gray;
                    break;
                case "הוקפאה":
                    backgroundColor = Color.Red;
                    textColor = Color.White;
                    break;
                default:
                    backgroundColor = checkedListBox.BackColor;
                    textColor = checkedListBox.ForeColor;
                    break;
            }

            // Draw the background
            e.Graphics.FillRectangle(new SolidBrush(backgroundColor), e.Bounds);

            // Draw the text
            TextRenderer.DrawText(e.Graphics, serial.ToString(), e.Font, e.Bounds, textColor, TextFormatFlags.Left);

            // Draw the focus rectangle if the item has focus
            e.DrawFocusRectangle();
        }
        private void cmbWarehouses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWarehouses.SelectedIndex >= 0)
            {
                // Get the selected warehouse name
                string selectedWarehouse = loadedWareHouses[cmbWarehouses.SelectedIndex].WARHSNAME;
                string selectedWarehouseDesc = loadedWareHouses[cmbWarehouses.SelectedIndex].WARHSDES;
                // Retrieve the work orders for the selected warehouse
                GetGetRobWosList(selectedWarehouse, selectedWarehouseDesc);
            }
            else
            {
                txtbLog.ForeColor = Color.Red;
                txtbLog.AppendText("Please select a warehouse.");
            }
        }




        private void UpdateSelectedLabel()
        {
            int selectedCount = dgvBomsList.Rows.Cast<DataGridViewRow>()
                .Count(row => Convert.ToBoolean(row.Cells["Selected"].Value));
            int totalCount = dgvBomsList.Rows.Count;
            lblSelected.Text = $"Selected {selectedCount} of {totalCount}";
        }




        //private async void btnSim1_Click(object sender, EventArgs e)
        //{
        //    var selectedWorkOrders = dgvBomsList.Rows.Cast<DataGridViewRow>()
        //        .Where(row => Convert.ToBoolean(row.Cells["Selected"].Value))
        //        .Select(row => new Serial
        //        {
        //            SERIALNAME = row.Cells["SerialName"].Value.ToString(),
        //            PARTNAME = row.Cells["PartName"].Value.ToString(),
        //            SERIALSTATUSDES = row.Cells["SerialStatusDes"].Value.ToString(),
        //            QUANT = Convert.ToInt32(row.Cells["Quant"].Value),
        //            REVNUM = row.Cells["RevNum"].Value.ToString()
        //        }).ToList();

        //    var ipnBalances = new Dictionary<string, int>();

        //    foreach (var workOrder in selectedWorkOrders)
        //    {
        //        string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{workOrder.SERIALNAME}'&$expand=TRANSORDER_K_SUBFORM";
        //        using (HttpClient client = new HttpClient())
        //        {
        //            try
        //            {
        //                AppendLogMessage($"Retrieving data for {workOrder.SERIALNAME} \n", Color.Yellow);
        //                client.DefaultRequestHeaders.Accept.Clear();
        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        //                HttpResponseMessage response = await client.GetAsync(url);
        //                response.EnsureSuccessStatusCode();
        //                string responseBody = await response.Content.ReadAsStringAsync();
        //                var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
        //                var transOrders = apiResponse["value"].First["TRANSORDER_K_SUBFORM"].ToObject<List<TransOrderKSubform>>();

        //                foreach (var transOrder in transOrders)
        //                {
        //                    int balance = transOrder.QUANT - transOrder.CQUANT;
        //                    if (ipnBalances.ContainsKey(transOrder.PARTNAME))
        //                    {
        //                        ipnBalances[transOrder.PARTNAME] += balance;
        //                    }
        //                    else
        //                    {
        //                        ipnBalances[transOrder.PARTNAME] = balance;
        //                    }
        //                }

        //                AppendLogMessage($"Loaded data for {workOrder.SERIALNAME} \n", Color.Green);
        //            }
        //            catch (HttpRequestException ex)
        //            {
        //                AppendLogMessage($"Request error: {ex.Message} \n", Color.Red);
        //            }
        //            catch (Exception ex)
        //            {
        //                AppendLogMessage($"Request error: {ex.Message}\n", Color.Red);
        //            }
        //        }
        //    }

        //    // Fetch warehouse stock levels
        //    var warehouseStock = new Dictionary<string, int>();
        //    string selectedWarehouseName = GetSelectedWarehouseName();

        //    if (selectedWarehouseName != null)
        //    {
        //        string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouseName}'&$expand=WARHSBAL_SUBFORM";
        //        using (HttpClient client = new HttpClient())
        //        {
        //            try
        //            {
        //                AppendLogMessage($"Retrieving data for {selectedWarehouseName} \n", Color.Yellow);
        //                client.DefaultRequestHeaders.Accept.Clear();
        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        //                HttpResponseMessage response = await client.GetAsync(url);
        //                response.EnsureSuccessStatusCode();
        //                string responseBody = await response.Content.ReadAsStringAsync();
        //                var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
        //                var warehouseBalances = apiResponse["value"].First["WARHSBAL_SUBFORM"].ToObject<List<WarehouseBalance>>();

        //                foreach (var balance in warehouseBalances)
        //                {
        //                    warehouseStock[balance.PARTNAME] = balance.BALANCE;
        //                }
        //                AppendLogMessage($"Loaded data for {selectedWarehouseName} \n", Color.Green);
        //            }
        //            catch (HttpRequestException ex)
        //            {
        //                AppendLogMessage($"Request error: {ex.Message} \n", Color.Red);
        //            }
        //            catch (Exception ex)
        //            {
        //                AppendLogMessage($"Request error: {ex.Message}\n", Color.Red);
        //            }
        //        }
        //    }

        //    // Calculate the required values
        //    int totalUniqueIPNs = ipnBalances.Count;
        //    int sufficientIPNs = ipnBalances.Count(ipn => warehouseStock.ContainsKey(ipn.Key) && (warehouseStock[ipn.Key] + ipn.Value) >= 0);
        //    double completionPercentage = (double)sufficientIPNs / totalUniqueIPNs * 100;

        //    AppendLogMessage($"Generating HTML report \n", Color.Yellow);
        //    // Generate HTML report
        //    string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
        //    string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\MultiKitsStatusReport_{_fileTimeStamp}.html";
        //    using (StreamWriter writer = new StreamWriter(filename))
        //    {
        //        writer.WriteLine("<html style='text-align:center;background-color:gray;color:white;'>");
        //        writer.WriteLine("<head>");
        //        writer.WriteLine("<title>Multi BOM simulation Report</title>");
        //        writer.WriteLine("<style>");
        //        writer.WriteLine("table { border-collapse: collapse; width: 100%; }");
        //        writer.WriteLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
        //        writer.WriteLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
        //        writer.WriteLine(".green { background-color: green; color: white; }");
        //        writer.WriteLine(".red { background-color: indianred; color: white; }");
        //        writer.WriteLine(".header-table td { font-size: 2em; font-weight: bold; }");
        //        writer.WriteLine("</style>");
        //        writer.WriteLine("<script>");
        //        writer.WriteLine("function sortTable(n) {");
        //        writer.WriteLine("var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;");
        //        writer.WriteLine("table = document.getElementById('kitsTable');");
        //        writer.WriteLine("switching = true;");
        //        writer.WriteLine("dir = 'asc';");
        //        writer.WriteLine("while (switching) {");
        //        writer.WriteLine("switching = false;");
        //        writer.WriteLine("rows = table.rows;");
        //        writer.WriteLine("for (i = 1; i < (rows.length - 1); i++) {");
        //        writer.WriteLine("shouldSwitch = false;");
        //        writer.WriteLine("x = rows[i].getElementsByTagName('TD')[n];");
        //        writer.WriteLine("y = rows[i + 1].getElementsByTagName('TD')[n];");
        //        writer.WriteLine("if (dir == 'asc') {");
        //        writer.WriteLine("if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {");
        //        writer.WriteLine("shouldSwitch = true;");
        //        writer.WriteLine("break;");
        //        writer.WriteLine("}");
        //        writer.WriteLine("} else if (dir == 'desc') {");
        //        writer.WriteLine("if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {");
        //        writer.WriteLine("shouldSwitch = true;");
        //        writer.WriteLine("break;");
        //        writer.WriteLine("}");
        //        writer.WriteLine("}");
        //        writer.WriteLine("}");
        //        writer.WriteLine("if (shouldSwitch) {");
        //        writer.WriteLine("rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);");
        //        writer.WriteLine("switching = true;");
        //        writer.WriteLine("switchcount++;");
        //        writer.WriteLine("} else {");
        //        writer.WriteLine("if (switchcount == 0 && dir == 'asc') {");
        //        writer.WriteLine("dir = 'desc';");
        //        writer.WriteLine("switching = true;");
        //        writer.WriteLine("}");
        //        writer.WriteLine("}");
        //        writer.WriteLine("}");
        //        writer.WriteLine("}");
        //        writer.WriteLine("</script>");
        //        writer.WriteLine("</head>");
        //        writer.WriteLine("<body>");
        //        writer.WriteLine($"<h1>Multiple Kits Simulation Report {_fileTimeStamp}</h1>");

        //        foreach (var item in selectedWorkOrders)
        //        {
        //            writer.WriteLine($"<h2>{item.SERIALNAME} - {item.PARTNAME} - {item.QUANT}PCS - {item.SERIALSTATUSDES}</h2>");
        //        }
        //        writer.WriteLine($"<h2>Unique IPNs: {sufficientIPNs} / {totalUniqueIPNs} ({completionPercentage:F2}%)</h2>");

        //        writer.WriteLine("<table id='kitsTable'>");
        //        writer.WriteLine("<tr>");
        //        writer.WriteLine("<th onclick='sortTable(0)'>IPN</th>");
        //        writer.WriteLine("<th onclick='sortTable(1)'>Balance</th>");
        //        writer.WriteLine("<th onclick='sortTable(2)'>Stock</th>");
        //        writer.WriteLine("<th onclick='sortTable(3)'>Simulation</th>");
        //        writer.WriteLine("</tr>");

        //        foreach (var ipn in ipnBalances.Keys)
        //        {
        //            int balance = ipnBalances[ipn];
        //            int stock = warehouseStock.ContainsKey(ipn) ? warehouseStock[ipn] : 0;
        //            int simulation = stock + balance;
        //            string rowClass = simulation >= 0 ? "green" : "red";

        //            writer.WriteLine($"<tr class='{rowClass}'><td>{ipn}</td><td>{balance}</td><td>{stock}</td><td>{simulation}</td></tr>");
        //        }

        //        writer.WriteLine("</table>");

        //        // Add the summary row


        //        writer.WriteLine("</body>");
        //        writer.WriteLine("</html>");
        //    }

        //    // Open the file in default browser
        //    var p = new Process();
        //    p.StartInfo = new ProcessStartInfo(filename)
        //    {
        //        UseShellExecute = true
        //    };
        //    p.Start();
        //}

        //private async void btnSim1_Click(object sender, EventArgs e)
        //{
        //    var selectedWorkOrders = dgvBomsList.Rows.Cast<DataGridViewRow>()
        //        .Where(row => Convert.ToBoolean(row.Cells["Selected"].Value))
        //        .Select(row => new Serial
        //        {
        //            SERIALNAME = row.Cells["SerialName"].Value.ToString(),
        //            PARTNAME = row.Cells["PartName"].Value.ToString(),
        //            SERIALSTATUSDES = row.Cells["SerialStatusDes"].Value.ToString(),
        //            QUANT = Convert.ToInt32(row.Cells["Quant"].Value),
        //            REVNUM = row.Cells["RevNum"].Value.ToString()
        //        }).ToList();

        //    var ipnBalances = new Dictionary<string, int>();

        //    foreach (var workOrder in selectedWorkOrders)
        //    {
        //        string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{workOrder.SERIALNAME}'&$expand=TRANSORDER_K_SUBFORM";
        //        using (HttpClient client = new HttpClient())
        //        {
        //            try
        //            {
        //                AppendLogMessage($"Retrieving data for {workOrder.SERIALNAME} \n", Color.Yellow);
        //                client.DefaultRequestHeaders.Accept.Clear();
        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        //                HttpResponseMessage response = await client.GetAsync(url);
        //                response.EnsureSuccessStatusCode();
        //                string responseBody = await response.Content.ReadAsStringAsync();
        //                var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
        //                var transOrders = apiResponse["value"].First["TRANSORDER_K_SUBFORM"].ToObject<List<TransOrderKSubform>>();

        //                foreach (var transOrder in transOrders)
        //                {
        //                    int balance = transOrder.QUANT - transOrder.CQUANT;
        //                    if (ipnBalances.ContainsKey(transOrder.PARTNAME))
        //                    {
        //                        ipnBalances[transOrder.PARTNAME] += balance;
        //                    }
        //                    else
        //                    {
        //                        ipnBalances[transOrder.PARTNAME] = balance;
        //                    }
        //                }

        //                AppendLogMessage($"Loaded data for {workOrder.SERIALNAME} \n", Color.Green);
        //            }
        //            catch (HttpRequestException ex)
        //            {
        //                AppendLogMessage($"Request error: {ex.Message} \n", Color.Red);
        //            }
        //            catch (Exception ex)
        //            {
        //                AppendLogMessage($"Request error: {ex.Message}\n", Color.Red);
        //            }
        //        }
        //    }

        //    // Fetch warehouse stock levels
        //    var warehouseStock = new Dictionary<string, int>();
        //    string selectedWarehouseName = GetSelectedWarehouseName();

        //    if (selectedWarehouseName != null)
        //    {
        //        string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouseName}'&$expand=WARHSBAL_SUBFORM";
        //        using (HttpClient client = new HttpClient())
        //        {
        //            try
        //            {
        //                AppendLogMessage($"Retrieving data for {selectedWarehouseName} \n", Color.Yellow);
        //                client.DefaultRequestHeaders.Accept.Clear();
        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        //                HttpResponseMessage response = await client.GetAsync(url);
        //                response.EnsureSuccessStatusCode();
        //                string responseBody = await response.Content.ReadAsStringAsync();
        //                var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
        //                var warehouseBalances = apiResponse["value"].First["WARHSBAL_SUBFORM"].ToObject<List<WarehouseBalance>>();

        //                foreach (var balance in warehouseBalances)
        //                {
        //                    warehouseStock[balance.PARTNAME] = balance.BALANCE;
        //                }
        //                AppendLogMessage($"Loaded data for {selectedWarehouseName} \n", Color.Green);
        //            }
        //            catch (HttpRequestException ex)
        //            {
        //                AppendLogMessage($"Request error: {ex.Message} \n", Color.Red);
        //            }
        //            catch (Exception ex)
        //            {
        //                AppendLogMessage($"Request error: {ex.Message}\n", Color.Red);
        //            }
        //        }
        //    }

        //    // Calculate the required values
        //    int totalUniqueIPNs = ipnBalances.Count;
        //    int sufficientIPNs = ipnBalances.Count(ipn => warehouseStock.ContainsKey(ipn.Key) && (warehouseStock[ipn.Key] + ipn.Value) >= 0);
        //    double completionPercentage = (double)sufficientIPNs / totalUniqueIPNs * 100;

        //    AppendLogMessage($"Generating HTML report \n", Color.Yellow);
        //    // Generate HTML report
        //    string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
        //    string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\MultiKitsStatusReport_{_fileTimeStamp}.html";
        //    using (StreamWriter writer = new StreamWriter(filename))
        //    {
        //        writer.WriteLine("<html style='text-align:center;background-color:gray;color:white;'>");
        //        writer.WriteLine("<head>");
        //        writer.WriteLine("<title>Multi BOM simulation Report</title>");
        //        writer.WriteLine("<style>");
        //        writer.WriteLine("table { border-collapse: collapse; width: 100%; }");
        //        writer.WriteLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
        //        writer.WriteLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
        //        writer.WriteLine(".green { background-color: green; color: white; }");
        //        writer.WriteLine(".red { background-color: indianred; color: white; }");
        //        writer.WriteLine(".red-balance { background-color: indianred; color: white; }");
        //        writer.WriteLine(".header-table td { font-size: 2em; font-weight: bold; }");
        //        writer.WriteLine("</style>");
        //        writer.WriteLine("<script>");
        //        writer.WriteLine("function sortTable(n) {");
        //        writer.WriteLine("var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;");
        //        writer.WriteLine("table = document.getElementById('kitsTable');");
        //        writer.WriteLine("switching = true;");
        //        writer.WriteLine("dir = 'asc';");
        //        writer.WriteLine("while (switching) {");
        //        writer.WriteLine("switching = false;");
        //        writer.WriteLine("rows = table.rows;");
        //        writer.WriteLine("for (i = 1; i < (rows.length - 1); i++) {");
        //        writer.WriteLine("shouldSwitch = false;");
        //        writer.WriteLine("x = rows[i].getElementsByTagName('TD')[n];");
        //        writer.WriteLine("y = rows[i + 1].getElementsByTagName('TD')[n];");
        //        writer.WriteLine("if (dir == 'asc') {");
        //        writer.WriteLine("if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {");
        //        writer.WriteLine("shouldSwitch = true;");
        //        writer.WriteLine("break;");
        //        writer.WriteLine("}");
        //        writer.WriteLine("} else if (dir == 'desc') {");
        //        writer.WriteLine("if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {");
        //        writer.WriteLine("shouldSwitch = true;");
        //        writer.WriteLine("break;");
        //        writer.WriteLine("}");
        //        writer.WriteLine("}");
        //        writer.WriteLine("}");
        //        writer.WriteLine("if (shouldSwitch) {");
        //        writer.WriteLine("rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);");
        //        writer.WriteLine("switching = true;");
        //        writer.WriteLine("switchcount++;");
        //        writer.WriteLine("} else {");
        //        writer.WriteLine("if (switchcount == 0 && dir == 'asc') {");
        //        writer.WriteLine("dir = 'desc';");
        //        writer.WriteLine("switching = true;");
        //        writer.WriteLine("}");
        //        writer.WriteLine("}");
        //        writer.WriteLine("}");
        //        writer.WriteLine("}");
        //        writer.WriteLine("</script>");
        //        writer.WriteLine("</head>");
        //        writer.WriteLine("<body>");
        //        writer.WriteLine($"<h1>Multiple Kits Simulation Report {_fileTimeStamp}</h1>");

        //        foreach (var item in selectedWorkOrders)
        //        {
        //            writer.WriteLine($"<h2>{item.SERIALNAME} - {item.PARTNAME} - {item.QUANT}PCS - {item.SERIALSTATUSDES}</h2>");
        //        }
        //        writer.WriteLine($"<h2>Unique IPNs: {sufficientIPNs} / {totalUniqueIPNs} ({completionPercentage:F2}%)</h2>");

        //        writer.WriteLine("<table id='kitsTable'>");
        //        writer.WriteLine("<tr>");
        //        writer.WriteLine("<th onclick='sortTable(0)'>IPN</th>");
        //        writer.WriteLine("<th onclick='sortTable(1)'>Balance</th>");
        //        writer.WriteLine("<th onclick='sortTable(2)'>Stock</th>");
        //        writer.WriteLine("<th onclick='sortTable(3)'>Simulation</th>");
        //        writer.WriteLine("</tr>");

        //        foreach (var ipn in ipnBalances.Keys)
        //        {
        //            int balance = ipnBalances[ipn];
        //            int stock = warehouseStock.ContainsKey(ipn) ? warehouseStock[ipn] : 0;
        //            int simulation = stock + balance;
        //            string rowClass = simulation >= 0 ? "green" : "red";
        //            string balanceClass = balance < 0 ? "red-balance" : "";

        //            writer.WriteLine($"<tr class='{rowClass}'>");
        //            writer.WriteLine($"<td>{ipn}</td>");
        //            writer.WriteLine($"<td class='{balanceClass}'>{balance}</td>");
        //            writer.WriteLine($"<td>{stock}</td>");
        //            writer.WriteLine($"<td>{simulation}</td>");
        //            writer.WriteLine("</tr>");
        //        }

        //        writer.WriteLine("</table>");

        //        writer.WriteLine("</body>");
        //        writer.WriteLine("</html>");
        //    }

        //    // Open the file in default browser
        //    var p = new Process();
        //    p.StartInfo = new ProcessStartInfo(filename)
        //    {
        //        UseShellExecute = true
        //    };
        //    p.Start();
        //}

        private async void btnSim1_Click(object sender, EventArgs e)
        {
            var selectedWorkOrders = dgvBomsList.Rows.Cast<DataGridViewRow>()
                .Where(row => Convert.ToBoolean(row.Cells["Selected"].Value))
                .Select(row => new Serial
                {
                    SERIALNAME = row.Cells["SerialName"].Value.ToString(),
                    PARTNAME = row.Cells["PartName"].Value.ToString(),
                    SERIALSTATUSDES = row.Cells["SerialStatusDes"].Value.ToString(),
                    QUANT = Convert.ToInt32(row.Cells["Quant"].Value),
                    REVNUM = row.Cells["RevNum"].Value.ToString()
                }).ToList();

            var tableData = await AggregatedSim(selectedWorkOrders);

            // Calculate the completion percentage
            int totalUniqueIPNs = tableData.Count;
            int sufficientIPNs = tableData.Count(ipn => ipn.Value.simulation >= 0);
            double completionPercentage = (double)sufficientIPNs / totalUniqueIPNs * 100;

            AppendLogMessage($"Generating HTML report \n", Color.Yellow);
            // Generate HTML report
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\MultiKitsStatusReport_{_fileTimeStamp}.html";
            GenerateHtmlReport(filename, tableData, $"Multiple Kits Simulation Report {_fileTimeStamp}", selectedWorkOrders, completionPercentage);

            // Open the file in default browser
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            p.Start();
        }



        private string GetSelectedWarehouseName()
        {
            if (cmbWarehouses.SelectedIndex >= 0)
            {
                string selectedItem = cmbWarehouses.SelectedItem.ToString();
                return selectedItem.Split('-')[0].Trim();
            }
            return null;
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            bool allChecked = dgvBomsList.Rows.Cast<DataGridViewRow>()
                .Where(row => row.Cells["SerialStatusDes"].Value.ToString() != "נסגרה")
                .All(row => Convert.ToBoolean(row.Cells["Selected"].Value));

            foreach (DataGridViewRow row in dgvBomsList.Rows)
            {
                if (row.Cells["SerialStatusDes"].Value.ToString() != "נסגרה")
                {
                    row.Cells["Selected"].Value = !allChecked;
                }
            }

            UpdateSelectedLabel();
        }

        private void btnFull_Click(object sender, EventArgs e)
        {
            bool allChecked = dgvBomsList.Rows.Cast<DataGridViewRow>()
                .Where(row => row.Cells["SerialStatusDes"].Value.ToString() == "קיט מלא")
                .All(row => Convert.ToBoolean(row.Cells["Selected"].Value));

            foreach (DataGridViewRow row in dgvBomsList.Rows)
            {
                if (row.Cells["SerialStatusDes"].Value.ToString() == "קיט מלא")
                {
                    row.Cells["Selected"].Value = !allChecked;
                }
            }

            UpdateSelectedLabel();
        }

        private void btnReleased_Click(object sender, EventArgs e)
        {
            bool allChecked = dgvBomsList.Rows.Cast<DataGridViewRow>()
              .Where(row => row.Cells["SerialStatusDes"].Value.ToString() == "שוחררה")
              .All(row => Convert.ToBoolean(row.Cells["Selected"].Value));

            foreach (DataGridViewRow row in dgvBomsList.Rows)
            {
                if (row.Cells["SerialStatusDes"].Value.ToString() == "שוחררה")
                {
                    row.Cells["Selected"].Value = !allChecked;
                }
            }

            UpdateSelectedLabel();
        }

        private void btnPartialAssy_Click(object sender, EventArgs e)
        {
            bool allChecked = dgvBomsList.Rows.Cast<DataGridViewRow>()
        .Where(row => row.Cells["SerialStatusDes"].Value.ToString() == "הרכבה בחוסר")
        .All(row => Convert.ToBoolean(row.Cells["Selected"].Value));

            foreach (DataGridViewRow row in dgvBomsList.Rows)
            {
                if (row.Cells["SerialStatusDes"].Value.ToString() == "הרכבה בחוסר")
                {
                    row.Cells["Selected"].Value = !allChecked;
                }
            }

            UpdateSelectedLabel();
        }

        private async Task<Dictionary<string, (int balance, int stock, int simulation)>> AggregatedSim(List<Serial> selectedWorkOrders)
        {
            var ipnBalances = new Dictionary<string, int>();

            foreach (var workOrder in selectedWorkOrders)
            {
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{workOrder.SERIALNAME}'&$expand=TRANSORDER_K_SUBFORM";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        AppendLogMessage($"Retrieving data for {workOrder.SERIALNAME} \n", Color.Yellow);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                        var transOrders = apiResponse["value"].First["TRANSORDER_K_SUBFORM"].ToObject<List<TransOrderKSubform>>();

                        foreach (var transOrder in transOrders)
                        {
                            int balance = transOrder.QUANT - transOrder.CQUANT;
                            if (ipnBalances.ContainsKey(transOrder.PARTNAME))
                            {
                                ipnBalances[transOrder.PARTNAME] += balance;
                            }
                            else
                            {
                                ipnBalances[transOrder.PARTNAME] = balance;
                            }
                        }

                        AppendLogMessage($"Loaded data for {workOrder.SERIALNAME} \n", Color.Green);
                    }
                    catch (HttpRequestException ex)
                    {
                        AppendLogMessage($"Request error: {ex.Message} \n", Color.Red);
                    }
                    catch (Exception ex)
                    {
                        AppendLogMessage($"Request error: {ex.Message}\n", Color.Red);
                    }
                }
            }

            // Fetch warehouse stock levels
            var warehouseStock = new Dictionary<string, int>();
            string selectedWarehouseName = GetSelectedWarehouseName();

            if (selectedWarehouseName != null)
            {
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouseName}'&$expand=WARHSBAL_SUBFORM";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        AppendLogMessage($"Retrieving data for {selectedWarehouseName} \n", Color.Yellow);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                        var warehouseBalances = apiResponse["value"].First["WARHSBAL_SUBFORM"].ToObject<List<WarehouseBalance>>();

                        foreach (var balance in warehouseBalances)
                        {
                            warehouseStock[balance.PARTNAME] = balance.BALANCE;
                        }
                        AppendLogMessage($"Loaded data for {selectedWarehouseName} \n", Color.Green);
                    }
                    catch (HttpRequestException ex)
                    {
                        AppendLogMessage($"Request error: {ex.Message} \n", Color.Red);
                    }
                    catch (Exception ex)
                    {
                        AppendLogMessage($"Request error: {ex.Message}\n", Color.Red);
                    }
                }
            }

            // Calculate the required values
            var result = new Dictionary<string, (int balance, int stock, int simulation)>();
            foreach (var ipn in ipnBalances.Keys)
            {
                int balance = ipnBalances[ipn];
                int stock = warehouseStock.ContainsKey(ipn) ? warehouseStock[ipn] : 0;
                int simulation = stock + balance;
                result[ipn] = (balance, stock, simulation);
            }

            return result;
        }
        private async Task<Dictionary<string, List<Serial>>> SimByIPN(List<Serial> selectedWorkOrders)
        {
            // Placeholder for SimByIPN logic
            // This function should return a dictionary where the key is the IPN and the value is a list of Serial objects that require that IPN
            return new Dictionary<string, List<Serial>>();
        }

        private async Task<Dictionary<string, List<Serial>>> SimByBoms(List<Serial> selectedWorkOrders)
        {
            // Placeholder for SimByBoms logic
            // This function should return a dictionary where the key is the BOM and the value is a list of Serial objects that are part of that BOM
            return new Dictionary<string, List<Serial>>();
        }


        private void GenerateHtmlReport(string filename, Dictionary<string, (int balance, int stock, int simulation)> tableData, string reportTitle, List<Serial> selectedWorkOrders, double completionPercentage)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='text-align:center;background-color:gray;color:white;'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>Multi BOM simulation Report</title>");
                writer.WriteLine("<style>");
                writer.WriteLine("table { border-collapse: collapse; width: 100%; }");
                writer.WriteLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
                writer.WriteLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
                writer.WriteLine(".green { background-color: green; color: white; }");
                writer.WriteLine(".red { background-color: indianred; color: white; }");
                writer.WriteLine(".red-balance { background-color: indianred; color: white; }");
                writer.WriteLine(".header-table td { font-size: 2em; font-weight: bold; }");
                writer.WriteLine("</style>");
                writer.WriteLine("<script>");
                writer.WriteLine("function sortTable(n) {");
                writer.WriteLine("var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;");
                writer.WriteLine("table = document.getElementById('kitsTable');");
                writer.WriteLine("switching = true;");
                writer.WriteLine("dir = 'asc';");
                writer.WriteLine("while (switching) {");
                writer.WriteLine("switching = false;");
                writer.WriteLine("rows = table.rows;");
                writer.WriteLine("for (i = 1; i < (rows.length - 1); i++) {");
                writer.WriteLine("shouldSwitch = false;");
                writer.WriteLine("x = rows[i].getElementsByTagName('TD')[n];");
                writer.WriteLine("y = rows[i + 1].getElementsByTagName('TD')[n];");
                writer.WriteLine("if (dir == 'asc') {");
                writer.WriteLine("if (n > 0 && n < 4) {"); // Numeric columns
                writer.WriteLine("if (parseFloat(x.innerHTML) > parseFloat(y.innerHTML)) {");
                writer.WriteLine("shouldSwitch = true;");
                writer.WriteLine("break;");
                writer.WriteLine("}");
                writer.WriteLine("} else {"); // String columns
                writer.WriteLine("if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {");
                writer.WriteLine("shouldSwitch = true;");
                writer.WriteLine("break;");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("} else if (dir == 'desc') {");
                writer.WriteLine("if (n > 0 && n < 4) {"); // Numeric columns
                writer.WriteLine("if (parseFloat(x.innerHTML) < parseFloat(y.innerHTML)) {");
                writer.WriteLine("shouldSwitch = true;");
                writer.WriteLine("break;");
                writer.WriteLine("}");
                writer.WriteLine("} else {"); // String columns
                writer.WriteLine("if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {");
                writer.WriteLine("shouldSwitch = true;");
                writer.WriteLine("break;");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("if (shouldSwitch) {");
                writer.WriteLine("rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);");
                writer.WriteLine("switching = true;");
                writer.WriteLine("switchcount++;");
                writer.WriteLine("} else {");
                writer.WriteLine("if (switchcount == 0 && dir == 'asc') {");
                writer.WriteLine("dir = 'desc';");
                writer.WriteLine("switching = true;");
                writer.WriteLine("} else if (switchcount == 0 && dir == 'desc') {");
                writer.WriteLine("dir = 'asc';");
                writer.WriteLine("switching = true;");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine($"<h1>{reportTitle}</h1>");

                foreach (var item in selectedWorkOrders)
                {
                    writer.WriteLine($"<h2>{item.SERIALNAME} - {item.PARTNAME} - {item.QUANT}PCS - {item.SERIALSTATUSDES}</h2>");
                }
                writer.WriteLine($"<h2>Unique IPNs: {tableData.Count} / {tableData.Count} ({completionPercentage:F2}%)</h2>");

                writer.WriteLine("<table id='kitsTable'>");
                writer.WriteLine("<tr>");
                writer.WriteLine("<th onclick='sortTable(0)'>IPN</th>");
                writer.WriteLine("<th onclick='sortTable(1)'>Balance</th>");
                writer.WriteLine("<th onclick='sortTable(2)'>Stock</th>");
                writer.WriteLine("<th onclick='sortTable(3)'>Simulation</th>");
                writer.WriteLine("</tr>");

                foreach (var ipn in tableData.Keys)
                {
                    var (balance, stock, simulation) = tableData[ipn];
                    string rowClass = simulation >= 0 ? "green" : "red";
                    string balanceClass = balance < 0 ? "red-balance" : "";

                    writer.WriteLine($"<tr class='{rowClass}'>");
                    writer.WriteLine($"<td>{ipn}</td>");
                    writer.WriteLine($"<td class='{balanceClass}'>{balance}</td>");
                    writer.WriteLine($"<td>{stock}</td>");
                    writer.WriteLine($"<td>{simulation}</td>");
                    writer.WriteLine("</tr>");
                }

                writer.WriteLine("</table>");

                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
        }


    }
}



