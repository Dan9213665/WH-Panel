using System;
using System.Data.Common;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
namespace WH_Panel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DateTime fileModifiedDate = File.GetLastWriteTime(@"ImperiumTabulaPrincipalis.exe");
            this.Text = "Imperium Tabula Principalis UPDATED " + fileModifiedDate.ToString();
        }
        //public List<ClientWarehouse> warehouses {  get; set; }
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
                string clAvlFile = Directory.GetFiles(subDir, "*_AVL.XLSM").FirstOrDefault();
                string clStockFile = Directory.GetFiles(subDir, "*_STOCK.XLSM").FirstOrDefault();
                string clLogoFile = Directory.GetFiles(subDir, "logo.png").FirstOrDefault();
                if (!string.IsNullOrEmpty(clAvlFile) && !string.IsNullOrEmpty(clStockFile))
                {
                    ClientWarehouse warehouse = new ClientWarehouse
                    {
                        clName = clName,
                        clPrefix = clPrefix,
                        clAvlFile = clAvlFile,
                        clStockFile = clStockFile,
                        clLogo = clLogoFile
                    };
                    warehouses.Add(warehouse);
                }
            }
            return warehouses;
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
            FrmClientAgnosticWH cl = new FrmClientAgnosticWH();
            //List<ClientWarehouse> warehouses = InitializeWarehouses();
            List<ClientWarehouse> warehouses = PopulateWarehouses();
            cl.InitializeGlobalWarehouses(warehouses);
            cl.Show();
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
    }
}