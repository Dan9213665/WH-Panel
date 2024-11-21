using System;
using System.Data.Common;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;

namespace WH_Panel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DateTime fileModifiedDate = File.GetLastWriteTime(@"ImperiumTabulaPrincipalis.exe");
            this.Text = "Imperium Tabula Principalis UPDATED " + fileModifiedDate.ToString("yyyyMMddHHmm");
            // Check if the machine name is "lgt"
            if (Environment.MachineName == "RT12"|| Environment.MachineName == "RT19")
            {
                // Set the starting position for the form
                SetFormStartPosition();
            }
        }
        private void SetFormStartPosition()
        {
            // Get the virtual screen bounds spanning multiple monitors
            Rectangle virtualScreenBounds = SystemInformation.VirtualScreen;
            // Set the starting position to the lower left corner
            int x = virtualScreenBounds.Left + this.Width - 100;
            int y = virtualScreenBounds.Bottom - this.Height - 50;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(x, y);
        }
        public List<ClientWarehouse> warehouses { get; set; }



        public List<ClientWarehouse> PopulateWarehouses()
        {
            string directoryPath = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS";
            List<ClientWarehouse> warehouses = new List<ClientWarehouse>();
            // Get all subdirectories under the specified directory
            string[] subDirectories = Directory.GetDirectories(directoryPath);
            foreach (string subDir in subDirectories)
            {
                string clName = new DirectoryInfo(subDir).Name;
                string clPrefix = GetPrefixFromFile(Path.Combine(subDir, "prefix.txt"));
                string clLogoFile = Directory.GetFiles(subDir, "logo.png").FirstOrDefault();
                string accDBfile = Directory.GetFiles(subDir, ".accdb").FirstOrDefault();
                string clAvlFile = Directory.GetFiles(subDir, "*_AVL.XLSM").FirstOrDefault();
                string clStockFile = Directory.GetFiles(subDir, "*_STOCK.XLSM").FirstOrDefault();

                // Determine migration status based on database existence in SQL Server
                bool isSqlMigrated = IsDatabaseInSQLServer(clName);

                // Use SQL columns if migrated
                string sqlAvl = isSqlMigrated ? GetSqlAvl(clName) : string.Empty;
                string sqlStock = isSqlMigrated ? GetSqlStock(clName) : string.Empty;

                if ((!isSqlMigrated && !string.IsNullOrEmpty(clAvlFile) && !string.IsNullOrEmpty(clStockFile)) || (isSqlMigrated && !string.IsNullOrEmpty(sqlAvl) && !string.IsNullOrEmpty(sqlStock)))
                {
                    ClientWarehouse warehouse = new ClientWarehouse
                    {
                        clName = clName,
                        clPrefix = clPrefix,
                        clAvlFile = isSqlMigrated ? string.Empty : Directory.GetFiles(subDir, "*_AVL.XLSM").FirstOrDefault(),
                        clStockFile = isSqlMigrated ? string.Empty : Directory.GetFiles(subDir, "*_STOCK.XLSM").FirstOrDefault(),
                        clLogo = clLogoFile,
                        claccDBfile = accDBfile,
                        sqlAvl = sqlAvl,
                        sqlStock = sqlStock
                    };
                    warehouses.Add(warehouse);
                }
            }
            return warehouses;
        }
        public bool IsDatabaseInSQLServer(string clName)
        {
            //string connectionString = "Data Source=RT12\\SQLEXPRESS;Integrated Security=True;";
            string connectionString = "Data Source=DBR3\\SQLEXPRESS;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM sys.databases WHERE name = @DatabaseName";
                command.Parameters.AddWithValue("@DatabaseName", clName);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        // Function to retrieve SQL Avl file
        private string GetSqlAvl(string clName)
        {
            // Connection string for SQL Server Express
            //return $"Data Source=RT12\\SQLEXPRESS;Initial Catalog={clName};Integrated Security=True;";
            return $"Data Source=DBR3\\SQLEXPRESS;Initial Catalog={clName};Integrated Security=True;";

        }

        // Function to retrieve SQL Stock file
        private string GetSqlStock(string clName)
        {
            // Connection string for SQL Server Express
            return $"Data Source=DBR3\\SQLEXPRESS;Initial Catalog={clName};Integrated Security=True;";
            //return $"Data Source=RT12\\SQLEXPRESS;Initial Catalog={clName};Integrated Security=True;";
        }






        private string GetPrefixFromFile(string prefixFilePath)
        {
            if (File.Exists(prefixFilePath))
            {
                try
                {
                    return File.ReadAllText(prefixFilePath).Trim();
                }
                catch (Exception)
                {
                    // Handle any exceptions that may occur while reading the prefix file
                }
            }
            // Return a default value if the prefix file is missing or invalid
            return string.Empty;
        }
        private void button14_Click(object sender, EventArgs e)
        {
            //FrmClientAgnosticWH cl = new FrmClientAgnosticWH();
            ////List<ClientWarehouse> warehouses = InitializeWarehouses();
            //List<ClientWarehouse> warehouses = PopulateWarehouses();
            //cl.InitializeGlobalWarehouses(warehouses);
            //cl.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (frmUberSearch == null || frmUberSearch.IsDisposed)
            {
                frmUberSearch = new FrmUberSearch();
                //List<ClientWarehouse> warehouses = InitializeWarehouses();
                List<ClientWarehouse> warehouses = PopulateWarehouses();
                frmUberSearch.InitializeGlobalWarehouses(warehouses);
                frmUberSearch.Show();
            }
            else
            {
                frmUberSearch.BringToFront();
            }
        }
        private void button11_Click_1(object sender, EventArgs e)
        {
            FrmBOM frmBOM = new FrmBOM();
            List<ClientWarehouse> warehouses = PopulateWarehouses();
            frmBOM.InitializeGlobalWarehouses(warehouses);
            frmBOM.Show();
        }
        private void openWHexcelDB(string thePathToFile)
        {
            Process excel = new Process();
            excel.StartInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.exe";
            excel.StartInfo.Arguments = thePathToFile;
            excel.Start();
        }
        private void AuthorizedExcelFileOpening(string fp)
        {
            if (Environment.UserName == "lgt")
            {
                openWHexcelDB(fp);
            }
            else
            {
                MessageBox.Show("Unauthorized ! Access denied !", "Unauthorized ! Access denied !", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void btnWorkProgramm_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\DocumentsForProduction\\WORK_PROGRAM.xlsm";
            openWHexcelDB(fp);
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            frmkitLabelPrint frmkit = new frmkitLabelPrint();
            frmkit.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\WareHouse\\PACKING_SLIPS\\_template.xlsm";
            openWHexcelDB(fp);
        }
        private FrmUberSearch openUberSearchForm = null;
        private FrmUberSearch frmUberSearch = null;
        private FrmKITShistory openKITShistoryForm = null;
        private void button5_Click(object sender, EventArgs e)
        {
            if (openKITShistoryForm == null || openKITShistoryForm.IsDisposed)
            {
                openKITShistoryForm = new FrmKITShistory();
                openKITShistoryForm.Show();
            }
            else
            {
                openKITShistoryForm.BringToFront();
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            FrmPackingSlips fps = new FrmPackingSlips();
            fps.Show();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            frmkitLabelPrint frmkit = new frmkitLabelPrint();
            frmkit.Show();
        }
        private void button13_Click(object sender, EventArgs e)
        {
            FrmPackingSlipShip ps = new FrmPackingSlipShip();
            ps.Show();
        }
        private void button7_Click_1(object sender, EventArgs e)
        {
            FrmFinishedGoodsLog ff = new FrmFinishedGoodsLog();
            ff.Show();
        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            FrmExcelFormatter fr = new FrmExcelFormatter();
            fr.Show();
            fr.Focus();
        }
        private void button9_Click_1(object sender, EventArgs e)
        {
            FrmQRPrint fq = new FrmQRPrint();
            fq.Show();
        }
        private void button10_Click_1(object sender, EventArgs e)
        {
            FrmLinkSimulator frm = new FrmLinkSimulator();
            List<ClientWarehouse> warehouses = PopulateWarehouses();
            frm.InitializeGlobalWarehouses(warehouses);
            frm.Show();
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            OpenWebAppInBroser();
        }
        static void OpenWebAppInBroser()
        {
            string url = "http://192.168.69.37:81/"; // Change this to the desired web address
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //// Close any open file streams
            //if (fileStream != null)
            //{
            //    fileStream.Close();
            //    fileStream.Dispose();
            //}
            //// Close any open database connections
            //if (dbConnection != null)
            //{
            //    dbConnection.Close();
            //    dbConnection.Dispose();
            //}
        }
        private void button12_Click(object sender, EventArgs e)
        {
            FrmWHStockStatusList w = new FrmWHStockStatusList();
            List<ClientWarehouse> warehouses = PopulateWarehouses();
            w.InitializeGlobalWarehouses(warehouses);
            w.Show();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            FrmSQLWHDB frm = new FrmSQLWHDB();
            frm.Show();
        }

        private void btnMFPN_Click(object sender, EventArgs e)
        {
            FrmMFPNsearcher frm = new FrmMFPNsearcher();
            frm.Show();
        }

        private void button14_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FrmClientAgnosticWH cl = new FrmClientAgnosticWH();
                List<ClientWarehouse> warehouses = PopulateWarehouses();
                cl.InitializeGlobalWarehouses(warehouses);
                cl.Show();
            }
            else if (e.Button == MouseButtons.Right)
            {
                try
                {
                    FrmStockCounter FrmStockCounter = new FrmStockCounter();
                    List<ClientWarehouse> warehouses = PopulateWarehouses();
                    FrmStockCounter.InitializeGlobalWarehouses(warehouses);
                    FrmStockCounter.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

    }
}