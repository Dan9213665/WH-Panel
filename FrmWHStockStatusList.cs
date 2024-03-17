using FastMember;
using Seagull.Framework.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WH_Panel
{
    public partial class FrmWHStockStatusList : Form
    {
        public FrmWHStockStatusList()
        {
            InitializeComponent();
        }
        List<ClientWarehouse> warehouses { get; set; }
        public List<WHitem> stockItems =new List<WHitem>();
        ClientWarehouse selectedWH { get; set; }
        public void InitializeGlobalWarehouses(List<ClientWarehouse> warehousesFromTheMain)
        {
            warehouses = warehousesFromTheMain;
            label2.Text = "Loaded warehouses : " + warehouses.Count.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Lines.Length > 0)
            {
               string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");

                if(isSql)
                {
                    StockViewDataLoaderSql(selectedWH.sqlStock);

                }
                else
                {

                    StockViewDataLoader(selectedWH.clStockFile, "STOCK");
                }
               

                GenerateFilteredReport();
            }
            else
            {
                MessageBox.Show("Paste IPNs to search for in the textbox !");
                textBox1.Focus();
            }
        }
        private void StockViewDataLoaderSql(string sqlStock)
        {
            stockItems.Clear();
            // Connection string for SQL Server Express
            string connectionString = sqlStock;

            try
            {
                // Load STOCK table into dataGridView1
                using (SqlConnection connection = new SqlConnection(connectionString))
                {


                    SqlDataAdapter adapterStock = new SqlDataAdapter("SELECT * FROM STOCK", connection);

                    DataTable stockTable = new DataTable();
                    adapterStock.Fill(stockTable);

                    foreach (DataRow row in stockTable.Rows)
                    {
                        WHitem item = new WHitem
                        {
                            IPN = row["IPN"].ToString(),
                            Manufacturer = row["Manufacturer"].ToString(),
                            MFPN = row["MFPN"].ToString(),
                            Description = row["Description"].ToString(),
                            Stock = Convert.ToInt32(row["Stock"]), // Assuming Stock is an integer field
                            Updated_on = row["Updated_on"].ToString(),
                            Comments = row["Comments"].ToString(),
                            Source_Requester = row["Source_Requester"].ToString()
                        };

                        stockItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show($"Error loading STOCK table: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GenerateFilteredReport()
        {
            //stockItems.OrderBy(item => item.IPN);
            // Assuming textBox1.Lines contains the filter values
            string[] filterValues = textBox1.Lines.Select(line => line.Trim()).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            // Assuming stockItems is your list of WHitem
            var groupedByIPN = stockItems
                .Where(item => filterValues.Contains(item.IPN))
                .GroupBy(item => item.IPN).OrderBy(item => item.Key)
            .ToList();

         

            //groupedByIPN.OrderBy(item => item.Key);
            // Generate and display the HTML report using the grouped list
            GenerateHTMLReport(groupedByIPN);
        }
       //private void GenerateHTMLReport(List<WHitem> items)
         private void GenerateHTMLReport(List<IGrouping<string?, WHitem>> groupedByIPN)
        {
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string filename = "\\\\dbr1\\Data\\WareHouse\\2024\\WHsearcher\\" + _fileTimeStamp + ".html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>Listed items to search for</title>");
                writer.WriteLine("</head>");
                // Assuming this is part of your HTML generation
                //writer.WriteLine("<button onclick='toggleDisplay()'>FILTER for PRINTOUT</button>");
                //writer.WriteLine("<div id='rowsCount'>Total Rows Loaded : </div>");
                //writer.WriteLine("<script>");
                //writer.WriteLine("var filterOn = false;"); // Variable to track filtering state
                //writer.WriteLine("function toggleDisplay() {");
                //writer.WriteLine("  filterOn = !filterOn;"); // Toggle the filter state
                //writer.WriteLine("  var rows = document.getElementsByTagName('tr');");
                //writer.WriteLine("  for (var i = 0; i < rows.length; i++) {");
                //writer.WriteLine("    var stockCell = rows[i].getElementsByTagName('td')[4];"); // Assuming the stock cell is at index 4
                //writer.WriteLine("    rows[i].style.display = filterOn ? (stockCell && stockCell.style.backgroundColor === 'lightgreen' ? '' : 'none') : '';");
                //writer.WriteLine("  }");
                //writer.WriteLine("}");
                //writer.WriteLine("</script>");
                //writer.WriteLine("<button onclick='toggleDisplay()'>FILTER for PRINTOUT</button>");
                //writer.WriteLine("<div id='rowsCount'>Total Rows Loaded : </div>");
                //writer.WriteLine("<script>");
                //writer.WriteLine("var filterOn = false;"); // Variable to track filtering state
                //writer.WriteLine("function toggleDisplay() {");
                //writer.WriteLine("  filterOn = !filterOn;"); // Toggle the filter state
                //writer.WriteLine("  var rows = document.getElementsByTagName('tr');");
                //writer.WriteLine("  var totalRows = 0;"); // Variable to track total rows
                //writer.WriteLine("  for (var i = 0; i < rows.length; i++) {");
                //writer.WriteLine("    var stockCell = rows[i].getElementsByTagName('td')[4];"); // Assuming the stock cell is at index 4
                //writer.WriteLine("    if (filterOn) {");
                //writer.WriteLine("      if (stockCell && stockCell.style.backgroundColor === 'lightgreen') {");
                //writer.WriteLine("        rows[i].style.display = '';");
                //writer.WriteLine("        totalRows++;");
                //writer.WriteLine("      } else {");
                //writer.WriteLine("        rows[i].style.display = 'none';");
                //writer.WriteLine("      }");
                //writer.WriteLine("    } else {");
                //writer.WriteLine("      rows[i].style.display = '';");
                //writer.WriteLine("      totalRows++;");
                //writer.WriteLine("    }");
                //writer.WriteLine("  }");
                //writer.WriteLine("  document.getElementById('rowsCount').innerHTML = 'Total Rows Loaded : ' + totalRows;");
                //writer.WriteLine("}");
                //writer.WriteLine("</script>");
                //writer.WriteLine("<button onclick='toggleDisplay()'>FILTER for PRINTOUT</button>");
                //writer.WriteLine("<div id='rowsCount'>Total Rows Loaded : </div>");
                //writer.WriteLine("<script>");
                //writer.WriteLine("var filterOn = false;"); // Variable to track filtering state
                //writer.WriteLine("document.addEventListener('DOMContentLoaded', function() {");
                //writer.WriteLine("  countRows();"); // Call the countRows function after the page has finished loading
                //writer.WriteLine("});");
                //writer.WriteLine("function toggleDisplay() {");
                //writer.WriteLine("  filterOn = !filterOn;"); // Toggle the filter state
                //writer.WriteLine("  var rows = document.getElementsByTagName('tr');");
                //writer.WriteLine("  var totalRows = 0;"); // Variable to track total rows
                //writer.WriteLine("  for (var i = 0; i < rows.length; i++) {");
                //writer.WriteLine("    var stockCell = rows[i].getElementsByTagName('td')[4];"); // Assuming the stock cell is at index 4
                //writer.WriteLine("    if (filterOn) {");
                //writer.WriteLine("      if (stockCell && stockCell.style.backgroundColor === 'lightgreen') {");
                //writer.WriteLine("        rows[i].style.display = '';");
                //writer.WriteLine("        totalRows++;");
                //writer.WriteLine("      } else {");
                //writer.WriteLine("        rows[i].style.display = 'none';");
                //writer.WriteLine("      }");
                //writer.WriteLine("    } else {");
                //writer.WriteLine("      rows[i].style.display = '';");
                //writer.WriteLine("      totalRows++;");
                //writer.WriteLine("    }");
                //writer.WriteLine("  }");
                //writer.WriteLine("  document.getElementById('rowsCount').innerHTML = 'Total Rows Loaded : ' + totalRows;");
                //writer.WriteLine("}");
                //writer.WriteLine("function countRows() {");
                //writer.WriteLine("  var rows = document.getElementsByTagName('tr');");
                //writer.WriteLine("  var totalRows = rows.length;");
                //writer.WriteLine("  document.getElementById('rowsCount').innerHTML = 'Total Rows Loaded : ' + totalRows;");
                //writer.WriteLine("}");
                //writer.WriteLine("</script>");
                writer.WriteLine("<button onclick='toggleDisplay()'>FILTER for PRINTOUT</button>");
                writer.WriteLine("<div id='rowsCount'>Total Rows Loaded : </div>");
                writer.WriteLine("<script>");
                writer.WriteLine("var filterOn = false;"); // Variable to track filtering state
                writer.WriteLine("document.addEventListener('DOMContentLoaded', function() {");
                writer.WriteLine("  countRows();"); // Call the countRows function after the page has finished loading
                writer.WriteLine("});");
                writer.WriteLine("function toggleDisplay() {");
                writer.WriteLine("  filterOn = !filterOn;"); // Toggle the filter state
                writer.WriteLine("  var rows = document.getElementsByTagName('tr');");
                writer.WriteLine("  var totalRows = 0;"); // Variable to track total rows
                writer.WriteLine("  for (var i = 0; i < rows.length; i++) {");
                writer.WriteLine("    var stockCell = rows[i].getElementsByTagName('td')[4];"); // Assuming the stock cell is at index 4
                writer.WriteLine("    if (filterOn) {");
                writer.WriteLine("      if (stockCell && stockCell.style.backgroundColor === 'lightgreen') {");
                writer.WriteLine("        rows[i].style.display = '';");
                writer.WriteLine("        totalRows++;");
                writer.WriteLine("      } else {");
                writer.WriteLine("        rows[i].style.display = 'none';");
                writer.WriteLine("      }");
                writer.WriteLine("    } else {");
                writer.WriteLine("      rows[i].style.display = '';");
                writer.WriteLine("      totalRows++;");
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine("  document.getElementById('rowsCount').innerHTML = 'Total Rows Loaded : ' + totalRows;");
                writer.WriteLine("}");
                writer.WriteLine("function countRows() {");
                writer.WriteLine("  var rows = document.querySelectorAll('table tr:not(:first-child)');"); // Exclude header rows
                writer.WriteLine("  var totalRows = rows.length;");
                writer.WriteLine("  document.getElementById('rowsCount').innerHTML = 'Total Rows Loaded : ' + totalRows;");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                foreach (var group in groupedByIPN)
                {
                    group.OrderBy(item => item.Updated_on);
                    //writer.WriteLine("<h2>IPN: " + group.Key + "</h2>");
                    writer.WriteLine("<table border='1'>");
                    // Table headers
                    writer.WriteLine("<tr>");
                    writer.WriteLine("<th>IPN</th>");
                    writer.WriteLine("<th>Manufacturer</th>");
                    writer.WriteLine("<th>MFPN</th>");
                    writer.WriteLine("<th>Description</th>");
                    writer.WriteLine("<th>Stock</th>");
                    writer.WriteLine("<th>Updated_on</th>");
                    writer.WriteLine("<th>Comments</th>");
                    writer.WriteLine("<th>Source_Requester</th>");
                    writer.WriteLine("</tr>");
                    writer.WriteLine("<h2>" + group.Key + " - Warehouse Balance: " + group.Sum(item => item.Stock) + "</h2>");
                    foreach (var item in group)
                    {
                        writer.WriteLine("<tr>");
                        writer.WriteLine("<td style='text-align: center;'>" + item.IPN + "</td>");
                        writer.WriteLine("<td style='text-align: center;'>" + item.Manufacturer + "</td>");
                        writer.WriteLine("<td style='text-align: center;'>" + item.MFPN + "</td>");
                        writer.WriteLine("<td style='text-align: center;'>" + item.Description + "</td>");
                        if (item.Stock > 0 && !group.Any(otherItem => otherItem.Stock == -item.Stock))
                        {
                            // Green background for positive stocks without a corresponding negative pair
                            writer.WriteLine("<td style='background-color: lightgreen; text-align: center;'>" + item.Stock + "</td>");
                        }
                        else
                        {
                            // No background color for other cells
                            writer.WriteLine("<td style='text-align: center;'>" + item.Stock + "</td>");
                        }
                        writer.WriteLine("<td style='text-align: center;'>" + item.Updated_on + "</td>");
                        writer.WriteLine("<td style='text-align: center;'>" + item.Comments + "</td>");
                        writer.WriteLine("<td style='text-align: center;'>" + item.Source_Requester + "</td>");
                        writer.WriteLine("</tr>");
                    }
                }
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(filename)
            {
                UseShellExecute = true
            };
            p.Start();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int selectionStart = textBox1.SelectionStart;
                int selectionLength = textBox1.SelectionLength;
                ProcessText();
                // Restore cursor position
                textBox1.SelectionStart = selectionStart;
                textBox1.SelectionLength = selectionLength;
            }
            catch (Exception ex)
            {
                HandleError(ex.Message);
            }
        }
        private void ProcessText()
        {
            string[] lines = textBox1.Lines;
            // Remove empty lines
            lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            // Trim leading and trailing spaces and convert to uppercase
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim().ToUpper();
            }
            Array.Sort(lines);
            // Set the trimmed and ordered lines back to the textBox1
            textBox1.Lines = lines;
            int rowCount = lines.Length;
            label1.Text = "Total rows to search for: " + rowCount;
            LoadImageBasedOnPrefix(lines);
        }
        public bool isSql=false;
        private void LoadImageBasedOnPrefix(string[] lines)
        {
            foreach (ClientWarehouse w in warehouses)
            {
                if (lines.Length > 0 && lines[0].StartsWith(w.clPrefix))
                {
                    try
                    {
                        selectedWH = w;
                        if(w.sqlStock!=string.Empty)
                        {
                            isSql=true;
                        }
                        else
                        {
                            isSql = false;
                        }
                        button2.BackgroundImageLayout = ImageLayout.Zoom;
                        button2.BackgroundImage = Image.FromFile(w.clLogo);
                        // Optionally, provide feedback to the user about the loaded image
                    }
                    catch (Exception ex)
                    {
                        HandleError($"Error loading image: {ex.Message}");
                    }
                }
            }
        }
        private void HandleError(string errorMessage)
        {
            // Handle errors, e.g., log them or show a user-friendly message box
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void StockViewDataLoader(string fp, string thesheetName)
        {
            stockItems.Clear();
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("Select * from [" + thesheetName + "$]", conn);
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                int res = 0;
                                int toStk;
                                bool stk = int.TryParse(reader[4].ToString(), out res);
                                if (stk)
                                {
                                    toStk = res;
                                }
                                else
                                {
                                    toStk = 0;
                                }
                                WHitem abc = new WHitem
                                {
                                    IPN = reader[0].ToString(),
                                    Manufacturer = reader[1].ToString(),
                                    MFPN = reader[2].ToString(),
                                    Description = reader[3].ToString(),
                                    Stock = toStk,
                                    Updated_on = reader[5].ToString(),
                                    Comments = reader[6].ToString(),
                                    Source_Requester = reader[7].ToString()
                                };
                                 stockItems.Add(abc);
                            }
                            catch (Exception E)
                            {
                                MessageBox.Show(E.Message);
                                throw;
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error");
            }
        }
    }
}
