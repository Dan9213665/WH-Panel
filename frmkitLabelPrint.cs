using FastMember;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using TextBox = System.Windows.Forms.TextBox;
using Seagull.Framework.OS;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using DataTable = System.Data.DataTable;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
namespace WH_Panel
{
    public partial class frmkitLabelPrint : Form
    {
        public frmkitLabelPrint()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='\\\\dbr1\\Data\\WareHouse\\KitLabel.xlsm'; Extended Properties=\"Excel 12.0 Xml;HDR=NO;\"";
        }
        private void btnBrowseToFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\"+ DateTime.Now.ToString("yyyy") + "\\"+ DateTime.Now.ToString("MM") + "."+ DateTime.Now.ToString("yyyy");
            openFileDialog1.Filter = "xlsm files (*.xlsm)|*.xlsm";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string fileN = openFileDialog1.SafeFileName;
                string r = fileN.Substring(0, fileN.Length - 5);
                txtbPasteCPQ.Text =r;
                try
                {
                    EXCELinserter(r);
                }
                catch (IOException)
                {
                }
            }
        }
        private void btnPrintKitLabel_Click(object sender, EventArgs e)
        {
            if(txtbPasteCPQ.Text!= string.Empty)
            {
                EXCELinserter(txtbPasteCPQ.Text);
            }
            else
            {
                MessageBox.Show("Input file name or browse to file to print the KIT label !","",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }
        private void EXCELinserter(string kitName)
        {
            try
            {
                string fp = "\\\\dbr1\\Data\\WareHouse\\KitLabel.xlsm";
                _Application docExcel = new Microsoft.Office.Interop.Excel.Application();
                docExcel.Visible = false;
                docExcel.DisplayAlerts = false;
                _Workbook workbooksExcel = docExcel.Workbooks.Open(@fp, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                _Worksheet worksheetExcel = (_Worksheet)workbooksExcel.Worksheets[1];
                ((Range)worksheetExcel.Cells[1, "B"]).Value2 = kitName;
                ((Range)worksheetExcel.Columns["B"]).ColumnWidth= 51;
                ((Range)worksheetExcel.Cells[3, "B"]).WrapText= true;
                ((Range)worksheetExcel.Cells[6, "B"]).WrapText = true;
                ((Range)worksheetExcel.Cells[9, "B"]).WrapText = true;
                workbooksExcel.PrintOutEx(1,1,1);
                workbooksExcel.Close(false, Type.Missing, Type.Missing);
                docExcel.Application.DisplayAlerts = false;
                docExcel.Application.Quit();
                MessageBox.Show("Label Sent to printer");
                this.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
