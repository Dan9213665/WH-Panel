using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using static WH_Panel.FrmPriorityAPI;
using Action = System.Action;
using Button = System.Windows.Forms.Button;
using DataTable = System.Data.DataTable;
using GroupBox = System.Windows.Forms.GroupBox;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace WH_Panel
{
    public partial class FrmPriorityPanDbSearch : Form
    {
        private DataTable dataTable;
        private DataView dataView;
        private List<Warehouse> loadedWareHouses = new List<Warehouse>();
        public AppSettings settings;

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

                    await LoadDataIntoDataTable(); // Load data into the DataTable after initializing
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
            string url = "https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$select=WARHSNAME,WARHSDES,WARHS";
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
                    WarehouseApiResponse apiResponse = JsonConvert.DeserializeObject<WarehouseApiResponse>(responseBody);
                    if (apiResponse.value != null && apiResponse.value.Count > 0)
                    {
                        loadedWareHouses.Clear();

                        var excludedWarehouses = new HashSet<string> { "666", "400", "450", "500", "Flr", "Main" };
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


            int currentWH = 0;

            foreach (var warehouse in loadedWareHouses)
            {
                AddLogRow($"Loading data for warehouse: {warehouse.WARHSNAME}", Color.Orange);
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{warehouse.WARHSNAME}'&$expand=WARHSBAL_SUBFORM";
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
                        var apiResponse = JsonConvert.DeserializeObject<WarehouseApiResponse>(responseBody);
                        if (apiResponse.value != null && apiResponse.value.Count > 0)
                        {
                            var warehouseBalances = apiResponse.value.SelectMany(w => w.WARHSBAL_SUBFORM).ToList();
                            foreach (var balance in warehouseBalances)
                            {
                                dataTable.Rows.Add(warehouse.WARHSNAME, balance.PARTNAME, balance.MNFPARTNAME, balance.PARTDES, balance.BALANCE, balance.CDATE.Substring(0,10), balance.PART);
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        AddLogRow($"LoadDataIntoDataTable: Request error: {ex.Message}\n{ex.StackTrace}", Color.Red);
                    }
                    catch (Exception ex)
                    {
                        AddLogRow($"LoadDataIntoDataTable: An error occurred: {ex.Message}\n{ex.StackTrace}", Color.Red);
                    }
                }
                currentWH++;
                AddLogRow($"Data loaded for warehouse: {warehouse.WARHSNAME} ({currentWH}/{countOFWHs})", Color.Green);
            }
            dataView = new DataView(dataTable);
            dgwALLDATA.DataSource = dataView;
        }

        private void InitializeDataTable()
        {
            dataTable = new DataTable();
            dataTable.Columns.Add("WH", typeof(string));
            dataTable.Columns.Add("PARTNAME", typeof(string));
            dataTable.Columns.Add("MNFPARTNAME", typeof(string));
            dataTable.Columns.Add("PARTDES", typeof(string));
            dataTable.Columns.Add("BALANCE", typeof(int));
            dataTable.Columns.Add("CDATE", typeof(string));
            dataTable.Columns.Add("PART", typeof(int));
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
        private void FilterData(object sender, EventArgs e)
        {
            StringBuilder filter = new StringBuilder();

            if (!string.IsNullOrEmpty(txtbWH.Text))
            {
                filter.Append($"WH LIKE '%{txtbWH.Text}%'");
            }
            if (!string.IsNullOrEmpty(txtbIPN.Text))
            {
                if (filter.Length > 0) filter.Append(" AND ");
                filter.Append($"PARTNAME LIKE '%{txtbIPN.Text}%'");
            }
            if (!string.IsNullOrEmpty(txtbMFPN.Text))
            {
                if (filter.Length > 0) filter.Append(" AND ");
                filter.Append($"MNFPARTNAME LIKE '%{txtbMFPN.Text}%'");
            }
            if (!string.IsNullOrEmpty(txtbDESC.Text))
            {
                if (filter.Length > 0) filter.Append(" AND ");
                string[] descTerms = txtbDESC.Text.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string term in descTerms)
                {
                    filter.Append($"PARTDES LIKE '%{term.Trim()}%' AND ");
                }
                filter.Length -= 5; // Remove the trailing " AND "
            }

            dataView.RowFilter = filter.ToString();
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                dgwINSTOCK.Rows.Clear();
                var selectedRow = dgwALLDATA.Rows[e.RowIndex];
                await ExtractMFPNForRow(selectedRow);
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
                        //UpdatePing(stopwatch.ElapsedMilliseconds);
                        // Read the response content
                        string logPartResponseBody = await logPartResponse.Content.ReadAsStringAsync();
                        // Parse the JSON response
                        var logPartApiResponse = JsonConvert.DeserializeObject<LogPartApiResponse>(logPartResponseBody);
                        // Check if the response contains any data
                        if (logPartApiResponse.value != null && logPartApiResponse.value.Count > 0)
                        {
                            // Set AutoGenerateColumns to false
                            dgwTRANSACTIONS.AutoGenerateColumns = false;
                            // Clear existing columns
                            dgwTRANSACTIONS.Columns.Clear();
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
                            dgwTRANSACTIONS.Columns.AddRange(new DataGridViewColumn[]
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
                            dgwTRANSACTIONS.Rows.Clear();
                            foreach (var logPart in logPartApiResponse.value)
                            {
                                foreach (var trans in logPart.PARTTRANSLAST2_SUBFORM)
                                {
                                    dgwTRANSACTIONS.Rows.Add("", trans.LOGDOCNO, trans.DOCDES, trans.SUPCUSTNAME, "", trans.TQUANT, "");
                                }
                            }
                            groupBox6.Text = $"TRANSACTIONS for {partName}";
                            //ColorTheRows(dataGridView2);
                            foreach (DataGridViewRow row in dgwTRANSACTIONS.Rows)
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
                            dgwTRANSACTIONS.Sort(dgwTRANSACTIONS.Columns[0], ListSortDirection.Descending);
                            // Populate dgwINSTOCK with items that are currently in stock
                            var actualStock = int.Parse(selectedRow.Cells["BALANCE"].Value.ToString());
                            await PopulateInStockData(partName, actualStock);
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
        private async Task PopulateInStockData(string partName, int zeroOrHero)
        {
            if(zeroOrHero > 0)
            {
                
           
            // Separate data into ROB and notRob lists
            var robList = new List<DataGridViewRow>();
            var notRobList = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgwTRANSACTIONS.Rows)
            {
                if (row.Cells["LOGDOCNO"].Value != null && row.Cells["UDATE"].Value != null && DateTime.TryParse(row.Cells["UDATE"].Value.ToString(), out _))
                {
                    string docNo = row.Cells["LOGDOCNO"].Value.ToString();
                    if (docNo.StartsWith("ROB") || docNo.StartsWith("IC")|| docNo.StartsWith("WR")||docNo.StartsWith("SH"))
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
            string partUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PART eq {partId}";
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
                url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_P?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_P_SUBFORM";
            }
            else if (logDocNo.StartsWith("WR"))
            {
                // Handle GR documents
                url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_T?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_T_SUBFORM";
            }
            else if (logDocNo.StartsWith("SH"))
            {
                // Handle GR documents
                url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_D?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_D_SUBFORM";
            }
            else if (logDocNo.StartsWith("ROB"))
            {
                url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{logDocNo}'";
            }
            else if (logDocNo.StartsWith("IC"))
            {
                url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_C?$filter=DOCNO eq '{logDocNo}'";
            }
            else
            {
                // Handle other document types if needed
                url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_P?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_P_SUBFORM";
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
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{docNo}'";
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
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_P?$filter=DOCNO eq '{docNo}'";
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
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_T?$filter=DOCNO eq '{docNo}'";
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

        private async void btnGETMFPN_Click(object sender, EventArgs e)
        {
            await FetchMFPNsForAllRows();
        }

        private async Task FetchMFPNsForAllRows()
        {
            AddLogRow("Fetching MFPNs for all rows\n",Color.Yellow);
            foreach (DataGridViewRow row in dgwALLDATA.Rows)
            {
                await ExtractMFPNForRow(row);
            }
            AddLogRow("MFPN fetching completed\n", Color.Green);
        }


    }
}
