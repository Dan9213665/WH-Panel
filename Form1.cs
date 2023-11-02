using System;
using System.Data.Common;
using System.Diagnostics;
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
        public List<ClientWarehouse> InitializeWarehouses()
        {
            List<ClientWarehouse> warehousesInitializedIntheMainForm = new List<ClientWarehouse>
            {
                new ClientWarehouse
                {
                    clName = "NETLINE",
                    clPrefix = "NET",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_AVL.xlsx",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "LEADER-TECH",
                    clPrefix = "C100",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "VAYYAR",
                    clPrefix = "VAY",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_AVL.xlsx",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_stock.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "CIS",
                    clPrefix = "CIS",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\CIS\\CIS_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\CIS\\CIS_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "VALENS",
                    clPrefix = "VAL",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_AVL.xlsx",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "ROBOTRON",
                    clPrefix = "ROB",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "ENERCON",
                    clPrefix = "ENE",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ENERCON\\ENERCON_AVL.xlsx",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ENERCON\\ENERCON_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "DIGITRONIX",
                    clPrefix = "DIG",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\DIGITRONIX\\DIGITRONIX_AVL.xlsx",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\DIGITRONIX\\DIGITRONIX_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "HEPTAGON",
                    clPrefix = "HEP",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\HEPTAGON\\HEPTAGON_AVL.xlsx",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\HEPTAGON\\HEPTAGON_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "EPS",
                    clPrefix = "EPS",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\EPS\\EPS_AVL.xlsx",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\EPS\\EPS_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "SOS",
                    clPrefix = "SOS",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOS\\SOS_AVL.xlsx",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOS\\SOS_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "ARAN",
                    clPrefix = "ARN",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ARAN\\ARAN_AVL.xlsx",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ARAN\\ARAN_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "SOLANIUM",
                    clPrefix = "BAN",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOLANIUM\\SOLANIUM_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SOLANIUM\\SOLANIUM_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "SONOTRON",
                    clPrefix = "SON",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SONOTRON\\SONOTRON_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SONOTRON\\SONOTRON_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "ASIO",
                    clPrefix = "ASO",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ASIO\\ASIO_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ASIO\\ASIO_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "SHILAT",
                    clPrefix = "SHT",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SHILAT\\SHILAT_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\SHILAT\\SHILAT_STOCK.xlsm"
                }
                    ,
                new ClientWarehouse
                {
                    clName = "TRILOGICAL",
                    clPrefix = "UTR",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\TRILOGICAL\\TRILOGICAL_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\TRILOGICAL\\TRILOGICAL_STOCK.xlsm"
                }
                     ,
                new ClientWarehouse
                {
                    clName = "QUANTUM-MACHINES",
                    clPrefix = "QNT",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\QUANTUM-MACHINES\\QUANTUM-MACHINES_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\QUANTUM-MACHINES\\QUANTUM-MACHINES_STOCK.xlsm"
                }
                        ,
                new ClientWarehouse
                {
                    clName = "GASNGO",
                    clPrefix = "GNG",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\GASNGO\\GASNGO_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\GASNGO\\GASNGO_STOCK.xlsm"
                }
                    ,
                new ClientWarehouse
                {
                    clName = "MS-TECH",
                    clPrefix = "MST",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\MS-TECH\\MS-TECH_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\MS-TECH\\MS-TECH_STOCK.xlsm"
                }
                     ,
                new ClientWarehouse
                {
                    clName = "RP-OPTICAL",
                    clPrefix = "RPO",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\RP-OPTICAL\\RP-OPTICAL_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\RP-OPTICAL\\RP-OPTICAL_STOCK.xlsm"
                },
                new ClientWarehouse
                {
                    clName = "ROBOTEAM",
                    clPrefix = "RBM",
                    clAvlFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTEAM\\ROBOTEAM_AVL.xlsm",
                    clStockFile = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTEAM\\ROBOTEAM_STOCK.xlsm"
                }
                // Add more entries for each warehouse as needed
            };
            return warehousesInitializedIntheMainForm;
        }
        private void button14_Click(object sender, EventArgs e)
        {
           
            FrmClientAgnosticWH cl = new FrmClientAgnosticWH();
            List<ClientWarehouse> warehouses = InitializeWarehouses();
            cl.InitializeGlobalWarehouses(warehouses);
            cl.Show();
           
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (frmUberSearch == null || frmUberSearch.IsDisposed)
            {
                frmUberSearch = new FrmUberSearch();
                List<ClientWarehouse> warehouses = InitializeWarehouses();
                frmUberSearch.InitializeGlobalWarehouses(warehouses);
                frmUberSearch.Show();
            }
            else
            {
                frmUberSearch.BringToFront();
            }
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


        private void button11_Click_1(object sender, EventArgs e)
        {
            FrmBOM frmBOM = new FrmBOM();
            frmBOM.Show();
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
            List<ClientWarehouse> warehouses = InitializeWarehouses();
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
        private void button1_Click(object sender, EventArgs e)
        {
            //Process.Start("C:\\1\\source\\repos\\1.Txt");
            var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm";
            AuthorizedExcelFileOpening(fp);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\WareHouse\STOCK_CUSTOMERS\NETLINE\NETLINE_STOCK.xlsm";
            AuthorizedExcelFileOpening(fp);
        }
        private void btnFIELDIN_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\FIELDIN\\FIELDIN_STOCK.xlsm";
            AuthorizedExcelFileOpening(fp);
        }
        private void btnLEADERTECH_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_STOCK.xlsm";
            AuthorizedExcelFileOpening(fp);
        }
        private void btnVAYYAR_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_stock.xlsm";
            AuthorizedExcelFileOpening(fp);
        }
        private void btnSHILAT_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\SHILAT\\SHILAT_STOCK.xlsm";
            AuthorizedExcelFileOpening(fp);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\CIS\\CIS_STOCK.xlsm";
            AuthorizedExcelFileOpening(fp);
        }
        private void button15_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_AVL.xlsm";
            AuthorizedExcelFileOpening(fp);
        }
        private void button16_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_AVL.xlsx";
            AuthorizedExcelFileOpening(fp);
        }
        private void button17_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\WareHouse\STOCK_CUSTOMERS\NETLINE\NETLINE_AVL.xlsx";
            AuthorizedExcelFileOpening(fp);
        }
        private void button18_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_AVL.xlsx";
            AuthorizedExcelFileOpening(fp);
        }
       
        private void button10_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\DIGITRONIX\\DIGITRONIX_STOCK.xlsm";
            AuthorizedExcelFileOpening(fp);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\DIGITRONIX\\DIGITRONIX_AVL.xlsx";
            AuthorizedExcelFileOpening(fp);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ST_MICRO\\ST_MICRO_STOCK.xlsm";
            AuthorizedExcelFileOpening(fp);
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
    }
}