using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace WH_Panel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Rectangle workingArea = Screen.GetWorkingArea(this);
            //this.Location = new Point(-1040,1300);
            //DateTime fileCreatedDate = File.GetCreationTime(@"ImperiumTabulaPrincipalis.exe");
            DateTime fileModifiedDate = File.GetLastWriteTime(@"ImperiumTabulaPrincipalis.exe");
            this.Text = "Imperium Tabula Principalis UPDATED "+ fileModifiedDate.ToString();
        }
        private void openWHexcelDB(string thePathToFile)
        {
            Process excel = new Process();
            excel.StartInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.exe";
            excel.StartInfo.Arguments = thePathToFile;
            excel.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Process.Start("C:\\1\\source\\repos\\1.Txt");
            var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm";
            openWHexcelDB(fp);
            //Process excel = new Process();
            //excel.StartInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.exe";
            //excel.StartInfo.Arguments = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm"; 
            //excel.Start();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\WareHouse\STOCK_CUSTOMERS\NETLINE\NETLINE_STOCK.xlsm";
            openWHexcelDB(fp);
        }
        private void btnFIELDIN_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\FIELDIN\\FIELDIN_STOCK.xlsm";
            openWHexcelDB(fp);
        }
        private void btnWorkProgramm_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\DocumentsForProduction\\WORK_PROGRAM.xlsm";
            openWHexcelDB(fp);
        }
        private void btnLEADERTECH_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_STOCK.xlsm";
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
        private void btnVAYYAR_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_stock.xlsm";
            openWHexcelDB(fp);
        }
        private void btnSHILAT_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\SHILAT\\SHILAT_STOCK.xlsm";
            openWHexcelDB(fp);
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            //Form f = new frmkitLabelPrint();
            //f.Show();
            //f.Focus();
            var fp = @"\\\\dbr1\\Data\\WareHouse\\KitLabel.xlsm";
            openWHexcelDB(fp);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\WareHouse\\PACKING_SLIPS\\_template.xlsm";
            openWHexcelDB(fp);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //this.TopMost = false;
            FrmUberSearch frmUber = new FrmUberSearch();
            frmUber.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\CIS\\CIS_STOCK.xlsm";
            openWHexcelDB(fp);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            FrmKITShistory frmkit= new FrmKITShistory();
            frmkit.Show();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ST_MICRO\\ST_MICRO_STOCK.xlsm";
            openWHexcelDB(fp);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            FrmAddItemsToDB addItemsToDB = new FrmAddItemsToDB();
            addItemsToDB.Show();
        }
    }
}