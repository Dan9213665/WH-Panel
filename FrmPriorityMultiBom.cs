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
using Button = System.Windows.Forms.Button;
using Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;
using ComboBox = System.Windows.Forms.ComboBox;
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
        }
        private void InitializeDataGridView()
        {
            dgvBomsList.Columns.Clear();
            dgvBomsList.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Selected", HeaderText = "בחירה", ReadOnly = false });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "SerialName", HeaderText = "פק\"ע" });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "PartName", HeaderText = "מק\"ט הרכבה" });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "SerialStatusDes", HeaderText = "סטטוס קיט" });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quant", HeaderText = "כמות" });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "RevNum", HeaderText = "רוויזיה" });
            dgvBomsList.Columns.Add(new DataGridViewTextBoxColumn { Name = "Priority", HeaderText = "עדיפות" });
            dgvBomsList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBomsList.AllowUserToAddRows = false;
            dgvBomsList.AllowUserToDeleteRows = false;
            dgvBomsList.ReadOnly = false; // Set the DataGridView to read-only
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
            //string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=PARTNAME eq '{warehouseName}*'";
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=PARTNAME eq '{warehouseName}*'&$select=SERIALNAME,PARTNAME,SERIALSTATUSDES,QUANT,REVNUM";

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
                    int notClosedCount = serials.Count(serial => serial.SERIALSTATUSDES != "נסגרה");
                    if (serials.Count == 0)
                    {
                        AppendLogMessage($"No work order data found for {warehouseName} - {warehouseDesc}", Color.Red);
                        return;
                    }
                    else
                    {
                        foreach (var serial in serials)
                        {
                            if (serial.SERIALSTATUSDES != "נסגרה")
                            {
                                int rowIndex = dgvBomsList.Rows.Add(false, serial.SERIALNAME, serial.PARTNAME, serial.SERIALSTATUSDES, serial.QUANT, serial.REVNUM);
                                dgvBomsList.Rows[rowIndex].Cells["Selected"].Value = false;
                            }
                        }
                        lblLoading.BackColor = Color.Green;
                        lblLoading.Text = "Data Loaded";
                        UpdateSelectedLabel();
                        AppendLogMessage($"{notClosedCount} not closed Work Orders loaded for {warehouseName} - {warehouseDesc}", Color.Green);
                        SortDataGridViewByStatus();
                    }
                }
                catch (HttpRequestException ex)
                {
                    AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                }
                catch (Exception ex)
                {
                    AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                }
            }
        }
        private void SortDataGridViewByStatus()
        {
            dgvBomsList.Sort(dgvBomsList.Columns["SerialStatusDes"], ListSortDirection.Ascending);
        }
        private void AppendLogMessage(string message, Color color)
        {
            string datestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            txtbLog.SelectionStart = txtbLog.TextLength;
            txtbLog.SelectionLength = 0;
            txtbLog.SelectionColor = color;
            txtbLog.AppendText(datestamp+"..."+message + "\n");
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
        private async void btnSim1_Click(object sender, EventArgs e)
        {
            if (dgvBomsList.Rows.Cast<DataGridViewRow>().Any(row => Convert.ToBoolean(row.Cells["Selected"].Value)))
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
                AppendLogMessage($"Generating HTML report", Color.Yellow);
                // Generate HTML report
                string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\MultiKitsStatusReport_{_fileTimeStamp}.html";
                await GenerateHTMLaggregated(filename, tableData, $"Multiple Kits Simulation Report {_fileTimeStamp}", selectedWorkOrders, completionPercentage);
           
                if (File.Exists(filename))
                {
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(filename)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                    AppendLogMessage($"Displaying HTML report in browser", Color.Green);
                }
                else
                {
                    AppendLogMessage($"Failed to create report file: {filename}", Color.Red);
                    MessageBox.Show($"Report file was not created: {filename}", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select at least one work order. \n");
            }
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
            ToggleButtonColor((Button)sender, Color.DarkGreen); // Toggle button color
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
        //private async Task<Dictionary<string, (int balance, int stock, int simulation)>> AggregatedSim(List<Serial> selectedWorkOrders)
        //{
        //    var ipnQuantities = new Dictionary<string, int>();
        //    var ipnCQuantities = new Dictionary<string, int>();
        //    // Fetch warehouse stock levels once
        //    var warehouseStock = new Dictionary<string, int>();
        //    string selectedWarehouseName = GetSelectedWarehouseName();
        //    if (selectedWarehouseName != null)
        //    {
        //        string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouseName}'&$expand=WARHSBAL_SUBFORM";
        //        using (HttpClient client = new HttpClient())
        //        {
        //            try
        //            {
        //                AppendLogMessage($"Retrieving data for {selectedWarehouseName}", Color.Yellow);
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
        //                AppendLogMessage($"Loaded data for {selectedWarehouseName}", Color.Green);
        //            }
        //            catch (HttpRequestException ex)
        //            {
        //                AppendLogMessage($"Request error: {ex.Message}", Color.Red);
        //            }
        //            catch (Exception ex)
        //            {
        //                AppendLogMessage($"Request error: {ex.Message}", Color.Red);
        //            }
        //        }
        //    }
        //    foreach (var workOrder in selectedWorkOrders)
        //    {
        //        if (workOrder.SERIALNAME.StartsWith("ROB"))
        //        {

        //            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{workOrder.SERIALNAME}'&$expand=TRANSORDER_K_SUBFORM";
        //            using (HttpClient client = new HttpClient())
        //            {
        //                try
        //                {
        //                    AppendLogMessage($"Retrieving data for {workOrder.SERIALNAME} \n", Color.Yellow);
        //                    client.DefaultRequestHeaders.Accept.Clear();
        //                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        //                    HttpResponseMessage response = await client.GetAsync(url);
        //                    response.EnsureSuccessStatusCode();
        //                    string responseBody = await response.Content.ReadAsStringAsync();
        //                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
        //                    var transOrders = apiResponse["value"].First["TRANSORDER_K_SUBFORM"].ToObject<List<TransOrderKSubform>>();
        //                    foreach (var transOrder in transOrders)
        //                    {
        //                        if (ipnQuantities.ContainsKey(transOrder.PARTNAME))
        //                        {
        //                            ipnQuantities[transOrder.PARTNAME] += transOrder.QUANT;
        //                        }
        //                        else
        //                        {
        //                            ipnQuantities[transOrder.PARTNAME] = transOrder.QUANT;


        //                        }

        //                        if (ipnCQuantities.TryGetValue(transOrder.PARTNAME, out int existingCQuant))
        //                        {
        //                            ipnCQuantities[transOrder.PARTNAME] = Math.Max(existingCQuant, transOrder.CQUANT);
        //                        }
        //                        else
        //                        {
        //                            ipnCQuantities[transOrder.PARTNAME] = transOrder.CQUANT;
        //                        }
        //                    }



        //                    AppendLogMessage($"Loaded data for {workOrder.SERIALNAME}", Color.Green);
        //                }
        //                catch (HttpRequestException ex)
        //                {
        //                    AppendLogMessage($"Request error: {ex.Message}", Color.Red);
        //                }
        //                catch (Exception ex)
        //                {
        //                    AppendLogMessage($"Request error: {ex.Message}", Color.Red);
        //                }
        //            }
        //        }
        //        else if (workOrder.SERIALNAME.StartsWith("SIM"))
        //        {
        //            // Construct the API URL for the SIM work order
        //            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PART('{workOrder.PARTNAME}')/REVISIONS_SUBFORM?$filter=REVNUM eq '{workOrder.REVNUM}'&$expand=REVPARTARC_SUBFORM";

        //            using (HttpClient client = new HttpClient())
        //            {
        //                try
        //                {
        //                    AppendLogMessage($"Retrieving BOM data for SIM work order '{workOrder.SERIALNAME}'", Color.Yellow);

        //                    // Set the request headers
        //                    client.DefaultRequestHeaders.Accept.Clear();
        //                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        //                    // Make the HTTP GET request
        //                    HttpResponseMessage response = await client.GetAsync(url);
        //                    response.EnsureSuccessStatusCode();

        //                    // Read the response content
        //                    string responseBody = await response.Content.ReadAsStringAsync();

        //                    // Parse the JSON response
        //                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
        //                    var bomItems = apiResponse["value"].First["REVPARTARC_SUBFORM"].ToObject<List<JObject>>();

        //                    // Process each BOM item
        //                    foreach (var item in bomItems)
        //                    {
        //                        string partName = item["SONNAME"].ToString();
        //                        double sonQuant = item["SONQUANT"].ToObject<double>();
        //                        int totalRequired = (int)(sonQuant * workOrder.QUANT); // Multiply SONQUANT by the Quant column value

        //                        if (ipnQuantities.ContainsKey(partName))
        //                        {
        //                            ipnQuantities[partName] -= totalRequired;
        //                        }
        //                        else
        //                        {
        //                            ipnQuantities[partName] = 0;
        //                            ipnCQuantities[partName] = totalRequired; // Initialize consumed quantity as 0
        //                        }
        //                    }

        //                    AppendLogMessage($"Loaded BOM data for SIM work order '{workOrder.SERIALNAME}'", Color.Green);
        //                }
        //                catch (HttpRequestException ex)
        //                {
        //                    AppendLogMessage($"Request error for SIM work order '{workOrder.SERIALNAME}': {ex.Message}", Color.Red);
        //                }
        //                catch (Exception ex)
        //                {
        //                    AppendLogMessage($"Error processing SIM work order '{workOrder.SERIALNAME}': {ex.Message}", Color.Red);
        //                }
        //            }
        //        }

        //    }
        //    // Calculate the balance for each PARTNAME
        //    var ipnBalances = new Dictionary<string, int>();
        //    foreach (var partName in ipnQuantities.Keys)
        //    {
        //        ipnBalances[partName] = ipnQuantities[partName] - ipnCQuantities[partName];
        //    }
        //    // Calculate the required values
        //    var result = new Dictionary<string, (int balance, int stock, int simulation)>();
        //    foreach (var ipn in ipnBalances.Keys)
        //    {
        //        int balance = ipnBalances[ipn];
        //        int stock = warehouseStock.ContainsKey(ipn) ? warehouseStock[ipn] : 0;
        //        int simulation = stock + balance;
        //        result[ipn] = (balance, stock, simulation);
        //    }
        //    return result;
        //}



        private async Task<Dictionary<string, (int kitBalance, int stock, int simulation,int inHowManyKitsUsed)>> AggregatedSim(List<Serial> selectedWorkOrders)
        {
            // For each IPN, keep a list of max CQUANT per kit and sum of QUANT per kit
            var ipnToMaxCQuantPerKit = new Dictionary<string, List<int>>();
            var ipnToTotalQuant = new Dictionary<string, int>();

            // Fetch warehouse stock levels once
            var warehouseStock = new Dictionary<string, int>();
            string selectedWarehouseName = GetSelectedWarehouseName();
            if (selectedWarehouseName != null)
            {
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouseName}'&$expand=WARHSBAL_SUBFORM";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        AppendLogMessage($"Retrieving data for {selectedWarehouseName}", Color.Yellow);
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
                        AppendLogMessage($"Loaded data for {selectedWarehouseName}", Color.Green);
                    }
                    catch (HttpRequestException ex)
                    {
                        AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                    }
                    catch (Exception ex)
                    {
                        AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                    }
                }
            }

            // For each kit (work order), collect max CQUANT and sum QUANT for each IPN
            foreach (var workOrder in selectedWorkOrders)
            {
                if (workOrder.SERIALNAME.StartsWith("ROB"))
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

                            // For this kit, track max CQUANT and sum QUANT for each IPN
                            var kitMaxCQuant = new Dictionary<string, int>();
                            var kitSumQuant = new Dictionary<string, int>();

                            foreach (var transOrder in transOrders)
                            {
                                // Max CQUANT per IPN in this kit
                                if (kitMaxCQuant.TryGetValue(transOrder.PARTNAME, out int existingCQuant))
                                    kitMaxCQuant[transOrder.PARTNAME] = (Math.Max(existingCQuant, transOrder.CQUANT));
                                else
                                    kitMaxCQuant[transOrder.PARTNAME] = (Math.Max(existingCQuant, transOrder.CQUANT));

                                // Sum QUANT per IPN in this kit
                                if (kitSumQuant.ContainsKey(transOrder.PARTNAME))
                                    kitSumQuant[transOrder.PARTNAME] += transOrder.QUANT;
                                else
                                    kitSumQuant[transOrder.PARTNAME] = transOrder.QUANT;
                            }

                            // Add this kit's max CQUANT and sum QUANT to the global dictionaries
                            foreach (var ipn in kitMaxCQuant.Keys)
                            {
                                if (!ipnToMaxCQuantPerKit.ContainsKey(ipn))
                                    ipnToMaxCQuantPerKit[ipn] = new List<int>();
                                ipnToMaxCQuantPerKit[ipn].Add((kitMaxCQuant[ipn]));
                            }
                            foreach (var ipn in kitSumQuant.Keys)
                            {
                                if (ipnToTotalQuant.ContainsKey(ipn))
                                    ipnToTotalQuant[ipn] += kitSumQuant[ipn];
                                else
                                    ipnToTotalQuant[ipn] = kitSumQuant[ipn];
                            }

                            AppendLogMessage($"Loaded data for {workOrder.SERIALNAME}", Color.Green);
                        }
                        catch (HttpRequestException ex)
                        {
                            AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                        }
                        catch (Exception ex)
                        {
                            AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                        }
                    }
                }


                else if (workOrder.SERIALNAME.StartsWith("SIM"))
                {
                    // SIM logic: fetch BOM and add required quantities
                    string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PART('{workOrder.PARTNAME}')/REVISIONS_SUBFORM?$filter=REVNUM eq '{workOrder.REVNUM}'&$expand=REVPARTARC_SUBFORM";
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            AppendLogMessage($"Retrieving BOM data for SIM work order '{workOrder.SERIALNAME}'", Color.Yellow);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                            HttpResponseMessage response = await client.GetAsync(url);
                            response.EnsureSuccessStatusCode();
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                            var bomItems = apiResponse["value"].First["REVPARTARC_SUBFORM"].ToObject<List<JObject>>();

                            foreach (var item in bomItems)
                            {
                                string partName = item["SONNAME"].ToString();
                                double sonQuant = item["SONQUANT"].ToObject<double>();
                                int totalRequired = (int)(sonQuant * workOrder.QUANT);

                                // For SIM, add to required (as if it's a kit to be simulated)
                                if (!ipnToMaxCQuantPerKit.ContainsKey(partName))
                                    ipnToMaxCQuantPerKit[partName] = new List<int>();
                                ipnToMaxCQuantPerKit[partName].Add(totalRequired);

                                // Optionally, you can also add to ipnToTotalQuant if you want to show the simulated kit as "in kits"
                                // If not, comment out the next two lines:
                                if (ipnToTotalQuant.ContainsKey(partName))
                                    ipnToTotalQuant[partName] += 0;
                                else
                                    ipnToTotalQuant[partName] = 0;
                            }
                            AppendLogMessage($"Loaded BOM data for SIM work order '{workOrder.SERIALNAME}'", Color.Green);
                        }
                        catch (HttpRequestException ex)
                        {
                            AppendLogMessage($"Request error for SIM work order '{workOrder.SERIALNAME}': {ex.Message}", Color.Red);
                        }
                        catch (Exception ex)
                        {
                            AppendLogMessage($"Error processing SIM work order '{workOrder.SERIALNAME}': {ex.Message}", Color.Red);
                        }
                    }
                }


            }

            // Now, for each IPN, calculate totals and simulation
            var result = new Dictionary<string, (int kitBalance, int stock, int simulation, int inHowManyKitsUsed)>();
            var allIpns = new HashSet<string>(ipnToMaxCQuantPerKit.Keys.Concat(ipnToTotalQuant.Keys));
            foreach (var ipn in allIpns)
            {
                int totalRequired = ipnToMaxCQuantPerKit.ContainsKey(ipn) ? ipnToMaxCQuantPerKit[ipn].Sum() : 0;
                int inHowManyKitsUsed = ipnToMaxCQuantPerKit.ContainsKey(ipn) ? ipnToMaxCQuantPerKit[ipn].Count : 0;
                int totalInKits = ipnToTotalQuant.ContainsKey(ipn) ? ipnToTotalQuant[ipn] : 0;
                int kitBalance =  totalInKits - totalRequired;
                int stock = warehouseStock.ContainsKey(ipn) ? warehouseStock[ipn] : 0;
                int simulation = stock + kitBalance;
                result[ipn] = (kitBalance, stock, simulation, inHowManyKitsUsed);
            }
            return result;
        }


        private async Task<Dictionary<string, List<(Serial serial, int quant, int cquant, int balance, int simulation)>>> SimByIPN(List<Serial> selectedWorkOrders)
        {
            var ipnToSerials = new Dictionary<string, List<(Serial serial, int quant, int cquant, int balance, int simulation)>>();
            // Fetch warehouse stock levels once
            var warehouseStock = new Dictionary<string, int>();
            string selectedWarehouseName = GetSelectedWarehouseName();
            if (selectedWarehouseName != null)
            {
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouseName}'&$expand=WARHSBAL_SUBFORM";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        AppendLogMessage($"Retrieving data for {selectedWarehouseName}", Color.Yellow);
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
                        AppendLogMessage($"Loaded data for {selectedWarehouseName}", Color.Green);
                    }
                    catch (HttpRequestException ex)
                    {
                        AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                    }
                    catch (Exception ex)
                    {
                        AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                    }
                }
            }
            foreach (var workOrder in selectedWorkOrders)
            {
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{workOrder.SERIALNAME}'&$expand=TRANSORDER_K_SUBFORM";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        AppendLogMessage($"Retrieving data for {workOrder.SERIALNAME}", Color.Yellow);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                        var transOrders = apiResponse["value"].First["TRANSORDER_K_SUBFORM"].ToObject<List<TransOrderKSubform>>();
                        var partQuantities = new Dictionary<string, int>();
                        var partCQuants = new Dictionary<string, int>();

                        //foreach (var transOrder in transOrders)
                        //{
                        //    if (partQuantities.ContainsKey(transOrder.PARTNAME))
                        //    {
                        //        partQuantities[transOrder.PARTNAME] += transOrder.QUANT;
                        //        cquant = transOrder.CQUANT;
                        //        txtbLog.AppendText(transOrder.PARTNAME +":"+ transOrder.CQUANT);
                        //    }
                        //    else
                        //    {
                        //        partQuantities[transOrder.PARTNAME] = transOrder.QUANT;
                        //        // Assuming cquant is the same for all parts in the kit
                        //        cquant = transOrder.CQUANT;
                        //    }

                        //}
                        foreach (var transOrder in transOrders)
                        {
                            if (partQuantities.ContainsKey(transOrder.PARTNAME))
                                partQuantities[transOrder.PARTNAME] += transOrder.QUANT;
                            else
                                partQuantities[transOrder.PARTNAME] = transOrder.QUANT;

                            //// Always override — CQUANT should be the same for the part
                            //partCQuants[transOrder.PARTNAME] = transOrder.CQUANT;
                            // Only set cquant if it wasn't already stored
                            if (!partCQuants.ContainsKey(transOrder.PARTNAME))
                                partCQuants[transOrder.PARTNAME] = transOrder.CQUANT;

                            txtbLog.AppendText(transOrder.PARTNAME + ":" + transOrder.CQUANT+"\n");
                        }

                        //foreach (var part in partQuantities)
                        //{
                        //    int quant = part.Value;
                        //    int balance =    quant - cquant;
                        //    int simulation = balance; // This will be updated later with warehouse stock
                        //    if (!ipnToSerials.ContainsKey(part.Key))
                        //    {
                        //        ipnToSerials[part.Key] = new List<(Serial serial, int quant, int cquant, int balance, int simulation)>();
                        //    }
                        //    ipnToSerials[part.Key].Add((workOrder, quant, cquant, balance, simulation));
                        //}

                        foreach (var part in partQuantities)
                        {
                            int quant = part.Value;
                            int cquant = partCQuants[part.Key];
                            int balance = cquant - quant;
                            int simulation = balance; // updated later

                            if (!ipnToSerials.ContainsKey(part.Key))
                                ipnToSerials[part.Key] = new List<(Serial, int, int, int, int)>();

                            ipnToSerials[part.Key].Add((workOrder, quant, cquant, balance, simulation));
                        }

                        AppendLogMessage($"Loaded data for {workOrder.SERIALNAME}", Color.Green);
                    }
                    catch (HttpRequestException ex)
                    {
                        AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                    }
                    catch (Exception ex)
                    {
                        AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                    }
                }
            }
            // Update simulation values with warehouse stock
            foreach (var ipn in ipnToSerials.Keys.ToList())
            {
                int stock = warehouseStock.ContainsKey(ipn) ? warehouseStock[ipn] : 0;
                var updatedList = new List<(Serial serial, int quant, int cquant, int balance, int simulation)>();
                foreach (var (serial, quant, cquant, balance, simulation) in ipnToSerials[ipn])
                {
                    int updatedSimulation = stock + balance;
                    updatedList.Add((serial, quant, cquant, balance, updatedSimulation));
                }
                ipnToSerials[ipn] = updatedList;
            }
            return ipnToSerials;
        }

        private async Task<Dictionary<string, string>> GetMfpnForIpnsAsync(IEnumerable<string> ipns)
        {
            var ipnToMfpn = new Dictionary<string, string>();
            var ipnList = ipns.Distinct().ToList();
            int batchSize = 5; // Adjust as needed for API limits

            for (int i = 0; i < ipnList.Count; i += batchSize)
            {
                var batch = ipnList.Skip(i).Take(batchSize).ToList();
                // Build OData filter: PARTNAME eq 'IPN1' or PARTNAME eq 'IPN2' ...
                var filter = string.Join(" or ", batch.Select(ipn => $"PARTNAME eq '{ipn}'"));
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter={filter}&$select=PARTNAME,MNFPARTNAME";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);

                        foreach (var item in apiResponse["value"])
                        {
                            string partName = item["PARTNAME"]?.ToString();
                            string mfpn = item["MNFPARTNAME"]?.ToString();
                            if (!string.IsNullOrEmpty(partName))
                                ipnToMfpn[partName] = string.IsNullOrEmpty(mfpn) ? "-" : mfpn;
                        }

                        // For IPNs not returned, set as "-"
                        foreach (var ipn in batch)
                        {
                            if (!ipnToMfpn.ContainsKey(ipn))
                                ipnToMfpn[ipn] = "-";
                        }
                    }
                    catch (Exception ex)
                    {
                        foreach (var ipn in batch)
                            ipnToMfpn[ipn] = "-";
                        AppendLogMessage($"Error fetching MFPNs: {ex.Message}", Color.Red);
                    }
                }
            }

            return ipnToMfpn;
        }



        private async Task GenerateHTMLaggregated(string filename, Dictionary<string, (int balance, int stock, int simulation,int inHowManyKitsUsed)> tableData, string reportTitle, List<Serial> selectedWorkOrders, double completionPercentage)
        {
            tableData.OrderBy(x => x.Key == "simulation");

            // 1. Fetch MFPNs for all IPNs in tableData
            var ipns = tableData.Keys.ToList();
            var ipnToMfpn = await GetMfpnForIpnsAsync(ipns);

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
                writer.WriteLine(".orange { background-color: #DD571C; color: white; }");
                writer.WriteLine(".orange-balance { background-color: #DD571C; color: white; }");
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
                    writer.WriteLine($"<h2>{item.SERIALNAME} - {item.PARTNAME}_REV({item.REVNUM})_{item.QUANT}PCS - {item.SERIALSTATUSDES}</h2>");
                }
                int ipnsWithPositiveSimulation = tableData.Count(ipn => ipn.Value.simulation >= 0);
                writer.WriteLine($"<h2>Unique IPNs: {ipnsWithPositiveSimulation} / {tableData.Count} ({completionPercentage:F2}%)</h2>");
                writer.WriteLine("<table id='kitsTable'>");
                writer.WriteLine("<tr>");
                writer.WriteLine("<th onclick='sortTable(0)'>IPN</th>");
                writer.WriteLine("<th onclick='sortTable(1)'>MFPN</th>");
                writer.WriteLine("<th onclick='sortTable(2)'>WH Stock</th>");
                writer.WriteLine("<th onclick='sortTable(3)'>Kit Balance</th>");
                writer.WriteLine("<th onclick='sortTable(4)'>Simulation</th>");
                writer.WriteLine("<th onclick='sortTable(5)'>Used in n Kits</th>");
                writer.WriteLine("</tr>");
                foreach (var ipn in tableData.OrderBy(x => x.Value.simulation))
                {
                    var (balance, stock, simulation,inHowManyKitsUse) = ipn.Value;
                    string mfpn = ipnToMfpn.TryGetValue(ipn.Key, out var mf) ? mf : "-";
                    //string rowClass = simulation >= 0 ? "green" : "red";
                    string rowClass = "red";
                    if (simulation >= 0 && simulation <11)
                    {
                        rowClass = "orange";
                    }
                    else if (simulation >= 11)
                    {
                        rowClass = "green";
                    }


                        string balanceClass = balance < 0 ? "red-balance" : "";
                    writer.WriteLine($"<tr class='{rowClass}'>");
                    writer.WriteLine($"<td>{ipn.Key}</td>");
                    writer.WriteLine($"<td>{mfpn}</td>"); // New column
                    writer.WriteLine($"<td>{stock}</td>");
                    writer.WriteLine($"<td class='{balanceClass}'>{balance}</td>");
                    writer.WriteLine($"<td>{simulation}</td>");
                    writer.WriteLine($"<td>{inHowManyKitsUse}</td>");
                    writer.WriteLine("</tr>");
                }
                writer.WriteLine("</table>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
        }


        private async void btnByIPN_Click(object sender, EventArgs e)
        {
            if (dgvBomsList.Rows.Cast<DataGridViewRow>().Any(row => Convert.ToBoolean(row.Cells["Selected"].Value)))
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
                var ipnToSerials = await SimByIPN(selectedWorkOrders);
                var warehouseStock = await GetWarehouseStock(); // Fetch warehouse stock levels
                AppendLogMessage($"Generating HTML report by IPN \n", Color.Yellow);
                // Generate HTML report by IPN
                string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\IPNBasedReport_{_fileTimeStamp}.html";
                GenerateHTMLbyIPN(filename, ipnToSerials, $"IPN-based Simulation Report {_fileTimeStamp}", warehouseStock, selectedWorkOrders);
                // Open the file in default browser
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(filename)
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            else
            {
                MessageBox.Show("Please select at least one work order.", "No Work Orders Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private async Task<Dictionary<string, int>> GetWarehouseStock()
        {
            var warehouseStock = new Dictionary<string, int>();
            string selectedWarehouseName = GetSelectedWarehouseName();
            if (selectedWarehouseName != null)
            {
                //string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouseName}'&$expand=WARHSBAL_SUBFORM";
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouseName}'&$select=WARHSNAME&$expand=WARHSBAL_SUBFORM($select=PARTNAME,BALANCE)";

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
            return warehouseStock;
        }
        private void GenerateHTMLbyIPN(string filename, Dictionary<string, List<(Serial serial, int quant, int cquant, int balance, int simulation)>> ipnToSerials, string reportTitle, Dictionary<string, int> warehouseStock, List<Serial> selectedWorkOrders)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='text-align:center;background-color:gray;color:white;'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>IPN-based Simulation Report</title>");
                writer.WriteLine("<style>");
                writer.WriteLine("table { border-collapse: collapse; width: 100%; border: solid 1px; }");
                writer.WriteLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
                writer.WriteLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
                writer.WriteLine(".green { background-color: green; color: white; }");
                writer.WriteLine(".red { background-color: indianred; color: white; }");
                writer.WriteLine(".red-balance { background-color: indianred; color: white; }");
                writer.WriteLine(".header-table td { font-size: 2em; font-weight: bold; }");
                writer.WriteLine(".ipn-section { border: 2px solid white; padding: 10px; margin-bottom: 20px; }");
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
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine($"<h1>{reportTitle}</h1>");
                // Add the list of selected work orders
                writer.WriteLine("<h2>Selected Work Orders</h2>");
                writer.WriteLine("<table id='selectedWorkOrdersTable'>");
                writer.WriteLine("<tr>");
                writer.WriteLine("<th>Serial Name</th>");
                writer.WriteLine("<th>Part Name</th>");
                writer.WriteLine("<th>Quantity</th>");
                writer.WriteLine("<th>Status</th>");
                writer.WriteLine("<th>Revision</th>");
                writer.WriteLine("</tr>");
                foreach (var workOrder in selectedWorkOrders)
                {
                    writer.WriteLine("<tr>");
                    writer.WriteLine($"<td>{workOrder.SERIALNAME}</td>");
                    writer.WriteLine($"<td>{workOrder.PARTNAME}</td>");
                    writer.WriteLine($"<td>{workOrder.QUANT}</td>");
                    writer.WriteLine($"<td>{workOrder.SERIALSTATUSDES}</td>");
                    writer.WriteLine($"<td>{workOrder.REVNUM}</td>");
                    writer.WriteLine("</tr>");
                }
                writer.WriteLine("</table>");
                foreach (var ipn in ipnToSerials.Keys)
                {
                    var serials = ipnToSerials[ipn];
                    int totalQuant = serials.Sum(s => s.quant);
                    int totalCQuant = serials.Sum(s => s.cquant);
                    int totalBalance = totalQuant- totalCQuant;
                    int warehouseBalance = warehouseStock.ContainsKey(ipn) ? warehouseStock[ipn] : 0;
                    int totalSimulation = warehouseBalance + totalQuant - totalCQuant;
                    string totalSimulationClass = totalSimulation > 0 ? "green" : "red";
                    writer.WriteLine("<div class='ipn-section'>");
                    writer.WriteLine($"<h2>IPN: {ipn}</h2>");
                    writer.WriteLine("<table id='kitsTable'>");
                    writer.WriteLine("<tr>");
                    writer.WriteLine("<th>Warehouse Balance</th>");
                    writer.WriteLine("<th>Total Qty in KITs</th>");
                    writer.WriteLine("<th>Total Required in KITS</th>");
                    writer.WriteLine("<th>Total Balance in KITs</th>");
                    writer.WriteLine("<th>Total DELTA for all KITs</th>");
                    writer.WriteLine("</tr>");
                    writer.WriteLine("<tr>");
                    writer.WriteLine($"<td>{warehouseBalance}</td>");
                    writer.WriteLine($"<td>{totalQuant}</td>");
                    writer.WriteLine($"<td>{totalCQuant}</td>");
                    writer.WriteLine($"<td>{totalBalance}</td>");
                    writer.WriteLine($"<td class='{totalSimulationClass}'>{totalSimulation}</td>");
                    writer.WriteLine("</tr>");
                    writer.WriteLine("</table>");
                    writer.WriteLine("<table id='kitsTable'>");
                    writer.WriteLine("<tr>");
                    writer.WriteLine("<th onclick='sortTable(0)'>Work Order</th>");
                    writer.WriteLine("<th onclick='sortTable(1)'>IPN</th>");
                    writer.WriteLine("<th onclick='sortTable(2)'>Qty in KIT</th>");
                    writer.WriteLine("<th onclick='sortTable(3)'>Required in KIT</th>");
                    writer.WriteLine("<th onclick='sortTable(4)'>KIT DELTA</th>");
                    writer.WriteLine("</tr>");
                    // Subsequent rows: display each serial's balance and simulation
                    int currentWarehouseBalance = warehouseBalance;
                    foreach (var (serial, quant, cquant, balance, simulation) in serials)
                    {
                        int serialBalance = quant-cquant;
                        int serialSimulation = currentWarehouseBalance + serialBalance;
                        currentWarehouseBalance += serialBalance;
                        string serialBalanceClass = serialBalance > 0 ? "green" : "red";
                        string serialSimulationClass = serialSimulation > 0 ? "green" : "red";
                        writer.WriteLine("<tr>");
                        writer.WriteLine($"<td>{serial.SERIALNAME}</td>");
                        writer.WriteLine($"<td>{serial.PARTNAME}</td>");
                       
                        writer.WriteLine($"<td>{quant}</td>");
                        writer.WriteLine($"<td>{cquant}</td>");
                        writer.WriteLine($"<td class='{serialBalanceClass}'>{serialBalance}</td>");
                        writer.WriteLine("</tr>");
                    }
                    writer.WriteLine("</table>");
                    writer.WriteLine("</div>");
                    writer.WriteLine("<br>");
                }
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
        }
        private async void btnByKit_Click(object sender, EventArgs e)
        {
            if (dgvBomsList.Rows.Cast<DataGridViewRow>().Any(row => Convert.ToBoolean(row.Cells["Selected"].Value)))
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
                var warehouseStock = await GetWarehouseStock(); // Fetch warehouse stock levels
                var (kitDeficits, allIPNs) = await SimByBoms(selectedWorkOrders, warehouseStock);
                AppendLogMessage($"Generating HTML report by KITs \n", Color.Yellow);
                // Generate HTML report by KITs
                string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\KITBasedReport_{_fileTimeStamp}.html";
                GenerateHTMLbyKITs(filename, kitDeficits, allIPNs, $"KIT-based Simulation Report {_fileTimeStamp}", selectedWorkOrders);
                // Open the file in default browser
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(filename)
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            else
            {
                MessageBox.Show("Please select at least one work order to simulate", "No work orders selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private async Task<(Dictionary<string, List<(string ipn, int quant, int cquant, int balance, int delta)>> kitDeficits, Dictionary<string, HashSet<string>> allIPNs)> SimByBoms(List<Serial> selectedWorkOrders, Dictionary<string, int> warehouseStock)
        {
            var kitDeficits = new Dictionary<string, List<(string ipn, int quant, int cquant, int balance, int delta)>>();
            var allIPNs = new Dictionary<string, HashSet<string>>();
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
                        var ipnQuantities = new Dictionary<string, int>();
                        var ipnCQuantities = new Dictionary<string, int>();
                        foreach (var transOrder in transOrders)
                        {
                            string ipn = transOrder.PARTNAME;
                            int quant = transOrder.QUANT;
                            int cquant = transOrder.CQUANT;
                            // Sum up the quantities for each IPN
                            if (ipnQuantities.ContainsKey(ipn))
                            {
                                ipnQuantities[ipn] += quant;
                            }
                            else
                            {
                                ipnQuantities[ipn] = quant;
                                ipnCQuantities[ipn] = cquant;
                            }
                            // Track all IPNs for each work order
                            if (!allIPNs.ContainsKey(workOrder.SERIALNAME))
                            {
                                allIPNs[workOrder.SERIALNAME] = new HashSet<string>();
                            }
                            allIPNs[workOrder.SERIALNAME].Add(ipn);
                        }
                        foreach (var ipn in ipnQuantities.Keys)
                        {
                            int quant = ipnQuantities[ipn];
                            int cquant = ipnCQuantities[ipn];
                            int balance = quant - cquant;
                            int delta = warehouseStock.ContainsKey(ipn) ? warehouseStock[ipn] + balance : balance;
                            // Exclude rows where KIT Balance is greater than or equal to zero
                            if (balance < 0 && delta < 0)
                            {
                                if (!kitDeficits.ContainsKey(workOrder.SERIALNAME))
                                {
                                    kitDeficits[workOrder.SERIALNAME] = new List<(string ipn, int quant, int cquant, int balance, int delta)>();
                                }
                                kitDeficits[workOrder.SERIALNAME].Add((ipn, quant, cquant, balance, delta));
                            }
                            if (warehouseStock.ContainsKey(ipn))
                            {
                                warehouseStock[ipn] += balance;
                            }
                            else
                            {
                                warehouseStock[ipn] = balance;
                            }
                        }
                        // Ensure a table is generated for each work order
                        if (!kitDeficits.ContainsKey(workOrder.SERIALNAME))
                        {
                            kitDeficits[workOrder.SERIALNAME] = new List<(string ipn, int quant, int cquant, int balance, int delta)>();
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
            return (kitDeficits, allIPNs);
        }
        private void GenerateHTMLbyKITs(string filename, Dictionary<string, List<(string ipn, int quant, int cquant, int balance, int delta)>> kitDeficits, Dictionary<string, HashSet<string>> allIPNs, string reportTitle, List<Serial> selectedWorkOrders)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='text-align:center;background-color:gray;color:white;'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>KIT-based Simulation Report</title>");
                writer.WriteLine("<style>");
                writer.WriteLine("table { border-collapse: collapse; width: 100%; border: solid 1px; }");
                writer.WriteLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
                writer.WriteLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
                writer.WriteLine(".green { background-color: green; color: white; }");
                writer.WriteLine(".red { background-color: indianred; color: white; }");
                writer.WriteLine(".header-table td { font-size: 2em; font-weight: bold; }");
                writer.WriteLine(".kit-section { border: 2px solid white; padding: 10px; margin-bottom: 20px; }");
                writer.WriteLine("</style>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine($"<h1>{reportTitle}</h1>");
                foreach (var workOrder in selectedWorkOrders)
                {
                    var deficits = kitDeficits.ContainsKey(workOrder.SERIALNAME) ? kitDeficits[workOrder.SERIALNAME] : new List<(string ipn, int quant, int cquant, int balance, int delta)>();
                    int totalIPNs = allIPNs.ContainsKey(workOrder.SERIALNAME) ? allIPNs[workOrder.SERIALNAME].Count : 0;
                    int nonDeficitIPNs = totalIPNs - deficits.Count;
                    int deficitIPNs = deficits.Count;
                    double completionPercentage = totalIPNs > 0 ? (double)nonDeficitIPNs / totalIPNs * 100 : 100;
                    string headerClass = completionPercentage == 100 ? "green" : "";
                    writer.WriteLine($"<div class='kit-section {headerClass}'>");
                    if (deficitIPNs > 0)
                    {
                        writer.WriteLine($"<h2>Work Order: {workOrder.SERIALNAME} - In kit {nonDeficitIPNs} of {totalIPNs}, missing {deficitIPNs} (  {completionPercentage:F2} %)</h2>");
                    }
                    else
                    {
                        writer.WriteLine($"<h2>Work Order: {workOrder.SERIALNAME} - In kit {nonDeficitIPNs} of {totalIPNs} (  {completionPercentage:F2} %)</h2>");
                    }
                    writer.WriteLine("<table id='workOrderDetailsTable'>");
                    writer.WriteLine("<tr>");
                    writer.WriteLine("<th>Serial Name</th>");
                    writer.WriteLine("<th>Part Name</th>");
                    writer.WriteLine("<th>Quantity</th>");
                    writer.WriteLine("<th>Status</th>");
                    writer.WriteLine("<th>Revision</th>");
                    writer.WriteLine("</tr>");
                    writer.WriteLine("<tr>");
                    writer.WriteLine($"<td>{workOrder.SERIALNAME}</td>");
                    writer.WriteLine($"<td>{workOrder.PARTNAME}</td>");
                    writer.WriteLine($"<td>{workOrder.QUANT}</td>");
                    writer.WriteLine($"<td>{workOrder.SERIALSTATUSDES}</td>");
                    writer.WriteLine($"<td>{workOrder.REVNUM}</td>");
                    writer.WriteLine("</tr>");
                    writer.WriteLine("</table>");
                    if (completionPercentage < 100)
                    {
                        writer.WriteLine("<table id='kitsTable'>");
                        writer.WriteLine("<tr>");
                        writer.WriteLine("<th>IPN</th>");
                        writer.WriteLine("<th>Quantity in KIT</th>");
                        writer.WriteLine("<th>Required in KIT</th>");
                        writer.WriteLine("<th>KIT Balance</th>");
                        writer.WriteLine("<th>Total DELTA</th>");
                        writer.WriteLine("</tr>");
                        foreach (var (ipn, quant, cquant, balance, delta) in deficits)
                        {
                            string deltaClass = delta < 0 ? "red" : "green";
                            writer.WriteLine("<tr>");
                            writer.WriteLine($"<td>{ipn}</td>");
                            writer.WriteLine($"<td>{quant}</td>");
                            writer.WriteLine($"<td>{cquant}</td>");
                            writer.WriteLine($"<td>{balance}</td>");
                            writer.WriteLine($"<td class='{deltaClass}'>{delta}</td>");
                            writer.WriteLine("</tr>");
                        }
                        writer.WriteLine("</table>");
                    }
                    writer.WriteLine("</div>");
                    writer.WriteLine("<br>");
                }
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
        }
        private void btnAwaitingComp_Click(object sender, EventArgs e)
        {
            bool allChecked = dgvBomsList.Rows.Cast<DataGridViewRow>()
          .Where(row => row.Cells["SerialStatusDes"].Value.ToString() == "ממתין להשלמה")
          .All(row => Convert.ToBoolean(row.Cells["Selected"].Value));
            foreach (DataGridViewRow row in dgvBomsList.Rows)
            {
                if (row.Cells["SerialStatusDes"].Value.ToString() == "ממתין להשלמה")
                {
                    row.Cells["Selected"].Value = !allChecked;
                }
            }
            UpdateSelectedLabel();
            ToggleButtonColor((Button)sender, Color.OrangeRed); // Toggle button color
        }
        private void btnNotSentYet_Click(object sender, EventArgs e)
        {
            bool allChecked = dgvBomsList.Rows.Cast<DataGridViewRow>()
            .Where(row => row.Cells["SerialStatusDes"].Value.ToString() == "טרם נשלח קיט")
            .All(row => Convert.ToBoolean(row.Cells["Selected"].Value));
            foreach (DataGridViewRow row in dgvBomsList.Rows)
            {
                if (row.Cells["SerialStatusDes"].Value.ToString() == "טרם נשלח קיט")
                {
                    row.Cells["Selected"].Value = !allChecked;
                }
            }
            UpdateSelectedLabel();
            ToggleButtonColor((Button)sender, Color.Blue); // Toggle button color
        }
        private void ToggleButtonColor(Button button, Color color)
        {
            if (button.BackColor == color)
            {
                button.BackColor = Color.FromArgb(37, 37, 38); // Default color
            }
            else
            {
                button.BackColor = color;
            }
        }
        private async void btnGetBOMs_Click(object sender, EventArgs e)
        {
            cmbBom.Items.Clear();

            if (cmbWarehouses.SelectedItem == null)
            {
                AppendLogMessage("Please select a warehouse first.", Color.Red);
                return;
            }

            // Extract the first 3 characters of the selected warehouse name
            string warehousePrefix = cmbWarehouses.SelectedItem.ToString().Substring(0, 3);

            // Construct the API URL
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PART?$filter=PARTNAME eq '{warehousePrefix}*' AND TYPE eq 'P'";

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

                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var boms = apiResponse["value"].Select(b => b["PARTNAME"].ToString()).ToList();

                    // Populate the ComboBox with the BOMs
                    cmbBom.Items.Clear();
                    foreach (var bom in boms)
                    {
                        cmbBom.Items.Add(bom);
                    }

                    // Enable autocomplete
                    cmbBom.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    cmbBom.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    var autoCompleteCollection = new AutoCompleteStringCollection();
                    autoCompleteCollection.AddRange(boms.ToArray());
                    cmbBom.AutoCompleteCustomSource = autoCompleteCollection;

                    if (boms.Count > 0)
                    {
                        cmbBom.DroppedDown = true; // Open the dropdown for user selection
                        AppendLogMessage($"{boms.Count} BOMs loaded successfully.", Color.Green);
                    }
                    else
                    {
                        AppendLogMessage("No BOMs found for the selected warehouse.", Color.Orange);
                    }
                }
                catch (HttpRequestException ex)
                {
                    AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                }
                catch (Exception ex)
                {
                    AppendLogMessage($"An error occurred: {ex.Message}", Color.Red);
                }
            }
        }
        private async void cmbBom_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbRev.Items.Clear(); // Clear previous revisions

            if (cmbBom.SelectedItem == null)
            {
                AppendLogMessage("Please select a BOM first.", Color.Red);
                return;
            }

            // Get the selected BOM
            string selectedBom = cmbBom.SelectedItem.ToString();

            // Construct the API URL
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PART('{selectedBom}')/REVISIONS_SUBFORM?$filter=REVNUM eq '*'";

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

                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var revisions = apiResponse["value"].Select(r => r["REVNUM"].ToString()).ToList();

                    // Populate the ComboBox with the revisions
                    foreach (var revision in revisions)
                    {
                        cmbRev.Items.Add(revision);
                    }

                    if (revisions.Count > 0)
                    {
                        cmbRev.DroppedDown = true; // Open the dropdown for user selection
                        AppendLogMessage($"{revisions.Count} revisions loaded for BOM '{selectedBom}'.", Color.Green);
                    }
                    else
                    {
                        AppendLogMessage($"No revisions found for BOM '{selectedBom}'.", Color.Orange);
                    }
                }
                catch (HttpRequestException ex)
                {
                    AppendLogMessage($"Request error: {ex.Message}", Color.Red);
                }
                catch (Exception ex)
                {
                    AppendLogMessage($"An error occurred: {ex.Message}", Color.Red);
                }
            }
        }

        private void btnAddBom_Click(object sender, EventArgs e)
        {
            if (cmbBom.SelectedItem == null)
            {
                AppendLogMessage("Please select a BOM before adding.", Color.Red);
                return;
            }

            if (cmbRev.SelectedItem == null)
            {
                AppendLogMessage("Please select a revision before adding.", Color.Red);
                return;
            }

            // Prompt the user for quantity
            string inputQty = Microsoft.VisualBasic.Interaction.InputBox("Enter quantity for the BOM:", "Quantity Input", "1");
            if (!int.TryParse(inputQty, out int quantity) || quantity <= 0)
            {
                AppendLogMessage("Invalid quantity entered. Please enter a positive number.", Color.Red);
                return;
            }

            // Generate the SerialName (SIM00000xx format)
            string simCount = DateTime.Now.ToString("yyMMddHHmmss");
            int rowCount = dgvBomsList.Rows.Count + 1;
            //string serialName = $"SIM{rowCount.ToString("D2")}";
            string serialName = $"SIM{simCount}";

            // Add a new row to the DataGridView
            dgvBomsList.Rows.Add(
                false, // Selected
                serialName, // SerialName
                cmbBom.SelectedItem.ToString(), // PartName
                "סימולציה", // SerialStatusDes
                quantity, // Quant
                cmbRev.SelectedItem.ToString(), // RevNum
                string.Empty // Priority (can be set later if needed)
            );

            AppendLogMessage($"Added BOM '{cmbBom.SelectedItem}' with revision '{cmbRev.SelectedItem}' and quantity {quantity}.", Color.Green);
        }

    }
}
