﻿using System;
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
using Microsoft.Extensions.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;
using System.Data.OleDb;
using Microsoft.VisualBasic;
using static WH_Panel.FrmBOM;
using System.Runtime.InteropServices;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DataTable = System.Data.DataTable;
using TextBox = System.Windows.Forms.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using System.Xml.Serialization;
using Label = System.Windows.Forms.Label;
using System.Web;
using Seagull.BarTender.Print;
using Range = Microsoft.Office.Interop.Excel.Range;
using Application = Microsoft.Office.Interop.Excel.Application;
using static Seagull.Framework.OS.ServiceControlManager;
using System;
using System.Windows.Forms;
using _Application = Microsoft.Office.Interop.Excel._Application;
using OfficeOpenXml;
using System.Drawing.Printing;
using Button = System.Windows.Forms.Button;
using GroupBox = System.Windows.Forms.GroupBox;
using WH_Panel;
using File = System.IO.File;
using Point = System.Drawing.Point;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Data.SqlClient;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using Font = System.Drawing.Font;
using Action = System.Action;
using OfficeOpenXml.Drawing.Slicer.Style;
using System.Security.Cryptography.Pkcs;
using static QRCoder.PayloadGenerator;
using OfficeOpenXml.Style;
using static WH_Panel.FrmPriorityBom;
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
            InitializeDataGridView();
            // Set the DrawMode property and handle the DrawItem event
            cmbROBxList.DrawMode = DrawMode.OwnerDrawFixed;
            cmbROBxList.DrawItem += cmbROBxList_DrawItem;
            cmbROBxList.SelectedIndexChanged += cmbROBxList_SelectedIndexChanged;
            // Initialize DataGridView columns
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
            public int PQUANT { get; set; }
            public int CQUANT { get; set; }
            public int KLINE { get; set; }
            public int TRANS { get; set; }
            public int QUANT { get; set; }
            public int DELTA => QUANT - CQUANT;
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
            public List<Warehouse> WarehouseList { get; set; }
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
        private string username = "api"; // Replace with your actual username
        private string password = "DdD@12345"; // Replace with your actual password
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
                    //string un = settings.ApiUsername;
                    string un = username;
                    //MessageBox.Show(un);
                    //string pw = settings.ApiPassword;
                    string pw = password;
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
                txtbInputIPN.Focus();
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
            dgwBom.Columns.Add("TBALANCE", "WH"); // Add WH column before KIT
            dgwBom.Columns.Add("QUANT", "KIT");
            dgwBom.Columns.Add("CQUANT", "Required");
            dgwBom.Columns.Add("DELTA", "Delta");
            dgwBom.Columns.Add("CALC", "CALC");
            dgwBom.Columns.Add("ALT", "ALT");
            dgwBom.Columns.Add("TRANS", "TRANS");
            dgwBom.Columns.Add("KLINE", "KLINE");
            dgwBom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private async Task LoadBomDetails(string serialName)
        {
            progressBar1.Value = 0;
            progressBar1.Update();
            int completedItems = 0;
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{serialName}'&$expand=TRANSORDER_K_SUBFORM";
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
                    // Check if the response contains the expected data
                    if (apiResponse["value"] != null && apiResponse["value"].Any())
                    {
                        var bomDetails = apiResponse["value"].First["TRANSORDER_K_SUBFORM"].ToObject<List<TransOrderKSubform>>();
                        // Aggregate the PQUANT values for each unique PARTNAME
                        var aggregatedDetails = bomDetails
                            .GroupBy(detail => detail.PARTNAME)
                            .Select(group => new TransOrderKSubform
                            {
                                PARTNAME = group.Key,
                                PARTDES = group.First().PARTDES,
                                CQUANT = group.First().CQUANT,
                                CALC = string.Join("+", group.Select(detail => detail.QUANT)),
                                QUANT = group.Sum(detail => detail.QUANT),
                                KLINE = group.First().KLINE,
                                TRANS = group.First().TRANS
                            })
                            .ToList();
                        // Log the CALC values
                        //foreach (var detail in aggregatedDetails)
                        //{
                        //    MessageBox.Show($"PARTNAME: {detail.PARTNAME}, CALC: {detail.CALC}");
                        //}
                        // Populate the DataGridView with the aggregated data
                        dgwBom.Rows.Clear();
                        foreach (var detail in aggregatedDetails)
                        {
                            //string calcValue = !string.IsNullOrEmpty(detail.CALC) && !detail.CALC.Contains("+0") ? detail.CALC : "";
                            if (detail.CALC.Contains("+0"))
                            {
                                detail.CALC = detail.CALC.Replace("+0", "");
                            }
                            else if (detail.CALC == "0")
                            {
                                detail.CALC = "";
                            }
                            dgwBom.Rows.Add(detail.PARTNAME,"", detail.PARTDES,"", detail.QUANT, detail.CQUANT, detail.DELTA, detail.CALC,"", detail.TRANS, detail.KLINE);
                            int pbTotal = aggregatedDetails.Count;
                            if (detail.DELTA>=0)
                            {
                                completedItems++;
                            }
                            progressBar1.Value = (completedItems / pbTotal) * 100;
                        }
                        // Update the progress label
                        UpdateProgressLabel();
                        progressBar1.Update();
                        txtbInputIPN.PlaceholderText = $"Filter by IPN ({aggregatedDetails.Count.ToString()})";
                        // Fetch MFPNs for each row with a delay
                        await FetchWarehouseBalances();
                        //await FetchMFPNsWithDelay();
                        //await FetchMFPNsForAllRows();
                    }
                    else
                    {
                        MessageBox.Show("No BOM details found for the selected serial.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void UpdateProgressLabel()
        {
            int totalItems = dgwBom.Rows.Count;
            int completedItems = dgwBom.Rows.Cast<DataGridViewRow>().Count(row => Convert.ToInt32(row.Cells["DELTA"].Value) >= 0);
            if (totalItems > 0)
            {
                int percentage = (completedItems * 100) / totalItems;
                lblProgress.Text = $"{completedItems} of {totalItems} IPNs in KIT ({percentage}%)";
            }
            else
            {
                lblProgress.Text = "0 / 0 items (0%)";
            }
        }
        private async Task FetchWarehouseBalances()
        {
            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                if (row.Cells["PARTNAME"].Value != null)
                {
                    string partName = row.Cells["PARTNAME"].Value.ToString();
                    string warehouseName = partName.Substring(0, 3); // Get the first 3 characters of the PARTNAME
                    string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{warehouseName}'&$expand=WARHSBAL_SUBFORM($filter=PARTNAME eq '{partName}')";
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
                            var warehouse = apiResponse["value"].FirstOrDefault();
                            if (warehouse != null)
                            {
                                var balance = warehouse["WARHSBAL_SUBFORM"].FirstOrDefault();
                                if (balance != null)
                                {
                                    int tBalance = balance["TBALANCE"].Value<int>();
                                    // Update the WH column in the DataGridView
                                    row.Cells["TBALANCE"].Value = tBalance;
                                }
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
                    // Introduce an artificial delay between each API call
                    await Task.Delay(200); // 100 milliseconds delay
                }
            }
        }
        private async Task FetchMFPNForRow(DataGridViewRow row)
        {
            //MessageBox.Show("Test");
            if (row.Cells["PARTNAME"].Value != null && (row.Cells["MFPN"].Value == string.Empty || row.Cells["MFPN"].Value == null))
            //if (row.Cells["PARTNAME"].Value != null) //&& (string.IsNullOrEmpty(row.Cells["MFPN"].Value?.ToString())|| row.Cells["MFPN"].Value=="")
            {
                string partName = row.Cells["PARTNAME"].Value.ToString();
                //MessageBox.Show("partName:"+partName);
                string partUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PARTNAME eq '{partName}'";
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
                        // Make the HTTP GET request for part details
                        HttpResponseMessage partResponse = await client.GetAsync(partUrl);
                        partResponse.EnsureSuccessStatusCode();
                        // Read the response content
                        string partResponseBody = await partResponse.Content.ReadAsStringAsync();
                        // Parse the JSON response
                        var partApiResponse = JsonConvert.DeserializeObject<ApiResponse>(partResponseBody);
                        //MessageBox.Show(partApiResponse.ToString());
                        // Check if the response contains any data
                        if (partApiResponse.value != null && partApiResponse.value.Count > 0)
                        {
                            var part = partApiResponse.value[0];
                            //MessageBox.Show(partApiResponse.value[0].ToString());
                            // Directly update the DataGridView cell
                            row.Cells["MFPN"].Value = part.MNFPARTNAME;
                            dgwBom.Refresh();
                            //MessageBox.Show($"Updated MFPN for {partName} to {part.MNFPARTNAME}");
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
            await Task.Delay(200); // 100 milliseconds delay
        }
        private async Task FetchMFPNsForAllRows()
        {
            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                await FetchMFPNForRow(row);
            }
        }
        private async Task FetchMFPNsWithDelay()
        {
            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                if (row.Cells["PARTNAME"].Value != null)  //&& ((row.Cells["MFPN"].Value == string.Empty) || (row.Cells["MFPN"].Value == null))
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
                           MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (textBox.PlaceholderText.Contains("Filter by") || textBox.PlaceholderText.Contains("Input Qty"))
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
                case "WH":
                    return "TBALANCE";
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
            txtbInputIPN.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            txtbINPUTqty.Clear();
            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                row.Visible = true;
            }
            dgwBom.Update();
            // Fetch MFPNs for each row with a delay
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
            else if (dgwBom.Columns[e.ColumnIndex].Name == "TBALANCE")
            {
                int DELTA = Convert.ToInt32(dgwBom.Rows[e.RowIndex].Cells["DELTA"].Value);
                int whValue = 0;
                if (e.Value != null && int.TryParse(e.Value.ToString(), out whValue))
                {
                    if (whValue >= Math.Abs(DELTA) && whValue != 0)
                    {
                        e.CellStyle.BackColor = Color.Green;
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.IndianRed;
                    }
                }
            }
            else if (dgwBom.Columns[e.ColumnIndex].Name == "QUANT")
            {
                int req = Convert.ToInt32(dgwBom.Rows[e.RowIndex].Cells["CQUANT"].Value);
                int kitValue = Convert.ToInt32(dgwBom.Rows[e.RowIndex].Cells["QUANT"].Value);
                //if (kitValue > 0)
                //{
                //   // MessageBox.Show(kitValue.ToString());
                //}
                if (e.Value != null && int.TryParse(e.Value.ToString(), out kitValue))
                {
                    if (kitValue >= req && kitValue != 0)
                    {
                        e.CellStyle.BackColor = Color.Green;
                    }
                    else if (kitValue != 0)
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
                            var SUPCUSTNAMEColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "SUPCUSTNAME",
                                HeaderText = "Source_Req",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "SUPCUSTNAME"
                            };
                            var tQuantColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "TQUANT",
                                HeaderText = "Quantity",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "TQUANT"
                            };
                            var PackColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "PACK",
                                HeaderText = "PACK",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "PACK"
                            };
                            // Add columns to the DataGridView
                            dgwIPNmoves.Columns.AddRange(new DataGridViewColumn[]
                            {
                        curDateColumn,
                        logDocNoColumn,
                        logDOCDESColumn,
                        SUPCUSTNAMEColumn,
                        tQuantColumn,
                        PackColumn
                            });
                            //// Populate the DataGridView with the data
                            //dgwIPNmoves.Rows.Clear();
                            //foreach (var logPart in logPartApiResponse.value)
                            //{
                            //    foreach (var trans in logPart.PARTTRANSLAST2_SUBFORM)
                            //    {
                            //        dgwIPNmoves.Rows.Add(trans.CURDATE, trans.LOGDOCNO, trans.DOCDES, trans.SUPCUSTNAME, trans.TQUANT, "");
                            //    }
                            //}
                            //gbxIPNstockMovements.Text = $"Stock Movements for {partName}";
                            //ColorTheRows(dgwIPNmoves);
                            // Populate the DataGridView with the data
                            dgwIPNmoves.Rows.Clear();
                            foreach (var logPart in logPartApiResponse.value)
                            {
                                foreach (var trans in logPart.PARTTRANSLAST2_SUBFORM)
                                {
                                    var rowIndex = dgwIPNmoves.Rows.Add(trans.CURDATE, trans.LOGDOCNO, trans.DOCDES, trans.SUPCUSTNAME, trans.TQUANT, "");
                                    var row = dgwIPNmoves.Rows[rowIndex];
                                    // Fetch the PACK code asynchronously
                                    _ = FetchAndSetPackCodeAsync(row, trans.LOGDOCNO, partName, trans.TQUANT);
                                }
                            }
                            gbxIPNstockMovements.Text = $"Stock Movements for {partName}";
                            ColorTheRows(dgwIPNmoves);
                            // Fetch MFPN for the selected row
                            await FetchMFPNForRow(selectedRow);
                            //await FetchMFPNsWithDelay();
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
        private async Task FetchAndSetPackCodeAsync(DataGridViewRow row, string logDocNo, string partName, int quant)
        {
            string packCode = await FetchPackCodeAsync(logDocNo, partName, quant);
            if (packCode != null)
            {
                row.Cells["PACK"].Value = packCode;
            }
        }
        public async Task<string> FetchPackCodeAsync(string logDocNo, string partName, int quant)
        {
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_P?$filter=DOCNO eq '{logDocNo}'&$expand=TRANSORDER_P_SUBFORM";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var transOrders = apiResponse["value"].SelectMany(d => d["TRANSORDER_P_SUBFORM"]).ToList();
                    // Find the matching PARTNAME and QUANT
                    var matchingOrder = transOrders.FirstOrDefault(t => t["PARTNAME"].ToString() == partName && int.Parse(t["TQUANT"].ToString()) == quant);
                    if (matchingOrder != null)
                    {
                        return matchingOrder["PACKCODE"].ToString();
                    }
                    return null;
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
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
        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string filterText = txtbInputIPN.Text.Trim();
                bool found = false;
                int visibleRowCount = 0;
                bool dontneedeMoreItems = false;
                foreach (DataGridViewRow row in dgwBom.Rows)
                {
                    var needMoreItems = int.Parse(row.Cells["DELTA"].Value.ToString());
                    if (row.Cells["PARTNAME"].Value != null && row.Cells["PARTNAME"].Value.ToString() == filterText)
                    {
                        if (needMoreItems < 0)
                        {
                            row.Visible = true;
                            found = true;
                            visibleRowCount++;
                        }
                        else
                        {
                            dontneedeMoreItems = true;
                        }
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
                dgwBom.Update();
                if (dontneedeMoreItems)
                {
                    txtbInputIPN.Clear();
                    txtbInputIPN.Focus();
                    ClearFilters();
                    AutoClosingMessageBox.Show($"{filterText} NOT needed anymore!", 2000, Color.Orange);
                    return;
                }
                if (visibleRowCount == 1)
                {
                    txtbINPUTqty.Focus();
                    await FetchMFPNForRow(dgwBom.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Visible));
                }
                if (!found)
                {
                    txtbInputIPN.Clear();
                    ClearFilters();
                    AutoClosingMessageBox.Show($"{filterText} NOT FOUND!", 2000, Color.Red); // Show message for 2 seconds
                }
            }
        }
        private void ClearFilters()
        {
            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                row.Visible = true;
            }
            dgwBom.Update();
        }
        private void txtbINPUTqty_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                txtbInputIPN.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                txtbINPUTqty.Clear();
                txtbInputIPN.Focus();
                foreach (DataGridViewRow row in dgwBom.Rows)
                {
                    row.Visible = true;
                }
                dgwBom.Update();
            }
        }
        private async void txtbINPUTqty_KeyDown(object sender, KeyEventArgs e)
        {
            // Count only the visible rows
            int visibleRowCount = dgwBom.Rows.Cast<DataGridViewRow>().Count(row => row.Visible);
            if (visibleRowCount == 1)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (int.TryParse(txtbINPUTqty.Text, out int qty) && qty > 0)
                    {
                        var filteredRow = dgwBom.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Visible);
                        if (filteredRow != null)
                        {
                            string partName = filteredRow.Cells["PARTNAME"].Value.ToString();
                            string serialName = txtbRob.Text; // Assuming txtbRob contains the SERIALNAME
                            int cQuant = int.Parse(filteredRow.Cells["CQUANT"].Value.ToString()); // Get the CQUANT value
                            int inKit = int.Parse(filteredRow.Cells["QUANT"].Value.ToString()); // Get the QUANT value
                            int neededQty = cQuant - inKit;
                            await AddItemToKit(partName, serialName, neededQty, qty, filteredRow);
                            txtbINPUTqty.Clear();
                            txtbInputIPN.Clear();
                            txtbInputIPN.Focus();
                            // Update the progress label
                            UpdateProgressLabel();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtbINPUTqty.Clear();
                        txtbINPUTqty.Focus();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid IPN", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtbInputIPN.Clear();
                txtbInputIPN.Focus();
            }
        }
        private async Task AddItemToKit(string partName, string serialName, int cQuant, int qty, DataGridViewRow filteredRow)
        {
            // Check quantity availability in the warehouse
            string checkUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq 'ENE'&$expand=WARHSBAL_SUBFORM($filter=PARTNAME eq '{partName}')";
            int availableQty = 0;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request to check quantity availability
                    HttpResponseMessage checkResponse = await client.GetAsync(checkUrl);
                    string checkResponseBody = await checkResponse.Content.ReadAsStringAsync();
                    checkResponse.EnsureSuccessStatusCode();
                    var checkApiResponse = JsonConvert.DeserializeObject<JObject>(checkResponseBody);
                    var warehouse = checkApiResponse["value"].FirstOrDefault();
                    if (warehouse != null)
                    {
                        var balance = warehouse["WARHSBAL_SUBFORM"].FirstOrDefault();
                        if (balance != null)
                        {
                            availableQty = balance["TBALANCE"].Value<int>();
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (availableQty < qty)
            {
                MessageBox.Show("Insufficient quantity available in the warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Retrieve the TRANSORDER_K_SUBFORM data
            string getUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL('{serialName}')/TRANSORDER_K_SUBFORM?$filter=PARTNAME eq '{partName}'";
            int kline = 0;
            string package = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Make the HTTP GET request to retrieve the TRANSORDER_K_SUBFORM data
                    HttpResponseMessage getResponse = await client.GetAsync(getUrl);
                    string getResponseBody = await getResponse.Content.ReadAsStringAsync();
                    getResponse.EnsureSuccessStatusCode();
                    var getApiResponse = JsonConvert.DeserializeObject<JObject>(getResponseBody);
                    var transOrderKSubform = getApiResponse["value"];
                    if (transOrderKSubform != null)
                    {
                        // Filter the data to find the row with the matching PARTNAME and CQUANT
                        var matchingRow = transOrderKSubform.FirstOrDefault(row =>
                            row["PARTNAME"].ToString() == partName &&
                            row["CQUANT"].Value<int>() == cQuant);
                        if (matchingRow != null)
                        {
                            kline = matchingRow["KLINE"].Value<int>();
                            if (matchingRow["PACKCODE"] != null)
                            {
                                package = matchingRow["PACKCODE"].Value<string>();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No matching row found in TRANSORDER_K_SUBFORM.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // Construct the PATCH request URL
            string patchUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL('{serialName}')/TRANSORDER_K_SUBFORM(TYPE='K',KLINE={kline})";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    // Create the JSON payload for the PATCH request
                    var payload = new
                    {
                        QUANT = qty,
                        WARHSNAME = "ENE",
                        TOWARHSNAME = "Flr",
                        PACKCODE = package
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    // Make the PATCH request
                    HttpResponseMessage patchResponse = await client.PatchAsync(patchUrl, content);
                    string patchResponseBody = await patchResponse.Content.ReadAsStringAsync();
                    patchResponse.EnsureSuccessStatusCode();
                    AutoClosingMessageBox.Show($"{partName} - {qty} PCS moved to {serialName}", 1000, Color.Green); // Show message for 2 seconds
                    // Make another GET request to update the WH cell
                    HttpResponseMessage checkResponse = await client.GetAsync(checkUrl);
                    string checkResponseBody = await checkResponse.Content.ReadAsStringAsync();
                    checkResponse.EnsureSuccessStatusCode();
                    var checkApiResponse = JsonConvert.DeserializeObject<JObject>(checkResponseBody);
                    var warehouse = checkApiResponse["value"].FirstOrDefault();
                    if (warehouse != null)
                    {
                        var balance = warehouse["WARHSBAL_SUBFORM"].FirstOrDefault();
                        if (balance != null)
                        {
                            availableQty = balance["TBALANCE"].Value<int>();
                            filteredRow.Cells["TBALANCE"].Value = availableQty;
                        }
                    }
                    // Update the DataGridView row
                    filteredRow.Cells["QUANT"].Value = qty;
                    filteredRow.Cells["DELTA"].Value = qty - cQuant;
                    txtbINPUTqty.Clear();
                    txtbINPUTqty.Focus();
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
        private void btnKitLabel_Click(object sender, EventArgs e)
        {
        }
        private void btnKitLabel_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the right mouse button was clicked
            if (e.Button == MouseButtons.Left && txtbName.Text != string.Empty)
            {
                string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                // Extract only the last part of the project name
                string projectName = txtbName.Text;
                using (CustomPrintDialog customDialog = new CustomPrintDialog())
                {
                    // Show the custom dialog
                    DialogResult result = customDialog.ShowDialog();
                    int copiesToPrint = 0;
                    // Determine the number of copies based on the user's selection
                    switch (result)
                    {
                        case DialogResult.OK:
                            copiesToPrint = 1;
                            break;
                        case DialogResult.Yes:
                            copiesToPrint = 2;
                            break;
                        case DialogResult.No:
                            copiesToPrint = 3;
                            break;
                        case DialogResult.Ignore:
                            string[] splitParts = projectName.Split('_');
                            WHitem itemToPrint = new WHitem();
                            itemToPrint.IPN = "קיט מלא";
                            itemToPrint.MFPN = splitParts[1];
                            itemToPrint.Description = splitParts[0];
                            itemToPrint.Stock = int.Parse(txtbQty.Text.ToString());
                            itemToPrint.Updated_on = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
                            printStickerFullKit(itemToPrint);
                            break;
                        case DialogResult.Abort:
                            string modifiedProjectName2 = projectName.Substring(0, projectName.Length - 5);
                            string[] splitParts2 = modifiedProjectName2.Split('_');
                            WHitem itemToPrint2 = new WHitem();
                            itemToPrint2.IPN = "רכיבים בגלילה";
                            itemToPrint2.MFPN = splitParts2[1];
                            itemToPrint2.Description = splitParts2[0];
                            itemToPrint2.Stock = int.Parse(txtbQty.Text.ToString());
                            itemToPrint2.Updated_on = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
                            printStickerFullKit(itemToPrint2);
                            break;
                        case DialogResult.Cancel:
                            SendEmail();
                            break;
                    }
                    if (result != DialogResult.Ignore && result != DialogResult.Abort && result != DialogResult.Cancel)
                    {
                        GenerateHTMLkitBoxLabel(copiesToPrint);
                    }
                    // Call the method with the chosen number of copies
                }
            }
        }
        private void GenerateHTMLkitBoxLabel(int qtyToPrint)
        {
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            // Extract only the last part of the project name
            string projectName = txtbName.Text;
            // Construct the filename
            string filename = "\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\" +
                _fileTimeStamp + "_box label for_" + projectName + ".html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='text-align:center'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>" + txtbName.Text.ToString() + "</title>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                string[] parts = { txtbName.Text, txtbRob.Text, txtbQty.Text + " PCS" };
                string imageUrl = string.Empty;
                string base64Image = string.Empty;
                // Search for the prefix in the customer folders
                string prefix = txtbName.Text.Substring(0, 3);
                string customersPath = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS";
                foreach (string customerDir in Directory.GetDirectories(customersPath))
                {
                    string prefixFilePath = Path.Combine(customerDir, "prefix.txt");
                    if (File.Exists(prefixFilePath))
                    {
                        string filePrefix = File.ReadAllText(prefixFilePath).Trim();
                        if (filePrefix == prefix)
                        {
                            string logoFilePath = Path.Combine(customerDir, "logo.png");
                            if (File.Exists(logoFilePath))
                            {
                                imageUrl = logoFilePath.Replace("\\", "/");
                                break;
                            }
                        }
                    }
                }
                string backgroundImageUrl = "eleBackGND.png";
                string altText = "WH image";
                for (int i = 0; i < qtyToPrint; i++)
                {
                    writer.WriteLine("<table border='1' style='width: 600px; margin: auto; display: table;'>");
                    writer.WriteLine("<col style='width: 25%; background: url(" + backgroundImageUrl + ") no-repeat center center; background-size: 100% 100%;'>");
                    writer.WriteLine("<col style='width: 75%;'>");
                    writer.WriteLine("<tr>");
                    writer.WriteLine("<td style='position: relative; vertical-align: middle;'>");
                    writer.WriteLine("    <img id='logoImage' src='" + imageUrl + "' alt='" + altText + "' style='height: 100%; width: 100%;'>");
                    writer.WriteLine("    <div style='position: absolute; top: 1%; left: 5%; transform: translate(-1%, -1%); color: white; font-size: 15px; font-weight: bold; text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.7);'>");
                    string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    writer.WriteLine(currentDateTime);
                    writer.WriteLine("    </div>");
                    writer.WriteLine("</td>");
                    writer.WriteLine("<td style='text-align: center; background: rgba(255, 255, 255, 0.1) url(" + backgroundImageUrl + ") no-repeat center center; background-size: 111% 111%; vertical-align: middle; transform: scaleX(-1);'>");
                    foreach (string part in parts)
                    {
                        writer.WriteLine("<div style='text-align: center;  border: 1px solid black; margin: 0px; padding: 5px; vertical-align: middle; font-size: 50px; font-weight: bold;text-shadow: -4px -4px 2px #fff, 4px -4px 2px #fff, -4px 4px 2px #fff, 4px 4px 2px #fff;transform: scaleX(-1);'>" + part + "</div>");
                    }
                    writer.WriteLine("</td>");
                    writer.WriteLine("</tr>");
                    writer.WriteLine("</table>");
                }
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
        private void printStickerFullKit(WHitem wHitem)
        {
            try
            {
                string userName = Environment.UserName;
                string fpst = @"C:\\Users\\" + userName + "\\Desktop\\Print_Stickers.xlsx";
                string thesheetName = "Sheet1";
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fpst + "; Extended Properties=\"Excel 12.0;HDR=YES;IMEX=0\"";
                OleDbConnection conn = new OleDbConnection(constr);
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE [" + thesheetName + "$] SET PN = @PN, MFPN = @MFPN, ItemDesc = @ItemDesc, QTY = @QTY, UPDATEDON = @Updated_on";
                cmd.Parameters.AddWithValue("@PN", wHitem.IPN);
                cmd.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                cmd.Parameters.AddWithValue("@ItemDesc", wHitem.Description);
                cmd.Parameters.AddWithValue("@QTY", wHitem.Stock);
                cmd.Parameters.AddWithValue("@Updated_on", wHitem.Updated_on);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                Microsoft.VisualBasic.Interaction.AppActivate("PN_STICKER_2022.btw - BarTender Designer");
                SendKeys.SendWait("^p");
                SendKeys.SendWait("{Enter}");
                //ComeBackFromPrint();
                Microsoft.VisualBasic.Interaction.AppActivate("Imperium Tabula Principalis");
            }
            catch (Exception e)
            {
                MessageBox.Show("Sticker printing failed : " + e.Message);
            }
        }
        private void SendEmail()
        {
            string windowTitle = this.Text; // 'this' refers to the current Form
            // Find the index of ".xlsm" in the window title
            int index = windowTitle.IndexOf(".xlsm");
            // Extract the project name, including ".xlsm"
            string fullProjectName = index >= 0 ? windowTitle.Substring(0, index + 5) : windowTitle;
            // Extract only the last part of the project name
            string projectName = fullProjectName.Split('\\').Last();
            //string fileName = openFileDialog1.FileName;
            string fileName = fullProjectName;
            //MessageBox.Show(fileName);
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Open(fileName);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1]; // First sheet (index 1 in Interop)
            // Find the "Alts" and "DELTA" column indices
            int altsColumnIndex = -1;
            int deltaColumnIndex = -1;
            Microsoft.Office.Interop.Excel.Range headerRow = (Microsoft.Office.Interop.Excel.Range)worksheet.Rows[1];
            for (int i = 1; i <= headerRow.Columns.Count; i++)
            {
                var cell = (Microsoft.Office.Interop.Excel.Range)headerRow.Cells[1, i];
                string columnHeader = cell.Value?.ToString();
                if (columnHeader == "Alts")
                {
                    altsColumnIndex = i;
                }
                else if (columnHeader == "DELTA")
                {
                    deltaColumnIndex = i;
                }
                if (altsColumnIndex != -1 && deltaColumnIndex != -1)
                {
                    break;
                }
            }
            if (deltaColumnIndex == -1)
            {
                MessageBox.Show("Could not find the 'DELTA' column.");
                workbook.Close(false);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(excelApp);
                return;
            }
            // Build HTML table from Excel data
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.Append("<table border='1' style='border-collapse:collapse;'>");
            // Add header row
            htmlTable.Append("<tr>");
            for (int col = 1; col <= altsColumnIndex; col++)
            {
                string header = ((Microsoft.Office.Interop.Excel.Range)headerRow.Cells[1, col]).Value?.ToString();
                htmlTable.AppendFormat("<th>{0}</th>", header ?? string.Empty);
            }
            htmlTable.Append("</tr>");
            // Add data rows with conditional formatting for "DELTA" values
            int row = 2;
            while (((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[row, 1]).Value != null)
            {
                htmlTable.Append("<tr>");
                for (int col = 1; col <= altsColumnIndex; col++)
                {
                    var cell = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[row, col];
                    string cellValue = cell.Value?.ToString() ?? string.Empty;
                    if (col == deltaColumnIndex && double.TryParse(cellValue, out double deltaValue))
                    {
                        string color = deltaValue < 0 ? "IndianRed" : "LightGreen";
                        htmlTable.AppendFormat("<td style='background-color:{0};'>{1}</td>", color, cellValue);
                    }
                    else
                    {
                        htmlTable.AppendFormat("<td>{0}</td>", cellValue);
                    }
                }
                htmlTable.Append("</tr>");
                row++;
            }
            htmlTable.Append("</table>");
            workbook.Close(false);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);
            var outlookApp = new Outlook.Application();
            Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
            mailItem.Subject = projectName.Substring(0, projectName.Length - 5).ToString() + "_UPDATED_" + DateAndTime.Now.ToString("yyyyMMddHHmm");
            // Set CC field
            mailItem.CC = "lgt@robotron.co.il";
            // Hardcoded in-house email addresses
            List<string> inhouseEmails = new List<string>
    {
        "production@robotron.co.il",
        "avishay@robotron.co.il",
        "rehesh@robotron.co.il",
        "vlad@robotron.co.il"
    };
            // Extract client domain from project name
            string clientDomain = projectName.Split('_')[0].ToLower();
            // Get emails from the client's domain
            List<string> clientEmails = GetUniqueClientEmails(clientDomain);//GetEmailsFromDomain(outlookApp, clientDomain); 
            // Display a form with checkboxes for all recipients
            RecipientSelectionForm selectionForm = new RecipientSelectionForm(inhouseEmails, clientEmails);
            if (selectionForm.ShowDialog() == DialogResult.OK)
            {
                // Combine selected emails into the "To" field
                mailItem.To = string.Join(";", selectionForm.SelectedEmails);
                // Embed the HTML table in the email body
                mailItem.HTMLBody = "<html><body>" + htmlTable.ToString() + "</body></html>";
                // Send the email
                mailItem.Send();
                MessageBox.Show("Email sent successfully.");
            }
            Marshal.ReleaseComObject(mailItem);
            Marshal.ReleaseComObject(outlookApp);
        }
        public List<string> GetUniqueClientEmails(string clientDomain)
        {
            var emails = new HashSet<string>();
            var outlookApp = new Outlook.Application();
            // Initialize and show the loading form
            LoadingForm loadingForm = new LoadingForm();
            // Display the loading form on a new thread to avoid blocking
            var loadingThread = new Thread(() =>
            {
                loadingForm.ShowDialog();
            });
            loadingThread.Start();
            try
            {
                Outlook.Folder inboxFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox) as Outlook.Folder;
                Outlook.Folder outboxFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail) as Outlook.Folder;
                // Helper function to add unique emails from a folder
                void AddEmailsFromFolder(Outlook.Folder folder, string domain)
                {
                    foreach (var item in folder.Items)
                    {
                        if (item is Outlook.MailItem mail && !string.IsNullOrEmpty(mail.SenderEmailAddress))
                        {
                            string senderEmailLower = mail.SenderEmailAddress.ToLower();
                            string domainLower = domain.ToLower();
                            if (senderEmailLower.Contains(domainLower))
                            {
                                string contactInfo = $"{mail.SenderName} ({mail.SenderEmailAddress})";
                                emails.Add(contactInfo); // HashSet prevents duplicate entries
                            }
                        }
                    }
                }
                // Add emails for the given client domain from both Inbox and Outbox folders
                AddEmailsFromFolder(inboxFolder, clientDomain);
                AddEmailsFromFolder(outboxFolder, clientDomain);
                // If no emails found for the client domain, fallback to local domain
                if (emails.Count == 0)
                {
                    string localDomain = "robotron";
                    AddEmailsFromFolder(inboxFolder, localDomain);
                    AddEmailsFromFolder(outboxFolder, localDomain);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Close the loading form once processing is complete
                if (loadingForm.InvokeRequired)
                {
                    loadingForm.Invoke(new Action(() => loadingForm.Close()));
                }
                else
                {
                    loadingForm.Close();
                }
            }
            return emails.ToList();
        }
        public class RecipientSelectionForm : Form
        {
            private CheckedListBox checkedListBox;
            private Button sendButton;
            private Button cancelButton;
            public List<string> SelectedEmails { get; private set; }
            public RecipientSelectionForm(List<string> inhouseEmails, List<string> clientEmails)
            {
                SelectedEmails = new List<string>();
                checkedListBox = new CheckedListBox
                {
                    Dock = DockStyle.Top,
                    Height = 200,
                    CheckOnClick = true
                };
                // Add in-house emails to the list with a label for clarity
                checkedListBox.Items.Add("In-House Emails:", false);
                foreach (var email in inhouseEmails)
                {
                    checkedListBox.Items.Add(email, false);
                }
                // Add client emails to the list with a label for clarity
                checkedListBox.Items.Add("Client Emails:", false);
                foreach (var email in clientEmails)
                {
                    checkedListBox.Items.Add(email, false);
                }
                sendButton = new Button
                {
                    Text = "Send",
                    Dock = DockStyle.Bottom
                };
                sendButton.Click += SendButton_Click;
                cancelButton = new Button
                {
                    Text = "Cancel",
                    Dock = DockStyle.Bottom
                };
                cancelButton.Click += (s, e) => DialogResult = DialogResult.Cancel;
                Controls.Add(checkedListBox);
                Controls.Add(sendButton);
                Controls.Add(cancelButton);
                Text = "Select Recipients";
                Height = 300;
                Width = 300;
                StartPosition = FormStartPosition.CenterScreen;
            }
            private void SendButton_Click(object sender, EventArgs e)
            {
                // Collect selected emails from the CheckedListBox
                foreach (var item in checkedListBox.CheckedItems)
                {
                    if (!item.ToString().EndsWith(":")) // Skip the section labels
                    {
                        SelectedEmails.Add(item.ToString());
                    }
                }
                //if (SelectedEmails.Count == 0)
                //{
                //    MessageBox.Show("Please select at least one recipient.");
                //    return;
                //}
                DialogResult = DialogResult.OK;
            }
        }
        private void btnReport_Click(object sender, EventArgs e)
        {
            // Sort the DataGridView by the DELTA column in descending order
            dgwBom.Sort(dgwBom.Columns["DELTA"], ListSortDirection.Ascending);
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            string filename = $"\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\KitsStatusReport_{_fileTimeStamp}.html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html style='text-align:center;background-color:gray;color:white;'>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>Kit Status Report</title>");
                writer.WriteLine("<style>");
                writer.WriteLine("table { border-collapse: collapse; width: 100%; }");
                writer.WriteLine("th, td { border: 1px solid black; padding: 8px; text-align: center;}");
                writer.WriteLine("th { cursor: pointer; position: sticky; top: 0; background: black; z-index: 1; }");
                writer.WriteLine(".green { background-color: green; color: white; }");
                writer.WriteLine(".red { background-color: indianred; color: white; }");
                writer.WriteLine(".header-table td { font-size: 2em; font-weight: bold; }");
                writer.WriteLine("</style>");
                writer.WriteLine("<script>");
                writer.WriteLine("function sortTable(n) {");
                writer.WriteLine("  var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;");
                writer.WriteLine("  table = document.getElementById('kitsTable');");
                writer.WriteLine("  switching = true;");
                writer.WriteLine("  dir = 'asc';");
                writer.WriteLine("  while (switching) {");
                writer.WriteLine("    switching = false;");
                writer.WriteLine("    rows = table.rows;");
                writer.WriteLine("    for (i = 1; i < (rows.length - 1); i++) {");
                writer.WriteLine("      shouldSwitch = false;");
                writer.WriteLine("      x = rows[i].getElementsByTagName('TD')[n];");
                writer.WriteLine("      y = rows[i + 1].getElementsByTagName('TD')[n];");
                writer.WriteLine("      if (dir == 'asc') {");
                writer.WriteLine("        if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {");
                writer.WriteLine("          shouldSwitch = true;");
                writer.WriteLine("          break;");
                writer.WriteLine("        }");
                writer.WriteLine("      } else if (dir == 'desc') {");
                writer.WriteLine("        if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {");
                writer.WriteLine("          shouldSwitch = true;");
                writer.WriteLine("          break;");
                writer.WriteLine("        }");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("    if (shouldSwitch) {");
                writer.WriteLine("      rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);");
                writer.WriteLine("      switching = true;");
                writer.WriteLine("      switchcount ++;");
                writer.WriteLine("    } else {");
                writer.WriteLine("      if (switchcount == 0 && dir == 'asc') {");
                writer.WriteLine("        dir = 'desc';");
                writer.WriteLine("        switching = true;");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("function filterTable() {");
                writer.WriteLine("  var input, filter, table, tr, td, i, j, txtValue;");
                writer.WriteLine("  input = document.getElementById('filterInput');");
                writer.WriteLine("  filter = input.value.toLowerCase();");
                writer.WriteLine("  table = document.getElementById('kitsTable');");
                writer.WriteLine("  tr = table.getElementsByTagName('tr');");
                writer.WriteLine("  for (i = 1; i < tr.length; i++) {");
                writer.WriteLine("    tr[i].style.display = 'none';");
                writer.WriteLine("    td = tr[i].getElementsByTagName('td');");
                writer.WriteLine("    for (j = 0; j < td.length; j++) {");
                writer.WriteLine("      if (td[j]) {");
                writer.WriteLine("        txtValue = td[j].textContent || td[j].innerText;");
                writer.WriteLine("        if (txtValue.toLowerCase().indexOf(filter) > -1) {");
                writer.WriteLine("          tr[i].style.display = '';");
                writer.WriteLine("          break;");
                writer.WriteLine("        }");
                writer.WriteLine("      }");
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("function clearFilter() {");
                writer.WriteLine("  document.getElementById('filterInput').value = '';");
                writer.WriteLine("  filterTable();");
                writer.WriteLine("}");
                writer.WriteLine("function printReport() {");
                writer.WriteLine("  window.print();");
                writer.WriteLine("}");
                writer.WriteLine("function changeFontColor(color) {");
                writer.WriteLine("  var elements = document.querySelectorAll('body, body *');");
                writer.WriteLine("  for (var i = 0; i < elements.length; i++) {");
                writer.WriteLine("    elements[i].style.color = color;");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("window.addEventListener('beforeprint', function() { changeFontColor('black'); });");
                writer.WriteLine("window.addEventListener('afterprint', function() { changeFontColor(''); });");
                writer.WriteLine("</script>");
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine($"<h1>Kit Status Report {_fileTimeStamp}</h1>");
                // Add the single row table with text from txtbRob, txtbName, txtbRev, and txtbQty
                writer.WriteLine("<table class='header-table' style='margin-bottom: 20px;'>");
                writer.WriteLine("<tr>");
                writer.WriteLine($"<td>{txtbRob.Text}</td>");
                writer.WriteLine($"<td>{txtbName.Text}</td>");
                writer.WriteLine($"<td>{txtbRev.Text}</td>");
                writer.WriteLine($"<td>{txtbQty.Text}</td>");
                writer.WriteLine("</tr>");
                writer.WriteLine("</table>");
                // Add the filter input box, clear button, and print button
                writer.WriteLine("<div style='margin-bottom: 20px; text-align: center;'>");
                writer.WriteLine("<input type='text' id='filterInput' onkeyup='filterTable()' placeholder='Filter table...' style='padding: 10px; width: 50%;text-align:center;background:orange;'>");
                writer.WriteLine("<button onclick='clearFilter()' style='padding: 10px;'>Clear</button>");
                writer.WriteLine("<button onclick='printReport()' style='padding: 10px;'>Print</button>");
                writer.WriteLine("</div>");
                writer.WriteLine("<table id='kitsTable'>");
                // Write table headers
                writer.WriteLine("<tr>");
                foreach (DataGridViewColumn column in dgwBom.Columns)
                {
                    if (column.Name != "TRANS" && column.Name != "KLINE")
                    {
                        writer.WriteLine($"<th onclick='sortTable({column.Index})'>{column.HeaderText}</th>");
                    }
                }
                writer.WriteLine("</tr>");
                // Write table rows
                foreach (DataGridViewRow row in dgwBom.Rows)
                {
                    writer.WriteLine("<tr>");
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (dgwBom.Columns[cell.ColumnIndex].Name != "TRANS" && dgwBom.Columns[cell.ColumnIndex].Name != "KLINE")
                        {
                            string cellValue = cell.Value?.ToString() ?? string.Empty;
                            string cellClass = string.Empty;
                            if (dgwBom.Columns[cell.ColumnIndex].Name == "DELTA")
                            {
                                if (int.TryParse(cellValue, out int deltaValue))
                                {
                                    cellClass = deltaValue >= 0 ? "green" : "red";
                                }
                            }
                            else if (dgwBom.Columns[cell.ColumnIndex].Name == "TBALANCE")
                            {
                                int DELTA = Convert.ToInt32(row.Cells["DELTA"].Value);
                                if (int.TryParse(cellValue, out int whValue))
                                {
                                    cellClass = (whValue >= Math.Abs(DELTA) && whValue != 0) ? "green" : "red";
                                }
                            }
                            else if (dgwBom.Columns[cell.ColumnIndex].Name == "QUANT")
                            {
                                int req = Convert.ToInt32(row.Cells["CQUANT"].Value);
                                if (int.TryParse(cellValue, out int kitValue))
                                {
                                    cellClass = (kitValue >= req && kitValue != 0) ? "green" : "red";
                                }
                            }
                            writer.WriteLine($"<td class='{cellClass}'>{cellValue}</td>");
                        }
                    }
                    writer.WriteLine("</tr>");
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
        private async void btnGetMFNs_Click(object sender, EventArgs e)
        {
            //await FetchMFPNsWithDelay();
            await FetchMFPNsForAllRows();
        }
    }
}
