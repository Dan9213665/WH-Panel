using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#pragma warning disable CS0618
namespace WH_Panel
{
    public partial class FrmMFPNsearcher : Form
    {
        public FrmMFPNsearcher()
        {
            InitializeComponent();




        }

        public static string connectionString = "Data Source=DBR3\\SQLEXPRESS;Integrated Security=True;";

        public List<string> databases = GetUserDatabases(connectionString);

        public List<DataTable> results = new List<DataTable>();

        public static List<WHitem> stockItems = new List<WHitem>();


        static DataTable SearchInDatabase(string connectionString, string dbName, string[] filterValues)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = BuildSearchQuery(dbName, filterValues);

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable resultTable = new DataTable();

                    try
                    {
                        adapter.Fill(resultTable);
                        return resultTable;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error searching in database {dbName}: {ex.Message}");
                        return null;
                    }
                }
            }
        }


        static string BuildSearchQuery(string dbName, string[] filterValues)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendLine($"USE [{dbName}];");
            queryBuilder.AppendLine("SELECT * FROM dbo.STOCK WHERE");

            for (int i = 0; i < filterValues.Length; i++)
            {
                if (i > 0)
                {
                    queryBuilder.AppendLine(" OR");
                }
                queryBuilder.Append($"MFPN LIKE '%{filterValues[i]}%'");
            }

            return queryBuilder.ToString();
        }

        static List<string> GetUserDatabases(string connectionString)
        {
            List<string> databases = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT name FROM sys.databases WHERE database_id > 4";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            databases.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return databases;
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
                MessageBox.Show(ex.Message);
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Lines.Length > 0)
            {
                GenerateFilteredReport();
            }
            else
            {
                MessageBox.Show("Paste MFPNs to search for in the textbox !");
                textBox1.Focus();
            }
        }

        

        //string sqlStock = "";
        private void GenerateFilteredReport()
        {

            string[] filterValues = textBox1.Lines
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();


            foreach (string dbName in databases)
            {
                DataTable result = SearchInDatabase(connectionString, dbName, filterValues);
                if (result != null)
                {

                    PopulateStockItems(result);
                }
            }



            var groupedByMFPN = stockItems
                .Where(item => filterValues.Contains(item.MFPN))
                .GroupBy(item => item.MFPN).OrderBy(item => item.Key)
            .ToList();



            //groupedByIPN.OrderBy(item => item.Key);
            // Generate and display the HTML report using the grouped list
            GenerateHTMLReport(groupedByMFPN);
        }

        static void PopulateStockItems(DataTable resultTable)
        {
            foreach (DataRow row in resultTable.Rows)
            {
                WHitem item = new WHitem
                {
                    IPN = row["IPN"].ToString(),
                    Manufacturer = row["Manufacturer"].ToString(),
                    MFPN = row["MFPN"].ToString(),
                    Description = row["Description"].ToString(),
                    Stock = Convert.ToInt32(row["Stock"]),
                    Updated_on = row["Updated_on"].ToString(),
                    Comments = row["Comments"].ToString(),
                    Source_Requester = row["Source_Requester"].ToString()
                };

                stockItems.Add(item);
            }
            stockItems.OrderBy(x => x.Updated_on);
        }

        private void GenerateHTMLReport(List<IGrouping<string?, WHitem>> groupedByIPN)
        {
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string filename = "\\\\dbr1\\Data\\WareHouse\\2025\\WHsearcher\\" + _fileTimeStamp + ".html";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("<html>");
                writer.WriteLine("<head>");
                writer.WriteLine("<title>Listed items to search for</title>");
                writer.WriteLine("</head>");
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
                    //group.OrderBy(item => item.Updated_on);


                    // Order the items by Updated_on
                    var orderedItems = group.OrderBy(item => item.Updated_on);

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
                    //foreach (var item in group)
                    foreach (var item in orderedItems)
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
    }
}
