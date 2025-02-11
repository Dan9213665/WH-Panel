using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;
using Seagull.Framework.Yaml;

namespace WH_Panel
{
    public partial class FrmPrioritySearchRob : Form
    {
        private AppSettings settings;

        public FrmPrioritySearchRob()
        {
            InitializeComponent();
            SetDarkModeColors(this);
            InitializeDataGridView();
        }
        private void FrmPrioritySearchRob_Load(object sender, EventArgs e)
        {
            settings = SettingsManager.LoadSettings();
            if (settings == null)
            {
                //MessageBox.Show("Failed to load settings.");
                txtbLog.SelectionColor = Color.Red;
                txtbLog.AppendText("Failed to load settings.");
                txtbLog.ScrollToCaret();
                return;
            }
        }
        private void InitializeDataGridView()
        {
            // Set the AutoSizeColumnsMode property to Fill
            dgwSerials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add columns if not already added
            if (dgwSerials.Columns.Count == 0)
            {
                var serialNameColumn = new DataGridViewButtonColumn
                {
                    Name = "SERIALNAME",
                    HeaderText = "ROB work order",
                    UseColumnTextForButtonValue = false, // Set to false to use cell value as button text
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 33.33f // Set the fill weight to 33.33%
                };
                dgwSerials.Columns.Add(serialNameColumn);

                var parentIpnColumn = new DataGridViewTextBoxColumn
                {
                    Name = "PARENTIPN",
                    HeaderText = "Parent IPN",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 33.33f // Set the fill weight to 33.33%
                };
                dgwSerials.Columns.Add(parentIpnColumn);

                var serialDesColumn = new DataGridViewTextBoxColumn
                {
                    Name = "SERIALDES",
                    HeaderText = "Description",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 33.33f // Set the fill weight to 33.33%
                };
                dgwSerials.Columns.Add(serialDesColumn);
            }
        }



        public class SerialInfo
        {
            public string SERIALNAME { get; set; }
            public string PARENTIPN { get; set; }
            public string SERIALDES { get; set; }
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
                if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = borderColor;
                    button.ForeColor = foregroundColor;
                    button.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    button.FlatAppearance.MouseOverBackColor = Color.FromArgb(51, 153, 255);
                }
                else if (control is GroupBox groupBox)
                {
                    groupBox.ForeColor = foregroundColor;
                    groupBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }
                else if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = backgroundColor;
                    textBox.ForeColor = foregroundColor;
                    textBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }
                else if (control is Label label)
                {
                    if (label.Name != "lblLoading")
                    {
                        label.BackColor = backgroundColor;
                        label.ForeColor = foregroundColor;
                        label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    }
                }
                else if (control is TabControl tabControl)
                {
                    tabControl.BackColor = backgroundColor;
                    tabControl.ForeColor = foregroundColor;
                    tabControl.Font = new Font("Segoe UI", 10, FontStyle.Regular);
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
                    dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
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
                    comboBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.BackColor = backgroundColor;
                    dateTimePicker.ForeColor = foregroundColor;
                    dateTimePicker.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }
                // Recursively update controls within containers
                if (control.Controls.Count > 0)
                {
                    SetDarkModeColors(control);
                }
            }
        }



        private async void txtbSearchIPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string partName = txtbSearchIPN.Text.Trim();
                if (string.IsNullOrEmpty(partName))
                {
                    MessageBox.Show("Please enter a valid IPN.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dgwSerials.Rows.Clear();

                string partUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/PART?$filter=PARTNAME eq '{partName}'&$expand=PARTPARENT_SUBFORM";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Set the request headers
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                        // Make the HTTP GET request for part details
                        HttpResponseMessage partResponse = await client.GetAsync(partUrl);
                        partResponse.EnsureSuccessStatusCode();
                        string partResponseBody = await partResponse.Content.ReadAsStringAsync();
                        var partApiResponse = JsonConvert.DeserializeObject<JObject>(partResponseBody);

                        if (partApiResponse == null)
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("partApiResponse is null\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }

                        if (partApiResponse["value"] == null)
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("partApiResponse[\"value\"] is null\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }

                        if (!partApiResponse["value"].Any())
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("partApiResponse[\"value\"] is empty\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }

                        var part = partApiResponse["value"].FirstOrDefault();
                        if (part == null)
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("part is null\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }

                        txtbLog.AppendText($"IPN: {partName}\n");
                        var partParentSubform = part["PARTPARENT_SUBFORM"]?.FirstOrDefault();
                        if (partParentSubform == null)
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("partParentSubform is null\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }

                        string parentName = partParentSubform["PARENTNAME"]?.ToString();
                        if (parentName == null)
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("parentName is null\n");
                            txtbLog.ScrollToCaret();
                            return;
                        }

                        txtbLog.AppendText($"Parent IPN: {parentName}\n");
                        await FetchAndDisplaySerials(parentName);
                    }
                    catch (HttpRequestException ex)
                    {
                        txtbLog.SelectionColor = Color.Red;
                        txtbLog.AppendText($"HttpRequestException txtbSearchIPN_KeyDown error: {ex.Message}\n");
                        txtbLog.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        txtbLog.SelectionColor = Color.Red;
                        txtbLog.AppendText($"Exception txtbSearchIPN_KeyDown error: {ex.Message}\n");
                        txtbLog.AppendText($"Stack Trace: {ex.StackTrace}\n");
                        txtbLog.ScrollToCaret();
                    }
                }
            }
        }



        private async Task FetchAndDisplaySerials(string parentName)
        {
            string serialUrl = $"https://p.priority-connect.online/odata/Priority/tabzad51.ini/a020522/SERIAL?$filter=PARTNAME eq '{parentName}'";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{settings.ApiUsername}:{settings.ApiPassword}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    // Make the HTTP GET request for serial details
                    HttpResponseMessage serialResponse = await client.GetAsync(serialUrl);
                    serialResponse.EnsureSuccessStatusCode();
                    string serialResponseBody = await serialResponse.Content.ReadAsStringAsync();
                    var serialApiResponse = JsonConvert.DeserializeObject<JObject>(serialResponseBody);

                    if (serialApiResponse["value"] != null && serialApiResponse["value"].Any())
                    {
                        var serials = serialApiResponse["value"]
                            .Where(s => s["SERIALSTATUSDES"]?.ToString() != "נסגרה" && s["SERIALSTATUSDES"]?.ToString() != "קיט מלא" && s["SERIALSTATUSDES"] != null)
                            .Select(s => new SerialInfo
                            {
                                SERIALNAME = s["SERIALNAME"]?.ToString(),
                                PARENTIPN = parentName,
                                SERIALDES = s["SERIALDES"]?.ToString()
                            })
                            .ToList();
                        if (serials.Count == 0)
                        {
                            txtbLog.SelectionColor = Color.Red;
                            txtbLog.AppendText("No open Work Orders found !\n");
                            txtbLog.ScrollToCaret();
                        }
                        else
                        {
                            DisplaySerials(serials);
                        }

                      
                    }
                   
                }
                catch (HttpRequestException ex)
                {
                    txtbLog.SelectionColor = Color.Red;
                    txtbLog.AppendText($"Request FetchAndDisplaySerials error: {ex.Message}\n");
                    txtbLog.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    txtbLog.SelectionColor = Color.Red;
                    txtbLog.AppendText($"Request FetchAndDisplaySerials error: {ex.Message}\n");
                    txtbLog.ScrollToCaret();
                }
            }
        }

        private void DisplaySerials(List<SerialInfo> serials)
        {
            try
            {
                // Clear existing rows
                dgwSerials.Rows.Clear();

                // Add rows
                foreach (var serial in serials)
                {
                    int rowIndex = dgwSerials.Rows.Add(serial.SERIALNAME, serial.PARENTIPN, serial.SERIALDES);
                    dgwSerials.Rows[rowIndex].Cells["SERIALNAME"].Value = serial.SERIALNAME; // Set the button text to SERIALNAME
                }


            }
            catch (Exception ex)
            {
                txtbLog.SelectionColor = Color.Red;
                txtbLog.AppendText($"Error in DisplaySerials: {ex.Message}\n");
                txtbLog.ScrollToCaret();
            }
        }



        private void OpenFrmPriorityBom(string serialName)
        {
            try
            {
                FrmPriorityBom frmPriorityBom = new FrmPriorityBom
                {
                    SelectedSerialName = serialName
                };
                frmPriorityBom.Show();
            }
            catch (Exception ex)
            {
                txtbLog.SelectionColor = Color.Red;
                txtbLog.AppendText($"Error in OpenFrmPriorityBom: {ex.Message}\n");
                txtbLog.ScrollToCaret();
            }
        }

        private void dgwSerials_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgwSerials.Columns["SERIALNAME"].Index)
                {
                    string serialName = dgwSerials.Rows[e.RowIndex].Cells["SERIALNAME"].Value.ToString();
                    OpenFrmPriorityBom(serialName);
                }
            }
            catch (Exception ex)
            {
                txtbLog.SelectionColor = Color.Red;
                txtbLog.AppendText($"Error in DgwSerials_CellClick: {ex.Message}\n");
                txtbLog.ScrollToCaret();
            }
        }

        private void txtbSearchIPN_DoubleClick(object sender, EventArgs e)
        {
            txtbSearchIPN.Clear();
        }

        private void txtbSearchIPN_Enter(object sender, EventArgs e)
        {
            txtbSearchIPN.BackColor = Color.LightGreen;
            txtbSearchIPN.ForeColor = Color.Black;
        }

        private void txtbSearchIPN_Leave(object sender, EventArgs e)
        {
            txtbSearchIPN.BackColor = Color.Gray;
        }
    }
}

