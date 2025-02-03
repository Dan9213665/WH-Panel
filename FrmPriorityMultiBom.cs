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

namespace WH_Panel
{

    public partial class FrmPriorityMultiBom : Form
    {
        private AppSettings settings;
        List<Warehouse> loadedWareHouses = new List<Warehouse>();
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
            //GetGetRobWosList();
            chkbBomsList.DrawMode = DrawMode.OwnerDrawFixed;
            //chkbBomsList.DrawItem += chkbBomsList_DrawItem;
        }
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
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var serials = apiResponse["value"].ToObject<List<Serial>>();
                    // Populate the CheckedListBox with the data
                    chkbBomsList.Items.Clear();
                    foreach (var serial in serials)
                    {
                        chkbBomsList.Items.Add(serial);
                    }
                    lblLoading.BackColor = Color.Green;
                    lblLoading.Text = "Data Loaded";
                }
                catch (HttpRequestException ex)
                {
                    //MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtbLog.ForeColor = Color.Red;
                    txtbLog.AppendText($"Request error: {ex.Message} \n");
                    txtbLog.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtbLog.ForeColor = Color.Red;
                    txtbLog.AppendText($"Request error: {ex.Message}");
                    txtbLog.ScrollToCaret();
                }
            }
        }


        //private async void GetGetRobWosList(string warehouseName, string warehouseDesc)
        //{
        //    string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=PARTNAME eq '{warehouseName}*'";
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            // Set the request headers if needed
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            // Set the Authorization header
        //            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        //            // Make the HTTP GET request
        //            HttpResponseMessage response = await client.GetAsync(url);
        //            response.EnsureSuccessStatusCode();
        //            // Read the response content
        //            string responseBody = await response.Content.ReadAsStringAsync();
        //            // Parse the JSON response
        //            var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
        //            var serials = apiResponse["value"].ToObject<List<Serial>>();
        //            chkbBomsList.Items.Clear();

        //            if (serials.Count == 0)
        //            {
        //                AppendLogMessage($"No data found for {warehouseName} - {warehouseDesc} \n", Color.Red);
        //                return;
        //            }
        //            else
        //            {
        //                // Populate the CheckedListBox with the data
        //                foreach (var serial in serials)
        //                {
        //                    chkbBomsList.Items.Add(serial);
        //                    if (serial.SERIALSTATUSDES != "נסגרה")
        //                    {
        //                        chkbBomsList.SetItemChecked(chkbBomsList.Items.Count - 1, true); // Select the item by default
        //                    }


        //                }

        //                lblLoading.BackColor = Color.Green;
        //                lblLoading.Text = "Data Loaded";

        //                if (serials.Count == 1)
        //                {
        //                    AppendLogMessage($"{serials.Count} Work Order loaded for {warehouseName} - {warehouseDesc} \n", Color.Green);
        //                }
        //                else
        //                {
        //                    AppendLogMessage($"{serials.Count} Work Orders loaded for {warehouseName} - {warehouseDesc} \n", Color.Green);
        //                }
        //                // Force redraw of the CheckedListBox
        //                chkbBomsList.Refresh();
        //            }
        //        }
        //        catch (HttpRequestException ex)
        //        {
        //            AppendLogMessage($"Request error: {ex.Message} \n", Color.Red);
        //        }
        //        catch (Exception ex)
        //        {
        //            AppendLogMessage($"Request error: {ex.Message}\n", Color.Red);
        //        }
        //    }
        //}

        private async void GetGetRobWosList(string warehouseName, string warehouseDesc)
        {
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=PARTNAME eq '{warehouseName}*'";
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
                    var serials = apiResponse["value"].ToObject<List<Serial>>();
                    chkbBomsList.Items.Clear();

                    if (serials.Count == 0)
                    {
                        AppendLogMessage($"No data found for {warehouseName} - {warehouseDesc} \n", Color.Red);
                        return;
                    }
                    else
                    {
                        // Populate the CheckedListBox with the data
                        foreach (var serial in serials)
                        {
                            chkbBomsList.Items.Add(serial);
                            if (serial.SERIALSTATUSDES != "נסגרה")
                            {
                                chkbBomsList.SetItemChecked(chkbBomsList.Items.Count - 1, true); // Select the item by default
                           
                            }
                        }

                        lblLoading.BackColor = Color.Green;
                        lblLoading.Text = "Data Loaded";

                        if (serials.Count == 1)
                        {
                            AppendLogMessage($"{serials.Count} Work Order loaded for {warehouseName} - {warehouseDesc} \n", Color.Green);
                        }
                        else
                        {
                            AppendLogMessage($"{serials.Count} Work Orders loaded for {warehouseName} - {warehouseDesc} \n", Color.Green);
                        }
                        // Force redraw of the CheckedListBox
                        chkbBomsList.Refresh();
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

        //private void chkbBomsList_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    if (e.Index < 0) return;

        //    var serial = chkbBomsList.Items[e.Index] as Serial;
        //    if (serial == null) return;

        //    // Debugging output
        //    Debug.WriteLine($"Drawing item: {serial.SERIALSTATUSDES}");

        //    // Set the background color based on the SERIALSTATUSDES
        //    Color backgroundColor;
        //    Color textColor = Color.Black;

        //    switch (serial.SERIALSTATUSDES)
        //    {
        //        case "קיט מלא":
        //            backgroundColor = Color.Green;
        //            textColor = Color.White;
        //            break;
        //        case "שוחררה":
        //            backgroundColor = Color.Orange;
        //            textColor = Color.Black;
        //            break;
        //        case "נסגרה":
        //            backgroundColor = Color.Black;
        //            textColor = Color.Gray;
        //            break;
        //        case "הוקפאה":
        //            backgroundColor = Color.Red;
        //            textColor = Color.White;
        //            break;
        //        default:
        //            backgroundColor = chkbBomsList.BackColor;
        //            textColor = chkbBomsList.ForeColor;
        //            break;
        //    }

        //    // Draw the background
        //    e.Graphics.FillRectangle(new SolidBrush(backgroundColor), e.Bounds);

        //    // Draw the text
        //    TextRenderer.DrawText(e.Graphics, serial.ToString(), e.Font, e.Bounds, textColor, TextFormatFlags.Left);

        //    // Draw the focus rectangle if the item has focus
        //    e.DrawFocusRectangle();
        //}


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



        //private void SetDarkModeColors(Control parentControl)
        //{
        //    Color backgroundColor = Color.FromArgb(37, 37, 38); // Dark background color
        //    Color foregroundColor = Color.FromArgb(241, 241, 241); // Light foreground color
        //    Color borderColor = Color.FromArgb(45, 45, 48); // Border color for controls
        //    foreach (Control control in parentControl.Controls)
        //    {
        //        // Set the background and foreground colors
        //        control.BackColor = backgroundColor;
        //        control.ForeColor = foregroundColor;
        //        // Handle specific control types separately
        //        if (control is System.Windows.Forms.Button button)
        //        {
        //            button.FlatStyle = FlatStyle.Flat;
        //            button.FlatAppearance.BorderColor = borderColor;
        //            button.ForeColor = foregroundColor;
        //        }
        //        else if (control is System.Windows.Forms.GroupBox groupBox)
        //        {
        //            groupBox.ForeColor = foregroundColor;
        //        }
        //        else if (control is System.Windows.Forms.TextBox textBox)
        //        {
        //            textBox.BorderStyle = BorderStyle.FixedSingle;
        //            textBox.BackColor = backgroundColor;
        //            textBox.ForeColor = foregroundColor;
        //        }
        //        else if (control is System.Windows.Forms.Label label)
        //        {
        //            if (label.Name != "lblLoading")
        //            {
        //                label.BackColor = backgroundColor;
        //                label.ForeColor = foregroundColor;
        //            }
        //        }
        //        else if (control is TabControl tabControl)
        //        {
        //            tabControl.BackColor = backgroundColor;
        //            tabControl.ForeColor = foregroundColor;
        //            foreach (TabPage tabPage in tabControl.TabPages)
        //            {
        //                tabPage.BackColor = backgroundColor;
        //                tabPage.ForeColor = foregroundColor;
        //            }
        //        }
        //        else if (control is DataGridView dataGridView)
        //        {
        //            dataGridView.EnableHeadersVisualStyles = false;
        //            dataGridView.BackgroundColor = backgroundColor;
        //            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = borderColor;
        //            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = foregroundColor;
        //            dataGridView.RowHeadersDefaultCellStyle.BackColor = borderColor;
        //            dataGridView.DefaultCellStyle.BackColor = backgroundColor;
        //            dataGridView.DefaultCellStyle.ForeColor = foregroundColor;
        //            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255);
        //            dataGridView.DefaultCellStyle.SelectionForeColor = foregroundColor;
        //            foreach (DataGridViewColumn column in dataGridView.Columns)
        //            {
        //                column.HeaderCell.Style.BackColor = borderColor;
        //                column.HeaderCell.Style.ForeColor = foregroundColor;
        //            }
        //        }
        //        else if (control is System.Windows.Forms.ComboBox comboBox)
        //        {
        //            comboBox.FlatStyle = FlatStyle.Flat;
        //            comboBox.BackColor = backgroundColor;
        //            comboBox.ForeColor = foregroundColor;
        //        }
        //        else if (control is DateTimePicker dateTimePicker)
        //        {
        //            dateTimePicker.BackColor = backgroundColor;
        //            dateTimePicker.ForeColor = foregroundColor;
        //        }
        //        // Recursively update controls within containers
        //        if (control.Controls.Count > 0)
        //        {
        //            SetDarkModeColors(control);
        //        }
        //    }
        //}
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
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                // Get the selected warehouse name
                string selectedWarehouse = loadedWareHouses[comboBox1.SelectedIndex].WARHSNAME;
                string selectedWarehouseDesc = loadedWareHouses[comboBox1.SelectedIndex].WARHSDES;
                // Retrieve the work orders for the selected warehouse
                GetGetRobWosList(selectedWarehouse, selectedWarehouseDesc);
            }
            else
            {
                txtbLog.ForeColor = Color.Red;
                txtbLog.AppendText("Please select a warehouse.");
            }
        }

        private void chkbBomsList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Delay the update to allow the check state to be updated first
            this.BeginInvoke((MethodInvoker)delegate
            {
                UpdateSelectedLabel();
            });
        }

        private void UpdateSelectedLabel()
        {
            int selectedCount = chkbBomsList.CheckedItems.Count;
            int totalCount = chkbBomsList.Items.Count;
            lblSelected.Text = $"Selected {selectedCount} of {totalCount}";
        }

        private async void btnSim1_Click(object sender, EventArgs e)
        {
            var selectedWorkOrders = chkbBomsList.CheckedItems.Cast<Serial>().ToList();
            var ipnBalances = new Dictionary<string, int>();

            foreach (var workOrder in selectedWorkOrders)
            {
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{workOrder.SERIALNAME}'&$expand=TRANSORDER_K_SUBFORM";
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
                        var transOrders = apiResponse["value"].First["TRANSORDER_K_SUBFORM"].ToObject<List<TransOrderKSubform>>();

                        foreach (var transOrder in transOrders)
                        {
                            int balance = transOrder.PQUANT - transOrder.CQUANT;
                            if (ipnBalances.ContainsKey(transOrder.PARTNAME))
                            {
                                ipnBalances[transOrder.PARTNAME] += balance;
                            }
                            else
                            {
                                ipnBalances[transOrder.PARTNAME] = balance;
                            }
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

            // Fetch warehouse stock levels
            var warehouseStock = new Dictionary<string, int>();
            foreach (var warehouse in loadedWareHouses)
            {
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{warehouse.WARHSNAME}'&$expand=WARHSBAL_SUBFORM";
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
                        var warehouseBalances = apiResponse["value"].First["WARHSBAL_SUBFORM"].ToObject<List<WarehouseBalance>>();

                        foreach (var balance in warehouseBalances)
                        {

                            warehouseStock[balance.PARTNAME] = balance.BALANCE;

                            //if (warehouseStock.ContainsKey(balance.PARTNAME))
                            //{
                            //    warehouseStock[balance.PARTNAME] += balance.TBALANCE;
                            //}
                            //else
                            //{
                            //    warehouseStock[balance.PARTNAME] = balance.TBALANCE;
                            //}
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

            // Generate HTML report
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\MultiKitsStatusReport_{_fileTimeStamp}.html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='text-align:center;background-color:gray;color:white;'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>Multi Kits Status Report</title>");
                writer.WriteLine("<style>");
                writer.WriteLine("table { border-collapse: collapse; width: 100%; }");
                writer.WriteLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
                writer.WriteLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
                writer.WriteLine(".green { background-color: green; color: white; }");
                writer.WriteLine(".red { background-color: indianred; color: white; }");
                writer.WriteLine(".header-table td { font-size: 2em; font-weight: bold; }");
                writer.WriteLine("</style>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine($"<h1>Multi Kits Status Report {_fileTimeStamp}</h1>");
                writer.WriteLine("<table id='kitsTable'>");
                writer.WriteLine("<tr><th>IPN</th><th>Balance</th><th>Stock</th><th>Simulation</th></tr>");

                foreach (var ipn in ipnBalances.Keys)
                {
                    int balance = ipnBalances[ipn];
                   // int stock = warehouseStock.ContainsKey(ipn) ? warehouseStock[ipn] : 0;
                   int stock = warehouseStock.FirstOrDefault(x => x.Key == ipn).Value;

                    int simulation = stock + balance;
                    string rowClass = simulation >= 0 ? "green" : "red";

                    writer.WriteLine($"<tr class='{rowClass}'><td>{ipn}</td><td>{balance}</td><td>{stock}</td><td>{simulation}</td></tr>");
                }

                writer.WriteLine("</table>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }

            // Open the file in default browser
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}



