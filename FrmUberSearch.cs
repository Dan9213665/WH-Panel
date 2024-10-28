using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using FastMember;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using Label = System.Windows.Forms.Label;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
using Button = System.Windows.Forms.Button;
using GroupBox = System.Windows.Forms.GroupBox;
using TextBox = System.Windows.Forms.TextBox;
using ComboBox = System.Windows.Forms.ComboBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using ToolTip = System.Windows.Forms.ToolTip;
using System.Data.SqlClient;
using Action = System.Action;

namespace WH_Panel
{
    public partial class FrmUberSearch : Form
    {
        public FrmUberSearch()
        {
            InitializeComponent();
        }
        private void TextBox_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter((TextBox)sender);
        }
        private void TextBox_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave((TextBox)sender);
        }
        public List<WHitem> wHitems = new List<WHitem>();
        public DataTable UDtable = new DataTable();
        public int countItems = 0;
        int i = 0;
        public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
        {
            public void Add(TKey key, TValue value)
            {
                Add(new KeyValuePair<TKey, TValue>(key, value));
            }
        }
        private async void FrmUberSearch_Load(object sender, EventArgs e)
        {
            //InitializeWarehouses();
            UpdateControlColors(this);
            //startUpLogic();
           await startUpLogicAsync();
        }
        List<ClientWarehouse> warehouses { get; set; }
        public void InitializeGlobalWarehouses(List<ClientWarehouse> warehousesFromTheMain)
        {
            warehouses = warehousesFromTheMain;
            // Ordering the warehouses list by clName
            warehouses = warehouses.OrderBy(warehouse => warehouse.clName).ToList();
        }

        //private async Task startUpLogicAsync()
        //{
        //    List<Label> _seachableFieldsLabels = new List<Label> { label2, label4, label5, label9 };
        //    foreach (Label l in _seachableFieldsLabels)
        //    {
        //        l.BackColor = Color.LightGreen;
        //    }

        //    List<TextBox> _searchableFieldsTextBoxes = new List<TextBox> { textBox2, textBox4, textBox5, textBox9 };
        //    foreach (TextBox textBox in _searchableFieldsTextBoxes)
        //    {
        //        textBox.Enter += TextBox_Enter;
        //        textBox.Leave += TextBox_Leave;
        //    }

        //    label1.BackColor = Color.IndianRed;

        //    // Parallelize data loading for warehouses
        //    List<Task> dataLoadingTasks = new List<Task>();

        //    foreach (ClientWarehouse warehouse in warehouses)
        //    {
        //        if (warehouse.sqlStock != null)
        //        {
        //            dataLoadingTasks.Add(Task.Run(() => DataLoaderSql(warehouse.sqlStock)));
        //        }
        //        else
        //        {
        //            // Handle file loading for non-SQL warehouses
        //            dataLoadingTasks.Add(Task.Run(() => DataLoader(warehouse.clStockFile, "STOCK")));
        //        }
        //    }

        //    await Task.WhenAll(dataLoadingTasks); // Wait for all data loading tasks to complete

        //    PopulateGridView();

        //    // Create and add buttons to the FlowLayoutPanel
        //    List<Button> buttons = new List<Button>();
        //    for (int i = 0; i < warehouses.Count; i++)
        //    {
        //        string warehousePath = warehouses[i].sqlStock;
        //        string warehouseName = warehouses[i].clName;
        //        Button button = new Button { Tag = warehouseName, AutoSize = false, Size = new Size(90, 40) };

        //        ToolTip toolTip = new ToolTip();
        //        toolTip.SetToolTip(button, warehouseName);
        //        button.Click += Button_Click;

        //        if (File.Exists(warehouses[i].clLogo))
        //        {
        //            try
        //            {
        //                string logoFilePath = Path.Combine("dbr1", "WareHouse", "STOCK_CUSTOMERS", warehouseName, warehouses[i].clLogo);
        //                button.BackgroundImage = Image.FromFile(logoFilePath);
        //                button.BackgroundImageLayout = ImageLayout.Stretch;
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Error loading logo: {ex.Message}");
        //            }
        //        }
        //        buttons.Add(button);
        //    }

        //    flowLayoutPanel1.Controls.Clear();
        //    foreach (Button button in buttons)
        //    {
        //        flowLayoutPanel1.Controls.Add(button);
        //    }
        //}

        //private async Task DataLoaderSql(string fp)
        //{
        //    string connectionString = fp;

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            SqlDataAdapter adapterStock = new SqlDataAdapter("SELECT * FROM STOCK", connection);
        //            DataTable stockTable = new DataTable();
        //            await Task.Run(() => adapterStock.Fill(stockTable)); // Load data asynchronously

        //            foreach (DataRow row in stockTable.Rows)
        //            {
        //                WHitem item = new WHitem
        //                {
        //                    IPN = row["IPN"].ToString(),
        //                    Manufacturer = row["Manufacturer"].ToString(),
        //                    MFPN = row["MFPN"].ToString(),
        //                    Description = row["Description"].ToString(),
        //                    Stock = Convert.ToInt32(row["Stock"]),
        //                    Updated_on = row["Updated_on"].ToString(),
        //                    Comments = row["Comments"].ToString(),
        //                    Source_Requester = row["Source_Requester"].ToString()
        //                };

        //                if (i > 0)
        //                {
        //                    countItems = i;
        //                    if (countItems % 5000 == 0)
        //                    {
        //                        label1.Invoke((Action)(() => label1.Text = "Rows:" + countItems.ToString()));
        //                        label1.Invoke((Action)(() => label1.Update()));
        //                    }
        //                    wHitems.Add(item);
        //                }
        //                i++;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle error
        //        Console.WriteLine($"Error loading STOCK table: {ex.Message}");
        //    }
        //}

        //private async Task startUpLogicAsync()
        //{
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();

        //    int initialThreadCount = 0;
        //    int finalThreadCount = 0;

        //    ThreadPool.GetAvailableThreads(out initialThreadCount, out _);

        //    List<Label> _seachableFieldsLabels = new List<Label> { label2, label4, label5, label9 };
        //    foreach (Label l in _seachableFieldsLabels)
        //    {
        //        l.BackColor = Color.LightGreen;
        //    }

        //    List<TextBox> _searchableFieldsTextBoxes = new List<TextBox> { textBox2, textBox4, textBox5, textBox9 };
        //    foreach (TextBox textBox in _searchableFieldsTextBoxes)
        //    {
        //        textBox.Enter += TextBox_Enter;
        //        textBox.Leave += TextBox_Leave;
        //    }

        //    label1.BackColor = Color.IndianRed;

        //    // Parallelize data loading for warehouses
        //    List<Task> dataLoadingTasks = new List<Task>();

        //    foreach (ClientWarehouse warehouse in warehouses)
        //    {
        //        if (warehouse.sqlStock != null)
        //        {
        //            dataLoadingTasks.Add(Task.Run(() => DataLoaderSql(warehouse.sqlStock)));
        //        }
        //        else
        //        {
        //            // Handle file loading for non-SQL warehouses
        //            dataLoadingTasks.Add(Task.Run(() => DataLoader(warehouse.clStockFile, "STOCK")));
        //        }
        //    }

        //    await Task.WhenAll(dataLoadingTasks); // Wait for all data loading tasks to complete

        //    PopulateGridView();

        //    // Create and add buttons to the FlowLayoutPanel
        //    List<Button> buttons = new List<Button>();
        //    for (int i = 0; i < warehouses.Count; i++)
        //    {
        //        string warehousePath = warehouses[i].sqlStock;
        //        string warehouseName = warehouses[i].clName;
        //        Button button = new Button { Tag = warehouseName, AutoSize = false, Size = new Size(90, 40) };

        //        ToolTip toolTip = new ToolTip();
        //        toolTip.SetToolTip(button, warehouseName);
        //        button.Click += Button_Click;

        //        if (File.Exists(warehouses[i].clLogo))
        //        {
        //            try
        //            {
        //                string logoFilePath = Path.Combine("dbr1", "WareHouse", "STOCK_CUSTOMERS", warehouseName, warehouses[i].clLogo);
        //                button.BackgroundImage = Image.FromFile(logoFilePath);
        //                button.BackgroundImageLayout = ImageLayout.Stretch;
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Error loading logo: {ex.Message}");
        //            }
        //        }
        //        buttons.Add(button);
        //    }

        //    flowLayoutPanel1.Controls.Clear();
        //    foreach (Button button in buttons)
        //    {
        //        flowLayoutPanel1.Controls.Add(button);
        //    }

        //    stopwatch.Stop();
        //    ThreadPool.GetAvailableThreads(out finalThreadCount, out _);

        //    // Update the form's title with time taken and threads used
        //    TimeSpan ts = stopwatch.Elapsed;
        //    this.Text = $"App Title (Time: {ts.Seconds}.{ts.Milliseconds:000} s, Threads used: {initialThreadCount - finalThreadCount})";
        //}

        //private async Task DataLoaderSql(string fp)
        //{
        //    string connectionString = fp;

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            SqlDataAdapter adapterStock = new SqlDataAdapter("SELECT * FROM STOCK", connection);
        //            DataTable stockTable = new DataTable();
        //            await Task.Run(() => adapterStock.Fill(stockTable)); // Load data asynchronously

        //            foreach (DataRow row in stockTable.Rows)
        //            {
        //                WHitem item = new WHitem
        //                {
        //                    IPN = row["IPN"].ToString(),
        //                    Manufacturer = row["Manufacturer"].ToString(),
        //                    MFPN = row["MFPN"].ToString(),
        //                    Description = row["Description"].ToString(),
        //                    Stock = Convert.ToInt32(row["Stock"]),
        //                    Updated_on = row["Updated_on"].ToString(),
        //                    Comments = row["Comments"].ToString(),
        //                    Source_Requester = row["Source_Requester"].ToString()
        //                };

        //                if (i > 0)
        //                {
        //                    countItems = i;
        //                    if (countItems % 5000 == 0)
        //                    {
        //                        label1.Invoke((Action)(() => label1.Text = "Rows:" + countItems.ToString()));
        //                        label1.Invoke((Action)(() => label1.Update()));
        //                    }
        //                    wHitems.Add(item);
        //                }
        //                i++;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle error
        //        Console.WriteLine($"Error loading STOCK table: {ex.Message}");
        //    }
        //}


        private async Task startUpLogicAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int initialThreadCount, finalThreadCount;
            ThreadPool.GetAvailableThreads(out initialThreadCount, out _);

            // Track the number of tasks (threads) executed
            int taskCount = 0;

            List<Label> _seachableFieldsLabels = new List<Label> { label2, label4, label5, label9 };
            foreach (Label l in _seachableFieldsLabels)
            {
                l.BackColor = Color.LightGreen;
            }

            List<TextBox> _searchableFieldsTextBoxes = new List<TextBox> { textBox2, textBox4, textBox5, textBox9 };
            foreach (TextBox textBox in _searchableFieldsTextBoxes)
            {
                textBox.Enter += TextBox_Enter;
                textBox.Leave += TextBox_Leave;
            }

            label1.BackColor = Color.IndianRed;

            // Parallelize data loading for warehouses
            List<Task> dataLoadingTasks = new List<Task>();

            foreach (ClientWarehouse warehouse in warehouses)
            {
                if (warehouse.sqlStock != null)
                {
                    dataLoadingTasks.Add(Task.Run(() =>
                    {
                        taskCount++; // Increment task counter
                        DataLoaderSql(warehouse.sqlStock);
                    }));
                }
                else
                {
                    // Handle file loading for non-SQL warehouses
                    dataLoadingTasks.Add(Task.Run(() =>
                    {
                        taskCount++; // Increment task counter
                        DataLoader(warehouse.clStockFile, "STOCK");
                    }));
                }
            }

            await Task.WhenAll(dataLoadingTasks); // Wait for all data loading tasks to complete

            PopulateGridView();

            // Create and add buttons to the FlowLayoutPanel
            List<Button> buttons = new List<Button>();
            for (int i = 0; i < warehouses.Count; i++)
            {
                string warehousePath = warehouses[i].sqlStock;
                string warehouseName = warehouses[i].clName;
                Button button = new Button { Tag = warehouseName, AutoSize = false, Size = new Size(90, 40) };

                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(button, warehouseName);
                button.Click += Button_Click;

                if (File.Exists(warehouses[i].clLogo))
                {
                    try
                    {
                        string logoFilePath = Path.Combine("dbr1", "WareHouse", "STOCK_CUSTOMERS", warehouseName, warehouses[i].clLogo);
                        button.BackgroundImage = Image.FromFile(logoFilePath);
                        button.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading logo: {ex.Message}");
                    }
                }
                buttons.Add(button);
            }

            flowLayoutPanel1.Controls.Clear();
            foreach (Button button in buttons)
            {
                flowLayoutPanel1.Controls.Add(button);
            }

            stopwatch.Stop();
            ThreadPool.GetAvailableThreads(out finalThreadCount, out _);

            // Update the form's title with time taken and task count
            TimeSpan ts = stopwatch.Elapsed;
            this.Text = $"App Title (Time: {ts.Seconds}.{ts.Milliseconds:000} s, Tasks used: {taskCount})";
        }

        private async Task DataLoaderSql(string fp)
        {
            string connectionString = fp;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapterStock = new SqlDataAdapter("SELECT * FROM STOCK", connection);
                    DataTable stockTable = new DataTable();
                    await Task.Run(() => adapterStock.Fill(stockTable)); // Load data asynchronously

                    foreach (DataRow row in stockTable.Rows)
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

                        if (i > 0)
                        {
                            countItems = i;
                            if (countItems % 5000 == 0)
                            {
                                label1.Invoke((Action)(() => label1.Text = "Rows:" + countItems.ToString()));
                                label1.Invoke((Action)(() => label1.Update()));
                            }
                            wHitems.Add(item);
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading STOCK table: {ex.Message}");
            }
        }

        private void startUpLogic()
        {
            List<Label> _seachableFieldsLabels = new List<Label>();
            _seachableFieldsLabels.Add(label2);
            _seachableFieldsLabels.Add(label4);
            _seachableFieldsLabels.Add(label5);
            _seachableFieldsLabels.Add(label9);
            foreach (Label l in _seachableFieldsLabels)
            {
                l.BackColor = Color.LightGreen;
            }
            List<TextBox> _searchableFieldsTextBoxes = new List<TextBox>();
            _searchableFieldsTextBoxes.Add(textBox2);
            _searchableFieldsTextBoxes.Add(textBox4);
            _searchableFieldsTextBoxes.Add(textBox5);
            _searchableFieldsTextBoxes.Add(textBox9);
            foreach (TextBox textBox in _searchableFieldsTextBoxes)
            {
                textBox.Enter += TextBox_Enter;
                textBox.Leave += TextBox_Leave;
            }
            label1.BackColor = Color.IndianRed;
            foreach (ClientWarehouse warehouse in warehouses)
            {
                if(warehouse.sqlStock!=null)
                {
                    DataLoaderSql(warehouse.sqlStock);
                }
                else
                {
                    DataLoader(warehouse.clStockFile, "STOCK");
                }
                
            }
            PopulateGridView();
          
            // Create a list to hold the buttons
            List<Button> buttons = new List<Button>();
            for (int i = 0; i < warehouses.Count; i++)
            {
                string warehousePath = warehouses[i].sqlStock; // Get the warehouse path from the KeyValuePair
                // Extract the warehouse name from the warehouse path
                string[] pathParts = warehousePath.Split('\\');
                string warehouseName = warehouses[i].clName;
                Button button = new Button();
                button.Tag = warehouseName;
                //button.AutoSize = true; // Adjust the button size based on the text length
                button.AutoSize = false; // Disable auto-sizing
                button.Size = new Size(90, 40); // Set the button size
                                                 // Add a tooltip to display warehouseName when hovering over the button
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(button, warehouseName);
                button.Click += Button_Click; // Assign a common event handler for button click event
                // Load the image from the first function and set it as the button's background
                if (File.Exists(warehouses[i].clLogo))
                {
                    try
                    {
                        string logoFilePath = Path.Combine("dbr1", "WareHouse", "STOCK_CUSTOMERS", warehouseName, warehouses[i].clLogo);
                        button.BackgroundImage = Image.FromFile(logoFilePath);
                        button.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (e.g., log it, display a message to the user)
                        Console.WriteLine($"Error loading logo: {ex.Message}");
                    }
                }
                buttons.Add(button); // Add the button to the list
            }
         
            flowLayoutPanel1.Controls.Clear();
            // Add the sorted buttons to the flowLayoutPanel1 control
            foreach (Button button in buttons)
            {
                flowLayoutPanel1.Controls.Add(button); // Add the button to the FlowLayoutPanel
            }
       
        }
        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string warehousePath = (string)clickedButton.Tag;
            string[] pathParts = warehousePath.Split('\\');
            //string warehouseName = pathParts[pathParts.Length - 2];
            FrmClientAgnosticWH w = new FrmClientAgnosticWH();
            w.InitializeGlobalWarehouses(warehouses);
            w.Show();
            w.Focus();
            // Call the public method to set the ComboBox text
            w.SetComboBoxText(warehousePath);
        }
        //private void DataLoaderSql (string fp)
        //{
        //    string connectionString = fp;

        //    try
        //    {
        //        // Load STOCK table into dataGridView1
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {


        //            SqlDataAdapter adapterStock = new SqlDataAdapter("SELECT * FROM STOCK", connection);

        //            DataTable stockTable = new DataTable();
        //            adapterStock.Fill(stockTable);

        //            foreach (DataRow row in stockTable.Rows)
        //            {
        //                WHitem item = new WHitem
        //                {
        //                    IPN = row["IPN"].ToString(),
        //                    Manufacturer = row["Manufacturer"].ToString(),
        //                    MFPN = row["MFPN"].ToString(),
        //                    Description = row["Description"].ToString(),
        //                    Stock = Convert.ToInt32(row["Stock"]), // Assuming Stock is an integer field
        //                    Updated_on = row["Updated_on"].ToString(),
        //                    Comments = row["Comments"].ToString(),
        //                    Source_Requester = row["Source_Requester"].ToString()
        //                };
        //                if (i > 0)
        //                {
        //                    countItems = i;
        //                    label1.Text = "Rows:" + (countItems).ToString();
        //                    if (countItems % 5000 == 0)
        //                    { label1.Update(); }
        //                    wHitems.Add(item);
        //                }
        //                i++;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // MessageBox.Show($"Error loading STOCK table: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void DataLoader(string fp, string thesheetName)
        {
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=1\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("Select * from [" + thesheetName + "$]", conn);
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int number;
                            bool success = int.TryParse(reader[4].ToString(), out number);
                            if (success)
                            {
                                WHitem abc = new WHitem
                                {
                                    IPN = reader[0].ToString(),
                                    Manufacturer = reader[1].ToString(),
                                    MFPN = reader[2].ToString(),
                                    Description = reader[3].ToString(),
                                    Stock = number,
                                    Updated_on = reader[5].ToString(),
                                    Comments = reader[6].ToString(),
                                    Source_Requester = reader[7].ToString()
                                };
                                if (i > 0)
                                {
                                    countItems = i;
                                    label1.Text = "Rows:" + (countItems).ToString();
                                    if (countItems % 5000 == 0)
                                    { label1.Update(); }
                                    wHitems.Add(abc);
                                }
                                i++;
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
        private void PopulateGridView()
        {
            UDtable.Clear();
            IEnumerable<WHitem> data = wHitems;
            using (var reader = ObjectReader.Create(data))
            {
                UDtable.Load(reader);
            }
            dataGridView1.DataSource = UDtable;
            SetColumsOrder();
            label1.BackColor = Color.LightGreen;
        }
        private void SetColumsOrder()
        {
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["IPN"].DisplayIndex = 0;
            dataGridView1.Columns["Manufacturer"].DisplayIndex = 1;
            dataGridView1.Columns["MFPN"].DisplayIndex = 2;
            dataGridView1.Columns["Description"].DisplayIndex = 3;
            dataGridView1.Columns["Stock"].DisplayIndex = 4;
            dataGridView1.Columns["Updated_on"].DisplayIndex = 5;
            dataGridView1.Columns["Comments"].DisplayIndex = 6;
            dataGridView1.Columns["Source_Requester"].DisplayIndex = 7;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ResetViews();
            startUpLogic();
            //SetColumsOrder();
            //ResetViews();
            //LoadDataFromFile();
            //PopulateGW();
        }
        private void ResetViews()
        {
            wHitems.Clear();
            countItems = 0;
            i = 0;
            label1.Text = "";
            UDtable.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label2.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void FilterTheDataGridView()
        {
            try
            {
                DataView dv = UDtable.DefaultView;
                StringBuilder filterQuery = new StringBuilder();
                if (!string.IsNullOrEmpty(textBox2.Text))
                    filterQuery.Append("[IPN] LIKE '%" + textBox2.Text + "%' AND ");
                if (!string.IsNullOrEmpty(textBox4.Text))
                    filterQuery.Append("[MFPN] LIKE '%" + textBox4.Text + "%' AND ");
                if (!string.IsNullOrEmpty(textBox5.Text))
                {
                    string[] searchTerms = textBox5.Text.Split(new char[] { '+', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string term in searchTerms)
                    {
                        filterQuery.Append("[Description] LIKE '%" + term + "%' AND ");
                    }
                }
                if (!string.IsNullOrEmpty(textBox9.Text))
                    filterQuery.Append("[Source_Requester] LIKE '%" + textBox9.Text + "%' AND ");
                if (filterQuery.Length > 0)
                {
                    filterQuery.Remove(filterQuery.Length - 5, 5); // Remove the extra 'AND' at the end
                    dv.RowFilter = filterQuery.ToString();
                }
                else
                {
                    dv.RowFilter = string.Empty; // No filter applied
                }
                dataGridView1.DataSource = dv;
                SetColumsOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Incorrect search pattern, remove invalid character and try again !\nError: " + ex.Message, "Search pattern error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label4.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            label5.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void openWHexcelDB(string thePathToFile)
        {
            Process excel = new Process();
            excel.StartInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.exe";
            excel.StartInfo.Arguments = thePathToFile;
            excel.Start();
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            label9.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            label2.BackColor = Color.LightGreen;
        }
        private void label4_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            label4.BackColor = Color.LightGreen;
        }
        private void label5_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            label5.BackColor = Color.LightGreen;
        }
        private void label9_Click(object sender, EventArgs e)
        {
            textBox9.Text = "";
            label9.BackColor = Color.LightGreen;
        }
        private void label2_DoubleClick(object sender, EventArgs e)
        {
            clearAllsearchTextboxes();
        }
        private void clearAllsearchTextboxes()
        {
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox9.Text = "";
            label2.BackColor = Color.LightGreen;
            label4.BackColor = Color.LightGreen;
            label5.BackColor = Color.LightGreen;
            label9.BackColor = Color.LightGreen;
        }
        private void label4_DoubleClick(object sender, EventArgs e)
        {
            clearAllsearchTextboxes();
        }
        private void label5_DoubleClick(object sender, EventArgs e)
        {
            clearAllsearchTextboxes();
        }
        private void label9_DoubleClick(object sender, EventArgs e)
        {
            clearAllsearchTextboxes();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (Environment.MachineName.ToString() == "RT12")
            {
                var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_STOCK.xlsm";
                openWHexcelDB(fp);
            }
            else
            {
                MessageBox.Show("ACCESS DENIED");
            }
        }
        private void UpdateControlColors(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                // Update control colors based on your criteria
                control.BackColor = Color.LightGray;
                control.ForeColor = Color.White;
                // Handle Button controls separately
                if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    button.FlatAppearance.BorderColor = Color.DarkGray; // Change border color
                    button.ForeColor = Color.Black;
                }
                // Handle Button controls separately
                if (control is GroupBox groupbox)
                {
                    groupbox.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    groupbox.ForeColor = Color.Black;
                }
                // Handle TextBox controls separately
                if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle; // Set border style to FixedSingle
                    textBox.BackColor = Color.LightGray; // Change background color
                    textBox.ForeColor = Color.Black; // Change text color
                }
                // Handle Label controls separately
                if (control is Label label)
                {
                    label.BorderStyle = BorderStyle.FixedSingle; // Set border style to FixedSingle
                    label.BackColor = Color.Gray; // Change background color
                    label.ForeColor = Color.Black; // Change text color
                }
                // Handle TabControl controls separately
                if (control is TabControl tabControl)
                {
                    //tabControl.BackColor = Color.Black; // Change TabControl background color
                    tabControl.ForeColor = Color.Black;
                    // Handle each TabPage within the TabControl
                    foreach (TabPage tabPage in tabControl.TabPages)
                    {
                        tabPage.BackColor = Color.Gray; // Change TabPage background color
                        tabPage.ForeColor = Color.Black; // Change TabPage text color
                    }
                }
                // Handle DataGridView controls separately
                if (control is DataGridView dataGridView)
                {
                    // Update DataGridView styles
                    dataGridView.EnableHeadersVisualStyles = false;
                    dataGridView.BackgroundColor = Color.DarkGray;
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView.RowHeadersDefaultCellStyle.BackColor = Color.Gray;
                    dataGridView.DefaultCellStyle.BackColor = Color.Gray;
                    dataGridView.DefaultCellStyle.ForeColor = Color.White;
                    dataGridView.DefaultCellStyle.SelectionBackColor = Color.Green;
                    dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;
                    // Change the header cell styles for each column
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.HeaderCell.Style.BackColor = Color.DarkGray;
                        column.HeaderCell.Style.ForeColor = Color.Black;
                    }
                }
                // Handle ComboBox controls separately
                if (control is ComboBox comboBox)
                {
                    comboBox.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    comboBox.BackColor = Color.DarkGray; // Change ComboBox background color
                    comboBox.ForeColor = Color.Black; // Change ComboBox text color
                }
                // Handle DateTimePicker controls separately
                if (control is DateTimePicker dateTimePicker)
                {
                    // Change DateTimePicker's custom properties here
                    dateTimePicker.BackColor = Color.DarkGray; // Change DateTimePicker background color
                    dateTimePicker.ForeColor = Color.White; // Change DateTimePicker text color
                                                            // Customize other DateTimePicker properties as needed
                }
                // Recursively update controls within containers
                if (control.Controls.Count > 0)
                {
                    UpdateControlColors(control);
                }
            }
        }
        private void textBox3_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private static void txtbColorGreenOnEnter(object sender)
        {
            TextBox? tb = (TextBox)sender;
            tb.BackColor = Color.LightGreen;
        }
        private static void txtbColorWhiteOnLeave(object sender)
        {
            TextBox? tb = sender as TextBox;
            tb.BackColor = Color.LightGray;
        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        private void textBox4_Enter(object sender, EventArgs e)
        {
            txtbColorGreenOnEnter(sender);
        }
        private void textBox4_Leave(object sender, EventArgs e)
        {
            txtbColorWhiteOnLeave(sender);
        }
        //private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    MessageBox.Show(e.ColumnIndex.ToString());
        //}
    }
}
