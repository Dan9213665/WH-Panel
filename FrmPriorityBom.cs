﻿using FastMember;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Outlook;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Slicer.Style;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using OfficeOpenXml.Style;
using Seagull.BarTender.Print;
using Seagull.Framework.Utility;
using Seagull.Framework.Yaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using WH_Panel;
using static OpenQA.Selenium.BiDi.Modules.BrowsingContext.Locator;
using static QRCoder.PayloadGenerator;
using static Seagull.Framework.OS.ServiceControlManager;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using static WH_Panel.FrmBOM;
using static WH_Panel.FrmPriorityAPI;
using static WH_Panel.FrmPriorityBom;
using _Application = Microsoft.Office.Interop.Excel._Application;
using Action = System.Action;
using Application = Microsoft.Office.Interop.Excel.Application;
using Button = System.Windows.Forms.Button;
using ComboBox = System.Windows.Forms.ComboBox;
using DataTable = System.Data.DataTable;
using Exception = System.Exception;
using File = System.IO.File;
using Font = System.Drawing.Font;
using GroupBox = System.Windows.Forms.GroupBox;
using Label = System.Windows.Forms.Label;
using Outlook = Microsoft.Office.Interop.Outlook;
using Point = System.Drawing.Point;
using Range = Microsoft.Office.Interop.Excel.Range;
using Rectangle = System.Drawing.Rectangle;
using TextBox = System.Windows.Forms.TextBox;
using Timer = System.Threading.Timer;
using ToolTip = System.Windows.Forms.ToolTip;
namespace WH_Panel
{
    public partial class FrmPriorityBom : Form
    {
        private ToolTip toolTip;
        public string SelectedSerialName { get; set; }
        //private bool isProgrammaticChange = false;
        private List<Serial> originalSerials; // List to store the original work orders
        private AppSettings settings;
        bool isItemAddedToKit = false;
        public string robSerial { get; set; } // Store the selected serial name for later use

        //private List<WarehouseBalance> warehouseBalances;
        // Define this at class level
        private ContextMenuStrip contextMenuSwitchToAlt;
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
            // Initialize the ToolTip and set up the delay for the ToolTip.
            toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;
            // Set up the ToolTip text for the btnGetMFNs button.
            toolTip.SetToolTip(btnGetMFNs, "Click to fetch Manufacturer Part Numbers (MFPNs) or right-click to fetch ALTs.");
            // Handle the CellFormatting event
            dgwBom.CellFormatting += dgwBom_CellFormatting;
            AttachTextBoxEvents(this);

            contextMenuSwitchToAlt = new ContextMenuStrip();
            var switchToAltItem = new ToolStripMenuItem("SWITCH TO ALT");
            switchToAltItem.Click += SwitchToAltItem_Click;
            contextMenuSwitchToAlt.Items.Add(switchToAltItem);

            
        }
        private void FrmPriorityBom_Load(object sender, EventArgs e)
        {
            settings = SettingsManager.LoadSettings();
            if (settings == null)
            {
              
                SafeAppendLog("Failed to load settings.", Color.Red);
                return;
            }
            GetRobWosList();
            // Select the serial name if it is set
            if (!string.IsNullOrEmpty(SelectedSerialName))
            {
                cmbROBxList.SelectedItem = cmbROBxList.Items.Cast<Serial>().FirstOrDefault(s => s.SERIALNAME == SelectedSerialName);
            }
            InitializeTpmMonitoring();
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
        private async void GetRobWosList()
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
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                    string usedUser = ApiHelper.AuthenticateClient(client);
                    // string usedUser = ApiHelper.AuthenticateClient(client);
                    RegisterTransaction(usedUser); // Log this transaction timestamp
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var serials = apiResponse["value"].ToObject<List<Serial>>();
                    // Store the original work orders
                    originalSerials = serials ?? new List<Serial>();
                    // Populate the dropdown with the data
                    cmbROBxList.Items.Clear();
                    foreach (var serial in serials)
                    {
                        // Hide work orders with status "נסגרה" if the checkbox is not checked
                        if ((serial.SERIALSTATUSDES != "נסגרה" && serial.SERIALSTATUSDES != "קיט מלא") || cnkbClosed.Checked)
                        {
                            cmbROBxList.Items.Add(serial);
                        }
                    }
                    lblLoading.BackColor = Color.Green;
                    lblLoading.Text = "Data Loaded";
                    cmbROBxList.DroppedDown = true;
                    // Attach event handler to the checkbox
                    cnkbClosed.CheckedChanged += (s, e) => FilterWorkOrders(serials);
                }
                catch (HttpRequestException ex)
                {
                   
                    SafeAppendLog($"Request error: {ex.Message}");
                  
                }
                catch (Exception ex)
                {
                    
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    
                }
            }
        }
        private void FilterWorkOrders(List<Serial> serials)
        {
            cmbROBxList.Items.Clear();
            foreach (var serial in serials)
            {
                if ((serial.SERIALSTATUSDES != "נסגרה" && serial.SERIALSTATUSDES != "קיט מלא") || cnkbClosed.Checked)
                {
                    cmbROBxList.Items.Add(serial);
                }
            }
        }
        public async void cmbROBxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbROBxList.SelectedItem is Serial selectedSerial)
            {
                isItemAddedToKit = false;
                //dgwBom.Rows.Clear();
                gbxLoadedWo.Text = selectedSerial.SERIALNAME + " status";
                txtbName.Text = selectedSerial.PARTNAME;
                txtbRob.Text = selectedSerial.SERIALNAME;
                txtbRev.Text = $"REV ( {selectedSerial.REVNUM} )";
                txtbQty.Text = selectedSerial.QUANT.ToString();
                txtbStatus.Text = selectedSerial.SERIALSTATUSDES;
                txtbInputIPN.Focus();
                // Load BOM details
                await LoadBomDetails(selectedSerial.SERIALNAME);
                await GetCommentsFromROBxxx();
                robSerial = txtbRob.Text.Trim();
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
                e.Graphics.DrawString(serial.ToString(), e.Font, Brushes.White, e.Bounds);
            }
            else if (serial.SERIALSTATUSDES == "שוחררה")
            {
                e.Graphics.FillRectangle(Brushes.CadetBlue, e.Bounds);
                e.Graphics.DrawString(serial.ToString(), e.Font, Brushes.Black, e.Bounds);
            }
            else if (serial.SERIALSTATUSDES == "ממתין להשלמה")
            {
                e.Graphics.FillRectangle(Brushes.OrangeRed, e.Bounds);
                e.Graphics.DrawString(serial.ToString(), e.Font, Brushes.Black, e.Bounds);
            }
            else if (serial.SERIALSTATUSDES == "נסגרה")
            {
                e.Graphics.FillRectangle(Brushes.Black, e.Bounds);
                e.Graphics.DrawString(serial.ToString(), e.Font, Brushes.Gray, e.Bounds);
            }
            else if (serial.SERIALSTATUSDES == "הרכבה בחוסר")
            {
                e.Graphics.FillRectangle(Brushes.DarkViolet, e.Bounds);
                e.Graphics.DrawString(serial.ToString(), e.Font, Brushes.White, e.Bounds);
            }
            else if (serial.SERIALSTATUSDES == "טרם נשלח קיט")
            {
                e.Graphics.FillRectangle(Brushes.DarkSlateGray, e.Bounds);
                e.Graphics.DrawString(serial.ToString(), e.Font, Brushes.White, e.Bounds);
            }
            else if (serial.SERIALSTATUSDES == "הוקפאה")
            {
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
                e.Graphics.DrawString(serial.ToString(), e.Font, Brushes.White, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(comboBox.BackColor), e.Bounds);
                e.Graphics.DrawString(serial.ToString(), e.Font, Brushes.White, e.Bounds);
            }
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
        public void SelectComboBoxItem(string serialName)
        {
            foreach (var item in cmbROBxList.Items)
            {
                if (item is Serial serial && serial.SERIALNAME == serialName)
                {
                    cmbROBxList.SelectedItem = item;
                    break;
                }
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
            dgwBom.Columns.Add("DELTA", "DELTA");
            dgwBom.Columns.Add("CALC", "CALC");
            dgwBom.Columns.Add("ALT", "ALT");
            dgwBom.Columns.Add("LEFTOVERS", "LEFTOVERS");
            dgwBom.Columns.Add("TRANS", "TRANS");
            dgwBom.Columns.Add("KLINE", "KLINE");
            // Ensure the LEFTOVERS column is sortable
            dgwBom.Columns["LEFTOVERS"].SortMode = DataGridViewColumnSortMode.Automatic;
            dgwBom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private async Task LoadBomDetails(string serialName)
        {
            SafeAppendLog($"Fetching warehouse balances...",Color.Yellow);
            if (dgwBom != null)
            {
                progressBar1.Value = 0;
                progressBar1.Update();
                int completedItems = 0;
                //string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{serialName}'&$expand=TRANSORDER_K_SUBFORM";
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{serialName}'&$expand=TRANSORDER_K_SUBFORM($select=PARTNAME,PARTDES,CQUANT,QUANT,KLINE,TRANS)";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                        string usedUser = ApiHelper.AuthenticateClient(client);
                        // string usedUser = ApiHelper.AuthenticateClient(client);
                        RegisterTransaction(usedUser); // Log this transaction timestamp
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
                            int bomCountFromDB = await FetchIPNcountFromBom(txtbName.Text);
                            SafeAppendLog($"Loading {bomCountFromDB} items from {txtbName.Text} bom", Color.Yellow);
                            dgwBom.Rows.Clear();
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
                            // Populate the DataGridView with the aggregated data
                            foreach (var detail in aggregatedDetails)
                            {
                                // Remove "+0" from CALC if present
                                if (detail.CALC.Contains("+0"))
                                {
                                    detail.CALC = detail.CALC.Replace("+0", "");
                                }
                                // If CALC is "0" or contains a single number, set it to an empty string
                                if (detail.CALC == "0" || !detail.CALC.Contains("+"))
                                {
                                    detail.CALC = "";
                                }
                                dgwBom.Rows.Add(detail.PARTNAME, "", detail.PARTDES, "", detail.QUANT, detail.CQUANT, detail.DELTA, detail.CALC, "", "", detail.TRANS, detail.KLINE);
                                int pbTotal = aggregatedDetails.Count;
                                if (detail.DELTA >= 0)
                                {
                                    completedItems++;
                                }
                                progressBar1.Value = (completedItems * 100) / pbTotal;
                            }
                            // Sort the DataGridView by the DELTA column in descending order
                            dgwBom.Sort(dgwBom.Columns["DELTA"], ListSortDirection.Ascending);
                            // Update the progress label
                            UpdateProgressLabel();
                            progressBar1.Update();
                            txtbInputIPN.PlaceholderText = $"Filter by IPN ({aggregatedDetails.Count})";
                            // Fetch warehouse balances
                            await FetchWarehouseBalances();
                            // Fetch MFPNs in a single API call
                            await FetchMFPNsForAllRowsInSinglePull();

                            await FetchAltsForAllRows();
                        }
                        else
                        {
                            SafeAppendLog("No BOM details found for the selected serial.", Color.Red);
                           
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                
                    }
                    catch (Exception ex)
                    {
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                        
                    }
                }
            }
        }
        private async Task FetchMFPNsForAllRowsInSinglePull()
        {
            SafeAppendLog("Fetching MFPNs for all rows in a single API call...",Color.Yellow);
            // Ensure there are rows in the DataGridView
            if (dgwBom.Rows.Count == 0)
            {
                SafeAppendLog("No rows found in the DataGridView to fetch MFPNs.");
                return;
            }
            // Get the warehouse name from the first 3 characters of the first PARTNAME
            string selectedWarehouse = dgwBom.Rows[0].Cells["PARTNAME"].Value?.ToString()?.Substring(0, 3);
            if (string.IsNullOrEmpty(selectedWarehouse))
            {
                SafeAppendLog("Unable to determine the warehouse from the first PARTNAME.");
                return;
            }
            // Construct the API URL using the warehouse name
            //string avlUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PARTNAME eq '{selectedWarehouse}_*'";

            string avlUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PARTNAME eq '{selectedWarehouse}_*'&$select=PARTNAME,MNFPARTNAME,PARTDES,MNFNAME,MNFDES";


            // SafeAppendLog($"API URL: {avlUrl}");
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                    string usedUser = ApiHelper.AuthenticateClient(client);
                    RegisterTransaction(usedUser); // Log this transaction timestamp
                    // string usedUser = ApiHelper.AuthenticateClient(client);
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(avlUrl);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // SafeAppendLog($"API Response: {responseBody}");
                    // Parse the JSON response
                    var apiResponseWrapper = JsonConvert.DeserializeObject<ApiMFPNResponseWrapper>(responseBody);
                    // Validate the API response
                    if (apiResponseWrapper?.Value == null || !apiResponseWrapper.Value.Any())
                    {
                        SafeAppendLog("No data returned from the API.");
                        return;
                    }
                    // Map the MFPNs to the DataGridView rows
                    foreach (DataGridViewRow row in dgwBom.Rows)
                    {
                        if (row.Cells["PARTNAME"].Value != null)
                        {
                            string partName = row.Cells["PARTNAME"].Value.ToString();
                            var matchingPart = apiResponseWrapper.Value.FirstOrDefault(p => p.PARTNAME == partName);
                            if (matchingPart != null)
                            {
                                row.Cells["MFPN"].Value = matchingPart.MNFPARTNAME;
                            }
                            else
                            {
                                SafeAppendLog($"No match found for PARTNAME: {partName}");
                            }
                        }
                    }
                    SafeAppendLog("MFPN fetching completed.",Color.LimeGreen);
                }
                catch (HttpRequestException ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}",Color.Red);
                   
                }
                catch (Exception ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                }
            }
            dgwBom.Update();
        }
        public class ApiMFPNResponseWrapper
        {
            [JsonProperty("@odata.context")]
            public string ODataContext { get; set; }
            [JsonProperty("value")]
            public List<ApiMFPNResponse> Value { get; set; }
        }
        public class ApiMFPNResponse
        {
            public string PARTNAME { get; set; }
            public string MNFPARTNAME { get; set; }
            public string PARTDES { get; set; }
            public string MNFNAME { get; set; }
            public string MNFDES { get; set; }
        }
        private async Task<int> FetchIPNcountFromBom(string partName)
        {
            SafeAppendLog($"Fetching IPN count for {partName}",Color.Yellow);
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PART?$filter=PARTNAME eq '{partName}'&$expand=PARTARC_SUBFORM($select=SONNAME)";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                    string usedUser = ApiHelper.AuthenticateClient(client);
                    // string usedUser = ApiHelper.AuthenticateClient(client);
                    RegisterTransaction(usedUser); // Log this transaction timestamp
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var part = apiResponse["value"].FirstOrDefault();
                    if (part != null)
                    {
                        var partArcSubform = part["PARTARC_SUBFORM"] as JArray;
                        return partArcSubform?.Count ?? 0;
                    }
                    return 0;
                }
                catch (HttpRequestException ex)
                {
                    
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                 
                    return 0;
                }
                catch (Exception ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    return 0;
                }
            }
        }
        private void UpdateProgressLabel()
        {
            if (dgwBom != null)
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
        }
        private async Task FetchWarehouseBalances()
        {
            // Create a list to store the fetched warehouse balances
            List<WarehouseBalance> warehouseBalances = new List<WarehouseBalance>();
            // Get the unique part names from the DataGridView
            var partNames = dgwBom.Rows.Cast<DataGridViewRow>()
                .Where(row => row.Cells["PARTNAME"].Value != null)
                .Select(row => row.Cells["PARTNAME"].Value.ToString())
                .Distinct()
                .ToList();
            if (partNames.Count == 0)
            {
                return;
            }
            // Construct the filter string for the API call
            string partNamesFilter = string.Join(" or ", partNames.Select(p => $"PARTNAME eq '{p}'"));
            string warehouseName = partNames.First().Substring(0, 3); // Assuming all parts belong to the same warehouse
            //string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{warehouseName}'&$expand=WARHSBAL_SUBFORM";
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{warehouseName}'&$expand=WARHSBAL_SUBFORM($select=PARTNAME,TBALANCE)";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                    string usedUser = ApiHelper.AuthenticateClient(client);
                    // string usedUser = ApiHelper.AuthenticateClient(client);
                    RegisterTransaction(usedUser); // Log this transaction timestamp
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
                        warehouseBalances = warehouse["WARHSBAL_SUBFORM"].ToObject<List<WarehouseBalance>>();
                        SafeAppendLog($"Fetched {warehouseBalances.Count} warehouse balances for warehouse {warehouseName}", Color.LimeGreen);
                    }
                }
                catch (HttpRequestException ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                }
                catch (Exception ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}",Color.Red);
                }
            }
            // Update the DataGridView rows using the fetched data
            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                if (row.Cells["PARTNAME"].Value != null)
                {
                    string partName = row.Cells["PARTNAME"].Value.ToString();
                    var balance = warehouseBalances.FirstOrDefault(b => b.PARTNAME == partName);
                    if (balance != null)
                    {
                        row.Cells["TBALANCE"].Value = balance.TBALANCE;
                    }
                    else
                    {
                        row.Cells["TBALANCE"].Value = 0;
                    }
                    // Calculate the LEFTOVERS for the current row
                    int delta = Convert.ToInt32(row.Cells["DELTA"].Value);
                    int whQuantity = row.Cells["TBALANCE"].Value != null ? Convert.ToInt32(row.Cells["TBALANCE"].Value) : 0;
                    int kitQuantity = row.Cells["QUANT"].Value != null ? Convert.ToInt32(row.Cells["QUANT"].Value) : 0;
                    int requiredQuantity = row.Cells["CQUANT"].Value != null ? Convert.ToInt32(row.Cells["CQUANT"].Value) : 0;
                    int leftovers = (whQuantity + kitQuantity) - requiredQuantity;
                    // Update the LEFTOVERS column in the DataGridView
                    row.Cells["LEFTOVERS"].Value = leftovers;
                }
            }
            UpdateSimulationLabel();
        }
        private void UpdateSimulationLabel()
        {
            int totalItems = dgwBom.Rows.Count;

            int coveredItems = dgwBom.Rows.Cast<DataGridViewRow>().Count(row =>
            {
                int delta = Convert.ToInt32(row.Cells["DELTA"].Value);
                int whQuantity = row.Cells["TBALANCE"].Value != null ? Convert.ToInt32(row.Cells["TBALANCE"].Value) : 0;
                int kitQuantity = row.Cells["QUANT"].Value != null ? Convert.ToInt32(row.Cells["QUANT"].Value) : 0;
                int requiredQuantity = row.Cells["CQUANT"].Value != null ? Convert.ToInt32(row.Cells["CQUANT"].Value) : 0;
                int leftovers = (whQuantity + kitQuantity) - requiredQuantity;
                return (leftovers >= 0);
            });
            if (totalItems > 0 && coveredItems > 0)
            {
                int simPercentage = (coveredItems * 100) / totalItems;
                lblSim.Text = $"{coveredItems} of {totalItems} IPNs simulation ({simPercentage}%)";
            }
            else
            {
                lblSim.Text = "0 / 0 items covered in simulation (0%)";
            }
        }
        private async Task FetchMFPNForRow(DataGridViewRow row)
        {
            //MessageBox.Show("Test");
            if (row.Cells["PARTNAME"].Value != null && (row.Cells["MFPN"].Value.ToString() == string.Empty || row.Cells["MFPN"].Value == null))
            //if (row.Cells["PARTNAME"].Value != null) //&& (string.IsNullOrEmpty(row.Cells["MFPN"].Value?.ToString())|| row.Cells["MFPN"].Value=="")
            {
                string partName = row.Cells["PARTNAME"].Value.ToString();
                //MessageBox.Show("partName:"+partName);
                //string partUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PARTNAME eq '{partName}'";
                string partUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PARTMNFONE?$filter=PARTNAME eq '{partName}'&$select=PARTNAME,MNFPARTNAME";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                        string usedUser = ApiHelper.AuthenticateClient(client);

                        // Register the transaction for that user immediately
                        RegisterTransaction(usedUser);


                        // string usedUser = ApiHelper.AuthenticateClient(client);
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
                            //dgwBom.Refresh();
                            //MessageBox.Show($"Updated MFPN for {partName} to {part.MNFPARTNAME}");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                       
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                     
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                      
                    }
                }
            }
            await Task.Delay(200); // 100 milliseconds delay
        }
        private async Task FetchMFPNsForAllRows()
        {
            SafeAppendLog("Fetching MFPNs for all rows",Color.Yellow);
            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                await FetchMFPNForRow(row);
            }
            SafeAppendLog("MFPN fetching completed",Color.LimeGreen);
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
                else if (e.KeyCode != Keys.Tab && e.KeyCode != Keys.Escape && sender as TextBox != txtbInputIPN)
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
            if (textBox != txtbINPUTqty)
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
                    if (deltaValue >= 10)
                    {
                        e.CellStyle.BackColor = Color.Green;
                    }
                    else if (deltaValue >= 0 && deltaValue < 10)
                    {
                        e.CellStyle.BackColor = Color.OrangeRed;
                    }
                    else if (deltaValue < 0)
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
            else if (dgwBom.Columns[e.ColumnIndex].Name == "LEFTOVERS")
            {
                if (e.Value != null && int.TryParse(e.Value.ToString(), out int leftoversValue))
                {
                    if (leftoversValue < 0)
                    {
                        e.CellStyle.BackColor = Color.IndianRed;
                    }
                }
            }
        }

        // Define these once (class-level or at the top of your method)
        private static readonly HashSet<string> ExcludedDocDescriptions = new HashSet<string>
{
    "ספירות מלאי",
    "קיזוז אוטומטי"
};

        private static readonly HashSet<string> ExcludedSupCustNames = new HashSet<string>
{
    "200052",
    "200048","200085","200009"
};


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
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                        string usedUser = ApiHelper.AuthenticateClient(client);
                        // string usedUser = ApiHelper.AuthenticateClient(client);
                        RegisterTransaction(usedUser); // Log this transaction timestamp
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
                                HeaderText = "Source_Req",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "SUPCUSTNAME"
                            };
                            var DocBOOKNUMColumn = new DataGridViewTextBoxColumn
                            {
                                DataPropertyName = "BOOKNUM",
                                HeaderText = "Client`s Document",
                                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                                Name = "BOOKNUM"
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
                        DocBOOKNUMColumn,
                        tQuantColumn,
                        PackColumn
                        //UDateColumn
                            });
                            dgwIPNmoves.Rows.Clear();
                            dgwINSTOCK.Rows.Clear();

                            var fetchTasks = new List<Task>();

                            foreach (var logPart in logPartApiResponse.value)
                            {
                                foreach (var trans in logPart.PARTTRANSLAST2_SUBFORM)
                                {
                              
                                    // Inside your loop:
                                    if (!ExcludedDocDescriptions.Contains(trans.DOCDES) &&
                                        !ExcludedSupCustNames.Contains(trans.SUPCUSTNAME))
                                    {
                                        var rowIndex = dgwIPNmoves.Rows.Add("", trans.LOGDOCNO, trans.DOCDES, trans.SUPCUSTNAME, "", trans.TQUANT, "", "");
                                        var row = dgwIPNmoves.Rows[rowIndex];
                                        var fetchTask = FetchAndSetPackCodeAndUDateAsync(row, trans.LOGDOCNO, partName, (int)trans.TQUANT);
                                        fetchTasks.Add(fetchTask);
                                        await Task.Delay(100); // Optional delay
                                    }

                                }
                            }
                            gbxIPNstockMovements.Text = $"Stock Movements for {partName}";
                            ColorTheRows(dgwIPNmoves);
                            SortIPNMovesByDate();
                            // Fetch MFPN for the selected row
                            //await FetchMFPNForRow(selectedRow);
                            //await FetchAltForRow(selectedRow);


                            // Wait until all UDATE fetching completes
                            await Task.WhenAll(fetchTasks);

                            await LoadDataAndFilterInStock();
                            
                        }
                        else
                        {
                            //MessageBox.Show("No stock movements found for the selected part.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                            SafeAppendLog("No stock movements found for the selected part.", Color.Red);
                    
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        //MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       
                        SafeAppendLog($"Request error: {ex.Message}",Color.Red);
                       
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      
                        SafeAppendLog($"Request error: {ex.Message} ", Color.Red);
                        
                    }
                }
            }
        }


        private async Task FetchAltsForAllRows()
        {
            SafeAppendLog("Fetching alts for all rows in batches of 25...", Color.Yellow);

            // Get all rows that need ALT fetching
            var rowsToProcess = dgwBom.Rows.Cast<DataGridViewRow>()
                .Where(row => row.Cells["PARTNAME"].Value != null &&
                              (row.Cells["ALT"].Value == null || string.IsNullOrEmpty(row.Cells["ALT"].Value.ToString())))
                .ToList();

            // Process rows in batches of 25
            const int batchSize = 25;
            for (int i = 0; i < rowsToProcess.Count; i += batchSize)
            {
                var batch = rowsToProcess.Skip(i).Take(batchSize).ToList();
                await FetchAltsForBatch(batch);
            }

            SafeAppendLog("ALTs fetching complete.", Color.LimeGreen);
        }

        private async Task FetchAltsForBatch(List<DataGridViewRow> batch)
        {
            // Construct the filter for the batch
            var partNames = batch
                .Select(row => $"PARTNAME eq '{row.Cells["PARTNAME"].Value}'")
                .ToList();
            string filter = string.Join(" or ", partNames);

            // Construct the API URL
            //string batchUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PART?$filter={filter}&$expand=PARTALT_SUBFORM";
            string batchUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PART?$filter={filter}&$select=PARTNAME&$expand=PARTALT_SUBFORM($select=ALTNAME)";



            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                    string usedUser = ApiHelper.AuthenticateClient(client);
                    // string usedUser = ApiHelper.AuthenticateClient(client);
                    RegisterTransaction(usedUser); // Log this transaction timestamp
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(batchUrl);
                    response.EnsureSuccessStatusCode();

                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);

                    // Map the ALT data to the corresponding rows
                    if (apiResponse["value"] != null && apiResponse["value"].Any())
                    {
                        foreach (var part in apiResponse["value"])
                        {
                            string partName = part["PARTNAME"]?.ToString();
                            var partAltSubform = part["PARTALT_SUBFORM"]?.FirstOrDefault();
                            if (partAltSubform != null)
                            {
                                string altName = partAltSubform["ALTNAME"]?.ToString();

                                // Find the corresponding row and update the ALT column
                                var matchingRow = batch.FirstOrDefault(row => row.Cells["PARTNAME"].Value.ToString() == partName);
                                if (matchingRow != null)
                                {
                                    matchingRow.Cells["ALT"].Value = altName;
                                }
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                 
                }
                catch (Exception ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                  
                }
            }

            // Optional: Add a delay between batches to avoid overwhelming the server
            await Task.Delay(1);
        }

        private async Task FetchAltForRow(DataGridViewRow row)
        {
            if (row.Cells["PARTNAME"].Value != null && (row.Cells["ALT"].Value.ToString() == string.Empty || row.Cells["ALT"].Value == null))
            {
                string partName = row.Cells["PARTNAME"].Value.ToString();
                //string partUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PART?$filter=PARTNAME eq '{partName}'&$expand=PARTALT_SUBFORM";
                string partUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PART?$filter= PARTNAME eq '{partName}'&$expand=PARTALT_SUBFORM($select=ALTNAME)";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                        // Make the HTTP GET request for part details
                        HttpResponseMessage partResponse = await client.GetAsync(partUrl);
                        partResponse.EnsureSuccessStatusCode();
                        // Read the response content
                        string partResponseBody = await partResponse.Content.ReadAsStringAsync();
                        // Parse the JSON response
                        var partApiResponse = JsonConvert.DeserializeObject<JObject>(partResponseBody);
                        // Check if the response contains any data
                        if (partApiResponse["value"] != null && partApiResponse["value"].Any())
                        {
                            var part = partApiResponse["value"].FirstOrDefault();
                            var partAltSubform = part?["PARTALT_SUBFORM"]?.FirstOrDefault();
                            if (partAltSubform != null)
                            {
                                string altName = partAltSubform["ALTNAME"]?.ToString();
                                // Directly update the DataGridView cell
                                row.Cells["ALT"].Value = altName;
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    }
                    catch (Exception ex)
                    {
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    }
                }
            }
            await Task.Delay(300); // 300 milliseconds delay
        }
        private void SortIPNMovesByDate()
        {
            if (dgwIPNmoves.Columns["UDATE"] != null)
            {
                dgwIPNmoves.Sort(dgwIPNmoves.Columns["UDATE"], ListSortDirection.Descending);
            }
        }
        private async Task FetchAndSetPackCodeAndUDateAsync(DataGridViewRow row, string logDocNo, string partName, int quant)
        {
            var results = await FetchPackCodeAsync(logDocNo, partName, quant);
            foreach (var result in results)
            {
                if (result.PackCode != null)
                {
                    row.Cells["PACK"].Value = result.PackCode;
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
            SortIPNMovesByDate();
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
            else if (logDocNo.StartsWith("IC"))
            {
                url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_C?$filter=DOCNO eq '{logDocNo}'";
            }
            else if (logDocNo.StartsWith("SH"))
            {
                url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_D?$filter=DOCNO eq '{logDocNo}'";
            }
            else if (logDocNo.StartsWith("WR"))
            {
                url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_T?$filter=DOCNO eq '{logDocNo}'";
            }
            else if (logDocNo.StartsWith("ROB"))
            {
                // Handle ROB documents
                url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{logDocNo}'";
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
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                    string usedUser = ApiHelper.AuthenticateClient(client);
                    RegisterTransaction(usedUser); // Log this transaction timestamp
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
                    else if (logDocNo.StartsWith("IC"))
                    {
                        // Handle IC document logic
                        string date = document["UDATE"]?.ToString();
                        results.Add((null, null, date));
                    }
                    else if (logDocNo.StartsWith("SH"))
                    {
                        // Handle IC document logic
                        string date = document["UDATE"]?.ToString();
                        results.Add((null, null, date));
                    }
                    else if (logDocNo.StartsWith("WR"))
                    {
                        // Handle IC document logic
                        string date = document["UDATE"]?.ToString();
                        results.Add((null, null, date));
                    }
                    else if (logDocNo.StartsWith("GR"))
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
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    return new List<(string PackCode, string BookNum, string Date)>();
                }
                catch (Exception ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    return new List<(string PackCode, string BookNum, string Date)>();
                }
            }
        }
        public async Task<string> FetchUDateAsync(string docNo)
        {
            string uDate = null;
            // Log the document number for debugging
            //txtLog.SelectionColor = Color.Blue; // Set the color to blue
            //txtLog.AppendText($"Document Number: '{docNo}'");
            //txtLog.ScrollToCaret();
            if (docNo.StartsWith("ROB"))
            {
                // Fetch UDATE from SERIAL
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{docNo}'&$select=UDATE";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                        string usedUser = ApiHelper.AuthenticateClient(client);
                        // string usedUser = ApiHelper.AuthenticateClient(client);
                        RegisterTransaction(usedUser); // Log this transaction timestamp
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
                            //txtLog.AppendText($"Data for SERIALNAME: {serial}");
                            uDate = serial["UDATE"]?.ToString();
                            if (uDate == null)
                            {
                                //txtLog.SelectionColor = Color.Red; // Set the color to red
                                //txtLog.AppendText($"UDATE is null for SERIALNAME: {docNo}");
                                //txtLog.ScrollToCaret();
                            }
                        }
                        else
                        {
                            //txtLog.SelectionColor = Color.Red; // Set the color to red
                            //txtLog.AppendText($"No serial found for SERIALNAME: {docNo}");
                            //txtLog.ScrollToCaret();
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    }
                    catch (Exception ex)
                    {
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    }
                }
            }
            else if (docNo.StartsWith("GR"))
            {
                // Fetch UDATE from DOCUMENTS_P
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_P?$filter=DOCNO eq '{docNo}'&$select=UDATE";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                        string usedUser = ApiHelper.AuthenticateClient(client);
                        // string usedUser = ApiHelper.AuthenticateClient(client);
                        RegisterTransaction(usedUser); // Log this transaction timestamp
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
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    }
                    catch (Exception ex)
                    {
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    }
                }
            }
            else if (docNo.StartsWith("IC"))
            {
                // Fetch UDATE from DOCUMENTS_P
                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_C?$filter=DOCNO eq '{docNo}'&$select=UDATE";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers if needed
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // Set the Authorization header
                        //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                        string usedUser = ApiHelper.AuthenticateClient(client);
                        // string usedUser = ApiHelper.AuthenticateClient(client);
                        RegisterTransaction(usedUser); // Log this transaction timestamp
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
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    }
                    catch (Exception ex)
                    {
                        SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                    }
                }
            }
            else
            {
                // Handle other document types if needed
                //txtLog.SelectionColor = Color.Orange; // Set the color to orange
                //txtLog.AppendText($"Unhandled document type for DOCNO: {docNo}");
                //txtLog.ScrollToCaret();
            }
            return uDate;
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
                            else if (docDesValue.Contains("ללקוח"))
                            {
                                cell.Style.BackColor = Color.DarkViolet;
                            }
                        }
                    }
                }
            }
        }
        private async void txtbInputIPN_KeyDown(object sender, KeyEventArgs e)
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
                    //ClearFilters();
                    //AutoClosingMessageBox.Show($"{filterText} NOT needed anymore!", 2000, Color.Orange);
                    MessageBox.Show($"{filterText} NOT needed anymore!");
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
                    AutoClosingMessageBox.Show($"{filterText} NOT FOUND!", 3000, Color.Red); // Show message for 2 seconds
                }
            }
            else
            {
                //
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
            else
            {
                //
            }
        }




        ////private const int maxTpm = 90; // Max allowed transactions per minute

        //private bool CanProceedWithTransaction()
        //{
        //    DateTime now = DateTime.Now;

        //    // Remove timestamps older than 60 seconds
        //    while (transactionTimestamps.Count > 0 && (now - transactionTimestamps.Peek()).TotalSeconds > 60)
        //    {
        //        transactionTimestamps.Dequeue();
        //    }

        //    return transactionTimestamps.Count < maxTpm;
        //}



        //// Add this to your form
        //private const int maxTpm = 90;            // UI display limit
        //private const int maxTpmGlobal = 100;     // Global hard limit

        //private readonly Queue<DateTime> transactionTimestamps = new();
        //private readonly Dictionary<string, Queue<DateTime>> userTransactionTimestamps = new();

        //private System.Windows.Forms.Timer tpmUpdateTimer;

        //// Call this once in your form constructor or Load event
        //private void InitializeTpmMonitoring()
        //{
        //    tpmUpdateTimer = new System.Windows.Forms.Timer();
        //    tpmUpdateTimer.Interval = 1000; // 1 second
        //    tpmUpdateTimer.Tick += TpmUpdateTimer_Tick;
        //    tpmUpdateTimer.Start();
        //}

        //// Register transaction per request
        //private void RegisterTransaction(string apiUsername)
        //{
        //    DateTime now = DateTime.Now;

        //    // Global queue
        //    transactionTimestamps.Enqueue(now);

        //    // User-specific queue
        //    if (!userTransactionTimestamps.ContainsKey(apiUsername))
        //        userTransactionTimestamps[apiUsername] = new Queue<DateTime>();

        //    userTransactionTimestamps[apiUsername].Enqueue(now);

        //    // Immediate cleanup
        //    CleanOldTransactions();
        //    UpdateTpmIndicator();
        //}

        //// Timer tick: background cleanup and UI update
        //private void TpmUpdateTimer_Tick(object sender, EventArgs e)
        //{
        //    CleanOldTransactions();
        //    UpdateTpmIndicator();
        //}

        //// Removes outdated (older than 60 sec) timestamps
        //private void CleanOldTransactions()
        //{
        //    DateTime now = DateTime.Now;

        //    // Global
        //    while (transactionTimestamps.Count > 0 && (now - transactionTimestamps.Peek()).TotalSeconds > 60)
        //        transactionTimestamps.Dequeue();

        //    // Per user
        //    foreach (var userQueue in userTransactionTimestamps.Values)
        //    {
        //        while (userQueue.Count > 0 && (now - userQueue.Peek()).TotalSeconds > 60)
        //            userQueue.Dequeue();
        //    }
        //}

        //// Update progress bar visual
        //private void UpdateTpmIndicator()
        //{
        //    int currentTpm = transactionTimestamps.Count;

        //    tpmProgressBar.Maximum = maxTpm;
        //    tpmProgressBar.Value = Math.Min(currentTpm, maxTpm);
        //    //tpmProgressBar.ToolTipText = $"Current TPM: {currentTpm} / {maxTpm}";

        //    // Set color based on threshold
        //    if (currentTpm < maxTpm * 0.7)
        //        tpmProgressBar.ForeColor = Color.Green;
        //    else if (currentTpm < maxTpm * 0.9)
        //        tpmProgressBar.ForeColor = Color.Orange;
        //    else
        //        tpmProgressBar.ForeColor = Color.Red;
        //}

        // Constants
        private const int maxTpm = 200;            // Soft UI limit
        private const int maxTpmGlobal = 200;     // Global hard limit
        private readonly Queue<DateTime> transactionTimestamps = new();
        private readonly Dictionary<string, Queue<DateTime>> userTransactionTimestamps = new();

        // Timer for periodic cleanup and UI update
        private System.Windows.Forms.Timer tpmUpdateTimer;

        // Initialize in Form_Load or constructor
        private void InitializeTpmMonitoring()
        {
            tpmUpdateTimer = new System.Windows.Forms.Timer();
            tpmUpdateTimer.Interval = 1000; // 1 second
            tpmUpdateTimer.Tick += TpmUpdateTimer_Tick;
            tpmUpdateTimer.Start();
        }

        // Call when API request is made
        private void RegisterTransaction(string apiUsername)
        {
            DateTime now = DateTime.Now;

            // Global
            transactionTimestamps.Enqueue(now);

            // Per user
            if (!userTransactionTimestamps.ContainsKey(apiUsername))
                userTransactionTimestamps[apiUsername] = new Queue<DateTime>();

            userTransactionTimestamps[apiUsername].Enqueue(now);

            CleanOldTransactions();
            int currentTpm = transactionTimestamps.Count;
            UpdateTpmIndicator(currentTpm);

            if (transactionTimestamps.Count >= maxTpmGlobal)
            {
                ShowTpmLimitWarning();
            }
        }

        // Timer tick event
        private void TpmUpdateTimer_Tick(object sender, EventArgs e)
        {
            CleanOldTransactions();
            int currentTpm = transactionTimestamps.Count;
            UpdateTpmIndicator(currentTpm);
        }

        // Clean old timestamps (older than 60s)
        private void CleanOldTransactions()
        {
            DateTime now = DateTime.Now;

            // Global
            while (transactionTimestamps.Count > 0 && (now - transactionTimestamps.Peek()).TotalSeconds > 60)
                transactionTimestamps.Dequeue();

            // Per user
            foreach (var key in userTransactionTimestamps.Keys.ToList())
            {
                var queue = userTransactionTimestamps[key];
                while (queue.Count > 0 && (now - queue.Peek()).TotalSeconds > 60)
                    queue.Dequeue();
            }
        }
        private ToolTip toolTip1 = new ToolTip();

        private void UpdateTpmIndicator(int currentTpm)
        {
            int maxWidth = progressBarContainer.Width;
            int fillWidth = (int)(maxWidth * Math.Min(currentTpm, maxTpm) / (double)maxTpm);

            progressBarFill.Width = fillWidth;

            if (currentTpm < 150)
                progressBarFill.BackColor = Color.Green;
            else if (currentTpm < 180)
                progressBarFill.BackColor = Color.Orange;
            else
                progressBarFill.BackColor = Color.Red;

            int cooldownSeconds = GetCooldownSeconds(maxTpmGlobal);

            string cooldownText = cooldownSeconds > 0 ? $"Cooldown: {cooldownSeconds}s" : "Ready";

            toolTip1.SetToolTip(progressBarContainer, $"TPM: {currentTpm} / {maxTpm} (Global limit: {maxTpmGlobal})\n{cooldownText}");

            lblTpmText.Text = $"{currentTpm} / {maxTpmGlobal} {(cooldownSeconds > 0 ? $" - Cooldown: {cooldownSeconds}s" : "")}";
        }


        private int GetCooldownSeconds(int maxLimit)
        {
            if (transactionTimestamps.Count < maxLimit)
                return 0; // No cooldown needed

            DateTime now = DateTime.Now;
            int itemsToRemove = transactionTimestamps.Count - maxLimit + 1; // +1 to get below the limit

            // Oldest timestamp that needs to "expire" for count to go below limit
            DateTime targetTimestamp = transactionTimestamps.ElementAt(itemsToRemove - 1);

            int cooldown = (int)(60 - (now - targetTimestamp).TotalSeconds);
            return cooldown > 0 ? cooldown : 0;
        }


        // Optional: Show visual warning when limit breached
        private void ShowTpmLimitWarning()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ShowTpmLimitWarning));
                return;
            }

            SafeAppendLog("Transaction rate limit exceeded! TPM has reached the global limit per user.",Color.Red);
        }

        // Check if allowed before sending transaction
        private bool CanProceedWithTransaction()
        {
            CleanOldTransactions(); // Always keep queue clean
            return transactionTimestamps.Count < maxTpmGlobal;
        }


        // Updated event handler:
        private async void txtbINPUTqty_KeyDown(object sender, KeyEventArgs e)
        {
            // Count only the visible rows
            int visibleRowCount = dgwBom.Rows.Cast<DataGridViewRow>().Count(row => row.Visible);
            if (visibleRowCount == 1)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (!CanProceedWithTransaction())
                    {
                        MessageBox.Show("Transaction rate limit exceeded. Please slow down.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (int.TryParse(txtbINPUTqty.Text, out int qty) && qty > 0)
                    {
                        var filteredRow = dgwBom.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Visible);
                        if (filteredRow != null)
                        {
                            string wh = filteredRow.Cells["PARTNAME"].Value.ToString().Substring(0, 3);
                            string partName = filteredRow.Cells["PARTNAME"].Value.ToString();
                            string serialName = txtbRob.Text; // Assuming txtbRob contains the SERIALNAME
                            int cQuant = int.Parse(filteredRow.Cells["CQUANT"].Value.ToString()); // Get the CQUANT value
                            int inKit = int.Parse(filteredRow.Cells["QUANT"].Value.ToString()); // Get the QUANT value
                            int neededQty = cQuant - inKit;

                            await AddItemToKit(partName, serialName, neededQty, qty, filteredRow, wh);

                            

                            txtbINPUTqty.Clear();
                            txtbInputIPN.Clear();
                            txtbInputIPN.Focus();

                            // Update the progress label
                            UpdateProgressLabel();
                            UpdateSimulationLabel();
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



        private async Task AddItemToKit(string partName, string serialName, int cQuant, int qty, DataGridViewRow filteredRow, string wh)
        {
            // Check quantity availability in the warehouse
            string checkUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{wh}'&$expand=WARHSBAL_SUBFORM($filter=PARTNAME eq '{partName}')";
            int availableQty = 0;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                    RegisterTransaction("Api2"); // Log this transaction timestamp


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
                    //MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
             
                    SafeAppendLog($"Request error: {ex.Message} ", Color.Red);
                   
                    return;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
              
                    SafeAppendLog($"Request error: {ex.Message} ", Color.Red);
               
                    return;
                }
            }
            if (availableQty < qty)
            {
                MessageBox.Show("Insufficient quantity available in the warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Retrieve the TRANSORDER_K_SUBFORM data
            string getUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL('{serialName}')/TRANSORDER_K_SUBFORM?$filter=PARTNAME eq '{partName}'&$select=PARTNAME,CQUANT,KLINE,PACKCODE";
            int kline = 0;
            string package = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    RegisterTransaction("Api2"); // Log this transaction timestamp
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
                    //MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   
                    SafeAppendLog($"Request error: {ex.Message} ", Color.Red);
                    
                    return;
                }
                catch (Exception ex)
                {
                  
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                 
                    if (ex.Message.Contains("429"))
                    {
                        MessageBox.Show("! נא להמתין דקה !  חריגת כמות קריאות ליחידת זמן ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
            }
            string partNameWH = dgwBom.Rows[0].Cells["PARTNAME"].Value.ToString();
            //string warehouseName = partNameWH.Substring(0, 3); // Get the first 3 characters of the PARTNAME
            // Construct the PATCH request URL
            string patchUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL('{serialName}')/TRANSORDER_K_SUBFORM(TYPE='K',KLINE={kline})";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    RegisterTransaction("Api2"); // Log this transaction timestamp
                    // Create the JSON payload for the PATCH request
                    var payload = new
                    {
                        QUANT = qty,
                        WARHSNAME = wh, // warehouse to make transfer from
                        TOWARHSNAME = "Flr",
                        PACKCODE = package
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    // Make the PATCH request
                    HttpResponseMessage patchResponse = await client.PatchAsync(patchUrl, content);
                    string patchResponseBody = await patchResponse.Content.ReadAsStringAsync();
                    patchResponse.EnsureSuccessStatusCode();
                    AutoClosingMessageBox.Show($"{partName} - {qty} PCS moved to {serialName}", 1000, Color.Green); // Show message for 2 seconds
                    isItemAddedToKit = true;
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
                    int prevQty = int.Parse(filteredRow.Cells["QUANT"].Value.ToString());
                    string currentCALC = filteredRow.Cells["CALC"].Value?.ToString();
                    // Update the DataGridView row
                    filteredRow.Cells["QUANT"].Value = prevQty + qty;
                    int currentINkit = int.Parse(filteredRow.Cells["QUANT"].Value.ToString());
                    int requiredQty = int.Parse(filteredRow.Cells["CQUANT"].Value.ToString());
                    filteredRow.Cells["DELTA"].Value = currentINkit - requiredQty;
                    // Update the CALC field
                    if (string.IsNullOrEmpty(currentCALC) && prevQty == 0)
                    {
                        // Do nothing if CALC is empty and QUANT is 0
                    }
                    else if (!string.IsNullOrEmpty(currentCALC) && currentINkit != 0)
                    {
                        // If CALC is not empty and QUANT is not 0, update CALC to include the new quantity
                        filteredRow.Cells["CALC"].Value = $"{currentCALC}+{qty}";
                    }
                    else if (string.IsNullOrEmpty(currentCALC) && currentINkit != 0)
                    {
                        // If CALC is empty but QUANT is not 0, initialize CALC with the current and new quantities
                        filteredRow.Cells["CALC"].Value = $"{prevQty}+{qty}";
                    }
                    // Calculate the LEFTOVERS for the current row
                    int delta = Convert.ToInt32(filteredRow.Cells["DELTA"].Value);
                    int whQuantity = filteredRow.Cells["TBALANCE"].Value != null ? Convert.ToInt32(filteredRow.Cells["TBALANCE"].Value) : 0;
                    int kitQuantity = filteredRow.Cells["QUANT"].Value != null ? Convert.ToInt32(filteredRow.Cells["QUANT"].Value) : 0;
                    int leftovers = (whQuantity + kitQuantity) - requiredQty;
                    // Update the LEFTOVERS column in the DataGridView
                    filteredRow.Cells["LEFTOVERS"].Value = leftovers;
                    txtbINPUTqty.Clear();
                    txtbINPUTqty.Focus();
                }
                catch (HttpRequestException ex)
                {
                    
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                  
                    txtbLog.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                 
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                
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
                        case DialogResult.Continue:
                            dgwBom.Sort(dgwBom.Columns["DELTA"], ListSortDirection.Ascending);
                            SendEmail();
                            break;
                    }
                    if (result != DialogResult.Ignore && result != DialogResult.Abort && result != DialogResult.Cancel && result != DialogResult.Continue)
                    {
                        GenerateHTMLkitBoxLabel(copiesToPrint);
                    }
                    else
                    {
                                               // If the user clicked Cancel, do nothing
                        return;
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
            // Build HTML table from DataGridView data
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.Append("<html><head>");
            htmlTable.Append("<style>");
            htmlTable.Append("table { border-collapse: collapse; }");
            htmlTable.Append("th, td { padding: 5px; }");
            htmlTable.Append(".green { background-color: green; color: white; }");
            htmlTable.Append(".red { background-color: indianred; color: white; }");
            htmlTable.Append(".center { text-align: center; }");
            htmlTable.Append("</style>");
            htmlTable.Append("</head><body>");
            // Add the additional table with text from txtbRob, txtbName, txtbQty, txtbStatus, and lblProgress
            htmlTable.Append("<table border='1' style='border-collapse:collapse; margin-bottom: 20px;'>");
            htmlTable.Append("<tr><th>Rob</th><th>Name</th><th>Qty</th><th>Status</th><th>Progress</th><th>Comments</th></tr>");
            string stat = string.Empty;
            if (lblProgress.Text.Contains("100%"))
            {
                stat = "קיט מלא";
            }
            else
            {
                stat = txtbStatus.Text;
            }
            htmlTable.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                txtbRob.Text, txtbName.Text, txtbQty.Text, stat, lblProgress.Text, rtxtbComments.Text);
            htmlTable.Append("</table>");
            // Add the main data table
            htmlTable.Append("<table border='1' style='border-collapse:collapse;'>");
            // Add header row
            htmlTable.Append("<tr>");
            foreach (DataGridViewColumn column in dgwBom.Columns)
            {
                if (column.Name != "TRANS" && column.Name != "KLINE")
                {
                    htmlTable.AppendFormat("<th>{0}</th>", column.HeaderText);
                }
            }
            htmlTable.Append("</tr>");
            // Add data rows with conditional formatting for "DELTA" values
            foreach (DataGridViewRow row in dgwBom.Rows)
            {
                htmlTable.Append("<tr>");
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (dgwBom.Columns[cell.ColumnIndex].Name != "TRANS" && dgwBom.Columns[cell.ColumnIndex].Name != "KLINE")
                    {
                        string cellValue = cell.Value?.ToString() ?? string.Empty;
                        string cellClass = string.Empty;
                        string alignmentClass = string.Empty;
                        if (dgwBom.Columns[cell.ColumnIndex].Name == "DELTA")
                        {
                            if (int.TryParse(cellValue, out int deltaValue))
                            {
                                cellClass = deltaValue >= 0 ? "green" : "red";
                            }
                            alignmentClass = "center";
                        }
                        else if (dgwBom.Columns[cell.ColumnIndex].Name == "TBALANCE" ||
                                 dgwBom.Columns[cell.ColumnIndex].Name == "QUANT" ||
                                 dgwBom.Columns[cell.ColumnIndex].Name == "CQUANT" ||
                                 dgwBom.Columns[cell.ColumnIndex].Name == "CALC" ||
                                 dgwBom.Columns[cell.ColumnIndex].Name == "ALT" ||
                                 dgwBom.Columns[cell.ColumnIndex].Name == "LEFTOVERS")
                        {
                            alignmentClass = "center";
                        }
                        if (dgwBom.Columns[cell.ColumnIndex].Name == "TBALANCE")
                        {
                            int DELTA = Convert.ToInt32(row.Cells["DELTA"].Value);
                            if (int.TryParse(cellValue, out int whValue))
                            {
                                cellClass = (whValue >= Math.Abs(DELTA) && whValue != 0) ? "green" : "red";
                            }
                        }
                        else if (dgwBom.Columns[cell.ColumnIndex].Name == "CQUANT")
                        {
                            int req = Convert.ToInt32(row.Cells["CQUANT"].Value);
                            int kitQty = txtbQty.Text != string.Empty ? Convert.ToInt32(txtbQty.Text) : 0;
                            if (kitQty != 0)
                            {
                                cellValue = $"{req} [{req / kitQty}]";
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
                        else if (dgwBom.Columns[cell.ColumnIndex].Name == "LEFTOVERS")
                        {
                            if (int.TryParse(cellValue, out int leftoversValue))
                            {
                                cellClass = leftoversValue < 0 ? "red" : string.Empty;
                            }
                        }
                        htmlTable.AppendFormat("<td class='{0} {1}'>{2}</td>", cellClass, alignmentClass, cellValue);
                    }
                }
                htmlTable.Append("</tr>");
            }
            htmlTable.Append("</table>");
            htmlTable.Append("</body></html>");
            var outlookApp = new Outlook.Application();
            Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
            mailItem.Subject = txtbName.Text + "_UPDATED_" + DateAndTime.Now.ToString("yyyyMMddHHmm");
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
            string clientDomain = txtbName.Text.Split('_')[0].ToLower();
            // Get emails from the client's domain
            List<string> clientEmails = GetUniqueClientEmails(clientDomain);
            // Display a form with checkboxes for all recipients
            RecipientSelectionForm selectionForm = new RecipientSelectionForm(inhouseEmails, clientEmails);
            if (selectionForm.ShowDialog() == DialogResult.OK)
            {
                // Combine selected emails into the "To" field
                mailItem.To = string.Join(";", selectionForm.SelectedEmails);
                // Embed the HTML table in the email body
                mailItem.HTMLBody = htmlTable.ToString();
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
            LoadingForm loadingForm = new LoadingForm();
            var loadingThread = new Thread(() => loadingForm.ShowDialog());
            loadingThread.Start();
            try
            {
                Outlook.Folder inboxFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox) as Outlook.Folder;
                Outlook.Folder outboxFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail) as Outlook.Folder;

                void AddEmailsFromFolder(Outlook.Folder folder, string domain)
                {
                    var items = folder.Items;
                    items.Sort("[ReceivedTime]", true); // Sort descending by date
                    int count = 0;
                    var enumerator = items.GetEnumerator();
                    while (enumerator.MoveNext() && count < 100)
                    {
                        if (enumerator.Current is Outlook.MailItem mail && !string.IsNullOrEmpty(mail.SenderEmailAddress))
                        {
                            string senderEmailLower = mail.SenderEmailAddress.ToLower();
                            string domainLower = domain.ToLower();
                            if (senderEmailLower.Contains(domainLower))
                            {
                                string contactInfo = $"{mail.SenderName} ({mail.SenderEmailAddress})";
                                emails.Add(contactInfo);
                            }
                        }
                        count++;
                    }
                }

                AddEmailsFromFolder(inboxFolder, clientDomain);
                AddEmailsFromFolder(outboxFolder, clientDomain);

                if (emails.Count == 0)
                {
                    string localDomain = "robotron";
                    AddEmailsFromFolder(inboxFolder, localDomain);
                    AddEmailsFromFolder(outboxFolder, localDomain);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
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
            // Sort the DataGridView by the DELTA column in ascending order
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
                writer.WriteLine(".orange { background-color: orangered; color: white; }");
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
                writer.WriteLine("      if (x && y) {");
                writer.WriteLine("        var xContent = x.innerHTML.toLowerCase();");
                writer.WriteLine("        var yContent = y.innerHTML.toLowerCase();");
                writer.WriteLine("        var xValue = isNaN(xContent) ? xContent : parseFloat(xContent);");
                writer.WriteLine("        var yValue = isNaN(yContent) ? yContent : parseFloat(yContent);");
                writer.WriteLine("        if (dir == 'asc') {");
                writer.WriteLine("          if (xValue > yValue) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
                writer.WriteLine("        } else if (dir == 'desc') {");
                writer.WriteLine("          if (xValue < yValue) {");
                writer.WriteLine("            shouldSwitch = true;");
                writer.WriteLine("            break;");
                writer.WriteLine("          }");
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
                writer.WriteLine("  var elements = document.getElementsByClassName('no-print');");
                writer.WriteLine("  for (var i = 0; i < elements.length; i++) {");
                writer.WriteLine("    elements[i].style.display = 'none';");
                writer.WriteLine("  }");
                writer.WriteLine("  window.print();");
                writer.WriteLine("  for (var i = 0; i < elements.length; i++) {");
                writer.WriteLine("    elements[i].style.display = 'block';");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("function changeFontColor(color) {");
                writer.WriteLine("  var elements = document.querySelectorAll('body, body *');");
                writer.WriteLine("  for (var i = 0; i < elements.length; i++) {");
                writer.WriteLine("    elements[i].style.color = color;");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("window.addEventListener('beforeprint', function() { changeFontColor('black'); });");
                writer.WriteLine("window.addEventListener('afterprint', function() { changeFontColor(''); });");
                // Add JavaScript for the SIM QTY button
                writer.WriteLine("function openSimQtyPopup() {");
                writer.WriteLine("  var currentQty = document.getElementById('currentQty').innerText;");
                writer.WriteLine("  var newQty = prompt('Enter new quantity for simulation:', currentQty);");
                writer.WriteLine("  if (newQty != null && !isNaN(newQty) && newQty > 0) {");
                writer.WriteLine("    document.getElementById('currentQty').innerText = newQty;");
                writer.WriteLine("    recalculateKitBalances(newQty);");
                writer.WriteLine("  }");
                writer.WriteLine("}");
                writer.WriteLine("function recalculateKitBalances(newQty) {");
                writer.WriteLine("  var table = document.getElementById('kitsTable');");
                writer.WriteLine("  for (var i = 1; i < table.rows.length; i++) {");
                writer.WriteLine("    var row = table.rows[i];");
                writer.WriteLine("    var cQuantCell = row.cells[5];"); // CQUANT column
                writer.WriteLine("    var quantCell = row.cells[4];"); // QUANT column
                writer.WriteLine("    var deltaCell = row.cells[6];"); // DELTA column
                writer.WriteLine("    var leftoversCell = row.cells[9];"); // LEFTOVERS column
                writer.WriteLine("    var requiredPerUnit = parseInt(cQuantCell.innerText.split('[')[1].split(']')[0]);");
                writer.WriteLine("    var newCQuant = requiredPerUnit * newQty;"); // Update the required quantity directly
                writer.WriteLine("    cQuantCell.innerText = newCQuant + ' [' + requiredPerUnit + ']';");
                writer.WriteLine("    var quant = parseInt(quantCell.innerText);");
                writer.WriteLine("    var delta = quant - newCQuant;");
                writer.WriteLine("    deltaCell.innerText = delta;");
                writer.WriteLine("    if (delta >= 10) {");
                writer.WriteLine("      deltaCell.className = 'green';");
                writer.WriteLine("    } else if (delta >= 0 && delta < 10) {");
                writer.WriteLine("      deltaCell.className = 'orange';");
                writer.WriteLine("    } else {");
                writer.WriteLine("      deltaCell.className = 'red';");
                writer.WriteLine("    }");
                writer.WriteLine("    var whQuantity = parseInt(row.cells[3].innerText);"); // WH column
                writer.WriteLine("    var leftovers = (whQuantity + quant) - newCQuant;");
                writer.WriteLine("    leftoversCell.innerText = leftovers;");
                writer.WriteLine("    if (leftovers < 0) {");
                writer.WriteLine("      leftoversCell.className = 'red';");
                writer.WriteLine("    } else {");
                writer.WriteLine("      leftoversCell.className = '';"); // Reset class if no condition met
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine("}");
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
                writer.WriteLine($"<td id='currentQty'>{txtbQty.Text}</td>");
                writer.WriteLine("</tr>");
                // Add another row for simulation and progress labels
                writer.WriteLine("<tr>");
                writer.WriteLine($"<td colspan='2' class='no-print'>{lblSim.Text}</td>");
                writer.WriteLine($"<td colspan='2'>{lblProgress.Text}</td>");
                writer.WriteLine("</tr>");
                writer.WriteLine("</table>");
                // Add a row displaying the comments if there are some in the work order
                if (!string.IsNullOrEmpty(rtxtbComments.Text) && rtxtbComments.Text != "No comments found for the selected ROB work order.")
                {
                    writer.WriteLine("<div style='border: 1px solid black; padding: 10px; margin-top: 20px;'>");
                    writer.WriteLine("<h2>Comments: ");
                    writer.WriteLine(System.Net.WebUtility.HtmlEncode(rtxtbComments.Text).Replace(Environment.NewLine, "<br>"));
                    writer.WriteLine("</h2></div>");
                }
                // Add the filter input box, clear button, print button, and SIM QTY button
                writer.WriteLine("<div class='no-print' style='margin-bottom: 20px; text-align: center;'>");
                writer.WriteLine("<input type='text' id='filterInput' onkeyup='filterTable()' placeholder='Filter table...' style='padding: 10px; width: 50%;text-align:center;background:orange;'>");
                writer.WriteLine("<button onclick='clearFilter()' style='padding: 10px;'>Clear</button>");
                writer.WriteLine("<button onclick='printReport()' style='padding: 10px;'>Print</button>");
                writer.WriteLine("<button onclick='openSimQtyPopup()' style='padding: 10px;'>SIM QTY</button>");
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
                                    if (deltaValue >= 10)
                                    {
                                        cellClass = "green";
                                    }
                                    else if (deltaValue >= 0 && deltaValue < 10)
                                    {
                                        cellClass = "orange";
                                    }
                                    else
                                    {
                                        cellClass = "red";
                                    }
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
                            else if (dgwBom.Columns[cell.ColumnIndex].Name == "CQUANT")
                            {
                                int req = Convert.ToInt32(row.Cells["CQUANT"].Value);
                                int requiredPerUnit = req / Convert.ToInt32(txtbQty.Text);
                                cellValue = $"{req} [{requiredPerUnit}]";
                            }
                            else if (dgwBom.Columns[cell.ColumnIndex].Name == "QUANT")
                            {
                                int req = Convert.ToInt32(row.Cells["CQUANT"].Value);
                                if (int.TryParse(cellValue, out int kitValue))
                                {
                                    cellClass = (kitValue >= req && kitValue != 0) ? "green" : "red";
                                }
                            }
                            else if (dgwBom.Columns[cell.ColumnIndex].Name == "LEFTOVERS")
                            {
                                if (int.TryParse(cellValue, out int leftoversValue))
                                {
                                    cellClass = leftoversValue < 0 ? "red" : string.Empty;
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
        private async void btnGetWHstock_Click(object sender, EventArgs e)
        {
            await FetchWarehouseBalances();
        }
        private async void btnGetComms_Click(object sender, EventArgs e)
        {
            await GetCommentsFromROBxxx();
        }
        private async Task GetCommentsFromROBxxx()
        {
            rtxtbComments.Clear();
            if (string.IsNullOrEmpty(txtbRob.Text))
            {
                MessageBox.Show("No ROB work order is currently loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string serialName = txtbRob.Text;
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=SERIALNAME eq '{serialName}'&$expand=SERIALTEXT_SUBFORM";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                    string usedUser = ApiHelper.AuthenticateClient(client);
                    // string usedUser = ApiHelper.AuthenticateClient(client);
                    RegisterTransaction(usedUser); // Log this transaction timestamp
                    // Make the HTTP GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Parse the JSON response
                    var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                    var serialTextSubform = apiResponse["value"].FirstOrDefault()?["SERIALTEXT_SUBFORM"]?.ToObject<SerialText>();
                    if (serialTextSubform != null)
                    {
                        rtxtbComments.Clear();
                        string decodedText = System.Net.WebUtility.HtmlDecode(serialTextSubform.TEXT);
                        string plainText = StripHtmlTags(decodedText);
                        rtxtbComments.AppendText(plainText + Environment.NewLine);
                    }
                    else
                    {
                        //rtxtbComments.Text = "No comments found for the selected ROB work order.";
                    }
                }
                catch (HttpRequestException ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                
                }
                catch (Exception ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                 
                }
            }
        }
        private string StripHtmlTags(string input)
        {
            input = input.Replace("<br>", string.Empty);
            input = input.Replace("<div>", string.Empty);
            input = input.Replace("</div>", string.Empty);
            input = input.Replace("<p>", string.Empty);
            input = input.Replace("</p>", string.Empty);
            input = input.Replace("p,div,li {margin:0cm;font-size:8.0pt;font-family:'Arial';}li > font > p {display: inline-block;}", string.Empty);
            return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", string.Empty);
        }
        public class SerialText
        {
            public string TEXT { get; set; }
        }
        private void rtxtbComments_Enter(object sender, EventArgs e)
        {
            if (rtxtbComments.Text == "No comments found for the selected ROB work order.")
            {
                rtxtbComments.Clear();
            }
        }
        private void rtxtbComments_Leave(object sender, EventArgs e)
        {
            if (rtxtbComments.Text == string.Empty)
            {
                //rtxtbComments.Text = "No comments found for the selected ROB work order.";
            }
        }
        private async void btnSaveComments_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbRob.Text))
            {
                MessageBox.Show("No ROB work order is currently loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string serialName = txtbRob.Text;
            string comments = rtxtbComments.Text;
            // Ask for confirmation before patching the data
            DialogResult result = MessageBox.Show("Are you sure you want to save the comments?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            // Reverse the process of stripping HTML tags
            string htmlComments = ConvertToHtml(comments);
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL('{serialName}')/SERIALTEXT_SUBFORM";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers if needed
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // Set the Authorization header
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    RegisterTransaction("Api2"); // Log this transaction timestamp
                    // Create the JSON payload for the PATCH request
                    var payload = new
                    {
                        TEXT = htmlComments
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                    // Make the PATCH request
                    HttpResponseMessage response = await client.PatchAsync(url, content);
                    response.EnsureSuccessStatusCode();
                    MessageBox.Show("Comments saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (HttpRequestException ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
           
                }
                catch (Exception ex)
                {
                    SafeAppendLog($"Request error: {ex.Message}", Color.Red);
                  
                }
            }
        }
        private string ConvertToHtml(string input)
        {
            input = input.Replace(Environment.NewLine, "<br>");
            input = input.Replace(" ", "&nbsp;");
            return $"<p>{input}</p>";
        }
        private void btnGetMFNs_KeyDown(object sender, KeyEventArgs e)
        {
        }
        private async void btnGetMFNs_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseButtons == MouseButtons.Right)
            {
                btnGetMFNs.Text = "GET ALTs";
                await FetchAltsForAllRows();
                await (Task.Delay(1000));
                btnGetMFNs.Text = "GET MFNPs";
            }
            else if (MouseButtons == MouseButtons.Left)
            {
                await FetchMFPNsForAllRows();
            }
        }
        private List<DataGridViewRow> originalRows = new List<DataGridViewRow>();
        private bool isFiltered = false;
        private void btnInStock_Click(object sender, EventArgs e)
        {
            if (!isFiltered)
            {
                // Store the original data
                originalRows = dgwIPNmoves.Rows.Cast<DataGridViewRow>().ToList();
                // Separate data into ROB and notRob lists
                var robList = new List<DataGridViewRow>();
                var notRobList = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in dgwIPNmoves.Rows)
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
                // Filter the DataGridView to show only items left from the notRob list
                dgwIPNmoves.Rows.Clear();
                foreach (var row in filteredNotRobList)
                {
                    dgwIPNmoves.Rows.Add(row);
                }
                isFiltered = true;
            }
            else
            {
                // Restore the original data
                dgwIPNmoves.Rows.Clear();
                foreach (var row in originalRows)
                {
                    dgwIPNmoves.Rows.Add(row);
                }
                isFiltered = false;
            }
            dgwIPNmoves.Update();
        }
        private void btnGetMFNs_Click(object sender, EventArgs e)
        {

        }


        private async Task LoadDataAndFilterInStock()
        {
            //SafeAppendLog("Starting LoadDataAndFilterInStock...");
            InitializeInStockDataGridView();

            var robList = new List<DataGridViewRow>();
            var notRobList = new List<DataGridViewRow>();

            SafeAppendLog($"Scanning {dgwIPNmoves.Rows.Count} rows in dgwIPNmoves...",Color.Yellow);



            // Split rows into rob and non-rob, ignoring UDATE
foreach (DataGridViewRow row in dgwIPNmoves.Rows)
{
    if (row.IsNewRow) continue;

    var docNoObj = row.Cells["LOGDOCNO"].Value;
    string docNo = docNoObj?.ToString()?.Trim() ?? "";

    if (string.IsNullOrWhiteSpace(docNo))
    {
        //SafeAppendLog($"Skipped row: LOGDOCNO is missing or empty.");
        continue;
    }

    if (docNo.StartsWith("ROB") || docNo.StartsWith("IC") || docNo.StartsWith("WR") || docNo.StartsWith("SH"))
    {
        if (docNo.StartsWith("IC"))
        {
            //SafeAppendLog($"Fixing quantity sign for IC doc {docNo}");
            try
            {
                row.Cells["TQUANT"].Value = Math.Abs(Convert.ToInt32(row.Cells["TQUANT"].Value));
            }
            catch (Exception ex)
            {
                //SafeAppendLog($"Error fixing quantity sign for {docNo}: {ex.Message}");
            }
        }
        robList.Add(row);
    }
    else
    {
        notRobList.Add(row);
    }
}


            //SafeAppendLog($"ROB list count: {robList.Count}");
            //SafeAppendLog($"NotROB list count: {notRobList.Count}");

            // Sort by transaction date
            robList = robList.OrderBy(r => DateTime.Parse(r.Cells["UDATE"].Value.ToString())).ToList();
            notRobList = notRobList.OrderBy(r => DateTime.Parse(r.Cells["UDATE"].Value.ToString())).ToList();

            // Filter out matching pairs
            var filteredNotRobList = new List<DataGridViewRow>(notRobList);
            foreach (var notRobRow in notRobList)
            {
                if (robList.Count == 0) break;

                int notRobQty = Convert.ToInt32(notRobRow.Cells["TQUANT"].Value);
                var match = robList.FirstOrDefault(robRow => Convert.ToInt32(robRow.Cells["TQUANT"].Value) == notRobQty);

                if (match != null)
                {
                    filteredNotRobList.Remove(notRobRow);
                    robList.Remove(match);
                }
            }

            //SafeAppendLog($"FilteredNotRob list count: {filteredNotRobList.Count}");

            // Prepare HttpClient
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);


            string usedUser = ApiHelper.AuthenticateClient(client);
            // string usedUser = ApiHelper.AuthenticateClient(client);
          

            dgwINSTOCK.Rows.Clear();
            dgwINSTOCK.Visible = false;

            int addedCount = 0;
            int excludedCount = 0;

            foreach (var row in filteredNotRobList)
            {
                string docNo = row.Cells["LOGDOCNO"].Value?.ToString();
                if (string.IsNullOrWhiteSpace(docNo))
                {
                    SafeAppendLog("Skipped a row with empty DOCNO.");
                    continue;
                }

                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_P?$filter=DOCNO eq '{docNo}'&$select=TOWARHSNAME";

                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    RegisterTransaction(usedUser);
                    var body = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<JObject>(body);
                    string warhs = json["value"]?.FirstOrDefault()?["TOWARHSNAME"]?.ToString()?.Trim();

                    if (string.IsNullOrWhiteSpace(warhs))
                    {
                        //SafeAppendLog($"DOCNO {docNo} => Warning: No TOWARHSNAME found.");
                        continue;
                    }

                    //SafeAppendLog($"DOCNO {docNo} => TOWARHSNAME = {warhs}");

                    if (warhs == "666")
                    {
                        excludedCount++;
                        //SafeAppendLog($"Excluded DOCNO {docNo} (TOWARHSNAME = 666)");
                        continue;
                    }

                    var newRow = dgwINSTOCK.Rows[dgwINSTOCK.Rows.Add()];
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        newRow.Cells[i].Value = row.Cells[i].Value;
                    }

                    addedCount++;
                }
                catch (Exception ex)
                {
                    SafeAppendLog($"Error checking DOCNO {docNo}: {ex.Message}", Color.Red);
                }
            }

            dgwINSTOCK.Update();
            dgwINSTOCK.Visible = true;

            SafeAppendLog($"Final: {addedCount} rows added to dgwINSTOCK, {excludedCount} rows excluded due to TOWARHSNAME = 666",Color.LimeGreen);
        }


        private async Task RemoveRowsWithTowarhsname666Async()
        {
            //  SafeAppendLog("RemoveRowsWithTowarhsname666Async: Start");
            var rowsToRemove = new List<DataGridViewRow>();
            var rows = dgwINSTOCK.Rows.Cast<DataGridViewRow>().ToList();

            foreach (var row in rows)
            {
                var docNo = row.Cells["LOGDOCNO"].Value?.ToString();
                if (string.IsNullOrEmpty(docNo))
                {
                    SafeAppendLog("Skipping row with empty LOGDOCNO");
                    continue;
                }

                string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/DOCUMENTS_P?$filter=DOCNO eq '{docNo}'&$select=TOWARHSNAME";
                //SafeAppendLog($"Checking TOWARHSNAME for DOCNO: {docNo}");
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<JObject>(responseBody);
                        var value = apiResponse["value"]?.FirstOrDefault();
                        if (value != null)
                        {
                            string toWarhsName = value["TOWARHSNAME"]?.ToString();
                            // SafeAppendLog($"DOCNO: {docNo}, TOWARHSNAME: {toWarhsName}");
                            if (toWarhsName == "666")
                            {
                                rowsToRemove.Add(row);
                                // SafeAppendLog($"Marked for removal: DOCNO {docNo} (TOWARHSNAME=666)");
                            }
                        }
                        else
                        {
                            SafeAppendLog($"No value found for DOCNO: {docNo}");
                        }
                    }
                    catch (Exception ex)
                    {
                        SafeAppendLog($"Error checking DOCNO {docNo}: {ex.Message}");
                    }
                }
            }



            // SafeAppendLog($"Rows to remove (TOWARHSNAME=666): {rowsToRemove.Count}");
            foreach (var row in rowsToRemove)
            {
                var logDocNo = row.Cells["LOGDOCNO"].Value?.ToString(); // Capture before removal
                dgwINSTOCK.Rows.Remove(row);
                // SafeAppendLog($"Removed row with DOCNO: {logDocNo}");
            }

            dgwINSTOCK.Refresh();
            // SafeAppendLog("RemoveRowsWithTowarhsname666Async: End");


            dgwINSTOCK.Visible = true; // Show grid after filtering
        }



        private void InitializeInStockDataGridView()
        {
            dgwINSTOCK.Columns.Clear();

            dgwINSTOCK.Columns.Add("UDATE", "Transaction Date");
            dgwINSTOCK.Columns.Add("LOGDOCNO", "Document Number");
            dgwINSTOCK.Columns.Add("DOCDES", "DOCDES");
            dgwINSTOCK.Columns.Add("SUPCUSTNAME", "Source/Requester");
            dgwINSTOCK.Columns.Add("BOOKNUM", "Client's Document");
            dgwINSTOCK.Columns.Add("TQUANT", "Qty");
            dgwINSTOCK.Columns.Add("PACK", "Pack");
            dgwINSTOCK.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgwINSTOCK.AllowUserToAddRows = false; // Optional: Prevent manual row addition
        }
        private async void FrmPriorityBom_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(robSerial) && isItemAddedToKit)
                {
                    await PatchUpdateUdateAsync(robSerial);
                    MessageBox.Show($"[✓] UDATE updated for ROB: {robSerial} on form close.");
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignore this specific exception because the form or controls are disposed
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[X] Failed to update UDATE for {robSerial}:{ex.Message}",
                                "Update Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }

        private async Task PatchUpdateUdateAsync(string serialName)
        {
            string url = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL('{serialName}')";

            var patchData = new
            {
                UDATE = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };

            using (HttpClient client = new HttpClient())
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                RegisterTransaction("Api2");
                var json = JsonConvert.SerializeObject(patchData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Patch, url)
                {
                    Content = content
                };

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }

        private void dgwBom_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dgwBom.HitTest(e.X, e.Y);
                int altColumnIndex =8;

                if (hit.Type == DataGridViewHitTestType.Cell && hit.ColumnIndex == altColumnIndex)
                {
                    var altCellValue = dgwBom.Rows[hit.RowIndex].Cells[altColumnIndex].Value;
                    if (altCellValue != null && !string.IsNullOrEmpty(altCellValue.ToString()))
                    {
                        dgwBom.ClearSelection();
                        dgwBom.Rows[hit.RowIndex].Cells[hit.ColumnIndex].Selected = true;

                        contextMenuSwitchToAlt.Show(dgwBom, e.Location);
                    }
                }
            }
        }

   
        private async void SwitchToAltItem_Click(object sender, EventArgs e)
    {
        if (dgwBom.SelectedCells.Count > 0)
        {
            int rowIndex = dgwBom.SelectedCells[0].RowIndex;

            int altColumnIndex = 8;
            int ipnColumnIndex = 0;
            int whColumnIndex = 3;

            var altCell = dgwBom.Rows[rowIndex].Cells[altColumnIndex];
            var ipnCell = dgwBom.Rows[rowIndex].Cells[ipnColumnIndex];
            var whCell = dgwBom.Rows[rowIndex].Cells[whColumnIndex];

            // Swap IPN and ALT values
            var temp = altCell.Value;
            string altIPN = altCell.Value?.ToString()?.Trim();
            string originalIPN = ipnCell.Value?.ToString()?.Trim();
                altCell.Value = ipnCell.Value;
            ipnCell.Value = temp;

            // Now the IPN cell contains the new partName to check stock for
            string partName = ipnCell.Value?.ToString();
            if (string.IsNullOrEmpty(partName))
            {
                whCell.Value = "N/A";
                return;
            }

                string selectedWarehouse = dgwBom.Rows[0].Cells["PARTNAME"].Value?.ToString()?.Substring(0, 3);

                string checkUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/WAREHOUSES?$filter=WARHSNAME eq '{selectedWarehouse}'&$expand=WARHSBAL_SUBFORM($filter=PARTNAME eq '{partName}';$select=TBALANCE)";

            int availableQty = 0;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    HttpResponseMessage checkResponse = await client.GetAsync(checkUrl);
                    checkResponse.EnsureSuccessStatusCode();
                        RegisterTransaction("Api2");
                        string checkResponseBody = await checkResponse.Content.ReadAsStringAsync();
                    var checkApiResponse = JObject.Parse(checkResponseBody);

                    var warehouse = checkApiResponse["value"]?.FirstOrDefault();
                    if (warehouse != null)
                    {
                        var balance = warehouse["WARHSBAL_SUBFORM"]?.FirstOrDefault();
                        if (balance != null)
                        {
                            availableQty = balance["TBALANCE"].Value<int>();
                                if (availableQty > 0)
                                {
                                    string serialName = txtbRob.Text;
                                    int qtyDelta = Convert.ToInt32(dgwBom.Rows[rowIndex].Cells["DELTA"].Value);

                                    if (qtyDelta < 0)
                                    {
                                        await postAltItemRowIntoKit(altIPN, (qtyDelta*(-1)), serialName);
                                        await nullyfyTheOriginalIPN(originalIPN, serialName);
                                    }
                                }
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error, optionally show a message or log
                whCell.Value = "Error";
                Console.WriteLine($"Error fetching stock: {ex.Message}");
                return;
            }

            // Update WH column with the fetched available quantity
            whCell.Value = availableQty;
        }
    }
     
        private async Task postAltItemRowIntoKit(string altIpnToAddToKit, int theQtyToInsert, string robWoToInsertInto)
        {
            if (string.IsNullOrWhiteSpace(altIpnToAddToKit) || string.IsNullOrWhiteSpace(robWoToInsertInto))
            {
                SafeAppendLog("⚠️ Missing input parameters for posting ALT item.");
                return;
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                string requestUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL('{robWoToInsertInto}')/KITITEMS_SUBFORM";

                var payload = new
                {
                    PARTNAME = altIpnToAddToKit,
                    QUANT = theQtyToInsert,
                    ACTNAME = "Prod"
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                
                try
                {
                    var response = await client.PostAsync(requestUrl, content);
                    RegisterTransaction("Api2");
                    if (response.IsSuccessStatusCode)
                    {
                        
                        SafeAppendLog($"✅ Successfully added ALT {altIpnToAddToKit} to {robWoToInsertInto} kit.");
                    }
                    else
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        SafeAppendLog($"❌ Failed to add ALT item {altIpnToAddToKit} to kit: {response.StatusCode} - {responseBody}");
                    }
                }
                catch (Exception ex)
                {
                    SafeAppendLog($"🔥 Exception in postAltItemRowIntoKit: {ex.Message}{ex.InnerException?.Message}");
                }
            }
        }


        private async Task nullyfyTheOriginalIPN(string originalIPN, string serialName)
        {
            string baseUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL('{serialName}')/KITITEMS_SUBFORM";
            string escapedOriginalIPN = originalIPN.Replace("'", "''");
            string filterUrl = $"{baseUrl}?$filter=PARTNAME eq '{escapedOriginalIPN}'";

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.Api2Username}:{settings.Api2Password}")));

            try
            {
                var getResponse = await client.GetAsync(filterUrl);
                RegisterTransaction("Api2");
                getResponse.EnsureSuccessStatusCode();

                var json = await getResponse.Content.ReadAsStringAsync();
                var data = JObject.Parse(json);

                var items = data["value"];
                if (items == null || !items.Any())
                {
                    SafeAppendLog($"No kit items found for original IPN '{originalIPN}'");
                    return;
                }

                foreach (var item in items)
                {
                    int kline = item["KLINE"].Value<int>();
                    string patchUrl = $"{baseUrl}({kline})";

                    var patchPayload = new { QUANT = 0 };
                    var content = new StringContent(JsonConvert.SerializeObject(patchPayload), Encoding.UTF8, "application/json");

                    var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), patchUrl)
                    {
                        Content = content
                    };

                    var patchResponse = await client.SendAsync(patchRequest);
                    if (patchResponse.IsSuccessStatusCode)
                        SafeAppendLog($"Successfully nulled QUANT for {originalIPN} at KLINE {kline}");
                    else
                        SafeAppendLog($"Failed to patch KLINE {kline} - Status: {patchResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                SafeAppendLog($"Error in nullyfyTheOriginalIPN: {ex.Message}");
            }
        }
    

        private void SafeAppendLog(string message, Color? color = null)
        {
            if (txtbLog == null || txtbLog.IsDisposed || txtbLog.Disposing)
                return;

            try
            {
                if (txtbLog.InvokeRequired)
                {
                    txtbLog.Invoke(new Action(() => SafeAppendLog(message, color)));
                    return;
                }

                if (color.HasValue)
                {
                    txtbLog.SelectionStart = txtbLog.TextLength;
                    txtbLog.SelectionLength = 0;
                    txtbLog.SelectionColor = color.Value;
                }
                else
                {
                    txtbLog.SelectionColor = txtbLog.ForeColor;
                }

                // ⬇️ Add the line break here
                txtbLog.AppendText(message + Environment.NewLine);

                txtbLog.SelectionColor = txtbLog.ForeColor; // reset color
                txtbLog.SelectionStart = txtbLog.Text.Length;
                txtbLog.ScrollToCaret();
            }
            catch (ObjectDisposedException)
            {
                // Control disposed, ignore safely
            }
        }

    }
}
